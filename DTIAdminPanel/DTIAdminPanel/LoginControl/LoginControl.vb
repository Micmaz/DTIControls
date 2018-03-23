Imports DTIServerControls
Imports System.Configuration.ConfigurationManager
Imports BaseClasses

''' <summary>
''' A Control to add login ability for the content management system. 
''' Default user is:     DTIAdmin
''' Default Password is: DTIPass
''' User is prompted for a new user/pw after the first login.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("A Control to add login ability for the content management system.  Default user is:   DTIAdmin Default Password is: DTIPass User is prompted for a new user/pw after the first login."), ToolboxData("<{0}:LoginControl ID=""LoginControl"" runat=""server"" />")> _
Public Class LoginControl
    Inherits DTIServerBase

#Region "Properties"

    ''' <summary>
    ''' Eumeration of the login modes.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Eumeration of the login modes.")> _
    Public Enum LoginModes
        EditOn = 0
        LayoutOn = 1
        Preview = 2
        LoggedOut = 3
    End Enum

    ''' <summary>
    ''' Gets or sets the different loging modes for editing text, changing layout, and logging out of 
    ''' the admin panel
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the different loging modes for editing text, changing layout, and logging out of    the admin panel")> _
    Public Shared Property LoginMode() As LoginModes
        Get
            If DTIServerControls.DTISharedVariables.LoggedIn Then
                If DTIServerControls.DTISharedVariables.AdminOn Then
                    DTIServerControls.DTISharedVariables.LayoutOn = False
                    Return LoginModes.EditOn
                ElseIf DTIServerControls.DTISharedVariables.LayoutOn Then
                    DTIServerControls.DTISharedVariables.AdminOn = False
                    Return LoginModes.LayoutOn
                Else
                    DTIServerControls.DTISharedVariables.AdminOn = False
                    DTIServerControls.DTISharedVariables.LayoutOn = False
                    Return LoginModes.Preview
                End If
            Else
                DTIServerControls.DTISharedVariables.AdminOn = False
                DTIServerControls.DTISharedVariables.LayoutOn = False
                Return LoginModes.LoggedOut
            End If
        End Get
        Set(ByVal value As LoginModes)
            Select Case value
                Case LoginModes.EditOn
                    DTIServerControls.DTISharedVariables.LoggedIn = True
                    DTIServerControls.DTISharedVariables.AdminOn = True
                    DTIServerControls.DTISharedVariables.LayoutOn = False
                Case LoginModes.LayoutOn
                    DTIServerControls.DTISharedVariables.LoggedIn = True
                    DTIServerControls.DTISharedVariables.LayoutOn = True
                    DTIServerControls.DTISharedVariables.AdminOn = False
                Case LoginModes.LoggedOut
                    DTIServerControls.DTISharedVariables.LoggedIn = False
                    DTIServerControls.DTISharedVariables.AdminOn = False
                    DTIServerControls.DTISharedVariables.LayoutOn = False
                Case LoginModes.Preview
                    DTIServerControls.DTISharedVariables.LayoutOn = False
                    DTIServerControls.DTISharedVariables.AdminOn = False
                    DTIServerControls.DTISharedVariables.LoggedIn = True
            End Select
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the login state
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the login state")> _
    Public Shared Property isLoggedIn() As Boolean
        Get
            Return DTIServerControls.DTISharedVariables.LoggedIn
        End Get
        Set(ByVal value As Boolean)
            If Not value Then
                DTIServerControls.DTISharedVariables.LayoutOn = False
                DTIServerControls.DTISharedVariables.AdminOn = False
            End If
            DTIServerControls.DTISharedVariables.LoggedIn = value
        End Set
    End Property

    Private _maxLoginAttempts As Integer = 10

    ''' <summary>
    ''' Number of consective login failures before account is locked
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Number of consective login failures before account is locked")> _
    Public Property MaxLoginAttempts As Integer
        Get
            Return _maxLoginAttempts
        End Get
        Set(value As Integer)
            _maxLoginAttempts = value
        End Set
    End Property

    Private _loginFailureWaittime As Integer = 60

    ''' <summary>
    ''' Number of minutes to wait before being able to log back in after account it locked
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Number of minutes to wait before being able to log back in after account it locked")> _
    Public Property LoginFailureWaitTime As Integer
        Get
            Return _loginFailureWaittime
        End Get
        Set(value As Integer)
            _loginFailureWaittime = value
        End Set
    End Property

    Private _DisableDefaultStyle As Boolean

    ''' <summary>
    ''' Remove defualt style of login control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Remove defualt style of login control")> _
    Public Property DisableDefaultStyle As Boolean
        Get
            Return _DisableDefaultStyle
        End Get
        Set(value As Boolean)
            _DisableDefaultStyle = value
        End Set
    End Property

    Public ReadOnly Property login As String
        Get
            Try
                Return Me.logUser.tbUser.Text
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property
    Public ReadOnly Property password As String
        Get
            Try
                Return Me.logUser.tbPass.Text
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

#End Region

#Region "User Control Properties"
    Private _isSiteEditLogin As Boolean = True

    ''' <summary>
    ''' If true all DTIControls items (editpanel, adminpanel, sortable) will be activated for site editing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If true all DTIControls items (editpanel, adminpanel, sortable) will be activated for site editing")> _
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
    ''' If set to true user will automatacally be redirected back to the current page after successfull login
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true user will automatacally be redirected back to the current page after successfull login")> _
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
    ''' If set to true the site will be placed into Edit Mode on successfull login
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true the site will be placed into Edit Mode on successfull login")> _
    Public Property LoginAndEditOn() As Boolean
        Get
            Return _loginAndEditOn
        End Get
        Set(ByVal value As Boolean)
            _loginAndEditOn = value
        End Set
    End Property

    Private _EnablePasswordStrength As Boolean = True

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

    ''' <summary>
    ''' Text show for the forgot password link.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Must set ForgotPasswordLink</remarks>
    <System.ComponentModel.Description("Text show for the forgot password link.")> _
    Public Property ForgotPasswordText() As String
        Get
            Return _forgotPasswordText
        End Get
        Set(ByVal value As String)
            _forgotPasswordText = value
        End Set
    End Property

    Private _forgotPasswordLink As String = ""

    ''' <summary>
    ''' Url for the forgot password page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Url for the forgot password page")> _
    Public Property ForgotPasswordURL() As String
        Get
            Return _forgotPasswordLink
        End Get
        Set(ByVal value As String)
            _forgotPasswordLink = value
        End Set
    End Property

    Private _enableRememberMe As Boolean = True

    ''' <summary>
    ''' Enables remember me checkbox for loging in with cookies
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables remember me checkbox for loging in with cookies")> _
    Public Property EnableRememberMe As Boolean
        Get
            Return _enableRememberMe
        End Get
        Set(value As Boolean)
            _enableRememberMe = value
        End Set
    End Property

    Private _RememberMeText As String = "Remember Me"

    ''' <summary>
    ''' Text to display for the Remember me checkbox
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display for the Remember me checkbox")> _
    Public Property RememberMeText() As String
        Get
            Return _RememberMeText
        End Get
        Set(ByVal value As String)
            _RememberMeText = value
        End Set
    End Property

    Private _rememberMeTimeout As Integer = 30

    ''' <summary>
    ''' Number of days past current time to set cookie expiration date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Number of days past current time to set cookie expiration date")> _
    Public Property RememberMeTimeout() As Integer
        Get
            Return _rememberMeTimeout
        End Get
        Set(ByVal value As Integer)
            _rememberMeTimeout = value
        End Set
    End Property

    Private _AccountLockedMessage As String = "You Account has been locked"

    ''' <summary>
    ''' Error message to display when a account has been locked due to failed logins
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Error message to display when a account has been locked due to failed logins")> _
    Public Property AccountLockedMessage() As String
        Get
            Return _AccountLockedMessage
        End Get
        Set(ByVal value As String)
            _AccountLockedMessage = value
        End Set
    End Property

    Private _LoginFailedMessage As String = "Username and/or password is incorrect"

    ''' <summary>
    ''' Error message to display on unsuccessful login
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Error message to display on unsuccessful login")> _
    Public Property LoginFailedMessage() As String
        Get
            Return _LoginFailedMessage
        End Get
        Set(ByVal value As String)
            _LoginFailedMessage = value
        End Set
    End Property
#End Region

#Region "Events"

	''' <summary>
	''' fires on a sucessfull login
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("fires on a sucessfull login")>
	Public Event LoggedIn(sender As Object, e As loginEventArgs)
	Public Class loginEventArgs : Inherits EventArgs
		Public loggedInUser As dsDTIAdminPanel.DTIUsersRow
	End Class

	''' <summary>
	''' fires after logout is pressed
	''' </summary>
	''' <remarks></remarks>
    <System.ComponentModel.Description("fires after logout is pressed")> _
    Public Event LoggedOut()

    ''' <summary>
    ''' fires on a failed login
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("fires on a failed login")> _
    Public Event LoginFailed()
#End Region

    Private WithEvents logUser As New LoginUserControl

    Private Sub LoginControl_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        If AppSettings("DTIAdminUser") Is Nothing AndAlso _
                AppSettings("DTIAdminPass") Is Nothing Then
            sqlhelper.checkAndCreateTable(New dsDTIAdminPanel.DTIUsersDataTable)
        End If
    End Sub

    Private Sub LoginControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim pantheman As New Panel
        Me.Controls.Add(pantheman)
        logUser = CType(Me.Page.LoadControl("~/res/DTIAdminPanel/LoginUserControl.ascx"), LoginUserControl)
        With logUser
            .AutoRedirect = AutoRedirect
            .LoginAndEditOn = LoginAndEditOn
            .EnablePasswordStrength = EnablePasswordStrength
            .ForgotPasswordLink = ForgotPasswordURL
            .ForgotPasswordText = ForgotPasswordText
            .EnableRememberMe = EnableRememberMe
            .RememberMeText = RememberMeText
            .RememberMeTimeout = RememberMeTimeout
            .isSiteEditLogin = isSiteEditLogin
            .MaxLoginAttempts = MaxLoginAttempts
            .LoginFailureWaitTime = LoginFailureWaitTime
            .MainID = MainID
        End With

        pantheman.Controls.Add(logUser)

        If Not DisableDefaultStyle Then
            jQueryLibrary.jQueryInclude.addStyleBlock(Page, ".Kurk-login {width:240px} " & _
                    ".Kurk-Spacer {clear:both; height:10px} " & _
                    ".Kurk-Error {width: 240px} " & _
                    ".Kurk-tbUser {width: 240px} " & _
                    ".Kurk-tbPass { width: 240px} " & _
                    ".Kurk-Remember {font-size: .8em;float:left} " & _
                    ".Kurk-btnLogin {float:right} " & _
                    ".Kurk-Forgot {font-size: .8em} ")
        End If
    End Sub

#Region "Event Handling"
	Private Sub logUser_LoggedIn(ByVal LoggedInUser As dsDTIAdminPanel.DTIUsersRow) Handles logUser.LoggedIn
		Dim e As New loginEventArgs
		e.loggedInUser = LoggedInUser
		RaiseEvent LoggedIn(Me, e)
	End Sub

	Private Sub logUser_LoginFailed() Handles logUser.LoginFailed
        RaiseEvent LoginFailed()
    End Sub

    Private Sub logUser_LoggedOut() Handles logUser.LoggedOut
        RaiseEvent LoggedOut()
    End Sub
#End Region

#Region "private shared"
    Private Shared Function isMemberLocked(ByRef user As dsDTIAdminPanel.DTIUsersRow, ByVal LoginFailureWaittime As Integer) As Boolean
        If LoginFailureWaittime = -1 Then Return False
        If user.IsLockoutDateTimeNull Then
            Return False
        Else
            Return user.LockoutDateTime > Now.AddMinutes(-1 * LoginFailureWaittime)
        End If
    End Function

    Private Shared Sub UpdateLastLogin(ByRef CurrentUser As dsDTIAdminPanel.DTIUsersRow)
        If CurrentUser IsNot Nothing Then
            'CurrentUser.Guid = Guid.NewGuid.ToString
            CurrentUser.LastLoginDate = CurrentUser.LoginDate
            CurrentUser.LoginDate = Now
            CurrentUser.LastIpAddress = CurrentUser.IpAddress
            CurrentUser.IpAddress = HttpContext.Current.Request.UserHostAddress
            CurrentUser.LoginFailureCount = 0
            CurrentUser.SetLockoutDateTimeNull()
            BaseClasses.BaseHelper.getHelper.Update(CurrentUser.Table)
        End If
    End Sub

    Private Shared Function UpdateLoginFailure(ByRef CurrentUser As dsDTIAdminPanel.DTIUsersRow, ByVal MaxLoginAttempts As Integer, Optional ByVal LoginFailureWaitTime As Integer = 60) As Boolean
        If MaxLoginAttempts > 0 Then
            If CurrentUser.LoginFailureCount = MaxLoginAttempts Then
                CurrentUser.LoginFailureCount = 0
                CurrentUser.SetLockoutDateTimeNull()
            End If
            If CurrentUser IsNot Nothing Then
                CurrentUser.LoginFailureCount += 1
            End If
            If CurrentUser.LoginFailureCount = MaxLoginAttempts Then
                CurrentUser.LockoutDateTime = Now
            End If
            BaseClasses.BaseHelper.getHelper.Update(CurrentUser.Table)
        End If
    End Function
#End Region

#Region "Depricated"

    ''' <summary>
    ''' Generates a MD5 hash of a given string
    ''' </summary>
    ''' <param name="input">String to hash</param>
    ''' <returns>MD5 hash of String</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Generates a MD5 hash of a given string"), _
    Obsolete("This method is deprecated, use BaseClasses.Hashing.GetHash() instead.")> _
    Public Shared Function CreateMD5Hash(ByVal input As String) As String
        Dim hash As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()
        Dim inputBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(input)
        Dim hashBytes As Byte() = hash.ComputeHash(inputBytes)
        Dim sb As New StringBuilder()

        For i As Integer = 0 To hashBytes.Length - 1
            sb.Append(hashBytes(i).ToString("x2"))
        Next
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' returns id of User if username and hashed password is in table.  
    ''' Also hashes any passwords not generated by this method and replaces password.
    ''' </summary>
    ''' <param name="dtusers"></param>
    ''' <param name="Username"></param>
    ''' <param name="password"></param>
    ''' <param name="salt"></param>
    ''' <param name="UserNameField"></param>
    ''' <param name="PasswordField"></param>
    ''' <param name="IdField"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("returns true if username and hashed password is in table.    Also hashes any passwords not generated by this method and replaces password."), _
     Obsolete("This method is Depricated.  PasswordHasher is a better alorightm but requires database changes")> _
    Public Shared Function isHashedValue(ByRef dtusers As DataTable, ByVal Username As String, ByVal password As String, Optional ByVal salt As String = "", _
                                        Optional ByVal UserNameField As String = "username", Optional ByVal PasswordField As String = "password", Optional ByVal IdField As String = "Id") As Integer
        Dim dv As New DataView(dtusers, UserNameField & " = '" & Username & "'", "", DataViewRowState.CurrentRows)
        If dv.Count > 0 Then
            Dim userhash As String = CreateMD5Hash(dv(0)(IdField).ToString)
            Dim passToHash As String = password & salt & Username.ToLower
            Dim hash As String = CreateMD5Hash(passToHash) & userhash

            dv.RowFilter = UserNameField & " = '" & Username & "' AND " & PasswordField & "='" & hash & "'"

            If dv.Count > 0 Then
                If String.Equals(dv(0)(PasswordField).ToString, hash, StringComparison.InvariantCulture) Then
                    Return DirectCast(dv(0)(IdField), Integer)
                Else : Return -1
                End If
            Else
                dv.RowFilter = UserNameField & " = '" & Username & "' AND " & PasswordField & "='" & password & "'"
                If dv.Count > 0 AndAlso String.Equals(dv(0)(PasswordField).ToString, password, StringComparison.InvariantCulture) Then
                    Dim drows As DataRow() = dtusers.Select(IdField & "=" & dv(0)(IdField).ToString)
                    Dim pas As String = drows(0)(PasswordField).ToString
                    If Not pas.EndsWith(userhash) Then
                        drows(0)(PasswordField) = CreateMD5Hash(passToHash) & userhash
                        BaseClasses.BaseHelper.getHelper.Update(dtusers)
                        Return DirectCast(drows(0)(IdField), Integer)
                    Else : Return -1
                    End If
                Else : Return -1
                End If
            End If
        Else
            Return -1
        End If
    End Function
#End Region

#Region "Shared Functions"

    ''' <summary>
    ''' Checks if Users Password is correct and hashed.  If password is not hashed this function will hash the
    ''' password
    ''' </summary>
    ''' <param name="CurrentUser">User to check</param>
    ''' <param name="password">Password to check</param>
    ''' <param name="iterations">Iterations to make if password is not hashed</param>
    ''' <returns>Currentuser if login successfull, Nothing if login fails</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Checks if Users Password is correct and hashed. If password is not hashed this function will hash the   password")> _
    Public Shared Function PasswordHashAndCheck(ByRef CurrentUser As dsDTIAdminPanel.DTIUsersRow, ByVal password As String, _
                                                Optional ByVal iterations As Integer = 5000) As dsDTIAdminPanel.DTIUsersRow
        If CurrentUser.Password <> password Then
            If Hashing.CheckPBKDF2Hash(password & CurrentUser.Username, CurrentUser.Salt, CurrentUser.Password) Then
                Return CurrentUser
            Else : Return Nothing
            End If
        Else
            Dim passToHash As String = password & CurrentUser.Username
            Dim newSalt As String = ""
            Dim hash As String = Hashing.SecureHash(passToHash, newSalt, iterations)
            CurrentUser.Password = hash
            CurrentUser.Salt = newSalt
            CurrentUser.Guid = Guid.NewGuid.ToString
            BaseHelper.getHelper.Update(CurrentUser.Table)
            Return CurrentUser
        End If
    End Function

    ''' <summary>
    ''' returns id of User if username and hashed password is in table.  
    ''' Also hashes any passwords not generated by this method and replaces password.
    ''' </summary>
    ''' <param name="dtusers"></param>
    ''' <param name="Username"></param>
    ''' <param name="password"></param>
    ''' <param name="UserNameField"></param>
    ''' <param name="PasswordField"></param>
    ''' <param name="IdField"></param>
    ''' <param name="SaltField"></param>
    ''' <param name="iterations"></param>
    ''' <returns></returns>
    ''' <remarks>Requires dtusers to contain user trying to login</remarks>
    <System.ComponentModel.Description("returns true if username and hashed password is in table.    Also hashes any passwords not generated by this method and replaces password.")> _
    Public Shared Function PasswordHashAndCheck(ByRef dtusers As DataTable, ByVal Username As String, ByVal password As String, _
                                        Optional ByVal UserNameField As String = "username", Optional ByVal PasswordField As String = "password", _
                                        Optional ByVal IdField As String = "Id", Optional ByVal SaltField As String = "Salt", _
                                        Optional ByVal iterations As Integer = 5000) As Integer
        If Username Is Nothing OrElse Username = "" OrElse password Is Nothing OrElse password = "" Then
            Return -1
        End If

        Dim dv As New DataView(dtusers, UserNameField & " = '" & Username & "'", "", DataViewRowState.CurrentRows)
        If dv.Count = 1 Then
            Dim StoredPassword As String = dv(0)(PasswordField).ToString
            Dim storedSalt As String = dv(0)(SaltField).ToString
            Dim id As Integer = DirectCast(dv(0)(IdField), Integer)
            Username = dv(0)(UserNameField).ToString
            'Dim userhash As String = Hashing.GetHash(Username, Hashing.HashTypes.MD5)
            'check if password is stored in plain text
            dv.RowFilter = UserNameField & " = '" & Username & "' AND " & PasswordField & "='" & password & "'"

            If dv.Count = 0 Then
                'If Hashing.CheckPBKDF2Hash(password & Username, storedSalt, StoredPassword.Replace(userhash, "")) Then
                If Hashing.CheckPBKDF2Hash(password & Username, storedSalt, StoredPassword) Then
                    Return id
                Else : Return -1
                End If
            Else
                Dim drows As DataRow() = dtusers.Select(IdField & "=" & id.ToString)

                'If Not StoredPassword.EndsWith(userhash) Then
                Dim passToHash As String = password & Username
                Dim newSalt As String = ""
                Dim hash As String = Hashing.SecureHash(passToHash, newSalt, iterations)
                drows(0)(PasswordField) = hash '& userhash
                drows(0)(SaltField) = newSalt
                BaseHelper.getHelper.Update(dtusers)
                Return DirectCast(drows(0)(IdField), Integer)
                'Else : Return -1
                'End If
            End If
        ElseIf dv.Count > 1 Then
            Throw New Exception("Multiple users with the same username exist in the database")
        Else
            Return -1
        End If
    End Function

    ''' <summary>
    ''' Adds user to the DTIUsersDataTable.  Takes care of hashing their password
    ''' </summary>
    ''' <param name="Username"></param>
    ''' <param name="password"></param>
    ''' <param name="email"></param>
    ''' <param name="isAdmin"></param>
    ''' <param name="isActive"></param>
    ''' <param name="MainID"></param>
    ''' <param name="iterations"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds user to the DTIUsersDataTable. Takes care of hashing their password")> _
    Public Shared Function createUser(ByVal Username As String, ByVal password As String, _
                                        ByVal email As String, Optional ByVal isAdmin As Boolean = True, Optional ByVal isActive As Boolean = True, _
                                        Optional ByVal MainID As Long = 0, _
                                        Optional ByVal iterations As Integer = 5000) As dsDTIAdminPanel.DTIUsersRow
        Dim helper As BaseHelper = BaseHelper.getHelper
        Dim dtusers As New dsDTIAdminPanel.DTIUsersDataTable
        helper.checkAndCreateTable(dtusers)
        Dim newUser As dsDTIAdminPanel.DTIUsersRow = dtusers.NewDTIUsersRow

        Dim passtoHash As String = password & Username
        Dim Salt As String = ""
        Dim hash As String = Hashing.SecureHash(passtoHash, Salt, iterations)
        Dim ip As String = HttpContext.Current.Request.UserHostAddress
        With newUser
            .Username = Username
            .Password = hash
            .Salt = Salt
            .MainID = MainID
            .Email = email
            .Guid = Guid.NewGuid.ToString
            .isAdmin = isAdmin
            .IpAddress = ip
            .LastIpAddress = ip
            .isActive = isActive
            .DateAdded = Now
            .LoginDate = Now
            .LastLoginDate = Now
            .PasswordChange = False
            .PasswordChangeDate = Now
            .SetLockoutDateTimeNull()
            .LoginFailureCount = 0
        End With
        dtusers.AddDTIUsersRow(newUser)
        helper.Update(CType(dtusers, DataTable))

        Return newUser
    End Function

    ''' <summary>
    ''' Processes user's login request.  Return row of user if successful or nothing if not successfull.  
    ''' Throws exception if user is required to change thier password, if their account is 
    ''' locked due to exceeding the max login attempts, or if user doesn't exist.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="password"></param>
    ''' <param name="mainID"></param>
    ''' <param name="MaxLoginAttempts"></param>
    ''' <param name="LoginFailureWaitTime"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Processes user's login request. Return row of user if successful or nothing if not successfull.    Throws exception if user is required to change thier password, if their account is    locked due to exceeding the max login attempts, or if user doesn't exist.")> _
    Public Shared Function loginUser(ByVal username As String, ByVal password As String, Optional MainID As Long = 0, _
                                     Optional ByVal MaxLoginAttempts As Integer = 10, Optional ByVal LoginFailureWaitTime As Integer = 1) As dsDTIAdminPanel.DTIUsersRow
        Dim dtusers As New dsDTIAdminPanel.DTIUsersDataTable
        Dim mainIDStr As String = " AND (MainID=0 OR MainID=@mainid)"
        If MainID = -1 Then
            mainIDStr = ""
        End If
        BaseClasses.BaseHelper.getHelper.FillDataTable("Select * from DTIUsers where username like @username" & mainIDStr, dtusers, username, MainID)

        If dtusers.Count >= 1 Then
            Dim CurrentUser As dsDTIAdminPanel.DTIUsersRow = dtusers(0)

            If CurrentUser.isActive AndAlso Not isMemberLocked(CurrentUser, LoginFailureWaitTime) Then
                CurrentUser = LoginControl.PasswordHashAndCheck(CurrentUser, password)
                If CurrentUser IsNot Nothing Then
                    If CurrentUser.PasswordChange Then
                        Throw New ChangePasswordException
                    End If
                    UpdateLastLogin(CurrentUser)
                    Return CurrentUser
                Else
                    UpdateLoginFailure(CurrentUser, MaxLoginAttempts, LoginFailureWaitTime)
                    Return Nothing
                End If
            ElseIf isMemberLocked(CurrentUser, LoginFailureWaitTime) Then
                Throw New AccountLockedException
            End If
        ElseIf dtusers.Count > 1 Then
            Throw New Exception("Usernames must be unique")
        Else
            Throw New NoUserException
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Changes password to new hash
    ''' </summary>
    ''' <param name="CurrentUser"></param>
    ''' <param name="NewPassword"></param>
    ''' <param name="iterations"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Changes password to new hash")> _
    Public Shared Function ChangePassword(ByRef CurrentUser As dsDTIAdminPanel.DTIUsersRow, ByVal NewPassword As String, _
                                          Optional ByVal iterations As Integer = 5000) As Boolean
        Dim passToHash As String = NewPassword & CurrentUser.Username
        Dim newSalt As String = ""
        Dim hash As String = Hashing.SecureHash(passToHash, newSalt, iterations)

        With CurrentUser
            .Password = hash
            .Salt = newSalt
            .Guid = Guid.NewGuid.ToString
            .PasswordChange = False
            .PasswordChangeDate = Now
        End With

        BaseHelper.getHelper.Update(CurrentUser.Table)
        Return True
    End Function

    ''' <summary>
    ''' Add encrypted LoginControl cookie for automatic logins
    ''' </summary>
    ''' <param name="username">Username to save</param>
    ''' <param name="password">Password to save</param>
    ''' <param name="DaysTilExpiration">Number of days until cookie expires</param>
    ''' <param name="cookieName">Name of cookie</param>
    ''' <param name="EncryptionKey">Key used to encrypt cookie data</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Add encrypted LoginControl cookie for automatic logins")> _
    Public Shared Sub SetUserCookie(ByVal username As String, ByVal password As String, Optional ByVal DaysTilExpiration As Integer = 30, _
                                Optional ByVal cookieName As String = "DTIControls", _
                                Optional ByVal EncryptionKey As String = "x146:GnP.2g)$UVec}x,aZj:A1l7w%Om")
        Dim uname As String = BaseClasses.EncryptionHelper.Encrypt(username, EncryptionKey)
        Dim upass As String = BaseClasses.EncryptionHelper.Encrypt(password, EncryptionKey)

        Dim cookie As HttpCookie = New HttpCookie(BaseSecurityPage.ValidCookieName(cookieName))
        With cookie
            .HttpOnly = True
            .Values.Add("5qWwYHoxEc", uname)
            .Values.Add("30dO4oY5o6", upass)
            .Expires = DateTime.Now.AddDays(DaysTilExpiration)
        End With
        HttpContext.Current.Response.Cookies.Add(cookie)
    End Sub

    ''' <summary>
    ''' Decrypts LoginControl cookie
    ''' </summary>
    ''' <param name="username">Stores user's username</param>
    ''' <param name="password">Stores user's password</param>
    ''' <param name="cookieName">Name of cookie</param>
    ''' <param name="DecryptionKey">Key used to decrypt cookie data</param>
    ''' <remarks>DecryptionKey must be the same as the EncryptionKey used in SetUserCookie</remarks>
    <System.ComponentModel.Description("Decrypts LoginControl cookie")> _
    Public Shared Sub GetUserCookie(ByRef username As String, ByRef password As String, _
                                Optional ByVal cookieName As String = "DTIControls", _
                                Optional ByVal DecryptionKey As String = "x146:GnP.2g)$UVec}x,aZj:A1l7w%Om")
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(BaseSecurityPage.ValidCookieName(cookieName))
        If cookie IsNot Nothing Then
            Dim u As String = cookie("5qWwYHoxEc")
            Dim p As String = cookie("30dO4oY5o6")
            If u IsNot Nothing And p IsNot Nothing Then
                username = BaseClasses.EncryptionHelper.Decrypt(u, DecryptionKey)
                password = BaseClasses.EncryptionHelper.Decrypt(p, DecryptionKey)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Marks cookie as expired so the browser will delete it.
    ''' </summary>
    ''' <param name="cookieName"></param>
    ''' <remarks>You cannot directly delete a cookie on a user's computer. 
    ''' However, you can direct the user's browser to delete the cookie by 
    ''' setting the cookie's expiration date to a past date.
    ''' http://msdn.microsoft.com/en-us/library/ms178195.aspx
    ''' </remarks>
    <System.ComponentModel.Description("Marks cookie as expired so the browser will delete it.")> _
    Public Shared Sub RemoveCookie(Optional ByVal cookieName As String = "DTIControls")
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(BaseSecurityPage.ValidCookieName(cookieName))
        If cookie IsNot Nothing Then
            cookie.Expires = DateTime.Now.AddDays(-1)
            HttpContext.Current.Response.Cookies.Add(cookie)
        End If
    End Sub

    ''' <summary>
    ''' Attempts Loing using stored cookie (if it exists)
    ''' </summary>
    ''' <param name="MainId"></param>
    ''' <param name="cookieName"></param>
    ''' <returns>CurrentUser if successful otherwise Nothing</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Attempts Loing using stored cookie (if it exists)")> _
    Public Shared Function CookieLogin(Optional ByVal MainId As Long = 0, Optional ByVal cookieName As String = "DTIControls") As dsDTIAdminPanel.DTIUsersRow
        Dim username As String = Nothing
        Dim password As String = Nothing
        GetUserCookie(username, password)
        If username IsNot Nothing AndAlso password IsNot Nothing Then
            Try
                Return loginUser(username, password, MainId, 1, -1)
            Catch ex As Exception
                RemoveCookie(cookieName)
                Return Nothing
            End Try
        End If
        Return Nothing
    End Function
#End Region

End Class

#Region "Exceptions"
Public Class ChangePasswordException
    Inherits Exception

    Public Sub New()
        MyBase.New("Your password has expired and needs to be changed")
    End Sub
End Class

Public Class AccountLockedException
    Inherits Exception

    Public Sub New()
        MyBase.New("Your account has been locked for exceeding the max number of failed logins")
    End Sub
End Class

Public Class NoUserException
    Inherits Exception

    Public Sub New()
        MyBase.New("This User Does not exist")
    End Sub
End Class
#End Region
