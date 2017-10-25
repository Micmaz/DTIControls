<ParseChildren(True), PersistChildren(False)> _
Public Class Accordion
    Inherits Panel

    'Private hfSelectedAcc As New HiddenField

    Private _animated As Effect = Effect.slide

    ''' <summary>
    ''' Choose your favorite animation, or disable them (set to none).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Choose your favorite animation, or disable them (set to none).")> _
    Public Property Animated() As Effect
        Get
            Return _animated
        End Get
        Set(ByVal value As Effect)
            _animated = value
        End Set
    End Property

    Private _autoheight As Boolean = True

    ''' <summary>
    ''' If set, the highest content part is used as height reference for all other parts. 
    ''' Provides more consistent animations
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set, the highest content part is used as height reference for all other parts.    Provides more consistent animations")> _
    Public Property AutoHeight() As Boolean
        Get
            Return _autoheight
        End Get
        Set(ByVal value As Boolean)
            _autoheight = value
        End Set
    End Property

    Private _clearStyle As Boolean = False

    ''' <summary>
    ''' If set, clears height and overflow styles after finishing animations. 
    ''' This enables accordions to work with dynamic content. Won't work together 
    ''' with autoHeight.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set, clears height and overflow styles after finishing animations.    This enables accordions to work with dynamic content. Won't work together    with autoHeight.")> _
    Public Property ClearStyle() As Boolean
        Get
            Return _clearStyle
        End Get
        Set(ByVal value As Boolean)
            _clearStyle = value
        End Set
    End Property

    Private _collapsible As Boolean = False

    ''' <summary>
    ''' Whether all the sections can be closed at once. Allows collapsing the 
    ''' active section by the triggering event (click is the default)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Whether all the sections can be closed at once. Allows collapsing the    active section by the triggering event (click is the default)")> _
    Public Property Collapsible() As Boolean
        Get
            Return _collapsible
        End Get
        Set(ByVal value As Boolean)
            _collapsible = value
        End Set
    End Property

    Public Enum AccordionEvent
        click
        mouseover
    End Enum

    Private _event As AccordionEvent = AccordionEvent.click

    ''' <summary>
    ''' The event on which to trigger the accordion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The event on which to trigger the accordion")> _
    Public Property [Event]() As AccordionEvent
        Get
            Return _event
        End Get
        Set(ByVal value As AccordionEvent)
            _event = value
        End Set
    End Property

    Private _fillspace As Boolean = False

    ''' <summary>
    ''' If set, the accordion completely fills the height of the parent element. Overrides autoheight
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set, the accordion completely fills the height of the parent element. Overrides autoheight")> _
    Public Property FillSpace() As Boolean
        Get
            Return _fillspace
        End Get
        Set(ByVal value As Boolean)
            _fillspace = value
        End Set
    End Property

    Private _icons As String = ""

    ''' <summary>
    ''' Icons to use for headers. Icons may be specified for 'header' and 'headerSelected', 
    ''' and we recommend using the icons native to the jQuery UI CSS Framework 
    ''' manipulated by jQuery UI ThemeRoller
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>{ 'header': 'ui-icon-triangle-1-e', 'headerSelected': 'ui-icon-triangle-1-s' }</remarks>
    <System.ComponentModel.Description("Icons to use for headers. Icons may be specified for 'header' and 'headerSelected',    and we recommend using the icons native to the jQuery UI CSS Framework    manipulated by jQuery UI ThemeRoller")> _
    Public Property Icons() As String
        Get
            Return _icons
        End Get
        Set(ByVal value As String)
            _icons = value
        End Set
    End Property

    ''' <summary>
    ''' Index of the currently selected pane.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Index of the currently selected pane.")> _
    Public Property ActivePaneIndex() As Integer
        Get
            Try
                ViewState(Me.ClientID & "_activePaneIndex") = Integer.Parse(Page.Request.Params(Me.ClientID & "_hidden"))
            Catch ex As Exception
            End Try
            Return ViewState(Me.ClientID & "_activePaneIndex")
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            ViewState(Me.ClientID & "_activePaneIndex") = value
        End Set
    End Property

    ''' <summary>
    ''' The currently selected pane.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The currently selected pane.")> _
    Public Property ActivePane() As AccordionPane
        Get
            Return Me.Panes(ActivePaneIndex)
        End Get
        Set(ByVal value As AccordionPane)
            ActivePaneIndex = Me.Panes.IndexOf(value)
        End Set
    End Property

    ' Private _panes As List(Of AccordionPane) = New List(Of AccordionPane)
    <PersistenceMode(PersistenceMode.InnerProperty)> _
    Public ReadOnly Property Panes() As System.Web.UI.ControlCollection 'List(Of AccordionPane)
        Get
            'Return New List(Of AccordionPane) Me.Controls.SyncRoot()
            Return Me.Controls '_panes
        End Get
    End Property

#Region "Callbacks"
    Private _createCallback As String

    ''' <summary>
    ''' This event is triggered when accordion is created.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when accordion is created.")> _
    Public Property CreateCallback() As String
        Get
            Return _createCallback
        End Get
        Set(ByVal value As String)
            _createCallback = value
        End Set
    End Property

    Private _changeCallback As String

    ''' <summary>
    ''' This event is triggered every time the accordion changes. If the accordion is 
    ''' animated, the event will be triggered upon completion of the animation; 
    ''' otherwise, it is triggered immediately
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered every time the accordion changes. If the accordion is    animated, the event will be triggered upon completion of the animation;    otherwise, it is triggered immediately")> _
    Public Property ChangeCallback() As String
        Get
            Return _changeCallback
        End Get
        Set(ByVal value As String)
            _changeCallback = value
        End Set
    End Property

    Private _changeStartCallback As String

    ''' <summary>
    ''' This event is triggered every time the accordion starts to change
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered every time the accordion starts to change")> _
    Public Property ChangeStartCallback() As String
        Get
            Return _changeStartCallback
        End Get
        Set(ByVal value As String)
            _changeStartCallback = value
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
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""
        If ActivePaneIndex > 0 Then
            outstr &= "active: " & ActivePaneIndex & ","
        End If
        If Not Enabled Then
            outstr &= "disabled: true,"
        End If
        If Animated <> Effect.slide Then
            outstr &= "animated: '" & [Enum].GetName(GetType(Effect), Animated) & "',"
        ElseIf Animated = Effect.none Then
            outstr &= "animated: false,"
        End If
        If ClearStyle Then
            outstr &= "clearStyle: true,"
        End If
        If Collapsible Then
            outstr &= "collapsible: true,"
        End If
        If [Event] = AccordionEvent.mouseover Then
            outstr &= "event: 'mouseover',"
        End If
        If FillSpace Then
            outstr &= "fillSpace: true,"
        End If
        If Icons <> "" Then
            outstr &= "icons: " & Icons & ","
        End If
        If Not AutoHeight Then
            outstr &= "autoHeight: false,"
        End If

        'callbacks
        outstr &= "change: function( event, ui ) {"
        outstr &= "$('#" & Me.ClientID() & "_hidden').val($('#" & Me.ClientID() & "').accordion('option','active'));"
        outstr &= ChangeCallback & "},"

        If CreateCallback <> "" Then
            outstr &= "create: function( event, ui ) {" & CreateCallback & "},"
        End If

        If ChangeStartCallback <> "" Then
            outstr &= "changestart: function( event, ui ) {" & ChangeStartCallback & "},"
        End If

        If outstr <> "" Then
            outstr = "{" & outstr.TrimEnd(",") & "}"
        End If
        Return outstr
    End Function

    'Private Sub Accordion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    For Each p As AccordionPane In Panes
    '        Me.Controls.Add(p)
    '    Next
    'End Sub

    Private Sub Accordion_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim s As String = ""
        s &= "$(function(){"
        s &= "     $('#" & Me.ClientID & "').accordion("
        s &= renderparams()
        s &= "      );"
        s &= "});"
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<input type=""hidden"" id=""" & Me.ClientID & "_hidden"" name=""" & Me.ClientID & "_hidden"" />")
        MyBase.Render(writer)
    End Sub
End Class

Public Class AccordionPane
    Inherits Panel

    Private _header As String = ""
    Public Property Header() As String
        Get
            Return _header
        End Get
        Set(ByVal value As String)
            _header = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal Header As String, Optional ByVal Content As String = "")
        Me.Header = Header
        If Content <> "" Then
            Me.Controls.Add(New LiteralControl(Content))
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<h3><a href=""#"">" & Header & "</a></h3>")
        MyBase.Render(writer)
    End Sub
End Class