Public Class Notify_SiteWide
    Inherits Notify

    Public Event notificationConfirmed(ByVal sender As Object, ByVal n As notification)

    Private redisplayUnconfirmedValue As Boolean = True
    Public Property redisplayUnconfirmed() As String
        Get
            Return redisplayUnconfirmedValue
        End Get
        Set(ByVal value As String)
            redisplayUnconfirmedValue = value
        End Set
    End Property


    Private showChromeNotificationsValue As Boolean = False
    Public Property ChromeNotifications() As Boolean
        Get
            Return showChromeNotificationsValue
        End Get
        Set(ByVal value As Boolean)
            showChromeNotificationsValue = value
        End Set
    End Property


    Private Shared defaultKeyVal As Integer = 1
    Private NotifyKeyValue As String = Nothing

    ''' <summary>
    ''' This property must be set befor or durring page init.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This property must be set befor or durring page init.")> _
    Public Property NotifyKey() As String
        Get
            If NotifyKeyValue Is Nothing Then
                If notificationKeyDefault Is Nothing Then
                    notificationKeyDefault = defaultKeyVal
                    defaultKeyVal += 1
                End If
                Return notificationKeyDefault
            Else
                Return NotifyKeyValue
            End If
        End Get
        Set(ByVal value As String)
            NotifyKeyValue = value
            If Not notificationKeyDefault Is Nothing Then
                notificationKeyDefault = value
            End If
        End Set
    End Property

    Private Shared keysession As New Hashtable
    Public ReadOnly Property notifications(Optional ByVal notifyKey As String = Nothing) As List(Of notification)
        Get
            If notifyKey Is Nothing Then notifyKey = Me.NotifyKey
            Return notificationList(notifyKey)
        End Get
    End Property

    Public Shared Property notificationKeyDefault() As String
        Get
            Return BaseClasses.DataBase.httpSession("SiteNotifyKeyDefault")
        End Get
        Set(ByVal value As String)
            If BaseClasses.DataBase.httpSession("SiteNotifyKeyDefault") IsNot Nothing Then
                If BaseClasses.DataBase.httpSession("SiteNotifyKeyDefault") = value Then
                    Return
                End If
                keysession.Remove(BaseClasses.DataBase.httpSession("SiteNotifyKeyDefault"))
            End If
            Dim i As Integer = notificationList(value).Count
            BaseClasses.DataBase.httpSession("SiteNotifyKeyDefault") = value
        End Set
    End Property

    Public Shared ReadOnly Property notificationKeys() As List(Of String)
        Get
            Dim ret As New List(Of String)
            For Each key As String In keysession.Keys
                ret.Add(key)
            Next
            Return ret
        End Get
    End Property

    Protected Shared Function notificationList(ByVal notifyKey As String) As List(Of notification)
        If Not notifyKey Is Nothing Then
            If Not keysession.Contains(notifyKey) Then
                keysession(notifyKey) = New List(Of notification)
            End If
            Return keysession(notifyKey)
        End If
        Return Nothing
    End Function

    Public Class notification
        Public shown As Boolean = False
        Public title As String
        Public text As String
        Public expires As Integer = -1
        Public theme As NotifyStyle = Nothing
    End Class

    Private Sub Notify_SiteWide_alertCheck(ByVal sender As Object, ByVal args As System.EventArgs) Handles Me.alertCheck
        If notifications.Count > 0 Then
            Dim retstr As String = ""
            Dim msgText As String = ""
            For Each n As notification In notifications
                If Not n.shown Then
                    retstr &= getRenderScript(n)
                    msgText &= n.text & " "
                End If
                n.shown = True
            Next
            ajax.respond("", retstr)
        End If
    End Sub

    Private Function getRenderScript(ByVal n As notification) As String
        If Not n.title Is Nothing Then Me.title = n.title
        If Not n.text Is Nothing Then Me.text = n.text
        'If Not n.theme = Nothing Then Me.theme = n.theme
        Me.theme = n.theme
        If Not n.expires = -1 Then Me.expires = n.expires
        Dim showChrome As Boolean = ChromeNotifications And Not n.shown
        Return renderShowScript(showChrome)
    End Function


	Protected Overrides Function getInitialScript() As String
		Dim str As String = ""
		str &= renderShowScriptFunction()
		If Me.redisplayUnconfirmedValue Then
			str &= "$(function(){"
			For Each n As notification In notifications
				str &= getRenderScript(n)
			Next
			str &= "        });"
		End If
		If checkInterval > 0 Then
			str &= "$(function(){ setInterval( 'ajax_" & Me.ID & "()'," & Me.checkInterval & "); });"
		End If
		Return str
	End Function


	Shared Sub sendNotification(ByVal notifyKey As String, ByVal message As String, Optional ByVal Title As String = Nothing, Optional ByVal expires As Integer = 0, Optional ByVal notifystyle As Notify.NotifyStyle = Nothing)
        Dim notification As New notification
        notification.title = Title
        notification.text = message
        notification.expires = expires
        notification.theme = notifystyle
        notificationList(notifyKey).Add(notification)
    End Sub

    Private Sub Notify_SiteWide_alertClicked(ByVal sender As Object, ByVal title As String, ByVal message As String) Handles Me.alertClicked
        For Each n As notification In New List(Of notification)(notifications)
            If n.title = title Then 'AndAlso n.text = message Then
                notifications.Remove(n)
                RaiseEvent notificationConfirmed(Me, n)
                Exit For
            End If
        Next
    End Sub

    'Private Sub Notify_SiteWide_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    If Not Page.IsPostBack AndAlso Not renderNotify = -1 Then
    '        Page.Response.Clear()
    '        If notifications.Count > renderNotify Then
    '            Dim n As notification = notifications.Item(renderNotify)
    '            Dim str As String = String.Format("<h2>{0}</h2>{1}", n.title, n.text)
    '            Page.Response.Write(str)
    '        End If
    '        Page.Response.End()
    '    End If

    'End Sub

End Class
