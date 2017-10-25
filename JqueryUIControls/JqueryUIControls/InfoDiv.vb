Public Class InfoDiv
    Inherits Panel

    'Private _text As String = ""
    'Public Property Text() As String
    '    Get
    '        For Each con As Control In Me.Controls
    '            If con.GetType Is GetType(LiteralControl) Then
    '                _text &= CType(con, LiteralControl).Text
    '            End If
    '        Next
    '        Return _text
    '    End Get
    '    Set(ByVal value As String)
    '        If Me.Controls.Count > 0 Then
    '            Dim litNum As Integer = 0
    '            Dim litcon As New LiteralControl
    '            For Each con As Control In Me.Controls
    '                If con.GetType Is GetType(LiteralControl) Then
    '                    litNum += 1
    '                    litcon = CType(con, LiteralControl)
    '                End If
    '            Next
    '            If litNum = 1 Then
    '                litcon.Text = value
    '            Else
    '                Me.Controls.Add(New LiteralControl(value))
    '            End If
    '        Else
    '            Me.Controls.Add(New LiteralControl(value))
    '        End If
    '    End Set
    'End Property

    Private _isError As Boolean = False
    Public Property isError() As Boolean
        Get
            Return _isError
        End Get
        Set(ByVal value As Boolean)
            _isError = value
        End Set
    End Property

    Private _hideIcon As Boolean = False
    Public Property HideIcon() As Boolean
        Get
            Return _hideIcon
        End Get
        Set(ByVal value As Boolean)
            _hideIcon = value
        End Set
    End Property

    Private _txtcontrol As New LiteralControl
    Public Property text() As String
        Get
            Return _txtcontrol.Text
        End Get
        Set(ByVal value As String)
            _txtcontrol.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.ThemeAdder.AddTheme(page)
        End If
    End Sub

    Private Sub control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
        'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "JqueryUIControls/fullcalendar.print.css")
    End Sub

    Private Sub InfoDiv_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
       
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim ico As String = "info"
        Dim cs As String = "highlight"

        If isError Then
            ico = "alert"
            cs = "error"
        End If

        Dim innercon As New Collection
        For Each con As Control In Me.Controls
            innercon.Add(con)
        Next
        Me.Controls.Clear()

        Dim panerr As New Panel
        Dim pIcon As New HtmlGenericControl
        Dim pText As New HtmlGenericControl
        pIcon.TagName = "p"
        pText.TagName = "p"

        pText.Style.Clear()
        pText.Style.Add("padding-left", "2em")
        panerr.CssClass = "ui-state-" & cs & " ui-corner-all"
        panerr.Style.Clear()
        panerr.Style.Add("padding", "0 .7em")

        If Not HideIcon Then
            pIcon.Controls.AddAt(0, New LiteralControl("<span class=""ui-icon ui-icon-" & ico & """ style=""float: left; margin-right: .3em;""></span>"))
        End If

        If _txtcontrol.Text = "" Then
            For Each con As Control In innercon
                pText.Controls.Add(con)
            Next
        Else
            pText.Controls.Add(_txtcontrol)
        End If

        panerr.Controls.Add(pIcon)
        panerr.Controls.Add(pText)
        Me.Controls.Add(panerr)
        Me.CssClass = Me.CssClass.Replace("ui-widget", "")
        Me.CssClass = ("ui-widget " & Me.CssClass).Trim
        MyBase.Render(writer)
    End Sub
End Class
