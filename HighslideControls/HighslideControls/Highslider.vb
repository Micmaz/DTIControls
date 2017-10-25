Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' Control to display a Highslide dialog element
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class Highslider
    Inherits PlaceHolder
#Else
    <DefaultProperty("DisplayText"), PersistChildren(False), ParseChildren(True, "DisplayText"), ToolboxData("<{0}:Highslider runat=server></{0}:Highslider>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Highslider
        Inherits PlaceHolder
#End If
        Protected pnlCaptionControls As New Panel
        Protected pnlContentControls As New Panel
        Protected litAnchor As New Literal

        Private _myHeader As HighslideHeaderControl

#Region "Properties"

        ''' <summary>
        ''' Header
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Header")> _
        Public ReadOnly Property myHeader() As HighslideHeaderControl
            Get
                If _myHeader Is Nothing Then
                    _myHeader = HighslideControls.HighslideHeaderControl.addToPage(Me.Page)
                End If
                Return _myHeader
            End Get
        End Property

        Private _ensure_header As Boolean = True
        Public Property EnsureHeader() As Boolean
            Get
                Return _ensure_header
            End Get
            Set(ByVal value As Boolean)
                _ensure_header = value
            End Set
        End Property

        ''' <summary>
        ''' Hashtable containing overriding highslide variables, see http://highslide.com/ref/
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Hashtable containing overriding highslide variables, see http://highslide.com/ref/")> _
        Public ReadOnly Property HighslideVariables() As Hashtable
            Get
                If ViewState("HighslideVariables") Is Nothing Then
                    ViewState("HighslideVariables") = New Hashtable
                End If

                Dim _highslideVariables As Hashtable = ViewState("HighslideVariables")
                Return _highslideVariables
            End Get
        End Property

        ''' <summary>
        ''' The anchor text displayed to the user.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The anchor text displayed to the user."),PersistenceMode(PersistenceMode.InnerDefaultProperty)> _
        Public Property DisplayText() As String
            Get
                Dim s As String = CStr(ViewState("DisplayText"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("DisplayText") = Value
            End Set
        End Property

        ''' <summary>
        ''' In image mode, the URL of the fullsize image.  In iframe mode, the URL of the expanded iframe.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("In image mode, the URL of the fullsize image. In iframe mode, the URL of the expanded iframe.")> _
        Public Overridable Property ExpandURL() As String
            Get
                Dim s As String = CStr(ViewState("ExpandURL"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("ExpandURL") = Value
            End Set
        End Property

        ''' <summary>
        ''' The URL of the thumbnail image
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The URL of the thumbnail image")> _
        Public Overridable Property ThumbURL() As String
            Get
                Dim s As String = CStr(ViewState("ThumbURL"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("ThumbURL") = Value
            End Set
        End Property

        ''' <summary>
        ''' Thumbnail width
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Thumbnail width")> _
        Public Property ThumbWidth() As String
            Get
                Dim s As String = CStr(ViewState("ThumbWidth"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("ThumbWidth") = Value
            End Set
        End Property

        ''' <summary>
        ''' Set this property to true to make HS frames fixed on a page
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Set this property to true to make HS frames fixed on a page")> _
        Public Property isFixed() As Boolean
            Get
                Return ViewState("isFixed")
            End Get
            Set(ByVal value As Boolean)
                ViewState("isFixed") = value
            End Set
        End Property

        ''' <summary>
        ''' Thumbnail height
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Thumbnail height")> _
        Public Property ThumbHeight() As String
            Get
                Dim s As String = CStr(ViewState("ThumbHeight"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("ThumbHeight") = Value
            End Set
        End Property

        Private captionLabel As Label

        ''' <summary>
        ''' Caption
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Caption")> _
        Public Property Caption() As String
            Get
                If captionLabel Is Nothing Then
                    captionLabel = New Label
                    CaptionControls.Add(captionLabel)
                End If
                Return captionLabel.Text
            End Get
            Set(ByVal Value As String)
                If captionLabel Is Nothing Then
                    captionLabel = New Label
                    CaptionControls.Add(captionLabel)
                End If
                captionLabel.Text = Value
            End Set
        End Property

        ''' <summary>
        ''' Caption Control Collection
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Caption Control Collection")> _
        Public ReadOnly Property CaptionControls() As ControlCollection
            Get
                Return pnlCaptionControls.Controls
            End Get
        End Property

        ''' <summary>
        ''' Content Control Collection for HTML mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Content Control Collection for HTML mode")> _
        Public ReadOnly Property ContentControls() As ControlCollection
            Get
                Return pnlContentControls.Controls
            End Get
        End Property

        ''' <summary>
        ''' Outline Scheme
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Outline Scheme")> _
        Public Property Outline_Scheme() As Highslide_Outline_Scheme
            Get
                If ViewState("Outline_Scheme") Is Nothing Then
                    Return Highslide_Outline_Scheme.RoundedWhite
                Else
                    Return ViewState("Outline_Scheme")
                End If
            End Get
            Set(ByVal Value As Highslide_Outline_Scheme)
                ViewState("Outline_Scheme") = Value

                If HighslideVariables.Contains("outlineType") Then
                    HighslideVariables.Item("outlineType") = getOutlineText(Value)
                Else
                    HighslideVariables("outlineType") = getOutlineText(Value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Display mode 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Display mode")> _
        Public Overridable Property HighslideDisplayMode() As HighslideDisplayModes
            Get
                If ViewState("HighslideDisplayMode") Is Nothing Then
                    ViewState("HighslideDisplayMode") = HighslideDisplayModes.Iframe
                End If
                Return ViewState("HighslideDisplayMode")
            End Get
            Set(ByVal value As HighslideDisplayModes)
                ViewState("HighslideDisplayMode") = value
            End Set
        End Property

        Private ReadOnly Property innerValue() As String
            Get
                If thumbnail <> "" Then
                    Return thumbnail
                Else
                    Return DisplayText
                End If
            End Get
        End Property

        Public ReadOnly Property hrefValue() As String
            Get
                If HighslideDisplayMode = HighslideDisplayModes.HTML Then
                    Return "index.htm"
                Else
                    Return ExpandURL
                End If
            End Get
        End Property

        Private ReadOnly Property onclickValue() As String
            Get
                Dim returnValue As String = ""
                If HighslideDisplayMode = HighslideDisplayModes.Image Then
                    returnValue = "return hs.expand(this" & HighslideVariablesString
                Else
                    returnValue = "return hs.htmlExpand(this" & HighslideVariablesString
                End If
                returnValue &= ")"
                If Not Me.Page Is Nothing AndAlso myHeader.isInnerFrame Then
                    returnValue = returnValue.Replace("return hs.", _
                        "parent.window.focus();parent.window.iframe = getIframe(this);return parent.window.hs.")
                End If
                Return returnValue
            End Get
        End Property

        Private ReadOnly Property HighslideHashTableString() As String
            Get
                Dim returnValue As String = ""
                For Each variableKey As String In HighslideVariables.Keys
                    If Double.TryParse(HighslideVariables.Item(variableKey), New Double) OrElse _
                        Boolean.TryParse(HighslideVariables.Item(variableKey), New Boolean) = True OrElse _
                        DirectCast(HighslideVariables.Item(variableKey), String).ToLower = "true" OrElse _
                        DirectCast(HighslideVariables.Item(variableKey), String).ToLower = "false" Then

                        returnValue &= variableKey & ": " & HighslideVariables.Item(variableKey).ToString.ToLower & ", "
                    Else
                        returnValue &= variableKey & ": '" & HighslideVariables.Item(variableKey) & "', "
                    End If
                Next
                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' This is used to statically set overriding Highslide variables (http://highslide.com/ref/).  Write in the format, "key: 'StringValue', key: NumericalValue, key: BooleanValue".  These values are tagged onto the end of the hashtable values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("This is used to statically set overriding Highslide variables (http://highslide.com/ref/). Write in the format, ""key: 'StringValue', key: NumericalValue, key: BooleanValue"". These values are tagged onto the end of the hashtable values")> _
        Public Property HighslideVariablesString() As String
            Get
                If HighslideDisplayMode = HighslideDisplayModes.Iframe AndAlso Not HighslideVariables.Contains("objectType") Then
                    HighslideVariables("objectType") = "iframe"
                End If
                Dim s As String = CStr(ViewState("HighslideVariablesString"))
                If s Is Nothing Then
                    If HighslideVariables.Count > 0 Then
                        Dim minusComma As String = HighslideHashTableString
                        minusComma = minusComma.Substring(0, minusComma.LastIndexOf(","))
                        s = ", {" & minusComma & "}"
                    Else
                        s = ", { }"
                    End If
                Else
                    s = ", {" & HighslideHashTableString & s & "}"
                End If
                If isFixed Then
                    s &= ",{Fixed: true}"
                End If
                Return s
            End Get
            Set(ByVal value As String)
                ViewState("HighslideVariablesString") = value
            End Set
        End Property

        Private ReadOnly Property thumbnail() As String
            Get
                Dim returnValue As String = ""
                If ThumbURL <> "" Then
                    returnValue = "<img src=""" & ThumbURL

                    If ThumbWidth <> "" Then
                        returnValue &= """ width=""" & ThumbWidth
                    End If

                    If ThumbWidth <> "" Then
                        returnValue &= """ height=""" & ThumbHeight
                    End If

                    returnValue &= """ title=""Click to enlarge"" />"
                End If

                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' The text displayed in a tooltip onmouseover
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The text displayed in a tooltip onmouseover")> _
        Public Property Title() As String
            Get
                Dim s As String = CStr(ViewState("Title"))
                If s Is Nothing Then
                    Return "Click to Enlarge"
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("Title") = Value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("")> _
        Public ReadOnly Property outputValue() As String
            Get
                Dim returnValue As String = "<a href=""" & hrefValue & """ id=""" & Me.ClientID & """ title=""" & Title & """ class=""highslide"""
                returnValue &= " onclick=""" & onclickValue & """>"
                returnValue &= innerValue & "</a>"
                'If Caption <> String.Empty Then
                '    returnValue &= "<div id='" & Me.ClientID & "caption' class=""highslide-caption""> " & Caption & "</div> "
                'End If
                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' Open JS Function
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Open JS Function")> _
        Public ReadOnly Property openJSFunction() As String
            Get
                Dim returnValue As String = onclickValue.Replace("(this", "(document.getElementById('" & Me.ClientID & "')")
                Return returnValue
            End Get
        End Property

#End Region

#Region "Overridable Variables"

        ''' <summary>
        ''' Position of the full image in the client, either 'auto' or 'center'.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Position of the full image in the client, either 'auto' or 'center'.")> _
        Public Property align() As String
            Get
                Return HighslideVariables("align")
            End Get
            Set(ByVal value As String)
                HighslideVariables("align") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow HTML popups to shrink to fit the width of the browser windoww.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Allow HTML popups to shrink to fit the width of the browser windoww.")> _
        Public Property allowHeightReduction() As Boolean
            Get
                Return HighslideVariables("allowHeightReduction")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("allowHeightReduction") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow the image to shrink to fit a small viewport.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Allow the image to shrink to fit a small viewport.")> _
        Public Property allowSizeReduction() As Boolean
            Get
                Return HighslideVariables("allowSizeReduction")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("allowSizeReduction") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow HTML popups to shrink to fit the width of the browser windoww.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Allow HTML popups to shrink to fit the width of the browser windoww.")> _
        Public Property allowWidthReduction() As Boolean
            Get
                Return HighslideVariables("allowWidthReduction")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("allowWidthReduction") = value
            End Set
        End Property

        ''' <summary>
        ''' Which corner or side of the thumbnail the popup should be anchored to.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Which corner or side of the thumbnail the popup should be anchored to.")> _
        Public Property anchor() As String
            Get
                Return HighslideVariables("anchor")
            End Get
            Set(ByVal value As String)
                HighslideVariables("anchor") = value
            End Set
        End Property

        ''' <summary>
        ''' Inline option to start running a slideshow from a thumbnail.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Inline option to start running a slideshow from a thumbnail.")> _
        Public Property autoplay() As Boolean
            Get
                Return HighslideVariables("autoplay")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("autoplay") = value
            End Set
        End Property

        ''' <summary>
        ''' Cache the contents of AJAX popup for instant display.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Cache the contents of AJAX popup for instant display.")> _
        Public Property cacheAjax() As Boolean
            Get
                Return HighslideVariables("cacheAjax")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("cacheAjax") = value
            End Set
        End Property

        ''' <summary>
        ''' An expression to be evaluated into the caption text.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("An expression to be evaluated into the caption text.")> _
        Public Property captionEval() As String
            Get
                Return HighslideVariables("captionEval")
            End Get
            Set(ByVal value As String)
                HighslideVariables("captionEval") = value
            End Set
        End Property

        ''' <summary>
        ''' The id of the element to be cloned to an image caption.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The id of the element to be cloned to an image caption.")> _
        Public Property captionId() As String
            Get
                Return HighslideVariables("captionId")
            End Get
            Set(ByVal value As String)
                HighslideVariables("captionId") = value
            End Set
        End Property

        ''' <summary>
        ''' Overlay options for the caption, as listed under hs.registerOverlay.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Overlay options for the caption, as listed under hs.registerOverlay.")> _
        Public Property captionOverlay() As Object
            Get
                Return HighslideVariables("captionOverlay")
            End Get
            Set(ByVal value As Object)
                HighslideVariables("captionOverlay") = value
            End Set
        End Property

        ''' <summary>
        ''' The id of the HTML element containing the content for HTML popups.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The id of the HTML element containing the content for HTML popups.")> _
        Public Property contentId() As String
            Get
                Return HighslideVariables("contentId")
            End Get
            Set(ByVal value As String)
                HighslideVariables("contentId") = value
            End Set
        End Property

        ''' <summary>
        ''' The position of the credits relative to the popup.
        ''' </summary>
        ''' <value>
        ''' Default value: 	top left
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The position of the credits relative to the popup.")> _
        Public Property creditsPosition() As String
            Get
                Return HighslideVariables("creditsPosition")
            End Get
            Set(ByVal value As String)
                HighslideVariables("creditsPosition") = value
            End Set
        End Property

        ''' <summary>
        ''' A float between 0 and 1 defining the opacity of a dimmed page background
        ''' </summary>
        ''' <value>
        ''' Default value: 	0
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("A float between 0 and 1 defining the opacity of a dimmed page background")> _
        Public Property dimmingOpacity() As Double
            Get
                Return HighslideVariables("dimmingOpacity")
            End Get
            Set(ByVal value As Double)
                HighslideVariables("dimmingOpacity") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow the image or HTML popup to be dragged by it's heading.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Allow the image or HTML popup to be dragged by it's heading.")> _
        Public Property dragByHeading() As Boolean
            Get
                Return HighslideVariables("dragByHeading")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("dragByHeading") = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the style of the expand/contract effect.
        ''' </summary>
        ''' <value>
        ''' Default value: 	easeInQuad
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Sets the style of the expand/contract effect.")> _
        Public Property easing() As String
            Get
                Return HighslideVariables("easing")
            End Get
            Set(ByVal value As String)
                HighslideVariables("easing") = value
            End Set
        End Property

        ''' <summary>
        ''' The easing when closing the expander. See hs.easing.
        ''' </summary>
        ''' <value>
        ''' Default value: 	inherited from hs.easing
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The easing when closing the expander. See hs.easing.")> _
        Public Property easingClose() As String
            Get
                Return HighslideVariables("easingClose")
            End Get
            Set(ByVal value As String)
                HighslideVariables("easingClose") = value
            End Set
        End Property

        ''' <summary>
        ''' Add a fading effect to the regular expand/contract effect.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Add a fading effect to the regular expand/contract effect.")> _
        Public Property fadeInOut() As Boolean
            Get
                Return HighslideVariables("fadeInOut")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("fadeInOut") = value
            End Set
        End Property

        ''' <summary>
        ''' An expression to be evaluated into the heading text.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("An expression to be evaluated into the heading text.")> _
        Public Property headingEval() As String
            Get
                Return HighslideVariables("headingEval")
            End Get
            Set(ByVal value As String)
                HighslideVariables("headingEval") = value
            End Set
        End Property

        ''' <summary>
        ''' The id of the element to be cloned to an image heading.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The id of the element to be cloned to an image heading.")> _
        Public Property headingId() As String
            Get
                Return HighslideVariables("headingId")
            End Get
            Set(ByVal value As String)
                HighslideVariables("headingId") = value
            End Set
        End Property

        ''' <summary>
        ''' Overlay options for the heading, as listed under hs.registerOverlay.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Overlay options for the heading, as listed under hs.registerOverlay.")> _
        Public Property headingOverlay() As Object
            Get
                Return HighslideVariables("headingOverlay")
            End Get
            Set(ByVal value As Object)
                HighslideVariables("headingOverlay") = value
            End Set
        End Property

        ''' <summary>
        ''' A text to use in the heading.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("A text to use in the heading.")> _
        Public Property headingText() As String
            Get
                Return HighslideVariables("headingText")
            End Get
            Set(ByVal value As String)
                HighslideVariables("headingText") = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the pixel height of the HTML expander.
        ''' </summary>
        ''' <value>
        ''' Default value: 	undefined
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Sets the pixel height of the HTML expander.")> _
        Public Property height() As Integer
            Get
                Return HighslideVariables("height")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("height") = value
            End Set
        End Property

        ''' <summary>
        ''' An expression to be evaluated into the main content while using self rendering content wrapper.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("An expression to be evaluated into the main content while using self rendering content wrapper.")> _
        Public Property maincontentEval() As String
            Get
                Return HighslideVariables("maincontentEval")
            End Get
            Set(ByVal value As String)
                HighslideVariables("maincontentEval") = value
            End Set
        End Property

        ''' <summary>
        ''' The id of an element to open in a HTML expander using self rendering content wrapper.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The id of an element to open in a HTML expander using self rendering content wrapper.")> _
        Public Property maincontentId() As String
            Get
                Return HighslideVariables("maincontentId")
            End Get
            Set(ByVal value As String)
                HighslideVariables("maincontentId") = value
            End Set
        End Property

        ''' <summary>
        ''' A text to use in a HTML popup when using self rendering content wrapper.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("A text to use in a HTML popup when using self rendering content wrapper.")> _
        Public Property maincontentText() As String
            Get
                Return HighslideVariables("maincontentText")
            End Get
            Set(ByVal value As String)
                HighslideVariables("maincontentText") = value
            End Set
        End Property

        ''' <summary>
        ''' Pixel value for the maximum height of the expanded content, see hs.maxWidth.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Pixel value for the maximum height of the expanded content, see hs.maxWidth.")> _
        Public Property maxHeight() As Integer
            Get
                Return HighslideVariables("maxHeight")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("maxHeight") = value
            End Set
        End Property

        ''' <summary>
        ''' Pixel width for the maximum width of the expanded image.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Pixel width for the maximum width of the expanded image.")> _
        Public Property maxWidth() As Integer
            Get
                Return HighslideVariables("maxWidth")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("maxWidth") = value
            End Set
        End Property

        ''' <summary>
        ''' Pixel value for the minimum height of the expanded content, see hs.minWidth.
        ''' </summary>
        ''' <value>
        ''' Default value: 	200
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Pixel value for the minimum height of the expanded content, see hs.minWidth.")> _
        Public Property minHeight() As Integer
            Get
                Return HighslideVariables("minHeight")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("minHeight") = value
            End Set
        End Property

        ''' <summary>
        ''' Pixel value for the minimum width of the expanded image.
        ''' </summary>
        ''' <value>
        ''' Default value: 	200
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Pixel value for the minimum width of the expanded image.")> _
        Public Property minWidth() As Integer
            Get
                Return HighslideVariables("minWidth")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("minWidth") = value
            End Set
        End Property

        ''' <summary>
        ''' Where to inject a text showing the number of the current image in a sequence.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Where to inject a text showing the number of the current image in a sequence.")> _
        Public Property numberPosition() As String
            Get
                Return HighslideVariables("numberPosition")
            End Get
            Set(ByVal value As String)
                HighslideVariables("numberPosition") = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the height of an included iframe or Flash movie.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Specifies the height of an included iframe or Flash movie.")> _
        Public Property objectHeight() As Integer
            Get
                Return HighslideVariables("objectHeight")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("objectHeight") = value
            End Set
        End Property

        ''' <summary>
        ''' Display an iframe or Flash object before or after the animation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Display an iframe or Flash object before or after the animation.")> _
        Public Property objectLoadTime() As String
            Get
                Return HighslideVariables("objectLoadTime")
            End Get
            Set(ByVal value As String)
                HighslideVariables("objectLoadTime") = value
            End Set
        End Property

        ''' <summary>
        ''' One of 'ajax', 'iframe' or 'swf'. Defines how you want to display the link target.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("One of 'ajax', 'iframe' or 'swf'. Defines how you want to display the link target.")> _
        Public Property objectType() As String
            Get
                Return HighslideVariables("objectType")
            End Get
            Set(ByVal value As String)
                HighslideVariables("objectType") = value
            End Set
        End Property

        ''' <summary>
        ''' The width of extended content like Flash.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("The width of extended content like Flash.")> _
        Public Property objectWidth() As Integer
            Get
                Return HighslideVariables("objectWidth")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("objectWidth") = value
            End Set
        End Property

        ''' <summary>
        ''' Defines a graphic outline to display around the expanded content.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Defines a graphic outline to display around the expanded content.")> _
        Public Property outlineType() As String
            Get
                Return HighslideVariables("outlineType")
            End Get
            Set(ByVal value As String)
                HighslideVariables("outlineType") = value
            End Set
        End Property

        ''' <summary>
        ''' Show the graphic outline during the expansion and contraction of the popup.
        ''' </summary>
        ''' <value>
        ''' Default value: 	2
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Show the graphic outline during the expansion and contraction of the popup.")> _
        Public Property outlineWhileAnimating() As Boolean
            Get
                Return HighslideVariables("outlineWhileAnimating")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("outlineWhileAnimating") = value
            End Set
        End Property

        ''' <summary>
        ''' Preserve changes made to the content and position of HTML content.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Preserve changes made to the content and position of HTML content.")> _
        Public Property preserveContent() As Boolean
            Get
                Return HighslideVariables("preserveContent")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("preserveContent") = value
            End Set
        End Property

        ''' <summary>
        ''' Places your popups into groups for Next and Previous navigation.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Places your popups into groups for Next and Previous navigation.")> _
        Public Property slideshowGroup() As String
            Get
                Return HighslideVariables("slideshowGroup")
            End Get
            Set(ByVal value As String)
                HighslideVariables("slideshowGroup") = value
            End Set
        End Property

        ''' <summary>
        ''' An alternative way to define the URL of the image or HTML content.
        ''' </summary>
        ''' <value>
        ''' Default value: 	undefined
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("An alternative way to define the URL of the image or HTML content.")> _
        Public Property src() As String
            Get
                Return HighslideVariables("src")
            End Get
            Set(ByVal value As String)
                HighslideVariables("src") = value
            End Set
        End Property

        ''' <summary>
        ''' Advanced options when showing Flash content using hs.contentType = "swf".
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Advanced options when showing Flash content using hs.contentType = ""swf"".")> _
        Public Property swfOptions() As Object
            Get
                Return HighslideVariables("swfOptions")
            End Get
            Set(ByVal value As Object)
                HighslideVariables("swfOptions") = value
            End Set
        End Property

        ''' <summary>
        ''' Positions the expanded image or HTML content exactly above a given element on your page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Positions the expanded image or HTML content exactly above a given element on your page.")> _
        Public Property targetX() As String
            Get
                Return HighslideVariables("targetX")
            End Get
            Set(ByVal value As String)
                HighslideVariables("targetX") = value
            End Set
        End Property

        ''' <summary>
        ''' Positions the expanded image or HTML content exactly above a given element on your page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Positions the expanded image or HTML content exactly above a given element on your page.")> _
        Public Property targetY() As String
            Get
                Return HighslideVariables("targetY")
            End Get
            Set(ByVal value As String)
                HighslideVariables("targetY") = value
            End Set
        End Property

        ''' <summary>
        ''' Define transitions other than the default zoom in and out
        ''' </summary>
        ''' <value>
        ''' Default value: 	[]
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Define transitions other than the default zoom in and out")> _
        Public Property transitions() As Array
            Get
                Return HighslideVariables("transitions")
            End Get
            Set(ByVal value As Array)
                HighslideVariables("transitions") = value
            End Set
        End Property

        ''' <summary>
        ''' Use a constraining box so that the borders, captions etc. don't change size from image to image.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Use a constraining box so that the borders, captions etc. don't change size from image to image.")> _
        Public Property useBox() As Boolean
            Get
                Return HighslideVariables("useBox")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("useBox") = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the pixel width of the HTML expander, or the width of the constraining box for images.
        ''' </summary>
        ''' <value>
        ''' Default value: 	undefined
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("Sets the pixel width of the HTML expander, or the width of the constraining box for images.")> _
        Public Property width() As Integer
            Get
                Return HighslideVariables("width")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("width") = value
            End Set
        End Property

        ''' <summary>
        ''' A specific CSS class for the wrapping div to enhance CSS control.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Overrideable inline
        ''' </remarks>
        <System.ComponentModel.Description("A specific CSS class for the wrapping div to enhance CSS control.")> _
        Public Property wrapperClassName() As String
            Get
                Return HighslideVariables("wrapperClassName")
            End Get
            Set(ByVal value As String)
                HighslideVariables("wrapperClassName") = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="overlay"></param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("")> _
        Public Sub registerOverlay(ByRef overlay As HighslideOverlay)
            Controls.Add(overlay)
            overlay.thumbnailId = Me.ClientID
        End Sub

        Private Sub Highslider_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Controls.Add(litAnchor)
            pnlCaptionControls.ID = Guid.NewGuid.ToString
            pnlContentControls.ID = Guid.NewGuid.ToString
            Controls.Add(pnlCaptionControls)
            Controls.Add(pnlContentControls)
        End Sub

        'Private Sub Highslider_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '    Controls.Add(litAnchor)
        '    pnlCaptionControls.ID = Guid.NewGuid.ToString
        '    pnlContentControls.ID = Guid.NewGuid.ToString
        '    Controls.Add(pnlCaptionControls)
        '    Controls.Add(pnlContentControls)
        'End Sub

        Private Sub Highslider_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'makes sure that the header is on the page
            If _ensure_header Then Dim x = myHeader
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            If CaptionControls.Count > 0 Then
                pnlCaptionControls.CssClass = "highslide-caption"
                captionId = pnlCaptionControls.ClientID
            Else
                pnlCaptionControls.Visible = False
            End If
            If ContentControls.Count > 0 Then
                pnlContentControls.CssClass = "highslide-maincontent"
                maincontentId = pnlContentControls.ClientID
            Else
                pnlContentControls.Visible = False
            End If

            litAnchor.Text = outputValue

            MyBase.Render(writer)

            'writer.Write(outputValue)
        End Sub

    End Class
