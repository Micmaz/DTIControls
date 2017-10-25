Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web
Imports System.ComponentModel
Imports System.Security.Permissions

#If DEBUG Then
Public Class ScriptBlock
    Inherits Panel
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("ScriptText"), _
    ToolboxData("<{0}:ScriptBlock runat=""server""> </{0}:ScriptBlock>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ScriptBlock
        Inherits Panel
#End If
        Private scriptlit As New LiteralControl

        ''' <summary>
        ''' Property to get/set the text of the script
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' Text string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the text of the script")> _
        Public Property ScriptText() As String
            Get
                EnsureChildControls()
                Return scriptlit.Text
            End Get
            Set(ByVal value As String)
                EnsureChildControls()
                scriptlit.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the language
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' language string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the language")> _
        Public Property Language() As String
            Get
                Return Me.Attributes("language")
            End Get
            Set(ByVal value As String)
                Me.Attributes("language") = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the attribute type
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' type string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the attribute type")> _
        Public Property TypeAttr() As String
            Get
                Return Me.Attributes("type")
            End Get
            Set(ByVal value As String)
                Me.Attributes("type") = value
            End Set
        End Property

        Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
            Get
                Return HtmlTextWriterTag.Script
            End Get
        End Property

        Sub New()
            Me.Attributes.Add("language", "javascript")
            Me.Attributes.Add("type", "text/javascript")
            Me.Controls.Add(scriptlit)
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            ScriptText = "/* <![CDATA[ */" & vbCrLf & ScriptText & vbCrLf & "/* ]]> */"
            MyBase.Render(writer)
        End Sub
    End Class