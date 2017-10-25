Imports DTIMediaManager
Imports DTIImageManager
Imports DTIVideoManager

#If DEBUG Then
Partial Public Class DeleteMedia
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class DeleteMedia
        Inherits BaseClasses.BaseSecurityPage
#End If

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim media_id As Integer = Request.QueryString("i")
            Dim mediaTable As New dsMedia.DTIMediaManagerDataTable
            sqlHelper.SafeFillTable("select * from DTIMediaManager where Id = @mid", mediaTable, New Object() {media_id})
            If mediaTable.Count > 0 Then
                Dim mediaRow As dsMedia.DTIMediaManagerRow = mediaTable(0)
                If mediaRow.Content_Type = "Image" Then
                    sqlHelper.SafeExecuteNonQuery("delete from DTIImageManager where Id = " & mediaRow.Content_Id)
                ElseIf mediaRow.Content_Type = "Video" Then
                    sqlHelper.SafeExecuteNonQuery("delete from DTIVideoManager where Id = " & mediaRow.Content_Id)
                End If
                mediaRow.Delete()
                sqlHelper.Update(mediaTable)
            End If

        End Sub

    End Class