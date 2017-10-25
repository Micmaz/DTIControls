

''' <summary>
''' Checks Password strengh against a Brute Force Attack
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Checks Password strengh against a Brute Force Attack")> _
Public Class PasswordStrength
    Inherits Panel

    Private _PasswordBox As String

    ''' <summary>
    ''' The ID of the Textbox to check password strength
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The ID of the Textbox to check password strength")> _
    Public Property PasswordTextBoxId() As String
        Get
            Return _PasswordBox
        End Get
        Set(ByVal value As String)
            _PasswordBox = value
        End Set
    End Property

    Private Sub PasswordStrength_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIAdminPanel/jquery.chronoStrength.js")
    End Sub

    Private Sub PasswordStrength_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If String.IsNullOrEmpty(Me.CssClass) Then
            Me.CssClass = "password-strength"
        End If
        Me.Style.Add("display", "none")
    End Sub

    Private Sub PasswordStrength_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim str As String = "$(function(){"
        str &= "ChronoStrengthGo('" & PasswordTextBoxId & "','" & Me.CssClass & "');"
        str &= "     });" & vbCrLf
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, str)
    End Sub
End Class
