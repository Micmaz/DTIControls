Imports DTIServerControls

Public Class ChangePassword
    Inherits DTIServerBase

    Private _CurrentUser As dsDTIAdminPanel.DTIUsersRow

    ''' <summary>
    ''' User to change password for
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("User to change password for")> _
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

    Public Event NoUserFound()
    Public Event PasswordChanged()

    Private WithEvents pwChange As New ChangePasswordControl

    Private Sub ChangePassword_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim pantheman As New Panel
        Me.Controls.Add(pantheman)
        pwChange = CType(Me.Page.LoadControl("~/res/DTIAdminPanel/ChangePasswordControl.ascx"), ChangePasswordControl)
        With pwChange
            .CurrentUser = CurrentUser
            .EmailSubject = EmailSubject
            .EmailText = EmailText
        End With

        pantheman.Controls.Add(pwChange)

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

    Private Sub pwChange_NoUserFound() Handles pwChange.NoUserFound
        RaiseEvent NoUserFound()
    End Sub

    Private Sub ChangePassword_PasswordChanged() Handles Me.PasswordChanged
        RaiseEvent PasswordChanged()
    End Sub
End Class
