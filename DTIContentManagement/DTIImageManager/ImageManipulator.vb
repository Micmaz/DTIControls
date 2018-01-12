Imports DTIImageManager.dsImageManager
Imports System.IO
Imports System.Drawing
Imports HighslideControls
Imports System.Drawing.Imaging
Imports DTIImageManager.SharedImageVariables
Imports DTIMediaManager

''' <summary>
''' control to crop and rotate an image
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ImageManipulator
    Inherits DTIServerControls.DTIServerBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ImageManipulator
        Inherits DTIServerControls.DTIServerBase
#End If
    Private WithEvents myCropper As New ImageCropper
    Private WithEvents btnImgRotateLeft As New ImageButton
    Private WithEvents btnImgRotateRight As New ImageButton
    Private WithEvents btnRotateLeft As New LinkButton
    Private WithEvents btnRotateRight As New LinkButton
    Private WithEvents btnApplyCrop As New LinkButton
    Private WithEvents btnSave As New Button
    Private WithEvents btnSaveNew As New Button
    Private WithEvents btnCancel As New Button
    Private myHTMLTable As New Table
    Private myHTMLRow1 As New TableRow
    Private myHTMLRow2 As New TableRow
    Private cell1_1 As New TableCell
    Private cell2_1 As New TableCell
    Private cell2_2 As New TableCell
    Private cell2_3 As New TableCell

        Public imageChanged As Boolean = False
        Private ReadOnly Property rdm() As Integer
            Get
                If Session("MyThumbNailNum_" & currImageId) Is Nothing OrElse imageChanged Then
                    Dim rdmNumGen As New Random
                    Session("MyThumbNailNum_" & currImageId) = rdmNumGen.Next
                End If
                Return Session("MyThumbNailNum_" & currImageId)
            End Get
        End Property


        Private ReadOnly Property currImageId() As Integer
            Get
            If Not HttpContext.Current.Request.QueryString("id") Is Nothing Then
                Return HttpContext.Current.Request.QueryString("id")
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

        Private _isIframe As Boolean = True
        Public Property IsIframe() As Boolean
            Get
                Return _isIframe
            End Get
            Set(ByVal value As Boolean)
                _isIframe = value
            End Set
        End Property

    Private Sub ImageManipulator_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Me.Page.IsPostBack AndAlso myImages.FindById(currImageId) IsNot Nothing Then
            myImages.FindById(currImageId).Delete()
            myImages.AcceptChanges()
        End If

        If imageRow Is Nothing Then
            imageRow = myImages.FindById(currImageId)
            If imageRow Is Nothing Then
                sqlhelper.FillDataTable("select * from DTIImageManager where Id = @id", myImages, currImageId)
            End If
        End If
        myCropper.ID = "myCropper"
        btnApplyCrop.ID = "btnApplyCrop"
        btnSave.ID = "btnSave"
        btnSaveNew.ID = "btnSaveNew"
        btnCancel.ID = "btnCancel"
        btnImgRotateRight.ID = "btnImgRotateRight"
        btnImgRotateLeft.ID = "btnImgRotateLeft"
        btnRotateLeft.ID = "btnRotateLeft"
        btnRotateLeft.ID = "btnRotateLeft"


        cell1_1.Controls.Add(myCropper)
        cell1_1.Controls.Add(New LiteralControl("<br />"))
        cell1_1.Controls.Add(btnApplyCrop)

        cell2_1.Controls.Add(btnImgRotateLeft)
        cell2_1.Controls.Add(New LiteralControl("<br />"))
        cell2_1.Controls.Add(btnRotateLeft)

        'cell2_2.Controls.Add(btnSave)
        cell2_2.Controls.Add(btnSaveNew)
        cell2_2.Controls.Add(btnCancel)

        cell2_3.Controls.Add(btnImgRotateRight)
        cell2_3.Controls.Add(New LiteralControl("<br />"))
        cell2_3.Controls.Add(btnRotateRight)

        myHTMLRow1.Cells.Add(cell1_1)

        myHTMLRow2.Cells.Add(cell2_1)
        myHTMLRow2.Cells.Add(cell2_2)
        myHTMLRow2.Cells.Add(cell2_3)

        myHTMLTable.Rows.Add(myHTMLRow1)
        myHTMLTable.Rows.Add(myHTMLRow2)

        Controls.Add(myHTMLTable)
    End Sub

        Private Sub ImageManipulator_DataReady() Handles Me.DataReady
            imageRow = myImages.FindById(currImageId)
        End Sub


        Private Sub ImageManipulator_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            cell1_1.ColumnSpan = 3
            cell1_1.Style("text-align") = "center"
            cell2_2.Style("text-align") = "center"
            cell2_3.Style("text-align") = "right"

            myHTMLTable.Style("font-size") = "small"

		btnImgRotateLeft.ImageUrl = BaseClasses.Scripts.ScriptsURL() & "DTIImageManager/rotateImage_left.jpg"
		btnImgRotateLeft.Width = 25
            btnImgRotateLeft.Height = 25
            btnRotateLeft.Text = "Rotate Left"

		btnImgRotateRight.ImageUrl = BaseClasses.Scripts.ScriptsURL() & "DTIImageManager/rotateImage_right.jpg"
		btnImgRotateRight.Width = 25
            btnImgRotateRight.Height = 25
            btnRotateRight.Text = "Rotate Right"

            btnSave.Text = "Apply and Return"
            btnSaveNew.Text = "Save as New Image"
            btnCancel.Text = "Cancel"

            myCropper.ImageUrl = "~/res/DTIImageManager/ViewImage.aspx?Id=" & currImageId & "&r=" & rdm

            btnApplyCrop.Text = "Apply Crop"
            btnApplyCrop.Style("display") = "none"

            'registerClientStartupScriptBlock("applyCropFade", "$('.jcrop-tracker').click(function() {$('#" & btnApplyCrop.ClientID & "').fadeIn(1000);});", True)
            'registerClientStartupScriptBlock("applyCropFade", "$(document).ready(function () { $('.jcrop-tracker').click(function() {$('#" & btnApplyCrop.ClientID & "').fadeIn(1000);})});", True)
            'registerClientStartupScriptBlock("applyCropFade", "$(document).ready(function () { $('div.jcrop-tracker').mouseover(function() {$('#" & btnApplyCrop.ClientID & "').fadeIn(1000);})});", True)
            'registerClientStartupScriptBlock("applyCropFade", "$(function() { $('#" & myCropper.ClientID & " ').Jcrop({ onChange: function() {$('#" & btnApplyCrop.ClientID & "').fadeIn(1000);}});});", True)
        registerClientStartupScriptBlock("applyCropFade", "function doCropSelect(c) { setHiddenField( c); $('#" & btnApplyCrop.ClientID & "').fadeIn(1000); }", True)
            myCropper.OnSelectFunctionName = "doCropSelect"

            If IsIframe Then

                'registerClientScriptBlock("queryInit", "$.query = { hash: true };", True)
                'registerClientScriptFile("JQQ", "~/res/jQueryLibrary/jquery.query-2.1.7.js")

                Dim refreshScript As String = "function doWork(myId) {" & vbCrLf
                refreshScript &= "  for(i=0;i<window.parent.frames.length;i++) {" & vbCrLf
                refreshScript &= "      $('.Gallery_Holder, div[id$=""pnlEdit""]', window.parent.frames[i].document).find('img[src*=""ViewImage""]').each(function() {" & vbCrLf
                refreshScript &= "          var q = $.query.load($(this).attr('src'))" & vbCrLf
                refreshScript &= "          if(q.get('id') == myId || q.get('Id') == myId || q.get('ID') == myId) {" & vbCrLf
                refreshScript &= "              $(this).attr('src', $(this).attr('src') + '&rr=' + Math.floor(Math.random()*1001));" & vbCrLf
                refreshScript &= "          }" & vbCrLf
                refreshScript &= "      });" & vbCrLf
                refreshScript &= "  }" & vbCrLf
                refreshScript &= "}"
                registerClientStartupScriptBlock("reloadIframes", refreshScript, True) '"function doWork(currImageId) { var i; for (i = 0; i < parent.frames.length; i++) { $.each($(""img[src*='id\="" + currImageId + ""'],img[src*='Id\="" + currImageId + ""'],img[src*='ID\="" + currImageId + ""']"", parent.frames[i].document), function () { this.attr('src', this.attr('src') + '&rr=' + Math.floor(Math.random()*1000)); })}}", True)
                'btnSave.Attributes.Add("onclick", "doWork(" & currImageId & ");") '"setTimeout('doWork(" & currImageId & ")', 2000);")
                'btnSaveNew.Attributes.Add("onclick", "doWork(" & currImageId & ");")
            End If
        End Sub

        Private Sub btnImgRotateLeft_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnImgRotateLeft.Click
            RotateImageHelper(-90)
        End Sub

        Private Sub btnImgRotateRight_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnImgRotateRight.Click
            RotateImageHelper(90)
        End Sub

        Private Sub btnRotateLeft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRotateLeft.Click
            RotateImageHelper(-90)
        End Sub

        Private Sub btnRotateRight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRotateRight.Click
            RotateImageHelper(90)
        End Sub

        Private Sub RotateImageHelper(ByVal angle As Single)
            If imageRow Is Nothing Then
                sqlhelper.SafeFillTable("select * from DTIImageManager where Id = @id", myImages, New Object() {currImageId})

                imageRow = myImages.FindById(currImageId)
            End If

            If Not imageRow Is Nothing Then
                Dim ms As New MemoryStream(imageRow.Image)
                Dim myImg As Image = Image.FromStream(ms)

                Dim rotatedImage As Bitmap = RotateImage(myImg, angle)
                Using stream As New System.IO.MemoryStream

                    If imageRow.Image_Content_Type.ToLower.Contains("jpg") Then
                        rotatedImage.Save(stream, Imaging.ImageFormat.Jpeg)
                    ElseIf imageRow.Image_Content_Type.ToLower.Contains("jpeg") Then
                        rotatedImage.Save(stream, Imaging.ImageFormat.Jpeg)
                    ElseIf imageRow.Image_Content_Type.ToLower.Contains("png") Then
                        rotatedImage.Save(stream, Imaging.ImageFormat.Png)
                    ElseIf imageRow.Image_Content_Type.ToLower.Contains("gif") Then
                        rotatedImage.Save(stream, Imaging.ImageFormat.Gif)
                    End If

                    imageRow.Image = stream.ToArray
                End Using

                imageRow.Width = rotatedImage.Width
                imageRow.Height = rotatedImage.Height

                imageChanged = True
            End If
        End Sub


        'Thanks to James T. Johnson from The Code Project for this function
        Public Shared Function RotateImage(ByVal image As Image, ByVal angle As Single) As Bitmap
            If image Is Nothing Then
                Throw New ArgumentNullException("image")
            End If

            Const pi2 As Double = Math.PI / 2.0R

            ' Why can't C# allow these to be const, or at least readonly
            ' *sigh*  I'm starting to talk like Christian Graus :omg:
            Dim oldWidth As Double = CDbl(image.Width)
            Dim oldHeight As Double = CDbl(image.Height)

            ' Convert degrees to radians
            Dim theta As Double = CDbl(angle) * Math.PI / 180.0R
            Dim locked_theta As Double = theta

            ' Ensure theta is now [0, 2pi)
            While locked_theta < 0.0R
                locked_theta += 2 * Math.PI
            End While

            Dim newWidth As Double, newHeight As Double
            Dim nWidth As Integer, nHeight As Integer
            ' The newWidth/newHeight expressed as ints
            '#Region "Explaination of the calculations"
            '
            '			 * The trig involved in calculating the new width and height
            '			 * is fairly simple; the hard part was remembering that when 
            '			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
            '			 * height are switched.
            '			 * 
            '			 * When you rotate a rectangle, r, the bounding box surrounding r
            '			 * contains for right-triangles of empty space.  Each of the 
            '			 * triangles hypotenuse's are a known length, either the width or
            '			 * the height of r.  Because we know the length of the hypotenuse
            '			 * and we have a known angle of rotation, we can use the trig
            '			 * function identities to find the length of the other two sides.
            '			 * 
            '			 * sine = opposite/hypotenuse
            '			 * cosine = adjacent/hypotenuse
            '			 * 
            '			 * solving for the unknown we get
            '			 * 
            '			 * opposite = sine * hypotenuse
            '			 * adjacent = cosine * hypotenuse
            '			 * 
            '			 * Another interesting point about these triangles is that there
            '			 * are only two different triangles. The proof for which is easy
            '			 * to see, but its been too long since I've written a proof that
            '			 * I can't explain it well enough to want to publish it.  
            '			 * 
            '			 * Just trust me when I say the triangles formed by the lengths 
            '			 * width are always the same (for a given theta) and the same 
            '			 * goes for the height of r.
            '			 * 
            '			 * Rather than associate the opposite/adjacent sides with the
            '			 * width and height of the original bitmap, I'll associate them
            '			 * based on their position.
            '			 * 
            '			 * adjacent/oppositeTop will refer to the triangles making up the 
            '			 * upper right and lower left corners
            '			 * 
            '			 * adjacent/oppositeBottom will refer to the triangles making up 
            '			 * the upper left and lower right corners
            '			 * 
            '			 * The names are based on the right side corners, because thats 
            '			 * where I did my work on paper (the right side).
            '			 * 
            '			 * Now if you draw this out, you will see that the width of the 
            '			 * bounding box is calculated by adding together adjacentTop and 
            '			 * oppositeBottom while the height is calculate by adding 
            '			 * together adjacentBottom and oppositeTop.
            '			 

            '#End Region

            Dim adjacentTop As Double, oppositeTop As Double
            Dim adjacentBottom As Double, oppositeBottom As Double

            ' We need to calculate the sides of the triangles based
            ' on how much rotation is being done to the bitmap.
            '   Refer to the first paragraph in the explaination above for 
            '   reasons why.
            If (locked_theta >= 0.0R AndAlso locked_theta < pi2) OrElse (locked_theta >= Math.PI AndAlso locked_theta < (Math.PI + pi2)) Then
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight
            Else
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth
            End If

            newWidth = adjacentTop + oppositeBottom
            newHeight = adjacentBottom + oppositeTop

            nWidth = CInt(Math.Ceiling(newWidth))
            nHeight = CInt(Math.Ceiling(newHeight))

            Dim rotatedBmp As New Bitmap(nWidth, nHeight)

            Using g As Graphics = Graphics.FromImage(rotatedBmp)
                ' This array will be used to pass in the three points that 
                ' make up the rotated image
                Dim points As Point()

                '
                '				 * The values of opposite/adjacentTop/Bottom are referring to 
                '				 * fixed locations instead of in relation to the
                '				 * rotating image so I need to change which values are used
                '				 * based on the how much the image is rotating.
                '				 * 
                '				 * For each point, one of the coordinates will always be 0, 
                '				 * nWidth, or nHeight.  This because the Bitmap we are drawing on
                '				 * is the bounding box for the rotated bitmap.  If both of the 
                '				 * corrdinates for any of the given points wasn't in the set above
                '				 * then the bitmap we are drawing on WOULDN'T be the bounding box
                '				 * as required.
                '				 

                If locked_theta >= 0.0R AndAlso locked_theta < pi2 Then

                    points = New Point() {New Point(CInt(oppositeBottom), 0), New Point(nWidth, CInt(oppositeTop)), New Point(0, CInt(adjacentBottom))}
                ElseIf locked_theta >= pi2 AndAlso locked_theta < Math.PI Then
                    points = New Point() {New Point(nWidth, CInt(oppositeTop)), New Point(CInt(adjacentTop), nHeight), New Point(CInt(oppositeBottom), 0)}
                ElseIf locked_theta >= Math.PI AndAlso locked_theta < (Math.PI + pi2) Then
                    points = New Point() {New Point(CInt(adjacentTop), nHeight), New Point(0, CInt(adjacentBottom)), New Point(nWidth, CInt(oppositeTop))}
                Else
                    points = New Point() {New Point(0, CInt(adjacentBottom)), New Point(CInt(oppositeBottom), 0), New Point(CInt(adjacentTop), nHeight)}
                End If

                g.DrawImage(image, points)
            End Using

            Return rotatedBmp
        End Function

        Private Sub myCropper_ImageCropped(ByRef croppedImage As System.Drawing.Image, ByVal contentType As String, ByVal format As System.Drawing.Imaging.ImageFormat) Handles myCropper.ImageCropped
            If imageRow Is Nothing Then
                imageRow = myImages.FindById(currImageId)
                If imageRow Is Nothing Then
                    sqlhelper.SafeFillTable("select * from DTIImageManager where Id = @id", myImages, New Object() {currImageId})
                    imageRow = myImages.FindById(currImageId)
                End If
            End If

            Dim ms As New MemoryStream
            If Not format Is System.Drawing.Imaging.ImageFormat.Png Then
                croppedImage.Save(ms, format)
            Else

                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                Dim encoderParameters As EncoderParameters
                encoderParameters = New EncoderParameters(1)
                encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100)

                croppedImage.Save(ms, info(1), encoderParameters)
            End If

            imageRow.Width = croppedImage.Width
            imageRow.Height = croppedImage.Height
            imageRow.Image = ms.ToArray
            imageChanged = True
        End Sub

        Private Sub btnSaveNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
            Dim newImg As dsImageManager.DTIImageManagerRow = myImages.NewDTIImageManagerRow
            newImg.Original_Filename = imageRow.Original_Filename.Replace(".", "_copy.")
            newImg.Image_Content_Type = imageRow.Image_Content_Type
            newImg.Height = imageRow.Height
            newImg.Width = imageRow.Width
            newImg.Image = imageRow.Image
            myImages.AddDTIImageManagerRow(newImg)

            imageRow.RejectChanges()
        sqlhelper.Update(myImages)

        Dim dtImgMan As New dsMedaManagerTbl.DTIMediaManagerDataTable
        dtImgMan.AddDTIMediaManagerRow("Image", newImg.Id, True, False, "", 0, 0, "", "", "ContentManagement", Date.Now, "DTIImageManager/ViewImage.aspx?Id=" & newImg.Id, 0, 0)
        sqlhelper.Update(dtImgMan)


            registerClose()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        imageRow.RejectChanges()

            registerClose()
        End Sub

        Private Sub registerClose()
        registerClientStartupScriptBlock("closeMe", "doWork(" & currImageId & ");window.location.href = '~/res/DTIContentManagement/ImageSelectorDlg.aspx';", True)
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            registerClose()
        End Sub

        Private Sub ImageManipulator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "$.query = { prefix: false };")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "jQueryLibrary/jquery.query.js")
        End Sub
    End Class

