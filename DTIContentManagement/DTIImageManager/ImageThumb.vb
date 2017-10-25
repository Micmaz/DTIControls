Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' simple control to display image with click-to-enlarge capability
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ImageThumb
    Inherits Highslider
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ImageThumb
        Inherits Highslider
#End If
        Private _imageId As Integer = -1
        Public Property ImageId() As Integer
            Get
                Return _imageId
            End Get
            Set(ByVal value As Integer)
                _imageId = value
            End Set
        End Property

        Private _thumbSize As Integer = 120
        Public Property ThumbSize() As Integer
            Get
                Return _thumbSize
            End Get
            Set(ByVal value As Integer)
                _thumbSize = value
            End Set
        End Property

        Private _maxWidth As Integer = _thumbSize
        Public Property MaxThumbWidth() As Integer
            Get
                Return _maxWidth
            End Get
            Set(ByVal value As Integer)
                _maxWidth = value
            End Set
        End Property

        Private _maxHeight As Integer = _thumbSize
        Public Property MaxThumbHeight() As Integer
            Get
                Return _maxHeight
            End Get
            Set(ByVal value As Integer)
                _maxHeight = value
            End Set
        End Property

        Private _refreshImage As Boolean = False
        Public Property RefreshImage() As Boolean
            Get
                Return _refreshImage
            End Get
            Set(ByVal value As Boolean)
                _refreshImage = value
            End Set
        End Property

        Public Overrides Property ExpandURL() As String
            Get
                If MyBase.ExpandURL = "" Then
                    MyBase.ExpandURL = "~/res/DTIImageManager/ViewImage.aspx?Id=" & ImageId
                End If
                Return MyBase.ExpandURL
            End Get
            Set(ByVal value As String)
                MyBase.ExpandURL = value
            End Set
        End Property

        Public Overrides Property ThumbURL() As String
            Get
                If MyBase.ThumbURL = "" Then
                    MyBase.ThumbURL = "~/res/DTIImageManager/ViewImage.aspx?Id=" & ImageId & _
                        "&maxHeight=" & MaxThumbHeight & "&maxWidth=" & MaxThumbWidth
                End If
                Dim rdm As New Random
                If RefreshImage Then
                    MyBase.ThumbURL &= "&r=" & rdm.Next(1000)
                End If
                Return MyBase.ThumbURL
            End Get
            Set(ByVal value As String)
                MyBase.ThumbURL = value
            End Set
        End Property

        Public Overrides Property HighslideDisplayMode() As HighslideDisplayModes
            Get
                Return HighslideDisplayModes.Image
            End Get
            Set(ByVal value As HighslideDisplayModes)
            End Set
        End Property
    End Class