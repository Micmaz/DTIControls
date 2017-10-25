#If DEBUG Then
Public Class NodeImageIcon
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class NodeImageIcon
#End If
        Private _image As String = "false"
        Public Property Image() As String
            Get
                Return _image
            End Get
            Set(ByVal value As String)
                _image = value
            End Set
        End Property

        Private _position As String = "false"
        Public Property Position() As String
            Get
                Return _position
            End Get
            Set(ByVal value As String)
                _position = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return "image:""" & Image & """," & "position:""" & Position & """"
        End Function

        Public Sub New(ByVal img As String, Optional ByVal position As String = Nothing)
            Image = img
            If position IsNot Nothing Then
                Me.Position = position
            End If
        End Sub

        Public Sub New()

        End Sub
    End Class