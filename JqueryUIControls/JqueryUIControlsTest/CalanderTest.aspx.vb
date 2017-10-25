Public Partial Class CalanderTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Private Sub cal1_fetchEvents(ByVal sender As JqueryUIControls.Calendar, ByVal startDate As Date, ByVal endDate As Date) Handles cal1.fetchEvents
        If Date.Now.AddDays(3) > startDate AndAlso Date.Now.AddDays(3) < endDate Then
            cal1.eventClick = "alert('yes');"

            'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test", startDate, , , , , True, , "#FF0000"))
            'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test2", startDate.AddHours(12), startDate.AddHours(13), , , , False, , "#00FFFF"))
            'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test2", endDate.AddHours(-5), endDate.AddHours(-2), , , , True, , "#00FF00"))
            'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test3", startDate.AddHours(4), startDate.AddHours(6), , , , True, , "#0000FF"))
            startDate = Date.Now.AddDays(3)
            'startDate = startDate.AddHours(24)
            'Threading.Thread.CurrentThread.Sleep("5000")
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test1", startDate, , , , , , "www.google.com"))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test2", startDate))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test3", startDate))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test4", startDate))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test5", startDate))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test6", startDate))
            sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test1", startDate, , , , , , "www.yahoo.com"))
        End If

        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test2", startDate.AddHours(12), startDate.AddHours(13), , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test3", startDate.AddHours(-5), endDate.AddHours(-2), , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test4", startDate.AddHours(4), startDate.AddHours(6), , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test5", startDate, , , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test6", startDate.AddHours(12), startDate.AddHours(13), , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test7", startDate.AddHours(-5), endDate.AddHours(-2), , , , , , ))
        'sender.eventList.Add(New JqueryUIControls.Calendar.CalendarEvent("test8", startDate.AddHours(4), startDate.AddHours(6), , , , , , ))


    End Sub

End Class