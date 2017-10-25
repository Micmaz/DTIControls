Partial Public Class _Default
    Inherits BaseClasses.BaseSecurityPage
    Dim ds As New DataSet1
    Dim r As New Random(4)


#Region "Button handlers"

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        create()
    End Sub

    Protected Sub btnFill_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFill.Click
        fill()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        fill()
        delete()
        sqlupdate()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        fill()
        add()
        sqlupdate()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        fill()
        update()
        sqlupdate()
    End Sub

    Protected Sub btnDrop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDrop.Click
        drop()
    End Sub

    Protected Sub btnDoAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDoAll.Click
        create()
        fill()
        add()
        add()
        add()
        sqlupdate()
        update()
        update()
        delete()
        add()
        delete()
        delete()
        delete()
        delete()
        delete()
        sqlupdate()
        sqlupdate()
        ds = New DataSet1
        fill()
        add()
        add()
        sqlupdate()
        sqlupdate()

        Dim dt As New DataTable
        sqlHelper.FillDataTable("select id from (select top 2 * from (select top 3 * from TestUserProps) as a order by name) as b order by id", dt)
        dt = New DataTable
        sqlHelper.FillDataTable("select id from (select top 2 * from (select TOP 3 * from TestUserProps) as a order by name) as b", dt)
        dt = New DataTable
        Dim typeddt As New DataSet1.TestUserDataTable
        sqlHelper.GetSortedPage(dt, "TestUserProps", 3, 0)
        Dim dt2 As DataTable = sqlHelper.FillDataTable("select * from TestUserProps")
        sqlHelper.FillDataTable("select * from TestUserProps", dt2)
        sqlHelper.FillDataTable("select * from TestUserProps where id > @id", dt2, 2)
        sqlHelper.FillDataTable("select * from TestUser where id > @id", typeddt, 2)

        delete()
        fill()
        drop()
    End Sub

#End Region


#Region "Worker Methods"

    Private Sub create()
        Dim ds1 As New DataSet1
        ds1.TestUser.Columns.Add(New DataColumn("newCol1", GetType(String)))
        ds1.TestUserProps.Columns.Add(New DataColumn("newCol1", GetType(String)))
        If sqlHelper.checkAndCreateAllTables(ds1) Then
            lblOutput.Text &= "Created Datatables<BR>"
        End If
    End Sub

    Private Sub fill()
        ds.Clear()
        ds.AcceptChanges()
        sqlHelper.FillDataSetMultiSelect("select * from testUser; select * from testUserProps; select * from dateTester; select * from baddata", ds, New String() {"testUser", "testUserProps", "dateTester", "baddata"})
        lblOutput.Text &= "Filled dataset.<BR>"
    End Sub

    Private Sub add()

        ds.dateTester.AdddateTesterRow("test1", Date.Now)
        Dim dr As DataSet1.dateTesterRow = ds.dateTester.NewdateTesterRow()
        dr.name = "AAA"
        'ds.dateTester.Rows.Add(dr)
        'ds.dateTester.AdddateTesterRow("test2", Nothing)
        ds.TestUser.AddTestUserRow("random usernoprops:" & r.Next)
        Dim row As DataSet1.TestUserRow = ds.TestUser.NewTestUserRow
        row.name = "random user:" & r.Next
        ds.TestUser.AddTestUserRow(row)
        ds.TestUserProps.AddTestUserPropsRow("prop:" & r.Next, r.Next, row)
        ds.badData.AddbadDataRow(ds.badData.Rows.Count, "badly", "formed")
        lblOutput.Text &= "Added 2 rows<BR>"

    End Sub

    Private Sub delete()
        If ds.TestUser.DefaultView.Count > 0 Then
            ds.TestUser.DefaultView(0).Delete()
            lblOutput.Text &= "Deleted first row<BR>"
        End If

        If ds.badData.Count > 0 Then
            ds.badData(0).Delete()
        End If
    End Sub

    Private Sub update()
        If ds.TestUser.Count > 1 Then
            ds.TestUser(ds.TestUser.Count - 1).name &= "Updated" & r.Next
            For Each rw As DataSet1.TestUserPropsRow In ds.TestUser(ds.TestUser.Count - 1).GetTestUserPropsRows()
                rw.name &= "U"
                rw.val &= "U"
            Next

            ds.TestUser(ds.TestUser.Count - 2).name &= "Updated" & r.Next
            For Each rw As DataSet1.TestUserPropsRow In ds.TestUser(ds.TestUser.Count - 2).GetTestUserPropsRows()
                rw.name &= "U"
                rw.val &= "U"
            Next

            lblOutput.Text &= "updated last 2 rows<BR>"
        End If
        For Each row As DataSet1.badDataRow In ds.badData
            row.Column2 &= " updated"
        Next

    End Sub

    Private Sub drop()
        Try
            For Each table As DataTable In ds.Tables
                sqlHelper.ExecuteNonQuery("drop table " & table.TableName)
            Next

            lblOutput.Text &= "Dropped Tables<BR>"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub sqlupdate()
        sqlHelper.Update(ds.TestUser)
        sqlHelper.Update(ds.TestUserProps)
        sqlHelper.Update(ds.dateTester)
        sqlHelper.Update(ds.badData)


        lblOutput.Text &= "Updated SQL<BR>"
    End Sub



#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
        Me.sqlHelper.createAdaptorsWithoutPrimaryKeys = True
        Dim x As New BaseClasses.SQLHelper(sqlHelper.defaultConnection)
        x.createAdaptorsWithoutPrimaryKeys = True
        'Dim da As System.Data.Common.DbDataAdapter = x.Adaptor("statutes_lookup")
        Dim i As Integer = 0
        i = 1
        Dim conn As System.Data.Common.DbConnection = Data.defaultSqliteConnection
    End Sub

    'Protected Sub btnUpdate0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate0.Click
    '    fill()
    '    add()
    '    update()
    '    delete()
    '    Dim da As New DataSet1TableAdapters.TestUserTableAdapter
    '    da.Update(ds)
    'End Sub
    'Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim x As SomeSillyTestClass = BaseClasses.AssemblyLoader.CreateInstance("SomeSillyTestClass")
    '    sqlHelper.checkAndCreateAllTables(New EDCData.DataSet1)
    'End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dt As New DataTable
        Dim totalpages As Integer = sqlHelper.GetSortedPage(dt, "(select Events.Name as 'Event', Member.Full_Name, Accounts.Name, Bookings.Order_Date, Products.Name as 'Product', Booking_Product_Pivot.Quantity as 'Qty', Booking_Product_Pivot.Total as 'Total', Distributors.Name as 'Distributor', Booking_Product_Pivot.Booking_Product_Pivot_Key, Bookings.Booking_Key as 'Booking' from Booking_Product_Pivot inner join Products on Booking_Product_Pivot.Product_Key = Products.Product_Key inner join Bookings on Bookings.Booking_Key = Booking_Product_Pivot.Booking_Key inner join Accounts on Bookings.Account_Key = Accounts.Account_Key inner join Events on Bookings.Event_Key = Events.Event_Key inner join Member on Bookings.Entered_By_Member_Key = Member.Member_Key inner join Distributors on Bookings.Distributor_Key = Distributors.Distributor_Key)", _
        2, 1, "Booking_Product_Pivot_Key", , "name like @acctname", New Object() {"%Golf%"})
        Dim sqlt As System.Data.SQLite.SQLiteConnection = New System.Data.SQLite.SQLiteConnection("Data Source=BayerAppData.db3;Pooling=true")
        Dim dt1 As New DataTable
        sqlHelper.FillDataTable("Select * from Distributors", dt1, sqlt)
        'Dim sqlt As System.Data.SQLite.SQLiteConnection = New System.Data.SQLite.SQLiteConnection("Data Source=ReallySillyFilenameTest.db3;Pooling=true;FailIfMissing=false;")
        'Dim dt As DataTable = sqlHelper.FillDataTable("select * from locks where id= @id", 1, sqlt)
        'sqlHelper.FillDataTable("select * from locks where id= @id", dt, 1, sqlt)
        'sqlHelper.FillDataTable("select * from locks", dt, sqlt)
        'dt = sqlHelper.FillDataTable("select * from locks", sqlt)
        'BaseClasses.DataBase.defaultConnectionAppWide = sqlt
    End Sub

    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Me.writeexcel(sqlHelper.FillDataTable("select * from testuser"))
    End Sub

    Protected Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim Query As String = "name like @query OR city like @query"
        Dim searchText As String = "%a%"

        Dim dt As New DataTable
        sqlHelper.GetSortedPage(dt, "accounts", 15, tbpage.Text, "Account_Key", "Name", Query, New Object() {searchText, "NA"})

        Dim dv As New DataView(dt)
        If dv.Count > 1 Then
            Dim tab As String = "<table width=""99%"">"
            Dim addQueryString As String = ""
            For Each acc As DataRowView In dv
                tab &= "<tr>"
                tab &= "<td><a href=""" & Request.Url.AbsolutePath & "?" & addQueryString & "acc=" & acc("Account_Key") & """>" & acc("Name") & "</a></td>" & "<td>" & acc("City") & ", " & acc("State") & "</td>"
                tab &= "</tr>"
            Next
            tab &= "</table><br /><br />"
            litAccounts.Text = tab

        End If
    End Sub

    Protected Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim sql1 As New BaseClasses.SQLHelper("Data Source=192.168.1.92;Initial Catalog=Northwind;Persist Security Info=True;User ID=sa;Password=letme1n")
        Dim dt As DataTable = sql1.FillDataTable("select top 10 CategoryName||'aaa'  from dbo.Categories")
        For Each row As DataRow In dt.Rows
            litAccounts.Text &= row(0) & "<br>"
        Next
    End Sub

    Protected Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim ds As New NorthWind
        Dim lite As New SQLiteHelper.SQLiteHelper(tbConnection.Text)
        lite.checkAndCreateAllTables(ds)

    End Sub

    Protected Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim ds As New NorthWind
        Dim lite As New SQLiteHelper.SQLiteHelper(tbConnection.Text)
        Dim start As Date = Date.Now

        ds.Clear()
        Dim query As String = ""
        Dim i As Integer = 0
        Dim tables(ds.Tables.Count) As String
        For Each t As DataTable In ds.Tables
            query &= "select * from [" & t.TableName & "];"
            tables(i) = t.TableName
            i += 1
        Next
        sqlHelper.FillDataSetMultiSelect(query, ds, tables)

        litAccounts.Text = "Select took: " & Date.Now.Subtract(start).Milliseconds & " miliseconds <br>"
        start = Date.Now

        sqlHelper.ExportData(ds, lite, False)
        lite.defaultConnection.Close()

        litAccounts.Text &= "Export took: " & Date.Now.Subtract(start).Milliseconds & " miliseconds"
    End Sub

    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button6.Click
        sqlHelper.createAdaptorsWithoutPrimaryKeys = True
        Dim dt As New DataTable
        sqlHelper.FillDataTable("select * from YHOPOP where HHFTRKEY=46170", dt)
        dt.Rows(0)("HHFIRST") = "CHADWICK1"
        'sqlHelper.Update(dt, , True)
    End Sub

End Class