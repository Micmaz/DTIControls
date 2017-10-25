Public Partial Class testPage
    Inherits System.Web.UI.Page

    Public ReadOnly Property buttons() As List(Of Button)
        Get
            If Session("buttons") Is Nothing Then Session("buttons") = New List(Of Button)
            Return Session("buttons")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.ThemeAdder.AddTheme(Me)
        'BaseClasses.DataBase.setCssClasstoId(Me)
        For Each newbtn As Button In buttons
            Panel1.Controls.Add(newbtn)
            AddHandler newbtn.Click, AddressOf dynamicBtn
        Next

    End Sub

    Shared buttonID As Integer = 0
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim newbtn As New Button
        newbtn.Text = "I'm a new button " & buttons.Count
        newbtn.ID = buttonID
        buttonID += 1
        buttons.Add(newbtn)
        Panel1.Controls.Add(newbtn)
    End Sub

    Private Sub dynamicBtn(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        buttons.Remove(btn)
        btn.Parent.Controls.Remove(btn)
    End Sub

    Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        If Not DropDownList1.SelectedValue = "" Then
            DropDownList2.Items.Clear()
            DropDownList2.Visible = True
            DropDownList2.Items.Add(DropDownList1.SelectedValue & " subitem: 1")
            DropDownList2.Items.Add(DropDownList1.SelectedValue & " subitem: 2")
        Else
            DropDownList2.Visible = False
        End If
    End Sub

    Private Sub DropDownList2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList2.TextChanged
        If DropDownList2.SelectedValue.EndsWith(" subitem: 2") Then
            Panel1.Controls.Add(New LiteralControl("<script type='text/javascript'>jqalert('Dynamic Alert!')</script>"))
        End If
    End Sub
End Class