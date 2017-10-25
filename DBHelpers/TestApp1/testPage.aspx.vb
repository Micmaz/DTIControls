Public Class testPage
    Inherits BaseClasses.BaseSecurityPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As DataTable = New DataTable
        sqlHelper.FillDataTable("select top 1 * from goal", dt)
        Dim newRow = dt.NewRow()
        newRow("name") = "dasdas"
        dt.Rows.Add(newRow)
        dt.Rows(0)("name") = "dasdas"
        sqlHelper.Update(dt)
    End Sub

End Class