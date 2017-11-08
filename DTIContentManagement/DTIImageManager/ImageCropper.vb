Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Security.Permissions
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class ImageCropper
    Inherits System.Web.UI.WebControls.Image
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("setSelect"), _
    ToolboxData("<{0}:ImageCropper runat=""server""> </{0}:ImageCropper>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ImageCropper
        Inherits System.Web.UI.WebControls.Image
#End If
#Region "Properties"
#Region "NonBrowsables"

        ''' <summary>
        ''' Property to get Cropped Coordinates
        ''' </summary>
        ''' <returns>
        ''' Cropped Coordinates string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
            <System.ComponentModel.Description("Property to get Cropped Coordinates"),Browsable(False)> _
            Public ReadOnly Property CroppedCoordinatesString() As String
            Get
                Return Me.Page.Request.Params(Me.ID & "_hidden")
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped X1 Coordinate
        ''' </summary>
        ''' <returns>
        ''' Cropped X1 Coordinate string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped X1 Coordinate"),Browsable(False)> _
        Public ReadOnly Property CroppedX1() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(0)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped Y1 Coordinate
        ''' </summary>
        ''' <returns>
        ''' Cropped Y1 Coordinate String returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped Y1 Coordinate"),Browsable(False)> _
        Public ReadOnly Property CroppedY1() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(1)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped X2 Coordinate
        ''' </summary>
        ''' <returns>
        ''' Cropped X2 Coordinate String returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped X2 Coordinate"),Browsable(False)> _
        Public ReadOnly Property CroppedX2() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(2)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped Y2 Coordinate
        ''' </summary>
        ''' <returns>
        ''' Cropped Y2 Coordinate String returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped Y2 Coordinate"),Browsable(False)> _
        Public ReadOnly Property CroppedY2() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(3)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped Width
        ''' </summary>
        ''' <returns>
        ''' Cropped Width String returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped Width"),Browsable(False)> _
        Public ReadOnly Property CroppedWidth() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(4)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Property to get the Cropped Height
        ''' </summary>
        ''' <returns>
        ''' Cropped Height string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get the Cropped Height"),Browsable(False)> _
        Public ReadOnly Property CroppedHeight() As String
            Get
                Try
                    Return CroppedCoordinatesString.Split(",")(5)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property
#End Region

        Private _bgOpacity As Integer = 6
        Private bgOpacityString As String = ".6"

        ''' <summary>
        ''' Property to get/set the opacity of outer image when cropping. Must be between 0 and 10
        ''' </summary>
        ''' <value>
        ''' Integer passed to the set method
        ''' Default Value: 6
        ''' </value>
        ''' <returns>
        ''' bgOpacity integer returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue(6), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the opacity of outer image when cropping. Must be between 0 and 10")> _
        Public Property bgOpacity() As Integer
            Get
                Return _bgOpacity
            End Get
            Set(ByVal value As Integer)
                If value > -1 AndAlso value < 11 Then
                    _bgOpacity = value
                    bgOpacityString = "." & 10 - value
                    If value = 0 Then
                        bgOpacityString = "1.0"
                    End If
                    If value = 10 Then
                        bgOpacityString = ".0"
                    End If
                End If
            End Set
        End Property

        Private _bgColor As String = "black"

        ''' <summary>
        ''' Property to get/set the color of background container
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: "black"
        ''' </value>
        ''' <returns>
        ''' bgColor string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue("black"), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the color of background container")> _
        Public Property bgColor() As String
            Get
                Return _bgColor
            End Get
            Set(ByVal value As String)
                _bgColor = value
            End Set
        End Property

        Private _aspectRatio As String = ""
        Private aspectRatioString As String = ""

        ''' <summary>
        ''' Property to get/set the aspect ratio of w/h (e.g. 1 for square)
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: ""
        ''' </value>
        ''' <returns>
        ''' aspectRatio string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue(""), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the aspect ratio of w/h (e.g. 1 for square)")> _
        Public Property aspectRatio() As String
            Get
                Return _aspectRatio
            End Get
            Set(ByVal value As String)
                _aspectRatio = value
                If value IsNot Nothing AndAlso value <> "" Then
                    aspectRatioString = "," & vbCrLf & "            aspectRatio:   " & value
                Else
                    aspectRatioString = ""
                End If
            End Set
        End Property

        Private _setSelect As String = ""
        Private setSelectString As String = ""

        ''' <summary>
        ''' Property to get/set an initial selection area
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: ""
        ''' </value>
        ''' <returns>
        ''' setSelect string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue(""), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set an initial selection area")> _
        Public Property setSelect() As String
            Get
                Return _setSelect
            End Get
            Set(ByVal value As String)
                _setSelect = value
                If value IsNot Nothing AndAlso value <> "" Then
                    setSelectString = "," & vbCrLf & "            setSelect:   " & value
                Else
                    setSelectString = ""
                End If
            End Set
        End Property

        Private _minSize As String = ""
        Private setMinSizeString As String = ""

        ''' <summary>
        ''' Property to get/set the minimum width/height (array [ w, h ]), use """" for unbounded dimension
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' minSize string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue(""), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the minimum width/height (array [ w, h ]), use """""""" for unbounded dimension")> _
        Public Property minSize() As String
            Get
                Return _minSize
            End Get
            Set(ByVal value As String)
                _minSize = value
                If value IsNot Nothing AndAlso value <> "" Then
                    setMinSizeString = "," & vbCrLf & "            minSize:   " & value
                Else
                    setMinSizeString = ""
                End If
            End Set
        End Property

        Private _maxSize As String = ""
        Private setMaxSizeString As String = ""

        ''' <summary>
        ''' Property to get/set the maximum width/height (array [ w, h ]), use """" for unbounded dimension
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' maxSize string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <DefaultValue(""), _
        Category("Appearance"), _
        PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the maximum width/height (array [ w, h ]), use """""""" for unbounded dimension")> _
        Public Property maxSize() As String
            Get
                Return _maxSize
            End Get
            Set(ByVal value As String)
                _maxSize = value
                If value IsNot Nothing AndAlso value <> "" Then
                    setMaxSizeString = "," & vbCrLf & "            maxSize:   " & value
                Else
                    setMaxSizeString = ""
                End If
            End Set
        End Property

        Private _onSelectFunctionName As String = "setHiddenField"

        ''' <summary>
        ''' Property to get/set the name of the JS function to handle the onSelect event
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' onSelectFunctionName string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <PersistenceMode(PersistenceMode.Attribute), _
        Description("Property to get/set the name of the JS function to handle the onSelect event")> _
        Public Property OnSelectFunctionName() As String
            Get
                Return _onSelectFunctionName
            End Get
            Set(ByVal value As String)
                _onSelectFunctionName = value
            End Set
        End Property
#End Region

        Public Event ImageCropped(ByRef croppedImage As Drawing.Image, ByVal contentType As String, ByVal format As System.Drawing.Imaging.ImageFormat)

        Private Sub ImageCropper_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If CroppedCoordinatesString IsNot Nothing AndAlso CroppedCoordinatesString.Trim <> "" Then
                Dim imgObj As returnImage = GetImageFromURL()
                RaiseEvent ImageCropped(CropBitmap(imgObj.img, Me.CroppedX1, Me.CroppedY1, Me.CroppedWidth, Me.CroppedHeight), imgObj.contentType, imgObj.format)
            End If
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIImageManager/jquery.Jcrop.css", "text/css", True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIImageManager/jquery.Jcrop.min.js", , True)
        End Sub

    Private Function GetImageFromURL() As returnImage
        Dim strURL As String = Me.ImageUrl
        If strURL.Startswith("~") Then strUrl = "/" & strurl
        Dim retVal As New returnImage
        Try
            If strURL.StartsWith("/") Then
                Dim host As String = Page.Request.Url.OriginalString
                If Page.Request.Url.AbsolutePath <> "" Then
                    host = host.Replace(Page.Request.Url.AbsolutePath, "")
                End If
                If Page.Request.Url.Query <> "" Then
                    host = host.Replace(Page.Request.Url.Query, "")
                End If
                If Page.Request.Url.Fragment <> "" Then
                    host = host.Replace(Page.Request.Url.Fragment, "")
                End If
                strURL = host & strURL
            End If
            'If Not strURL.tolower().startswith("http://") Then strURL = "http://" & strURL
            Dim request As HttpWebRequest = HttpWebRequest.Create(strURL)
            request.Timeout = 5000
            request.ReadWriteTimeout = 20000
            Dim response As HttpWebResponse = request.GetResponse
            retVal.contentType = response.ContentType
            retVal.img = Drawing.Image.FromStream(response.GetResponseStream)
        Catch ex As Exception
            retVal = Nothing
        End Try
        Return retVal
    End Function

        Private Function CropBitmap(ByRef bmp As Bitmap, ByVal cropX As Integer, ByVal cropY As Integer, ByVal cropWidth As Integer, ByVal cropHeight As Integer) As Bitmap
            Dim rect As New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)
            Return cropped
        End Function

	Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		MyBase.Render(writer)
		writer.Write("<input type=""hidden"" name=""" & Me.ID & "_hidden"" id=""" & Me.ID & "_hidden"" />")
	End Sub

	Private Function myInsertScript() As String
		Return "var img = $('#myCropper'); // Get my img elem " & vbCrLf &
"var pic_real_width, pic_real_height; " & vbCrLf &
"$(""<img/>"") // Make in memory copy of image to avoid css issues " & vbCrLf &
"    .attr(""src"", $(img).attr(""src"")) " & vbCrLf &
"    .load(function() { " & vbCrLf &
"        pic_real_width = this.width;   // Note: $(this).width() will not " & vbCrLf &
"        pic_real_height = this.height; // work for in memory images. " & vbCrLf &
"		$('#myCropper').Jcrop({ " & vbCrLf &
"				onSelect: doCropSelect,             " & vbCrLf &
"				bgColor: 'black', " & vbCrLf &
"				bgOpacity: .6, " & vbCrLf &
"				trueSize: [pic_real_width,pic_real_height] " & vbCrLf &
"		}); " & vbCrLf &
"	}); " & vbCrLf
	End Function

	Private Sub ImageCropper_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
		jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, myInsertScript)
	End Sub

	Private Class returnImage
		Public img As Drawing.Image
		Private _contentType As String
		Public Property contentType() As String
			Get
				Return _contentType
			End Get
			Set(ByVal value As String)
				_contentType = value
				setFormat()
			End Set
		End Property

		Private _format As System.Drawing.Imaging.ImageFormat

		''' <summary>
		''' Property to get the image format
		''' </summary>
		''' <returns>
		''' format returned by the get method
		''' </returns>
		''' <remarks></remarks>
		<System.ComponentModel.Description("Property to get the image format")>
		Public ReadOnly Property format() As System.Drawing.Imaging.ImageFormat
			Get
				Return _format
			End Get
		End Property

		Private Sub setFormat()
			Select Case _contentType
				Case "image/jpeg", "image/jpg"
					_format = System.Drawing.Imaging.ImageFormat.Jpeg
				Case "image/gif"
					_format = System.Drawing.Imaging.ImageFormat.Gif
				Case "image/png"
					_format = System.Drawing.Imaging.ImageFormat.Png
				Case "image/bmp"
					_format = System.Drawing.Imaging.ImageFormat.Png
					_contentType = "image/png"
				Case Else
					_format = System.Drawing.Imaging.ImageFormat.Png
			End Select
		End Sub
	End Class
End Class