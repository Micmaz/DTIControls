Imports DTIMiniControls.TreeList
<ComponentModel.ToolboxItem(False)> _
Public Class TreeListItem
    Inherits HTMLListItem

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
            If _containerHTMLList IsNot Nothing AndAlso value Then
                _containerHTMLList.CssClass = "open"
            End If
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

    Private _checkStyle As CheckBoxStyles = CheckBoxStyles.None
    Public Property CheckStyle() As CheckBoxStyles
        Get
            Return _checkStyle
        End Get
        Set(ByVal value As CheckBoxStyles)
            _checkStyle = value
        End Set
    End Property

    Private _nodeType As String
    Public Property NodeType() As String
        Get
            If _nodeType = Nothing Then
                Return ""
            Else
                Return _nodeType
            End If
        End Get
        Set(ByVal value As String)
            _nodeType = value
            If _nodeType = "" Then
                _nodeType = Nothing
            End If
            Me.Attributes("rel") = _nodeType
        End Set
    End Property

    'this is not the parent
    Protected Shadows _containerHTMLList As TreeListUL
    Protected Shadows ReadOnly Property ContainerHTMLList() As TreeListUL
        Get
            If _containerHTMLList Is Nothing Then
                _containerHTMLList = New TreeListUL()
                Controls.Add(_containerHTMLList)
            End If
            Return _containerHTMLList
        End Get
    End Property

    Private ReadOnly Property MyRootDiv() As TreeList
        Get
            Return RootHTMLDiv(Me)
        End Get
    End Property

    Private Function RootHTMLDiv(ByRef item As Object) As TreeList
        If item.Parent.GetType Is GetType(TreeList) Then
            Return item.Parent
        End If
        Return RootHTMLDiv(item.Parent)
    End Function

    Public Overrides Property ID() As String
        Get
            If MyBase.ID = "" Then
                MyRootDiv.ItemCount += 1
                MyBase.ID = MyRootDiv.ID & "_" & MyRootDiv.ItemCount
            End If
            Return MyBase.ID
        End Get
        Set(ByVal value As String)
            MyBase.ID = value
        End Set
    End Property

    Public ReadOnly Property ParentNode() As TreeListItem
        Get
            Try
                If TypeOf Me.Parent.Parent Is TreeListItem Then
                    Return Me.Parent.Parent
                End If
            Catch ex As Exception
            End Try
            Return Nothing
        End Get
    End Property

    Private _value As String = "-1"
    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
        End Set
    End Property

    Private _text As String
    Public Overrides Property Text() As String
        Get
            If _text Is Nothing Then
                Return "NODE_" & ContainerHTMLList.Controls.IndexOf(Me)
            End If
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Li
        End Get
    End Property

    Public Sub New(ByVal _text As String, ByVal val As String, Optional ByVal nodeType As String = Nothing, Optional ByVal className As String = Nothing)
        Attributes.Add("rel", "default")
        Text = _text
        Value = val
        Attributes.Add("key", val)
        If className IsNot Nothing Then
            CssClass = className
        End If
        If nodeType IsNot Nothing AndAlso nodeType <> "" Then
            Me.NodeType = nodeType
        End If
    End Sub

    Public Sub New()
        Attributes.Add("rel", "default")
    End Sub

    Private Sub TreeListItem_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Try
            'if I have leafs and if I'm expanded
            If _containerHTMLList IsNot Nothing AndAlso Expanded Then
                MyRootDiv.addOpenNodeId(Me.ID)
            End If
            Dim str As String = CType(Me.Controls(0), HyperLink).Text
            If Not str.Contains("<ins>&nbsp;</ins>") Then
                str = "<ins>&nbsp;</ins>" & str
            End If
            CType(Me.Controls(0), HyperLink).Text = str
        Catch ex As Exception
        End Try
    End Sub

    Public Shadows Sub addItems(ByRef items As TreeListItem())
        For Each item As TreeListItem In items
            Me.ContainerHTMLList.addListItemRoot(item)
        Next
    End Sub

    Public Shadows Sub addItem(ByRef item As TreeListItem)
        Me.ContainerHTMLList.addListItemRoot(item)
    End Sub
End Class