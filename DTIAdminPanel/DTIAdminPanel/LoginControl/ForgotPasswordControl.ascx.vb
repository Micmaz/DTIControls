Public Class ForgotPasswordControl
    Inherits BaseClasses.BaseSecurityUserControl

    Private _emailSentText As String = "Password Reset information has been sent to your email address."
    Public Property EmailSentText As String
        Get
            Return _emailSentText
        End Get
        Set(value As String)
            _emailSentText = value
        End Set
    End Property

    Private _emailNotfoundtext As String = "Your email Address was not found."
    Public Property EmailNotFoundText As String
        Get
            Return _emailNotfoundtext
        End Get
        Set(value As String)
            _emailNotfoundtext = value
        End Set
    End Property

    Private _messageText As String = "Please enter the email address associated with your account."
    Public Property MessageText As String
        Get
            Return _messageText
        End Get
        Set(value As String)
            _messageText = value
        End Set
    End Property

    Private _ChangePasswordURL As String
    Public Property ChangePasswordURL As String
        Get
            Return _ChangePasswordURL
        End Get
        Set(value As String)
            _ChangePasswordURL = value
        End Set
    End Property

    Private _LinkExpirationHours As Integer
    Public Property LinkExpirationHours As Integer
        Get
            Return _LinkExpirationHours
        End Get
        Set(value As Integer)
            _LinkExpirationHours = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If ChangePasswordURL Is Nothing Then
            Throw New Exception("Property ChangePasswordUrl is nothing.  Must have a place to change users password")
        End If
        idSent.text = EmailSentText
        idEmailError.text = EmailNotFoundText
        lblMessage.Text = MessageText
        'lblText.Text = 
    End Sub

    Private Sub btnSendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        Dim dt As New dsDTIAdminPanel.DTIUsersDataTable
        sqlHelper.FillDataTable("Select * from DTIUsers where Email = @email", dt, tbEmailRecovery.Text)
        If dt.Count = 1 Then
            Dim user As dsDTIAdminPanel.DTIUsersRow = dt(0)
            Dim host As String = Request.Url.Host

            Dim LinkData As String = ForgotPassword.CreateChangePassCode(user, LinkExpirationHours * 60)
            ChangePasswordURL = ChangePasswordURL.TrimStart("~"c)
            Dim url As String = Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & ChangePasswordURL & "?d=" & LinkData

            Dim body As String = ""
            Dim subject As String = ""

            If EmailText IsNot Nothing Then
                EmailText = EmailText.Replace("##USERNAME##", user.Username)
                EmailText = EmailText.Replace("##RESETURL##", url)
                EmailText = EmailText.Replace("##RESETTEXT##", url)
                body = EmailText
            Else
                body = "Dear " & user.Username & ",<br /><br />" & _
                        "Please click on the link below to log in to " & host & " and create a new password<br /><br />" & _
                        "<a href=""" & url & """>Click here to reset your password</a><br /><br />Thank you"
            End If

            If EmailSubject IsNot Nothing Then
                subject = EmailSubject
            Else
                subject = "Password Reset"
            End If

            mailhandler(body, user.Email, "no-reply@" & host, subject)
            idEmailError.Visible = False
            idSent.Visible = True
        Else
            idEmailError.Visible = True
            idSent.Visible = False
        End If
    End Sub
End Class