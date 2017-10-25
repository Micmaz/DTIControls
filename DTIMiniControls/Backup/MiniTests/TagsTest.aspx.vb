Public Partial Class TagsTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim pops As New List(Of String)
        'pops.Add("Mike")
        'pops.Add("Don")
        'pops.Add("Leo")
        'pops.Add("Raph")
        'Tagger1.popularTagsSet = pops

        'Dim curr As New List(Of String)
        'curr.Add("Digital")
        'curr.Add("Tadpole")
        'Tagger1.currentTagsList = curr
    End Sub

    Private Sub Tagger1_CurrentTagsChanged() Handles Tagger1.CurrentTagsChanged
        Dim i = 0
    End Sub
End Class