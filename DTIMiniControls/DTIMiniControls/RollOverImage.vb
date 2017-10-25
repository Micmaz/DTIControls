Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class RollOverImage
    Inherits System.Web.UI.WebControls.Image
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("ImageRollOverUrl"), _
    ToolboxData("<{0}:RollOverImage runat=""server""> </{0}:RollOverImage>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class RollOverImage
        Inherits System.Web.UI.WebControls.Image
#End If
        Private _imageRollOverUrl As String = ""

        ''' <summary>
        ''' Property to get/set the Image Roll Over URL
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: ""
        ''' </value>
        ''' <returns>
        ''' imageRollOverUrl string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Image Roll Over URL"),Category("Appearance"), Bindable(True)> _
        Public Property ImageRollOverUrl() As String
            Get
                Return _imageRollOverUrl
            End Get
            Set(ByVal value As String)
                _imageRollOverUrl = value
            End Set
        End Property

        Private Sub RollOverImage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RollOverImageInclude.RegisterJs(Me.Page)
        End Sub

        Private Sub RollOverImage_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Me.Attributes.Add("srcover", ResolveClientUrl(ImageRollOverUrl))
        End Sub

        Private Class RollOverImageInclude
            Inherits WebControl

            Protected Overrides ReadOnly Property TagKey() _
                As HtmlTextWriterTag
                Get
                    Return HtmlTextWriterTag.Script
                End Get
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <param name="page"></param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("")> _
            Public Shared Sub RegisterJs(ByRef page As Page)
                Dim jQueryIncludeHeader As RollOverImageInclude = BaseClasses.Spider.spiderPageforType(page, GetType(RollOverImageInclude))
                If jQueryIncludeHeader Is Nothing Then
                    BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
                    page.Header.Controls.Add(New RollOverImageInclude("text/javascript", "oodomimagerollover.js"))
                End If
            End Sub

            ''' <summary>
            ''' Constructor for the RollOverImage class
            ''' </summary>
            ''' <param name="type">
            ''' Type of script
            ''' </param>
            ''' <param name="filename">
            ''' Filename of script
            ''' </param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Constructor for the RollOverImage class")> _
            Public Sub New(ByVal type As String, ByVal filename As String)
                Me.Attributes.Add("type", type)
                Me.Attributes.Add("src", BaseClasses.Scripts.ScriptsURL(True) & "DTIMiniControls/" & filename)
            End Sub

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                writer.Write("<script type=""" & Me.Attributes("type") & """ src=""" & Me.Attributes("src") & """></script>")
            End Sub
        End Class
    End Class
