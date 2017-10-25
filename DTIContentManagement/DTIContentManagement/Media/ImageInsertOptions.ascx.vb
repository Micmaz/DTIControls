Imports DTIImageManager.dsImageManager
Imports DTIMediaManager.dsMedia
Imports DTIMediaManager.SharedMediaVariables

#If DEBUG Then
Partial Public Class ImageInsertOptions
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ImageInsertOptions
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private _media_row As DTIMediaManagerRow
        Public Property MediaRow() As DTIMediaManagerRow
            Get
                Return _media_row
            End Get
            Set(ByVal value As DTIMediaManagerRow)
                _media_row = value
            End Set
        End Property

        Public ReadOnly Property ImageID() As Integer
            Get
                Return MediaRow.Content_Id
            End Get
        End Property

        Public ReadOnly Property ImageCaption() As String
            Get
                If Not MediaRow.IsDescriptionNull Then
                    Return MediaRow.Description
                Else : Return ""
                End If
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAdminPanel/iframe-default.css", "text/css")
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim imgPreview As String = "<img src=""~/res/DTIImageManager/ViewImage.aspx?Id=" & ImageID & "&maxWidth=200"" />"
            hlInsertImage.Attributes.Add("onclick", "insertMedia('" & imgPreview & "'); return false;")

            hlInsertImageThumb.Attributes.Add("onclick", "insertMedia('" & _
                DTIImageManager.ViewImage.getZoomableThmb(ImageID, , ImageCaption).Replace("'", "\'") & "'); return false;")

            hsEdit.ExpandURL = "~/res/DTIMediaManager/EditMediaItem.aspx?mId=" & MediaRow.Id
            hsEdit.width = 650

            btnDeleteImage.Attributes.Add("onclick", "deleteMedia(" & MediaRow.Id & "); return false;")
            '$(this).parent().children('.Search_Holder').find('.MediaSearchButton').click(); 
        End Sub


    End Class