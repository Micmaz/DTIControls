#If DEBUG Then
Public Class SharedHighslideVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SharedHighslideVariables
#End If
        Public Enum Highslide_Outline_Scheme
            RoundedWhite
            RoundedDark
            Beveled
            DropShadow
            GlossyDark
            OuterGlow
        End Enum

        Public Enum HighslideDisplayModes
            Image
            HTML
            Iframe
        End Enum

        Public Shared Function getOutlineText(ByVal enumVal As Highslide_Outline_Scheme) As String
            Select Case enumVal
                Case Highslide_Outline_Scheme.Beveled
                    Return "beveled"
                Case Highslide_Outline_Scheme.DropShadow
                    Return "drop-shadow"
                Case Highslide_Outline_Scheme.GlossyDark
                    Return "glossy-dark"
                Case Highslide_Outline_Scheme.OuterGlow
                    Return "outer-glow"
                Case Highslide_Outline_Scheme.RoundedDark
                    Return "rounded-black"
                Case Highslide_Outline_Scheme.RoundedWhite
                    Return "rounded-white"
            End Select
            Return "rounded-white"
        End Function

    End Class
