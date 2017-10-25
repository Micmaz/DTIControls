Public Class maskedTextbox
    Inherits TextBox

    Private _mask As String = "(999) 999-9999"
    Public Property mask() As String
        Get
            Return _mask
        End Get
        Set(ByVal value As String)
            _mask = value
            _maskPreset = maskType.Custom
        End Set
    End Property

    Private _maskPreset As maskType = maskType.Phone
    Public Property maskPreset() As maskType
        Get
            Return _maskPreset
        End Get
        Set(ByVal value As maskType)
            If value = maskType.Date Then
                _mask = "99/99/9999"
            ElseIf value = maskType.Numeric Then
                _mask = "?99999999999999999"
            ElseIf value = maskType.Phone Then
                _mask = "(999) 999-9999"
            ElseIf value = maskType.PhoneWithExtension Then
                _mask = "(999) 999-9999? x99999"
            ElseIf value = maskType.SSN Then
                _mask = "999-99-9999"
            End If
            _maskPreset = value
        End Set
    End Property

    Public ReadOnly Property unMaskedText() As String
        Get
            Dim input As String = Me.Text
            Dim out As String = ""
            Dim strIndex As Integer = 0
            For maskindex As Integer = 0 To mask.Length - 1
                Dim c As Char = mask(maskindex)
                If strIndex < input.Length Then
                    If c = "9" Or c = "a" Or c = "*" Then
                        out &= input(strIndex)
                    End If
                    If Not c = "?" Then
                        strIndex += 1
                    End If
                End If
            Next
            Return out
        End Get
    End Property

    Public Enum maskType
        Numeric
        Phone
        PhoneWithExtension
        SSN
        [Date]
        Custom
    End Enum

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.maskedinput-1.3.min.js")
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Sub maskedTextbox_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, "$(""#" & Me.ClientID & """).mask(""" & mask & """);")
    End Sub

End Class
