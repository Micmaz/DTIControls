Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' Control for inserting and managing Highslide javascript on a web page
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class HighslideHeaderControl
    Inherits PlaceHolder
#Else
    <DefaultProperty("HighslideVariablesString"), PersistChildren(False), ParseChildren(True, "HighslideVariablesString"), ToolboxData("<{0}:HighslideHeaderControl runat=server></{0}:HighslideHeaderControl>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class HighslideHeaderControl
        Inherits PlaceHolder
#End If
        Private baseURL As String = BaseClasses.Scripts.ScriptsURL & "HighslideControls/"

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
                    HighslideVariables.Add("outlineType", getOutlineText(Value))
                End If
            End Set
        End Property

        Private _debug As Boolean = False
        Public Property Debug() As Boolean
            Get
                Return _debug
            End Get
            Set(ByVal value As Boolean)
                _debug = value
            End Set
        End Property

	'Private _regString As String = "//a13fe4e6f3a0bf746da103090dd47568"

	''' <summary>
	''' A unique string javascript comment that relays lisensing information for Highslide
	''' </summary>
	''' <remarks></remarks>
	'<System.ComponentModel.Description("A unique string javascript comment that relays lisensing information for Highslide")> _
	'       Public Property registrationString() As String
	'           Get
	'               Return _regString
	'           End Get
	'           Set(ByVal value As String)
	'               If Not _regString.StartsWith("//") Then
	'                   _regString = "//" & _regString
	'               End If
	'           End Set
	'       End Property

	''' <summary>
	''' Hashtable containing global highslide variables, see http://highslide.com/ref/
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
        <System.ComponentModel.Description("Hashtable containing global highslide variables, see http://highslide.com/ref/")> _
        Public ReadOnly Property HighslideVariables() As Hashtable
            Get
                If ViewState("HighslideVariables") Is Nothing Then
                    ViewState("HighslideVariables") = New Hashtable
                End If

                Dim _highslideVariables As Hashtable = ViewState("HighslideVariables")

                If Not _highslideVariables.Contains("graphicsDir") Then
                    _highslideVariables.Add("graphicsDir", baseURL)
                End If

                If Not _highslideVariables.Contains("showCredits") Then
                    _highslideVariables.Add("showCredits", "false")
                End If

                Return _highslideVariables
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

                        returnValue &= "hs." & variableKey & " = " & HighslideVariables.Item(variableKey).ToString.ToLower & ";" & vbCrLf
                    Else
                        returnValue &= "hs." & variableKey & " = '" & HighslideVariables.Item(variableKey) & "';" & vbCrLf
                    End If
                Next
                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' This is used to statically set global Highslide variables (http://highslide.com/ref/).  Write in the format, "key: 'StringValue', key: NumericalValue, key: BooleanValue".  These values are tagged onto the end of the hashtable values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("This is used to statically set global Highslide variables (http://highslide.com/ref/). Write in the format, ""key: 'StringValue', key: NumericalValue, key: BooleanValue"". These values are tagged onto the end of the hashtable values"),PersistenceMode(PersistenceMode.InnerDefaultProperty)> _
        Public Property HighslideVariablesString() As String
            Get
                Dim s As String = CStr(ViewState("HighslideVariablesString"))
                If s Is Nothing Then
                    If HighslideVariables.Count > 0 Then
                        Return HighslideHashTableString
                    Else
                        Return String.Empty
                    End If
                Else
                    Return HighslideHashTableString & s
                End If
            End Get
            Set(ByVal value As String)
                ViewState("HighslideVariablesString") = value
            End Set
        End Property

        ''' <summary>
        ''' Set this property to true if the page is contained within an iframe.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Set this property to true if the page is contained within an iframe.")> _
        Public Property isInnerFrame() As Boolean
            Get
                Return ViewState("isInnerFrame")
            End Get
            Set(ByVal value As Boolean)
                ViewState("isInnerFrame") = value
                isOuterFrame = False
            End Set
        End Property

        ''' <summary>
        ''' Set this property to true if the page contains iframe's with highslide elements
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Set this property to true if the page contains iframe's with highslide elements")> _
        Public Property isOuterFrame() As Boolean
            Get
                Return ViewState("isOuterFrame")
            End Get
            Set(ByVal value As Boolean)
                ViewState("isOuterFrame") = value
            End Set
        End Property

        Private issetup As Boolean = False
        Private Sub setupHs()
            If Not issetup Then

                If myPage Is Nothing Then myPage = Me.Page
                Dim returnValue As String = ""
                If isInnerFrame Then
                    jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/hsinnerframe.js")
                    'returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsinnerframe.js""></script>"
                Else
                    Dim filename As String = "highslide-full.js"
                    If Not Debug Then
                        filename = filename.Replace(".js", ".min.js")
                    End If
                    jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/" & filename)
                    'returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "highslide-full.js""></script>"
                End If
                returnValue &= vbCrLf

                If isOuterFrame Then
                    jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/hsouterframe.js")
                    'returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsouterframe.js""></script>"
                    returnValue &= vbCrLf
                End If
                jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/highslide.css", "text/css")
			'returnValue &= "<link rel=""stylesheet"" type=""text/css"" href=""" & baseURL & "highslide.css"" />"
			'returnValue &= vbCrLf
			''returnValue &= "<script type=""text/javascript"">"
			'returnValue &= vbCrLf
			'returnValue &= registrationString
			'returnValue &= vbCrLf
			'returnValue &= HighslideVariablesString
			'returnValue &= vbCrLf
			'returnValue &= "hs.onActivate = function() {" & vbCrLf
			'If isInnerFrame Then
			'    returnValue &= "var theForm = parent.window.document.forms[0];" & vbCrLf
			'Else
			'    returnValue &= "var theForm = document.forms[0];" & vbCrLf
			'End If
			'returnValue &= "if (theForm) theForm.appendChild(hs.container);" & vbCrLf & "}" & vbCrLf

			'If isInnerFrame Then
			'    returnValue &= "addLoadEvent(function() {parent.window.iframe = getIframe();hs.updateAnchors();});"
			'    returnValue &= vbCrLf
			'End If
			''returnValue &= "</script>"

			'If isInnerFrame Then
			'    returnValue = returnValue.Replace("hs.", "parent.window.hs.")
			'End If

			'jQueryLibrary.jQueryInclude.addScriptBlock(myPage, "try{" & returnValue & "}catch(err){}", False)

			If isInnerFrame Then
                    jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/hsFixedInner.js")
                    'returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsFixedInner.js""></script>"
                Else
                    jQueryLibrary.jQueryInclude.addScriptFile(myPage, "HighslideControls/hsFixed.js")
                    'returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsFixed.js""></script>"
                End If
                issetup = True
            End If


        End Sub

        Private ReadOnly Property outputValue() As String
            Get
                Dim returnValue As String = ""
                If isInnerFrame Then
                    returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsinnerframe.js""></script>"
                Else
                    Dim filename As String = "highslide-full.js"
                    If Not Debug Then
                        filename = filename.Replace(".js", ".min.js")
                    End If
                    returnValue &= "<script type=""text/javascript"" src=""" & baseURL & filename & """></script>"
                End If
                returnValue &= vbCrLf

                If isOuterFrame Then
                    returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsouterframe.js""></script>"
                    returnValue &= vbCrLf
                End If

                returnValue &= "<link rel=""stylesheet"" type=""text/css"" href=""" & baseURL & "highslide.css"" />"
			'returnValue &= vbCrLf
			'returnValue &= "<script type=""text/javascript"">"
			'returnValue &= vbCrLf
			'returnValue &= registrationString
			'returnValue &= vbCrLf
			'returnValue &= HighslideVariablesString
			'returnValue &= vbCrLf
			'returnValue &= "hs.onActivate = function() {" & vbCrLf
			'If isInnerFrame Then
			'    returnValue &= "var theForm = parent.window.document.forms[0];" & vbCrLf
			'Else
			'    returnValue &= "var theForm = document.forms[0];" & vbCrLf
			'End If
			'returnValue &= "if (theForm) theForm.appendChild(hs.container);" & vbCrLf & "}" & vbCrLf

			'If isInnerFrame Then
			'    returnValue &= "addLoadEvent(function() {parent.window.iframe = getIframe();hs.updateAnchors();});"
			'    returnValue &= vbCrLf
			'End If
			'returnValue &= "</script>"

			If isInnerFrame Then
                    returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsFixedInner.js""></script>"
                Else
                    returnValue &= "<script type=""text/javascript"" src=""" & baseURL & "hsFixed.js""></script>"
                End If

                returnValue &= vbCrLf

			'If isInnerFrame Then
			'    returnValue = returnValue.Replace("hs.", "parent.window.hs.")
			'End If
			Return returnValue
            End Get
        End Property


        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            'writer.Write(outputValue)
            setupHs()
        End Sub

        Private Sub HighslideHeaderControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Me.Page.Items.Item("hsHeader") Is Nothing Then
                Me.Page.Items.Item("hsHeader") = Me
            End If
        End Sub

        Private Sub HighslideHeaderControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            setupHs()
        End Sub

        Public myPage As Page

        ''' <summary>
        ''' Add a HighslideHeaderControl to the page given as an argument if there is not one already present.  Returns the resulting control.
        ''' </summary>
        ''' <param name="page"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Add a HighslideHeaderControl to the page given as an argument if there is not one already present. Returns the resulting control.")> _
        Public Shared Function addToPage(ByRef page As Page) As HighslideHeaderControl
            If page.Items.Item("hsHeader") Is Nothing Then
                Dim hsheader As New HighslideHeaderControl
                hsheader.myPage = page
                page.Items.Item("hsHeader") = hsheader
                jQueryLibrary.jQueryInclude.addControlToHeader(page, hsheader)
            End If
            'Dim highslideHeader As HighslideHeaderControl = spiderPageforType(page, GetType(HighslideHeaderControl))
            'If highslideHeader Is Nothing Then
            '    highslideHeader = New HighslideHeaderControl
            '    If page.Header Is Nothing Then
            '        jQueryLibrary.jQueryInclude.addControlToHeader(page, highslideHeader)
            '    Else
            '        page.Header.Controls.Add(highslideHeader)
            '    End If
            'End If
            Return page.Items.Item("hsHeader")
        End Function


        'Protected Shared Function spiderPageforType(ByVal pg As Page, ByVal tp As Type) As Control
        '    For Each ctrl As Control In pg.Controls
        '        Dim ret As Control = spidercontrolforType(ctrl, tp)
        '        If Not ret Is Nothing Then
        '            Return ret
        '        End If
        '    Next
        '    Return Nothing
        'End Function

        Protected Shared Function spidercontrolforType(ByVal ctrl As Control, ByVal tp As Type) As Control
            If ctrl.GetType() Is tp Then
                Return ctrl
            End If
            For Each subctrl As Control In ctrl.Controls
                Dim ret As Control = spidercontrolforType(subctrl, tp)
                If Not ret Is Nothing Then
                    Return ret
                End If
            Next
            Return Nothing
        End Function

        Private Sub HighslideHeaderControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            BaseVirtualPathProvider.registerVirtualPathProvider()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="overlay"></param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("")> _
        Public Sub registerOverlay(ByRef overlay As HighslideOverlay)
            Me.Page.Form.Controls.Add(overlay)
        End Sub

#Region "Variables"

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
        ''' Allow more than one popup expander to be open at the same time.
        ''' </summary>
        ''' <value>
        ''' Default value:	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Allow more than one popup expander to be open at the same time.")> _
        Public Property allowMultipleInstances() As Boolean
            Get
                Return HighslideVariables("allowMultipleInstances")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("allowMultipleInstances") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow multiple popups to be opened at the same time
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Allow multiple popups to be opened at the same time")> _
        Public Property allowSimultaneousLoading() As Boolean
            Get
                Return HighslideVariables("allowSimultaneousLoading")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("allowSimultaneousLoading") = value
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
        ''' Block right clicking and context menu on the full size image.
        ''' </summary>
        ''' <value>
        ''' Default value:	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Block right clicking and context menu on the full size image.")> _
        Public Property blockRightClick() As Boolean
            Get
                Return HighslideVariables("blockRightClick")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("blockRightClick") = value
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
        ''' A text to use in the caption.
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("A text to use in the caption.")> _
        Public Property captionText() As String
            Get
                Return HighslideVariables("captionText")
            End Get
            Set(ByVal value As String)
                HighslideVariables("captionText") = value
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
        ''' The hyperlink reference for the credits label.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The hyperlink reference for the credits label.")> _
        Public Property creditsHref() As String
            Get
                Return HighslideVariables("creditsHref")
            End Get
            Set(ByVal value As String)
                HighslideVariables("creditsHref") = value
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
        ''' The target of the credits link
        ''' </summary>
        ''' <value>
        ''' Default value: 	_self
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The target of the credits link")> _
        Public Property creditsTarget() As String
            Get
                Return HighslideVariables("creditsTarget")
            End Get
            Set(ByVal value As String)
                HighslideVariables("creditsTarget") = value
            End Set
        End Property

        ''' <summary>
        ''' The duration in milliseconds of the fading in and out of the background dimming effect.
        ''' </summary>
        ''' <value>
        ''' Default value: 	50
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The duration in milliseconds of the fading in and out of the background dimming effect.")> _
        Public Property dimmingDuration() As Integer
            Get
                Return HighslideVariables("dimmingDuration")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("dimmingDuration") = value
            End Set
        End Property

        ''' <summary>
        ''' Fix Gecko/Mac bug that causes problems with dimming and Flash.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Fix Gecko/Mac bug that causes problems with dimming and Flash.")> _
        Public Property dimmingGeckoFix() As Boolean
            Get
                Return HighslideVariables("dimmingGeckoFix")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("dimmingGeckoFix") = value
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
        ''' How many pixels to drag the full image before it starts moving.
        ''' </summary>
        ''' <value>
        ''' Default value: 	5
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("How many pixels to drag the full image before it starts moving.")> _
        Public Property dragSensitivity() As Integer
            Get
                Return HighslideVariables("dragSensitivity")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("dragSensitivity") = value
            End Set
        End Property

        ''' <summary>
        ''' Allow Highslide to update the anchors collection automatically after the DOM has changed.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Allow Highslide to update the anchors collection automatically after the DOM has changed.")> _
        Public Property dynamicallyUpdateAnchors() As Boolean
            Get
                Return HighslideVariables("dynamicallyUpdateAnchors")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("dynamicallyUpdateAnchors") = value
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
        ''' Listen for keystrokes like left and right arrow to control Highslide.
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Listen for keystrokes like left and right arrow to control Highslide.")> _
        Public Property enableKeyListener() As Boolean
            Get
                Return HighslideVariables("enableKeyListener")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("enableKeyListener") = value
            End Set
        End Property

        ''' <summary>
        ''' The filename of the cursor that indicates zoom in on the thumbnails.
        ''' </summary>
        ''' <value>
        ''' Default value: 	'zoomin.cur'
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The filename of the cursor that indicates zoom in on the thumbnails.")> _
        Public Property expandCursor() As String
            Get
                Return HighslideVariables("expandCursor")
            End Get
            Set(ByVal value As String)
                HighslideVariables("expandCursor") = value
            End Set
        End Property

        ''' <summary>
        ''' Defines in milliseconds how long the zoom in effect should take.
        ''' </summary>
        ''' <value>
        ''' Default value: 	250
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Defines in milliseconds how long the zoom in effect should take.")> _
        Public Property expandDuration() As Integer
            Get
                Return HighslideVariables("expandDuration")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("expandDuration") = value
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
        ''' Flush IE's cashed image width and height after an image is physically resized.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Flush IE's cashed image width and height after an image is physically resized.")> _
        Public Property flushImgSize() As Boolean
            Get
                Return HighslideVariables("flushImgSize")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("flushImgSize") = value
            End Set
        End Property

        ''' <summary>
        ''' IE-specific option to force Ajax-loaded content to be reloaded from the server on each popup opening
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("IE-specific option to force Ajax-loaded content to be reloaded from the server on each popup opening")> _
        Public Property forceAjaxReload() As Boolean
            Get
                Return HighslideVariables("forceAjaxReload")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("forceAjaxReload") = value
            End Set
        End Property

        ''' <summary>
        ''' A float between 0 and 1 giving the opacity of the full expand label
        ''' </summary>
        ''' <value>
        ''' Default value: 	1
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("A float between 0 and 1 giving the opacity of the full expand label")> _
        Public Property fullExpandOpacity() As Double
            Get
                Return HighslideVariables("fullExpandOpacity")
            End Get
            Set(ByVal value As Double)
                HighslideVariables("fullExpandOpacity") = value
            End Set
        End Property

        ''' <summary>
        ''' The position of the full expand label relative to the image.
        ''' </summary>
        ''' <value>
        ''' Default value: 	bottom right
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The position of the full expand label relative to the image.")> _
        Public Property fullExpandPosition() As String
            Get
                Return HighslideVariables("fullExpandPosition")
            End Get
            Set(ByVal value As String)
                HighslideVariables("fullExpandPosition") = value
            End Set
        End Property

        ''' <summary>
        ''' The path to the directory where your graphics are.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The path to the directory where your graphics are.")> _
        Public Property graphicsDir() As String
            Get
                Return HighslideVariables("graphicsDir")
            End Get
            Set(ByVal value As String)
                HighslideVariables("graphicsDir") = value
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
        ''' The object containing language strings for localisation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The object containing language strings for localisation.")> _
        Public Property lang() As Object
            Get
                Return HighslideVariables("lang")
            End Get
            Set(ByVal value As Object)
                HighslideVariables("lang") = value
            End Set
        End Property

        ''' <summary>
        ''' The opacity of the loading label.
        ''' </summary>
        ''' <value>
        ''' Default value: 	0.75
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The opacity of the loading label.")> _
        Public Property loadingOpacity() As Double
            Get
                Return HighslideVariables("loadingOpacity")
            End Get
            Set(ByVal value As Double)
                HighslideVariables("loadingOpacity") = value
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
        ''' The expanded image or HTML content never exceeds this bottom margin on the page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	15
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The expanded image or HTML content never exceeds this bottom margin on the page.")> _
        Public Property marginBottom() As Integer
            Get
                Return HighslideVariables("marginBottom")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("marginBottom") = value
            End Set
        End Property

        ''' <summary>
        ''' The expanded image or HTML content never exceeds this left margin on the page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	15
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The expanded image or HTML content never exceeds this left margin on the page.")> _
        Public Property marginLeft() As Integer
            Get
                Return HighslideVariables("marginLeft")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("marginLeft") = value
            End Set
        End Property

        ''' <summary>
        ''' The expanded image or HTML content never exceeds this right margin on the page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	15
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The expanded image or HTML content never exceeds this right margin on the page.")> _
        Public Property marginRight() As Integer
            Get
                Return HighslideVariables("marginRight")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("marginRight") = value
            End Set
        End Property

        ''' <summary>
        ''' The expanded image or HTML content never exceeds this top margin on the page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	15
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The expanded image or HTML content never exceeds this top margin on the page.")> _
        Public Property marginTop() As Integer
            Get
                Return HighslideVariables("marginTop")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("marginTop") = value
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
        ''' Highslide by default preloads the first 5 images in your page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	5
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Highslide by default preloads the first 5 images in your page.")> _
        Public Property numberOfImagesToPreload() As Integer
            Get
                Return HighslideVariables("numberOfImagesToPreload")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("numberOfImagesToPreload") = value
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
        ''' Which elements should Highslide iterate to look for thumbnail openers.
        ''' </summary>
        ''' <value>
        ''' Default value: 	["a"]
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Which elements should Highslide iterate to look for thumbnail openers.")> _
        Public Property openerTagNames() As Array
            Get
                Return HighslideVariables("openerTagNames")
            End Get
            Set(ByVal value As Array)
                HighslideVariables("openerTagNames") = value
            End Set
        End Property

        ''' <summary>
        ''' The directory within the hs.graphicsDir where the outline pngs are found.
        ''' </summary>
        ''' <value>
        ''' Default value: 	outlines/
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The directory within the hs.graphicsDir where the outline pngs are found.")> _
        Public Property outlinesDir() As String
            Get
                Return HighslideVariables("outlinesDir")
            End Get
            Set(ByVal value As String)
                HighslideVariables("outlinesDir") = value
            End Set
        End Property

        ''' <summary>
        ''' When hs.outlineWhileAnimating is true, this defines the starting pixel offset.
        ''' </summary>
        ''' <value>
        ''' Default value: 	3
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("When hs.outlineWhileAnimating is true, this defines the starting pixel offset.")> _
        Public Property outlineStartOffset() As Integer
            Get
                Return HighslideVariables("outlineStartOffset")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("outlineStartOffset") = value
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
        ''' Defines which settings of the hs object that will be inherited by the single hs.Expander.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Defines which settings of the hs object that will be inherited by the single hs.Expander.")> _
        Public Property overrides_hsVariable() As Array
            Get
                Return HighslideVariables("overrides_hsVariable")
            End Get
            Set(ByVal value As Array)
                HighslideVariables("overrides_hsVariable") = value
            End Set
        End Property

        ''' <summary>
        ''' On narrow images, pad the width of the expander to the minWidth to make room for the caption.
        ''' </summary>
        ''' <value>
        ''' Default value: 	false
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("On narrow images, pad the width of the expander to the minWidth to make room for the caption.")> _
        Public Property padToMinWidth() As Boolean
            Get
                Return HighslideVariables("padToMinWidth")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("padToMinWidth") = value
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
        ''' The filename of the cursor that indicates zoom out on the expanded image.
        ''' </summary>
        ''' <value>
        ''' Default value: 	'zoomout.cur'
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("The filename of the cursor that indicates zoom out on the expanded image.")> _
        Public Property restoreCursor() As String
            Get
                Return HighslideVariables("restoreCursor")
            End Get
            Set(ByVal value As String)
                HighslideVariables("restoreCursor") = value
            End Set
        End Property

        ''' <summary>
        ''' Equivalent to hs.expandDuration but applies to the closing of the expander.
        ''' </summary>
        ''' <value>
        ''' Default value: 	250
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Equivalent to hs.expandDuration but applies to the closing of the expander.")> _
        Public Property restoreDuration() As Integer
            Get
                Return HighslideVariables("restoreDuration")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("restoreDuration") = value
            End Set
        End Property

        ''' <summary>
        ''' Whether to show a "Powered by..." label in the upper left corner. 
        ''' </summary>
        ''' <value>
        ''' Default value: 	true
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Whether to show a ""Powered by..."" label in the upper left corner.")> _
        Public Property showCredits() As Boolean
            Get
                Return HighslideVariables("showCredits")
            End Get
            Set(ByVal value As Boolean)
                HighslideVariables("showCredits") = value
            End Set
        End Property

        ''' <summary>
        ''' A collection of HTML to be inserted automatically.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("A collection of HTML to be inserted automatically.")> _
        Public Property skin() As Object
            Get
                Return HighslideVariables("skin")
            End Get
            Set(ByVal value As Object)
                HighslideVariables("skin") = value
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
        ''' Defines the id of a graphic to expand the image from
        ''' </summary>
        ''' <value>
        ''' Default value: 	null
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Defines the id of a graphic to expand the image from")> _
        Public Property thumbnailId() As String
            Get
                Return HighslideVariables("thumbnailId")
            End Get
            Set(ByVal value As String)
                HighslideVariables("thumbnailId") = value
            End Set
        End Property

        ''' <summary>
        ''' Defines in milliseconds how long the transition effect should take.
        ''' </summary>
        ''' <value>
        ''' Default value: 	500
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Defines in milliseconds how long the transition effect should take.")> _
        Public Property transitionDuration() As Integer
            Get
                Return HighslideVariables("transitionDuration")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("transitionDuration") = value
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

        ''' <summary>
        ''' Adjust the z-index to other elements on your page.
        ''' </summary>
        ''' <value>
        ''' Default value: 	1001
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adjust the z-index to other elements on your page.")> _
        Public Property zIndexCounter() As Integer
            Get
                Return HighslideVariables("zIndexCounter")
            End Get
            Set(ByVal value As Integer)
                HighslideVariables("zIndexCounter") = value
            End Set
        End Property

#End Region


    End Class
