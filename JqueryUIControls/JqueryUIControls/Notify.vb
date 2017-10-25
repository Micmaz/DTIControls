Public Class Notify
    Inherits Panel

#Region "Properties"

    Private textValue As String

    ''' <summary>
    ''' The text body of the notification
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The text body of the notification")> _
    Public Property text() As String
        Get
            Return textValue
        End Get
        Set(ByVal value As String)
            textValue = value
        End Set
    End Property

    Private titleValue As String

    ''' <summary>
    ''' Title of the notification
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Title of the notification")> _
    Public Property title() As String
        Get
            Return titleValue
        End Get
        Set(ByVal value As String)
            titleValue = value
        End Set
    End Property

    Private expiresValue As Integer = 0

    ''' <summary>
    ''' The amount of time in ms the notification will stay visable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The amount of time in ms the notification will stay visable")> _
    Public Property expires() As Integer
        Get
            Return expiresValue
        End Get
        Set(ByVal value As Integer)
            expiresValue = value
        End Set
    End Property

    ''' <summary>
    ''' Helper to make alert stay on the page untill closed. Sets expire to 0 if set to true.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Helper to make alert stay on the page untill closed. Sets expire to 0 if set to true.")> _
    Public Property sticky() As Boolean
        Get
            Return expires = 0
        End Get
        Set(ByVal value As Boolean)
            If value Then expires = 0
            If Not value Then
                If expires <= 0 Then
                    expires = 5000
                End If
            End If
        End Set
    End Property

    Private speedValue As Integer = 500

    ''' <summary>
    ''' The animation speed in ms
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The animation speed in ms")> _
    Public Property speed() As Integer
        Get
            Return speedValue
        End Get
        Set(ByVal value As Integer)
            speedValue = value
        End Set
    End Property

    Private stackValue As stackDirection = stackDirection.below

    ''' <summary>
    ''' The direction new notifications stack in relation to this one.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The direction new notifications stack in relation to this one.")> _
    Public Property stack() As stackDirection
        Get
            Return stackValue
        End Get
        Set(ByVal value As stackDirection)
            stackValue = value
        End Set
    End Property

    Private styleNotifyValue As NotifyStyle = NotifyStyle.default

    ''' <summary>
    ''' Style the notify message
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Style the notify message")> _
    Public Property theme() As NotifyStyle
        Get
            Return styleNotifyValue
        End Get
        Set(ByVal value As NotifyStyle)
            styleNotifyValue = value
        End Set
    End Property


    Private checkIntervalValue As Integer = 1000

    ''' <summary>
    ''' The interval the server is checked for a new notification
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The interval the server is checked for a new notification")> _
    Public Property checkInterval() As Integer
        Get
            Return checkIntervalValue
        End Get
        Set(ByVal value As Integer)
            If value > 0 AndAlso value < 200 Then value = 200
            checkIntervalValue = value
        End Set
    End Property



    Private showOnLoadValue As Boolean

    ''' <summary>
    ''' Show the notification on page load
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Show the notification on page load")> _
    Public Property showOnLoad() As Boolean
        Get
            Return showOnLoadValue
        End Get
        Set(ByVal value As Boolean)
            showOnLoadValue = value
        End Set
    End Property


    Private alignVValue As alignVlocation = alignVlocation.top

    ''' <summary>
    ''' Verticle alignment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Verticle alignment")> _
    Public Property alignV() As alignVlocation
        Get
            Return alignVValue
        End Get
        Set(ByVal value As alignVlocation)
            alignVValue = value
        End Set
    End Property


    Private alignHValue As alignHlocation = alignHlocation.right

    ''' <summary>
    ''' Horizontal alignment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Horizontal alignment")> _
    Public Property alignH() As alignHlocation
        Get
            Return alignHValue
        End Get
        Set(ByVal value As alignHlocation)
            alignHValue = value
        End Set
    End Property


#End Region

#Region "Enums"
    Public Enum NotifyStyle
        [default]
        [error]
        [info]
    End Enum

    Public Enum StackDirection
        above
        below
    End Enum

    Enum alignHlocation
        left
        right
    End Enum

    Enum alignVlocation
        top
        bottom
    End Enum
#End Region

    Friend WithEvents ajax As New AjaxCall

    Public Event alertCheck(ByVal sender As Object, ByVal args As EventArgs)
    Public Event alertClicked(ByVal sender As Object, ByVal title As String, ByVal message As String)

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.topzindex.min.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.notify.min.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/ui.notify.css")
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ajax.returnFormat = AjaxCall.returnCall.Javascript
        ajax.ID = "ajax_" & Me.ID
        Me.Controls.Add(ajax)
        registerControl(Me.Page)
    End Sub

    Public Function renderShowScript(Optional ByVal chromnotify As Boolean = False)
        If chromnotify Then
            Return String.Format("{0}('{1}','{2}','{3}',12);" & vbCrLf, Me.ID, BaseClasses.BaseSecurityPage.JavaScriptEncode(Me.title), BaseClasses.BaseSecurityPage.JavaScriptEncode(Me.text), [Enum].GetName(GetType(NotifyStyle), Me.theme))
        End If
        Return String.Format("{0}('{1}','{2}','{3}');" & vbCrLf, Me.ID, BaseClasses.BaseSecurityPage.JavaScriptEncode(Me.title), BaseClasses.BaseSecurityPage.JavaScriptEncode(Me.text), [Enum].GetName(GetType(NotifyStyle), Me.theme))
    End Function

    Public Function renderShowScriptFunction()
        Dim positionStr As String = ""
        If Me.alignH = alignHlocation.left Then positionStr &= "left: 0pt;"
        If Me.alignV = alignVlocation.bottom Then positionStr &= "bottom: 0pt;"
        Dim outstr As String = vbCrLf & "function " & Me.ID & "(inTitle,inText,theme,showChromeNotify){" & vbCrLf
        outstr &= "appendNotifyContainer('container_" & Me.ID & "','" & positionStr & "');" & vbCrLf
        outstr &= "$('#container_" & Me.ID & "').notify('create', theme+'-notifytemplate', { title: inTitle, text: inText }, { " & vbCrLf
        outstr &= jsPropString("expires", expires)
        outstr &= jsPropString("speed", speed)
        If stack = StackDirection.above Then
            jsPropString("stack", "above")
        End If
        outstr &= vbCrLf & "open:function(e,instance){$(this).parent().topZIndex();}," & vbCrLf
        outstr &= vbCrLf & "click:function(e,instance){instance.close();}," & vbCrLf
        outstr &= vbCrLf & "close:function(e,instance){" & vbCrLf & "ajax_" & Me.ID & "('clicked##'+inTitle+'##'+inText);}" & vbCrLf
        outstr &= "});" & vbCrLf
        outstr &= "if(showChromeNotify)chromeNotification(inTitle,inText,'',function(e,instance){ ajax_" & Me.ID & "('clicked##'+inTitle+'##'+inText);});" & vbCrLf
        outstr &= "}" & vbCrLf
        Return outstr
    End Function

    Protected Overridable Function getInitialScript()
        Dim str As String = "<script type=""text/javascript"">"
        str &= renderShowScriptFunction()
        If Me.showOnLoad Then
            str &= "$(function(){"
            str &= renderShowScript()
            str &= "        });"
        End If
        If checkInterval > 0 Then
            str &= "$(function(){ setInterval( 'ajax_" & Me.ID & "()'," & Me.checkInterval & "); });"
        End If
        str &= "</script>"
        Return str
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write(getInitialScript)
        MyBase.Render(writer)
    End Sub

    Private Sub ajax_callBack(ByVal sender As AjaxCall, ByVal value As String) Handles ajax.callBack
        If value.StartsWith("clicked") Then
            Dim vals As String() = value.Split(New String() {"##"}, StringSplitOptions.None)
            RaiseEvent alertClicked(Me, vals(1), vals(2))
        Else
            RaiseEvent alertCheck(Me, EventArgs.Empty)
        End If
        ajax.respond(Nothing)
    End Sub

    Public Sub respond()
        ajax.respond(Nothing)
    End Sub

    Public Sub respond(ByVal Title As String, ByVal text As String)
        Me.title = Title
        Me.text = text
        ajax.respond("", renderShowScript())
    End Sub

    Private Sub Notify_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ajax.jsFunction = "ajax_" & Me.ID
    End Sub
End Class

