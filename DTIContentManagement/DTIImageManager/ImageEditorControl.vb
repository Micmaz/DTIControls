Imports DTIImageManager.dsImageManager
Imports HighslideControls

''' <summary>
''' control to preview image, click to edit
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ImageEditorControl
    Inherits DTIServerControls.DTIServerBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ImageEditorControl
        Inherits DTIServerControls.DTIServerBase
#End If
        Protected myEditor As EditImageUserControl

    Private _imageRow As DTIImageManagerRow

    ''' <summary>
    ''' The datarow of the current image.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The datarow of the current image.")> _
        Public Property ImageRow() As DTIImageManagerRow
            Get
                Return _imageRow
            End Get
            Set(ByVal value As DTIImageManagerRow)
                _imageRow = value
            End Set
        End Property

        Private Sub mypage_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles mypage.InitComplete
            HighslideHeaderControl.addToPage(mypage)
        End Sub

    ''' <summary>
    ''' Save any changes to the current image row.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Save any changes to the current image row.")> _
        Public Sub save()
            myEditor.saveValues()
        End Sub

        Private Sub ImageEditorControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            myEditor = DirectCast(mypage.LoadControl("~/res/DTIImageManager/EditImageUserControl.ascx"), EditImageUserControl)
            myEditor.ID = "previewUserControl"
            myEditor.imageRow = ImageRow
            Controls.Add(myEditor)
        End Sub
    End Class


