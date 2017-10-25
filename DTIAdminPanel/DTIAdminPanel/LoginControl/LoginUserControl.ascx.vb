#If DEBUG Then
Partial Public Class LoginUserControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class LoginUserControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If

    Private _MainID As Long
    Public Property MainID() As Long
        Get
            Return _MainID
        End Get
        Set(ByVal value As Long)
            _MainID = value
        End Set
    End Property

#Region "Properties"
    Private _isSiteEditLogin As Boolean = True
    Public Property isSiteEditLogin() As Boolean
        Get
            Return _isSiteEditLogin
        End Get
        Set(ByVal value As Boolean)
            _isSiteEditLogin = value
        End Set
    End Property

    Private _autoRedirect As Boolean = True

    ''' <summary>
    ''' If set to true user will automatacally be redirected back to the current page.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true user will automatacally be redirected back to the current page.")> _
    Public Property AutoRedirect() As Boolean
        Get
            Return _autoRedirect
        End Get
        Set(ByVal value As Boolean)
            _autoRedirect = value
        End Set
    End Property

    Private _loginAndEditOn As Boolean = True

    ''' <summary>
    ''' If set to true the site will be placed into Edit Mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true the site will be placed into Edit Mode")> _
    Public Property LoginAndEditOn() As Boolean
        Get
            Return _loginAndEditOn
        End Get
        Set(ByVal value As Boolean)
            _loginAndEditOn = value
        End Set
    End Property

    Private _EnablePasswordStrength As Boolean = False

    ''' <summary>
    ''' Determines if the Password Strength Text is shown on creating initial login
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines if the Password Strength Text is shown on creating initial login")> _
    Public Property EnablePasswordStrength() As Boolean
        Get
            Return _EnablePasswordStrength
        End Get
        Set(ByVal value As Boolean)
            _EnablePasswordStrength = value
        End Set
    End Property

    Private _forgotPasswordText As String = "Forgot Password?"
    Public Property ForgotPasswordText() As String
        Get
            Return _forgotPasswordText
        End Get
        Set(ByVal value As String)
            _forgotPasswordText = value
        End Set
    End Property

    Private _forgotPasswordLink As String = ""
    Public Property ForgotPasswordLink() As String
        Get
            Return _forgotPasswordLink
        End Get
        Set(ByVal value As String)
            _forgotPasswordLink = value
        End Set
    End Property

    Private _enableRememberMe As Boolean = False
    Public Property EnableRememberMe As Boolean
        Get
            Return _enableRememberMe
        End Get
        Set(value As Boolean)
            _enableRememberMe = value
        End Set
    End Property

    Private _RememberMeText As String = "Remember Me"
    Public Property RememberMeText() As String
        Get
            Return _RememberMeText
        End Get
        Set(ByVal value As String)
            _RememberMeText = value
        End Set
    End Property

    Private _rememberMeTimeout As Integer = 5
    Public Property RememberMeTimeout() As Integer
        Get
            Return _rememberMeTimeout
        End Get
        Set(ByVal value As Integer)
            _rememberMeTimeout = value
        End Set
    End Property

    Private _AccountLockedMessage As String = "Your Account has been locked"
    Public Property AccountLockedMessage() As String
        Get
            Return _AccountLockedMessage
        End Get
        Set(ByVal value As String)
            _AccountLockedMessage = value
        End Set
    End Property

    Private _LoginFailedMessage As String = "Username and/or password is incorrect"
    Public Property LoginFailedMessage() As String
        Get
            Return _LoginFailedMessage
        End Get
        Set(ByVal value As String)
            _LoginFailedMessage = value
        End Set
    End Property

    Private _maxLoginAttempts As Integer = 10
    Public Property MaxLoginAttempts As Integer
        Get
            Return _maxLoginAttempts
        End Get
        Set(value As Integer)
            _maxLoginAttempts = value
        End Set
    End Property

    Private _loginFailureWaittime As Integer = 60
    Public Property LoginFailureWaitTime As Integer
        Get
            Return _loginFailureWaittime
        End Get
        Set(value As Integer)
            _loginFailureWaittime = value
        End Set
    End Property



#End Region

#Region "Events"
    Public Event attemptLogin(ByVal username As String, ByVal password As String)
	Public Event LoggedIn(ByVal CurrentUser As dsDTIAdminPanel.DTIUsersRow)
	Public Event LoginFailed()
    Public Event LoggedOut()
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loggedInControl.Visible = DTIServerControls.DTISharedVariables.LoggedIn
        adminOn.Visible = (DTIServerControls.DTISharedVariables.LoggedIn AndAlso DTIServerControls.DTISharedVariables.AdminOn)
        lblError.Visible = False

        cbRemember.Visible = EnableRememberMe
        cbRemember.Text = RememberMeText
        linkForgot.Visible = ForgotPasswordLink <> ""
        linkForgot.NavigateUrl = ForgotPasswordLink
        linkForgot.Text = ForgotPasswordText
        PasswordStrength1.Visible = EnablePasswordStrength
        pnlPwd.Visible = EnablePasswordStrength
        If EnablePasswordStrength Then
            pnlPwd.Height = New Unit(50, UnitType.Pixel)
        End If

        PasswordStrength1.PasswordTextBoxId = tbpass2.ClientID
        lblError.text = LoginFailedMessage
        lblErrorAttempts.text = AccountLockedMessage

        Dim bscript As String = "var clUser,clEmail,clPass,vlEmail,smPass;"
        bscript &= "clUser = requiredField('" & tbuser2.ClientID & "','sprequ');"
        bscript &= "clEmail = requiredField('" & tbEmail.ClientID & "','spreqe');"
        bscript &= "clPass = requiredField('" & tbpass2.ClientID & "','spreqp');"
        bscript &= "if (clEmail)"
        bscript &= "vlEmail = EmailField('" & tbEmail.ClientID & "','spEmails');"
        bscript &= "if (clPass)"
        bscript &= "smPass = compareField('" & tbpass2.ClientID & "','" & tbConfirm.ClientID & "','spConfirm');"
        bscript &= "if (clUser && clEmail && clPass && vlEmail && smPass){"
        Dim ascript As String = "$(this).dialog('close');}"
        diAdminSetup.Visible = False
        diAdminSetup.AutoOpen = False
        diAdminSetup.addButton(btnSubmit, False, bscript, ascript)

        If DTIServerControls.DTISharedVariables.LoggedIn Then
            If isSiteEditLogin Then addLogoutText()
        Else
            KurkLogin.Visible = True
            Dim user As dsDTIAdminPanel.DTIUsersRow = LoginControl.CookieLogin(MainID)
            If user IsNot Nothing Then
                RaiseEvent LoggedIn(user)
            End If
        End If
    End Sub

#Region "Button Clicks"
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If isSiteEditLogin Then
            If ConfigurationManager.AppSettings("DTIAdminUser") IsNot Nothing AndAlso _
                ConfigurationManager.AppSettings("DTIAdminPass") IsNot Nothing Then
                Dim user As String = ConfigurationManager.AppSettings("DTIAdminUser")
                Dim pass As String = ConfigurationManager.AppSettings("DTIAdminPass")
                If user = tbUser.Text AndAlso pass = tbPass.Text Then
                    LoginAdmin(0)
                Else
                    LoginAdminfailed()
                End If
            Else
                Dim count As Integer = CType(sqlHelper.FetchSingleValue("Select Count(*) from DTIUsers where isAdmin = 1"), Integer)
                If count = 0 AndAlso (tbUser.Text.ToLower = "dtiadmin" or tbUser.Text.ToLower = "admin") then 'AndAlso tbPass.Text = "DTIPass" Then
                    diAdminSetup.Visible = True
                    diAdminSetup.AutoOpen = True
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIAdminPanel/Validation.js")
                End If
            End If
        End If

        Dim currentuser As dsDTIAdminPanel.DTIUsersRow = login(tbUser.Text, tbPass.Text)
        If currentuser IsNot Nothing Then
            If cbRemember.Checked Then
                LoginControl.SetUserCookie(tbUser.Text, tbPass.Text, RememberMeTimeout)
            End If
            RaiseEvent LoggedIn(currentuser)
            If isSiteEditLogin AndAlso AutoRedirect Then
                Page.Response.Redirect(Page.Request.Url.OriginalString)
            End If
		Else
			RaiseEvent LoginFailed()
        End If
    End Sub

    Private Sub lbLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogout.Click
        logout()
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim user As dsDTIAdminPanel.DTIUsersRow = LoginControl.createUser(tbuser2.Text, tbpass2.Text, tbEmail.Text, True, True, MainID)
        login(tbuser2.Text, tbpass2.Text)
    End Sub
#End Region

#Region "Helper Functions"
    Private Function login(ByVal username As String, ByVal password As String) As dsDTIAdminPanel.DTIUsersRow
        Dim currentuser As dsDTIAdminPanel.DTIUsersRow = Nothing
        Try
            lblError.Visible = False
            lblErrorAttempts.Visible = False
            currentuser = LoginControl.loginUser(username, password, MainID, MaxLoginAttempts, LoginFailureWaitTime)
            If currentuser IsNot Nothing Then
                If isSiteEditLogin AndAlso currentuser.isAdmin Then
                    LoginAdmin(currentuser)
                End If
            End If
            lblError.Visible = currentuser Is Nothing
        Catch pc As ChangePasswordException

        Catch ac As AccountLockedException
            lblErrorAttempts.Visible = True
        Catch ex As Exception
            lblError.Visible = True
        End Try
        Return currentuser
    End Function

    Private Sub LoginAdmin()
        If LoginAndEditOn Then
            DTIServerControls.DTISharedVariables.LoggedIn = True
            DTIServerControls.DTISharedVariables.AdminOn = True
        Else
            DTIServerControls.DTISharedVariables.LoggedIn = True
            DTIServerControls.DTISharedVariables.AdminOn = False
        End If
        addLogoutText()
    End Sub

    Private Sub LoginAdmin(ByVal mainid As Long)
        LoginAdmin()
        DTIServerControls.DTISharedVariables.siteEditMainID = mainid
        If AutoRedirect Then
            Page.Response.Redirect(Page.Request.Url.OriginalString)
        End If
    End Sub

    Private Sub LoginAdmin(ByRef CurrentUser As dsDTIAdminPanel.DTIUsersRow)
        LoginAdmin()
        DTIServerControls.DTISharedVariables.siteEditMainID = CurrentUser.MainID
        If cbRemember.Checked Then
            LoginControl.SetUserCookie(tbUser.Text, tbPass.Text, RememberMeTimeout)
        End If
        RaiseEvent LoggedIn(CurrentUser)
        If AutoRedirect Then
            Page.Response.Redirect(Page.Request.Url.OriginalString)
        End If
    End Sub

    Private Sub LoginAdminfailed()
        DTIServerControls.DTISharedVariables.LoggedIn = False
        DTIServerControls.DTISharedVariables.AdminOn = False
        diAdminSetup.AutoOpen = False
        KurkLogin.Visible = True
        loggedInControl.Visible = False
        adminOn.Visible = False
        lblError.Visible = True
    End Sub

    Private Sub addLogoutText()
        diAdminSetup.AutoOpen = False
        KurkLogin.Visible = False
        loggedInControl.Visible = True
        If DTIServerControls.DTISharedVariables.AdminOn = True Then
            adminOn.Visible = True
        End If
    End Sub

    Private Sub logout()
        DTIServerControls.DTISharedVariables.LoggedIn = False
        DTIServerControls.DTISharedVariables.AdminOn = False
        diAdminSetup.AutoOpen = False
        KurkLogin.Visible = True
        loggedInControl.Visible = False
        adminOn.Visible = False
        RaiseEvent LoggedOut()
        If AutoRedirect Then
            Page.Response.Redirect(Page.Request.Url.OriginalString)
        End If
    End Sub
#End Region

End Class