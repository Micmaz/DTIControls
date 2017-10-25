Public Class HighlighedEditor
    Inherits TextBox

#Region "Properties"

    Public Property language() As languageEnum = languageEnum.javascript

    Public Property theme() As themeEnum = themeEnum.default

    Public Property autocomplete() As Boolean = True

    Public Property autoGrow() As Boolean = True

    Public Property lineNumbers() As Boolean = True

    Public Property autoCloseTags() As Boolean = True

    Public Property matchBrackets() As Boolean = True

    Public Property allThemeCssAvailable() As Boolean= False

    Public Property indentSpaces() As Integer = 4

    Public Property additionalAutocomp() As List(Of String) = New List(Of String)

    Public Property customMode As String = Nothing

#End Region

#Region "Enums"

    Public Enum languageEnum
        c
        cpp
        java
        Csharp
        groovy
        text
        css
        diff
        haskell
        html
        javascript
        lua
        php
        plsql
        python
        rst
        scheme
        smalltalk
        sparql
        stex
        xml
        yaml
        custom
    End Enum

    Public Enum themeEnum
        [default]
        elegant
        neat
        night
    End Enum

#End Region

    Private Function getName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function

    Private Sub HighlighedEditor_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/codemirror.js")
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/codemirror.css")
    End Sub

	Shared Sub registerClientScript(page As Page)
		jQueryLibrary.jQueryInclude.addScriptFile(page, "DTIMiniControls/codemirror.js")
		jQueryLibrary.jQueryInclude.addScriptFile(page, "DTIMiniControls/codemirror.css")
		jQueryLibrary.jQueryInclude.addScriptFile(page, "DTIMiniControls/jquery.urlencode.js", , True)
	End Sub

	Protected Overrides Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean
        MyBase.LoadPostData(postDataKey, postCollection)
        Me.Text = TextBoxEncoded.decodeFromURL(Me.Page.Request.Params("hidden_" & Me.ClientID))
        Return True
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        writer.Write("<input type=""hidden"" id=""hidden_" & Me.ClientID & """ name=""hidden_" & Me.ClientID & """ value=""""/>")
    End Sub

    Private Sub HighlighedEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.urlencode.js", , True)
    End Sub

    Private Sub HighlighedEditor_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim mode As String = ""
        If language = languageEnum.custom Then
            mode = customMode
        Else
            mode = "'" & getdocType(Me.language) & "'"
        End If


        Dim script As String = "editor_" & Me.ClientID & " = CodeMirror.fromTextArea(document.getElementById('" & Me.ClientID & "'), { " & vbCrLf & _
        "        lineNumbers: " & IIf(lineNumbers, "true", "false") & ", " & vbCrLf & _
        "        matchBrackets: " & IIf(matchBrackets, "true", "false") & ", " & vbCrLf & _
        "        autoCloseTags: " & IIf(autoCloseTags, "true", "false") & ", " & vbCrLf & _
        "        indentUnit: " & indentSpaces & ", " & vbCrLf & _
        "        theme: '" & getName(theme) & "', " & vbCrLf & _
        "        mode: " & mode & "," & vbCrLf & _
        "        lineWrapping: " & IIf(Wrap, "true", "false") & "," & vbCrLf & _
        "        onKeyEvent: function(i, e) {" & IIf(Me.autocomplete, "autoComplete_" & Me.ClientID & ".Complete(i,e);", "") & "} " & vbCrLf & _
        "      }); " & vbCrLf
		script &= "if(window.jQuery){$('#" & Me.ClientID & "').data('editor', editor_" & Me.ClientID & ");}"
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/" & getScriptName(Me.language))
        'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/" & getName(Me.language) & ".css")
        If allThemeCssAvailable Then
            For Each name As String In [Enum].GetNames(theme.GetType())
                jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/" & name & ".css")
            Next
        Else
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/" & getName(Me.theme) & ".css")
        End If

        If autocomplete Then
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/complete.js")
            script &= "var autoComplete_" & Me.ClientID & " = new autoComplete;" & vbCrLf & _
            "autoComplete_" & Me.ClientID & ".setEditor(editor_" & Me.ClientID & ");" & vbCrLf
            If language = languageEnum.html Or language = languageEnum.plsql Or language = languageEnum.css Or language = languageEnum.xml Then
                script &= "autoComplete_" & Me.ClientID & ".checkcase(false);" & vbCrLf
            End If
            If Not additionalAutocomp Is Nothing AndAlso additionalAutocomp.Count > 0 Then
                script &= "autoComplete_" & Me.ClientID & ".addAutoComps(["
                For Each itm As String In additionalAutocomp
                    script &= "'" & itm & "',"
                Next
                script = script.Trim(",")
                script &= "]);" & vbCrLf
            End If

        End If
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "var editor_" & Me.ClientID & ";", False)
        'Me.Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, Me.ClientID, _
        '"editor_" & Me.ClientID & ".setValue($.urlEncode(editor_" & Me.ClientID & ".getValue()));")
        Me.Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, Me.ClientID, _
        "$('#hidden_" & Me.ClientID & "').val($.urlEncode(editor_" & Me.ClientID & ".getValue()));editor_" & Me.ClientID & ".setValue('')")

        jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, script, False)

        If autoGrow Then
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/autoGrow.css")
        End If

    End Sub

    Private Function getScriptName(ByVal l As languageEnum) As String
        If l = languageEnum.cpp Or l = languageEnum.java Or l = languageEnum.Csharp Or l = languageEnum.c Or l = languageEnum.groovy Then
            Return "clike.js"
        ElseIf l = languageEnum.text Then
            Return "complete.js"
            'ElseIf l = languageEnum.css Then
            '    Return "scheme.js"
        Else
            Return getName(l) & ".js"
        End If
    End Function



    Private Function getdocType(ByVal l As languageEnum) As String

        If l = languageEnum.css Or l = languageEnum.html Or l = languageEnum.javascript Then
            Return "text/" & getName(l)
        ElseIf l = languageEnum.css Then
            Return "text/x-scheme"
        ElseIf l = languageEnum.cpp Then
            Return "text/x-c++src"
        ElseIf l = languageEnum.c Then
            Return "text/x-csrc"
        ElseIf l = languageEnum.smalltalk Then
            Return "text/x-stsrc"
        ElseIf l = languageEnum.xml Then
            Return "text/html"
        End If
        Return "text/x-" & getName(l)
    End Function


    Public Sub New()
        Me.TextMode = TextBoxMode.MultiLine
    End Sub

End Class
