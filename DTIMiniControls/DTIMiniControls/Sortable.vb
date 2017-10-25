#If DEBUG Then
Public Class Sortable
    Inherits Panel
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Sortable
        Inherits Panel
#End If
        Private updateOnDrop As String = ""
        Private OriginalOrder As String = ""
        Private hfNewOrder As New HiddenField
        Private SortableList As New Panel
        Private DataSourceView As DataView

#Region "events"

        ''' <summary>
        ''' Occurs when a control has been re-ordered
        ''' </summary>
        ''' <param name="item">DTIMiniControls.SortableItem that has been re-ordered</param>
        ''' <param name="newIndex">System.Integer 0 based index of new position</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Occurs when a control has been re-ordered")> _
        Public Event ItemReOrdered(ByRef item As SortableItem, ByVal newIndex As Integer)

        ''' <summary>
        ''' Occurs when a control is bound to a datasource
        ''' </summary>
        ''' <param name="item">DTIMiniControls.SortableItem that is being bound</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Occurs when a control is bound to a datasource")> _
        Public Event DataBound(ByRef item As SortableItem)

        ''' <summary>
        ''' Occurs after all items have been successfully re-ordered
        ''' </summary>
        ''' <param name="newOrder">System.String() order of panel ids after being re-ordered</param>
        ''' <param name="oldOrder">System.String() order of panel ids before being re-ordered</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Occurs after all items have been successfully re-ordered")> _
        Public Event OrderChanged(ByVal newOrder() As String, ByVal oldOrder() As String)
#End Region

#Region "behavior properties"

        ''' <summary>
        ''' Determines is Sortable can be reordered
        ''' </summary>
        <System.ComponentModel.Description("Determines is Sortable can be reordered")> _
        Public Property AdminOn() As Boolean
            Get
                If Page.Session("IsAdminOn_" & Page.Request.RawUrl & Me.UniqueID) Is Nothing Then
                    Page.Session("IsAdminOn_" & Page.Request.RawUrl & Me.UniqueID) = False
                End If
                Return Page.Session("IsAdminOn_" & Page.Request.RawUrl & Me.UniqueID)
            End Get
            Set(ByVal value As Boolean)
                Page.Session("IsAdminOn_" & Page.Request.RawUrl & Me.UniqueID) = value
            End Set
        End Property

        Private _AxisRestriction As Axis

        ''' <summary>
        ''' Restricts Items to being moved only vertically (restrict y) or horizantlly (restrict x)
        ''' </summary>
        <System.ComponentModel.Description("Restricts Items to being moved only vertically (restrict y) or horizantlly (restrict x)")> _
        Public Property AxisRestriction() As Axis
            Get
                Return _AxisRestriction
            End Get
            Set(ByVal value As Axis)
                _AxisRestriction = value
            End Set
        End Property

        Private _ConnectByClass As Boolean

        ''' <summary>
        ''' connects multiple sortables by the same classname,
        ''' Items can be moved between sortables
        ''' </summary>
        <System.ComponentModel.Description("connects multiple sortables by the same classname,     Items can be moved between sortables")> _
        Public Property ConnectByClass() As Boolean
            Get
                Return _ConnectByClass
            End Get
            Set(ByVal value As Boolean)
                _ConnectByClass = value
            End Set
        End Property

        Private _ConnectToID As String

        ''' <summary>
        ''' Connects sortable to another specific sortable based on its ID
        ''' so items can be moved between sortables
        ''' </summary>
        <System.ComponentModel.Description("Connects sortable to another specific sortable based on its ID     so items can be moved between sortables")> _
        Public Property ConnectToID() As String
            Get
                Return _ConnectToID
            End Get
            Set(ByVal value As String)
                _ConnectToID = value
            End Set
        End Property

        Private _PlaceHolderClass As String

        ''' <summary>
        ''' Class name for showing where an item can be placed while sorting
        ''' </summary>
        <System.ComponentModel.Description("Class name for showing where an item can be placed while sorting")> _
        Public Property PlaceHolderClass() As String
            Get
                Return _PlaceHolderClass
            End Get
            Set(ByVal value As String)
                _PlaceHolderClass = value
            End Set
        End Property

        Private _cursortype As Cursor

        ''' <summary>
        ''' Determines what the cursor looks like while moving items
        ''' </summary>
        <System.ComponentModel.Description("Determines what the cursor looks like while moving items")> _
        Public Property CursorType() As Cursor
            Get
                Return _cursortype
            End Get
            Set(ByVal value As Cursor)
                _cursortype = value
            End Set
        End Property

        Private _CursorPosition As CursorAt

        ''' <summary>
        ''' determines where in the item the cursor is located when moving
        ''' </summary>
        <System.ComponentModel.Description("determines where in the item the cursor is located when moving")> _
        Public Property CursorPosition() As CursorAt
            Get
                Return _CursorPosition
            End Get
            Set(ByVal value As CursorAt)
                _CursorPosition = value
            End Set
        End Property

        Private _CursorPositionValue As Integer

        ''' <summary>
        ''' Gets or Sets the offset value of the CursorPosition Property
        ''' </summary>
        <System.ComponentModel.Description("Gets or Sets the offset value of the CursorPosition Property")> _
        Public Property CursorPositionValue() As Integer
            Get
                Return _CursorPositionValue
            End Get
            Set(ByVal value As Integer)
                _CursorPositionValue = value
            End Set
        End Property

        Private _DragOnEmpty As Boolean

        ''' <summary>
        ''' Whether or not items can be put into sortable once it is empty
        ''' </summary>
        <System.ComponentModel.Description("Whether or not items can be put into sortable once it is empty")> _
        Public Property DragOnEmpty() As Boolean
            Get
                Return _DragOnEmpty
            End Get
            Set(ByVal value As Boolean)
                _DragOnEmpty = value
            End Set
        End Property

        Private _HelperType As Helper

        ''' <summary>
        ''' Gets or sets the type of object moved, actual or a copy
        ''' </summary>
        <System.ComponentModel.Description("Gets or sets the type of object moved, actual or a copy")> _
        Public Property HelperType() As Helper
            Get
                Return _HelperType
            End Get
            Set(ByVal value As Helper)
                _HelperType = value
            End Set
        End Property

        Private _UpdatOnDrop As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether a postback to the server 
        ''' automatically occurs when the user changes the order of the list.
        ''' </summary>
        <System.ComponentModel.Description("Gets or sets a value indicating whether a postback to the server      automatically occurs when the user changes the order of the list.")> _
        Public Property AutoPostBack() As Boolean
            Get
                Return _UpdatOnDrop
            End Get
            Set(ByVal value As Boolean)
                _UpdatOnDrop = value
            End Set
        End Property
#End Region

#Region "data properties"
        Private _dataSource As Object

        ''' <summary>
        ''' Gets or sets the DataTable or DataView from which the data-bound control retrieves its list of data items.
        ''' </summary>
        ''' <value>System.Object that represents the data source from which the data-bound control retrieves its data. 
        ''' The default is Nothing.</value>
        ''' <remarks>Only System.DataTable or System.DataView will produce a valid DataSource</remarks>
        <System.ComponentModel.Description("Gets or sets the DataTable or DataView from which the data-bound control retrieves its list of data items.")> _
        Public Property DataSource() As Object
            Get
                Return _dataSource
            End Get
            Set(ByVal value As Object)
                If value IsNot Nothing Then
                    If value.GetType.BaseType Is GetType(DataTable) OrElse value.GetType Is GetType(DataTable) Then
                        DataSourceView = CType(value, DataTable).DefaultView
                    ElseIf value.GetType Is GetType(DataView) OrElse value.GetType.BaseType Is GetType(DataView) Then
                        DataSourceView = CType(value, DataView)
                    End If
                End If
                _dataSource = value
            End Set
        End Property

        Private _dataValueField As String = "Id"

        ''' <summary>
        ''' Gets or sets the field of the data source that provides the control ID of each list item
        ''' </summary>
        ''' <value>System.String that specifies the field of the data source that provides the value 
        ''' of each list item. The default is String.Empty.</value>
        <System.ComponentModel.Description("Gets or sets the field of the data source that provides the control ID of each list item")> _
        Public Property DataValueField() As String
            Get
                Return _dataValueField
            End Get
            Set(ByVal value As String)
                _dataValueField = value
            End Set
        End Property

        Private _dataTextField As String = ""

        ''' <summary>
        ''' Gets or sets the field of the data source that provides the text content of the list items.
        ''' </summary>
        ''' <value>System.String that specifies the field of the data source that provides the text 
        ''' of each list item. The default is String.Empty.</value>
        <System.ComponentModel.Description("Gets or sets the field of the data source that provides the text content of the list items.")> _
        Public Property DataTextField() As String
            Get
                Return _dataTextField
            End Get
            Set(ByVal value As String)
                _dataTextField = value
            End Set
        End Property

        Private _dataSortField As String = ""

        ''' <summary>
        ''' Gets or sets the field of the data source that provides the order of the list items.
        ''' </summary>
        ''' <value>System.String that specifies the field of the data source that provides the order 
        ''' of each list item. The default is String.Empty.</value>
        <System.ComponentModel.Description("Gets or sets the field of the data source that provides the order of the list items.")> _
        Public Property DataSortField() As String
            Get
                Return _dataSortField
            End Get
            Set(ByVal value As String)
                _dataSortField = value
            End Set
        End Property

        Private _isOrderChanged As Boolean = False

        ''' <summary>
        ''' Gets a value indicating whether the control has be re-ordered
        ''' </summary>
        ''' <value>Sytem.Boolean true if list has be re-ordered; otherwise false</value>
        <System.ComponentModel.Description("Gets a value indicating whether the control has be re-ordered")> _
        Public ReadOnly Property isOrderChanged() As Boolean
            Get
                Return _isOrderChanged
            End Get
        End Property
#End Region

#Region "enums"
        Public Enum Axis As Integer
            none = 0
            x = 1
            y = 2
        End Enum

        Public Enum Cursor As Integer
            move = 0
            pointer = 1
            crosshair = 2
        End Enum

        Public Enum Helper As Integer
            original = 0
            clone = 1
        End Enum

        Public Enum CursorAt As Integer
            [false] = 0
            top = 1
            left = 2
            right = 3
            bottom = 4
        End Enum
#End Region

        Private Sub Sortable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(Me.Page)

            SortableList.ID = Me.ClientID & "_SortableList"
            hfNewOrder.ID = Me.ClientID & "_HiddenField1"

            If AdminOn Then
                If Page.Request.Params(hfNewOrder.ClientID) <> "" And SortableList.Controls.Count > 0 Then
                    Dim newOrder() As String = Page.Request.Params(hfNewOrder.ClientID).Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                    Dim oldOrder() As String = OriginalOrder.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                    If newOrder.Length = oldOrder.Length Then
                        Dim i As Integer = 0
                        For Each id As String In newOrder
                            If id <> oldOrder.GetValue(i) Then
                                Dim newvalueIndex As Integer = Array.IndexOf(oldOrder, id)
                                _isOrderChanged = True
                                RaiseEvent ItemReOrdered(CType(SortableList.Controls(newvalueIndex), SortableItem), i)
                            End If
                            i += 1
                        Next
                        If _isOrderChanged Then
                            RaiseEvent OrderChanged(newOrder, oldOrder)
                        End If
                    End If
                End If
                If ConnectByClass = True AndAlso CssClass = "" Then
                    CssClass = "connectedSortable"
                End If
                SortableList.CssClass = CssClass
                CssClass = ""
                hfNewOrder.Value = ""
                Me.Controls.Add(hfNewOrder)
            End If

            Me.Controls.Add(SortableList)
        End Sub

        ''' <summary>
        ''' Binds a data source to the invoked server control and all its child controls.
        ''' </summary>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Binds a data source to the invoked server control and all its child controls.")> _
        Public Shadows Sub DataBind()
            SortableList.Controls.Clear()
            OriginalOrder = ""
            If DataSourceView IsNot Nothing AndAlso DataSourceView.Count > 0 Then
                Dim items As DataView
                If DataSortField <> "" Then
                    items = New DataView(DataSourceView.ToTable, "", DataSortField, DataViewRowState.CurrentRows)
                Else
                    items = New DataView(DataSourceView.ToTable, "", "", DataViewRowState.CurrentRows)
                End If
                For Each item As DataRowView In items
                    Dim lit As LiteralControl = Nothing
                    If Not String.IsNullOrEmpty(DataTextField) Then
                        lit = New LiteralControl
                        lit.Text = item(DataTextField)
                    End If
                    Dim sort As SortableItem = New SortableItem(Me.ID & "_Value_" & item(DataValueField), lit)
                    RaiseEvent DataBound(sort)
                    addSortableItem(sort)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Adds Item to end of control collection
        ''' </summary>
        ''' <param name="SortItem">DTIMiniControls.SortableItem to add to end of collection</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds Item to end of control collection")> _
        Public Sub addSortableItem(ByRef SortItem As SortableItem)
            SortableList.Controls.Add(SortItem)
            OriginalOrder &= SortItem.PanelID & ","
        End Sub

        ''' <summary>
        ''' Adds Item to end of control collection
        ''' </summary>
        ''' <param name="SortItem">System.String to add to end of collection</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds Item to end of control collection")> _
        Public Sub addSortableItem(ByVal SortItem As String)
            Dim sort As SortableItem = New SortableItem(Me.ID & "_" & SortableList.Controls.Count, New LiteralControl(SortItem))
            SortableList.Controls.Add(sort)
            OriginalOrder &= sort.PanelID & ","
        End Sub

        ''' <summary>
        ''' Adds Item to end of control collection
        ''' </summary>
        ''' <param name="SortItem">System.Web.UI.Control to add to end of collection</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds Item to end of control collection")> _
        Public Sub addSortableItem(ByVal SortItem As Control)
            Dim sort As SortableItem = New SortableItem(Me.ID & "_" & SortableList.Controls.Count, SortItem)
            SortableList.Controls.Add(sort)
            OriginalOrder &= sort.PanelID & ","
        End Sub

        ''' <summary>
        ''' Adds Item to end of control collection
        ''' </summary>
        ''' <param name="SortItem">System.String to add to end of collection</param>
        ''' <param name="ContainerID">System.String Id of parent Panel</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds Item to end of control collection")> _
        Public Sub addSortableItem(ByVal SortItem As String, ByVal ContainerID As String)
            Dim sort As SortableItem = New SortableItem(ContainerID, New LiteralControl(SortItem))
            SortableList.Controls.Add(sort)
            OriginalOrder &= sort.PanelID & ","
        End Sub

        ''' <summary>
        ''' Adds Item to end of control collection
        ''' </summary>
        ''' <param name="SortItem">System.Web.UI.Control to add to end of collection</param>
        ''' <param name="ContainerID">System.String Id of parent Panel</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds Item to end of control collection")> _
        Public Sub addSortableItem(ByVal SortItem As Control, ByVal ContainerID As String)
            Dim sort As SortableItem = New SortableItem(ContainerID, SortItem)
            SortableList.Controls.Add(sort)
            OriginalOrder &= sort.PanelID & ","
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If AdminOn Then
                Dim Autopost As String = ""
                If AutoPostBack Then
                    Autopost = "__doPostBack('" & hfNewOrder.ClientID & "', '');"
                End If
                Dim st As String = "" & vbCrLf
                st &= "<script type=""text/javascript""> " & vbCrLf
                st &= "     $(function() { " & vbCrLf
                st &= "          $('#" & SortableList.ClientID() & "').sortable({ " & vbCrLf
                st &= "               cursor:'" & getEnumName(CursorType) & "'," & vbCrLf
                If PlaceHolderClass Is Nothing Then
                    st &= "               placeholder:'SortablePlaceHolder'," & vbCrLf
                Else
                    st &= "               placeholder:'" & PlaceHolderClass & "'," & vbCrLf
                End If
                If CursorPosition <> CursorAt.false OrElse CursorPositionValue > 0 Then
                    st &= "               cursorAt:{" & getEnumName(CursorPosition) & ":" & CursorPositionValue & "}," & vbCrLf
                End If
                If ConnectByClass = True Then
                    st &= "               connectWith:'." & SortableList.CssClass & "'," & vbCrLf
                ElseIf ConnectToID IsNot Nothing Then
                    st &= "               connectWith:'#" & ConnectToID & "'," & vbCrLf
                End If
                If AxisRestriction <> Axis.none Then
                    st &= "               axis:'" & getEnumName(AxisRestriction) & "'," & vbCrLf
                End If
                If DragOnEmpty = True Then
                    st &= "               dropOnEmpty:true," & vbCrLf
                End If
                If HelperType <> Helper.original Then
                    st &= "               helper:'clone'," & vbCrLf
                End If
                st &= "               update: function() {" & vbCrLf
                st &= "                            $('#" & hfNewOrder.ClientID() & "').val($('#" & SortableList.ClientID() & "').sortable(""toArray""));" & vbCrLf
                If Autopost <> "" Then
                    st &= "                            " & Autopost & vbCrLf
                End If
                st &= "               }" & vbCrLf
                st &= "          });" & vbCrLf
                st &= "     });" & vbCrLf
                st &= "</script>" & vbCrLf
                writer.Write(st)
            End If

            MyBase.Render(writer)
        End Sub

        Private Function getEnumName(ByVal enumeration As Object) As String
            Return [Enum].GetName(enumeration.GetType, enumeration)
        End Function

        ''' <summary>
        ''' Gets the array of re-ordered panel ID's
        ''' </summary>
        ''' <returns>
        ''' A System.String array of the Panel IDs after re-ordering
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets the array of re-ordered panel ID's")> _
        Public Function getNewOrderList() As String()
            Return Page.Request.Params(hfNewOrder.ClientID).Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
        End Function

        ''' <summary>
        ''' Gets the array of panel ID's before re-ording
        ''' </summary>
        ''' <returns>
        ''' A System.String array of the Panel IDs before re-ordering
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets the array of panel ID's before re-ording")> _
        Public Function getOriginalOrderList() As String()
            Return OriginalOrder.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
        End Function
    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Class SortableItem
        Inherits Panel
        Private _value As String = "-1"

        ''' <summary>
        ''' Gets the PanelID or the Databound value
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets the PanelID or the Databound value")> _
        Public ReadOnly Property Value() As String
            Get
                Return _value
            End Get
        End Property

        Private _PanelID As String = ""

        ''' <summary>
        ''' Gets the ID of the Parent Panel
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets the ID of the Parent Panel")> _
        Public ReadOnly Property PanelID() As String
            Get
                Return _PanelID
            End Get
        End Property

        ''' <summary>
        ''' Gets text of sortable LiteralControl, its type, or its Databound text
        ''' </summary>
        ''' <value></value>
        ''' <returns>System.String of Literal Control or string representation of it type</returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets text of sortable LiteralControl, its type, or its Databound text")> _
        Public ReadOnly Property Text() As String
            Get
                If Me.Controls.Count > 0 Then
                    If Me.Controls(0).GetType Is GetType(LiteralControl) Then
                        Dim lit As LiteralControl = CType(Me.Controls(0), LiteralControl)
                        Return lit.Text
                    Else
                        Return Me.Controls(0).ToString
                    End If
                Else
                    Return ""
                End If
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the SortableItem class.
        ''' </summary>
        ''' <param name="ParentPanelID">System.String ID of parent panel</param>
        ''' <param name="SortableControl">System.Web.UI.Control to be sorted</param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Initializes a new instance of the SortableItem class.")> _
        Public Sub New(ByVal ParentPanelID As String, ByRef SortableControl As Control)
            If ParentPanelID.Contains("_Value_") Then
                _value = ParentPanelID.Substring(ParentPanelID.LastIndexOf("_")).Replace("_", "")
            Else
                _value = ParentPanelID
            End If
            _PanelID = ParentPanelID
            Me.ID = PanelID
            If SortableControl IsNot Nothing Then
                Me.Controls.Add(SortableControl)
            End If
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the SortableItem class.
        ''' </summary>
        ''' <param name="ParentPanelID">System.String ID of parent panel</param>
        ''' <param name="SortableControls">System.Web.UI.ControlCollection to be sorted</param>
        ''' <remarks>All of the Controls in SortableControls will be as one SortableItem</remarks>
        <System.ComponentModel.Description("Initializes a new instance of the SortableItem class.")> _
        Public Sub New(ByVal ParentPanelID As String, ByRef SortableControls As ControlCollection)
            If ParentPanelID.Contains("_Value_") Then
                _value = ParentPanelID.Substring(ParentPanelID.LastIndexOf("_")).Replace("_", "")
            Else
                _value = ParentPanelID
            End If
            _PanelID = ParentPanelID
            Me.ID = PanelID
            For Each con As Control In SortableControls
                Me.Controls.Add(con)
            Next
        End Sub
    End Class