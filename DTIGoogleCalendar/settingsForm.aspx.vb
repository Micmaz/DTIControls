Imports System.Text.RegularExpressions

Partial Public Class settingsForm
    Inherits DTIServerControls.SettingsEditorPage

    Public ReadOnly Property ctrl() As DTIGoogleCal
        Get
            Return DTISeverControlTarget
        End Get
    End Property

    Private Sub settingsForm_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.tbWidth.Text = ctrl.Width.Value
        Me.tbHeight.Text = ctrl.Height.Value

        tbFrameBorder.Text = ctrl.frameBorder
        tbBGColor.Text = ctrl.backgroundColor
        tbBorderWidth.Text = ctrl.borderWidth
        tbSrc.Text = ctrl.src
        cbAllowTransparency.Checked = ctrl.allowTransparency
        cbScrolling.Checked = ctrl.scrolling
    End Sub

    Private Sub settingsForm_saveClicked() Handles Me.saveClicked
        If tbSrc.Text.StartsWith("<iframe") Then
            Dim result As String = findAttributeValue(tbSrc.Text, "src")
            If result Is Nothing Then
                lblSrcError.Visible = True
            Else
                ctrl.src = result
                lblSrcError.Visible = False
            End If

        ElseIf tbSrc.Text.StartsWith("http") Then
            ctrl.src = tbSrc.Text
            lblSrcError.Visible = False
        Else
            lblSrcError.Visible = True
        End If

        If tbWidth.Text.Trim.Length > 0 Then
            ctrl.Width = tbWidth.Text
        End If
        If tbHeight.Text.Trim.Length > 0 Then
            ctrl.Height = tbHeight.Text
        End If
        ctrl.frameBorder = tbFrameBorder.Text
        ctrl.backgroundColor = tbBGColor.Text
        ctrl.borderWidth = tbBorderWidth.Text
        ctrl.allowTransparency = cbAllowTransparency.Checked
        ctrl.scrolling = cbScrolling.Checked

    End Sub

    Private Function getTagAttributes(ByVal tag As String) As Collection
        Dim attributes As New Collection
        Dim attPattern As String = "(?<name>\b\w+\b)\s*=\s*""(?<value>""[^""]*""|'[^']*'|[^""'<>\s]+)"""

        For Each match As Match In Regex.Matches(tag, attPattern)
            attributes.Add(match.Groups("value").Value, match.Groups("name").Value)
        Next

        Return attributes
    End Function

    Private Function findAttributeValue(ByVal tag As String, ByVal attributeName As String) As String
        Try
            Dim attributes As Collection = getTagAttributes(tag)
            Dim result As String = attributes(attributeName)
            Return result
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class