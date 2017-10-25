Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses
Imports System.Text.RegularExpressions

''' <summary>
''' Automatically adds jquery dependancy to a page. Accessed via static calls.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Automatically adds jquery dependancy to a page. Accessed via static calls."),ComponentModel.ToolboxItem(False)> _
Public Class jQueryInclude
    Inherits WebControl

    Private erroradding As Boolean = False
	'Private Shared lastJq As String = "1.6.2"
	'Private Shared lastJqUI As String = "1.8.13"
	Private Shared lastJq As String = "3.2.1"
	Private Shared lastJqUI As String = "1.12.1"

	''' <summary>
	''' determines if the include has been added already.
	''' </summary>
	''' <remarks></remarks>
    <System.ComponentModel.Description("determines if the include has been added already.")> _
    Public isinitial As Boolean = False
    Protected Overrides ReadOnly Property TagKey() _
        As HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Script
        End Get
    End Property

    Private _ctrlList As New Generic.List(Of Control)

    ''' <summary>
    ''' Lists all controls in the page head
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Lists all controls in the page head")> _
    Public Property ctrlList() As Generic.List(Of Control)
        Get
            Return _ctrlList
        End Get
        Set(ByVal value As Generic.List(Of Control))
            _ctrlList = value
        End Set
    End Property

    Friend Shared ReadOnly Property inDesigner() As Boolean
        Get
            Dim process As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
            Dim ret As Boolean = process.ProcessName.ToLower().Trim() = "devenv"
            process.Dispose()
            Return ret
        End Get
    End Property

    Public Enum scriptType
        css
        javascript
    End Enum

    ''' <summary>
    ''' Adds the jQuery script block to the page.
    ''' </summary>
    ''' <param name="page">
    ''' The page that the script block will be added to.
    ''' </param>
    ''' <param name="script">
    ''' The script to be added
    ''' </param>
    ''' <param name="minify"></param>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQuery script block to the page.")> _
    Public Shared Sub addScriptBlock(ByVal page As Page, ByVal script As String, Optional ByVal minify As Boolean = True, Optional ByVal type As String = Nothing, Optional ByVal id As String = Nothing)
        Dim jQueryIncludeHeader As jQueryInclude = getInitialInclude(page)
        Dim sf As New ScriptFile("", type)
        If minify Then
            Dim Str As New System.IO.MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(script))
            sf.src = BaseClasses.JsMinimizer.SMinify(Str)
        Else
            sf.src = script
        End If
        sf.id = id
        sf.isRawScript = True
        jQueryIncludeHeader.addInclude(sf)
    End Sub

    ''' <summary>
    ''' Adds a style block to the page.
    ''' </summary>
    ''' <param name="page">The page that the script block will be added to.</param>
    ''' <param name="style">The Style to be Added</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a style block to the page.")> _
    Public Shared Sub addStyleBlock(ByVal page As Page, ByVal style As String, Optional ByVal id As String = Nothing)
        jQueryLibrary.jQueryInclude.addScriptBlock(page, style, False, "text/css", id)
    End Sub

    ''' <summary>
    ''' Adds the jQuery script block that executes after page load.
    ''' </summary>
    ''' <param name="page">
    ''' The page that the script block will be added to.
    ''' </param>
    ''' <param name="script">
    ''' The script to be added
    ''' </param>
    ''' <param name="minify"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQuery script block that executes after page load.")> _
    Public Shared Sub addScriptBlockPageLoad(ByVal page As Page, ByVal script As String, Optional ByVal minify As Boolean = False, Optional ByVal id As String = "")
        Dim jQueryIncludeHeader As jQueryInclude = getInitialInclude(page)
        Dim sf As New ScriptFile("")
        script = "$(function(){" & script & "});"
        If minify Then
            Dim Str As New System.IO.MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(script))
            sf.src = BaseClasses.JsMinimizer.SMinify(Str)
        Else
            sf.src = script
        End If
        sf.id = id
        sf.isRawScript = True
        jQueryIncludeHeader.addInclude(sf)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="scriptLocation"></param>
    ''' <param name="type"></param>
    ''' <param name="disableMinimize"></param>
    ''' <param name="useFullPath"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Shared Sub addScriptFile(ByVal page As Page, ByVal scriptLocation As String, Optional ByVal type As String = Nothing, Optional ByVal disableMinimize As Boolean = False, Optional ByVal useFullPath As Boolean = False, Optional ByVal id As String = "")
        Dim jQueryIncludeHeader As jQueryInclude = getInitialInclude(page)
        jQueryIncludeHeader.addInclude(scriptLocation, type, useFullPath, disableMinimize, id)
    End Sub

    ''' <summary>
    ''' removes a script file from the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="scriptLocation"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("removes a script file from the page.")> _
    Public Shared Sub deleteScriptFile(ByVal page As Page, ByVal scriptLocation As String)
        Dim jQueryIncludeHeader As jQueryInclude = getInitialInclude(page)
        jQueryIncludeHeader.deleteInclude(scriptLocation)
    End Sub

    ''' <summary>
    ''' Gets the jQuery include on a given page
    ''' </summary>
    ''' <param name="page">
    ''' The Page to get the include from
    ''' </param>
    ''' <returns>
    ''' Returns the jQuery include as a jQueryInclude type
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the jQuery include on a given page")> _
    Public Shared Function getInitialInclude(ByVal page As Page) As jQueryInclude
        If page.Items.Item("jqueryInclude") Is Nothing Then
            RegisterJQuery(page)
        End If
        Return page.Items.Item("jqueryInclude")
    End Function

    ''' <summary>
    '''  Adds the jQueryUI include header to a given page
    ''' </summary>
    ''' <param name="page">
    ''' The page to add the header to
    ''' </param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQueryUI include header to a given page")> _
    Public Shared Sub RegisterJQueryUI(ByRef page As Page, Optional ByVal jQueryVersion As String = Nothing)
        If jQueryVersion Is Nothing Then
            addScriptFile(page, "/jQueryLibrary/jquery-ui." & lastJqUI & ".min.js")
        Else
            Select Case jQueryVersion
                Case "1.3.2"
                    addScriptFile(page, "/jQueryLibrary/jquery-ui.1.7.3.min.js")
                Case "1.4.1", "1.4.2", "1.8.1"
                    addScriptFile(page, "/jQueryLibrary/jquery-ui.1.8.1.min.js")
                Case Else
                    addScriptFile(page, "/jQueryLibrary/jquery-ui." & lastJqUI & ".min.js")
            End Select
        End If
        addScriptFile(page, "/jQueryLibrary/DTIjqUIFunctions.js")
        ThemeAdder.AddTheme(page)
    End Sub

    ''' <summary>
    '''  Adds the jQueryUI include header to a given page
    ''' </summary>
    ''' <param name="page">
    ''' The page to add the header to
    ''' </param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQueryUI include header to a given page")> _
    Public Shared Sub RegisterJQueryUIThemed(ByRef page As Page, ByVal theme As ThemeAdder.themes, Optional ByVal jQueryVersion As String = Nothing, Optional ByVal ThemeButtons As Boolean = False)
        RegisterJQueryUI(page, jQueryVersion)
        ThemeAdder.AddTheme(page, theme, ThemeButtons)
    End Sub

    ''' <summary>
    '''  Adds the jQueryUI include header to a given page
    ''' </summary>
    ''' <param name="page">
    ''' The page to add the header to
    ''' </param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQueryUI include header to a given page")> _
    Public Shared Sub RegisterJQueryUIThemed(ByRef page As Page)
        RegisterJQueryUI(page)
        ThemeAdder.AddTheme(page, Nothing)
    End Sub

    ''' <summary>
    '''  Adds the jQuery include header to a given page
    ''' </summary>
    ''' <param name="page">
    ''' The page to add the header to
    ''' </param>
    ''' <param name="version"></param>
    ''' <param name="asWebResource"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the jQuery include header to a given page")> _
    Public Shared Sub RegisterJQuery(ByRef page As Page, Optional ByVal version As String = Nothing, Optional ByVal asWebResource As Boolean = False)
        Dim jQueryIncludeHeader As jQueryInclude = page.Items.Item("jqueryInclude")
        If jQueryIncludeHeader Is Nothing Then
            jQueryIncludeHeader = New jQueryInclude
            Dim jsFile As String = "jquery-" & lastJq & ".min.js"
            If version IsNot Nothing Then
                jsFile = "jquery-" & version & ".min.js"
            End If
            If inDesigner Then asWebResource = True
            If Not asWebResource Then
                jQueryIncludeHeader.addInclude("/jQueryLibrary/" & jsFile)
                'jQueryIncludeHeader.addInclude("/jQueryLibrary/jquery-ui.custom.min.js")
                jQueryIncludeHeader.addInclude("/jQueryLibrary/DTIprototypes.js")
            Else
                jQueryIncludeHeader.addInclude(page.ClientScript.GetWebResourceUrl(GetType(jQueryInclude), "jQueryLibrary." & jsFile))
                'jQueryIncludeHeader.addInclude(page.ClientScript.GetWebResourceUrl(GetType(jQueryInclude), "jQueryLibrary.jquery-ui.custom.min.js"))
                jQueryIncludeHeader.addInclude(page.ClientScript.GetWebResourceUrl(GetType(jQueryInclude), "jQueryLibrary.DTIprototypes.js"))
            End If
            jQueryIncludeHeader.isinitial = True
            page.Items.Item("jqueryInclude") = jQueryIncludeHeader
            Try
                page.Header.Controls.AddAt(0, jQueryIncludeHeader)
            Catch ex As Exception
                'Try
                addJQueryToHead(page, jQueryIncludeHeader)

                'Catch ex2 As Exception

                'End Try
            End Try
        ElseIf version IsNot Nothing Then
            Dim jsFile As String = BaseClasses.Scripts.ScriptsURL(True) & "jQueryLibrary/jquery-" & version & ".min.js"
            For Each include As ScriptFile In jQueryIncludeHeader.jqueryIncludeList.Values
                If Regex.IsMatch(include.src, "jquery-\d+\.\d+\.\d+") Then
                    include.src = jsFile
                End If
            Next
        End If
    End Sub

    Private Sub jQueryInclude_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not isinitial Then
            addScriptFile(Me.Page, src, jqtype)
        End If
    End Sub

    Private Sub jQueryInclude_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If isinitial Then
            BaseVirtualPathProvider.registerVirtualPathProvider()
        End If
    End Sub

    Public Function RenderControlToString(ByVal ctrl As Control) As String
        Dim sb As StringBuilder = New StringBuilder()
        Dim tw As IO.StringWriter = New IO.StringWriter(sb)
        Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)

        ctrl.RenderControl(hw)
        Return sb.ToString()
    End Function


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If isinitial Then
               Dim ControlsString As String = ""
                For Each ctrl As Control In ctrlList
                    ControlsString = RenderControlToString(ctrl)
                Next
            writer.Write("<meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"">")
            For Each sf As ScriptFile In jqueryIncludeList.Values
                Dim idstring As String = ""
                If sf.id <> sf.src Then
                    idstring = " id=""" & sf.id & """ "
                End If
                If sf.isRawScript Then
                    If sf.type.EndsWith("css") Then
                        writer.Write("<style" & idstring & " type=""" & sf.type & """>" & sf.src & "</style>")
                    ElseIf sf.type.EndsWith("javascript") Then
                        writer.Write("<script" & idstring & " type=""" & sf.type & """ language=""javascript"">//<![CDATA[" & vbCrLf & sf.src & vbCrLf & "//]]></script>")
                    Else
                        writer.Write("<script" & idstring & " type=""" & sf.type & """>" & sf.src & "</script>")
                    End If

                Else
                    If sf.type.EndsWith("css") Then
                        writer.Write("<link" & idstring & " rel=""stylesheet"" type=""" & sf.type & """ href=""" & sf.src & """ />")
                    ElseIf sf.type.EndsWith("javascript") Then
                        writer.Write("<script" & idstring & " type=""" & sf.type & """ src=""" & sf.src & """ language=""javascript""></script>")
                    Else
                        writer.Write("<script" & idstring & " type=""" & sf.type & """ src=""" & sf.src & """></script>")
                    End If
                End If
 
                writer.Write(ControlsString)
                writer.Write(vbCrLf)

            Next

        End If
    End Sub

    Private jqtype As String = Nothing
    Private src As String = Nothing

    ''' <summary>
    ''' jQueryInclude constructor, sets jqtype and src
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="src"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("jQueryInclude constructor, sets jqtype and src")> _
    Public Sub New(Optional ByVal type As String = Nothing, Optional ByVal src As String = Nothing)
        Me.jqtype = type
        Me.src = src
    End Sub

    Private _jqueryIncludeList As New Generic.Dictionary(Of String, ScriptFile)

    ''' <summary>
    ''' jQuery Include List read only property
    ''' </summary>
    ''' <value></value>
    ''' <returns>
    ''' jQuery Include List
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("jQuery Include List read only property")> _
    Public ReadOnly Property jqueryIncludeList() As Generic.Dictionary(Of String, ScriptFile)
        Get
            Return _jqueryIncludeList
        End Get
    End Property

    ''' <summary>
    ''' Adds the JQueryIncludeHeader to the head tag
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds the JQueryIncludeHeader to the head tag")> _
    Private Shared Sub addJQueryToHead(ByVal page As Page, ByVal header As jQueryInclude)
        Dim headLitCon As LiteralControl = Nothing
        Dim litctrls As Generic.List(Of Control) = BaseClasses.Spider.spiderPageforAllOfType(page, GetType(LiteralControl))
        Dim head As New Regex("\<\s*head.*\>")
        Dim headstring As String = ""
        For Each lit As LiteralControl In litctrls
            If head.Matches(CType(lit, LiteralControl).Text).Count > 0 Then
                headstring = head.Matches(CType(lit, LiteralControl).Text)(0).Value
                headLitCon = lit
                Exit For
            End If
        Next
        Try

            If headLitCon IsNot Nothing Then
                Dim splitResults() As String

                splitResults = head.Split(CType(headLitCon, LiteralControl).Text)
                Dim startLitCon As New LiteralControl(splitResults(0) & headstring)

                'page.Controls.RemoveAt(0)
                Dim index As Integer = headLitCon.Parent.Controls.IndexOf(headLitCon)
                headLitCon.Parent.Controls.AddAt(index, header)
                headLitCon.Parent.Controls.AddAt(index, startLitCon)
                headLitCon.Text = splitResults(1)
            End If
        Catch ex As Exception
            header.erroradding = True
            header.mypage = page
            If Not inDesigner Then
                Throw New Exception("Please add runat=""server"" to the <head> tag of your page." & vbCrLf & "The head tag should look like: " & vbCrLf & "<head runat=""server"">")
            End If
        End Try


    End Sub

    ''' <summary>
    ''' Adds a script file to the jQuery include list
    ''' </summary>
    ''' <param name="sf">
    ''' Script file to be included
    ''' </param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a script file to the jQuery include list")> _
    Public Sub addInclude(ByVal sf As ScriptFile)
        jqueryIncludeList(sf.id) = sf
    End Sub

    ''' <summary>
    ''' Creates an instance of ScriptFile in order to add it to the jQuery include list
    ''' </summary>
    ''' <param name="filename">
    ''' Name of file to be added to ScriptFile instance
    ''' </param>
    ''' <param name="type">Type of script</param>
    ''' <param name="suppressRes"></param>
    ''' <param name="disableMinimize"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates an instance of ScriptFile in order to add it to the jQuery include list")> _
    Public Sub addInclude(ByVal filename As String, Optional ByVal type As String = Nothing, Optional ByVal suppressRes As Boolean = False, Optional ByVal disableMinimize As Boolean = True, Optional ByVal id As String = "")
        Dim sf As New ScriptFile(filename, type, suppressRes, disableMinimize, id)
        jqueryIncludeList(sf.id) = sf
    End Sub

    ''' <summary>
    ''' Removes filename
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Removes filename")> _
    Public Sub deleteInclude(ByVal filename As String)
        Dim sf As New ScriptFile(filename)
        jqueryIncludeList.Remove(sf.id)
        jqueryIncludeList.Remove(filename)
    End Sub

    ''' <summary>
    ''' Removes an id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Removes an id")> _
    Public Sub deleteIncludeByID(ByVal id As String)
        jqueryIncludeList.Remove(id)
    End Sub

    ''' <summary>
    ''' Replaces an id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="newcontents"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Replaces an id")> _
    Public Sub replaceIncludeByID(ByVal id As String, ByVal newcontents As String)
        If jqueryIncludeList.ContainsKey(id) Then
            jqueryIncludeList(id).src = newcontents
        End If
    End Sub

    Public Class ScriptFile

        Public type As String = "text/javascript"
        Public src As String
        Public isRawScript As Boolean = False
        Public Property id() As String
            Get
                If _id Is Nothing OrElse _id = "" Then
                    Return src
                End If
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Private _id As String = ""

        ''' <summary>
        ''' Constructor for ScriptFile class
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="type">
        ''' Type of script
        ''' </param>
        ''' <param name="suppressRes"></param>
        ''' <param name="disableMinimize"></param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Constructor for ScriptFile class")> _
        Public Sub New(ByVal src As String, Optional ByVal type As String = Nothing, Optional ByVal suppressRes As Boolean = False, Optional ByVal disableMinimize As Boolean = False, Optional ByVal id As String = Nothing)
            If Not type Is Nothing Then
                If type.ToLower = "css" Then type = "text/css"
                Me.type = type
            Else
                If src.ToLower.EndsWith(".js") Then
                    Me.type = "text/javascript"
                ElseIf src.ToLower.EndsWith(".css") Then
                    Me.type = "text/css"
                End If
            End If
            Me.id = id
            If src.ToLower.Contains("http://") OrElse src.ToLower.Contains("https://") Then suppressRes = True
            If suppressRes Then
                Me.src = src
            Else
                Me.src = BaseClasses.Scripts.ScriptsURL(disableMinimize) & src
            End If
        End Sub

    End Class

    Friend WithEvents mypage As Page
    Private Sub mypage_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles mypage.PreRender
        If erroradding Then
            addJQueryToHead(mypage, Me)
        End If

    End Sub

    ''' <summary>
    ''' Adds a control to the page header.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="ctrl"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a control to the page header.")> _
    Public Shared Sub addControlToHeader(ByVal page As Page, ByVal ctrl As Control)
        Dim jQueryIncludeHeader As jQueryInclude = getInitialInclude(page)
        jQueryIncludeHeader.ctrlList.Add(ctrl)
    End Sub

    ''' <summary>
    ''' helper method to get the name of an eumeration object.
    ''' </summary>
    ''' <param name="enumeration"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("helper method to get the name of an eumeration object.")> _
    Public Shared Function getEnumName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function
End Class