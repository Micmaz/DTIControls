Partial Public Class autoCompleteTest
    Inherits BaseClasses.BaseSecurityPage

    Shared hasinit As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        doinit()

    End Sub

    Private Sub AutocompleteDropDown1_search(ByVal query As String) Handles AutocompleteDropDown1.search
        Dim dt As New dsTest.NamesDataTable
        'sqlHelper.FillDataTable("select * from names where fname like @name order by fname desc", dt, query & "%")
        sqlHelper.FillDataTable("select top 3 fname||' '||lname||id as name,id from names where fname like @name order by fname desc", dt, query & "%")
        AutocompleteDropDown1.respond(dt, "name", "id")
    End Sub

    Private Sub AutocompleteDropDown2_search(ByVal query As String) Handles AutocompleteDropDown2.search
        Dim dt As New dsTest.NamesDataTable
        sqlHelper.FillDataTable("select Phone,id from names where Phone like @Phone", dt, query & "%")
        AutocompleteDropDown1.respond(dt, "Phone", "id")
    End Sub

    Private Sub doinit()
        If Not hasinit Then
            Dim ds As New dsTest
            'If sqlHelper.checkAndCreateTable(ds.Names) Then
            ds.Names.AddNamesRow("Bob", "Smith", "111-111-1111")
            ds.Names.AddNamesRow("John", "Ashly", "121-111-1111")
            ds.Names.AddNamesRow("Mike", "Kent", "131-111-1111")
            ds.Names.AddNamesRow("Mary", "Shoe", "141-111-1111")
            ds.Names.AddNamesRow("Sue", "Book", "151-111-1111")
            ds.Names.AddNamesRow("Cathy", "Hand", "161-111-1111")
            ds.Names.AddNamesRow("Jill", "Kirk", "171-111-1111")
            ds.Names.AddNamesRow("Kate", "Jones", "181-111-1111")
            ds.Names.AddNamesRow("Ringo", "Star", "191-111-1111")
            'End If
            sqlHelper.checkAndCreateAllTables(ds)
            sqlHelper.Update(ds.Names)
        End If
    End Sub

End Class