Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables

#If DEBUG Then
Partial Public Class ThumbControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ThumbControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If

        Public Property ThumbSpanWidth() As Unit
            Get
                Return lblThumb.Width
            End Get
            Set(ByVal value As Unit)
                lblThumb.Width = value
            End Set
        End Property

        Public Property ThumbSpanHeight() As Unit
            Get
                Return lblThumb.Height
            End Get
            Set(ByVal value As Unit)
                lblThumb.Height = value
            End Set
        End Property

        Public Property ThumbId() As String
            Get
                Return lblThumb.ID
            End Get
            Set(ByVal value As String)
                lblThumb.ID = value
            End Set
        End Property

        Public ReadOnly Property ThumbControls() As ControlCollection
            Get
                Return phThumbControls.Controls
            End Get
        End Property


    End Class