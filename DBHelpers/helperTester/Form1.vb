Public Class Form1
    'substitute helper class here
    Dim sqlhelper As BaseClasses.BaseHelper

    'Test Data depends on 
    'Table: Test
    'Columns: Id, name, value

    Dim ds As New DataSet1
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mysql "server=10.9.8.95;user id=root; password=trixbox; database=mysql; pooling=false"
        'vistadb "Data Source=C:\\test.vdb3;Password=test"
        sqlhelper = New BaseClasses.SQLHelper(New SqlClient.SqlConnection("Data Source=192.168.1.91;Initial Catalog=TRES_CMS_TEST;Persist Security Info=True;User ID=sa;Password=letme1n"))
        sqlhelper.checkAndCreateAllTables(ds)
        sqlhelper.FillDataSetMultiSelect("select * from testUser; select * from testUserProps; ", ds, New String() {"testUser", "testUserProps"})
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        'ds.TestUserProps.AddTestUserPropsRow("testprop" & ds.TestUserProps.Count,"test", (ds.TestUser.Count)

        'Dim dt As New DataTable
        'sqlhelper.SafeFillTable("select * from test where id > @id", dt, New Object() {1})
        'For Each row As DataRow In dt.Rows
        '    row("name") = "updated" & row("id")
        'Next
        'dt.Rows.Add(New Object() {998, "name1", "val1"})
        'dt.Rows.Add(New Object() {999, "name2", "val2"})
        'dt.Rows(0).Delete()
        'sqlhelper.Update(dt)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dt As New DataTable
        sqlhelper.FillDataTable(TextBox1.Text, dt)
        Me.DataGridView1.DataSource = dt
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        sqlhelper.Update(Me.DataGridView1.DataSource)
    End Sub

    Private Sub btnExe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExe.Click
        Dim ds As New DataSet
        Dim pagecount As Integer = sqlhelper.SafeGetSortedPage("Customers", 2, 1, "CustomerID", , , ds)
        Me.DataGridView1.DataSource = ds.Tables("Customers")
        MsgBox(pagecount)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ds.TestUser.AddTestUserRow("newuser" & ds.TestUser.Count)
        ds.TestUserProps.AddTestUserPropsRow("aprop", ds.TestUserProps.Count, ds.TestUser(ds.TestUser.Count - 1))
        'SqlDataAdapter1.Update(ds.TestUser)
        sqlhelper.Update(ds.TestUser)
        sqlhelper.Update(ds.TestUserProps)

        Me.DataGridView1.DataSource = ds.TestUser
        Me.DataGridView2.DataSource = ds.TestUserProps
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        For Each row As DataRow In ds.TestUserProps
            row.Delete()
        Next
        For Each row As DataRow In ds.TestUser
            row.Delete()
        Next
        sqlhelper.Update(ds.TestUserProps)
        sqlhelper.Update(ds.TestUser)
    End Sub
End Class
