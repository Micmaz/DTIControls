Imports BaseClasses
Imports System.Web.UI
Imports DTIServerControls
Imports DTITagManager.dsTagger

''' <summary>
''' Page object with DTI-managed media meta data
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIBasePage
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIBasePage
        Inherits BaseClasses.BaseSecurityPage
#End If

        Public Enum modes
            Read = 1
            Write = 2
        End Enum

        Public MyMediaRow As dsMedia.DTIMediaManagerRow
        Protected tagTable As New DTITagManager.dsTagger.DTI_Content_TagsDataTable
        Public Event ModeChanged()
        Public Event LoadControls(ByVal modeChanged As Boolean)
        Protected Event DataChanged()


#Region "Properties"

        Public Shared ReadOnly Property siteEditOnDefaultKey() As String
            Get
                Return DTISharedVariables.siteEditOnDefaultKey
            End Get
        End Property

        Public Shared ReadOnly Property siteEditMainID() As String
            Get
                Return DTISharedVariables.siteEditMainID
            End Get
        End Property

        Public Shared ReadOnly Property siteLanguageID() As String
            Get
                Return DTISharedVariables.siteLanguageID
            End Get
        End Property

        Private _MainID As Long = 0
        Public Property MainID() As Long
            Get
                If MasterMainId >= 0 Then
                    Return MasterMainId
                Else
                    'language id is bit shifted to the top 10 bits and then ORed into the mainid
                    Dim shifted_language_id As Long = LanguageID * Math.Pow(2, 55)
                    Return shifted_language_id Or _MainID
                End If
            End Get
            Set(ByVal value As Long)
                Dim oldMain = _MainID
                _MainID = value
                If oldMain <> _MainID Then
                    RaiseEvent DataChanged()
                End If
            End Set
        End Property

        Public Overridable Property MasterMainId() As Long
            Get
                Return DTISharedVariables.MasterMainId
            End Get
            Set(ByVal value As Long)
                Dim oldMainId = DTISharedVariables.MasterMainId
                DTISharedVariables.MasterMainId = value
                If oldMainId <> value Then
                    RaiseEvent DataChanged()
                End If
            End Set
        End Property

        Private _LanguageID As Integer = 0
        Public Property LanguageID() As Integer
            Get
                If MasterLanguageId >= 0 Then
                    Return MasterLanguageId
                Else
                    Return _LanguageID
                End If
            End Get
            Set(ByVal value As Integer)
                Dim oldLanguage = _LanguageID
                _LanguageID = value
                If oldLanguage <> _LanguageID Then
                    RaiseEvent DataChanged()
                End If
            End Set
        End Property

        Public Overridable Property MasterLanguageId() As Integer
            Get
                Return DTISharedVariables.MasterLanguageId
            End Get
            Set(ByVal value As Integer)
                Dim oldMasterLangId = DTISharedVariables.MasterLanguageId
                DTISharedVariables.MasterLanguageId = value
                If oldMasterLangId <> value Then
                    RaiseEvent DataChanged()
                End If
            End Set
        End Property

        Private _staticMode As Boolean = False
        Public Property StaticMode() As Boolean
            Get
                Return _staticMode
            End Get
            Set(ByVal value As Boolean)
                _staticMode = value
            End Set
        End Property

        Public Overridable Property AdminOn() As Boolean
            Get
                Return DTISharedVariables.AdminOn
            End Get
            Set(ByVal value As Boolean)
                DTISharedVariables.AdminOn = value
            End Set
        End Property

        Public Property Mode() As modes
            Get
                If Session("ContentDisplayMode." & Me.ClientID) Is Nothing Then
                    Session("ContentDisplayMode." & Me.ClientID) = modes.Read
                End If

                If AdminOn AndAlso Session("ContentDisplayMode." & Me.ClientID) = modes.Read Then
                    Session("ContentDisplayMode." & Me.ClientID) = modes.Write
                End If

                If Not AdminOn AndAlso Not StaticMode AndAlso Session("ContentDisplayMode." & Me.ClientID) <> modes.Read Then
                    Session("ContentDisplayMode." & Me.ClientID) = modes.Read
                End If

                Return Session("ContentDisplayMode." & Me.ClientID)
            End Get
            Set(ByVal value As modes)
                Session("ContentDisplayMode." & Me.ClientID) = value
                RaiseEvent ModeChanged()
            End Set
        End Property

        Public Overrides Property Visible() As Boolean
            Get
                If Mode = modes.Read Then
                    Return MyBase.Visible
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                MyBase.Visible = value
            End Set
        End Property

        Private _ajaxEnabled As Boolean = False
        Public Property AJAXEnabled() As Boolean
            Get
                Return _ajaxEnabled
            End Get
            Set(ByVal value As Boolean)
                _ajaxEnabled = value
            End Set
        End Property

#End Region

#Region "Events"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        End Sub

        'Private Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        '    addscriptmanagerToPage()
        'End Sub

        Private Sub DTIServerControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RaiseEvent LoadControls(False)
        End Sub

        Private Sub DTIServerControl_ModeChanged() Handles Me.ModeChanged
            RaiseEvent LoadControls(True)
        End Sub

        Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If MyMediaRow IsNot Nothing Then
                Dim metaData As String = ""

                If Not MyMediaRow.IsTitleNull Then
                    Me.Title = MyMediaRow.Title
                End If

                If Not MyMediaRow.IsDescriptionNull Then
                    metaData &= "<META name=""description"" content="""
                    metaData &= Server.HtmlEncode(MyMediaRow.Description)
                    metaData &= """ />"
                End If

                If tagTable.Count = 0 Then
                    Try
                        sqlHelper.SafeFillTable("select * from DTI_Content_Tags where Id in " & _
                            "(select Tag_Id from DTI_Content_Tag_Pivot where " & _
                            "Component_Type = @compType and Content_Id in (select Id from DTIMediaManager where Content_Id = @pid))", tagTable, _
                            New Object() {MyMediaRow.Component_Type, MyMediaRow.Content_Id})
                    Catch ex As Exception
                        sqlHelper.checkAndCreateTable(tagTable)
                        sqlHelper.checkAndCreateTable(New DTITagManager.dsTagger.DTI_Content_Tag_PivotDataTable)
                        sqlHelper.SafeFillTable("select * from DTI_Content_Tags where Id in " & _
                            "(select Tag_Id from DTI_Content_Tag_Pivot where " & _
                            "Component_Type = @compType and Content_Id = @pid)", tagTable, _
                            New Object() {MyMediaRow.Component_Type, MyMediaRow.Content_Id})
                    End Try

                End If

                If tagTable.Count > 0 Then
                    metaData &= "<META name=""keywords"" content="""
                    For Each tag As DTI_Content_TagsRow In tagTable
                        metaData &= Server.HtmlEncode(tag.Tag_Name) & ","
                    Next
                    metaData = metaData.Trim()
                    metaData = metaData.Trim(",") 'New Char() {","c}
                    metaData &= """ />"
                End If
                If Page.Header Is Nothing Then
                    jQueryLibrary.jQueryInclude.addControlToHeader(Page, New LiteralControl(metaData))
                Else
                    Page.Header.Controls.Add(New LiteralControl(metaData))
                End If
            End If

        End Sub

#End Region

#Region "Helper Function"

        Public Sub moveIt(ByVal sourceSelector As String, ByVal destSelector As String, Optional ByRef pageRef As Page = Nothing)
            If pageRef Is Nothing Then pageRef = Me
            DTISharedVariables.moveIt(sourceSelector, destSelector, pageRef)
        End Sub

        'Public Sub registerClientScriptBlock(ByVal key As String, ByVal script As String, Optional ByVal addScriptTags As Boolean = False, Optional ByRef addPage As Page = Nothing)
        '    ClientScript.RegisterClientScriptBlock(Me.GetType, key, script, addScriptTags)
        'End Sub

        Public Sub registerClientStartupScriptBlock(ByVal key As String, ByVal script As String, Optional ByVal addScriptTags As Boolean = False, Optional ByRef addPage As Page = Nothing)
            ClientScript.RegisterStartupScript(Me.GetType, key, script, addScriptTags)
        End Sub

        Public Sub registerClientScriptFile(ByVal key As String, ByVal URL As String, Optional ByRef addPage As Page = Nothing)
            ClientScript.RegisterClientScriptInclude(Me.GetType, key, URL)
        End Sub

        'Protected Sub addscriptmanagerToPage()
        '    If AJAXEnabled AndAlso ScriptManager.GetCurrent(Me) Is Nothing Then
        '        Form.Controls.AddAt(0, New ScriptManager)
        '    End If
        'End Sub

        Protected Sub raiseDataChanged()
            RaiseEvent DataChanged()
        End Sub

        Protected Function getPageMediaRow(ByVal page_Id As Integer) As dsMedia.DTIMediaManagerRow
            If DTIServerControl.initExternalType(GetType(DTIBasePage)) Then
                Dim dstag As New DTITagManager.dsTagger
                sqlHelper.checkAndCreateTable(dstag.DTI_Content_Tags)
                sqlHelper.checkAndCreateTable(dstag.DTI_Content_Tag_Pivot)
            End If
            Dim mediaRow As DTIMediaManager.dsMedia.DTIMediaManagerRow
            For Each row As DTIMediaManager.dsMedia.DTIMediaManagerRow In SharedMediaVariables.myMediaTable
                If row.Content_Type = "page" AndAlso row.Content_Id = page_Id Then
                    mediaRow = row
                    Exit For
                End If
            Next
            If mediaRow Is Nothing Then
                parallelDataHelper.addFillDataTable("select * from DTI_Content_Tags where Id in " & _
                    "(select Tag_Id from DTI_Content_Tag_Pivot where " & _
                    "Component_Type = 'ContentManagement' and Content_Id in (select Id from DTIMediaManager where Content_Id = @pid))", tagTable, _
                    New Object() {page_Id})
                parallelDataHelper.addFillDataTable("select * from DTIMediaManager where Content_Id = @pid and " & _
                    "Content_Type = 'page'", SharedMediaVariables.myMediaTable, _
                    New Object() {page_Id})

                parallelDataHelper.executeParallelDBCall()

                Dim dv As New DataView(SharedMediaVariables.myMediaTable, "Content_Type = 'page' and Content_Id = " & _
                    page_Id, "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    mediaRow = dv(0).Row
                End If

                If mediaRow Is Nothing AndAlso page_Id <> -1 Then
                    mediaRow = SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow
                    With mediaRow
                        .Component_Type = "ContentManagement"
                        .Content_Type = "page"
                        .Content_Id = page_Id
                        .Date_Added = Now
                        .Published = True
                        .Removed = False
                        .User_Id = -1 'currentUser.id
                        .Permanent_URL = Request.Url.PathAndQuery
                    End With
                    SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(mediaRow)
                    sqlHelper.Update(mediaRow.Table)
                End If
            End If

            Return mediaRow
        End Function



#End Region


    End Class
