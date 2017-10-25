Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses
Imports DTIMiniControls
Imports HighslideControls

''' <summary>
''' A place holder class with some mode aware atributes.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
<ToolboxData("<{0}:DTIServerControl runat=server />"), ComponentModel.ToolboxItem(False)> _
Public Class DTIPlaceHolder
#Else
Public Class DTIPlaceHolder
#End If
    Inherits PlaceHolder
    Protected Friend WithEvents mypage As Page 'BaseClasses.BaseSecurityPage
    Protected Friend setupPanel As Panel

    Public Enum modes
        Read = 1
        Write = 2
    End Enum

    Public Event ModeChanged()
    Public Event DataReady()
    Public Event LoadControls(ByVal modeChanged As Boolean)
    Protected Event DataChanged()


#Region "Properties"

    Private _setupHighslide As Highslider
    Public ReadOnly Property SetupHighslide() As Highslider
        Get
            If _setupHighslide Is Nothing Then
                _setupHighslide = New Highslider
            End If
            Return _setupHighslide
        End Get
    End Property

    Public Property setupControlList() As ControlCollection
        Get
            If setupPanel Is Nothing Then
                setupPanel = New Panel
                'setupPanel.ID = "setupPanel"
            End If
            Return setupPanel.Controls
        End Get
        Set(ByVal value As ControlCollection)

        End Set
    End Property

    Private ReadOnly Property showSetup() As Boolean
        Get
            If setupPanel Is Nothing OrElse setupPanel.Controls.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private _myHighslideHeader As HighslideControls.HighslideHeaderControl
    Public ReadOnly Property MyHighslideHeader() As HighslideControls.HighslideHeaderControl
        Get
            If _myHighslideHeader Is Nothing Then
                _myHighslideHeader = HighslideControls.HighslideHeaderControl.addToPage(Me.Page)
            End If
            Return _myHighslideHeader
        End Get
    End Property

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

    Public Shared ReadOnly Property siteEditLanguageID() As String
        Get
            Return DTISharedVariables.siteEditLanguageID
        End Get
    End Property

    Public Overridable ReadOnly Property Menu_Icon_Url() As String
        Get
            Return Nothing
        End Get
    End Property

    Private _component_type As String = Me.GetType.ToString
    Public Overridable Property Component_Type() As String
        Get
            Return _component_type
        End Get
        Set(ByVal value As String)
            Dim oldCtype = _component_type
            _component_type = value
            If oldCtype <> "" AndAlso oldCtype <> _component_type Then
                RaiseEvent DataChanged()
            End If
        End Set
    End Property

    Protected Shared ReadOnly Property Session() As DTISharedVariables.HttpSessionState
        Get
            Return DTISharedVariables.Session
        End Get
    End Property

    <Browsable(False)> Public Property DataSource() As Object
        Get
            Return Session("DTIServerControlDataSource." & Me.GetType.ToString)
        End Get
        Set(ByVal Value As Object)
            Session("DTIServerControlDataSource." & Me.GetType.ToString) = Value
        End Set
    End Property

    Private _contentType As String = ""
    Public Overridable Property contentType() As String
        Get
            If _contentType = "" Then Return ID
            Return _contentType
        End Get
        Set(ByVal value As String)
            Dim oldCtype = _contentType
            _contentType = value
            If oldCtype <> "" AndAlso oldCtype <> _contentType Then
                RaiseEvent DataChanged()
            End If
        End Set
    End Property

    Private _sortableId As Long = -1
    Public Overridable Property SortableItemId() As String
        Get
            Return _sortableId
        End Get
        Set(ByVal value As String)
            _sortableId = value
        End Set
    End Property

    Public Overridable ReadOnly Property identifierString() As String
        Get
            Return contentType & "_" & MainID 'ClientID
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
            If Session("ContentDisplayMode." & identifierString) Is Nothing Then
                Session("ContentDisplayMode." & identifierString) = modes.Read
            End If

            If AdminOn AndAlso Session("ContentDisplayMode." & identifierString) = modes.Read Then
                Session("ContentDisplayMode." & identifierString) = modes.Write
            End If

            If Not AdminOn AndAlso Not StaticMode AndAlso Session("ContentDisplayMode." & identifierString) <> modes.Read Then
                Session("ContentDisplayMode." & identifierString) = modes.Read
            End If

            Return Session("ContentDisplayMode." & identifierString)
        End Get
        Set(ByVal value As modes)
            Session("ContentDisplayMode." & identifierString) = value
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

    Private _helper As BaseHelper
    Protected ReadOnly Property sqlhelper() As BaseHelper
        Get
            If _helper Is Nothing Then
                If Not Me.Page Is Nothing Then
                    If TypeOf Me.Page Is BaseSecurityPage Then
                        _helper = CType(Me.Page, BaseSecurityPage).sqlHelper
                    ElseIf TypeOf Me.Page.Master Is MasterBase Then
                        _helper = CType(Me.Page.Master, MasterBase).sqlHelper
                    End If
                End If
                If _helper Is Nothing Then _helper = parallelhelper.sqlHelper
            End If
            Return _helper
        End Get
    End Property

    Private WithEvents _parallelhelper As ParallelDataHelper
    Protected ReadOnly Property parallelhelper() As ParallelDataHelper
        Get
            If _parallelhelper Is Nothing Then
                If Not Me.Page Is Nothing Then
                    If TypeOf Me.Page Is BaseSecurityPage Then
                        _parallelhelper = CType(Me.Page, BaseSecurityPage).parallelDataHelper
                    ElseIf TypeOf Me.Page.Master Is MasterBase Then
                        _parallelhelper = CType(Me.Page.Master, MasterBase).parallelDataHelper
                    End If
                End If
                If _parallelhelper Is Nothing Then _parallelhelper = jointParallelHelper
            End If
            Return _parallelhelper
        End Get
    End Property

    Private ReadOnly Property jointParallelHelper() As ParallelDataHelper
        Get
            If Session("jointParallelHelper") Is Nothing Then
                Session("jointParallelHelper") = New ParallelDataHelper
            End If
            Return Session("jointParallelHelper")
        End Get
    End Property

    Private _ajaxEnabled As Boolean = False
    <Description("Specifies whether a script manager is ensured to be on the page")> _
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

    Private Sub _parallelhelper_DataReady() Handles _parallelhelper.DataReady
        RaiseEvent DataReady()
    End Sub

    Private Sub DTIServerControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        mypage = Me.Page
        If Not TypeOf (mypage) Is BaseSecurityPage Then
            BaseVirtualPathProvider.registerVirtualPathProvider()
        End If
    End Sub

    'Private Sub mypage_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles mypage.InitComplete
    '    addscriptmanagerToPage()
    'End Sub

    Private Sub DTIServerControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RaiseEvent LoadControls(False)
    End Sub

    Private Sub DTIServerControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If showSetup AndAlso Mode <> modes.Read Then
            SetupHighslide.DisplayText = "<img style='border-width:0px;padding:5px;float:right;margin-top:-18px;margin-right:-19px;' src='" & BaseClasses.Scripts.ScriptsURL & "DTIServerControls/settingTab.png' border='0' />"
            SetupHighslide.HighslideDisplayMode = SharedHighslideVariables.HighslideDisplayModes.HTML

            setupPanel.CssClass = "highslide-maincontent"

            Controls.Add(SetupHighslide)
            Controls.Add(setupPanel)

            SetupHighslide.maincontentId = setupPanel.ClientID
        End If
    End Sub

    Private Sub DTIServerControl_ModeChanged() Handles Me.ModeChanged
        RaiseEvent LoadControls(True)
    End Sub

#End Region

#Region "Helper Function"

    Public Sub moveIt(ByVal sourceSelector As String, ByVal destSelector As String, Optional ByRef pageRef As Page = Nothing)
        If pageRef Is Nothing Then pageRef = mypage
        DTISharedVariables.moveIt(sourceSelector, destSelector, pageRef)
    End Sub

    Public Sub registerClientScriptBlock(ByVal key As String, ByVal script As String, Optional ByVal addScriptTags As Boolean = False, Optional ByRef addPage As Page = Nothing)
        If addPage Is Nothing Then addPage = mypage
        If Not addPage.ClientScript.IsClientScriptBlockRegistered(key) Then
            addPage.ClientScript.RegisterClientScriptBlock(Me.GetType, key, script, addScriptTags)
        End If
    End Sub

    Public Sub registerClientStartupScriptBlock(ByVal key As String, ByVal script As String, Optional ByVal addScriptTags As Boolean = False, Optional ByRef addPage As Page = Nothing)
        If addPage Is Nothing Then addPage = mypage
        If Not addPage.ClientScript.IsStartupScriptRegistered(key) Then
            addPage.ClientScript.RegisterStartupScript(Me.GetType, key, script, addScriptTags)
        End If
    End Sub

    Public Sub registerClientScriptFile(ByVal key As String, ByVal URL As String, Optional ByRef addPage As Page = Nothing)
        If addPage Is Nothing Then addPage = mypage
        If Not addPage.ClientScript.IsClientScriptIncludeRegistered(key) Then
            addPage.ClientScript.RegisterClientScriptInclude(Me.GetType, key, URL)
        End If
    End Sub

    'Protected Sub addscriptmanagerToPage()
    '    If AJAXEnabled AndAlso ScriptManager.GetCurrent(mypage) Is Nothing Then
    '        mypage.Form.Controls.AddAt(0, New ScriptManager)
    '    End If
    'End Sub

    Protected Sub raiseDataChanged()
        RaiseEvent DataChanged()
    End Sub

#End Region

End Class