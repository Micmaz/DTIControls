Imports System.Web.SessionState
Imports System.Web
Imports DTIMapControl.dsMapControl
Imports DTIServerControls.DTISharedVariables

#If DEBUG Then
Public Class SharedMapVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SharedMapVariables
#End If
        Public Shared ReadOnly Property addressTable() As DTIMapControlDataTable
            Get
                Return dsMaps.DTIMapControl
            End Get
        End Property

        Public Shared ReadOnly Property googleKeyTable() As DTIGoogleMapKeyDataTable
            Get
                Return dsMaps.DTIGoogleMapKey
            End Get
        End Property

        Public Shared ReadOnly Property dsMaps() As dsMapControl
            Get
                If Session("DTIMapControlDataset") Is Nothing Then
                    Session("DTIMapControlDataset") = New dsMapControl
                End If
                Return Session("DTIMapControlDataset")
            End Get
        End Property


    End Class
