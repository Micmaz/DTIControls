Imports DTIMediaManager
Imports DTIServerControls

''' <summary>
''' Control to display dynamic page information.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class PageSummary
    Inherits MediaInfo
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class PageSummary
        Inherits MediaInfo
#End If

        Private Sub PageSummary_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            ShowPubInfo = False
            TitleIsLink = True
            ShowSharing = False
            Me.MyInfoControl.starRater.ValuesPerStar = 1
            'Me.MyInfoControl.starRater.Enabled = False
        End Sub
    End Class
