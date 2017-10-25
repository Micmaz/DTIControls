Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Reflection
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses.Scripts

#If DEBUG Then
Public Class DivRotator
    Inherits Panel
#Else
        <AspNetHostingPermission(SecurityAction.Demand, _
            Level:=AspNetHostingPermissionLevel.Minimal), _
        AspNetHostingPermission(SecurityAction.InheritanceDemand, _
            Level:=AspNetHostingPermissionLevel.Minimal), _
        DefaultProperty("Container"), _
        ToolboxData("<{0}:DivRotator runat=""server""> </{0}:DivRotator>")> _
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class DivRotator
            Inherits Panel
#End If

#Region "Properties"
    <Browsable(False)> _
    Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
        Get
            Return System.Web.UI.HtmlTextWriterTag.Script
        End Get
    End Property

    Private _ContainerIdManual As String

    ''' <summary>
    ''' Property to get/set the manual Container ID
    ''' </summary>
    ''' <value>
    ''' ContainerIDManual string passed to the set method
    ''' </value>
    ''' <returns>
    ''' ContainerIDManual string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("Options"), _
    Description("Property to get/set the manual Container ID") _
    > _
    Public Property ContainerIdManual() As String
        Get
            Return _ContainerIdManual
        End Get
        Set(ByVal value As String)
            If value.Length < 1 OrElse value = "" Then
                _ContainerIdManual = Nothing
            Else
                _ContainerIdManual = value
            End If
        End Set
    End Property

    Private _enabled As Boolean = True

    ''' <summary>
    ''' Property to get/set whether or not the rotator is enabled
    ''' </summary>
    ''' <value>
    ''' enabled boolean passed to the set method
    ''' </value>
    ''' <returns>
    ''' enabled boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("Options"), _
    Description("Property to get/set whether or not the rotator is enabled") _
    > _
    Public Property RotatorEnabled() As Boolean
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
        End Set
    End Property

    Private _random As Boolean = False

    ''' <summary>
    ''' Property to get/set whether or not the rotator randomizes slides
    ''' </summary>
    ''' <value>
    ''' random boolean passed to the set method
    ''' </value>
    ''' <returns>
    ''' random boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("Options"), _
    Description("Property to get/set whether or not the rotator randomizes slides") _
    > _
    Public Property random() As Boolean
        Get
            Return _random
        End Get
        Set(ByVal value As Boolean)
            _random = value
        End Set
    End Property

    Private _container As WebControl

    ''' <summary>
    ''' Property to get/set the Container
    ''' </summary>
    ''' <value>
    ''' Webcontrol passed to the set method
    ''' </value>
    ''' <returns>
    ''' container webcontrol returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("Options"), _
    Description("Property to get/set the Container"), _
    IDReferencePropertyAttribute(GetType(Object)) _
    > _
    Public Property Container() As WebControl
        Get
            Return _container
        End Get
        Set(ByVal value As WebControl)
            _container = value
        End Set
    End Property

    Private _fx As String = "scrollRight"

    ''' <summary>
    ''' Property to get/set the Sroll Right effects during transition
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "scrollRight"
    ''' </value>
    ''' <returns>
    ''' fx string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("scrollRight"), _
    Description("Property to get/set the Sroll Right effects during transition") _
    > _
    Public Property fx() As String
        Get
            Return _fx
        End Get
        Set(ByVal value As String)
            _fx = value
        End Set
    End Property

    Private _timeout As Integer = 4000

    ''' <summary>
    ''' Property to get/set the timeout between slide transition in milliseconds
    ''' </summary>
    ''' <value>
    ''' Integer passed to the set method
    ''' Default Value: 4000
    ''' </value>
    ''' <returns>
    ''' timeout integer returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(4000), _
    Description("Property to get/set the timeout between slide transition in milliseconds") _
    > _
    Public Property timeout() As Integer
        Get
            Return _timeout
        End Get
        Set(ByVal value As Integer)
            _timeout = value
        End Set
    End Property

    Private _continuous As Boolean = False

    ''' <summary>
    ''' Property to get/set the Continuous option which determines whether or not the next transition will start immediately
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: False
    ''' </value>
    ''' <returns>
    ''' continuous boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(False), _
    Description("Property to get/set the Continuous option which determines whether or not the next transition will start immediately") _
    > _
    Public Property continuous() As Boolean
        Get
            Return _continuous
        End Get
        Set(ByVal value As Boolean)
            _continuous = value
        End Set
    End Property

    Private _speed As Integer = 1000

    ''' <summary>
    ''' Property to get/set the Speed of the transition
    ''' </summary>
    ''' <value>
    ''' integer passed to the set method
    ''' Default Value: 1000
    ''' </value>
    ''' <returns>
    ''' speed integer returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(1000), _
    Description("Property to get/set the Speed of the transition") _
    > _
    Public Property speed() As Integer
        Get
            Return _speed
        End Get
        Set(ByVal value As Integer)
            _speed = value
        End Set
    End Property

    Private _speedIn As String = "null"

    ''' <summary>
    ''' Property to get/set the Speed of the 'in' transition
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' speedIn string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Speed of the 'in' transition") _
    > _
    Public Property speedIn() As String
        Get
            Return _speedIn
        End Get
        Set(ByVal value As String)
            _speedIn = value
        End Set
    End Property

    Private _speedOut As String = "null"

    ''' <summary>
    ''' Property to get/set the Speed of the 'out' transition
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' speedOut string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Speed of the 'out' transition") _
    > _
    Public Property speedOut() As String
        Get
            Return _speedOut
        End Get
        Set(ByVal value As String)
            _speedOut = value
        End Set
    End Property

    Private _nextId As String = "null"

    ''' <summary>
    ''' Property to get/set the Next ID selector for the element to use as a click trigger for the next slide
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' nextID string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Next ID selector for the element to use as a click trigger for the next slide") _
    > _
    Public Property [next]() As String
        Get
            Return _nextId
        End Get
        Set(ByVal value As String)
            _nextId = value
        End Set
    End Property

    Private _prevId As String = "null"

    ''' <summary>
    ''' Property to get/set the Prev ID selector for the element to use as a click trigger for the previous slide
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' prevID string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Prev ID selector for the element to use as a click trigger for the previous slide") _
    > _
    Public Property prev() As String
        Get
            Return _prevId
        End Get
        Set(ByVal value As String)
            _prevId = value
        End Set
    End Property

    Private _easing As String = "null"

    ''' <summary>
    ''' Property to get/set the Easing method for both in and out transitions
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' easing string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Easing method for both in and out transitions") _
    > _
    Public Property easing() As String
        Get
            Return _easing
        End Get
        Set(ByVal value As String)
            _easing = value
        End Set
    End Property

    Private _easeIn As String = "null"

    ''' <summary>
    ''' Property to get/set the Ease In for ""in"" transitions
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' easeIn string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Ease In for """"in"""" transitions") _
    > _
    Public Property easeIn() As String
        Get
            Return _easeIn
        End Get
        Set(ByVal value As String)
            _easeIn = value
        End Set
    End Property

    Private _easeOut As String = "null"

    ''' <summary>
    ''' Property to get/set the Ease Out for out transitions
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' easeOut string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Ease Out for out transitions") _
    > _
    Public Property easeOut() As String
        Get
            Return _easeOut
        End Get
        Set(ByVal value As String)
            _easeOut = value
        End Set
    End Property

    Private _shuffle As String = "null"

    ''' <summary>
    ''' Property to get/set the Shuffle coordinates for the shuffle animation
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: "null"
    ''' </value>
    ''' <returns>
    ''' shuffle string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue("null"), _
    Description("Property to get/set the Shuffle coordinates for the shuffle animation") _
    > _
    Public Property shuffle() As String
        Get
            Return _shuffle
        End Get
        Set(ByVal value As String)
            _shuffle = value
        End Set
    End Property

    Private _pause As Boolean = False

    ''' <summary>
    ''' Property to get/set the Pause which enables/disables 'pause on hover'
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: False
    ''' </value>
    ''' <returns>
    ''' pause boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(False), _
    Description("Property to get/set the Pause which enables/disables 'pause on hover'") _
    > _
    Public Property pause() As Boolean
        Get
            Return _pause
        End Get
        Set(ByVal value As Boolean)
            _pause = value
        End Set
    End Property

    Private _delay As Integer = 0

    ''' <summary>
    ''' Property to get/set the Delay in milliseconds for the first transition
    ''' </summary>
    ''' <value>
    ''' Integer passed to the set method
    ''' Default Value: 0
    ''' </value>
    ''' <returns>
    ''' delay integer returned by the get method
    ''' </returns>
    ''' <remarks>
    ''' Can be negative
    ''' </remarks>
    <Category("ScriptOptions"), _
    DefaultValue(0), _
    Description("Property to get/set the Delay in milliseconds for the first transition") _
    > _
    Public Property delay() As Integer
        Get
            Return _delay
        End Get
        Set(ByVal value As Integer)
            _delay = value
        End Set
    End Property

    Private _nowrap As Boolean = False

    ''' <summary>
    ''' Property to get/set the No Wrap which enables/disables slideshow wrapping
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: False
    ''' </value>
    ''' <returns>
    ''' nowrap boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(False), _
    Description("Property to get/set the No Wrap which enables/disables slideshow wrapping") _
    > _
    Public Property nowrap() As Boolean
        Get
            Return _nowrap
        End Get
        Set(ByVal value As Boolean)
            _nowrap = value
        End Set
    End Property

    Private _requeueTimeout As Integer = 250

    ''' <summary>
    ''' Property to get/set the Requeue Timeout in ms 
    ''' </summary>
    ''' <value>
    ''' Integer passed to the set method
    ''' Default Value: 0
    ''' </value>
    ''' <returns>
    ''' requeueTimeout integer returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
    DefaultValue(0), _
    Description("Property to get/set the Requeue Timeout in ms") _
    > _
    Public Property requeueTimeout() As Integer
        Get
            Return _requeueTimeout
        End Get
        Set(ByVal value As Integer)
            _requeueTimeout = value
        End Set
    End Property

    Private _onPrevNextEvent As String = "null"

    ''' <summary>
    ''' callback fn for prev/next events: function(isNext, zeroBasedSlideIndex, slideElement)  
    ''' </summary>
    ''' <value>
    ''' string passed to the set method
    ''' </value>
    ''' <returns>
    ''' onPrevNextEvent string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <Category("ScriptOptions"), _
        DefaultValue("null"), _
        Description("callback fn for prev/next events: function(isNext, zeroBasedSlideIndex, slideElement)") _
        > _
    Public Property onPrevNextEvent() As String
        Get
            Return _onPrevNextEvent
        End Get
        Set(ByVal value As String)
            _onPrevNextEvent = value
        End Set
    End Property

#End Region

    Private Sub DivRotator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.cycle.all.min.js", , True)
    End Sub

    Private Sub DivRotator_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Container.Style.Add("visibility", "hidden")
        Me.Attributes.Add("language", "javascript")
        Me.Attributes.Add("type", "text/javascript")
        Dim scrpt As New Literal()
        Me.Controls.Add(scrpt)
        scrpt.Text = "$(window).ready(function(){" & vbCrLf
        'scrpt.Text = "window.onload = function(){" & vbCrLf
        If Me.ContainerIdManual Is Nothing Then
            scrpt.Text &= "$('#" & Me.Container.ClientID & "').cycle({"
        Else
            scrpt.Text &= "$('#" & Me.ContainerIdManual & "').cycle({"
        End If
        Dim removeLastComma As Boolean = False
        Dim properties As PropertyInfo() = GetType(DivRotator).GetProperties
        For Each prop As PropertyInfo In properties
            Dim vAttribA As Object() = prop.GetCustomAttributes(GetType(CategoryAttribute), True)
            If vAttribA.Length > 0 AndAlso DirectCast(vAttribA(0), CategoryAttribute).Category = "ScriptOptions" Then
                Dim orig As Object = prop.GetValue(Me, Nothing)
                Dim addValue As String
                If TypeOf orig Is String AndAlso orig <> "null" Then
                    addValue = "'" & orig & "',"
                ElseIf TypeOf orig Is Boolean Then
                    addValue = orig.ToString.ToLower & ","
                Else
                    addValue = orig.ToString & ","
                End If
                scrpt.Text &= vbCrLf & prop.Name & ":     " & addValue
                removeLastComma = True
            End If
        Next
        If removeLastComma Then
            scrpt.Text = scrpt.Text.Remove(scrpt.Text.Length - 1)
        End If
        scrpt.Text &= "});" & vbCrLf & "$('#" & Me.Container.ClientID & "').css('visibility','');" & vbCrLf & "});"
    End Sub
End Class
