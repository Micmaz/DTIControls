Imports BaseClasses

Public Class ForgotPassword
    Inherits DTIServerControls.DTIServerBase

#Region "Control Properties"
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

    Private _LinkExpirationMinutes As Integer = 60
    Public Property LinkExpirationMinutes As Integer
        Get
            Return _LinkExpirationMinutes
        End Get
        Set(value As Integer)
            _LinkExpirationMinutes = value
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
#End Region

    Private _DisableDefaultStyle As Boolean

    ''' <summary>
    ''' Remove defualt style of change password control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Remove defualt style of change password control")> _
    Public Property DisableDefaultStyle As Boolean
        Get
            Return _DisableDefaultStyle
        End Get
        Set(value As Boolean)
            _DisableDefaultStyle = value
        End Set
    End Property

    Private WithEvents forgot As New ForgotPasswordControl

    Private Sub ForgotPassword_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim pantheman As New Panel
        Me.Controls.Add(pantheman)
        forgot = CType(Me.Page.LoadControl("~/res/DTIAdminPanel/ForgotPasswordControl.ascx"), ForgotPasswordControl)
        With forgot
            .MessageText = MessageText
            .EmailNotFoundText = EmailNotFoundText
            .EmailSentText = EmailSentText
            .EmailSubject = EmailSubject
            .EmailText = EmailText
            .ChangePasswordURL = ChangePasswordURL
            .LinkExpirationHours = LinkExpirationMinutes
        End With

        pantheman.Controls.Add(forgot)

        If Not DisableDefaultStyle Then
            'jQueryLibrary.jQueryInclude.addStyleBlock(Page, ".Kurk-login {width:240px} " & _
            '        ".Kurk-Spacer {clear:both; height:10px} " & _
            '        ".Kurk-Error {width: 240px} " & _
            '        ".Kurk-tbUser {width: 240px} " & _
            '        ".Kurk-tbPass { width: 240px} " & _
            '        ".Kurk-Remember {font-size: .8em;float:left} " & _
            '        ".Kurk-btnLogin {float:right} " & _
            '        ".Kurk-Forgot {font-size: .8em} ")
        End If
    End Sub

    Public Shared Function CreateChangePassCode(ByRef User As dsDTIAdminPanel.DTIUsersRow, ByVal MinutesTilLinkExpires As Double, _
                                                Optional ByVal EncryptionKey As String = "U?Ae26P7Z`#Mg@l>&sxJ?kLX2Kwg/S;g") As String
        Dim linkData As String = BaseClasses.EncryptionHelper.Encrypt(User.Id & "@" & Now.AddMinutes(MinutesTilLinkExpires).ToString & _
                                                                      "@" & User.Guid, EncryptionKey)
        Return HttpUtility.UrlEncode(linkData)
    End Function

    Public Shared Function GetUserFromPassCode(ByVal Code As String, Optional ByVal DecryptionKey As String = "U?Ae26P7Z`#Mg@l>&sxJ?kLX2Kwg/S;g") As dsDTIAdminPanel.DTIUsersRow
        Code = HttpUtility.UrlDecode(Code).Replace(" ", "+")
        Dim linkData As String = BaseClasses.EncryptionHelper.Decrypt(Code, DecryptionKey)
        Dim s As String() = linkData.Split("@"c)
        Dim id As Integer = -1
        Dim dt As DateTime = Nothing
        Dim gid As String = Nothing

        If s.Length = 3 Then
            Try
                id = Integer.Parse(s(0))
                dt = DateTime.Parse(s(1))
                gid = s(2)
            Catch ex As Exception
                Return Nothing
            End Try
        Else : Return Nothing
        End If

        If id <> -1 AndAlso dt <> Nothing AndAlso gid IsNot Nothing AndAlso Now <= dt Then
            Dim dtUsers As New dsDTIAdminPanel.DTIUsersDataTable
            BaseHelper.getHelper.FillDataTable("Select * from DTIUsers where Id=@id AND Guid=@gid", dtUsers, id, gid)

            If dtUsers.Count = 1 Then
                Return dtUsers(0)
            Else : Return Nothing
            End If
        Else : Return Nothing
        End If
    End Function
End Class
