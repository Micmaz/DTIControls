Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports BaseClasses.BaseVirtualPathProvider

#If DEBUG Then
Partial Public Class ViewVideoScreenShot
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ViewVideoScreenShot
        Inherits BaseClasses.BaseSecurityPage
#End If
        Protected img_row As dsDTIVideo.DTIVideoManagerRow
        Protected tmpImgDt As New dsDTIVideo.DTIVideoManagerDataTable
        Private img_id As Integer = -1

#Region "Query String properties"

        Const _maxheight As Integer = 10000000
        Const _maxwidth As Integer = 10000000

        Public ReadOnly Property reqwidth() As Integer
            Get
                If Request.Params("width") Is Nothing Then Return 0
                Return Request.Params("width")
            End Get
        End Property

        Public ReadOnly Property reqheight() As Integer
            Get
                If Request.Params("height") Is Nothing Then Return 0
                Return Request.Params("height")
            End Get
        End Property

        Public ReadOnly Property maxHeight() As Integer
            Get
                If Request.Params("maxHeight") Is Nothing Then Return _maxheight
                Return Request.Params("maxHeight")
            End Get
        End Property

        Public ReadOnly Property maxWidth() As Integer
            Get
                If Request.Params("maxWidth") Is Nothing Then Return _maxwidth
                Return Request.Params("maxWidth")
            End Get
        End Property

        Public ReadOnly Property sizeHeight() As String
            Get
                Return Request.Params("sizeHeight")
            End Get
        End Property

        Public ReadOnly Property ShowPlayButtonOverlay() As Boolean
            Get
                Return Not Request.Params("showPlayOverlay") Is Nothing
            End Get
        End Property

#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            SecurePage = False

            'find both bad query string id and invalid img id
            If Integer.TryParse(Request.Params("Id"), New Integer) Then _
                img_id = Integer.Parse(Request.Params("Id"))
            parallelDataHelper.addFillDataTable("select Id, width, height, Screenshot from DTIVideoManager where id = @img_id", tmpImgDt, New Object() {img_id})
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Clear()
            Response.ClearHeaders()
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            'Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
            'Response.Cache.SetNoStore()

            img_row = tmpImgDt.FindById(img_id)
            If Not img_row Is Nothing Then
                Response.ContentType = "image/jpeg"


                Try
                    Dim img As Byte()
                    Dim height As Integer = 0
                    Dim width As Integer = 0

                    If img_row.height = 0 OrElse img_row.width = 0 OrElse Request.Params.Count = 1 Then
                        img = img_row.ScreenShot
                    Else
                        getDimentions(height, width, img_row.height, img_row.width)
                        If height = img_row.height AndAlso width = img_row.width Then
                            img = img_row.ScreenShot
                        Else
                            img = processImageArr(img_row.ScreenShot, "", height, width)
                        End If

                    End If

                    If ShowPlayButtonOverlay Then
                        Dim tmpMS As New MemoryStream(img)
                        Dim tmpimg As System.Drawing.Image = Image.FromStream(tmpMS)
                        Dim g As Graphics = Graphics.FromImage(tmpimg)
                        g.SmoothingMode = SmoothingMode.HighQuality
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality


                        Dim fsVTemp As New BaseVirtualFile("/res/DTIVideoManager/PlayButton.png")
                        Dim fsTemp As Stream = fsVTemp.Open
                        Dim fsLength As Integer = fsTemp.Length
                        Dim fsByteArray(fsLength) As Byte
                        fsTemp.Read(fsByteArray, 0, fsLength)
                        fsTemp.Close()
                        Dim tempPlayFileName As String = Path.GetTempFileName
                        Dim tempPlay As New FileStream(tempPlayFileName, FileMode.Create)
                        tempPlay.Write(fsByteArray, 0, fsLength)
                        tempPlay.Close()
                        Dim pic As New Bitmap(tempPlayFileName)
                        pic.MakeTransparent(pic.GetPixel(0, 0))
                        Dim overlayWidth As Integer = 0
                        Dim overlayHeight As Integer = 0
                        getDimention(overlayHeight, overlayWidth, pic.Height, pic.Width, width / 4, width / 4, height / 3, width / 3)

                        Dim overlayX As Integer = (width / 2) - (overlayWidth / 2)
                        Dim overlayY As Integer = (height / 2) - (overlayHeight / 2)
                        g.DrawImage(pic, overlayX, overlayY, overlayWidth, overlayHeight)

                        Dim outMS As New MemoryStream
                        tmpimg.Save(outMS, Imaging.ImageFormat.Jpeg)
                        img = outMS.ToArray
                        outMS.Close()

                        File.Delete(tempPlayFileName)
                    End If

                    Response.AddHeader("Content-Disposition", "inline; filename=""screenshot.jpg""")
                    Response.OutputStream.Write(img, 0, img.Length)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                    'Response.End()

                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End If
        End Sub


        Public Shared Sub getDimention(ByRef height As Integer, ByRef width As Integer, ByVal actualHeight As Integer, ByVal actualwidth As Integer, Optional ByVal reqheight As Integer = 0, Optional ByVal reqwidth As Integer = 0, Optional ByVal maxheight As Integer = 1000000, Optional ByVal maxwidth As Integer = 1000000, Optional ByVal sizeHeight As String = Nothing)
            height = actualHeight
            width = actualwidth
            If Not sizeHeight Is Nothing Then
                If sizeHeight.IndexOf("%") > 0 Then
                    Dim percent As Double
                    Try
                        percent = Double.Parse(sizeHeight.Replace("%", "")) * 0.01
                    Catch ex As Exception
                    End Try
                    height = percent * CType(actualHeight, Double)
                    width = percent * CType(actualwidth, Double)
                Else
                    Select Case sizeHeight
                        Case "S"
                            maxheight = 100
                            maxwidth = 100
                        Case "M"
                            maxheight = 150
                            maxwidth = 150
                        Case "L"
                            maxheight = 225
                            maxwidth = 225
                    End Select
                End If
            End If

            If Not reqheight = 0 AndAlso Not reqwidth = 0 Then
                height = reqheight
                width = reqwidth
            ElseIf Not reqheight = 0 Then
                height = reqheight
                Dim x As Double = actualwidth * reqheight / actualHeight
                width = x
            ElseIf Not reqwidth = 0 Then
                width = reqwidth
                Dim y As Double = actualHeight * reqwidth / actualwidth
                height = y
            End If

            If maxwidth < width Then
                Dim y As Double = height * maxwidth / width
                height = y
                width = maxwidth
            End If

            If maxheight < height Then
                Dim x As Double = width * maxheight / height
                width = x
                height = maxheight
            End If

        End Sub

        Private Sub getDimentions(ByRef height As Integer, ByRef width As Integer, ByVal actualHeight As Integer, ByVal actualwidth As Integer)
            getDimention(height, width, actualHeight, actualwidth, reqheight, reqwidth, maxHeight, maxWidth, sizeHeight)
        End Sub

        Private Shared Sub processImageArrSelect(ByRef Img_Array As Byte(), ByRef pic As Bitmap, ByRef toStream As MemoryStream, ByVal Img_Type As String)
            Select Case Img_Type
                Case "gif"
                    'Dim quantizer As New OctreeQuantizer(255, 8)
                    'Using quantized As Bitmap = quantizer.Quantize(pic)
                    '    quantized.Save(toStream, ImageFormat.Gif)
                    'End Using

                    pic.MakeTransparent(pic.GetPixel(0, 0))
                    pic.Save(toStream, ImageFormat.Png)
                    Img_Array = toStream.ToArray()
                    'Case "pjpeg"
                    '    pic.Save(toStream, ImageFormat.Jpeg)
                    '    Img_Array = toStream.ToArray()
                    'Case "jpeg"
                    '    pic.Save(toStream, ImageFormat.Jpeg)
                    '    Img_Array = toStream.ToArray()
                Case "x-png"
                    pic.Save(toStream, ImageFormat.Png)
                    Img_Array = toStream.ToArray()
                Case Else
                    Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                    Dim encoderParameters As EncoderParameters
                    encoderParameters = New EncoderParameters(1)
                    encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100)
                    pic.Save(toStream, info(1), encoderParameters)

                    'pic.Save(toStream, ImageFormat.Jpeg)
                    Img_Array = toStream.ToArray()
            End Select
        End Sub

        Public Shared Function processImageArr(ByRef _imgArr As Byte(), ByVal cntType As String, ByVal _height As Integer, ByVal _width As Integer) As Byte()
            'get the image from the bytestream
            Dim ms As New MemoryStream
            Dim pStream As New MemoryStream
            Dim myImage As Image = Nothing
            ms.Write(_imgArr, 0, _imgArr.Length)
            myImage = Image.FromStream(ms)

            'height and width processing
            Dim proHeight As Integer = _height
            Dim proWidth As Integer = _width

            Dim processedBP As Bitmap = New Bitmap(proWidth, proHeight)
            Dim g As Graphics = Graphics.FromImage(processedBP)

            Try
                g.SmoothingMode = SmoothingMode.HighQuality
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                Dim rect As Rectangle = New Rectangle(0, 0, proWidth, _
                    proHeight)
                g.DrawImage(myImage, rect, 0, 0, myImage.Width, myImage.Height, GraphicsUnit.Pixel)
                'processedBP.RotateFlip(RotateFlipType.Rotate180FlipNone)
                'processedBP.RotateFlip(RotateFlipType.Rotate180FlipNone)
                'processedBP.MakeTransparent()
                Dim imgType As String = cntType.Substring(cntType.LastIndexOf("/") + 1)
                processImageArrSelect(_imgArr, processedBP, pStream, imgType)
            Finally
                g.Dispose()
                processedBP.Dispose()
            End Try
            pStream.Close()
            ms.Close()
            myImage.Dispose()
            Return _imgArr
        End Function

        'Public Shared Sub getHeightWidth(ByRef _imgArr As Byte(), ByRef height As Integer, ByRef width As Integer)
        '    Dim ms As New MemoryStream
        '    Dim myImage As Image = Nothing
        '    Try
        '        ms.Write(_imgArr, 0, _imgArr.Length)
        '        myImage = Image.FromStream(ms)
        '        height = myImage.Height
        '        width = myImage.Width
        '    Catch ex As Exception
        '        height = 0
        '        width = 0
        '    End Try
        'End Sub

        'Shared Function getZoomableThmb(ByVal imgrow As DataSet1.GalleryImagesRow, Optional ByVal tmbsize As Integer = 120)
        '    Dim str As String = "<a id=""thumb" & imgrow.id & """ href=""/res/ContentHolder/ViewImage.aspx?id=" & imgrow.id & """ class=""highslide"" onclick=""return hs.expand(this, {captionId: 'caption" & imgrow.id & "'})""> " & _
        '        "	<img src=""/res/ContentHolder/ViewImage.aspx?id=" & imgrow.id & "&maxWidth=" & tmbsize & "&maxHeight=" & tmbsize & """ title=""Click to enlarge"" /></a> " & _
        '        "<div id='caption" & imgrow.id & "' class=""highslide-caption""> " & imgrow.Caption & "</div> "

        '    Return str
        'End Function

    End Class
