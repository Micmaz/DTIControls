Public Class TreeList
    Inherits Panel

    Private _enableDragNDrop As Boolean = True
    Public Property EnableDragNDrop() As Boolean
        Get
            Return _enableDragNDrop
        End Get
        Set(ByVal value As Boolean)
            _enableDragNDrop = value
        End Set
    End Property

    Private _enableCRRM As Boolean = True
    Public Property EnableCRRM() As Boolean
        Get
            Return _enableCRRM
        End Get
        Set(ByVal value As Boolean)
            _enableCRRM = value
        End Set
    End Property

    Private _enableSelecting As Boolean = False
    Public Property EnableSelecting() As Boolean
        Get
            Return _enableSelecting
        End Get
        Set(ByVal value As Boolean)
            _enableSelecting = value
        End Set
    End Property

    Private _items As New TreeListItemsCollection
    Public ReadOnly Property Items() As TreeListItemsCollection
        Get
            Return _items
        End Get
    End Property

    Private _themeName As String = "default"
    Public Property ThemeName() As String
        Get
            'ensure checkbox themes for checkbox enabling
            'If CheckStyle <> None AndAlso _themeName.IndexOf("checkbox") < 0 Then
            '    Return "checkbox"
            'End If
            ''if the style is checkbox and we don't have checkboxes enabled
            'If CheckStyle = None AndAlso _themeName.IndexOf("checkbox") > -1 Then
            '    Return "default"
            'End If
            Return _themeName
        End Get
        Set(ByVal value As String)
            _themeName = value
        End Set
    End Property

    Private _dropTarget As Boolean = True
    Public Property DropTarget() As Boolean
        Get
            Return _dropTarget
        End Get
        Set(ByVal value As Boolean)
            _dropTarget = value
        End Set
    End Property

    Private _dragTarget As Boolean = True
    Public Property DragTarget() As Boolean
        Get
            Return _dragTarget
        End Get
        Set(ByVal value As Boolean)
            _dragTarget = value
        End Set
    End Property

    Private _nodes As TreeListItem() = Nothing
    Private ReadOnly Property Nodes() As TreeListItem()
        Get
            If _nodes Is Nothing Then
                Dim itms As ArrayList = New ArrayList
                For Each i As TreeListItem In Me.Items
                    FlattenTree(itms, i)
                Next
                _nodes = itms.ToArray(GetType(TreeListItem))
            End If
            Return _nodes
        End Get
    End Property

    Private Enum ItemFieldIndex
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

    Private _hiddenString As String
    Private Property HiddenDataString() As String
        Get
            If _hiddenString Is Nothing Then
                _hiddenString = Me.Page.Request.Params(Me.ClientID & "_hidden")
            End If
            Return _hiddenString
        End Get
        Set(ByVal value As String)
            _hiddenString = value
        End Set
    End Property

#Region "Javascript callbacks"
    Private _onItemMove As String
    Public Property onItemMove() As String
        Get
            Return _onItemMove
        End Get
        Set(ByVal value As String)
            _onItemMove = value
        End Set
    End Property

    Private _onTreeLoad As String
    Public Property onTreeLoad() As String
        Get
            Return _onTreeLoad
        End Get
        Set(ByVal value As String)
            _onTreeLoad = value
        End Set
    End Property
#End Region

#Region "Data Properties"

    Private _DataValueField As String = "Id"
    Public Property DataValueField() As String
        Get
            Return _DataValueField
        End Get
        Set(ByVal value As String)
            _DataValueField = value
        End Set
    End Property

    Private _DataTextField As String = ""
    Public Property DataTextField() As String
        Get
            Return _DataTextField
        End Get
        Set(ByVal value As String)
            _DataTextField = value
        End Set
    End Property

    Private _DataParentValueField As String = "ParentId"
    Public Property DataParentValueField() As String
        Get
            Return _DataParentValueField
        End Get
        Set(ByVal value As String)
            _DataParentValueField = value
        End Set
    End Property

    Private _DataItemTypeField As String
    Public Property DataItemTypeField() As String
        Get
            Return _DataItemTypeField
        End Get
        Set(ByVal value As String)
            _DataItemTypeField = value
        End Set
    End Property

    Private _DataSortValueField As String
    Public Property DataSortValueField() As String
        Get
            Return _DataSortValueField
        End Get
        Set(ByVal value As String)
            _DataSortValueField = value
        End Set
    End Property

    Private _dataMember As String = ""
    Public Property DataMember() As String
        Get
            Return _dataMember
        End Get
        Set(ByVal value As String)
            _dataMember = value
        End Set
    End Property

    Private _datasource As Object
    Public Property DataSource() As Object
        Get
            Return _datasource
        End Get
        Set(ByVal value As Object)
            _datasource = value
        End Set
    End Property

    Private ReadOnly Property dv() As DataView
        Get
            If _datasource Is Nothing OrElse Not TypeOf _datasource Is DataView Then
                If TypeOf _datasource Is DataTable Then
                    _datasource = CType(_datasource, DataTable).DefaultView
                ElseIf TypeOf _datasource Is DataSet AndAlso DataMember <> "" Then
                    Dim ds As DataSet = CType(_datasource, DataSet)
                    _datasource = ds.Tables(DataMember).DefaultView
                ElseIf TypeOf _datasource Is DataView Then
                    _datasource = CType(_datasource, DataView)
                End If
            End If
            Return _datasource
        End Get
    End Property
#End Region

#Region "Events"
    Public Event ItemInserted(ByRef Item As TreeListItem)
    Public Event ItemUpdated(ByRef Item As TreeListItem, ByVal newText As String)
    Public Event ItemDeleted(ByRef Item As TreeListItem)
    Public Event NodeChildrenReOrdered(ByRef Item As TreeListItem)
    Public Event ItemDropped(ByRef Item As TreeListItem, ByRef originalTree As TreeList)
    Public Event TreeReOrdered(ByRef NewTree As TreeListItem())
    Public Event ItemBound(ByRef Item As TreeListItem)
    Public Event ItemTypeChanged(ByRef Item As TreeListItem)
#End Region

    Public Sub New()

    End Sub

    Private Sub TreeList_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "TreeList/jquery.jstree.js")
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "TreeList/TreeList.js")
    End Sub

    Private Sub TreeList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            Dim clientTree As TreeList = treeFromJavascript()
            'check for new nodes
            Dim throwReOrder As Boolean = False
            Dim changeMade As Boolean = False
            For Each node As TreeListItem In clientTree.Nodes
                If searchForNode(Me.Nodes, node) Is Nothing AndAlso Not node.Dropped Then
                    throwReOrder = True
                    changeMade = True
                    RaiseEvent ItemInserted(node)
                End If
            Next
            'check for dropped nodes
            For Each node As TreeListItem In clientTree.Nodes
                If node.Dropped Then
                    throwReOrder = True
                    changeMade = True
                    Dim foundTree As TreeList = findTree(node.ID.Substring(0, node.ID.LastIndexOf("_")))
                    RaiseEvent ItemDropped(node, foundTree)
                End If
            Next
            'check for updated nodes
            For Each node As TreeListItem In clientTree.Nodes
                Dim tmp As TreeListItem = searchForNode(Me.Nodes, node)
                If tmp IsNot Nothing Then
                    If tmp.Text <> node.Text Then
                        changeMade = True
                        RaiseEvent ItemUpdated(tmp, node.Text)
                    End If
                    'check for type change
                    If node.ItemType <> "EmptyTreeHolder" AndAlso tmp.ItemType <> node.ItemType Then
                        RaiseEvent ItemTypeChanged(node)
                    End If
                End If
            Next
            'check for deleted nodes
            For Each node As TreeListItem In Me.Nodes()
                If node.ItemType <> "EmptyTreeHolder" Then
                    If searchForNode(clientTree.Nodes, node) Is Nothing Then
                        changeMade = True
                        RaiseEvent ItemDeleted(node)
                    End If
                End If
            Next
            'check for reOrdered nodes
            If throwReOrder Then
                RaiseEvent TreeReOrdered(clientTree.Nodes)
            Else
                'examine flattened trees for reorder
                Dim i As Integer = 0
                For i = 0 To clientTree.Nodes.Length - 1
                    If clientTree.Nodes(i).Value <> Me.Nodes(i).Value OrElse _
                        clientTree.Nodes(i).ParentValue <> Me.Nodes(i).ParentValue Then
                        changeMade = True
                        RaiseEvent TreeReOrdered(clientTree.Nodes)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Public Shadows Sub DataBind()
        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            Me.Items.Clear()
            'get the roots
            dv.RowFilter = DataParentValueField & " is null"
            If DataSortValueField <> "" Then
                dv.Sort = datasortvaluefield
            End If

            For Each root As DataRowView In dv
                Dim parent As TreeListItem
                If DataItemTypeField IsNot Nothing Then
                    parent = New TreeListItem(root(DataValueField), root(DataTextField), root(DataItemTypeField))
                Else
                    parent = New TreeListItem(root(DataValueField), root(DataTextField))
                End If
                parent.ParentValue = -1
                RaiseEvent ItemBound(parent)
                Me.Items.Add(parent)
                addChildren(root, parent)
            Next
        End If
    End Sub

    Private Sub addChildren(ByRef dr As DataRowView, ByRef node As TreeListItem)
        Dim babies As DataView
        If DataSortValueField <> "" Then
            babies = New DataView(dr.DataView.Table, DataParentValueField & " = " & dr(DataValueField), DataSortValueField, dv.RowStateFilter)
        Else
            babies = New DataView(dr.DataView.Table, DataParentValueField & " = " & dr(DataValueField), "", dv.RowStateFilter)
        End If

        For Each baby As DataRowView In babies
            Dim babyItem As TreeListItem
            If DataItemTypeField IsNot Nothing Then
                babyItem = New TreeListItem(baby(DataValueField), baby(DataTextField), baby(DataItemTypeField))
            Else
                babyItem = New TreeListItem(baby(DataValueField), baby(DataTextField))
            End If
            babyItem.ParentValue = node.Value

            RaiseEvent ItemBound(babyItem)
            node.Items.Add(babyItem)
            addChildren(baby, babyItem)
        Next
    End Sub

    Public Function RenderTree()
        Dim s As String = ""
        s &= "<ul>" & vbCrLf
        For Each itm As TreeListItem In Items
            s &= renderItems(itm)
        Next
        s &= "</ul>" & vbCrLf
        Return s
    End Function

    Private Function renderItems(ByRef ti As TreeListItem, Optional ByRef parent As TreeListItem = Nothing, Optional ByRef depth As Integer = 0) As String
        Dim s As String = ""
        s &= "<li>" & vbCrLf
        s &= "<a href=""#"" id=""" & Me.ClientID & "_" & ti.Value & """ parent=""" & ti.ParentValue & """ key=""" & ti.Value & """>" & ti.Text & "</a>" & vbCrLf
        If ti.Items.Count > 0 Then
            s &= "<ul>" & vbCrLf
            For Each itm As TreeListItem In ti.Items
                s &= renderItems(itm, ti)
            Next
            s &= "</ul>" & vbCrLf
        End If
        s &= "</li>" & vbCrLf
        Return s
    End Function

    Private Function treeFromJavascript() As TreeList
        Dim tmp As New TreeList
        If HiddenDataString IsNot Nothing AndAlso HiddenDataString.Length > 2 Then
            HiddenDataString = HiddenDataString.Substring(1, HiddenDataString.Length - 2)
            For Each NodeString As String In HiddenDataString.Split(New String() {"]["}, StringSplitOptions.None)
                Dim nodeProperties As String() = NodeString.Split(New String() {":,"}, StringSplitOptions.None)
                Dim node As New TreeListItem
                node.Text = nodeProperties(ItemFieldIndex.Text)
                node.ID = nodeProperties(ItemFieldIndex.Id)
                If Not node.ID.IndexOf(Me.ClientID) > -1 Then
                    node.Dropped = True
                End If
                node.Value = nodeProperties(ItemFieldIndex.Value)
                'node.CheckStyle = parseNodeCheckedStyle(nodeProperties(ItemFieldIndex.CheckState))
                node.Expanded = Boolean.Parse(nodeProperties(ItemFieldIndex.Expanded))
                node.Selected = Boolean.Parse(nodeProperties(ItemFieldIndex.Selected))
                node.ItemType = nodeProperties(ItemFieldIndex.NodeType)
                node.ParentValue = Integer.Parse(nodeProperties(ItemFieldIndex.ParentId))

                If node.ParentValue = -1 Then
                    tmp.Items.Add(node)
                Else
                    Dim parentNode As TreeListItem = tmp.Items.Find(node.ParentValue)
                    If parentNode IsNot Nothing Then
                        parentNode.Items.Add(node)
                    End If
                End If
            Next
        End If

        Return tmp
    End Function

    Private Function searchForNode(ByRef nodeArray As TreeListItem(), ByRef nodeToSearchFor As TreeListItem) As TreeListItem
        For Each node As TreeListItem In nodeArray
            If node.Value = nodeToSearchFor.Value Then
                Return node
            End If
        Next
        Return Nothing
    End Function

    Private Sub FlattenTree(ByRef itms As ArrayList, Optional ByRef node As TreeListItem = Nothing)
        itms.Add(node)
        For Each i As TreeListItem In node.Items
            FlattenTree(itms, i)
        Next
    End Sub

    Private Function findTree(ByVal nodeId As String) As TreeList
        For Each ctrl As Control In Me.Page.Controls
            Dim ret As Control = BaseClasses.Spider.spidercontrolforType(ctrl, GetType(TreeList))
            If Not ret Is Nothing AndAlso nodeId.IndexOf(ret.ID) > -1 Then
                Return ret
            End If
        Next
        Return Nothing
    End Function

    Private Sub TreeList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.Controls.Add(New LiteralControl(RenderTree))
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Items.Count > 0 Then
            Dim s As String = ""
            Dim plugins As String = """themes"", ""html_data"""
            If EnableDragNDrop Then
                plugins &= ", ""dnd"""
            End If
            If EnableCRRM Then
                plugins &= ", ""crrm"""
            End If
            If EnableSelecting Then
                plugins &= ", ""ui"""
            End If

            s &= "<script type=""text/javascript"">" & vbCrLf
            s &= "  $(function () {" & vbCrLf
            s &= "      $(""#" & Me.ClientID & """)" & vbCrLf
            If onTreeLoad IsNot Nothing Then
                s &= "      .bind(""loaded.jstree"", function (event, data) {" & vbCrLf
                s &= "          var tree = $.jstree._reference('#" & Me.ClientID & "');" & vbCrLf
                s &= "          " & onTreeLoad & vbCrLf
                s &= "      })" & vbCrLf
            End If
            s &= "          .jstree({ " & vbCrLf
            s &= "              ""themes"" : {" & vbCrLf
            s &= "                  ""url"" : """ & BaseClasses.Scripts.ScriptsURL & "TreeList/themes/" & ThemeName & "/style.css""," & vbCrLf
            s &= "                  ""icons"" : false" & vbCrLf
            s &= "              }," & vbCrLf
            If writecrrm() Then
                s &= "              ""crrm"" : {" & vbCrLf
                s &= "                  ""move"" : {" & vbCrLf
                If onItemMove IsNot Nothing Then
                    s &= "                      ""check_move"" : function(m){" & vbCrLf
                    s &= "                          var tree = $.jstree._reference('#" & Me.ClientID & "');" & vbCrLf
                    s &= "                          return " & onItemMove & vbCrLf
                    s &= "                      }" & vbCrLf
                End If
                s &= "                  }" & vbCrLf
                s &= "              }," & vbCrLf
            End If
            If writednd() Then
                s &= "              ""dnd"" : {" & vbCrLf
                If Not DropTarget Then
                    s &= "                  ""drop_target"" : false," & vbCrLf
                End If
                If Not DragTarget Then
                    s &= "                  ""drag_target"" : false" & vbCrLf
                End If
                s &= "              }," & vbCrLf
            End If
            s &= "              ""plugins"" : [ " & plugins & " ]" & vbCrLf
            s &= "	        });" & vbCrLf
            s &= "  });" & vbCrLf
            s &= "</script>" & vbCrLf

            'If EnableSelecting Then
            '    s &= "<script type=""text/javascript"" class=""source"">" & vbCrLf
            '    s &= "$(function () {" & vbCrLf
            '    s &= "	$(""#" & Me.ClientID & """).bind(""select_node.jstree"", function (e, data) {" & vbCrLf
            '    s &= "		selectNodes(data,'#" & Me.ClientID & "_selected');" & vbCrLf
            '    s &= "	});" & vbCrLf
            '    s &= "});" & vbCrLf
            '    s &= "</script>" & vbCrLf
            'End If
            writer.Write(s)
            writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_hidden"" id=""" & Me.ClientID & "_hidden"" />" & vbCrLf)
            'writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_deleted"" id=""" & Me.ClientID & "_deleted"" />" & vbCrLf)
            'writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_selected"" id=""" & Me.ClientID & "_selected"" />" & vbCrLf)
            writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_treelist"" id=""" & Me.ClientID & "_treelist"" class=""treelist"" />" & vbCrLf)
            MyBase.Render(writer)
        End If
    End Sub

    Private Function writecrrm() As Boolean
        If EnableCRRM Then
            If onItemMove IsNot Nothing Then Return True
        End If
        Return False
    End Function

    Private Function writednd() As Boolean
        If Not DropTarget Then Return True
        If Not DragTarget Then Return True
        Return False
    End Function
End Class
