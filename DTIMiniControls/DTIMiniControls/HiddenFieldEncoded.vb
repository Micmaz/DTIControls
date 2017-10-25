Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class HiddenFieldEncoded
    Inherits Web.UI.WebControls.HiddenField
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    ToolboxData("<{0}:HiddenFieldEncoded runat=""server""> </{0}:HiddenFieldEncoded>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class HiddenFieldEncoded
        Inherits Web.UI.WebControls.HiddenField
#End If
        Private reference As String = Nothing

        ''' <summary>
        ''' Reference ID
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' reference string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Reference ID")> _
        Public Property ReferenceId() As String
            Get
                Return reference
            End Get
            Set(ByVal value As String)
                If value = "" Then
                    value = Nothing
                End If
                reference = value
            End Set
        End Property

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)
            Dim script As New ScriptBlock

            Dim setEncodedScript As String = ""
            If ReferenceId Is Nothing Then
                setEncodedScript = "$.urlAddWatch('" & Me.ClientID & "', '" & Me.ClientID & "');"
            Else
                setEncodedScript = "$.urlAddWatch('" & Me.ReferenceId & "', '" & Me.ClientID & "');"
            End If
            script.ScriptText = _
                "        $(document).ready(function() {  " & vbCrLf & _
                "          " & setEncodedScript & vbCrLf & _
                "        });  " & vbCrLf
            '    "          $('form').submit(function() {  " & vbCrLf & _
            '    "            $.base64EncodePairs(); " & vbCrLf & _
            '    "            return true;  " & vbCrLf & _
            '    "          }); " & vbCrLf & _
            script.RenderControl(writer)
        End Sub

        Protected Overrides Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean
            MyBase.LoadPostData(postDataKey, postCollection)
            Me.Value = TextBoxEncoded.decodeFromURL(Me.Value)
            Return True
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("")> _
        Private Sub Base_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.urlencode.js", , True)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.form.js", , True)
            'Page.ClientScript.RegisterOnSubmitStatement(GetType(TextBoxEncoded), "Encode64forPost", "resetEncodedboxes()")
        End Sub

        Private Sub HiddenFieldEncoded_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Me.Page.ClientScript.RegisterOnSubmitStatement(Me.GetType(), "hiddenfield", "$.urlEncodePairs();")
        End Sub
    End Class
