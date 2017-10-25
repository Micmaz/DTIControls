Imports System.Drawing.Imaging

Partial Public Class ImageCropper
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ImageCropper1_ImageCropped(ByRef croppedImage As System.Drawing.Image, ByVal contentType As String, ByVal format As System.Drawing.Imaging.ImageFormat) Handles ImageCropper1.ImageCropped
        Response.Clear()
        Response.ClearHeaders()
        Response.ContentType() = contentType
        If Not format Is System.Drawing.Imaging.ImageFormat.Png Then
            croppedImage.Save(Response.OutputStream, format)
        Else
            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
            Dim encoderParameters As EncoderParameters
            encoderParameters = New EncoderParameters(1)
            encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100)
            croppedImage.Save(Response.OutputStream, info(1), encoderParameters)
            'croppedimage.Save(response.OutputStream, new System.Drawing.Imaging.ImageCodecInfo(),
        End If
        Response.End()
    End Sub
End Class