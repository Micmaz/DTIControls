Imports DTIImageManager.dsImageManager
Imports DTIServerControls.DTISharedVariables

#If DEBUG Then
Public Class SharedImageVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SharedImageVariables
#End If
        Public Shared ReadOnly Property myImages() As DTIImageManagerDataTable
        Get
            If Session("DTIImageManagerDataTable") Is Nothing Then
                Session("DTIImageManagerDataTable") = New DTIImageManagerDataTable
            End If
            Return Session("DTIImageManagerDataTable")
        End Get
        End Property

    End Class
