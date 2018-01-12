Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses

''' <summary>
''' Adds a jquery theme to the page.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Adds a jquery theme to the page.")> _
Public Class ThemeAdder
    Inherits WebControl

    Public Enum themes
        black_tie = 1
        blitzer = 2
        cupertino = 3
        dark_hive = 4
        dot_luv = 5
        eggplant = 6
        excite_bike = 7
        flick = 8
        hot_sneaks = 9
        humanity = 10
        le_frog = 11
        minc_choc = 12
        overcast = 13
        pepper_grinder = 14
        redmond = 15
        smoothness = 16
        south_street = 17
        start = 18
        sunny = 19
        swanky_purse = 20
        trontastic = 21
        ui_darkness = 22
        ui_lightness = 23
        vader = 24
        aristo = 25
        bootstrap = 26
        united = 27
        absolution = 28
        delta = 29
        selene = 30
        metro = 31
        custom = 255
    End Enum

    Protected Overrides ReadOnly Property TagKey() _
        As HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Link
        End Get
    End Property

    Private Shared bodyCssFix As String = ".ui-widget{ font-size:0.8em; } input.ui-button{ padding: 0.2em 1em; }"

#Region "Theme Adding Function"

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="themepath"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddCustomTheme(ByRef page As Page, ByVal themepath As String, Optional ByVal ThemeButtons As Boolean = True, Optional ByVal ThemeBody As Boolean = True, Optional ByVal ThemeTextBoxes As Boolean = True, Optional ByVal ThemeCheckandRadioButtons As Boolean = True)
        AddTheme(page, themes.custom, ThemeButtons, ThemeBody, ThemeTextBoxes, ThemeCheckandRadioButtons, themepath)
    End Sub

    ''' <summary>
    ''' Gets the themeAdder object for the specified page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the themeAdder object for the specified page.")> _
    Public Shared Function getThemeAdder(ByVal page As Page) As ThemeAdder
        If page.Items("themeAdder") Is Nothing Then
            page.Items("themeAdder") = New ThemeAdder
            page.Header.Controls.Add(page.Items("themeAdder"))
            jQueryInclude.RegisterJQuery(page)
        End If
        Return page.Items("themeAdder")
    End Function

#Region "Add Theme"

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page)
        Dim ta As ThemeAdder = getThemeAdder(page)
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean, ByVal ThemeBody As Boolean)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
        ta.themeBody = ThemeBody
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean, ByVal ThemeBody As Boolean, ByVal ThemeTextBoxes As Boolean)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
        ta.themeBody = ThemeBody
        ta.themeTextBoxes = ThemeTextBoxes
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean, ByVal ThemeBody As Boolean, ByVal ThemeTextBoxes As Boolean, ByVal ThemeCheckandRadioButtons As Boolean)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
        ta.themeBody = ThemeBody
        ta.themeTextBoxes = ThemeTextBoxes
        ta.themeCheckboxes = ThemeCheckandRadioButtons
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean, ByVal ThemeBody As Boolean, ByVal ThemeTextBoxes As Boolean, ByVal ThemeCheckandRadioButtons As Boolean, ByVal themepath As String)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
        ta.themeBody = ThemeBody
        ta.themeTextBoxes = ThemeTextBoxes
        ta.themeCheckboxes = ThemeCheckandRadioButtons
        ta.customThemePath = themepath
    End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="theme"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddTheme(ByRef page As Page, ByVal theme As themes, ByVal ThemeButtons As Boolean, ByVal ThemeBody As Boolean, ByVal ThemeTextBoxes As Boolean, ByVal ThemeCheckandRadioButtons As Boolean, ByVal themepath As String, ByVal fadeEffect As Boolean)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.theme = theme
        ta.themeButtons = ThemeButtons
        ta.themeBody = ThemeBody
        ta.themeTextBoxes = ThemeTextBoxes
        ta.themeCheckboxes = ThemeCheckandRadioButtons
        ta.customThemePath = themepath
        ta.fadeEffect = fadeEffect
    End Sub

#End Region


    'Friend Shared Sub AddThemeFiles(ByRef page As Page, ByVal theme As themes, Optional ByVal ThemeButtons As Boolean = True, _
    '                                Optional ByVal ThemeBody As Boolean = True, Optional ByVal ThemeTextBoxes As Boolean = True, _
    '                                Optional ByVal ThemeCheckandRadioButtons As Boolean = True, Optional ByVal themepath As String = "", _
    '                                Optional ByVal fadeEffect As Boolean = True)
    '    Dim ta As ThemeAdder = getThemeAdder(page)
    '    ta.themeButtons = ThemeButtons
    '    ta.themeBody = ThemeBody
    '    ta.themeTextBoxes = ThemeTextBoxes
    '    ta.themeCheckboxes = ThemeCheckandRadioButtons
    '    ta.customThemePath = themepath
    '    ta.fadeEffect = fadeEffect
    'End Sub

    ''' <summary>
    ''' Adds a theme to the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a theme to the page.")> _
    Public Shared Sub AddThemeToIframe(ByRef page As Page, Optional ByVal themeBody As Boolean = True)
        Dim ta As ThemeAdder = getThemeAdder(page)
        ta.themeBody = themeBody
        ta.isIframe = True
    End Sub

#End Region

#Region "ThemeAdder Component"

    'Private Sub jQueryInclude_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    '    BaseVirtualPathProvider.registerVirtualPathProvider()
    '    If _customeThemePath.Length > 0 Then
    '        AddTheme(Me.Page, theme, themeButtons, themeBody, themeTextBoxes, themeCheckboxes, customThemePath)
    '    Else
    '        AddTheme(Me.Page, theme, themeButtons, themeBody, themeTextBoxes, themeCheckboxes)
    '    End If
    'End Sub

    Private WithEvents pg As Page
    Public Overrides Property Page As System.Web.UI.Page
        Get
            Return MyBase.Page
        End Get
        Set(value As System.Web.UI.Page)
            MyBase.Page = value
            pg = value
        End Set
    End Property

    Private _customeThemePath As String = ""
    Public Property customThemePath() As String
        Get
            Return _customeThemePath
        End Get
        Set(ByVal value As String)
            _customeThemePath = value
        End Set
    End Property

	Private _themeButtons As Boolean = True
	Public Property themeButtons() As Boolean
        Get
            Return _themeButtons
        End Get
        Set(ByVal value As Boolean)
            _themeButtons = value
        End Set
    End Property

	Private _themeBody As Boolean = False
	Public Property themeBody() As Boolean
        Get
            Return _themeBody
        End Get
        Set(ByVal value As Boolean)
            _themeBody = value
        End Set
    End Property

    Private fadeEffectValue As Boolean = True
    Public Property fadeEffect() As Boolean
        Get
            Return fadeEffectValue
        End Get
        Set(ByVal value As Boolean)
            fadeEffectValue = value
        End Set
    End Property

    Private _themeTextBoxes As Boolean = True
    Public Property themeTextBoxes() As Boolean
        Get
            Return _themeTextBoxes
        End Get
        Set(ByVal value As Boolean)
            _themeTextBoxes = value
        End Set
    End Property

    Private _themeCheckboxes As Boolean = True
    Public Property themeCheckboxes() As Boolean
        Get
            Return _themeCheckboxes
        End Get
        Set(ByVal value As Boolean)
            _themeCheckboxes = value
        End Set
    End Property

    Private _isIframe As Boolean = False
    Public Property isIframe() As Boolean
        Get
            Return _isIframe
        End Get
        Set(ByVal value As Boolean)
            _isIframe = value
        End Set
    End Property

    Private _theme As themes = themes.smoothness
    Public Property theme() As themes
        Get
            Return _theme
        End Get
        Set(ByVal value As themes)
            _theme = value
        End Set
    End Property


#End Region

    Private Sub AddThemeFilesP()
        jQueryInclude.RegisterJQueryUI(Page)
        jQueryInclude.addScriptFile(Page, "jQueryLibrary/jquery.formThemes.js")
        jQueryInclude.addScriptFile(Page, "jQueryLibrary/Base.css")

        If theme <> Nothing Then
            If Not Page.Items("currentJQUITheme") Is Nothing Then
                jQueryInclude.deleteScriptFile(Page, "ui-theme")
                'Dim tName As String = jQueryInclude.getEnumName(page.Items("currentJQUITheme")).Replace("_", "-")
                'jQueryInclude.deleteScriptFile(page, "jQueryLibrary/" & tName & "." & tName & ".css")
            End If
            Page.Items("currentJQUITheme") = theme
            Page.Items("currentCustomJQUITheme") = customThemePath
        Else
            theme = Page.Items("currentJQUITheme")
            customThemePath = Page.Items("currentCustomJQUITheme")
            If theme = Nothing Then
                theme = themes.smoothness
                'page.Items("currentJQUITheme") = theme
            End If
        End If

        Dim themeName As String = jQueryInclude.getEnumName(theme).Replace("_", "-")
        If Not theme = themes.custom Then
            jQueryInclude.addScriptFile(Page, "jQueryLibrary/" & themeName & "." & themeName & ".css", , , , "ui-theme")
        Else
            jQueryInclude.addScriptFile(Page, customThemePath, , , True, "ui-theme")
        End If
        jQueryInclude.addStyleBlock(Page, bodyCssFix, "UI-sizeFix")
		jQueryInclude.addScriptBlockPageLoad(Page, "$('button, input:submit, input:button, input:reset').button();", False, "ThemeButtons")
		jQueryInclude.addScriptBlockPageLoad(Page, "$('input[type=text],input[type=password],textarea,select').textbox();", False, "ThemeTextBoxes")
		jQueryInclude.addScriptBlockPageLoad(Page, "$('body').addClass('ui-widget');", False, "ThemeBody")
		'jQueryInclude.addScriptBlock(page, "window.onerror=function(){if(document.body){document.body.style.display='block';}else{document.write(""<style type='text/css'>body{ display:block; }</style>"");}}; document.write(""<style type='text/css'>body{ display:none; }</style>""); $(function(){document.body.style.display='block';$('body').hide().fadeIn(500,function(){}); });", False, , "bodyFadein")
		jQueryInclude.addScriptBlock(Page, "window.onerror=function(){if(document.body)document.body.style.display='block';}; document.write(""<style type='text/css'>body{ display:none; }</style>""); $(function(){document.body.style.display='block';$('body').hide().fadeIn(500,function(){}); });", False, Nothing, "bodyFadein", Nothing)
		jQueryInclude.addScriptBlockPageLoad(Page, "$('input:checkbox').checkbox();", False, "ThemeCheckBoxes")
		jQueryInclude.addScriptBlockPageLoad(Page, "$('input:radio').radio();", False, "ThemeRadioButtons")
		Dim header As jQueryInclude = jQueryInclude.getInitialInclude(Page)
        If Not themeButtons Then
            header.replaceIncludeByID("ThemeButtons", "")
        End If
        If Not themeTextBoxes Then
            header.replaceIncludeByID("ThemeTextBoxes", "")
        End If
        If Not themeBody Then
            header.replaceIncludeByID("ThemeBody", "")
        End If
        If Not fadeEffect Then
            header.replaceIncludeByID("bodyFadein", "")
        End If
        If Not themeCheckboxes Then
            header.replaceIncludeByID("ThemeCheckBoxes", "")
        End If

    End Sub

    Private Sub AddThemeToIframeP()
        jQueryInclude.RegisterJQueryUI(page)
        jQueryInclude.addScriptFile(page, "jQueryLibrary/jquery.formThemes.js")
        jQueryInclude.addScriptFile(page, "jQueryLibrary/Base.css")
        jQueryInclude.addScriptFile(page, "jQueryLibrary/smoothness.smoothness.css", , , , "ui-theme")
        jQueryInclude.addStyleBlock(page, bodyCssFix, "UI-sizeFix")

        'jQueryInclude.addScriptBlock(page, "", , , "ThemeButtons")
        'jQueryInclude.addScriptBlock(page, "", , , "ThemeTextBoxes")
        'jQueryInclude.addScriptBlock(page, "", , , "ThemeCheckBoxes")


        'jQueryInclude.addStyleBlock(page, ".ui-widget{ font-size:0.8em; }")
        Dim bodyStr As String = ""
        If themeBody Then
            bodyStr = "$('body').addClass('ui-widget-content ui-widget');"
        End If
		jQueryInclude.addScriptBlock(Page, "window.onerror=function(){document.body.style.display='block';}; document.write(""<style type='text/css'>body{ display:none; }</style>""); $(function(){document.body.style.display='block';$('body').hide().fadeIn(500,function(){}); });", minify:=False, id:="bodyFadein", jQueryIncludeHeader:=Nothing)
		'jQueryInclude.addScriptBlockPageLoad(page, "$('head').append(parent.$('#ui-theme').clone()); $('head').append(parent.$('#UI-sizeFix').clone()); " & bodyStr & " $('head').append(parent.$('#ThemeButtons').clone());$('head').append(parent.$('#ThemeTextBoxes').clone());$('head').append(parent.$('#ThemeCheckBoxes').clone());")
		jQueryInclude.addScriptBlockPageLoad(page, "addFromParent('#ui-theme');addFromParent('#UI-sizeFix'); " & bodyStr & _
            " addFromParent('#ThemeButtons');addFromParent('#ThemeTextBoxes');addFromParent('#ThemeCheckBoxes'); ")
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
    End Sub

    Private Sub ThemeAdder_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        If Not isIframe Then
            AddThemeFilesP()
        Else
            AddThemeToIframeP()
        End If
    End Sub
End Class