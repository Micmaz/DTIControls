Partial Public Class _Default
    Inherits System.Web.UI.Page

    Public ReadOnly Property userID() As String
        Get
            Try
                Return User.Identity.Name.Substring(User.Identity.Name.LastIndexOf("\") + 1)
            Catch e As Exception
            End Try
            Return Nothing
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.tbMasked.Text = "1231231234"

        End If
        ajax1.addControlsToWatchList(pnl1)
        'jQueryLibrary.ThemeAdder.AddTheme(Me, Nothing, True, True, False, False)

        dlg1.addButtonFromIframe("Button1", "Test", False)
        'DropDownList()
        JsAjaxCall1.watchedControlList.Add(Me.Button1)
        JsAjaxCall1.watchedControlList.Add(Me.TextBox1)
        JsAjaxCall1.watchedControlList.Add(Me.CheckBox1)
        JsAjaxCall1.watchedControlList.Add(Me.DropDownList1)
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'mailhandler("test", "neil@digitaltadpole.com", "public@digitaltadpole.com", "test")
        Dim x As String = tbMasked.unMaskedText
        InfoDiv1.text = "Test Button Error " & colorpicker1.colorValue
    End Sub

    Public Enum enableSSl
        Auto = 2
        [True] = 1
        [False] = 0
    End Enum

    Public Sub mailhandler(ByVal body As String, ByVal toaddress As String, ByVal fromaddress As String, ByVal subject As String, _
                            Optional ByVal ishtml As Boolean = True, Optional ByVal enableSSl As enableSSl = enableSSl.Auto)

        Dim emailmsg As New Net.Mail.MailMessage()
        With emailmsg
            .From = New Net.Mail.MailAddress(fromaddress)
            .Body = body
            .IsBodyHtml = ishtml
        End With

        Dim tos() As String = toaddress.Split(New Char() {";"}, System.StringSplitOptions.RemoveEmptyEntries)
        For Each t As String In tos
            emailmsg.To.Add(t)
        Next

        Dim client As New Net.Mail.SmtpClient
        Select Case enableSSl
            Case _Default.enableSSl.Auto
                client.EnableSsl = client.Port = 465 OrElse client.Port = 587
            Case Else
                client.EnableSsl = Boolean.Parse(CType(enableSSl, Integer))
        End Select
        client.Send(emailmsg)
    End Sub

    Private Sub JsAjaxCall1_callBack(ByVal sender As JqueryUIControls.AjaxCall, ByVal query As String) Handles JsAjaxCall1.callBack
        Button1.Text = TextBox1.Text
        For Each notifyKey As String In JqueryUIControls.Notify_SiteWide.notificationKeys
            'If notifyKey <> not1.NotifyKey Then _
            not1.sendNotification(notifyKey, TextBox1.Text & "<br/> Test html", "Alert Sent to:" & notifyKey, 0, Me.DropDownList1.SelectedValue)
        Next
        'sendmessage = True
        'message = TextBox1.Text
        sender.respond("ok")
    End Sub

    Shared x As Integer = 1
    Friend WithEvents not1 As New JqueryUIControls.Notify_SiteWide
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        not1.ID = "not1"
        JqueryUIControls.Notify_SiteWide.notificationKeyDefault = userID
        not1.ChromeNotifications = True
        Me.form1.Controls.Add(not1)

        JqueryUIControls.Share.BindToEnum(GetType(JqueryUIControls.Notify.NotifyStyle), Me.DropDownList1)
        'If not1.notificationKeyDefault Is Nothing Then
        '    not1.notificationKeyDefault = Guid.NewGuid().ToString
        'End If

        'If Me.Session("x") Is Nothing Then
        '    Me.Session("x") = x
        '    x += 1
        'End If
        'not1.NotifyKey = Me.Session("x")
    End Sub

    'Public Shared sendmessage As Boolean = False
    'Public Shared message = ""
    'Private Sub not1_alertCheck(ByVal sender As Object, ByVal args As EventArgs) Handles not1.alertCheck
    '    If sendmessage Then
    '        sendmessage = False
    '        not1.respond("Alert!", message)
    '    End If
    'End Sub

    Private Sub not1_alertClicked(ByVal sender As Object, ByVal title As String, ByVal message As String) Handles not1.alertClicked

    End Sub

    Private Sub addUC()
        pnl1.Controls.Add(Me.LoadControl("currentTime.ascx"))
    End Sub

    Private Sub ajax1_callBack(ByVal sender As JqueryUIControls.AjaxCall, ByVal value As String) Handles ajax1.callBack
        addUC()
        sender.respond("ok")
    End Sub


End Class