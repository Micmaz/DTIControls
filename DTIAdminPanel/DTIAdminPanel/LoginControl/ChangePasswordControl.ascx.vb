Public Class ChangePasswordControl
    Inherits BaseClasses.BaseSecurityUserControl

    Private _CurrentUser As dsDTIAdminPanel.DTIUsersRow
    Public Property CurrentUser As dsDTIAdminPanel.DTIUsersRow
        Get
            Return _CurrentUser
        End Get
        Set(value As dsDTIAdminPanel.DTIUsersRow)
            _CurrentUser = value
        End Set
    End Property

    Private _EmailText As String = Nothing
    Public Property EmailText As String
        Get
            Return _EmailText
        End Get
        Set(value As String)
            _EmailText = value
        End Set
    End Property

    Private _EmailSubject As String = Nothing
    Public Property EmailSubject As String
        Get
            Return _EmailSubject
        End Get
        Set(value As String)
            _EmailSubject = value
        End Set
    End Property

    Public Event NoUserFound()
    Public Event PasswordChanged()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("d") IsNot Nothing Then
            CurrentUser = ForgotPassword.GetUserFromPassCode(Request.QueryString("d"))
            If CurrentUser Is Nothing Then
                InfoDiv1.Visible = True
                RaiseEvent NoUserFound()
            End If
        End If
    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        If CurrentUser IsNot Nothing Then
            LoginControl.ChangePassword(CurrentUser, tbpass.Text)
            Dim host As String = Request.Url.Host
            Dim body As String = ""
            Dim subject As String = ""

            If EmailText IsNot Nothing Then
                EmailText = EmailText.Replace("##USERNAME##", CurrentUser.Username)
                body = EmailText
            Else
                body = "Dear " & CurrentUser.Username & ",<br /><br />" & _
                        "This is confirmation that you recently changed your password.<br /><br />" & _
                        "Thank you"
            End If

            If EmailSubject IsNot Nothing Then
                subject = EmailSubject
            Else
                subject = "Password Reset"
            End If
            mailhandler(body, CurrentUser.Email, "no-reply@" & host, subject)
            RaiseEvent PasswordChanged()
        Else
            InfoDiv1.Visible = True
            RaiseEvent NoUserFound()
        End If
    End Sub
End Class