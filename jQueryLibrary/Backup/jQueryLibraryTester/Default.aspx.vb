Partial Public Class _Default
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
        dt.Rows.Add(New Object() {1, "<b>Mic</b>", 4856})
        dt.Rows.Add(New Object() {2, "Ryan", 3777})
        dt.Rows.Add(New Object() {3, "Neil", 7702})
        dtBuilt = True
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'jQueryLibrary.ThemeAdder.AddCustomTheme(Me, "/crap-theme/jquery-ui-1.8.16.custom.css", False, False, False, False)
        PlaceHolder1.Controls.Add(JqueryUIControls.Dialog.CreateDialogueUrl("/Iframe.aspx", _
                            "Iframe", JqueryUIControls.Dialog.DialogOpener.Link, 640, 600, "style='font-size: x-small;'"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'jQueryLibrary.ThemeAdder.AddTheme(Me.Page, , False, False, False, False, , False)
        'jQueryLibrary.ThemeAdder.AddTheme(Me.Page, jQueryLibrary.ThemeAdder.themes.aristo, , , , , , Request.QueryString("e"))
        If Not dtBuilt Then
            builddt()
        End If
        tabs.AddTab("Ars", "/Iframe.aspx", True)
        tabs.AddTab("test", "/ContentFrame2.aspx", True)
        tabs.AddTab("Ars", "/ContentFrame.aspx", True)
        tabs.AddTab("test", "/ContentFrame2.aspx", True)
        tabs.AddTab("Ars", "/ContentFrame.aspx", True)
        tabs.AddTab("test", "/ContentFrame2.aspx", True)
    End Sub

    Private Sub Autocomplete1_Search(ByVal sender As JqueryUIControls.Autocomplete, ByVal query As String) Handles AutoComplete1.Search
        Dim dv As New DataView(dt, "Name like '%" & query & "%'", "", DataViewRowState.CurrentRows)
        sender.respond(dv, "Name", "Id")
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s As String = AutoComplete1.Value
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim i = 0
    End Sub
End Class