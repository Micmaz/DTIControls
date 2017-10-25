Partial Public Class DivRotatorTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.DivRotator1.Container = Me.Panel1
        Me.DivRotator2.Container = Me.Panel2
    End Sub

End Class