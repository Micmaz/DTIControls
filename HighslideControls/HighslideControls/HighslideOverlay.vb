''' <summary>
''' control for displaying a Highslide overly on a Highslide dialog element
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class HighslideOverlay
    Inherits LiteralControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class HighslideOverlay
        Inherits LiteralControl
#End If
        Private _className As String

        ''' <summary>
        ''' Apply a class name for styling the overlay.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Apply a class name for styling the overlay.")> _
        Public Property className() As String
            Get
                Return _className
            End Get
            Set(ByVal value As String)
                _className = value
            End Set
        End Property

        Private _fade As String

        ''' <summary>
        ''' Whether the overlay should fade in and out. Possible values 0/false, 1/true, 2. 
        ''' Fading in and out is problematic in IE if you put alphatransparent PNGs in the overlay, 
        ''' either through AlphaImageLoader or not. In this case, set the fade to 2 to skip fading in IE only. 
        ''' An example can be seen at example-no-border.html. hideOnMouseOut (bool) Defines whether the overlay 
        ''' should be hidden when the mouse leaves the full-size image. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Whether the overlay should fade in and out. Possible values 0/false, 1/true, 2.      Fading in and out is problematic in IE if you put alphatransparent PNGs in the overlay,      either through AlphaImageLoader or not. In this case, set the fade to 2 to skip fading in IE only.      An example can be seen at example-no-border.html. hideOnMouseOut (bool) Defines whether the overlay      should be hidden when the mouse leaves the full-size image.")> _
        Public Property fade() As String
            Get
                Return _fade
            End Get
            Set(ByVal value As String)
                _fade = value
            End Set
        End Property

        Private _overlayId As String = Me.ClientID

        ''' <summary>
        ''' The id of the overlay div defined in your page's source code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The id of the overlay div defined in your page's source code.")> _
        Public Property overlayId() As String
            Get
                Return _overlayId
            End Get
            Set(ByVal value As String)
                _overlayId = value
            End Set
        End Property

        Private _offsetX As Integer = 0

        ''' <summary>
        ''' The number of pixels to offset the overlay in the horizontal
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The number of pixels to offset the overlay in the horizontal")> _
        Public Property offsetX() As Integer
            Get
                Return _offsetX
            End Get
            Set(ByVal value As Integer)
                _offsetX = value
            End Set
        End Property

        Private _offsetY As Integer = 0

        ''' <summary>
        ''' The number of pixels to offset the overlay in the vertical
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The number of pixels to offset the overlay in the vertical")> _
        Public Property offsetY() As Integer
            Get
                Return _offsetY
            End Get
            Set(ByVal value As Integer)
                _offsetY = value
            End Set
        End Property

        Private _opacity As Double = 1

        ''' <summary>
        ''' Can be a float number between 0 and 1 (fully opaque).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Can be a float number between 0 and 1 (fully opaque).")> _
        Public Property opacity() As Double
            Get
                Return _opacity
            End Get
            Set(ByVal value As Double)
                _opacity = value
            End Set
        End Property

        Private _pos As String

        ''' <summary>
        ''' Specifies where the overlay will appear related to the full-size image. 
        ''' The first word specifies the vertical position and can be above, top, middle, bottom or below, 
        ''' and the second word can be left, center, right. In addition, the position can be the single word 
        ''' positions leftpanel or rightpanel. If you want to put some air between the overlay and the edge 
        ''' of the image, you use regular CSS margins on the overlay div. You can also offset the overlay 
        ''' by using position:relative in it's CSS.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Specifies where the overlay will appear related to the full-size image.      The first word specifies the vertical position and can be above, top, middle, bottom or below,      and the second word can be left, center, right. In addition, the position can be the single word      positions leftpanel or rightpanel. If you want to put some air between the overlay and the edge      of the image, you use regular CSS margins on the overlay div. You can also offset the overlay      by using position:relative in it's CSS.")> _
        Public Property position() As String
            Get
                Return _pos
            End Get
            Set(ByVal value As String)
                _pos = value
            End Set
        End Property

        Private _relativeTo As String

        ''' <summary>
        ''' Can be 'viewport' or 'expander'. Overlays in most positions can be rendered relative to 
        ''' the viewport rather than the default, which is relative to the expander. When relative to 
        ''' the viewport, the overlays stay fixed on scrolling. This option requires that the 
        ''' 'Viewport overlays' component be activated in the Configurator.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Can be 'viewport' or 'expander'. Overlays in most positions can be rendered relative to      the viewport rather than the default, which is relative to the expander. When relative to      the viewport, the overlays stay fixed on scrolling. This option requires that the      'Viewport overlays' component be activated in the Configurator.")> _
        Public Property relativeTo() As String
            Get
                Return _relativeTo
            End Get
            Set(ByVal value As String)
                _relativeTo = value
            End Set
        End Property


        Private _slideshowGroup As String

        ''' <summary>
        ''' Refers to a hs.slideshowGroup that will use this overlay.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Refers to a hs.slideshowGroup that will use this overlay.")> _
        Public Property slideshowGroup() As String
            Get
                Return _slideshowGroup
            End Get
            Set(ByVal value As String)
                _slideshowGroup = value
            End Set
        End Property

        Private _thumbnailId As String

        ''' <summary>
        ''' Either a string specifying the id of the thumbnail, or you can set it to null to apply 
        ''' the overlay on all thumbnails.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Either a string specifying the id of the thumbnail, or you can set it to null to apply      the overlay on all thumbnails.")> _
        Public Property thumbnailId() As String
            Get
                Return _thumbnailId
            End Get
            Set(ByVal value As String)
                _thumbnailId = value
            End Set
        End Property

        Private _width As String

        ''' <summary>
        ''' The width of the overlay can be specified either as a pixel value or as a percentage, 
        ''' like 100px or 100%. However, overlays in the leftpanel or rightpanel positions require 
        ''' a fixed pixel width, or they will fall back to the default 200px width.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The width of the overlay can be specified either as a pixel value or as a percentage,      like 100px or 100%. However, overlays in the leftpanel or rightpanel positions require      a fixed pixel width, or they will fall back to the default 200px width.")> _
        Public Property width() As String
            Get
                Return _width
            End Get
            Set(ByVal value As String)
                _width = value
            End Set
        End Property

        Private _useOnHtml As Boolean
        Private _useOnHtmlSet As Boolean = False

        ''' <summary>
        ''' By defalut overlays are not added to HTML content. Set useOnHtml to true to allow this. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("By defalut overlays are not added to HTML content. Set useOnHtml to true to allow this.")> _
        Public Property useOnHtml() As Boolean
            Get
                Return _useOnHtml
            End Get
            Set(ByVal value As Boolean)
                _useOnHtml = value
                _useOnHtmlSet = True
            End Set
        End Property


        Private Sub HighslideOverlay_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim script As String = "hs.registerOverlay({"

            If Not Me._className Is Nothing Then
                script &= "classname:'" & _className & "',"
            End If
            If Not Me._fade Is Nothing Then
                script &= "fade:'" & _fade & "',"
            End If
            If Me._offsetX <> 0 Then
                script &= "offsetX:" & _offsetX & ","
            End If
            If Me._offsetY <> 0 Then
                script &= "offsetY:" & _offsetY & ","
            End If
            If Me._opacity < 1 AndAlso Me._opacity >= 0 Then
                script &= "opacity:" & _opacity & ","
            End If
            If Not Me._pos Is Nothing Then
                script &= "position:'" & _pos & "',"
            End If
            If Not Me._relativeTo Is Nothing Then
                script &= "relativeTo:'" & _relativeTo & "',"
            End If
            If Not Me._slideshowGroup Is Nothing Then
                script &= "slideshowGroup:'" & _slideshowGroup & "',"
            End If
            If Not Me._thumbnailId Is Nothing Then
                script &= "thumbnailId:'" & _thumbnailId & "',"
            End If
            If _useOnHtmlSet Then
                script &= "useOnHtml:" & _useOnHtml.ToString.ToLower & ","
            End If
            script &= "overlayId:'" & _overlayId & "'});"

            Page.ClientScript.RegisterStartupScript(Me.GetType, Me.ClientID, script, True)
        End Sub
    End Class
