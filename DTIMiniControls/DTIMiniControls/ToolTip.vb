<ToolboxData("<{0}:ToolTip ID=""ToolTip1"" runat=""server""> </{0}:ToolTip>"), ComponentModel.ToolboxItem(False), Obsolete("This Method is Deprecated, use JqueryUIControls.ToolTip instead.")> _
Public Class ToolTip
    Inherits Panel

#Region "Properties"
    Private _text As String = ""

    ''' <summary>
    ''' Text inside the html element the causes the tootip to show
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text inside the html element the causes the tootip to show")> _
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Private _title As String = ""

    ''' <summary>
    '''  displayed in the top of the tooltip
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("displayed in the top of the tooltip")> _
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _targetControlId As String = ""

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Property TargetControlID() As String
        Get
            Return _targetControlId
        End Get
        Set(ByVal value As String)
            _targetControlId = value
        End Set
    End Property

    Private _href As String = "#"

    ''' <summary>
    ''' if content is local this will make the link usable as well as displaying a 
    ''' tooltip.  If it is an iframe (ie not local) it contains the page to display
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("if content is local this will make the link usable as well as displaying a    tooltip. If it is an iframe (ie not local) it contains the page to display")> _
    Public Property href() As String
        Get
            Return _href
        End Get
        Set(ByVal value As String)
            _href = value
        End Set
    End Property

    Private _sticky As Boolean = False

    ''' <summary>
    ''' keep visible until manually closed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("keep visible until manually closed")> _
    Public Property Sticky() As Boolean
        Get
            Return _sticky
        End Get
        Set(ByVal value As Boolean)
            _sticky = value
        End Set
    End Property

    Private _zIndexToolTip As Integer = 97

    ''' <summary>
    ''' Sets the z-index style property of the clueTip
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the z-index style property of the clueTip")> _
    Public Property ZIndexToolTip() As Integer
        Get
            Return _zIndexToolTip
        End Get
        Set(ByVal value As Integer)
            _zIndexToolTip = value
        End Set
    End Property

    Private _positionToolTip As PositionBy = PositionBy.auto

    ''' <summary>
    ''' Sets the type of positioning.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Available options are 'auto', 'mouse', 'bottomTop', 'fixed'. 
    ''' Change to 'mouse' if you want to override positioning by element and position the 
    ''' clueTip based on where the mouse is instead. Change to 'bottomTop' if you want 
    ''' positioning to begin below the mouse when there is room or above if not -- rather than 
    ''' right or left of the elemnent and flush with element's top Change to 'fixed' if you 
    ''' want the clueTip to appear in exactly the same location relative to the linked element no 
    ''' matter where it appears on the page. Use 'fixed' at your own risk.
    ''' </remarks>
    <System.ComponentModel.Description("Sets the type of positioning.")> _
    Public Property PositionTooltip() As PositionBy
        Get
            Return _positionToolTip
        End Get
        Set(ByVal value As PositionBy)
            _positionToolTip = value
        End Set
    End Property

    Private _topOffset As Integer = 15

    ''' <summary>
    ''' Number of px to offset clueTip from top of invoking element.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' For positionBy "auto", "mouse", and "bottomTop", the number will be added to the 
    ''' clueTip's "top" value if the clueTip appears below the invoking element and subtracted 
    ''' from it if the clueTip appears above. For positionBy "fixed", the number will always be 
    ''' added to the "top" value, offsetting the clueTip from the top of the invoking element.
    ''' </remarks>
    <System.ComponentModel.Description("Number of px to offset clueTip from top of invoking element.")> _
    Public Property TopOffset() As Integer
        Get
            Return _topOffset
        End Get
        Set(ByVal value As Integer)
            _topOffset = value
        End Set
    End Property

    Private _leftOffset As Integer = 15

    ''' <summary>
    ''' Number of px to offset clueTip from left of invoking element.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' For positionBy "auto", "mouse", and "bottomTop", the number will be added to 
    ''' clueTip's "left" value if the clueTip appears to the right of the invoking element 
    ''' and subtracted if the clueTip appears to the left. For positionBy "fixed", the number 
    ''' will always be added to the "left" value of the clueTip, offsetting it from the right 
    ''' side of the invoking element.
    ''' </remarks>
    <System.ComponentModel.Description("Number of px to offset clueTip from left of invoking element.")> _
    Public Property LeftOffset() As Integer
        Get
            Return _leftOffset
        End Get
        Set(ByVal value As Integer)
            _leftOffset = value
        End Set
    End Property

    Private _mouseOutClose As Boolean = False

    ''' <summary>
    ''' close when clueTip is moused out
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("close when clueTip is moused out")> _
    Public Property MouseOutClose() As Boolean
        Get
            Return _mouseOutClose
        End Get
        Set(ByVal value As Boolean)
            _mouseOutClose = value
        End Set
    End Property

    Private _clickThrough As Boolean = True

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Private Property ClickThrough() As Boolean
        Get
            Return _clickThrough
        End Get
        Set(ByVal value As Boolean)
            _clickThrough = value
        End Set
    End Property

    Private _local As Boolean = True

    ''' <summary>
    ''' Whether to use content from the same page for the clueTip's body or it is an iframe
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Whether to use content from the same page for the clueTip's body or it is an iframe")> _
    Public Property Local() As Boolean
        Get
            Return _local.ToString.ToLower
        End Get
        Set(ByVal value As Boolean)
            _local = value
        End Set
    End Property

    Private _hideLocal As Boolean = True

    ''' <summary>
    ''' If local option is set to true, this determines whether local content
    ''' to be shown in clueTip should be hidden at its original location
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If local option is set to true, this determines whether local content   to be shown in clueTip should be hidden at its original location")> _
    Public Property HideLocal() As Boolean
        Get
            Return _hideLocal.ToString.ToLower
        End Get
        Set(ByVal value As Boolean)
            _hideLocal = value
        End Set
    End Property

    Private _showTitle As Boolean = True

    ''' <summary>
    ''' show title bar of the clueTip, even if title attribute not set
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("show title bar of the clueTip, even if title attribute not set")> _
    Public Property ShowTitle() As Boolean
        Get
            Return _showTitle.ToString.ToLower
        End Get
        Set(ByVal value As Boolean)
            _showTitle = value
        End Set
    End Property

    Private _width As Unit = New Unit(275, UnitType.Pixel)

    ''' <summary>
    ''' The width of the clueTip only uses its value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The width of the clueTip only uses its value")> _
    Public Overrides Property Width() As Unit
        Get
            Return _width
        End Get
        Set(ByVal value As Unit)
            _width = value
        End Set
    End Property

    Private _height As Unit = New Unit(0, UnitType.Pixel)

    ''' <summary>
    ''' he height of the clueTip only uses its value
    ''' Setting a specific height also sets the tootip to "overflow:auto"
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("he height of the clueTip only uses its value   Setting a specific height also sets the tootip to ""overflow:auto""")> _
    Public Overrides Property Height() As Unit
        Get
            Return _height
        End Get
        Set(ByVal value As Unit)
            _height = value
        End Set
    End Property

    Private _htmlTag As String = "a"

    ''' <summary>
    ''' type of tag the invoking element is
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("type of tag the invoking element is")> _
    Public Property HtmlTag() As String
        Get
            Return _htmlTag
        End Get
        Set(ByVal value As String)
            _htmlTag = value
        End Set
    End Property

    Private _closePosition As Position = Position.top

    ''' <summary>
    ''' location of close text for sticky cluetips; can be 'top' or 'bottom' or 'title'
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("location of close text for sticky cluetips; can be 'top' or 'bottom' or 'title'")> _
    Public Property ClosePosition() As Position
        Get
            Return _closePosition
        End Get
        Set(ByVal value As Position)
            _closePosition = value
        End Set
    End Property

    Private _closeText As String = "Close"

    ''' <summary>
    ''' text (or HTML) to to be clicked to close sticky clueTips
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("text (or HTML) to to be clicked to close sticky clueTips")> _
    Public Property CloseText() As String
        Get
            Return _closeText
        End Get
        Set(ByVal value As String)
            _closeText = value
        End Set
    End Property

    Private _cssTheme As String = ""

    ''' <summary>
    ''' class added to outermost clueTip div in the form of 'cluetip-' + clueTipClass.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' this is also used for a "directional" class on the same div, depending on where 
    ''' the clueTip is in relation to the invoking element. The class appears in the form 
    ''' of 'cluetip-' + direction + cluetipClass. this allows you to create your own clueTip 
    ''' theme in a separate CSS file or use one of the three pre-packaged 
    ''' themes: default, jtip, or rounded.
    ''' </remarks>
    <System.ComponentModel.Description("class added to outermost clueTip div in the form of 'cluetip-' + clueTipClass.")> _
    Public Property CssTheme() As String
        Get
            Return _cssTheme
        End Get
        Set(ByVal value As String)
            _cssTheme = value
        End Set
    End Property

    Private _arrows As Boolean = False

    ''' <summary>
    ''' if true, displays arrow on appropriate side of clueTip.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' this option displays a div containing an arrow background image. Arrow images are set 
    ''' using the background-image property in the CSS. The direction of the arrow changes depending 
    ''' on which side of the invoking element the clueTip appears. The arrows option sets the 
    ''' background-position of the cluetip div so that the arrow will accurately point to the invoking 
    ''' element, regardless of where it appears in relation to it.
    ''' </remarks>
    <System.ComponentModel.Description("if true, displays arrow on appropriate side of clueTip.")> _
    Public Property Arrows() As Boolean
        Get
            Return _arrows
        End Get
        Set(ByVal value As Boolean)
            _arrows = value
        End Set
    End Property

    Private _dropShadow As Boolean = False

    ''' <summary>
    ''' set to false if you don't want the drop-shadow effect on the clueTip
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("set to false if you don't want the drop-shadow effect on the clueTip")> _
    Public Property DropShadow() As Boolean
        Get
            Return _dropShadow
        End Get
        Set(ByVal value As Boolean)
            _dropShadow = value
        End Set
    End Property

    Private _dropShadowSteps As Integer = 6

    ''' <summary>
    ''' adjusts the size of the drop shadow
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("adjusts the size of the drop shadow")> _
    Public Property DropShadowSteps() As Integer
        Get
            Return _dropShadowSteps
        End Get
        Set(ByVal value As Integer)
            _dropShadowSteps = value
        End Set
    End Property

    Private _activation As UserActivation = UserActivation.hover

    ''' <summary>
    ''' set to 'click' to force user to click to show clueTip
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("set to 'click' to force user to click to show clueTip")> _
    Public Property Activation() As UserActivation
        Get
            Return _activation
        End Get
        Set(ByVal value As UserActivation)
            _activation = value
        End Set
    End Property

    Private _delayedClose As Integer = 0

    ''' <summary>
    ''' close clueTip on a timed delay (experimental)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("close clueTip on a timed delay (experimental)")> _
    Public Property DelayedClose() As Integer
        Get
            Return _delayedClose
        End Get
        Set(ByVal value As Integer)
            _delayedClose = value
        End Set
    End Property

    Private _truncate As Integer = 0

    ''' <summary>
    ''' number of characters to truncate clueTip's contents. if 0, no truncation occurs
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("number of characters to truncate clueTip's contents. if 0, no truncation occurs")> _
    Public Property Truncate() As Integer
        Get
            Return _truncate
        End Get
        Set(ByVal value As Integer)
            _truncate = value
        End Set
    End Property

    Private _openEffect As Open = Open.show

    ''' <summary>
    ''' can be 'show' or 'slideDown' or 'fadeIn'
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("can be 'show' or 'slideDown' or 'fadeIn'")> _
    Public Property OpenEffect() As Open
        Get
            Return _openEffect
        End Get
        Set(ByVal value As Open)
            _openEffect = value
        End Set
    End Property

    Private _openSpeed As Integer = 0

    ''' <summary>
    ''' how fast the tootip opens in ms
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("how fast the tootip opens in ms")> _
    Public Property OpenSpeed() As Integer
        Get
            Return _openSpeed
        End Get
        Set(ByVal value As Integer)
            _openSpeed = value
        End Set
    End Property

    Private _hoverClass As String = ""

    ''' <summary>
    ''' class applied to the invoking element onmouseover and removed onmouseout
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("class applied to the invoking element onmouseover and removed onmouseout")> _
    Public Property HoverClass() As String
        Get
            Return _hoverClass
        End Get
        Set(ByVal value As String)
            _hoverClass = value
        End Set
    End Property

    Private _tracking As Boolean = False

    ''' <summary>
    ''' if true, clueTip will track mouse movement (experimental)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("if true, clueTip will track mouse movement (experimental)")> _
    Public Property MouseTracking() As Boolean
        Get
            Return _tracking
        End Get
        Set(ByVal value As Boolean)
            _tracking = True
        End Set
    End Property

    Private _onActivate As String = ""

    ''' <summary>
    ''' function to run just before clueTip is shown.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("function to run just before clueTip is shown.")> _
    Public Property onActivate() As String
        Get
            Return _onActivate
        End Get
        Set(ByVal value As String)
            _onActivate = value
        End Set
    End Property

    Private _onShow As String = ""

    ''' <summary>
    ''' function to run just after clueTip is shown.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("function to run just after clueTip is shown.")> _
    Public Property onShow() As String
        Get
            Return _onShow
        End Get
        Set(ByVal value As String)
            _onShow = value
        End Set
    End Property
#End Region

#Region "Enums"
    Public Enum Position
        top
        bottom
        title
    End Enum

    Public Enum UserActivation
        hover
        click
    End Enum

    Public Enum Open
        show
        slideDown
        fadeIn
    End Enum

    Public Enum PositionBy
        auto
        mouse
        bottomTop
        fixed
    End Enum
#End Region

    Private linkCSS = ""
    Dim linkRel As String = ""
    Dim linkHref As String = ""
    'Dim linkTitle As String = ""
    Dim targetControl As Control = Nothing

    Private Sub ToolTip_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jqueryCluetip.css", "text/css")
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.cluetip.js", , True)

        'linkCSS = Me.CssClass
        'Me.CssClass = Me.ClientID & "Css"

        If TargetControlID <> "" Then
            targetControl = FindControl(TargetControlID)
        End If

        If Local Then
            linkRel = "#" & Me.ClientID '"div." & Me.CssClass
        Else
            linkRel = href
        End If

        If HtmlTag = "a" OrElse Not Local Then
            linkHref = " href=""" & href & """"
        End If
        If linkCSS <> "" Then
            linkCSS = " class=""" & linkCSS & """"
        End If
        If targetControl IsNot Nothing Then
            Try
                Dim t As WebControl = CType(targetControl, WebControl)
                t.Attributes.Add("rel", linkRel)
                If Title <> "" Then
                    t.Attributes.Add("title", Title)
                End If
            Catch ex As Exception
                Try
                    Dim t As HtmlControl = CType(targetControl, HtmlControl)
                    t.Attributes.Add("rel", linkRel)
                    If Title <> "" Then
                        t.Attributes.Add("title", Title)
                    End If
                Catch exc As Exception
                    Throw New Exception("Target Control must be either a Webcontrol or a HtmlControl")
                End Try
            End Try
        Else
            linkRel = " rel=""" & linkRel & """"
            If Title <> "" Then
                Title = " title=""" & Title & """"
            End If
            ClickThrough = (href <> "#")
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim str As String = ""
        If targetControl Is Nothing Then
            If Local Then
                str &= "<" & HtmlTag & " id=""" & Me.ClientID & "_link""" & linkCSS & linkHref & Title & linkRel & ">" & Text & "</" & HtmlTag & ">" & vbCrLf
            Else
                str &= "<a " & " id=""" & Me.ClientID & "_link""" & linkCSS & linkHref & Title & linkRel & ">" & Text & "</a>" & vbCrLf
                Me.Style.Add("display", "none")
            End If
        End If

        str &= "<script type=""text/javascript""> " & vbCrLf
        str &= "    $(function(){ " & vbCrLf
        If TargetControlID = "" Then
            str &= "        $('#" & Me.ClientID & "_link').cluetip({ " & vbCrLf
        Else
            str &= "        $('#" & targetControl.ClientID & "').cluetip({ " & vbCrLf
        End If
        If Local Then
            str &= "            local: true," & vbCrLf
        End If
        If Not HideLocal Then
            str &= "            hideLocal: false," & vbCrLf
        Else
            str &= "            hideLocal: true," & vbCrLf
        End If
        If Not ShowTitle OrElse Title = "" Then
            str &= "            showTitle: false," & vbCrLf
        End If
        If ZIndexToolTip <> 97 Then
            str &= "            cluezIndex: " & ZIndexToolTip & "," & vbCrLf
        End If
        If TopOffset <> 15 Then
            str &= "            topOffset: " & TopOffset & "," & vbCrLf
        End If
        If LeftOffset <> 15 Then
            str &= "            leftOffset: " & LeftOffset & "," & vbCrLf
        End If
        If (ClickThrough AndAlso Local) OrElse (ClickThrough AndAlso targetControl IsNot Nothing) Then
            str &= "            clickThrough: true," & vbCrLf
        End If
        If Sticky Then
            str &= "            sticky: true," & vbCrLf
        End If
        If PositionTooltip <> PositionBy.auto Then
            str &= "            positionBy: '" & getName(PositionTooltip) & "'," & vbCrLf
        End If
        If MouseOutClose Then
            str &= "            mouseOutClose: true," & vbCrLf
        End If
        If ClosePosition <> Position.top Then
            str &= "            closePosition: '" & getName(ClosePosition) & "'," & vbCrLf
        End If
        If CloseText <> "Close" Then
            str &= "            closeText: '" & CloseText & "'," & vbCrLf
        End If
        If CssTheme <> "" Then
            str &= "            cluetipClass: '" & CssTheme & "'," & vbCrLf
        End If
        If HoverClass <> "" Then
            str &= "            hoverClass: '" & HoverClass & "'," & vbCrLf
        End If
        If Arrows Then
            str &= "            arrows: true," & vbCrLf
        End If
        If Not DropShadow Then
            str &= "            dropShadow: false," & vbCrLf
        End If
        If DropShadowSteps <> 6 Then
            str &= "            dropShadowSteps: " & DropShadowSteps & "," & vbCrLf
        End If
        If Truncate <> 0 Then
            str &= "            truncate: " & Truncate & "," & vbCrLf
        End If
        If Activation <> UserActivation.hover Then
            str &= "            activation: '" & getName(Activation) & "'," & vbCrLf
        End If
        If DelayedClose <> 0 Then
            str &= "            delayedClose: " & DelayedClose & "," & vbCrLf
        End If
        If Width.Value <> 275 Then
            str &= "            width: " & Width.Value & "," & vbCrLf
        End If
        If Height.Value <> 0 Then
            str &= "            height: '" & Height.ToString & "'," & vbCrLf
        End If
        If MouseTracking Then
            str &= "            tracking: true," & vbCrLf
        End If
        If OpenEffect <> Open.show OrElse OpenSpeed <> 0 Then
            str &= "            fx {" & vbCrLf
            str &= "                        open: '" & getName(OpenEffect) & "'," & vbCrLf
            str &= "                        openSpeed: '" & OpenSpeed & "'" & vbCrLf
            str &= "            }," & vbCrLf
        End If
        If onActivate <> "" Then
            str &= "            onActivate: " & onActivate & "," & vbCrLf
        End If
        If onShow <> "" Then
            str &= "            onShow: " & onShow & "," & vbCrLf
        End If

        str = str.Trim(vbLf).Trim(vbCr).Trim(",") & vbCrLf
        str &= "        }); " & vbCrLf
        str &= "    }); " & vbCrLf
        str &= "    </script> " & vbCrLf



        writer.Write(str)

        If Local Then _
            MyBase.Render(writer)
    End Sub

    Private Function getName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function
End Class
