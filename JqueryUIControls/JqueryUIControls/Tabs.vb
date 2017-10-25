<ParseChildren(True), PersistChildren(False)> _
Public Class Tabs
    Inherits Panel

    'Private hfSelectedTab As New HiddenField
    Private btnPostBack As New Button

#Region "Public Properties"

    ''' <summary>
    ''' Index of the currently selected tab.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Index of the currently selected tab.")> _
    Public Property SelectedTabIndex() As Integer
        Get
            Try
                If Me.DesignMode Then Return Nothing
                ViewState(Me.ClientID & "_selectedTabIndex") = Integer.Parse(Page.Request.Params(Me.ClientID & "_hidden"))
            Catch ex As Exception
            End Try
            Return ViewState(Me.ClientID & "_selectedTabIndex")
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            ViewState(Me.ClientID & "_selectedTabIndex") = value
        End Set
    End Property

    ''' <summary>
    ''' The currently selected tab.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The currently selected tab.")> _
    Public Property SelectedTab() As Tab
        Get
            Return Me.Tabs(SelectedTabIndex)
        End Get
        Set(ByVal value As Tab)
            SelectedTabIndex = Me.Tabs.IndexOf(value)
        End Set
    End Property

    Private _AutoPostBack As Boolean = False

    ''' <summary>
    ''' Trigger a post back event when the selected tab is changed (on tab click).  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Trigger a post back event when the selected tab is changed (on tab click).")> _
    Public Property AutoPostBack() As Boolean
        Get
            Return _AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            _AutoPostBack = value
        End Set
    End Property

    Private _tabs As TabsCollection = New TabsCollection

    ''' <summary>
    ''' Collection of Tab objects within Tabs
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Collection of Tab objects within Tabs"),PersistenceMode(PersistenceMode.InnerProperty)> _
    Public ReadOnly Property Tabs() As TabsCollection
        Get
            Return _tabs
        End Get
    End Property
#End Region

#Region "Callbacks"
    Private _onSelectCallback As String
    Private _onCreateCallback As String
    Private _onAddCallback As String
    Private _onShowCallback As String

    ''' <summary>
    ''' triggered when clicking a tab.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("triggered when clicking a tab.")> _
    Public Property onSelectCallback() As String
        Get
            Return _onSelectCallback
        End Get
        Set(ByVal value As String)
            _onSelectCallback = value
        End Set
    End Property

    ''' <summary>
    ''' triggered when tabs is created.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("triggered when tabs is created.")> _
    Public Property onCreateCallback() As String
        Get
            Return _onCreateCallback
        End Get
        Set(ByVal value As String)
            _onCreateCallback = value
        End Set
    End Property

    ''' <summary>
    ''' triggered when a tab is added.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("triggered when a tab is added.")> _
    Public Property onAddCallback() As String
        Get
            Return _onAddCallback
        End Get
        Set(ByVal value As String)
            _onAddCallback = value
        End Set
    End Property

    ''' <summary>
    ''' triggered when a tab is shown.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("triggered when a tab is shown.")> _
    Public Property onShowCallback() As String
        Get
            Return _onShowCallback
        End Get
        Set(ByVal value As String)
            _onShowCallback = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.autoheight.js")
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""
        If SelectedTabIndex > 0 Then
            outstr &= "selected: " & SelectedTabIndex & ","
        End If
        If Not Enabled Then
            outstr &= "disabled: true,"
        Else
            Dim ids As String = ""
            For Each tb As Tab In Me.Tabs
                If tb.Enabled = False Then
                    ids &= Me.Tabs.IndexOf(tb) & ","
                End If
            Next
            ids = ids.TrimEnd(",")
            If ids <> "" Then
                outstr &= "disabled: [" & ids & "],"
            End If
        End If
        If onSelectCallback <> "" Then
            outstr &= "select: function(event,ui){" & onSelectCallback & "},"
        End If
        If onCreateCallback <> "" Then
            outstr &= "create: function(event,ui){" & onCreateCallback & "},"
        End If
        If onAddCallback <> "" Then
            outstr &= "add: function(event,ui){" & onAddCallback & "},"
        End If
        outstr &= "show: function(event,ui){try{setHeight(ui.panel.children[0]);}catch(err){};$('#" & Me.ClientID() & "_hidden').val($('#" & Me.ClientID() & "').tabs('option','selected'));" & onShowCallback & "}"

        outstr = outstr.TrimEnd(",")

        If outstr <> "" Then
            outstr = "{" & outstr & "}"
        End If
        Return outstr
    End Function

    Private Sub Tabs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Tabs.Count > 0 Then
            Me.Controls.Add(New LiteralControl("<ul>" & vbCrLf))
            For Each tb As Tab In Tabs
                Me.Controls.Add(New LiteralControl("<li><a href=""#" & Me.ClientID & "-" & Tabs.IndexOf(tb) & """>" & tb.Title & "</a></li>" & vbCrLf))
            Next
            Me.Controls.Add(New LiteralControl("</ul>" & vbCrLf))
        End If
        For Each tb As Tab In Tabs
            tb.ID = Me.ID & "-" & Tabs.IndexOf(tb)
            Me.Controls.Add(tb)
        Next
    End Sub

    Private Sub Tabs_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim s As String = ""
        s &= "$(function(){"
        s &= "     $('#" & Me.ClientID & "').tabs("
        s &= renderparams()
        s &= "      );"
        s &= "});"
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<input type=""hidden"" id=""" & Me.ClientID & "_hidden"" name=""" & Me.ClientID & "_hidden"" />")
        MyBase.Render(writer)
    End Sub

    Public Sub AddTab(ByVal title As String, ByVal Content As String, Optional ByVal ContentIsUrl As Boolean = False, Optional ByVal key As String = "")
        Tabs.Add(New Tab(title, Content, ContentIsUrl))
    End Sub

    Public Sub AddTab(ByVal title As String, ByVal Content As String, ByVal ContentIsUrl As Boolean, ByVal height As Unit)
        Tabs.Add(New Tab(title, Content, ContentIsUrl, height))
    End Sub

    Public Sub AddTab(ByVal title As String, ByVal Content As Control)
        Tabs.Add(New Tab(title, Content))
    End Sub
End Class

Public Class Tab
    Inherits Panel

    Private _title As String = ""

    ''' <summary>
    ''' Name of the tab
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Name of the tab")> _
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _targetURL As String = ""

    ''' <summary>
    ''' Url for iframe within tab
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Url for iframe within tab")> _
    Public Property TargetURL() As String
        Get
            Return _targetURL
        End Get
        Set(ByVal value As String)
            _targetURL = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal title As String, ByVal Content As Control, Optional ByVal key As String = "")
        Me.Title = title
        Me.Controls.Add(Content)
    End Sub

    Public Sub New(ByVal title As String, ByVal Content As String, Optional ByVal ContentIsUrl As Boolean = False, Optional ByVal key As String = "")
        Me.Title = title
        If ContentIsUrl Then
            Me.TargetURL = Content
        End If
        addControls(Content)
    End Sub

    Public Sub New(ByVal title As String, ByVal Content As String, ByVal ContentIsUrl As Boolean, ByVal height As Unit, Optional ByVal key As String = "")
        Me.Title = title
        Me.Height = height
        If ContentIsUrl Then
            Me.TargetURL = Content
        End If
        addControls(Content)
    End Sub

    Private Sub addControls(ByVal content As String)
        If Me.TargetURL = "" Then
            Me.Controls.Add(New LiteralControl(content))
        Else
            Me.Controls.Add(New LiteralControl("<iframe src=""" & Me.TargetURL & """ class=""autoHeight"" width=""100%"" frameBorder=""0""></iframe>"))
        End If
    End Sub
End Class

Public Class TabsCollection
    Inherits System.Collections.ObjectModel.Collection(Of Tab)

    Public Shadows Sub add(ByVal item As Tab)
        If Find(item.Title) Is Nothing Then
            MyBase.Add(item)
        Else
            Throw New ArgumentException("A Tab with that title already exists in the Tab List.")
        End If
    End Sub

    Public Function Find(ByVal title As String) As Tab
        Dim it As Tab = Nothing
        For Each itm As Tab In Me
            If itm.Title.ToLower = title.ToLower Then it = itm
            If it IsNot Nothing Then Exit For
        Next
        Return it
    End Function

    Public Overloads Function IndexOf(ByVal title As String) As Integer
        Dim it As Tab = Find(title)
        Return IndexOf(it)
    End Function
End Class