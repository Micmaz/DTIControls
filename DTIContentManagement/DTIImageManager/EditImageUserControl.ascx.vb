Imports DTIImageManager.dsImageManager
Imports System.IO
Imports System.Drawing
Imports HighslideControls.SharedHighslideVariables
Imports DTIImageManager.SharedImageVariables

#If DEBUG Then
Partial Public Class EditImageUserControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class EditImageUserControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private _width As Integer
        Private _height As Integer
        Private userId As Integer = -1
        Public Event ImageSaved(ByVal ImageId As Integer)

        Private ReadOnly Property currImageId() As Integer
            Get
            If Not Request.QueryString("mid") Is Nothing Then
                Return Request.QueryString("mid")
            Else
                Return -1
            End If
            End Get
        End Property

        Private _imageRow As DTIImageManagerRow
        Public Property imageRow() As DTIImageManagerRow
            Get
                Return _imageRow
            End Get
            Set(ByVal value As DTIImageManagerRow)
                _imageRow = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                fillValues()
            End If
        End Sub

        Private Sub fillValues()
            If imageRow Is Nothing Then
                sqlHelper.SafeFillTable("select * from DTIImageManager where Id = @id", myImages, New Object() {currImageId})

                imageRow = myImages.FindById(currImageId)
            End If

            _width = imageRow.Width
            _height = imageRow.Height
            lblSize.Text = _width & "x" & _height
            imgPreviewEdit.Visible = True
            imgPreviewEdit.HighslideDisplayMode = HighslideDisplayModes.Iframe
            imgPreviewEdit.Title = "Click to rotate or crop image"
            imgPreviewEdit.ExpandURL = "~/res/DTIImageManager/ManipulateImage.aspx?Id=" & imageRow.Id
            imgPreviewEdit.ThumbURL = "~/res/DTIImageManager/ViewImage.aspx?Id=" & imageRow.Id & "&width=180&r=" & (New Random).Next

            If Not imgPreviewEdit.HighslideVariables.Contains("width") Then
                imgPreviewEdit.HighslideVariables.Add("width", imageRow.Width + 40)
            End If
            If Not imgPreviewEdit.HighslideVariables.Contains("preserveContent") Then
                imgPreviewEdit.HighslideVariables.Add("preserveContent", False)
            End If

            pnlEdit.Attributes.Add("name", "i" & imageRow.Id)

        End Sub

        Friend Sub saveValues()
            If imageRow Is Nothing Then
                imageRow = myImages.FindById(currImageId)
            End If
            If imageRow Is Nothing Then
                sqlHelper.FillDataTable("select * from DTIImageManager where id = " & currImageId, myImages)
                imageRow = myImages.FindById(currImageId)

                _width = imageRow.Width
                _height = imageRow.Height
                lblSize.Text = _width & "x" & _height
            End If

            If Not imageRow Is Nothing Then
                sqlHelper.Update(imageRow.Table)

                RaiseEvent ImageSaved(imageRow.Id)
            End If
        End Sub
    End Class