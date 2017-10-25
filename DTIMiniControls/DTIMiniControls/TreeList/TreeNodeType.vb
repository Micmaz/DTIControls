#If DEBUG Then
Public Class TreeNodeType
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class TreeNodeType
#End If
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _clickable As Boolean = True
        Public Property Clickable() As Boolean
            Get
                Return _clickable
            End Get
            Set(ByVal value As Boolean)
                _clickable = value
            End Set
        End Property

        Private _renameable As Boolean = True
        Public Property Renameable() As Boolean
            Get
                Return _renameable
            End Get
            Set(ByVal value As Boolean)
                _renameable = value
            End Set
        End Property

        Private _deletable As Boolean = True
        Public Property Deletable() As Boolean
            Get
                Return _deletable
            End Get
            Set(ByVal value As Boolean)
                _deletable = value
            End Set
        End Property

        Private _creatable As Boolean = True
        Public Property Creatable() As Boolean
            Get
                Return _creatable
            End Get
            Set(ByVal value As Boolean)
                _creatable = value
            End Set
        End Property

        Private _draggable As Boolean = True
        Public Property Draggable() As Boolean
            Get
                Return _draggable
            End Get
            Set(ByVal value As Boolean)
                _draggable = value
            End Set
        End Property

        Private _maxChildren As Integer = -1
        Public Property MaxChildren() As Integer
            Get
                Return _maxChildren
            End Get
            Set(ByVal value As Integer)
                _maxChildren = value
            End Set
        End Property

        Private _maxDepth As Integer = -1
        Public Property MaxDepth() As Integer
            Get
                Return _maxDepth
            End Get
            Set(ByVal value As Integer)
                _maxDepth = value
            End Set
        End Property

        Private _validChildren As String = """all"""
        Public Property ValidChildren() As String
            Get
                Return _validChildren.Replace("""", "")
            End Get
            Set(ByVal value As String)
                If value.IndexOf("[") = -1 Then
                    value = """" & value & """"
                End If
                _validChildren = value
            End Set
        End Property

        Private _icon As NodeImageIcon
        Public Property Icon() As NodeImageIcon
            Get
                Return _icon
            End Get
            Set(ByVal value As NodeImageIcon)
                _icon = value
            End Set
        End Property

        Public Sub New(ByVal name As String)
            Me.Name = name
        End Sub

        Public Sub New(ByVal name As String, ByVal isClickable As Boolean, ByVal isRenameable As Boolean, ByVal isDeletable As Boolean, _
                        ByVal isCreatable As Boolean, ByVal isDraggable As Boolean, ByVal Max_Children As Integer, _
                        ByVal Max_Depth As Integer, ByVal ValidChildrenSet As String, Optional ByVal iconimage As NodeImageIcon = Nothing)
            Me.Name = name
            Clickable = isClickable
            Renameable = isRenameable
            Deletable = isDeletable
            Creatable = isCreatable
            Draggable = isDraggable
            MaxChildren = Max_Children
            MaxDepth = Max_Depth
            ValidChildren = ValidChildrenSet
            Icon = iconimage
        End Sub

        Public Overrides Function ToString() As String
            Dim str As String = """" & Name & """ : {"
            str &= "clickable:" & Clickable.ToString.ToLower & ","
            str &= "renameable:" & Renameable.ToString.ToLower & ","
            str &= "deletable:" & Deletable.ToString.ToLower & ","
            str &= "creatable:" & Creatable.ToString.ToLower & ","
            str &= "draggable:" & Draggable.ToString.ToLower & ","
            str &= "max_children:" & MaxChildren & ","
            str &= "max_depth:" & MaxDepth & ","
            str &= "valid_children:" & _validChildren
            If Icon IsNot Nothing Then
                str &= ",icon:{" & Icon.ToString & "}"
            End If
            str &= "}"
            Return str
        End Function
    End Class