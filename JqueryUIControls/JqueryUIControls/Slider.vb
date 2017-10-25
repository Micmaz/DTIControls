

''' <summary>
''' The jQuery UI Slider plugin makes selected elements into sliders. 
''' There are various options such as multiple handles, and ranges. 
''' The handle can be moved with the mouse or the arrow keys.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("The jQuery UI Slider plugin makes selected elements into sliders.  There are various options such as multiple handles, and ranges.  The handle can be moved with the mouse or the arrow keys.")> _
Public Class Slider
    Inherits Panel

#Region "properties"
    Private _max As Integer = 100

    ''' <summary>
    ''' The maximum value of the slider.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The maximum value of the slider.")> _
    Public Property Max() As Integer
        Get
            Return _max
        End Get
        Set(ByVal value As Integer)
            _max = value
        End Set
    End Property

    Private _min As Integer = 0

    ''' <summary>
    ''' The minimum value of the slider.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The minimum value of the slider.")> _
    Public Property Min() As Integer
        Get
            Return _min
        End Get
        Set(ByVal value As Integer)
            _min = value
        End Set
    End Property

    Public Enum Position
        Vertical
        Horizontal
    End Enum

    Private _orientation As Position = Position.Horizontal

    ''' <summary>
    ''' This option determines whether the slider has the min at the left, 
    ''' the max at the right or the min at the bottom, the max at the top. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This option determines whether the slider has the min at the left,    the max at the right or the min at the bottom, the max at the top.")> _
    Public Property Orientation() As Position
        Get
            Return _orientation
        End Get
        Set(ByVal value As Position)
            _orientation = value
        End Set
    End Property

    Public Enum limit
        none
        [true]
        min
        max
    End Enum

    Private _range As limit = limit.none

    ''' <summary>
    ''' If set to true, the slider will detect if you have two handles and create a stylable range 
    ''' element between these two. Two other possible values are 'min' and 'max'. 
    ''' A min range goes from the slider min to one handle. 
    ''' A max range goes from one handle to the slider max
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, the slider will detect if you have two handles and create a stylable range    element between these two. Two other possible values are 'min' and 'max'.    A min range goes from the slider min to one handle.    A max range goes from one handle to the slider max")> _
    Public Property Range() As limit
        Get
            Return _range
        End Get
        Set(ByVal value As limit)
            _range = value
        End Set
    End Property

    Private _step As Integer = 1

    ''' <summary>
    ''' Determines the size or amount of each interval or step the slider takes between min and max. 
    ''' The full specified value range of the slider (max - min) needs to be evenly divisible by 
    ''' the step
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines the size or amount of each interval or step the slider takes between min and max.    The full specified value range of the slider (max - min) needs to be evenly divisible by    the step")> _
    Public Property [Step]() As Integer
        Get
            Return _step
        End Get
        Set(ByVal value As Integer)
            _step = value
        End Set
    End Property

    Private _value As String = ""

    ''' <summary>
    ''' Determines the value of the slider, if there's only one handle. 
    ''' If there is more than one handle, determines the value of the first handle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines the value of the slider, if there's only one handle.    If there is more than one handle, determines the value of the first handle")> _
    Public Property Value() As String
        Get
            If Me.DesignMode Then Return _value
            If Not Me.Page Is Nothing AndAlso Not Me.Page.IsPostBack Then
                Return _value
            End If
            Dim val As String = Me.Page.Request.Params(Me.ClientID & "_hidden")
            If val IsNot Nothing Then
                val = val.TrimEnd(",")
            Else
                val = ""
            End If
            If val = "" AndAlso Values.Count > 0 Then
                For Each i As Integer In Values
                    val &= i & ","
                Next
                val = val.TrimEnd(",")
            End If
            If val = "" Then
                val = "0"
            End If
            Return val
        End Get
        Set(ByVal value2 As String)
            _value = value2
        End Set
    End Property

    Private _values As IntegerCollection

    ''' <summary>
    ''' This option can be used to specify multiple handles. If range is set to true, 
    ''' the count of 'values' should be 2.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This option can be used to specify multiple handles. If range is set to true,    the count of 'values' should be 2.")> _
    Public ReadOnly Property Values() As IntegerCollection
        Get
            If _values Is Nothing Then
                _values = New IntegerCollection
                '_values.Add(Value)
            End If
            Return _values
        End Get
    End Property

    Private _animate As SliderAnimation = Nothing

    ''' <summary>
    ''' Whether to slide handle smoothly when user click outside handle on the bar. 
    ''' Will also accept one of the three predefined speeds ("slow", "normal", or "fast") 
    ''' or the number of milliseconds to run the animation (e.g. 1000)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Whether to slide handle smoothly when user click outside handle on the bar.    Will also accept one of the three predefined speeds (""slow"", ""normal"", or ""fast"")    or the number of milliseconds to run the animation (e.g. 1000)")> _
    Public Property Animate() As SliderAnimation
        Get
            Return _animate
        End Get
        Set(ByVal value As SliderAnimation)
            _animate = value
        End Set
    End Property

    Private _updateControl As Control

    ''' <summary>
    ''' Updates value of control as slider is slid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Updates value of control as slider is slid.")> _
    Public Property UpdateControl() As Control
        Get
            Return _updateControl
        End Get
        Set(ByVal value As Control)
            _updateControl = value
        End Set
    End Property
#End Region

#Region "Callbacks"
    Private _createCallback As String

    ''' <summary>
    ''' This event is triggered when slider is created.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when slider is created.")> _
    Public Property CreateCallback() As String
        Get
            Return _createCallback
        End Get
        Set(ByVal value As String)
            _createCallback = value
        End Set
    End Property

    Private _changeCallback As String

    ''' <summary>
    ''' This event is triggered on slide stop, or if the value is changed programmatically 
    ''' (by the value method). Takes arguments event and ui. Use event.orginalEvent to detect 
    ''' whether the value changed by mouse, keyboard, or programmatically. Use ui.value 
    ''' (single-handled sliders) to obtain the value of the current handle, 
    ''' $(this).slider('values', index) to get another handle's value.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered on slide stop, or if the value is changed programmatically    (by the value method). Takes arguments event and ui. Use event.orginalEvent to detect    whether the value changed by mouse, keyboard, or programmatically. Use ui.value    (single-handled sliders) to obtain the value of the current handle,    $(this).slider('values', index) to get another handle's value.")> _
    Public Property ChangeCallback() As String
        Get
            Return _changeCallback
        End Get
        Set(ByVal value As String)
            _changeCallback = value
        End Set
    End Property

    Private _startCallback As String

    ''' <summary>
    ''' This event is triggered when the user starts sliding.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when the user starts sliding.")> _
    Public Property StartCallback() As String
        Get
            Return _startCallback
        End Get
        Set(ByVal value As String)
            _startCallback = value
        End Set
    End Property

    Private _stopCallback As String

    ''' <summary>
    ''' This event is triggered when the user stops sliding.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when the user stops sliding.")> _
    Public Property StopCallback() As String
        Get
            Return _stopCallback
        End Get
        Set(ByVal value As String)
            _stopCallback = value
        End Set
    End Property

    Private _slideCallback As String

    ''' <summary>
    ''' This event is triggered on every mouse move during slide. Use ui.value 
    ''' (single-handled sliders) to obtain the value of the current handle, 
    ''' $(..).slider('value', index) to get another handles' value.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered on every mouse move during slide. Use ui.value    (single-handled sliders) to obtain the value of the current handle,    $(..).slider('value', index) to get another handles' value.")> _
    Public Property SlideCallback() As String
        Get
            Return _slideCallback
        End Get
        Set(ByVal value As String)
            _slideCallback = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""

        If Not Enabled Then
            outstr &= "disabled: true,"
        End If
        If Max <> 100 Then
            outstr &= "max: " & Max & ","
        End If
        If Min <> 0 Then
            outstr &= "min: " & Min & ","
        End If
        If Orientation <> Position.Horizontal Then
            outstr &= "orientation: 'vertical',"
        End If

        Select Case Range
            Case limit.true
                outstr &= "range: true,"
            Case limit.min
                outstr &= "range: 'min',"
            Case limit.max
                outstr &= "range: 'max',"
        End Select

        If [Step] > 1 Then
            outstr &= "step: " & [Step] & ","
        End If
        If Value <> "" AndAlso Value <> "0" AndAlso Not Value.Contains(",") Then
            outstr &= "value: " & Value & ","
        End If

        Dim valuesStr As String = ""
        If Values.Count > 1 Then
            valuesStr = "values: ["
            For Each i As Integer In Values
                valuesStr &= i & ","
            Next
            If valuesStr <> "" Then
                valuesStr = valuesStr.TrimEnd(",") & "]"
                outstr &= valuesStr & ","
            End If
        End If

        If Animate IsNot Nothing Then
            outstr &= Animate.ToString & ","
        End If

        Dim UpdateControlStr = ""
        If UpdateControl IsNot Nothing Then
            UpdateControlStr = "$('#" & UpdateControl.ClientID & "').val(ui.value);"
        End If

        'callbacks
        If CreateCallback <> "" Then
            outstr &= "create: function( event, ui ) {" & CreateCallback & "},"
        End If
        If StartCallback <> "" Then
            outstr &= "start: function( event, ui ) {" & StartCallback & "},"
        End If
        If StopCallback <> "" Then
            outstr &= "stop: function( event, ui ) {" & StopCallback & "},"
        End If
        If SlideCallback <> "" OrElse UpdateControlStr <> "" Then
            outstr &= "slide: function( event, ui ) {" & UpdateControlStr & SlideCallback & "},"
        End If

        If valuesStr = "" Then
            outstr &= "change: function( event, ui ) {$('#" & Me.ClientID & "_hidden').val(ui.value);" & ChangeCallback & "},"
        Else
            outstr &= "change: function( event, ui ){var values = $('#" & Me.ClientID & "').slider('option', 'values');var valstr = '';$.each(values, function() {valstr = valstr + this + '###';});$('#" & Me.ClientID & "_hidden').val(valstr);" & ChangeCallback & "},"
        End If

        If outstr <> "" Then
            outstr = "{" & outstr.TrimEnd(",") & "}"
        End If
        Return outstr
    End Function

    Private Sub Slider_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.DesignMode Then Return
        If Min > Max Then
            Throw New FormatException("Then Min cannot be greater than the Max")
        End If
        Dim val As String = Me.Page.Request.Params(Me.ClientID & "_hidden")
        If val IsNot Nothing Then
            Dim valarr() As String = val.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

            If valarr.Length = 1 Then
                _value = Me.Page.Request.Params(Me.ClientID & "_hidden")
            ElseIf valarr.Length > 0 Then
                _values.Clear()
                For Each s As String In valarr
                    _values.Add(s)
                Next
            End If
        End If
    End Sub

    Private Sub Slider_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim id As String = "" & Me.ID
        If id = "" Then id = ClientID

        Dim s As String = "var " & id & ";"
        s &= "$(function(){"
        s &= "     " & id & "=$('#" & Me.ClientID & "').slider("
        s &= renderparams()
        s &= "      );"
        s &= "});"
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<input type=""hidden"" value=""" & Me.Value & """ name=""" & Me.ClientID & "_hidden"" id=""" & Me.ClientID & "_hidden"" />")
        MyBase.Render(writer)
    End Sub
End Class

Public Class IntegerCollection
    Inherits List(Of Integer)
End Class

Public Class SliderAnimation
    Private SetAnimation As Boolean = False
    Private AniSpeed As Integer = 0
    Private AniTxtSpeed As speed = speed.none

    Public Enum speed
        none
        slow
        normal
        fast
    End Enum

    Public Sub New()
        SetAnimation = True
    End Sub

    Public Sub New(ByVal AnimationSpeed As speed)
        AniTxtSpeed = AnimationSpeed
    End Sub

    Public Sub New(ByVal AnimationSpeed As Integer)
        Me.AniSpeed = AnimationSpeed
    End Sub

    Public Overrides Function ToString() As String
        Dim str As String = ""
        If SetAnimation Then
            str = "animate: true"
        End If
        If AniTxtSpeed <> speed.none Then
            str = "animate: '" & [Enum].GetName(GetType(speed), AniTxtSpeed) & "'"
        End If
        If AniSpeed > 0 Then
            str = "animate: " & AniSpeed
        End If
        Return str
    End Function
End Class
