Public Class Calendar
    Inherits Panel

#Region "Properties"


    Private SelectedDateValue As Date = Date.Today
    Public Property SelectedDate() As Date
        Get
            Return SelectedDateValue
        End Get
        Set(ByVal value As Date)
            SelectedDateValue = value
        End Set
    End Property

    Private themeValue As Boolean = True
    Public Property theme() As Boolean
        Get
            Return themeValue
        End Get
        Set(ByVal value As Boolean)
            themeValue = value
        End Set
    End Property

    Private editableValue As Boolean = True
    Public Property editable() As Boolean
        Get
            Return editableValue
        End Get
        Set(ByVal value As Boolean)
            editableValue = value
        End Set
    End Property

    Private selectableValue As Boolean = True
    Public Property selectable() As Boolean
        Get
            Return selectableValue
        End Get
        Set(ByVal value As Boolean)
            selectableValue = value
        End Set
    End Property

    Private eventListValue As New List(Of CalendarEvent)
    Public Property eventList() As List(Of CalendarEvent)
        Get
            Return eventListValue
        End Get
        Set(ByVal value As List(Of CalendarEvent))
            eventListValue = value
        End Set
    End Property


    Private showWeekendsValue As Boolean = True
    Public Property showWeekends() As Boolean
        Get
            Return showWeekendsValue
        End Get
        Set(ByVal value As Boolean)
            showWeekendsValue = value
        End Set
    End Property


    Private headerLeftValue As String = "prev,next today"
    Public Property headerLeft() As String
        Get
            Return headerLeftValue
        End Get
        Set(ByVal value As String)
            headerLeftValue = value
        End Set
    End Property


    Private headerRightValue As String = "month,agendaWeek,agendaDay"
    Public Property headerRight() As String
        Get
            Return headerRightValue
        End Get
        Set(ByVal value As String)
            headerRightValue = value
        End Set
    End Property


    Private headerCenterValue As String = "title"
    Public Property headerCenter() As String
        Get
            Return headerCenterValue
        End Get
        Set(ByVal value As String)
            headerCenterValue = value
        End Set
    End Property

    Private maxTimeValue As Integer = -1
    Public Property maxTime() As Integer
        Get
            Return maxTimeValue
        End Get
        Set(ByVal value As Integer)
            maxTimeValue = value
        End Set
    End Property

    Private minTimeValue As Integer = -1
    Public Property minTime() As Integer
        Get
            Return minTimeValue
        End Get
        Set(ByVal value As Integer)
            minTimeValue = value
        End Set
    End Property

    Private firstHourValue As Integer = -1
    Public Property firstHour() As Integer
        Get
            Return firstHourValue
        End Get
        Set(ByVal value As Integer)
            firstHourValue = value
        End Set
    End Property

    Private slotMinutesValue As Integer = -1
    Public Property slotMinutes() As Integer
        Get
            Return slotMinutesValue
        End Get
        Set(ByVal value As Integer)
            slotMinutesValue = value
        End Set
    End Property

    Private DefaultViewValue As view = view.month
    Public Property DefaultView() As view
        Get
            Return DefaultViewValue
        End Get
        Set(ByVal value As view)
            DefaultViewValue = value
        End Set
    End Property


    Private eventClickValue As String

    ''' <summary>
    ''' js function name for when an event is clicked.
    ''' function should be in the format:
    ''' functionName( event, jsEvent, view ) { }
    ''' property string is just the function name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("js function name for when an event is clicked.   function should be in the format:   functionName( event, jsEvent, view ) { }   property string is just the function name.")> _
    Public Property eventClick() As String
        Get
            Return eventClickValue
        End Get
        Set(ByVal value As String)
            eventClickValue = value
        End Set
    End Property



    Private dayClickValue As String

    ''' <summary>
    ''' js function name for when a day is clicked.
    ''' function should be in the format:
    ''' functionName( date, allDay, jsEvent, view ) { }
    ''' property string is just the function name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("js function name for when a day is clicked.   function should be in the format:   functionName( date, allDay, jsEvent, view ) { }   property string is just the function name.")> _
    Public Property dayClick() As String
        Get
            Return dayClickValue
        End Get
        Set(ByVal value As String)
            dayClickValue = value
        End Set
    End Property



    Private eventMouseoverValue As String

    ''' <summary>
    ''' js function name for when an event is moused over.
    ''' function should be in the format:
    ''' functionName( event, jsEvent, view ) { }
    ''' property string is just the function name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("js function name for when an event is moused over.   function should be in the format:   functionName( event, jsEvent, view ) { }   property string is just the function name.")> _
    Public Property eventMouseover() As String
        Get
            Return eventMouseoverValue
        End Get
        Set(ByVal value As String)
            eventMouseoverValue = value
        End Set
    End Property


    Private eventMouseoutValue As String

    ''' <summary>
    ''' js function name for when an event is moused out.
    ''' function should be in the format:
    ''' functionName( event, jsEvent, view ) { }
    ''' property string is just the function name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("js function name for when an event is moused out.   function should be in the format:   functionName( event, jsEvent, view ) { }   property string is just the function name.")> _
    Public Property eventMouseout() As String
        Get
            Return eventMouseoutValue
        End Get
        Set(ByVal value As String)
            eventMouseoutValue = value
        End Set
    End Property


    Private MonthLimitValue As Integer = -1
    Public Property MonthViewLimit() As Integer
        Get
            Return MonthLimitValue
        End Get
        Set(ByVal value As Integer)
            MonthLimitValue = value
        End Set
    End Property


    Private ajaxLoadValue As Boolean = False
    Public Property ajaxLoad() As Boolean
        Get
            Return ajaxLoadValue
        End Get
        Set(ByVal value As Boolean)
            ajaxLoadValue = value
        End Set
    End Property



    Public Enum view
        month
        basicWeek
        basicDay
        agendaWeek
        agendaDay
    End Enum

#End Region

    Event fetchEvents(ByVal sender As Calendar, ByVal startDate As Date, ByVal endDate As Date)
    Friend WithEvents ajaxLoader As AjaxCall

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/fullcalendar.min.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/FreezeIt.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/fullcalendar.css")

            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/viewmore.css")
            'jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.ui.tooltip.css")
            'jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.ui.tooltip.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/fullcalendar.viewmore.js")
            JqueryUIControls.ToolTip.registerControl(page)
        End If
    End Sub

    Private Sub ajaxLoader_callBack(ByVal sender As AjaxCall, ByVal value As String) Handles ajaxLoader.callBack
        Dim startDate As Date = dateFromUnixTime(Page.Request.QueryString("start"))
        Dim endDate As Date = dateFromUnixTime(Page.Request.QueryString("end"))
        RaiseEvent fetchEvents(Me, startDate, endDate)
        'Page.Response.Clear()
        Page.Response.Write(renderEvents(Me.eventList, True))
        Page.Response.End()
    End Sub

    Public Shared Function dateFromUnixTime(ByVal input As Long) As Date
        Return New DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(input).ToLocalTime()
    End Function

    Private Sub control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
        If ajaxLoad Then
            ajaxLoader = New AjaxCall
            ajaxLoader.ID = Me.ID & "_ajax"
            Me.Controls.Add(ajaxLoader)
        End If
        'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "JqueryUIControls/fullcalendar.print.css")
    End Sub

    Private Function renderEvents() As String
        If ajaxLoad Then
            Return "events: """ & ajaxLoader.postURL & """, loading: function(bool) {if (bool){FreezeIt($(this).attr('id')); }else{unFreezeIt($(this).attr('id'));}} "
        End If
        Return "events: " & renderEvents(Me.eventList)
    End Function

    Public Shared Function renderEvents(ByVal l As List(Of CalendarEvent), Optional ByVal asJson As Boolean = False) As String
        Dim outstr As String = "["
        For Each c As CalendarEvent In l
            If asJson Then
                outstr &= vbCrLf & c.toJsonString & ","
            Else
                outstr &= vbCrLf & c.toString & ","
            End If
        Next
        Return outstr.Trim(",") & "]"
    End Function

    Private Function renderparams() As String
        Dim outstr As String = ""
        outstr &= jsPropString("theme", theme)
        outstr &= jsPropString("editable", editable)
        outstr &= jsPropString("selectable", selectable)
        outstr &= jsPropString("weekends", showWeekends)
        outstr &= jsPropString("defaultView", [Enum].GetName(GetType(view), DefaultView))
        outstr &= "header: {left: '" & headerLeft & "',center: '" & headerCenter & "',right: '" & headerRight & "'},"
        outstr &= jsPropFunctionString("eventClick", eventClick)
        outstr &= jsPropFunctionString("dayClick", dayClick)
        outstr &= jsPropFunctionString("eventMouseover", eventMouseover)
        outstr &= jsPropFunctionString("eventMouseout", eventMouseout)
        If minTime > -1 Then outstr &= jsPropFunctionString("minTime", minTime)
        If maxTime > -1 Then outstr &= jsPropFunctionString("maxTime", maxTime)
        If firstHour > -1 Then outstr &= jsPropFunctionString("firstHour", firstHour)
        If slotMinutes > -1 Then outstr &= jsPropFunctionString("slotMinutes", slotMinutes)

        Return outstr
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Not ajaxLoad Then
            Dim firstOfmonth As Date = Me.SelectedDate.AddDays(-1 * Me.SelectedDate.Day + 1)
            RaiseEvent fetchEvents(Me, firstOfmonth, firstOfmonth.AddDays(Date.DaysInMonth(firstOfmonth.Year, firstOfmonth.Month) - 1))
        End If
        Dim str As String = "<script type=""text/javascript"">"
        Dim id As String = "" & Me.ID
        If id = "" Then
            id = ClientID
        End If

        str &= "var " & id & "; $(function(){" & id & " = $('#" & Me.ClientID & "').fullCalendar({" & vbCrLf
        str &= renderparams()
        str &= renderEvents()
        str &= "        })"
        If MonthViewLimit > 0 Then
            str &= ".limitEvents(" & MonthViewLimit & ")"
        End If
        str &= "; });</script>"
        writer.Write(str)

        MyBase.Render(writer)
    End Sub

    Public Class CalendarEvent
        Public id As Integer = -1
        Public title As String = ""
        Public start As DateTime
        Public allDay As Boolean = False
        Public endDate As DateTime
        Public className As String
        Public editable As Boolean
        Public url As String
        Public color As System.Drawing.Color
        Public backgroundColor As System.Drawing.Color
        Public borderColor As System.Drawing.Color
        Public textColor As System.Drawing.Color
        Public otherContent As String


        Public Sub New(ByVal title As String, ByVal start As Date, Optional ByVal endDate As DateTime = Nothing, Optional ByVal id As Integer = -1, Optional ByVal allDay As Boolean = False, Optional ByVal className As String = Nothing, Optional ByVal editable As Boolean = False, Optional ByVal url As String = Nothing, Optional ByVal colorString As String = Nothing, Optional ByVal backgroundColorString As String = Nothing, Optional ByVal borderColorString As String = Nothing, Optional ByVal textColorString As String = Nothing)
            Me.title = title
            Me.start = start
            Me.endDate = endDate
            Me.id = id
            Me.allDay = allDay
            Me.className = className
            Me.editable = editable
            Me.url = url
            If Not colorString Is Nothing Then _
                Me.color = System.Drawing.ColorTranslator.FromHtml(colorString)
            If Not backgroundColorString Is Nothing Then _
                Me.backgroundColor = System.Drawing.ColorTranslator.FromHtml(backgroundColorString)
            If Not borderColorString Is Nothing Then _
                Me.borderColor = System.Drawing.ColorTranslator.FromHtml(borderColorString)
            If Not textColorString Is Nothing Then _
                Me.textColor = System.Drawing.ColorTranslator.FromHtml(textColorString)
        End Sub

        Public Sub New()

        End Sub

        Public Overrides Function toString() As String
            Dim out As String = "{ "
            out &= jsPropString("title", title)
            out &= jsPropString("start", start)
            If id > -1 Then out &= jsPropString("id", id)
            out &= jsPropString("allDay", allDay)
            out &= jsPropString("end", endDate)
            out &= jsPropString("className", className)
            out &= jsPropString("editable", editable)
            out &= jsPropString("url", url)
            out &= jsPropString("color", color)
            out &= jsPropString("backgroundColor", backgroundColor)
            out &= jsPropString("borderColor", borderColor)
            out &= jsPropString("textColor", textColor)
            out &= jsPropString("otherContent", otherContent)
            out = out.Trim(",") & " }"
            Return out
        End Function

        Public Function toJsonString() As String
            Dim out As String = "{ "
            out &= jsonPropString("title", title)
            out &= jsonPropString("start", start)
            out &= jsonPropString("end", endDate)
            If id > -1 Then out &= jsonPropString("id", id)

            out &= jsonPropString("className", className)
            out &= jsonPropString("editable", IIf(editable, "true", ""))
            out &= jsonPropString("allDay", IIf(allDay, "true", ""))
            out &= jsonPropString("url", url)
            out &= jsonPropString("color", color)
            out &= jsonPropString("backgroundColor", backgroundColor)
            out &= jsonPropString("borderColor", borderColor)
            out &= jsonPropString("textColor", textColor)
            out &= jsonPropString("otherContent", otherContent)
            out = out.Trim(",") & " }"
            Return out
        End Function

        Private Function propString(ByVal prop As String, ByVal value As Object) As String
            If value Is Nothing Then Return ""
            Dim out As String = """" & prop & """:"
            If IsNumeric(value) Then
                out &= value
            Else
                out &= """" & value.ToString & """"
            End If
            Return out & ","
        End Function

    End Class




End Class

