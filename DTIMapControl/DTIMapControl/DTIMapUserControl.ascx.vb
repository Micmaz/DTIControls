Imports BaseClasses
Imports HighslideControls

#If DEBUG Then
Partial Public Class DTIMapUserControl
    Inherits BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class DTIMapUserControl
        Inherits BaseSecurityUserControl
#End If

        Public ReadOnly Property directionsSpan() As Label
            Get
                Return lblDirection
            End Get
        End Property

        Public ReadOnly Property mapSpan() As Label
            Get
                Return lblMap
            End Get
        End Property

    End Class