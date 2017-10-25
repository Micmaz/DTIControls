Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses

Namespace Plugins
#If DEBUG Then
    Public Class Center
        Inherits WebControl
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class Center
            Inherits WebControl
#End If
            Private _tagKey As HtmlTextWriterTag = HtmlTextWriterTag.Script

            Protected Overrides ReadOnly Property TagKey() _
                As HtmlTextWriterTag
                Get
                    Return _tagKey
                End Get
            End Property

            Public Shared Sub Register(ByRef page As Page, Optional ByVal addAtPosition As Integer = 4, Optional ByVal asWebResource As Boolean = False)
                jQueryInclude.RegisterJQuery(page, asWebResource)
                Dim jQueryIncludeHeader As Center = BaseClasses.Spider.spiderPageforType(page, GetType(Center))
                If jQueryIncludeHeader Is Nothing Then
                    If Not asWebResource Then
                        page.Header.Controls.AddAt(addAtPosition, New Center("text/javascript", "jquery.center.js"))
                    Else
                        Dim scriptLocation1 As String = page.ClientScript.GetWebResourceUrl(GetType(Center), "jQueryLibrary.jquery.center.js")
                        page.Header.Controls.AddAt(addAtPosition, New Center("text/javascript", scriptLocation1, True))
                    End If
                End If
            End Sub

            Private Sub jQueryInclude_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                BaseVirtualPathProvider.registerVirtualPathProvider()
            End Sub

            ''' <summary>
            ''' Constructor for the Center class
            ''' </summary>
            ''' <param name="type">
            ''' Type of script
            ''' </param>
            ''' <param name="filename">
            ''' Filename of the script
            ''' </param>
            ''' <param name="noscripts">
            ''' 
            ''' </param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Constructor for the Center class")> _
            Public Sub New(ByVal type As String, ByVal filename As String, Optional ByVal noscripts As Boolean = False)
                Dim src As String = "src"
                If type = "text/css" Then
                    _tagKey = HtmlTextWriterTag.Link
                    src = "href"
                    Me.Attributes.Add("rel", "stylesheet")
                End If
                Me.Attributes.Add("type", type)
                If noscripts Then
                    Me.Attributes.Add(src, filename)
                Else
                    Me.Attributes.Add(src, BaseClasses.Scripts.ScriptsURL(True) & "jQueryLibrary/" & filename)
                End If
            End Sub
        End Class
End Namespace