Public Class ImageSelectorDlg
    Inherits BaseClasses.BaseSecurityPage

    Public WithEvents ph1 As PlaceHolder
    Public WithEvents ucImageSelector1 As ucImageSelector
    Public WithEvents HiddenField1 As Global.System.Web.UI.WebControls.HiddenField

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ucImageSelector1 = Me.Page.LoadControl("~/res/DTIContentManagement/ucImageSelector.ascx")
        ucImageSelector1.HideButton = True
        ucImageSelector1.UploadOnly = True
        ph1.Controls.Add(ucImageSelector1)

        jQueryLibrary.ThemeAdder.AddThemeToIframe(Me)
        Dim cat As String = Request.QueryString("cat")
        Dim func As String = Request.QueryString("func")
        Dim i As String = Request.QueryString("if")
        Dim sel() As String = New String() {}
        If Not String.IsNullOrEmpty(Request.QueryString("sel")) Then
            sel = Request.QueryString("sel").Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        End If
        Dim vis() As String = New String() {}
        If Not String.IsNullOrEmpty(Request.QueryString("vis")) Then
            vis = Request.QueryString("vis").Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        End If

        Dim mulit As Boolean = False
        Try
            mulit = Boolean.Parse(Request.QueryString("multi"))
        Catch ex As Exception

        End Try

        If i = "" Then
            i = "parent"
        Else
            i = "parent.frames['" & i & "']"
        End If

        ucImageSelector1.SelectedImageIDs.Clear()
        ucImageSelector1.VisibleCategories.Clear()
        For Each s As String In sel
            ucImageSelector1.SelectedImageIDs.Add(s)
        Next
        For Each v As String In vis
            ucImageSelector1.VisibleCategories.Add(v)
        Next

        ucImageSelector1.Category = cat
        ucImageSelector1.MultiSelect = mulit
        If Not func Is Nothing Then _
        ucImageSelector1.onSelectCallback = "imgSelected(imageid, " & i & "." & func & ", name)"
    End Sub

    Private Sub ImageSelector_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

    End Sub
End Class