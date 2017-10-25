Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports DTIImageManager.dsImageManager
Imports DTIImageManager.SharedImageVariables

'#If DEBUG Then
Partial Class ViewImage
    Inherits BaseClasses.BaseSecurityPage
'#Else
'    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
'    Partial Class ViewImage
'        Inherits BaseClasses.BaseSecurityPage
'#End If
        

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

    Public ReadOnly Property forceSize() As Boolean
        Get
            If Request.Params("maxHeight") Is Nothing AndAlso Request.Params("maxWidth") Is Nothing Then
                If Request.Params("forcesize") Is Nothing Then Return True
                If Request.Params("forcesize").ToLower() = "n" Then Return False
                If Request.Params("forcesize").ToLower() = "0" Then Return False
                Return True
            End If
            If Request.Params("forcesize") Is Nothing Then Return False
            If Request.Params("forcesize").ToLower() = "y" Then Return True
            If Request.Params("forcesize").ToLower() = "1" Then Return True
            Return False
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


    Private cropoffsetValue As Double = -2
    Public ReadOnly Property cropOffset() As Double
        Get
            Dim ret As Double = 0
            If Double.TryParse(Request.Params("cropoffset"), ret) Then
                Return ret
            End If
            Return cropoffsetValue
        End Get
    End Property

    Public ReadOnly Property sizeHeight() As String
        Get
            Return Request.Params("sizeHeight")
        End Get
    End Property

        Private _img_id As Integer = -1
        Public ReadOnly Property img_id() As Integer
            Get
                If _img_id = -1 Then
                    If Not Integer.TryParse(Request.Params("Id"), _img_id) Then
                        _img_id = -1
                    End If
                End If
                Return _img_id
            End Get
        End Property

    Public ReadOnly Property ParamCount As Integer
        Get
            Return Request.Params.Count
        End Get
    End Property
#End Region

#Region "Overidable data acessors"
    Protected Overridable Function getRow() As DataRow
        Dim img_row As DataRow = myImages.FindById(img_id)
        If img_row Is Nothing Then
            sqlHelper.SafeFillTable("select * from DTIImageManager where id = @img_id", myImages, New Object() {img_id})
            img_row = myImages.FindById(img_id)
        End If
        Return img_row
    End Function

    Protected Overridable Function getRowHeight(ByVal row As DataRow) As Integer
        Try
            Return row("Height")
        Catch ex As Exception
        End Try
        Return 0
    End Function

    Protected Overridable Function getRowWidth(ByVal row As DataRow) As Integer
        Try
            Return row("Width")
        Catch ex As Exception
        End Try
        Return 0
    End Function

    Protected Overridable Function getRowContentType(ByVal row As DataRow) As String
        Try
            Return row("Image_Content_Type")
        Catch ex As Exception
        End Try
        Return "image/jpeg"
    End Function

    Protected Overridable Function getRowFilename(ByVal row As DataRow) As String
        Try
            Return row("Original_Filename")
        Catch ex As Exception
        End Try
        Return ""
    End Function

    Protected Overridable Function getRowImageData(ByVal row As DataRow) As Byte()
        Return row("Image")
    End Function

#End Region

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not Response.IsClientConnected Then Return
		Dim img_row As DataRow = getRow()

		If Not img_row Is Nothing Then
			Response.ContentType = getRowContentType(img_row)
			Try
				Dim img As Byte()
				Dim height As Integer = 0
				Dim width As Integer = 0

				If getRowHeight(img_row) = 0 OrElse getRowWidth(img_row) = 0 OrElse ParamCount = 1 Then
					img = getRowImageData(img_row)
				Else
					If forceSize AndAlso reqheight > 0 AndAlso reqwidth > 0 Then
						img = processImageArr(getRowImageData(img_row), getRowContentType(img_row), reqheight, reqwidth, True, cropOffset)
					Else
						getDimensions(height, width, getRowHeight(img_row), getRowWidth(img_row))
						If height = getRowHeight(img_row) AndAlso width = getRowWidth(img_row) Then
							img = getRowImageData(img_row)
						Else
							img = processImageArr(getRowImageData(img_row), getRowContentType(img_row), height, width)
						End If
					End If
				End If

				Response.AddHeader("Content-Disposition", "inline; filename=""" & getRowFilename(img_row) & """")
				Response.OutputStream.Write(img, 0, img.Length)
				HttpContext.Current.ApplicationInstance.CompleteRequest() 'Response.End()

			Catch ex As Exception
				Response.Write(ex.Message)
			End Try
		End If
	End Sub


	Public Shared Function GZipSupported() As Boolean
        Dim AcceptEncoding As String = System.Web.HttpContext.Current.Request.Headers("Accept-Encoding")
        If AcceptEncoding Is Nothing Then Return False
        If Not String.IsNullOrEmpty(AcceptEncoding) And (AcceptEncoding.Contains("gzip") Or AcceptEncoding.Contains("deflate")) Then
            Return True
        End If
        Return False
    End Function

        Public Shared LastModified As Date = Nothing
        Public Function isModified() As Boolean
            Dim modSince As DateTime
            If Not String.IsNullOrEmpty(Request.Headers("If-None-Match")) Then
                If Request.Headers("If-None-Match") = etag Then Return False Else Return True
            End If
            If Not String.IsNullOrEmpty(("If-Modified-Since")) Then
                If Date.TryParse(Request.Headers("If-Modified-Since"), modSince) Then
                    If LastModified.AddSeconds(-1) > modSince Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If
            Return True
        End Function

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If LastModified = Nothing Then LastModified = Date.Now
            Response.Cache.SetCacheability(Web.HttpCacheability.ServerAndPrivate)
            Response.Cache.SetLastModified(LastModified)
            Response.AppendHeader("Vary", "Content-Encoding")
            Response.Cache.SetETag(etag)
        If Not isModified() Then
			Response.StatusCode = 304
			Response.StatusDescription = "Not Modified"
			Response.AddHeader("Content-Length", "0")
			Response.Cache.SetCacheability(Web.HttpCacheability.Public)
			Response.Cache.SetLastModified(LastModified)
			BaseClasses.DataBase.endResponse()
			Return
		End If

            If GZipSupported() Then
                Dim AcceptEncoding As String = System.Web.HttpContext.Current.Request.Headers("Accept-Encoding")
                If AcceptEncoding.Contains("deflate") Then
                    Response.Filter = New System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress)
                    Response.AppendHeader("Content-Encoding", "deflate")
                Else

                    Response.Filter = New Compression.GZipStream(Response.Filter, Compression.CompressionMode.Compress)
                    Response.AddHeader("Content-Encoding", "gzip")
                End If
            End If

        End Sub

        ''' <summary>
        ''' Generates a MD5 hash of a given password
        ''' </summary>
        ''' <param name="input">String to hash</param>
        ''' <returns>MD5 hash of String</returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Generates a MD5 hash of a given password")> _
        Public Shared Function GenerateHash(ByVal input As String) As String
            Dim md5Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()
            Dim hashedBytes As Byte()
            Dim encoder As New System.Text.UTF8Encoding()

            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(input))

            Dim strOutput As New System.Text.StringBuilder(hashedBytes.Length)

            For i As Integer = 0 To hashedBytes.Length - 1
                strOutput.Append(hashedBytes(i).ToString("X2"))
            Next

            Return strOutput.ToString()
        End Function

        Public ReadOnly Property etag() As String
            Get
                Return """" & GenerateHash(img_id & "_" & reqwidth & "_" & reqheight & "_" & maxHeight & "_" & maxWidth & "_" & sizeHeight).Replace("-", "") & """"
            End Get
        End Property


    Public Sub getDimensions(ByRef height As Integer, ByRef width As Integer, ByVal actualHeight As Integer, ByVal actualwidth As Integer)
        getDimention(height, width, actualHeight, actualwidth, reqheight, reqwidth, maxHeight, maxWidth, sizeHeight)
    End Sub


#Region "Shared Methods"

    ''' <summary>
    ''' The height and width are output variables only. For request height use the 
    ''' optioanl parameters reqHeight and reqwidth 
    ''' For quick sizing use the sizeHeight string as the value as either a percentage 
    ''' or S,M,L,XL,XXL for 100,150,225,500,100 pixle images 
    ''' </summary>
    ''' <param name="height"></param>
    ''' <param name="width"></param>
    ''' <param name="actualHeight"></param>
    ''' <param name="actualwidth"></param>
    ''' <param name="reqheight"></param>
    ''' <param name="reqwidth"></param>
    ''' <param name="maxheight"></param>
    ''' <param name="maxwidth"></param>
    ''' <param name="sizeHeight"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The height and width are output variables only. For request height use the    optioanl parameters reqHeight and reqwidth    For quick sizing use the sizeHeight string as the value as either a percentage    or S,M,L,XL,XXL for 100,150,225,500,100 pixle images")> _
    Public Shared Sub getDimention(ByRef height As Integer, ByRef width As Integer, ByVal actualHeight As Integer, ByVal actualwidth As Integer, Optional ByVal reqheight As Integer = 0, Optional ByVal reqwidth As Integer = 0, Optional ByVal maxheight As Integer = 10000, Optional ByVal maxwidth As Integer = 10000, Optional ByVal sizeHeight As String = Nothing)
        height = actualHeight
        width = actualwidth
        If Not sizeHeight Is Nothing Then
            sizeHeight = sizeHeight.ToUpper()
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
                    Case "XL"
                        maxheight = 500
                        maxwidth = 500
                    Case "XXL"
                        maxheight = 1000
                        maxwidth = 1000
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

	Private Shared Sub processImageArrSelect(ByRef Img_Array As Byte(), ByRef pic As Bitmap, ByVal Img_Type As String)
		Using toStream As New MemoryStream
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
				Case Else '"x-png"
					pic.Save(toStream, ImageFormat.Png)
					Img_Array = toStream.ToArray()
					'Case Else
					'    Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
					'    Dim encoderParameters As EncoderParameters
					'    encoderParameters = New EncoderParameters(1)
					'    encoderParameters.Param(0) = New EncoderParameter(Encoder.Quality, 100)
					'    pic.Save(toStream, info(1), encoderParameters)

					'    'pic.Save(toStream, ImageFormat.Jpeg)
					'    Img_Array = toStream.ToArray()
			End Select
		End Using
	End Sub


	Public Shared Function processImageArr(ByVal _imgArr As Byte(), ByVal cntType As String, ByVal _height As Integer, ByVal _width As Integer, Optional ByVal forceSize As Boolean = False, Optional ByVal cropOffset As Double = -2, Optional ByVal orientation As RotateFlipType = RotateFlipType.RotateNoneFlipNone) As Byte()
		'get the image from the bytestream


		Using ms As New MemoryStream(_imgArr)
			Using myImage As Image = Image.FromStream(ms)

				'This code auto-orients images taken from cell phones
				If orientation = 0 AndAlso Array.IndexOf(myImage.PropertyIdList, 274) > -1 Then
					Select Case CInt(myImage.GetPropertyItem(274).Value(0))
						Case 1
							orientation = RotateFlipType.RotateNoneFlipNone
							Exit Select
						Case 2
							orientation = RotateFlipType.RotateNoneFlipX
							Exit Select
						Case 3
							orientation = RotateFlipType.Rotate180FlipNone
							Exit Select
						Case 4
							orientation = RotateFlipType.Rotate180FlipX
							Exit Select
						Case 5
							orientation = RotateFlipType.Rotate90FlipX
							Exit Select
						Case 6
							orientation = RotateFlipType.Rotate90FlipNone
							Exit Select
						Case 7
							orientation = RotateFlipType.Rotate270FlipX
							Exit Select
						Case 8
							orientation = RotateFlipType.Rotate270FlipNone
							Exit Select
					End Select
					myImage.RemovePropertyItem(274)
				End If
				If orientation = Nothing Then orientation = RotateFlipType.RotateNoneFlipNone
				If orientation = 1 OrElse orientation = 3 OrElse orientation = 5 OrElse orientation = 7 Then
					Dim tmp As Integer = _height
					_height = _width
					_width = tmp
				End If

				If _height = 0 OrElse _width = 0 Then _
					getDimention(_height, _width, myImage.Height, myImage.Width, _height, _width)


				'If Not orientation = RotateFlipType.RotateNoneFlipNone Then

				'End If
				' This EXIF data is now invalid and should be removed.

				'height and width processing
				Dim proHeight As Integer = _height
				Dim proWidth As Integer = _width
				If forceSize Then
					Dim matchwidth As Boolean = Math.Abs(myImage.Height - _height) > Math.Abs(myImage.Width - _width)
					If matchwidth Then
						If _height > myImage.Height * (_width / myImage.Width) Then matchwidth = False
					Else
						If _width > myImage.Width * (_height / myImage.Height) Then matchwidth = True
					End If
					If matchwidth Then
						proWidth = _width
						proHeight = myImage.Height * (_width / myImage.Width)
					Else
						proWidth = myImage.Width * (_height / myImage.Height)
						proHeight = _height
					End If
					If cropOffset < -1 OrElse cropOffset > 1 Then
						If myImage.Width > myImage.Height Then
							cropOffset = 0
						Else
							cropOffset = -0.9
						End If
					End If
				End If
				Dim Format As PixelFormat = myImage.PixelFormat
				If Format.ToString().Contains("Indexed") Then
					Format = PixelFormat.Format24bppRgb
				End If

				Dim processedBP As Bitmap = New Bitmap(proWidth, proHeight, Format)
				Dim g As Graphics = Graphics.FromImage(processedBP)

				Try
					Dim ia As New ImageAttributes
					ia.SetWrapMode(WrapMode.TileFlipXY)
					g.SmoothingMode = SmoothingMode.HighQuality
					g.InterpolationMode = InterpolationMode.HighQualityBicubic
					g.PixelOffsetMode = PixelOffsetMode.HighQuality
					Dim rect As Rectangle = New Rectangle(0, 0, proWidth,
						proHeight)
					g.DrawImage(myImage, rect, 0, 0, myImage.Width, myImage.Height, GraphicsUnit.Pixel, ia)
					If forceSize Then
						processedBP = cropImage(processedBP, _width, _height, cropOffset)
					End If
					processedBP.RotateFlip(orientation)
					'processedBP.RotateFlip(RotateFlipType.Rotate180FlipNone)
					'processedBP.RotateFlip(RotateFlipType.Rotate180FlipNone)
					'processedBP.MakeTransparent()
					Dim imgType As String = cntType.Substring(cntType.LastIndexOf("/") + 1)
					processImageArrSelect(_imgArr, processedBP, imgType)
				Finally
					g.Dispose()
					processedBP.Dispose()
				End Try

			End Using
		End Using

		Return _imgArr
	End Function

	Public Shared Function cropImage(ByVal img As Image, ByVal width As Integer, ByVal height As Integer, Optional ByVal Cropoffset As Double = 0) As Image
        Dim rec As Rectangle
        Cropoffset = Cropoffset + 1
        Dim wid As Double = (img.Width - width) * (Cropoffset / 2)
        Dim hei As Double = (img.Height - height) * (Cropoffset / 2)
        rec = New Rectangle(wid, hei, width, height)
        'If Cropoffset = -1 Then
        '    rec = New Rectangle(0, 0, width, height)
        'ElseIf Cropoffset = 1 Then
        '    rec = New Rectangle((img.Width - width), (img.Height - height), width, height)
        'Else
        '    rec = New Rectangle((img.Width - width) / 2, (img.Height - height) / 2, width, height)
        'End If

        Return cropImage(img, rec)
    End Function

    Public Shared Function cropImage(ByVal img As Image, ByVal croparea As Rectangle) As Image
        Dim bm As New Bitmap(img)
        Dim bmCrop As Bitmap = bm.Clone(croparea, bm.PixelFormat)
        Return bmCrop
    End Function

    Public Shared Sub getHeightWidth(ByRef _imgArr As Byte(), ByRef height As Integer, ByRef width As Integer)
        Dim ms As New MemoryStream
        Dim myImage As Image = Nothing
        Try
            ms.Write(_imgArr, 0, _imgArr.Length)
            myImage = Image.FromStream(ms)
            height = myImage.Height
            width = myImage.Width
        Catch ex As Exception
            height = 0
            width = 0
        End Try
    End Sub

    Shared Function getZoomableThmb(ByVal id As Integer, Optional ByVal tmbsize As Integer = 120, Optional ByVal caption As String = "") As String
        Dim myHS As New ImageThumb
        myHS.ImageId = id
        If caption <> "" Then myHS.Caption = caption
        myHS.ThumbSize = tmbsize

        Return myHS.outputValue
    End Function

#End Region

    End Class
