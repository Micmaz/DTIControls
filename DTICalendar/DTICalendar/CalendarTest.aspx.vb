Public Partial Class CalendarTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub DTICalendar1_DateRangeChanged(ByVal starttime As Date, ByVal endtime As Date) Handles DTICalendar1.DateRangeChanged
        DTICalendar1.AsyncText = "[{""id"":111,""title"":""Event1"",""start"":""2010-01-10""},{""id"":222,""title"":""Event2"",""start"":""2010-01-20"",""end"":""2010-01-22"",""description"":""event 222"",""where"":""my house""}]"
        DTICalendar1.EndAction()
    End Sub
End Class