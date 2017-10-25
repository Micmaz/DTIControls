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
    Public Class TimePicker
        Inherits WebControl
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class TimePicker
            Inherits WebControl
#End If
            Private _tagKey As HtmlTextWriterTag = HtmlTextWriterTag.Script

            Protected Overrides ReadOnly Property TagKey() _
                As HtmlTextWriterTag
                Get
                    Return _tagKey
                End Get
            End Property

            ''' <summary>
            ''' Registers a script on a given page
            ''' </summary>
            ''' <param name="page">
            ''' The page to register the script to
            ''' </param>
            ''' <param name="addAtPosition">
            ''' Position the script will be added at
            ''' </param>
            ''' <param name="asWebResource">
            ''' Boolean to identify as a webresource
            ''' </param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Registers a script on a given page")> _
            Public Shared Sub Register(ByRef page As Page, Optional ByVal addAtPosition As Integer = 4, Optional ByVal asWebResource As Boolean = False)
                jQueryInclude.RegisterJQuery(page, asWebResource)
                Dim jQueryIncludeHeader As TimePicker = BaseClasses.Spider.spiderPageforType(page, GetType(TimePicker))
                If jQueryIncludeHeader Is Nothing Then
                    If Not asWebResource Then
                        page.Header.Controls.AddAt(addAtPosition, New TimePicker("text/javascript", "jquery.timePicker.js"))
                        page.Header.Controls.AddAt(addAtPosition, New TimePicker("text/css", "timePicker.css"))
                    Else
                        Dim scriptLocation1 As String = page.ClientScript.GetWebResourceUrl(GetType(TimePicker), "jQueryLibrary.jquery.timePicker.js")
                        page.Header.Controls.AddAt(addAtPosition, New TimePicker("text/javascript", scriptLocation1, True))
                        Dim scriptLocation2 As String = page.ClientScript.GetWebResourceUrl(GetType(TimePicker), "jQueryLibrary.timePicker.css")
                        page.Header.Controls.AddAt(addAtPosition, New TimePicker("text/css", scriptLocation2, True))
                    End If
                End If
            End Sub

            Private Sub jQueryInclude_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                BaseVirtualPathProvider.registerVirtualPathProvider()
            End Sub

            ''' <summary>
            ''' Constructor for the TimePicker class
            ''' </summary>
            ''' <param name="type">
            ''' Type of script
            ''' </param>
            ''' <param name="filename">
            ''' Filename for script
            ''' </param>
            ''' <param name="noscripts"></param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Constructor for the TimePicker class")> _
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