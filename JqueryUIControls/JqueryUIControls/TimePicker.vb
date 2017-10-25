Public Class TimePicker
    Inherits TextBox

    Private _showTimeAndDate As Boolean
    Public Property ShowTimeAndDate() As Boolean
        Get
            Return _showTimeAndDate
        End Get
        Set(ByVal value As Boolean)
            _showTimeAndDate = value
        End Set
    End Property

    Private _datetime As DateTime = Nothing
    Public Property SelectedDateTime() As DateTime
        Get
            Try
                _datetime = DateTime.Parse(Me.Text)
            Catch ex As Exception
            End Try
            Return _datetime
        End Get
        Set(ByVal value As DateTime)
            _datetime = value
            If Not ShowTimeAndDate Then
                Me.Text = value.ToString("hh:mm tt")
            Else
                Me.Text = value.ToString
            End If
        End Set
    End Property

#Region "Time Properties"
    Private _Clock24Hour As Boolean

    ''' <summary>
    ''' Shows time in 24 hour format
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Shows time in 24 hour format")> _
    Public Property Clock24Hour() As Boolean
        Get
            Return _Clock24Hour
        End Get
        Set(ByVal value As Boolean)
            _Clock24Hour = value
        End Set
    End Property

    Private _timeFormat As String = ""

    ''' <summary>
    ''' Change the display format of the time
    ''' h - Hour with no leading 0
    ''' hh - Hour with leading 0
    ''' m - Minute with no leading 0
    ''' mm - Minute with leading 0
    ''' s - Second with no leading 0
    ''' ss - Second with leading 0
    ''' t - a or p for AM/PM
    ''' T - A or P for AM/PM
    ''' tt - am or pm for AM/PM
    ''' TT - AM or PM for AM/PM
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Change the display format of the time   h - Hour with no leading 0   hh - Hour with leading 0   m - Minute with no leading 0   mm - Minute with leading 0   s - Second with no leading 0   ss - Second with leading 0   t - a or p for AM/PM   T - A or P for AM/PM   tt - am or pm for AM/PM   TT - AM or PM for AM/PM")> _
    Public Property TimeFormat() As String
        Get
            Return _timeFormat
        End Get
        Set(ByVal value As String)
            _timeFormat = value
        End Set
    End Property

    Private _separator As String

    ''' <summary>
    ''' Place holder between date and time, default=" "
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Place holder between date and time, default="" """)> _
    Public Property Separator() As String
        Get
            Return _separator
        End Get
        Set(ByVal value As String)
            _separator = value
        End Set
    End Property

    Private _showHour As Boolean = True

    ''' <summary>
    ''' Show the Hour
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Show the Hour")> _
    Public Property ShowHour() As Boolean
        Get
            Return _showHour
        End Get
        Set(ByVal value As Boolean)
            _showHour = value
        End Set
    End Property

    Private _showMinute As Boolean = True

    ''' <summary>
    ''' Show the minute
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Show the minute")> _
    Public Property ShowMinute() As Boolean
        Get
            Return _showMinute
        End Get
        Set(ByVal value As Boolean)
            _showMinute = value
        End Set
    End Property

    Private _showSecond As Boolean

    ''' <summary>
    ''' Show the Second
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Show the Second")> _
    Public Property ShowSecond() As Boolean
        Get
            Return _showSecond
        End Get
        Set(ByVal value As Boolean)
            _showSecond = value
        End Set
    End Property

    Private _stepHour As Integer = 0

    ''' <summary>
    ''' Increments the hours move by
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Increments the hours move by")> _
    Public Property StepHour() As Integer
        Get
            Return _stepHour
        End Get
        Set(ByVal value As Integer)
            _stepHour = value
        End Set
    End Property

    Private _stepMinute As Integer = 0

    ''' <summary>
    ''' Increments the minutes move by
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Increments the minutes move by")> _
    Public Property StepMinute() As Integer
        Get
            Return _stepMinute
        End Get
        Set(ByVal value As Integer)
            _stepMinute = value
        End Set
    End Property

    Private _stepSecond As Integer = 0

    ''' <summary>
    ''' Increments the seconds move by
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Increments the seconds move by")> _
    Public Property StepSecond() As Integer
        Get
            Return _stepSecond
        End Get
        Set(ByVal value As Integer)
            _stepSecond = value
        End Set
    End Property

    Private _maxHour As Integer = 23
    Public Property MaxHour() As Integer
        Get
            Return _maxHour
        End Get
        Set(ByVal value As Integer)
            _maxHour = value
        End Set
    End Property

    Private _minHour As Integer
    Public Property MinHour() As Integer
        Get
            Return _minHour
        End Get
        Set(ByVal value As Integer)
            _minHour = value
        End Set
    End Property

    Private _maxMinute As Integer = 59
    Public Property MaxMinute() As Integer
        Get
            Return _maxMinute
        End Get
        Set(ByVal value As Integer)
            _maxMinute = value
        End Set
    End Property

    Private _minMinute As Integer
    Public Property MinMinute() As Integer
        Get
            Return _minMinute
        End Get
        Set(ByVal value As Integer)
            _minMinute = value
        End Set
    End Property

    Private _maxSecond As Integer = 59
    Public Property MaxSecond() As Integer
        Get
            Return _maxSecond
        End Get
        Set(ByVal value As Integer)
            _maxSecond = value
        End Set
    End Property

    Private _minSecond As Integer
    Public Property MinSecond() As Integer
        Get
            Return _minSecond
        End Get
        Set(ByVal value As Integer)
            _minSecond = value
        End Set
    End Property

    Public Property SelectedTime() As TimeSpan
        Get
            Return SelectedDateTime.TimeOfDay
        End Get
        Set(ByVal value As TimeSpan)
            Dim dt As DateTime = New DateTime(value.Ticks)
            dt = DateTime.Parse(Now.Date.ToShortDateString & " " & dt.ToLongTimeString)
            SelectedDateTime = dt
        End Set
    End Property

    Private _hourGrid As Integer

    ''' <summary>
    ''' Increments for the Hours to display at the bottom of the slider
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Increments for the Hours to display at the bottom of the slider")> _
    Public Property HourGrid() As Integer
        Get
            Return _hourGrid
        End Get
        Set(ByVal value As Integer)
            _hourGrid = value
        End Set
    End Property

    Private _minuteGrid As Integer

    ''' <summary>
    ''' Increments for the minutes to display at the bottom of the slider
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Increments for the minutes to display at the bottom of the slider")> _
    Public Property MinuteGrid() As Integer
        Get
            Return _minuteGrid
        End Get
        Set(ByVal value As Integer)
            _minuteGrid = value
        End Set
    End Property
#End Region

#Region "Date Picker Properties"
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

    Public Property SelectedDate() As DateTime
        Get
            Return SelectedDateTime.Date
        End Get
        Set(ByVal value As DateTime)
            SelectedDateTime = value
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
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.timepicker.js", , True)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jqueryTimepicker.css", "text/css")
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""

        If Not Clock24Hour Then
            outstr &= "ampm: true,"
        End If
        If TimeFormat <> "" Then
            outstr &= "timeFormat: '" & TimeFormat & "',"
        End If
        If Separator <> "" Then
            outstr &= "separator: '" & Separator & "',"
        End If
        If Not ShowHour Then
            outstr &= "showHour: false,"
        End If
        If Not ShowMinute Then
            outstr &= "showMinute: false,"
        End If
        If ShowSecond Then
            outstr &= "showSecond: true,"
        End If
        If StepHour > 0 Then
            outstr &= "stepHour: " & StepHour & ","
        End If
        If StepMinute > 0 Then
            outstr &= "stepMinute: " & StepMinute & ","
        End If
        If StepSecond > 0 Then
            outstr &= "stepSecond: " & StepSecond & ","
        End If

        If ShowHour AndAlso SelectedTime.Hours <> 0 Then
            outstr &= "hour: " & SelectedTime.Hours & ","
        End If
        If ShowMinute AndAlso SelectedTime.Minutes <> 0 Then
            outstr &= "minute: " & SelectedTime.Minutes & ","
        End If
        If ShowSecond AndAlso SelectedTime.Seconds <> 0 Then
            outstr &= "second: " & SelectedTime.Seconds & ","
        End If
        If HourGrid > 0 Then
            outstr &= "hourGrid: " & HourGrid & ","
        End If
        If MinuteGrid > 0 Then
            outstr &= "minuteGrid: " & MinuteGrid & ","
        End If

        If ShowTimeAndDate Then
            If ButtonImageOnly And ButtonImage = "" Then
                ButtonImage = BaseClasses.Scripts.ScriptsURL & "JqueryUIControls/calendar.gif"
            End If
            If ButtonImage <> "" AndAlso ButtonImageOnly AndAlso ShowOn = showElement.textbox Then
                ShowOn = showElement.button
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
                    outstr &= "maxDate:" & MinDate & ","
                Else
                    outstr &= "maxDate:'" & MinDate & "',"
                End If
            End If
            If ShowAnimation <> animations.Show Then
                outstr = "showAnim:" & getEnumName(ShowAnimation) & ","
            End If
            If YearRange <> "" Then
                outstr &= "yearRange:'" & YearRange & "',"
            End If
        End If

        If outstr <> "" Then
            outstr = "{" & outstr.TrimEnd(",") & "}"
        End If
        Return outstr
    End Function

    Private Sub TimePicker_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim s As String = ""
        s &= "$(function(){"
        If Not ShowTimeAndDate Then
            s &= "     $('#" & Me.ClientID & "').timepicker("
        Else
            s &= "     $('#" & Me.ClientID & "').datetimepicker("
        End If
        s &= renderparams()
        s &= "      );"
        s &= "});"
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub

    Public Shared Function getEnumName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function
End Class
