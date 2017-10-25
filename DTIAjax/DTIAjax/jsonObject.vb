''' <summary>
''' json return object
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class jsonObject
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class jsonObject
#End If
        Public action As String
        Public data As Object

    End Class
