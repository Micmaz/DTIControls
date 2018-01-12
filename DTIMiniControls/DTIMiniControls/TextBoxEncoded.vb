Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class TextBoxEncoded
    Inherits Web.UI.WebControls.TextBox
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    ToolboxData("<{0}:TextBoxEncoded runat=""server""> </{0}:TextBoxEncoded>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class TextBoxEncoded
        Inherits Web.UI.WebControls.TextBox
#End If

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		MyBase.Render(writer)

		writer.Write(jQueryLibrary.jQueryInclude.isolateJquery("$.urlAddWatch('" & Me.ClientID & "', '" & Me.ClientID & "');", True))
	End Sub

        Protected Overrides Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean
            MyBase.LoadPostData(postDataKey, postCollection)
            Me.Text = decodeFromURL(Me.Text)
            Return True
        End Function

        ''' <summary>
        ''' Decodes a given string from URL encoding 
        ''' </summary>
        ''' <param name="value">
        ''' String to be decoded
        ''' </param>
        ''' <returns>
        ''' Decoded string
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Decodes a given string from URL encoding")> _
        Public Shared Function decodeFromURL(ByVal value As String) As String
            Return HttpUtility.UrlDecode(value, System.Text.Encoding.Default)
        End Function

        ''' <summary>
        ''' Encodes a given string into  string
        ''' </summary>
        ''' <param name="value">
        ''' String to be encoded
        ''' </param>
        ''' <returns>
        ''' Encoded string
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Encodes a given string into string")> _
        Public Shared Function encodeToURLString(ByVal value As String) As String
            Return HttpUtility.UrlEncode(value, System.Text.Encoding.Default)
        End Function

        Private Sub Base_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.urlencode.js", , True)
            'Page.ClientScript.RegisterOnSubmitStatement(GetType(TextBoxEncoded), "Encode64forPost", "$.base64EncodePairs()")
        End Sub

        Private Sub Event_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		Me.Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "hiddenfield", jQueryLibrary.jQueryInclude.jqueryVar & ".urlEncodePairs();")
	End Sub

    End Class
