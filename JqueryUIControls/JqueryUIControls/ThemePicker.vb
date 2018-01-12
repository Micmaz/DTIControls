Public Class ThemePicker
    Inherits Panel

    Event SetTheme(ByVal theme As jQueryLibrary.ThemeAdder.themes)


#Region "properties"
    Private _useCookie As Boolean = False
    Public Property useCookie() As Boolean
        Get
            Return _useCookie
        End Get
        Set(ByVal value As Boolean)
            _useCookie = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/themeswitchertool.js", , True)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""
        If Width <> Nothing Then
            outstr &= "width:" & Width.Value.ToString & ","
        End If
        If Height <> Nothing Then
            outstr &= "height:" & Height.Value.ToString & ","
        End If
        If useCookie Then
            outstr &= "useCookie:true,"
        End If
        outstr = outstr.TrimEnd(",")
        Return outstr
    End Function


    Private isHidden As Boolean = False
    Public Overrides Property Visible() As Boolean
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            Hidden = Not value
            MyBase.Visible = True
        End Set
    End Property

    Public Property Hidden() As Boolean
		Get
			Return isHidden
		End Get
		Set(ByVal value As Boolean)
            isHidden = value
            MyBase.Visible = True
        End Set
    End Property

    Private DefaultThemeValue As jQueryLibrary.ThemeAdder.themes
    Public Property DefaultTheme() As jQueryLibrary.ThemeAdder.themes
        Get
            Return DefaultThemeValue
        End Get
        Set(ByVal value As jQueryLibrary.ThemeAdder.themes)
            DefaultThemeValue = value
        End Set
    End Property

    Private Sub Picker_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.DesignMode Then Return
		Dim s As String = ""
		s &= "     $('#" & Me.ClientID & "').themeswitcher({" & renderparams() & "});"
		If Me.useCookie Then
            If Not Page.Request.Cookies("jquery-ui-theme") Is Nothing Then
                'If Page.Items("currentJQUITheme") Is Nothing Then
                Dim theme As String = Page.Request.Cookies("jquery-ui-theme").Value
                theme = theme.ToLower().Replace("-", "_")
                For Each item As Integer In System.Enum.GetValues(GetType(jQueryLibrary.ThemeAdder.themes))
                    If [Enum].GetName(GetType(jQueryLibrary.ThemeAdder.themes), item) = theme Then
                        jQueryLibrary.ThemeAdder.AddTheme(Me.Page, item)
                    End If
                Next
                'End If
            Else
                If Not DefaultTheme = Nothing Then
                    Dim aCookie As New HttpCookie("jquery-ui-theme")
                    aCookie.Value = DefaultTheme
                    Page.Response.Cookies.Add(aCookie)
                    jQueryLibrary.ThemeAdder.AddTheme(Me.Page, DefaultTheme)
                End If
            End If
        Else
            If Not DefaultTheme = Nothing Then
                jQueryLibrary.ThemeAdder.AddTheme(Me.Page, DefaultTheme)
            End If
        End If
        If Not Hidden Then _
			jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, s)
	End Sub

End Class

