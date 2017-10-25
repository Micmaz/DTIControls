Public Partial Class BarChartTest
    Inherits System.Web.UI.Page

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
        dt.Columns.Add(New DataColumn("Id", GetType(Integer)))
        dt.Columns.Add(New DataColumn("Name", GetType(String)))
        dt.Columns.Add(New DataColumn("Value", GetType(Integer)))
        dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
        dt.Columns(0).AutoIncrement = True
        dt.Columns(0).AutoIncrementSeed = 1
        dt.Columns(0).AutoIncrementStep = 1

        dt.Rows.Add(New Object() {0, "Tres", 645})
        dt.Rows.Add(New Object() {1, "Mic", 4856})
        dt.Rows.Add(New Object() {2, "Ryan", 3777})
        dt.Rows.Add(New Object() {3, "Neil", 7702})
        dtBuilt = True
    End Sub

    Private Sub BarChartTest_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not dtBuilt Then
            builddt()
        End If
        With PollChart1
            .DataSource = dt
            .DataTextField = "Name"
            .DataValueField = "Value"
            .DataBind()
        End With
    End Sub

End Class