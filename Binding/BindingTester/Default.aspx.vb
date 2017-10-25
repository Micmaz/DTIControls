Imports Binding
Public Class _Default
    Inherits BaseClasses.BaseSecurityPage

    Private row As DataRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As DataTable = sqlHelper.FillDataTable("select * from newsItem where id = @id", 3)
        row = dt(0)
        Me.autoBind(row)

        Dim dt2 As DataTable = sqlHelper.FillDataTable("select * from newsItem where id <> @id", 3)
        For Each r As DataRow In dt2.Rows
            Dim c As TestCtrl1 = Page.LoadControl("TestCtrl1.ascx")
            PlaceHolder1.Controls.Add(c)
            c.autoBind(r)

            'Dim lbl As New Label
            'Dim tb As New TextBox
            'PlaceHolder1.Controls.Add(lbl)
            'PlaceHolder1.Controls.Add(tb)
            'lbl.setText(r, "id")
            'tb.setText(r, "Title")

        Next

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'setRowValues(row)
        'sqlHelper.Update(row.Table)
        Me.saveAllRows(True)
    End Sub
End Class