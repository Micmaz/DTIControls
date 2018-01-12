Public Class ColorPicker
    Inherits Panel

#Region "Properties"

    Public Property color() As System.Drawing.Color
        Get
            Try
                Return System.Drawing.ColorTranslator.FromHtml(colorValue)
            Catch ex As Exception
                Return Drawing.Color.Black
            End Try
        End Get
        Set(ByVal value As System.Drawing.Color)
            hidVal.Value = System.Drawing.ColorTranslator.ToHtml(value)
        End Set
    End Property

    Public Property colorValue() As String
        Get
            Return hidVal.Value
        End Get
        Set(ByVal value As String)
            If Not value.StartsWith("#") Then
                value = "#" & value
            End If
            hidVal.Value = value
        End Set
    End Property

    Private minViewValue As Boolean = True
    Public Property minView() As Boolean
        Get
            Return minViewValue
        End Get
        Set(ByVal value As Boolean)
            minViewValue = value
        End Set
    End Property

    Private miniPickerValue As Boolean = False
    Public Property miniPicker() As Boolean
        Get
            Return miniPickerValue
        End Get
        Set(ByVal value As Boolean)
            miniPickerValue = value
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
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/colorpicker.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/colorpicker.css")
        End If
    End Sub

    Private Sub control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = "onShow: function (colpkr) {$(colpkr).css('z-index', getMaxZ('*') + 1);$(colpkr).fadeIn(500);return false;}," & vbCrLf
        outstr &= "onHide: function (colpkr) {$(colpkr).fadeOut(500);return false;}," & vbCrLf
        outstr &= "onChange: function (hsb, hex, rgb) {$('#" & Me.ClientID & " div').css('backgroundColor', '#' + hex);" & vbCrLf
        outstr &= "$('#" & Me.hidVal.ClientID & "').val('#'+hex);"
        outstr &= "},"
        outstr &= jsPropString("min", minView)
        outstr &= jsPropString("color", color)
        outstr = outstr.Trim(",")
        Return outstr
    End Function



	Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		if miniPicker then Me.CssClass = "colorSelector colorSelectormini"
		Dim script As String = ""
		Me.Controls.Add(New LiteralControl("<div style='background-color: " & colorValue & ";'></div>"))
        'str &= "$('#" & Me.ClientID & "').append(""<div style='background-color: " & colorValue & ";'></div>"");" & vbCrLf
        Dim id As String = "" & Me.ID
        If id = "" Then
            id = ClientID
        End If

		'str &= "$('#" & Me.ClientID & "').addClass('colorSelector');" & vbCrLf
		script &= "window." & id & " = $('#" & Me.ClientID & "').ColorPicker({" & vbCrLf
		script &= renderparams()
		script &= "        }); "
		writer.Write(jQueryLibrary.jQueryInclude.isolateJqueryLoad(script, True))

		MyBase.Render(writer)

    End Sub

    Public hidVal As New HiddenField
    Public Sub New()
        Me.Controls.Add(hidVal)
        Me.CssClass = "colorSelector"
    End Sub

	Private Sub ColorPicker_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
		If miniPicker Then Me.CssClass = "colorSelector colorSelectormini"
		'Dim str As String = ""

		''str &= "$('#" & Me.ClientID & "').addClass('colorSelector');" & vbCrLf
		'str &= "window." & ID & " = $('#" & Me.ClientID & "').ColorPicker({" & vbCrLf
		'str &= renderparams()
		'str &= "        });"
		'jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, str)
	End Sub
End Class

