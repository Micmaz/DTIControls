Public Class TreeListItem
    Inherits HtmlGenericControl

    Private _items As New TreeListItemsCollection
    Public ReadOnly Property Items() As TreeListItemsCollection
        Get
            Return _items
        End Get
    End Property

    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal value As Integer)
            _value = value
        End Set
    End Property

    Private _text As String
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Public Overrides Property InnerText() As String
        Get
            Return "Value: " & Me.Value & ", Text: " & Me.Text
        End Get
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Private _selected As Boolean = False
    Public Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Private _expanded As Boolean = False
    Public Property Expanded() As Boolean
        Get
            Return _expanded
        End Get
        Set(ByVal value As Boolean)
            _expanded = value
        End Set
    End Property

    Private _dropped As Boolean = False
    Public Property Dropped() As Boolean
        Get
            Return _dropped
        End Get
        Set(ByVal value As Boolean)
            _dropped = value
        End Set
    End Property

    Private _checkStyle As TreeList.CheckBoxStyles = TreeList.CheckBoxStyles.None
    Public Property CheckStyle() As TreeList.CheckBoxStyles
        Get
            Return _checkStyle
        End Get
        Set(ByVal value As TreeList.CheckBoxStyles)
            _checkStyle = value
        End Set
    End Property

    Private _ItemType As String
    Public Property ItemType() As String
        Get
            If _ItemType = Nothing Then
                Return ""
            Else
                Return _ItemType
            End If
        End Get
        Set(ByVal value As String)
            _ItemType = value
        End Set
    End Property

    Private _parentValue As Integer = -1
    Public Property ParentValue() As Integer
        Get
            Return _parentValue
        End Get
        Set(ByVal value As Integer)
            _parentValue = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal Value As Integer, ByVal Text As String, Optional ByVal nodeType As String = Nothing)
        Me.Value = Value
        Me.Text = Text
        Me.ItemType = nodeType
    End Sub
End Class
