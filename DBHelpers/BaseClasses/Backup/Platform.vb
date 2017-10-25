#If DEBUG Then
Public Class Platform
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Platform
#End If
    Private Shared _ismono As Integer = -1

    ''' <summary>
    ''' Returns true if the application is running on the mono platform.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Returns true if the application is running on the mono platform.")> _
        Public Shared ReadOnly Property isMono() As Boolean
            Get
                If _ismono < 0 Then
                    Try
                        Dim t As Type = Type.GetType("Mono.Runtime")
                        If t Is Nothing Then
                            _ismono = 0
                        Else : _ismono = 1
                        End If
                    Catch ex As Exception
                        _ismono = 0
                    End Try
                End If
                Return _ismono > 0
            End Get
        End Property


    End Class
