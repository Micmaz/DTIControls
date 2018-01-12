<ToolboxData("<{0}:DatePicker ID=""DatePicker1"" runat=""server""> </{0}:DatePicker>")> _
Public Class DatePicker
    Inherits TextBox

#Region "Properties"
    Private _changeMonth As Boolean = False

    ''' <summary>
    ''' Allows you to change the month by selecting from a drop-down list. 
    ''' You can enable this feature by setting the attribute to true.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Allows you to change the month by selecting from a drop-down list.    You can enable this feature by setting the attribute to true.")> _
    Public Property ChangeMonth() As Boolean
        Get
            Return _changeMonth
        End Get
        Set(ByVal value As Boolean)
            _changeMonth = value
        End Set
    End Property

    Private _changeYear As Boolean = False

    ''' <summary>
    ''' Allows you to change the year by selecting from a drop-down list. You can enable this 
    ''' feature by setting the attribute to true. Use the yearRange option to control which 
    ''' years are made available for selection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Allows you to change the year by selecting from a drop-down list. You can enable this    feature by setting the attribute to true. Use the yearRange option to control which    years are made available for selection")> _
    Public Property ChangeYear() As Boolean
        Get
            Return _changeYear
        End Get
        Set(ByVal value As Boolean)
            _changeYear = value
        End Set
    End Property

    Private _minDate As String = ""

    ''' <summary>
    ''' Set a minimum selectable date via a Date object or as a string in the current 
    ''' dateFormat, or a number of days from today (e.g. +7) or a string of values and 
    ''' periods ('y' for years, 'm' for months, 'w' for weeks, 'd' 
    ''' for days, e.g. '-1y -1m'), or null for no limit.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Set a minimum selectable date via a Date object or as a string in the current    dateFormat, or a number of days from today (e.g. +7) or a string of values and    periods ('y' for years, 'm' for months, 'w' for weeks, 'd'    for days, e.g. '-1y -1m'), or null for no limit.")> _
    Public Property MinDate() As String
        Get
            Return _minDate
        End Get
        Set(ByVal value As String)
            _minDate = value
        End Set
    End Property

    Private _maxDate As String = ""

    ''' <summary>
    '''Set a maximum selectable date via a Date object or as a string in the current 
    ''' dateFormat, or a number of days from today (e.g. +7) or a string of values and 
    ''' periods ('y' for years, 'm' for months, 'w' for weeks, 'd' 
    ''' for days, e.g. '+1m +1w'), or null for no limit.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Set a maximum selectable date via a Date object or as a string in the current    dateFormat, or a number of days from today (e.g. +7) or a string of values and    periods ('y' for years, 'm' for months, 'w' for weeks, 'd'    for days, e.g. '+1m +1w'), or null for no limit.")> _
    Public Property MaxDate() As String
        Get
            Return _maxDate
        End Get
        Set(ByVal value As String)
            _maxDate = value
        End Set
    End Property

    Private _buttonImage As String = ""

    ''' <summary>
    ''' The URL for the popup button image. If set, buttonText becomes the alt value 
    ''' and is not directly displayed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The URL for the popup button image. If set, buttonText becomes the alt value    and is not directly displayed.")> _
    Public Property ButtonImage() As String
        Get
            Return _buttonImage
        End Get
        Set(ByVal value As String)
            _buttonImage = value
        End Set
    End Property

    Private _ShowButtonImage As Boolean = False

    ''' <summary>
    ''' Shows the default Calendar icon and allows both the textbox and the icon to open the 
    ''' calendar control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Shows the default Calendar icon and allows both the textbox and the icon to open the    calendar control")> _
    Public Property ShowDefaultButtonImage() As Boolean
        Get
            Return _ShowButtonImage
        End Get
        Set(ByVal value As Boolean)
            _ShowButtonImage = value
        End Set
    End Property

    Private _buttonText As String = "..."

    ''' <summary>
    ''' The text to display on the trigger button. Use in conjunction 
    ''' with showOn equal to 'button' or 'both'.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The text to display on the trigger button. Use in conjunction    with showOn equal to 'button' or 'both'.")> _
    Public Property ButtonText() As String
        Get
            Return _buttonText
        End Get
        Set(ByVal value As String)
            _buttonText = value
        End Set
    End Property

    Private _buttonImageOnly As Boolean = False

    ''' <summary>
    ''' Set to true to place an image after the field to use as the trigger 
    ''' without it appearing on a button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Set to true to place an image after the field to use as the trigger    without it appearing on a button.")> _
    Public Property ButtonImageOnly() As Boolean
        Get
            Return _buttonImageOnly
        End Get
        Set(ByVal value As Boolean)
            _buttonImageOnly = value
        End Set
    End Property

    Private _showon As showElement = showElement.textbox

    ''' <summary>
    ''' Have the datepicker appear automatically when the field receives focus ('textbox'), 
    ''' appear only when a button is clicked ('button'), or appear when either event takes 
    ''' place ('both').
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Have the datepicker appear automatically when the field receives focus ('textbox'),    appear only when a button is clicked ('button'), or appear when either event takes    place ('both').")> _
    Public Property ShowOn() As showElement
        Get
            Return _showon
        End Get
        Set(ByVal value As showElement)
            _showon = value
        End Set
    End Property

    Private _showAnim As animations = animations.Show

    ''' <summary>
    ''' Set the name of the animation used to show/hide the datepicker. 
    ''' Use 'show' (the default), 'slideDown', 'fadeIn', any of the show/hide 
    ''' jQuery UI effects, or '' for no animation.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Set the name of the animation used to show/hide the datepicker.    Use 'show' (the default), 'slideDown', 'fadeIn', any of the show/hide    jQuery UI effects, or '' for no animation.")> _
    Public Property ShowAnimation() As animations
        Get
            Return _showAnim
        End Get
        Set(ByVal value As animations)
            _showAnim = value
        End Set
    End Property

    Private _showOtherMonths As Boolean = False

    ''' <summary>
    ''' Display dates in other months (non-selectable) at the start or end of the 
    ''' current month. To make these days selectable use selectOtherMonths.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Display dates in other months (non-selectable) at the start or end of the    current month. To make these days selectable use selectOtherMonths.")> _
    Public Property ShowOtherMonths() As Boolean
        Get
            Return _showOtherMonths
        End Get
        Set(ByVal value As Boolean)
            _showOtherMonths = value
        End Set
    End Property

    Private _selectOtherMonths As Boolean = False

    ''' <summary>
    ''' When true days in other months shown before or after the current 
    ''' month are selectable. This only applies if showOtherMonths is also true.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("When true days in other months shown before or after the current    month are selectable. This only applies if showOtherMonths is also true.")> _
    Public Property SelectOtherMonths() As Boolean
        Get
            Return _selectOtherMonths
        End Get
        Set(ByVal value As Boolean)
            _selectOtherMonths = value
        End Set
    End Property

    Private _yearRange As String = ""

    ''' <summary>
    ''' Control the range of years displayed in the year drop-down: either relative 
    ''' to today's year (-nn:+nn), relative to the currently selected year (c-nn:c+nn), 
    ''' absolute (nnnn:nnnn), or combinations of these formats (nnnn:-nn). Note that 
    ''' this option only affects what appears in the drop-down, to restrict which dates 
    ''' may be selected use the minDate and/or maxDate options.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Control the range of years displayed in the year drop-down: either relative    to today's year (-nn:+nn), relative to the currently selected year (c-nn:c+nn),    absolute (nnnn:nnnn), or combinations of these formats (nnnn:-nn). Note that    this option only affects what appears in the drop-down, to restrict which dates    may be selected use the minDate and/or maxDate options.")> _
    Public Property YearRange() As String
        Get
            Return _yearRange
        End Get
        Set(ByVal value As String)
            _yearRange = value
        End Set
    End Property

    Private _showButtonPanel As Boolean = False

    ''' <summary>
    ''' Whether to show the button panel.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Whether to show the button panel.")> _
    Public Property ShowButtonPanel() As Boolean
        Get
            Return _showButtonPanel
        End Get
        Set(ByVal value As Boolean)
            _showButtonPanel = value
        End Set
    End Property

    Private _closeText As String = ""

    ''' <summary>
    ''' The text to display for the close link. 
    ''' Use the showButtonPanel to display this button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The text to display for the close link.    Use the showButtonPanel to display this button.")> _
    Public Property CloseText() As String
        Get
            Return _closeText
        End Get
        Set(ByVal value As String)
            _closeText = value
        End Set
    End Property

    Private _date As DateTime = Nothing
    Public Property SelectedDate() As DateTime
        Get
            Try
                _date = Date.Parse(Me.Text)
            Catch ex As Exception
            End Try
            Return _date
        End Get
        Set(ByVal value As DateTime)
            Try
                _date = Date.Parse(value.ToString).ToShortDateString
            Catch ex As Exception
            End Try
        End Set
    End Property

    Public Property value() As DateTime
        Get
            Return SelectedDate
        End Get
        Set(ByVal valuein As DateTime)
            SelectedDate = valuein
        End Set
    End Property

#End Region

    Public Enum showElement
        textbox
        button
        both
    End Enum

    Public Enum animations
        Show
        Blind
        Clip
        Drop
        Explode
        Fade
        Fold
        Puff
        Slide
        Scale
    End Enum

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
        If (ButtonImageOnly AndAlso ButtonImage = "") OrElse (ShowDefaultButtonImage AndAlso ButtonImage = "") Then
            ButtonImage = BaseClasses.Scripts.ScriptsURL & "JqueryUIControls/calendar.gif"
        End If
        If ButtonImage <> "" AndAlso ButtonImageOnly AndAlso ShowOn = showElement.textbox Then
            ShowOn = showElement.button
        End If
        If ButtonImage <> "" AndAlso ShowDefaultButtonImage Then
            ShowOn = showElement.both
            ButtonImageOnly = True
        End If

        If ChangeMonth Then
            outstr &= "changeMonth: true,"
        End If
        If ChangeYear Then
            outstr &= "changeYear: true,"
        End If
        If ShowOn <> showElement.textbox Then
            outstr &= "showOn: '" & getEnumName(ShowOn) & "',"
        End If
        If ButtonImageOnly Then
            outstr &= "buttonImageOnly: true,"
        End If
        If ButtonImage <> "" Then
            outstr &= "buttonImage: '" & ButtonImage & "',"
        End If
        If ButtonText <> "..." Then
            outstr &= "buttonText:'" & ButtonText & "',"
        End If
        If ShowOtherMonths Then
            outstr &= "showOtherMonths:true,"
            If SelectOtherMonths Then
                outstr &= "selectOtherMonths:true,"
            End If
        End If
        If ShowButtonPanel Then
            outstr &= "showButtonPanel:true,"
            If CloseText <> "" Then
                outstr &= "closeText:'" & CloseText & "',"
            End If
        End If
        If MaxDate <> "" Then
            Dim i As Integer = 0
            Integer.TryParse(MaxDate, i)
            If i <> 0 Then
                outstr &= "maxDate:" & MaxDate & ","
            Else
                outstr &= "maxDate:'" & MaxDate & "',"
            End If
        End If
        If MinDate <> "" Then
            Dim i As Integer = 0
            Integer.TryParse(MinDate, i)
            If i <> 0 Then
                outstr &= "minDate:" & MinDate & ","
            Else
                outstr &= "minDate:'" & MinDate & "',"
            End If
        End If
        If ShowAnimation <> animations.Show Then
            outstr = "showAnim:" & getEnumName(ShowAnimation) & ","
        End If
        If YearRange <> "" Then
            outstr &= "yearRange:'" & YearRange & "',"
        End If
        outstr = outstr.TrimEnd(",")
        If outstr <> "" Then
            outstr = "{" & outstr & "}"
        End If
        Return outstr
    End Function

    Private Sub DatePicker_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If SelectedDate <> Nothing Then
            Me.Text = SelectedDate.ToShortDateString
        End If
        Dim id As String = "" & Me.ID
        If id = "" Then
            id = ClientID
        End If
		Dim s As String = ""
		s &= "window." & id & " = $('#" & Me.ClientID & "').datepicker("
		s &= renderparams()
		s &= "      );"
		jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, s)
	End Sub

    Public Shared Function getEnumName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function
End Class
