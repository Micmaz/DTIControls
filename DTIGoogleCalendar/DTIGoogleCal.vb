Imports DTIServerControls
Imports DTIGoogleCalendar.SharedGoogleCalendarVariables

''' <summary>
''' Control to display and edit a Google calendar.  Available in the DTIControls toolbox
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Control to display and edit a Google calendar. Available in the DTIControls toolbox"),ComponentModel.ToolboxItem(False)> _
Public Class DTIGoogleCal
    Inherits DTIServerControl

    'Public googleCalEditControl As GoogleCalendarEditControl
    'Public DataFetched As Boolean = False
    'Private FrameWidth As Integer = 800
    'Private FrameHeight As Integer = 600
    Protected WithEvents cal_frame As New System.Web.UI.HtmlControls.HtmlGenericControl

    Public Overrides ReadOnly Property Menu_Icon_Url() As String
        Get
            Return BaseClasses.Scripts.ScriptsURL & "DTIGoogleCalendar/CalIcon.png"
        End Get
    End Property

    'Private _calRow As dsGoogleCal.DTIGoogleCalendarRow
    'Private Property calendarRow() As dsGoogleCal.DTIGoogleCalendarRow
    '    Get
    '        Return _calRow
    '    End Get
    '    Set(ByVal value As dsGoogleCal.DTIGoogleCalendarRow)
    '        _calRow = value
    '    End Set
    'End Property

    Public Property src() As String
        Get
            Return cal_frame.Attributes("src")
        End Get
        Set(ByVal value As String)
            If cal_frame.Attributes("src") Is Nothing Then
                cal_frame.Attributes.Add("src", value)
            Else
                cal_frame.Attributes("src") = value
            End If
        End Set
    End Property

    'Private Sub DTIMapServerControl_DataChanged() Handles Me.DataChanged
    '    addSQLCall()
    'End Sub

    'Private Sub DTIMapServerControl_DataReady() Handles Me.DataReady
    '    DataFetched = True
    'End Sub

    Private _tagName As String
    Public Property tagName() As String
        Get
            Return _tagName
        End Get
        Set(ByVal value As String)
            _tagName = value
        End Set
    End Property

    Private _allowTransparency As Boolean
    Public Property allowTransparency() As Boolean
        Get
            Return _allowTransparency
        End Get
        Set(ByVal value As Boolean)
            _allowTransparency = value
        End Set
    End Property

    Private _frameBorder As Integer
    Public Property frameBorder() As Integer
        Get
            Return _frameBorder
        End Get
        Set(ByVal value As Integer)
            _frameBorder = value
        End Set
    End Property

    Private _scrolling As Boolean
    Public Property scrolling() As Boolean
        Get
            Return _scrolling
        End Get
        Set(ByVal value As Boolean)
            _scrolling = value
        End Set
    End Property

    Private _backgroundColor As String
    Public Property backgroundColor() As String
        Get
            Return _backgroundColor
        End Get
        Set(ByVal value As String)
            _backgroundColor = value
        End Set
    End Property

    Private _borderWidth As Integer
    Public Property borderWidth() As Integer
        Get
            Return _borderWidth
        End Get
        Set(ByVal value As Integer)
            _borderWidth = value
        End Set
    End Property

    Private Sub DTIGoogleCal_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        With cal_frame
            .TagName = "iframe"
            .Attributes.Add("allowTransparency", "true")
            .Attributes.Add("frameBorder", "0")
            .Attributes.Add("scrolling", "no")
            .Style("backgroundColor") = "transparent"
            .Style("border-Width") = "0"
        End With

        src = "http://www.google.com/calendar/embed"

        'If Not mypage.IsPostBack Then
        '    addSQLCall()
        'End If
    End Sub

    Private Sub DTIGoogleCal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not DataFetched AndAlso Not mypage.IsPostBack Then
        '    parallelhelper.executeParallelDBCall()
        'End If
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
    End Sub

    Private Sub DTIGoogleCal_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
        cal_frame.ID = ClientID & "_iframe"
        Controls.Add(cal_frame)
        'If Mode <> modes.Read Then
        'googleCalEditControl = DirectCast(mypage.LoadControl("~/res/DTIGoogleCalendar/GoogleCalendarEditControl.ascx"), GoogleCalendarEditControl)
        'setupControlList.Add(googleCalEditControl)
        'setupPanel.Width = 250
        'End If
    End Sub

    Private Sub DTIGoogleCal_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'cal_frame.ClientID = ClientID & "_iframe"
        setMode()
        'setCalendar()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        setDimensions()
        MyBase.Render(writer)
    End Sub

    Private Sub setDimensions()
        Dim FrameWidth As Integer = 800
        Dim FrameHeight As Integer = 600
        If Me.Width.IsEmpty OrElse Me.Width.Value = 0 Then
            Me.Width = FrameWidth
        Else
            FrameWidth = Me.Width.Value
        End If
        If Me.Height.IsEmpty OrElse Me.Height.Value = 0 Then
            Me.Height = FrameHeight
        Else
            FrameHeight = Me.Height.Value
        End If

        cal_frame.Attributes.Add("width", FrameWidth)
        cal_frame.Attributes.Add("height", FrameHeight)
    End Sub

    Private Sub setMode()

        If Mode = modes.Read Then
            ShowBorder = False
        Else
            ShowBorder = True
            'googleCalEditControl.contentType = contentType
            'googleCalEditControl.ControlId = Me.ClientID

            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
			jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, "$.query = { prefix: false };")
			jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "jQueryLibrary/jquery.query.js")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIGoogleCalendar/dsGoogleCal.js")
            'registerClientScriptBlock("queryInit", "$.query = { prefix: false };", True)
            'registerClientScriptFile("JQQ", BaseClasses.Scripts.ScriptsURL(True) & _
            '    "jQueryLibrary/jquery.query.js")
            'registerClientScriptFile("dtiGoogleCalendar", BaseClasses.Scripts.ScriptsURL(True) & _
            '    "DTIGoogleCalendar/dtiGoogleCal.js")
        End If
    End Sub

    'Public Overridable Sub setCalendar()
    '    Dim rowSet As Boolean = setCalendarRow()

    '    If rowSet Then
    '        If calendarRow.IsIframe_SourceNull Then
    '            src = calendarRow.Iframe_Source
    '        End If
    '        If Not calendarRow.IsHeightNull Then
    '            Height = calendarRow.Height
    '        End If
    '        If Not calendarRow.IsWidthNull Then
    '            Width = calendarRow.Width
    '        End If
    '    ElseIf Mode = modes.Read Then
    '        'src = ""
    '        Width = 0
    '        Height = 0
    '    End If
    'End Sub

    'Private Function setCalendarRow() As Boolean
    '    If calendarRow Is Nothing Then
    '        For Each row As dsGoogleCal.DTIGoogleCalendarRow In calendarTable
    '            If row.Content_Type = contentType Then
    '                calendarRow = row
    '                Return True
    '            End If
    '        Next

    '    Else : Return True
    '    End If
    '    Return False
    'End Function

    'Private Sub addSQLCall()
    '    parallelhelper.addFillDataTable("select * from DTIGoogleCalendar where Main_ID = @mainId_" & contentType & " and Content_Type = @contentType_" & contentType, calendarTable, New Object() {MainID, contentType})
    'End Sub

    'Private Sub DTIGoogleCal_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
    '    Dim ds As New dsGoogleCal
    '    sqlhelper.checkAndCreateAllTables(ds)
    'End Sub

    Public Sub New()
        Me.settingsPageUrl = "settingsForm.aspx"
        Me.useGenericDTIControlsProperties = True
        'Me.ShowBorder = False
    End Sub
End Class
