Public Partial Class AutoCompleteTest
    Inherits basepage

    Private Property dt() As DataTable
        Get
            If Session("datasource") Is Nothing Then
                Session("datasource") = New DataTable
            End If
            Return Session("datasource")
        End Get
        Set(ByVal value As DataTable)
            Session("datasource") = value
        End Set
    End Property

    Private Property dtBuilt() As Boolean
        Get
            If Session("datasouceBuilded") Is Nothing Then
                Session("datasouceBuilded") = False
            End If
            Return Session("datasouceBuilded")
        End Get
        Set(ByVal value As Boolean)
            Session("datasouceBuilded") = value
        End Set
    End Property

    Private Sub builddt()
        If Not dtBuilt Then
            dt.Columns.Add(New DataColumn("Id", GetType(Integer)))
            dt.Columns.Add(New DataColumn("Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Value", GetType(Integer)))
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
            dt.Columns(0).AutoIncrement = True
            dt.Columns(0).AutoIncrementSeed = 1
            dt.Columns(0).AutoIncrementStep = 1

            dt.Rows.Add(New Object() {0, "Tres", 645})
            dt.Rows.Add(New Object() {1, "<b>Mic</b>", 4856})
            dt.Rows.Add(New Object() {2, "Ryan", 3777})
            dt.Rows.Add(New Object() {3, "Neil", 7702})
            dtBuilt = True
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim dt As New DataTable("NameTable")
        'dt.Columns.Add("ID", GetType(Integer))
        'dt.Columns(0).AutoIncrement = True
        'dt.Columns(0).AutoIncrementSeed = -1
        'dt.Columns(0).AutoIncrementStep = -1
        'dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
        'dt.Columns.Add("name", GetType(String))

        'acDynamic.setDistinctAutocomplete("YHOPOP", "HHALIAS")

        Dim conn As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("Demographics").ConnectionString)
        AutoComplete1.setSelectAutocomplete("select top 20 * from ( " & vbCrLf & _
            "	select Last_Name + ', ' + First_Name + ' ' + Middle_Name + ' (' + SSN + ')' as name, " & vbCrLf & _
            "	ClientID from dbo.Client " & vbCrLf & _
            ") as a " & vbCrLf & _
            "where name like @name order by name ", "name", "clientID", , conn)

        'acDynamic.setSelectAutocomplete("select top 20 * from (SELECT top 100 percent HHFIRST + ' ' + HHLAST + ' (' + convert(varchar,HHFTRKEY) + ')' as name, HHFTRKEY from YHOPOP order by HHLAST ) as youthNames where name like @name", _
        '                                "name", "HHFTRKEY")

        DropDownList1.Items.Add(New ListItem("dynamic1", "dynVal1"))
        DropDownList1.Items.Add(New ListItem("dynamic2", "dynVal2"))
        If Not dtBuilt Then
            '    builddt()
        End If
        jQueryLibrary.ThemeAdder.AddTheme(Me)
        'Me.AutoComplete1.addControlsToWatchList(Me.TextBox1, Me.DropDownList1)
        Me.jsfunc1.addControlsToWatchList(Me.TextBox1, Me.DropDownList1, AutoComplete1)
    End Sub

    Private Sub Autocomplete1_Search(ByVal sender As JqueryUIControls.Autocomplete, ByVal query As String) Handles Autocomplete1.Search
        Dim dv As New DataView(dt, "Name like '%" & query & "%'", "", DataViewRowState.CurrentRows)
        sender.respond(dv, "Name", "Id")
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s As String = AutoComplete1.Value
    End Sub

    Private Sub acDynamic_Search(ByVal sender As JqueryUIControls.Autocomplete, ByVal query As String) Handles acDynamic.Search
        builddt()
        Dim dv As New DataView(dt, "name like '%" & query & "%'", "", DataViewRowState.CurrentRows)
        acDynamic.respond(dv, "Name")
    End Sub
End Class