Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DTIMiniControls.TreeList
Imports DTIMiniControls.TreeList.CheckBoxStyles
Imports BaseClasses

'please note that this file contains four classes that are used for the tree stuff

<AspNetHostingPermission(SecurityAction.Demand, _
    Level:=AspNetHostingPermissionLevel.Minimal), _
AspNetHostingPermission(SecurityAction.InheritanceDemand, _
    Level:=AspNetHostingPermissionLevel.Minimal), _
DefaultProperty("CheckStyle"), _
ToolboxData("<{0}:TreeList runat=""server""> </{0}:TreeList>")> _
<ComponentModel.ToolboxItem(False)> _
Public Class TreeList
    Inherits HTMLList

#Region "Controls"
    Private openNodes As String = ""

    Private litCreate As New Literal
    Private litRename As New Literal
    Private litDelete As New Literal
    Private litNewNode As New Literal
    Private litEmptyNode As New Literal
#End Region

#Region "Enumerators"
    'ele.push(NODE.id, ParentID, Value, this.get_text(NODE), checked, selected, open);
    Public Enum ItemFieldIndex
        Id
        ParentId
        Value
        Text
        CheckState
        Selected
        Expanded
        NodeType
    End Enum

    Public Enum CheckBoxStyles
        Binary
        Trinary
        None
    End Enum

    Public Enum MultiSelectStyles
        [false]
        [on]
        ctrl
    End Enum
#End Region

#Region "Properties"
    Public dt As DataTable

    Private _dv As DataView
    Public Property dv() As DataView
        Get
            If _dv Is Nothing Then
                _dv = dt.DefaultView
            End If
            Return _dv
            'Return New DataView(_dv.Table, _dv.RowFilter, _dv.Sort, _dv.RowStateFilter)
        End Get
        Set(ByVal value As DataView)
            _dv = value
        End Set
    End Property

    Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Div
        End Get
    End Property

#Region "Non-Browsable"
    <Browsable(False)> _
    Private ReadOnly Property HiddenDataString() As String
        Get
            Return Me.Page.Request.Params(Me.ClientID & "_hidden")
        End Get
    End Property

    <Browsable(False)> _
    Private ReadOnly Property HiddenDeletedString() As String
        Get
            Return Me.Page.Request.Params(Me.ClientID & "_deleted")
        End Get
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

    <Browsable(False)> _
    Public ReadOnly Property CheckStyleString() As String
        Get
            Return System.Enum.GetName(GetType(CheckBoxStyles), CheckStyle)
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property MultiStyleString() As String
        Get
            Return System.Enum.GetName(GetType(MultiSelectStyles), MultiSelection)
        End Get
    End Property

    Private _itemCount As Integer = 0
    <Browsable(False)> _
    Public Property ItemCount() As Integer
        Get
            Return _itemCount
        End Get
        Set(ByVal value As Integer)
            _itemCount = value
        End Set
    End Property

    Private _myFirstUL As TreeListUL
    <Browsable(False)> _
    Public ReadOnly Property MyFirstUL() As TreeListUL
        Get
            If _myFirstUL Is Nothing Then
                _myFirstUL = New TreeListUL
                Controls.Add(_myFirstUL)
            End If
            Return _myFirstUL
        End Get
    End Property

    Private _Nodes As TreeListItem() = Nothing
    <Browsable(False)> _
    Public ReadOnly Property Nodes() As TreeListItem()
        Get
            If _Nodes Is Nothing Then
                Dim i As Integer = 0
                _Nodes = GetMyNodes()
            End If
            Return _Nodes
        End Get
    End Property

    Private _SelectedNodes As TreeListItem() = Nothing
    <Browsable(False)> _
    Public ReadOnly Property SelectedNodes() As TreeListItem()
        Get
            If _SelectedNodes Is Nothing Then
                _SelectedNodes = GetMySelectedNodes()
            End If
            Return _SelectedNodes
        End Get
    End Property

    Private _CheckedNodes As TreeListItem() = Nothing
    <Browsable(False)> _
    Public ReadOnly Property CheckedNodes() As TreeListItem()
        Get
            If _CheckedNodes Is Nothing Then
                _CheckedNodes = GetMyCheckedNodes()
            End If
            Return _CheckedNodes
        End Get
    End Property

    Private _NodeTypes As List(Of TreeNodeType)
    <Browsable(False)> _
    Public ReadOnly Property NodeTypes() As List(Of TreeNodeType)
        Get
            If _NodeTypes Is Nothing Then
                _NodeTypes = New List(Of TreeNodeType)
            End If
            Return _NodeTypes
        End Get
    End Property

    Private _customContextMenuCollection As Collection
    <Browsable(False)> _
    Private ReadOnly Property CustomContextMenuCollection() As Collection
        Get
            If _customContextMenuCollection Is Nothing Then
                _customContextMenuCollection = New Collection
            End If
            Return _customContextMenuCollection
        End Get
    End Property
#End Region

#Region "Data Properties"

    Private _IdColumnName As String = "Id"
    Public Property IdColumnName() As String
        Get
            Return _IdColumnName
        End Get
        Set(ByVal value As String)
            _IdColumnName = value
        End Set
    End Property

    Private _TextColumnName As String = ""
    Public Property TextColumnName() As String
        Get
            Return _TextColumnName
        End Get
        Set(ByVal value As String)
            _TextColumnName = value
        End Set
    End Property

    Private _ParentIdColumnName As String = "Parent_Id"
    Public Property ParentIdColumnName() As String
        Get
            Return _ParentIdColumnName
        End Get
        Set(ByVal value As String)
            _ParentIdColumnName = value
        End Set
    End Property

    Private _SortColumnName As String = ""
    Public Property SortColumnName() As String
        Get
            Return _SortColumnName
        End Get
        Set(ByVal value As String)
            _SortColumnName = value
        End Set
    End Property

    Private _NodeTypeColumnName As String
    Public Property NodeTypeColumnName() As String
        Get
            Return _NodeTypeColumnName
        End Get
        Set(ByVal value As String)
            _NodeTypeColumnName = value
        End Set
    End Property
#End Region

#Region "Behavior Properties"

    Private _multiStyle As MultiSelectStyles = MultiSelectStyles.ctrl

    ''' <summary>
    ''' This controls multiple selection. Can be either false - multiple 
    ''' selection is off, "ctrl" - multiple selection is on when the Ctrl 
    ''' key is held down or "on" - multiple selection is always on.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("This controls multiple selection. Can be either false - multiple    selection is off, ""ctrl"" - multiple selection is on when the Ctrl    key is held down or ""on"" - multiple selection is always on.")> _
    Public Property MultiSelection() As MultiSelectStyles
        Get
            Return _multiStyle
        End Get
        Set(ByVal value As MultiSelectStyles)
            _multiStyle = value
        End Set
    End Property


    Private _multiTreeEnabled As Boolean = False

    ''' <summary>
    ''' If set to true all trees can have drop to this tree.
    ''' If specific trees are desired use MultiTreeString
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("If set to true all trees can have drop to this tree.   If specific trees are desired use MultiTreeString")> _
    Public Property MultiTreeEnabled() As Boolean
        Get
            Return _multiTreeEnabled
        End Get
        Set(ByVal value As Boolean)
            _multiTreeEnabled = value
            If _multiTreeEnabled Then
                MultiTreeString = "all"
            Else
                MultiTreeString = "none"
            End If
        End Set
    End Property


    Private _multiTreeString As String = """none"""

    ''' <summary>
    ''' will accept from any tree or javascript Array - a javascript 
    ''' array of strings, each one representing the ID of the container
    ''' of a tree to accept from
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("will accept from any tree or javascript Array - a javascript    array of strings, each one representing the ID of the container   of a tree to accept from")> _
    Public Property MultiTreeString() As String
        Get
            Return _multiTreeString.Replace("""", "")
        End Get
        Set(ByVal value As String)
            If value.IndexOf("[") = -1 Then
                value = """" & value & """"
            End If
            _multiTreeString = value
        End Set
    End Property

    Private _checkChildren As Boolean = False

    ''' <summary>
    ''' Propagates check to children
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Propagates check to children")> _
    Public Property CheckChildren() As Boolean
        Get
            Return _checkChildren
        End Get
        Set(ByVal value As Boolean)
            _checkChildren = value
        End Set
    End Property

    Private _themeName As String = "default"
    Public Property ThemeName() As String
        Get
            'ensure checkbox themes for checkbox enabling
            If CheckStyle <> None AndAlso _themeName.IndexOf("checkbox") < 0 Then
                Return "checkbox"
            End If
            'if the style is checkbox and we don't have checkboxes enabled
            If CheckStyle = None AndAlso _themeName.IndexOf("checkbox") > -1 Then
                Return "default"
            End If
            Return _themeName
        End Get
        Set(ByVal value As String)
            _themeName = value
        End Set
    End Property

    Private _themePath As String = BaseClasses.Scripts.ScriptsURL & "DTIMiniControls/themes"
    Public Property ThemePath() As String
        Get
            Return _themePath
        End Get
        Set(ByVal value As String)
            _themePath = value
        End Set
    End Property

    Public Property EmbeddedThemePath() As String
        Get
            Return _themePath
        End Get
        Set(ByVal value As String)
            _themePath = BaseClasses.Scripts.ScriptsURL & value
        End Set
    End Property

    Private _showContextMenu As Boolean = True

    ''' <summary>
    ''' Sets the visiblility of the context menu
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Sets the visiblility of the context menu")> _
    Public Property ShowContextMenu() As Boolean
        Get
            Return _showContextMenu
        End Get
        Set(ByVal value As Boolean)
            _showContextMenu = value
        End Set
    End Property

    Private _newNodeText As String = "New Node"
    Public Property NewNodeText() As String
        Get
            Return _newNodeText
        End Get
        Set(ByVal value As String)
            _newNodeText = value
        End Set
    End Property

    Private _emptyNodeText As String = "Tree Empty"
    Public Property EmptyNodeText() As String
        Get
            Return _emptyNodeText
        End Get
        Set(ByVal value As String)
            _emptyNodeText = value
        End Set
    End Property

    Private _createMenuText As String = "Create"
    Public Property CreateMenuText() As String
        Get
            Return _createMenuText
        End Get
        Set(ByVal value As String)
            _createMenuText = value
        End Set
    End Property

    Private _renameMenuText As String = "Rename"
    Public Property RenameMenuText() As String
        Get
            Return _renameMenuText
        End Get
        Set(ByVal value As String)
            _renameMenuText = value
        End Set
    End Property

    Private _deleteMenuText As String = "Delete"
    Public Property DeleteMenuText() As String
        Get
            Return _deleteMenuText
        End Get
        Set(ByVal value As String)
            _deleteMenuText = value
        End Set
    End Property

    Private _validChildren As String = ""

    ''' <summary>
    ''' Types of nodes allowed as root nodes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Can be an Array of string values (one for each allowed type), 
    ''' if set to String "all", nodes of any type can be root nodes.</remarks>
    <Description("Types of nodes allowed as root nodes.")> _
    Public Property ValidChildren() As String
        Get
            Return _validChildren
        End Get
        Set(ByVal value As String)
            _validChildren = value
        End Set
    End Property

    Private _maxChildDepth As Integer = -1

    ''' <summary>
    ''' Gets/Sets the maximum number of children a node can have.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Gets/Sets the maximum number of children a node can have.")> _
    Public Property MaxChildDepth() As Integer
        Get
            Return _maxChildDepth
        End Get
        Set(ByVal value As Integer)
            _maxChildDepth = value
        End Set
    End Property

    Private _maxChildDepthDefalutType As Integer = -1

    ''' <summary>
    ''' Gets/Sets the maximum number of children a default node can have.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Gets/Sets the maximum number of children a default node can have.")> _
    Public Property MaxChildDepthDefaultType() As Integer
        Get
            Return _maxChildDepthDefalutType
        End Get
        Set(ByVal value As Integer)
            _maxChildDepthDefalutType = value
        End Set
    End Property

    Private _maxDepth As Integer = -1

    ''' <summary>
    ''' Gets/Sets the maximum depth of the tree.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Gets/Sets the maximum depth of the tree.")> _
    Public Property MaxDepth() As Integer
        Get
            Return _maxDepth
        End Get
        Set(ByVal value As Integer)
            _maxDepth = value
        End Set
    End Property

    Private _maxDepthDefaultType As Integer = -1

    ''' <summary>
    ''' Gets/Sets the maximum depth of a default node.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Gets/Sets the maximum depth of a default node.")> _
    Public Property MaxDepthDefaultType() As Integer
        Get
            Return _maxDepthDefaultType
        End Get
        Set(ByVal value As Integer)
            _maxDepthDefaultType = value
        End Set
    End Property

    Private _readModeOnly As Boolean = False

    ''' <summary>
    ''' set to false if you want postback events on the tree
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("set to false if you want postback events on the tree")> _
    Public Property ReadModeOnly() As Boolean
        Get
            Return _readModeOnly
        End Get
        Set(ByVal value As Boolean)
            _readModeOnly = value
        End Set
    End Property

    Private _drag_copy As MultiSelectStyles = MultiSelectStyles.ctrl

    ''' <summary>
    ''' controls how to copy when dragging
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("controls how to copy when dragging")> _
    Public Property Drag_Copy() As MultiSelectStyles
        Get
            Return _drag_copy
        End Get
        Set(ByVal value As MultiSelectStyles)
            _drag_copy = value
        End Set
    End Property

    Private _noIntCopy As Boolean = True

    ''' <summary>
    ''' true disables copying within the same tree
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("true disables copying within the same tree")> _
    Public Property No_Int_Copy() As Boolean
        Get
            Return _noIntCopy
        End Get
        Set(ByVal value As Boolean)
            _noIntCopy = value
        End Set
    End Property

    Private _auto_insert_on_drop As Boolean = True

    ''' <summary>
    ''' True automatcally creates the node after after dropping it in
    ''' another tree.  False and the node would have to be created manually
    ''' in the ondrop callback
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("True automatcally creates the node after after dropping it in   another tree. False and the node would have to be created manually   in the ondrop callback")> _
    Public Property AutoInsertOnDrop() As Boolean
        Get
            Return _auto_insert_on_drop
        End Get
        Set(ByVal value As Boolean)
            _auto_insert_on_drop = value
        End Set
    End Property
#End Region

#Region "Callbacks"

    Private _onLoadCallBack As String

    ''' <summary>
    ''' Javascript to fire when a tree is loaded
    ''' <code>
    ''' <example>
    ''' function(TREE_OBJ) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Triggered when the tree is loaded with data for the first time or refreshed. 
    ''' Receives one parameter - a reference to the tree instance.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a tree is loaded   <code>   <example>   function(TREE_OBJ) { }   </example>   </code>")> _
    Public Property OnLoadCallBack() As String
        Get
            Return _onLoadCallBack
        End Get
        Set(ByVal value As String)
            _onLoadCallBack = value
        End Set
    End Property

    Private _onMoveCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is moved
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives five parameters - the node that was moved, the reference node in the move, 
    ''' the new position relative to the reference node (one of "before", "after" or "inside"), 
    ''' a reference to the tree instance and a rollback object that you can use with 
    ''' jQuery.tree.rollback.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is moved   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }   </example>   </code>")> _
    Public Property OnMoveCallBack() As String
        Get
            Return _onMoveCallBack
        End Get
        Set(ByVal value As String)
            _onMoveCallBack = value
        End Set
    End Property

    Private _onSelectCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is selected
    ''' <code>
    ''' <example>
    ''' function(NODE, TREE_OBJ) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives two parameters - the selected node and a reference to the tree instance.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is selected   <code>   <example>   function(NODE, TREE_OBJ) { }   </example>   </code>")> _
    Public Property OnSelectCallBack() As String
        Get
            Return _onSelectCallBack
        End Get
        Set(ByVal value As String)
            _onSelectCallBack = value
        End Set
    End Property

    Private _onCopyCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is Copied
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives five parameters - the node that was copied, the reference node in the copy, 
    ''' the new position relative to the reference node (one of "before", "after" or "inside"), 
    ''' a reference to the tree instance and a rollback object that you can use with 
    ''' jQuery.tree.rollback.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is Copied   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }   </example>   </code>")> _
    Public Property OnCopyCallBack() As String
        Get
            Return _onCopyCallBack
        End Get
        Set(ByVal value As String)
            _onCopyCallBack = value
        End Set
    End Property

    Private _onDropCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is Droped
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives four parameters - the foreign node that was dropped, 
    ''' the reference node in the move, the new position relative to the reference 
    ''' node (one of "before", "after" or "inside") and a reference to 
    ''' the tree instance.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is Droped   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ) { }   </example>   </code>")> _
    Public Property OnDropCallBack() As String
        Get
            Return _onDropCallBack
        End Get
        Set(ByVal value As String)
            _onDropCallBack = value
        End Set
    End Property

    Private _onCreateCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is created
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives five parameters - the node that was created, 
    ''' the reference node in the create operation, the new position relative to the 
    ''' reference node (one of "before", "after" or "inside"), a reference to the tree 
    ''' instance and a rollback object that you can use with jQuery.tree.rollback.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is created   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ, RB) { }   </example>   </code>")> _
    Public Property OnCreateCallBack() As String
        Get
            Return _onCreateCallBack
        End Get
        Set(ByVal value As String)
            _onCreateCallBack = value
        End Set
    End Property

    Private _onRenameCallBack As String

    ''' <summary>
    ''' Javascript to fire when a node is renamed
    ''' <code>
    ''' <example>
    ''' function(NODE, TREE_OBJ, RB) { }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives three parameters - the renamed node, 
    ''' a reference to the tree instance and a rollback object that you can use 
    ''' with jQuery.tree.rollback.</remarks>
    <System.ComponentModel.Description("Javascript to fire when a node is renamed   <code>   <example>   function(NODE, TREE_OBJ, RB) { }   </example>   </code>")> _
    Public Property OnRenameCallBack() As String
        Get
            Return _onRenameCallBack
        End Get
        Set(ByVal value As String)
            _onRenameCallBack = value
        End Set
    End Property

    Private _onSearchCallBack As String

    ''' <summary>
    ''' Javascript to fire after a search is performed and results are ready
    ''' <code>
    ''' <example>
    ''' function(NODES, TREE_OBJ) { NODES.addClass("search"); }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives two parameters - a jQuery collection of nodes matching the search 
    ''' and a reference to the tree instance.</remarks>
    <System.ComponentModel.Description("Javascript to fire after a search is performed and results are ready   <code>   <example>   function(NODES, TREE_OBJ) { NODES.addClass(""search""); }   </example>   </code>")> _
    Public Property OnSearchCallBack() As String
        Get
            Return _onSearchCallBack
        End Get
        Set(ByVal value As String)
            _onSearchCallBack = value
        End Set
    End Property

    Private _beforeMoveCallBack As String

    ''' <summary>
    ''' Javascript to fire before a node is moved
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ) { return true }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives four parameters - the node about to be moved, 
    ''' the reference node in the move, the new position relative to the 
    ''' reference node (one of "before", "after" or "inside") and a reference 
    ''' to the tree instance. If false is returned the node is not moved.</remarks>
    <System.ComponentModel.Description("Javascript to fire before a node is moved   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ) { return true }   </example>   </code>")> _
    Public Property BeforeMoveCallBack() As String
        Get
            Return _beforeMoveCallBack
        End Get
        Set(ByVal value As String)
            _beforeMoveCallBack = value
        End Set
    End Property

    Private _beforeCreateCallBack As String

    ''' <summary>
    ''' Javascript to fire before a node is created
    ''' <code>
    ''' <example>
    ''' function(NODE, REF_NODE, TYPE, TREE_OBJ) { return true }
    ''' </example>
    ''' </code>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Receives four parameters - the node about to be created, 
    ''' the reference node in the creation, the new position relative to 
    ''' the reference node (one of "before", "after" or "inside") and a 
    ''' reference to the tree instance. If false is returned the 
    ''' node is not created.</remarks>
    <System.ComponentModel.Description("Javascript to fire before a node is created   <code>   <example>   function(NODE, REF_NODE, TYPE, TREE_OBJ) { return true }   </example>   </code>")> _
    Public Property BeforeCreateCallBack() As String
        Get
            Return _beforeCreateCallBack
        End Get
        Set(ByVal value As String)
            _beforeCreateCallBack = value
        End Set
    End Property
#End Region


#End Region

#Region "Events"
    Public Event NodeInserted(ByRef node As TreeListItem)
    Public Event NodeUpdated(ByRef node As TreeListItem, ByVal newText As String)
    Public Event NodeDeleted(ByRef node As TreeListItem)
    Public Event NodeChildrenReOrdered(ByRef node As TreeListItem)
    Public Event NodeDropped(ByRef node As TreeListItem, ByRef originalTree As TreeList)
    Public Event TreeReOrdered(ByRef newTree As TreeListItem())
    Public Event NodeBound(ByRef node As TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean)
    Public Event NodeTypeChanged(ByRef node As TreeListItem)
#End Region

    Private Sub TreeList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.RegisterJQuery(Page, "1.3.2")
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.tree.js", , True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.tree.checkbox.js", , True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.tree.contextmenu.js", , True)
        'this happens after load of the page i.e. all nodes exist now
        If Me.Page.IsPostBack AndAlso Not ReadModeOnly Then
            Dim results As String() = "[]".Split(New String() {"],["}, StringSplitOptions.RemoveEmptyEntries)
            'create new tree from javascript array
            Dim tmpTreeFromJavacript As TreeList = treeFromJavascript()
            'cycle through each node in the dataString and make the TreeListItem from it.
            Dim tmpNodes As TreeListItem() = tmpTreeFromJavacript.Nodes
            'check for new nodes
            Dim throwReOrder As Boolean = False
            Dim changeMade As Boolean = False
            For Each node As TreeListItem In tmpNodes
                If searchForNode(Me.Nodes(), node) Is Nothing AndAlso Not node.Dropped Then
                    throwReOrder = True
                    changeMade = True
                    RaiseEvent NodeInserted(node)
                End If
            Next
            'check for dropped nodes
            For Each node As TreeListItem In tmpNodes
                If node.Dropped Then
                    throwReOrder = True
                    changeMade = True
                    Dim foundTree As TreeList = findTree(node.ID)
                    RaiseEvent NodeDropped(node, foundTree)
                End If
            Next
            'check for updated nodes
            For Each node As TreeListItem In tmpNodes
                Dim tmp As TreeListItem = searchForNode(Me.Nodes(), node)
                If tmp IsNot Nothing Then
                    If tmp.Text <> node.Text Then
                        changeMade = True
                        RaiseEvent NodeUpdated(tmp, node.Text)
                    End If
                    'check for type change
                    If node.NodeType <> "EmptyTreeHolder" AndAlso tmp.NodeType <> node.NodeType Then
                        RaiseEvent NodeTypeChanged(node)
                    End If
                End If
            Next
            'check for deleted nodes
            For Each node As TreeListItem In Me.Nodes()
                If node.NodeType <> "EmptyTreeHolder" Then
                    If searchForNode(tmpNodes, node) Is Nothing Then
                        changeMade = True
                        RaiseEvent NodeDeleted(node)
                    End If
                End If
            Next
            'check for reOrdered nodes
            If throwReOrder Then
                RaiseEvent TreeReOrdered(tmpNodes)
            Else
                'examine flattened trees for reorder
                Dim i As Integer = 0
                For i = 0 To tmpNodes.Length - 1
                    If tmpNodes(i).Value <> Me.Nodes(i).Value OrElse _
                        ((tmpNodes(i).ParentNode IsNot Nothing AndAlso Me.Nodes(i).ParentNode IsNot Nothing) _
                        AndAlso Not tmpNodes(i).ParentNode.Equals(Me.Nodes(i).ParentNode) _
                        OrElse (tmpNodes(i).ParentNode IsNot Nothing AndAlso Me.Nodes(i).ParentNode Is Nothing OrElse _
                            tmpNodes(i).ParentNode Is Nothing AndAlso Me.Nodes(i).ParentNode IsNot Nothing)) Then
                        changeMade = True
                        RaiseEvent TreeReOrdered(tmpNodes)
                        Exit For
                    End If
                Next
            End If
            If changeMade AndAlso dt IsNot Nothing Then
                DataBind()
            End If
        End If
        litCreate.Text = CreateMenuText
        litRename.Text = RenameMenuText
        litDelete.Text = DeleteMenuText
        litNewNode.Text = NewNodeText
        litEmptyNode.Text = EmptyNodeText
    End Sub

    Public Shadows Sub DataBind()
        'remove my treelistitems
        For Each _ctrl As Control In Controls
            If TypeOf _ctrl Is TreeListUL Then
                _ctrl.Controls.Clear()
            End If
        Next
        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            'get the roots
            Dim roots As DataView
            If SortColumnName <> "" Then
                roots = New DataView(dv.ToTable, ParentIdColumnName & " is null", SortColumnName, DataViewRowState.CurrentRows)
            Else
                roots = New DataView(dv.ToTable, ParentIdColumnName & " is null", "", DataViewRowState.CurrentRows)
            End If
            For Each root As DataRowView In roots
                Dim parent As TreeListItem
                If _NodeTypeColumnName IsNot Nothing Then
                    parent = New TreeListItem(root(TextColumnName), root(IdColumnName), root(NodeTypeColumnName))
                Else
                    parent = New TreeListItem(root(TextColumnName), root(IdColumnName))
                End If
                addListItemRoot(parent)
                addChildren(root, parent, True)
            Next
        Else
            addListItemRoot(New TreeListItem(EmptyNodeText, -1, "EmptyTreeHolder"))
        End If
    End Sub

    Private Sub addChildren(ByRef dr As DataRowView, ByRef node As TreeListItem, ByVal root As Boolean)
        Dim babies As DataView
        If SortColumnName <> "" Then
            babies = New DataView(dr.DataView.Table, ParentIdColumnName & " = " & dr(IdColumnName), SortColumnName, DataViewRowState.CurrentRows)
        Else
            babies = New DataView(dr.DataView.Table, ParentIdColumnName & " = " & dr(IdColumnName), "", DataViewRowState.CurrentRows)
        End If
        If babies.Count > 0 Then
            node.Expanded = True
            RaiseEvent NodeBound(node, root, True)
        Else
            RaiseEvent NodeBound(node, root, False)
        End If
        For Each baby As DataRowView In babies
            Dim babyItem As TreeListItem
            If _NodeTypeColumnName IsNot Nothing Then
                babyItem = New TreeListItem(baby(TextColumnName), baby(IdColumnName), baby(NodeTypeColumnName))
            Else
                babyItem = New TreeListItem(baby(TextColumnName), baby(IdColumnName))
            End If
            node.addItem(babyItem)
            addChildren(baby, babyItem, False)
        Next
    End Sub

    Private Function searchForNode(ByRef nodeArray As TreeListItem(), ByRef nodeToSearchFor As TreeListItem) As TreeListItem
        For Each node As TreeListItem In nodeArray
            If node.Value = nodeToSearchFor.Value Then
                Return node
            End If
        Next
        Return Nothing
    End Function

    Private Function treeFromJavascript() As TreeList
        Dim tmp As New TreeList
        Dim lastInsertedNode As TreeListItem = Nothing
        If HiddenDataString.Length > 2 Then
            For Each NodeString As String In HiddenDataString.Substring(1, HiddenDataString.Length - 2).Split(New String() {"], ["}, StringSplitOptions.RemoveEmptyEntries)
                Dim nodeProperties As String() = NodeString.Split(",")
                Dim node As New TreeListItem
                node.Text = nodeProperties(ItemFieldIndex.Text)
                node.ID = nodeProperties(ItemFieldIndex.Id)
                If Not node.ID.IndexOf(Me.ClientID) > -1 Then
                    node.Dropped = True
                End If
                node.Value = nodeProperties(ItemFieldIndex.Value)
                node.CheckStyle = parseNodeCheckedStyle(nodeProperties(ItemFieldIndex.CheckState))
                node.Expanded = Boolean.Parse(nodeProperties(ItemFieldIndex.Expanded))
                node.Selected = Boolean.Parse(nodeProperties(ItemFieldIndex.Selected))
                node.NodeType = nodeProperties(ItemFieldIndex.NodeType)
                If lastInsertedNode IsNot Nothing Then
                    If nodeProperties(ItemFieldIndex.ParentId) <> "" Then
                        addMeToMyParent(lastInsertedNode, node, nodeProperties(ItemFieldIndex.ParentId))
                    Else
                        tmp.addListItemRoot(node)
                    End If
                Else
                    tmp.addListItemRoot(node)
                End If
                lastInsertedNode = node
            Next
        End If

        Return tmp
    End Function

    Private Sub addMeToMyParent(ByRef lastNode As TreeListItem, ByRef insert As TreeListItem, ByVal ParentId As String)
        If lastNode.ID = ParentId Then
            lastNode.addItem(insert)
        Else
            If lastNode.Parent.Parent IsNot Nothing AndAlso TypeOf lastNode.Parent.Parent Is TreeListItem Then
                addMeToMyParent(lastNode.Parent.Parent, insert, ParentId)
            End If
        End If
    End Sub

    Private Function findMyParentWithId(ByVal IdToSearchFor As String, ByRef Node As TreeListItem) As TreeListItem
        If Node.ID = IdToSearchFor Then Return Node
        If TypeOf Node.Parent Is TreeListItem Then
            If CType(Node.Parent, TreeListItem).ID = IdToSearchFor Then
                Return Node.Parent
                'Else : Return findMyParentWithId(IdToSearchFor, Node.Parent)
            End If
        End If
        Return Nothing
    End Function

    Public Shadows Sub addListItem(ByVal text As String, ByVal value As String, Optional ByVal nodeType As String = Nothing, Optional ByVal className As String = Nothing)
        MyFirstUL.addListItem(text, value, nodeType, className)
    End Sub

    Public Shadows Sub addListItemRoot(ByRef li As TreeListItem)
        MyFirstUL.addListItemRoot(li)
    End Sub

    Public Sub addOpenNodeId(ByVal id As String)
        If openNodes <> "" Then
            openNodes &= ", """ & id & """"
        Else
            openNodes &= """" & id & """"
        End If
    End Sub

    Private Sub ClearNodes()
        For Each ctrl As Object In Controls
            If TypeOf ctrl Is TreeListItem Then
                Controls.Remove(ctrl)
            End If
        Next
    End Sub

    Private Function parseNodeCheckedStyle(ByVal style As String) As CheckBoxStyles
        Select Case style
            Case "undetermined"
                Return Trinary
            Case "checked"
                Return Binary
            Case Else
                Return None
        End Select
    End Function

#Region "Get Nodes Functions"
    Public Function GetMyNodes(Optional ByRef node As TreeListItem = Nothing) As TreeListItem()
        Dim items As New ArrayList
        For Each _ctrl As Control In Controls
            If TypeOf _ctrl Is TreeListUL Then
                If node Is Nothing Then
                    For Each ctrl As Object In _ctrl.Controls
                        If TypeOf ctrl Is TreeListItem Then
                            items.Add(ctrl)
                            GetMyNodes(ctrl, items)
                        End If
                    Next
                Else
                    For Each ctrl As Object In node.Controls(0).Controls
                        If TypeOf ctrl Is TreeListItem Then
                            items.Add(ctrl)
                            GetMyNodes(ctrl, items)
                        End If
                    Next
                End If
            End If
        Next
        Return items.ToArray(GetType(TreeListItem))
    End Function

    Private Sub GetMyNodes(ByRef node As TreeListItem, ByRef items As ArrayList)
        If node.Controls.Count > 0 Then
            For Each ctrl As Object In node.Controls(0).Controls
                If TypeOf ctrl Is TreeListItem Then
                    items.Add(ctrl)
                    GetMyNodes(ctrl, items)
                End If
            Next
        End If
    End Sub

    Public Function GetMyCheckedNodes(Optional ByRef node As TreeListItem = Nothing) As TreeListItem()
        Dim items As New ArrayList
        If node Is Nothing Then
            For Each ctrl As Object In Controls(0).Controls
                If TypeOf ctrl Is TreeListItem Then
                    If CType(ctrl, TreeListItem).CheckStyle <> None Then
                        items.Add(ctrl)
                    End If
                    GetMyCheckedNodes(ctrl, items)
                End If
            Next
        Else
            GetMyCheckedNodes(node, items)
        End If
        Return items.ToArray(GetType(TreeListItem))
    End Function

    Private Sub GetMyCheckedNodes(ByRef node As TreeListItem, ByRef items As ArrayList)
        If node.Controls.Count > 0 Then
            For Each ctrl As Object In node.Controls(0).Controls
                If TypeOf ctrl Is TreeListItem Then
                    If CType(ctrl, TreeListItem).CheckStyle <> None Then
                        items.Add(ctrl)
                    End If
                    GetMyCheckedNodes(ctrl, items)
                End If
            Next
        End If
    End Sub

    Public Function GetMySelectedNodes(Optional ByRef node As TreeListItem = Nothing) As TreeListItem()
        Dim items As New ArrayList
        If node Is Nothing Then
            For Each ctrl As Object In Controls(0).Controls
                If TypeOf ctrl Is TreeListItem Then
                    If CType(ctrl, TreeListItem).Selected Then
                        items.Add(ctrl)
                    End If
                    GetMySelectedNodes(ctrl, items)
                End If
            Next
        Else
            GetMySelectedNodes(node, items)
        End If
        Return items.ToArray(GetType(TreeListItem))
    End Function

    Private Sub GetMySelectedNodes(ByRef node As TreeListItem, ByRef items As ArrayList)
        If node.Controls.Count > 0 Then
            For Each ctrl As Object In node.Controls(0).Controls
                If TypeOf ctrl Is TreeListItem Then
                    If CType(ctrl, TreeListItem).Selected Then
                        items.Add(ctrl)
                    End If
                    GetMySelectedNodes(ctrl, items)
                End If
            Next
        End If
    End Sub
#End Region

#Region "Render and javascript"
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_hidden"" id=""" & Me.ClientID & "_hidden"" />")
        writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_deleted"" id=""" & Me.ClientID & "_deleted"" />")
        writer.Write(myInsertScript)
    End Sub

    Private Function myInsertScript() As String
        Dim str As String = "<script type=""text/javascript"">" & vbCrLf
        str &= "   $(function () { " & vbCrLf
        str &= "		$(""#" & Me.ClientID & """).tree({" & vbCrLf
        str &= "			opened : [" & openNodes & "], " & vbCrLf
        str &= "			ui : {" & vbCrLf
        str &= "				theme_name : """ & ThemeName & """," & vbCrLf
        str &= "				theme_path : """ & ThemePath & """" & vbCrLf
        str &= "			}," & vbCrLf
        str &= "			plugins : { " & vbCrLf
        Select Case CheckStyle
            Case None
            Case Binary
                str &= "			    checkbox : { }," & vbCrLf
            Case Trinary
                If CheckChildren Then
                    str &= "			    checkbox : {three_state : true}," & vbCrLf
                Else
                    str &= "				checkbox : {three_state : true, check_children : true}," & vbCrLf
                End If
            Case Else
        End Select
        If ShowContextMenu Then
            str &= "			    contextmenu : { " & vbCrLf
            str &= "				    items : { " & vbCrLf
            For Each contxtMen As ContextMenuItem In CustomContextMenuCollection
                str &= contxtMen.renderMenuItem
            Next
            str &= "				        create:{ label : """ & litCreate.Text & """}," & vbCrLf
            str &= "				        rename:{ label : """ & litRename.Text & """}," & vbCrLf
            str &= "				        remove:{ label : """ & litDelete.Text & """}" & vbCrLf
            str &= "				    }" & vbCrLf
            str &= "				}" & vbCrLf
        End If
        str &= "			}," & vbCrLf
        str &= "			rules : {" & vbCrLf
        str &= "				no_int_copy : " & No_Int_Copy.ToString.ToLower & "," & vbCrLf
        If Not AutoInsertOnDrop Then
            str &= "				auto_insert_on_drop : false," & vbCrLf
        End If
        If MaxChildDepth <= 0 Then
            str &= "				use_max_children : false," & vbCrLf
        Else
            str &= "				max_children : " & MaxDepth & "," & vbCrLf
        End If
        If MaxDepth <= 0 Then
            str &= "				use_max_depth : false," & vbCrLf
        Else
            str &= "				max_depth : " & MaxDepth & "," & vbCrLf
        End If
        If ValidChildren <> "" Then
            str &= "				valid_children : " & ValidChildren & "," & vbCrLf
        End If
        Select Case MultiSelection
            Case MultiSelectStyles.ctrl
                str &= "				multiple : ""ctrl""," & vbCrLf
            Case MultiSelectStyles.false
                str &= "				multiple : false," & vbCrLf
            Case MultiSelectStyles.on
                str &= "				multiple : ""on""," & vbCrLf
        End Select
        Select Case Drag_Copy
            Case MultiSelectStyles.ctrl
                str &= "				drag_copy : ""ctrl""," & vbCrLf
            Case MultiSelectStyles.false
                str &= "				drag_copy : false," & vbCrLf
            Case MultiSelectStyles.on
                str &= "				drag_copy : ""on""," & vbCrLf
        End Select
        str &= "				multitree :  " & _multiTreeString & "," & vbCrLf
        str &= "				new_node_name : """ & Me.ClientID & "_New""" & vbCrLf
        str &= "			}," & vbCrLf
        Dim callbacks As String = ""
        If OnLoadCallBack <> "" Then
            callbacks &= "              onload : " & OnLoadCallBack & "," & vbCrLf
        End If
        If OnSelectCallBack <> "" Then
            callbacks &= "				onselect : " & OnSelectCallBack & "," & vbCrLf
        End If
        If OnCopyCallBack <> "" Then
            callbacks &= "				oncopy : " & OnCopyCallBack & "," & vbCrLf
        End If
        If OnDropCallBack <> "" Then
            callbacks &= "				ondrop : " & OnDropCallBack & "," & vbCrLf
        End If
        If OnCreateCallBack <> "" Then
            callbacks &= "				oncreate : " & OnCreateCallBack & "," & vbCrLf
        End If
        If OnRenameCallBack <> "" Then
            callbacks &= "				onrename : " & OnRenameCallBack & "," & vbCrLf
        End If
        If OnSearchCallBack <> "" Then
            callbacks &= "				onsearch : " & OnSearchCallBack & "," & vbCrLf
        End If
        If BeforeMoveCallBack <> "" Then
            callbacks &= "				beforemove : " & BeforeMoveCallBack & "," & vbCrLf
        End If
        If BeforeCreateCallBack <> "" Then
            callbacks &= "				beforecreate : " & BeforeCreateCallBack & "," & vbCrLf
        End If
        If Not String.IsNullOrEmpty(callbacks) Then
            callbacks = callbacks.Remove(callbacks.LastIndexOf(","), 1)
            str &= "			callback: {" & vbCrLf
            str &= callbacks & vbCrLf
            str &= "			}," & vbCrLf
        End If
        str &= "			lang : {" & vbCrLf
        str &= "				new_node : """ & litNewNode.Text & """," & vbCrLf
        str &= "				tree_empty : """ & litEmptyNode.Text & """" & vbCrLf
        str &= "			}," & vbCrLf
        str &= "			types : {" & vbCrLf
        str &= "			    ""default"" : {" & vbCrLf
        If MaxChildDepthDefaultType > -1 Then
            str &= "				    max_children : " & MaxChildDepthDefaultType & "," & vbCrLf
        End If
        If MaxDepthDefaultType > -1 Then
            str &= "				    max_depth : " & MaxDepthDefaultType & "," & vbCrLf
        End If
        str &= "   				    valid_children : ""all""" & vbCrLf
        str &= "			    }," & vbCrLf
        For Each ntype As TreeNodeType In NodeTypes
            str &= "			    " & ntype.ToString & "," & vbCrLf
        Next
        str = str.Substring(0, str.LastIndexOf(","))
        str &= "			}" & vbCrLf
        str &= "		});" & vbCrLf
        str &= "	});" & vbCrLf
        str &= "</script>"
        Return str
    End Function
#End Region

    Private Function findTree(ByVal nodeName As String) As TreeList
        For Each ctrl As Control In Me.Page.Controls
            Dim ret As Control = BaseClasses.Spider.spiderControlForType(ctrl, GetType(TreeList))
            If Not ret Is Nothing AndAlso nodeName.IndexOf(ret.ID) > -1 Then
                Return ret
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Adds a custom context menu item to the tree
    ''' </summary>
    ''' <param name="label">Visible text of menu</param>
    ''' <param name="icon">you can set this to a classname or a path to an icon like ./myimage.gif or empty string</param>
    ''' <param name="visibleFunction">javascript to call to determine items visibility  return 0 or 1, If empty returns true
    ''' <example>function (node, tree) {if(tree.parent(node) != -1){return 1;}else{return 0;}} only shows up for children</example>
    ''' </param>
    ''' <param name="actionFunction">javascritpt to call when clicked function (NODE, TREE_OBJ)</param>
    ''' <param name="separatorBefore">display separation line above item</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a custom context menu item to the tree")> _
    Public Sub addCustomContextMenu(ByVal label As String, ByVal icon As String, ByVal visibleFunction As String, ByVal actionFunction As String, ByVal separatorBefore As Boolean)
        CustomContextMenuCollection.Add(New ContextMenuItem(label, icon, visibleFunction, actionFunction, separatorBefore))
    End Sub

    Private Class ContextMenuItem
        Private _label As String = ""
        Public Property Label() As String
            Get
                Return _label
            End Get
            Set(ByVal value As String)
                _label = value
            End Set
        End Property

        Private _icon As String = ""
        Public Property Icon() As String
            Get
                Return _icon
            End Get
            Set(ByVal value As String)
                _icon = value
            End Set
        End Property

        Private _visibleFunction As String = ""
        Public Property VisibleFunction() As String
            Get
                Return _visibleFunction
            End Get
            Set(ByVal value As String)
                _visibleFunction = value
            End Set
        End Property

        Private _actionFunction As String = ""
        Public Property ActionFunction() As String
            Get
                Return _actionFunction
            End Get
            Set(ByVal value As String)
                _actionFunction = value
            End Set
        End Property

        Private _separatorBefore As Boolean = False
        Public Property SeparatorBefore() As Boolean
            Get
                Return _separatorBefore
            End Get
            Set(ByVal value As Boolean)
                _separatorBefore = value
            End Set
        End Property

        Public Sub New(ByVal lbl As String, ByVal icn As String, ByVal vFunct As String, ByVal aFunct As String, Optional ByVal sepBefore As Boolean = False)
            Label = lbl
            Icon = icn
            VisibleFunction = vFunct
            ActionFunction = aFunct
            SeparatorBefore = sepBefore
        End Sub

        Public Sub New()

        End Sub

        Public Function renderMenuItem()
            Dim str As String = ""
            If Label <> "" AndAlso Not String.IsNullOrEmpty(ActionFunction) Then
                Dim lbl As String = Regex.Replace(Label, "[^0-9a-zA-Z_\-]+?", "")
                Dim vContext As String = ""
                If String.IsNullOrEmpty(VisibleFunction) Then
                    vContext = "function (NODE, TREE_OBJ) {return 1;}"
                Else
                    vContext = VisibleFunction
                End If
                str &= "				        " & lbl & ": { " & vbCrLf
                str &= "				            label	: """ & New LiteralControl(Label).Text & """,  " & vbCrLf
                str &= "						    icon	: """ & New LiteralControl(Icon).Text & """," & vbCrLf
                str &= "						    visible	: " & New LiteralControl(vContext).Text & "," & vbCrLf
                str &= "						    action	: " & New LiteralControl(ActionFunction).Text & "," & vbCrLf
                str &= "						    separator_before : " & SeparatorBefore.ToString.ToLower & vbCrLf
                str &= "				        }, " & vbCrLf
            End If
            Return str
        End Function
    End Class
End Class