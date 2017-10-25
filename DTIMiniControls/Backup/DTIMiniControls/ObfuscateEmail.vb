Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

<ComponentModel.ToolboxItem(False), _
AspNetHostingPermission(SecurityAction.Demand, _
    Level:=AspNetHostingPermissionLevel.Minimal), _
AspNetHostingPermission(SecurityAction.InheritanceDemand, _
    Level:=AspNetHostingPermissionLevel.Minimal), _
ToolboxData("<{0}:ObfuscateEmail runat=""server""> </{0}:ObfuscateEmail>")> _
Public Class ObfuscateEmail
    Inherits Web.UI.WebControls.Literal

    <Category("Appearance"), _
    Description("Email address to be sent to.") _
    > _
    Private _address As String = ""

    ''' <summary>
    ''' Property to get/set the email address to be sent to
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' address string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set the email address to be sent to")> _
    Public Property Address() As String
        Get
            Return _address
        End Get
        Set(ByVal value As String)
            _address = value
        End Set
    End Property

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If String.IsNullOrEmpty(Me.Text) Then
            Me.Text = "<script language='javascript' type='text/javascript'>/* <![CDATA[ */document.write(base64Decode('" & encodeToBase64(Me.Address) & "'));/* ]]> */</script>"
        End If
        writer.Write("<a href='javascript:var e0=obfEmail(""" & encodeToBase64(Me.Address) & """);(window.location?window.location.replace(e0):document.write(e0));'>" & Text & "</a>")
    End Sub

    ''' <summary>
    ''' Decodes a given string from Base 64 encoding 
    ''' </summary>
    ''' <param name="value">
    ''' String to be decoded
    ''' </param>
    ''' <returns>
    ''' Decoded string
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Decodes a given string from Base 64 encoding")> _
    Public Function decodeFromBase64(ByVal value As String) As String
        Return System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(value))
    End Function

    ''' <summary>
    ''' Encodes a given string into Base 64
    ''' </summary>
    ''' <param name="value">
    ''' String to be encoded
    ''' </param>
    ''' <returns>
    ''' Encoded string
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Encodes a given string into Base 64")> _
    Public Function encodeToBase64(ByVal value As String) As String
        Return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(value))
    End Function

    Private Sub Base_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/base64.js")
    End Sub
End Class
