Public Class LazyImgLoad
    Inherits Image

    Private _render As String = ""
    Private i As Integer = 0
    Private Origid As String = ""

    Private _PreLoadImage As String = BaseClasses.Scripts.ScriptsURL & "JqueryUIControls/transparent.gif"

    ''' <summary>
    ''' Specifies the image that will be loaded until the actual image can be loaded.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies the image that will be loaded until the actual image can be loaded.")> _
    Public Property PreLoadImage As String
        Get
            Return _PreLoadImage
        End Get
        Set(value As String)
            _PreLoadImage = value
        End Set
    End Property

    Private _effect As Effects = Effects.show

    ''' <summary>
    ''' Effect used to show the image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Effect used to show the image")> _
    Public Property Effect As Effects
        Get
            Return _effect
        End Get
        Set(value As Effects)
            _effect = value
        End Set
    End Property

    Private Property _effectDuration As String = ""
    Public Property EffectDuration As String
        Get
            Return _effectDuration
        End Get
        Set(value As String)
            _effectDuration = value
        End Set
    End Property

    Private _Threshold As Integer = -1

    ''' <summary>
    ''' By default images are loaded when they appear on the screen. 
    ''' If you want images to load earlier you can set threshold parameter. 
    ''' Setting threshold to 200 causes image to load 200 pixels before it is visible.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("By default images are loaded when they appear on the screen.    If you want images to load earlier you can set threshold parameter.    Setting threshold to 200 causes image to load 200 pixels before it is visible.")> _
    Public Property Threshold As Integer
        Get
            Return _Threshold
        End Get
        Set(value As Integer)
            _Threshold = value
        End Set
    End Property

    Public Enum TriggerEvents
        scroll
        click
        dblclick
        focusin
        focusout
        hover
        mousedown
        mouseenter
        mouseleave
        mousemove
        mouseout
        mouseover
        mouseup
        custom
    End Enum

    Private _triggerEvent As TriggerEvents = TriggerEvents.scroll

    ''' <summary>
    ''' any jQuery event such as click or mouseover. 
    ''' You can also use your own custom events such as sporty or foobar by
    ''' selecting custom and defining the CustomTriggerEvent Property. 
    ''' Default is to wait until user scrolls down and image appears on the window.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("any jQuery event such as click or mouseover.    You can also use your own custom events such as sporty or foobar by   selecting custom and defining the CustomTriggerEvent Property.    Default is to wait until user scrolls down and image appears on the window.")> _
    Public Property triggerEvent As TriggerEvents
        Get
            Return _triggerEvent
        End Get
        Set(value As TriggerEvents)
            _triggerEvent = value
        End Set
    End Property

    Private _customTriggerEvent As String = Nothing

    ''' <summary>
    ''' Custom event to trigger when a image is shown
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Custom event to trigger when a image is shown")> _
    Public Property CustomTriggerEvent As String
        Get
            Return _customTriggerEvent
        End Get
        Set(value As String)
            _customTriggerEvent = value
        End Set
    End Property

    Public Enum Effects
        show
        fadeIn
        slideDown
        slideUp
    End Enum

    Private _failureLimit As Integer = -1

    ''' <summary>
    ''' After scrolling page Lazy Load loops though unloaded images. 
    ''' In loop it checks if image has become visible. By default loop is stopped 
    ''' when first image below the fold (not visible) is found. This is based
    '''  on following assumption. Order of images on page is same as order of 
    ''' images in HTML code. With some layouts assumption this might be wrong. 
    ''' You can control loading behaviour with failure_limit option.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("After scrolling page Lazy Load loops though unloaded images.    In loop it checks if image has become visible. By default loop is stopped    when first image below the fold (not visible) is found. This is based    on following assumption. Order of images on page is same as order of    images in HTML code. With some layouts assumption this might be wrong.    You can control loading behaviour with failure_limit option.")> _
    Public Property FailureLimit As Integer
        Get
            Return _failureLimit
        End Get
        Set(value As Integer)
            _failureLimit = value
        End Set
    End Property

    Private _skipInvisible As Boolean = True

    ''' <summary>
    ''' There are cases when you have invisible images. For example when using 
    ''' plugin in together with a large filterable list of items that can have 
    ''' their visibility state changed dynamically. To improve performance 
    ''' Lazy Load ignores hidden images by default. If you want to load hidden 
    ''' images set skip_invisible to false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("There are cases when you have invisible images. For example when using    plugin in together with a large filterable list of items that can have    their visibility state changed dynamically. To improve performance    Lazy Load ignores hidden images by default. If you want to load hidden    images set skip_invisible to false.")> _
    Public Property SkipInvisible As Boolean
        Get
            Return _skipInvisible
        End Get
        Set(value As Boolean)
            _skipInvisible = value
        End Set
    End Property

    Private _container As String = ""

    ''' <summary>
    ''' You can also use plugin for images inside scrolling container, such as div with scrollbar. 
    ''' Just pass the container as jQuery object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("You can also use plugin for images inside scrolling container, such as div with scrollbar.    Just pass the container as jQuery object.")> _
    Public Property Container As String
        Get
            Return _container
        End Get
        Set(value As String)
            _container = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the border width of the Web server control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the border width of the Web server control.")> _
    Public Overrides Property BorderWidth As System.Web.UI.WebControls.Unit
        Get
            If MyBase.BorderWidth.IsEmpty Then
                Return Unit.Pixel(0)
            Else : Return MyBase.BorderWidth()
            End If
        End Get
        Set(value As System.Web.UI.WebControls.Unit)
            MyBase.BorderWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQuery(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.lazyload.min.js", , True)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""
        If Effect <> Effects.show Then
            outstr &= "effect: '" & DatePicker.getEnumName(Effect)
            If EffectDuration <> "" Then
                outstr &= "("
                Dim i As Integer = 0
                If Not Integer.TryParse(EffectDuration, i) Then
                    outstr &= "'" & EffectDuration & "'"
                Else
                    outstr &= EffectDuration
                End If
                outstr &= ")"
            End If
            outstr &= "',"
        End If
        If Threshold > -1 Then
            outstr &= "threshold: " & Threshold & ","
        End If
        If triggerEvent <> TriggerEvents.scroll AndAlso triggerEvent <> TriggerEvents.custom Then
            outstr &= "event: '" & DatePicker.getEnumName(triggerEvent) & "',"
        ElseIf triggerEvent = TriggerEvents.custom AndAlso CustomTriggerEvent IsNot Nothing Then
            outstr &= "event: '" & CustomTriggerEvent & "',"
        End If
        If FailureLimit > -1 Then
            outstr &= "failure_limit: " & FailureLimit & ","
        End If
        If Container <> "" Then
            outstr &= "container: " & Container & ","
        End If
        If Not SkipInvisible Then
            outstr &= "skip_invisible: false,"
        End If
        If outstr <> "" Then
            outstr = "{" & outstr.TrimEnd(",") & "}"
        End If
        Return outstr
    End Function

    Private Sub DatePicker_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If i = 0 Then
            'rendering noscript version incase user doesn't have javascript
            Origid = Me.ID
            Me.ID = Origid & "_noscript"
            Dim sb As New StringBuilder()
            Using sw As New IO.StringWriter(sb)
                Using textWriter As New HtmlTextWriter(sw)
                    Me.RenderControl(textWriter)
                End Using
            End Using
            _render = "<noscript>" & sb.ToString & "</noscript>"
        End If
        If i > 0 Then
            Dim s As String = ""
            s &= "$(function () {$('img.lazyImgLoad').lazyload("
            s &= renderparams()
            s &= ");"
            s &= "});"
            jQueryLibrary.jQueryInclude.addScriptBlock(Page, s, False, , "LazyImgLoad")
            Me.ID = Origid
            Dim orig As String = ImageUrl
            ImageUrl = PreLoadImage
            Me.Attributes.Remove("data-original")
            Me.Attributes.Add("data-original", orig)
            If Not Me.CssClass.Contains("lazyImgLoad") Then
                Me.CssClass &= " lazyImgLoad"
                Me.CssClass = Me.CssClass.Trim
            End If
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        If i > 0 Then
            writer.Write(_render)
        End If
        i += 1
    End Sub
End Class
