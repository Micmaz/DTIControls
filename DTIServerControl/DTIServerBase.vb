Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.SessionState
Imports BaseClasses
Imports DTIMiniControls
Imports HighslideControls
Imports System.Text.RegularExpressions

''' <summary>
''' Base class for datacentric controls. Adds data accessors and design-time support.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Base class for datacentric controls. Adds data accessors and design-time support."),ToolboxData("<{0}:DTIServerBase runat=server></{0}:DTIServerBase>"), _
Designer(GetType(DTIServerBaseDesigner)), ViewStateModeById()> _
Public MustInherit Class DTIServerBase
    Inherits Panel

    Protected Friend WithEvents mypage As Page 'BaseClasses.BaseSecurityPage

    ''' <summary>
    ''' Gets the html for rendering at design time.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the html for rendering at design time."),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Overridable Function getDesignTimeHtml() As String
        Return Nothing
        'Return " "
    End Function

    ''' <summary>
    ''' Raised when the internal parallel data helper prepares a the data
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Raised when the internal parallel data helper prepares a the data")> _
    Public Event DataReady()
    Protected Event DataChanged()

    ''' <summary>
    ''' Called once in an application context when this type is first initialized. Used for data setup etc.
    ''' </summary>
    ''' <param name="t"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Called once in an application context when this type is first initialized. Used for data setup etc.")> _
    Public Event typeFirstInitialized(ByVal t As Type)

    ''' <summary>
    ''' Called once per type per request. Usefull if many controls are on a page but a page event only needs to occure once.
    ''' </summary>
    ''' <param name="t"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Called once per type per request. Usefull if many controls are on a page but a page event only needs to occure once.")> _
    Public Event typeInitThisRequest(ByVal t As Type)


#Region "Properties"

    ''' <summary>
    ''' Component type name used to group sql commands into a que.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Component type name used to group sql commands into a que.")> _
    Public Overridable Property Component_Type() As String
        Get
            Return _component_type
        End Get
        Set(ByVal value As String)
            Dim oldCtype = _component_type
            _component_type = value
            If oldCtype <> "" AndAlso oldCtype <> _component_type Then
                raiseDataChanged()
            End If
        End Set
    End Property
    Private _component_type As String = Me.GetType.ToString



    'Protected WithEvents _freezer As FreezeIt

    '''' <summary>
    '''' Returns the Freezis control for freezing the panel in edit or layout modes.
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Returns the Freezis control for freezing the panel in edit or layout modes.")> _
    'Public ReadOnly Property Freezer() As FreezeIt
    '    Get
    '        If Not Me.DesignMode Then
    '            enableFreezing()
    '            Return _freezer
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    'End Property

    Dim _designmodeSession As DTISharedVariables.HttpSessionState

    ''' <summary>
    ''' The session object that supports design time access.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The session object that supports design time access.")> _
    Protected ReadOnly Property Session() As DTISharedVariables.HttpSessionState
        Get
            If _designmodeSession Is Nothing Then
                _designmodeSession = New DTISharedVariables.HttpSessionState
                _designmodeSession.isdesign = DesignMode
            End If
            Return _designmodeSession
        End Get
    End Property

    ''' <summary>
    ''' The optional data object 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> Public Property DataSource() As Object
        Get
            Return Session("DTIServerControlDataSource." & Me.GetType.ToString)
        End Get
        Set(ByVal Value As Object)
            Session("DTIServerControlDataSource." & Me.GetType.ToString) = Value
        End Set
    End Property

    Private _contentType As String = ""
	private r As new Regex( _
		  "[^A-Za-z0-9_]+", _
		RegexOptions.IgnoreCase _
		Or RegexOptions.Multiline _
		Or RegexOptions.CultureInvariant _
		Or RegexOptions.Compiled _
		)

    Private _uniquePerPage As Boolean = False

    ''' <summary>
    ''' If set true, the page name is automatically appended to the contentType.
    ''' This allows you to add serverControls to a master page and have them associate with different data for each page.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set true, the page name is automatically appended to the contentType.   This allows you to add serverControls to a master page and have them associate with different data for each page.")> _
    Public Overridable Property UniquePerPage As Boolean
        Get
            Return _uniquePerPage
        End Get
        Set(value As Boolean)
            _uniquePerPage = value
        End Set
    End Property

    ''' <summary>
    ''' The string identifier for a control instance. 
    ''' This allong with the mainId forms a unique identifier for the object for the entire application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The string identifier for a control instance.    This allong with the mainId forms a unique identifier for the object for the entire application.")> _
    Public Overridable Property contentType() As String
        Get
            If _contentType = "" Then
                If DesignMode Then
                    Return Me.Page.Site.Name.ToLower.Trim("_") & "_" & ID
                Else
                    Dim Typename As String = Me.Page.GetType.Name.ToLower
                    If Not Me.Page.Master Is Nothing AndAlso Me.NamingContainer Is Me.Page.Master AndAlso Not UniquePerPage Then
                        Typename = Me.Page.MasterPageFile.ToLower.Replace(".master", "").Replace("/", "").Replace("\", "")
                    End If

                    If Typename.EndsWith("_aspx") Then Typename = Me.Page.GetType.BaseType.Name
                    Return Typename.ToLower.Trim("_") & "_" & ID
                    '    Return System.IO.Path.GetFileName(Page.Request.PhysicalPath).ToLower.Replace(".aspx", "") & "_" & ID

                End If
            End If

                Return _contentType
        End Get
        Set(ByVal value As String)
            Dim oldCtype = _contentType
            value = value.Replace(" ", "_")
			If UniquePerPage Then
				Dim Typename As String = ""
				If Not DesignMode Then
					Typename = HttpContext.Current.CurrentHandler.GetType.Name.ToLower.Trim("_")
				End If

				value = value & "_" & Typename
			End If
			_contentType = r.Replace(value,"")				
            If oldCtype <> _contentType Then
                raiseDataChanged()
            End If
        End Set
    End Property

    Public Overridable ReadOnly Property identifierString() As String
        Get
            Return contentType & "_" & MainID 'ClientID
        End Get
    End Property

    Private doDataChangedOninit As Boolean = False
    Private _MainID As Long = DTISharedVariables.MasterMainId
    Public Property MainID() As Long
        Get
            If DesignMode Then
                Return _MainID
            End If
            'If MasterMainId > 0 Then
            '    Return MasterMainId
            'Else
            'language id is bit shifted to the top 10 bits and then Read into the mainid
            Dim shifted_language_id As Long = LanguageID * Math.Pow(2, 55)
            Return shifted_language_id Or _MainID
            'End If
        End Get
        Set(ByVal value As Long)
            Dim oldMain = _MainID
            _MainID = value
            If DesignMode Then Return
            If oldMain <> _MainID Then
                If Not Me.Page Is Nothing Then
                    raiseDataChanged()
                Else
                    doDataChangedOninit = True
                End If
            End If
        End Set
    End Property

	'Public Overridable Property MasterMainId() As Long
	'    Get
	'        Return DTISharedVariables.MasterMainId
	'    End Get
	'    Set(ByVal value As Long)
	'        Dim oldMainId = DTISharedVariables.MasterMainId
	'        DTISharedVariables.MasterMainId = value
	'        If oldMainId <> value Then
	'            RaiseEvent DataChanged()
	'        End If
	'    End Set
	'End Property

	'   Protected Shadows ReadOnly Property DesignMode() As Boolean
	'	Get
	'		Return System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio")
	'	End Get
	'End Property


	Private _LanguageID As Integer = 0
    Public Property LanguageID() As Integer
        Get
            'If MasterLanguageId >= 0 Then
            '    Return MasterLanguageId
            'Else
            Return _LanguageID
            'End If
        End Get
        Set(ByVal value As Integer)
            Dim oldLanguage = _LanguageID
            _LanguageID = value
            If oldLanguage <> _LanguageID Then
                raiseDataChanged()
            End If
        End Set
    End Property

    'Public Overridable Property MasterLanguageId() As Integer
    '    Get
    '        Return DTISharedVariables.MasterLanguageId
    '    End Get
    '    Set(ByVal value As Integer)
    '        Dim oldMasterLangId = DTISharedVariables.MasterLanguageId
    '        DTISharedVariables.MasterLanguageId = value
    '        If oldMasterLangId <> value Then
    '            RaiseEvent DataChanged()
    '        End If
    '    End Set
    'End Property

    'Public Overridable Property AdminOn() As Boolean
    '    Get
    '        Return DTISharedVariables.AdminOn
    '    End Get
    '    Set(ByVal value As Boolean)
    '        DTISharedVariables.AdminOn = value
    '    End Set
    'End Property

	private myprivateHelper as BaseHelper = nothing
    Public Property sqlhelper() As BaseHelper
        Get
			if myprivateHelper is nothing then 
				Return BaseClasses.BaseHelper.getHelper
			else
				return myprivateHelper
			end if
        End Get
		Set(ByVal value As BaseHelper)
			myprivateHelper = value
		End Set
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

    Public Enum renderModes
        Span
        Div
    End Enum

    Private _renderMode As renderModes = renderModes.Div
    Public Property RenderMode() As renderModes
        Get
            Return _renderMode
        End Get
        Set(ByVal value As renderModes)
            _renderMode = value
        End Set
    End Property

    Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
        Get
            If RenderMode = renderModes.Div Then
                Return HtmlTextWriterTag.Div
            ElseIf RenderMode = renderModes.Span Then
                Return HtmlTextWriterTag.Span
            End If
        End Get
    End Property

#End Region

#Region "Events"



    Private Sub _parallelhelper_DataReady() Handles _parallelhelper.DataReady
        RaiseEvent DataReady()
    End Sub


    Private Shared _typeLst As Generic.List(Of Type)
    Private Shared ReadOnly Property typeLst() As Generic.List(Of Type)
        Get
            SyncLock GetType(DTIServerControl)
                If _typeLst Is Nothing Then _typeLst = New Generic.List(Of Type)
            End SyncLock
            Return _typeLst
        End Get
    End Property

    Private Sub inittypeLst()
        inittypeLst(Me.GetType)
    End Sub

    Protected Function initTypePerRequest(ByVal aType As Type, ByVal page As Page) As Boolean
        SyncLock aType
            If Not page.Items.Contains(aType.Name) Then
                page.Items.Add(aType.Name, True)
                If Not DesignMode Then
                    RaiseEvent typeInitThisRequest(aType)
                End If
                Return True
            End If
            Return False
        End SyncLock
    End Function


    Protected Function inittypeLst(ByVal aType As Type) As Boolean
        SyncLock aType
            If Not typeLst.Contains(aType) Then
                typeLst.Add(aType)
                If Not DesignMode Then
                    RaiseEvent typeFirstInitialized(aType)
                End If
                Return True
            End If
            Return False
        End SyncLock
    End Function

    Public Shared Function initExternalType(ByVal aType As Type) As Boolean
        SyncLock aType
            If Not typeLst.Contains(aType) Then
                typeLst.Add(aType)
                Return True
            End If
            Return False
        End SyncLock
    End Function

    Private Sub DTIServerControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.DesignMode Then
            mypage = Me.Page
            If Not typeLst.Contains(Me.GetType) Then inittypeLst()
            initTypePerRequest(Me.GetType, mypage)
            'If Not TypeOf (mypage) Is BaseSecurityPage Then
            BaseVirtualPathProvider.registerVirtualPathProvider()
            'RaiseEvent LoadControls(False)
            'End If
            If doDataChangedOninit Then
                raiseDataChanged()
            End If
        End If
    End Sub


    Public ReadOnly Property uniqueIdentifier() As String
        Get
            Return BaseClasses.Scripts.GenerateHash(Me.contentType & "_" & Me.MainID & Me.GetType.Name)
        End Get
    End Property

    Public Sub enableFreezing()
        FreezeIt.addFreezitScript(Me.Page)
        'If _freezer Is Nothing Then
        '    _freezer = New FreezeIt
        '    _freezer.FreezeId = "freezer_" & Me.ClientID
        '    _freezer.ID = "freezer_" & Me.ClientID
        '    _freezer.DisplayOnAnyPostback = False
        '    Me.Controls.AddAt(0, _freezer)
        '    moveIt("#" & _freezer.ClientID, "#form1")
        '    'jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "function freeze_" & Me.uniqueIdentifier & "(){FreezeItEl('" & Me.ClientID & "','" & Freezer.ClientID & "');}")
        '    'jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "function unfreeze_" & Me.uniqueIdentifier & "(){UnfreezeItEl('" & Freezer.ClientID & "');}")
        '    jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "function freeze_" & Me.ID & "(){FreezeItEl('" & Me.ClientID & "','" & Freezer.ClientID & "');} " & _
        '                "function unfreeze_" & Me.ID & "(){UnfreezeItEl('" & Freezer.ClientID & "');}")
        'End If
    End Sub


    'Public Sub disableFreezing()
    '    If Not _freezer Is Nothing Then
    '        Me.Controls.Remove(_freezer)
    '        _freezer = Nothing
    '    End If
    'End Sub



    'Private Sub setFreezer()
    '    'If Not _freezer Is Nothing AndAlso Freezer.IsFrozen Then
    '        Me.BorderStyle = UI.WebControls.BorderStyle.Dashed
    '        Me.BorderColor = Drawing.Color.Gray
    '        Me.BorderWidth = New WebControls.Unit("3px")
    '        Me.Attributes.Add("onmouseover", "style.borderColor='#FF2222'")
    '        Me.Attributes.Add("onmouseout", "style.borderColor='#777777'")
    '    End If
    'End Sub

#End Region

#Region "Helper Function"

    ''' <summary>
    ''' Creates a new highslide oject if one doesnot exist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a new highslide oject if one doesnot exist.")> _
    Public Function SetupHighslide() As Highslider
        If _setupHighslide Is Nothing Then
            _setupHighslide = New Highslider
        End If
        Return _setupHighslide
    End Function
    Private _setupHighslide As Highslider


    Private _myHighslideHeader As HighslideControls.HighslideHeaderControl

    ''' <summary>
    ''' Creates and registers a highslide header control. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates and registers a highslide header control.")> _
    Public Function MyHighslideHeader() As HighslideControls.HighslideHeaderControl
        If Not DesignMode Then
            registerHighslide()
            Return _myHighslideHeader
        Else : Return Nothing
        End If
    End Function
    Public Sub moveIt(ByVal sourceSelector As String, ByVal destSelector As String, Optional ByRef pageRef As Page = Nothing)
        If pageRef Is Nothing Then pageRef = mypage
        DTISharedVariables.moveIt(sourceSelector, destSelector, pageRef)
    End Sub

    Public Sub registerClientScriptBlock(ByVal key As String, ByVal script As String, Optional ByVal addScriptTags As Boolean = False, Optional ByRef addPage As Page = Nothing)
        If addPage Is Nothing Then addPage = mypage
        jQueryLibrary.jQueryInclude.addScriptBlock(mypage, script)
        'If Not addPage.ClientScript.IsClientScriptBlockRegistered(key) Then
        '    addPage.ClientScript.RegisterClientScriptBlock(Me.GetType, key, script, addScriptTags)
        'End If
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

    Protected Sub raiseDataChanged()
        If Not Me.DesignMode Then
            RaiseEvent DataChanged()
        End If
    End Sub

    Public Sub truncateMe(Optional ByVal max_length As Integer = 140, Optional ByVal more_text As String = ". . . more", Optional ByVal less_text As String = "less")
        DTISharedVariables.truncateIt("#" & Me.ClientID, Me.Page, max_length, more_text, less_text)
    End Sub

    Public Sub registerHighslide()
        If _myHighslideHeader Is Nothing Then
            _myHighslideHeader = HighslideControls.HighslideHeaderControl.addToPage(Me.Page)
            _myHighslideHeader.wrapperClassName = "draggable-header"
            _myHighslideHeader.Outline_Scheme = SharedHighslideVariables.Highslide_Outline_Scheme.RoundedWhite
        End If
    End Sub

#End Region


End Class

