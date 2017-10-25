Imports System.Reflection
Imports DTIServerControls

''' <summary>
''' A data-drive sortable panel. Data is saved in the database and dynamic elements are addable from an admin panel.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("A data-drive sortable panel. Data is saved in the database and dynamic elements are addable from an admin panel."),ToolboxData("<{0}:DTISortable ID=""Sortable"" runat=""server"" contentType=""Sortable""> </{0}:DTISortable>")> _
Public Class DTISortable
    Inherits DTIServerBase

    Private DataFetched As Boolean = False
    Private sortableID As Integer
    Private hiddenString As String = ""
    Protected hiddenfield1 As New HiddenField
    Private doRedirect As Boolean = False
    Private options As String = ""
    Private SortableList As New Panel
    Private mediaHash As Hashtable
    Private update As Boolean = False

#Region "Properties"

    ''' <summary>
    ''' Hashtable that Holds Sortables added directly to the page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Hashtable that Holds Sortables added directly to the page")> _
    Private ReadOnly Property sotableServerTempList() As Hashtable
        Get
            If Session("SortableServerTempList") Is Nothing Then
                Session("SortableServerTempList") = New Hashtable
            End If
            Return Session("SortableServerTempList")
        End Get
    End Property

    Private Sub removeSortableServerList()
        Session.Remove("SortableServerTempList")
    End Sub

    ''' <summary>
    ''' Holds Sortables added directly to the page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Holds Sortables added directly to the page")> _
    Private ReadOnly Property sotableServerTempList2() As Generic.List(Of DTISortable)
        Get
            If Session("SortableServerTempList2") Is Nothing Then
                Session("SortableServerTempList2") = New Generic.List(Of DTISortable)
            End If
            Return Session("SortableServerTempList2")
        End Get
    End Property

    Private Sub removeSortableServerList2()
        Session.Remove("SortableServerTempList2")
    End Sub

    Private Property doUpdate() As Boolean
        Get
            If Session("HasOneOfTheSortablesBeenUpdatedAndNeedToBePushedToServer") Is Nothing Then
                Session("HasOneOfTheSortablesBeenUpdatedAndNeedToBePushedToServer") = False
            End If
            Return Session("HasOneOfTheSortablesBeenUpdatedAndNeedToBePushedToServer")
        End Get
        Set(ByVal value As Boolean)
            Session("HasOneOfTheSortablesBeenUpdatedAndNeedToBePushedToServer") = value
        End Set
    End Property

    ''' <summary>
    ''' Enables Layout mode for all sortables, Menu's and recyclebins
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables Layout mode for all sortables, Menu's and recyclebins")> _
    Public Property isRecyclable() As Boolean
        Get
            If Session("DoesSortableServerHaveARecycleBinOnThePageTopPutItems") Is Nothing Then
                Session("DoesSortableServerHaveARecycleBinOnThePageTopPutItems") = False
            End If
            Return Session("DoesSortableServerHaveARecycleBinOnThePageTopPutItems")
        End Get
        Set(ByVal value As Boolean)
            Session("DoesSortableServerHaveARecycleBinOnThePageTopPutItems") = value
        End Set
    End Property

    ''' <summary>
    ''' Enables sortable mode for all sortables, Menu's and recyclebins
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables sortable mode for all sortables, Menu's and recyclebins")> _
    Public Property LayoutOnAll() As Boolean
        Get
            Return DTIServerControls.DTISharedVariables.LayoutOn
        End Get
        Set(ByVal value As Boolean)
            DTIServerControls.DTISharedVariables.LayoutOn = value
        End Set
    End Property

    Private _LayoutOn As Boolean

    ''' <summary>
    ''' Enables Sortable Mode for this sortable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables Sortable Mode for this sortable")> _
    Public Property LayoutOn() As Boolean
        Get
            If DTISharedVariables.LayoutOn Then
                If DTISharedVariables.siteEditMainID = Me.MainID Or DTISharedVariables.siteEditMainID = 0 Then
                    Return True
                End If
            End If
            If Session("IsLayoutOn_" & Me.ID) Is Nothing Then
                Session("IsLayoutOn_" & Me.ID) = False
            End If
            Return Session("IsLayoutOn_" & Me.ID)
        End Get
        Set(ByVal value As Boolean)
            Session("IsLayoutOn_" & Me.ID) = value
        End Set
    End Property

    Public Enum Axis As Integer
        none = 0
        x = 1
        y = 2
    End Enum

    Private _AxisRestriction As Axis

    ''' <summary>
    ''' Restricts the axis to one particular direction x or y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Restricts the axis to one particular direction x or y")> _
    Public Property AxisRestriction() As Axis
        Get
            Return _AxisRestriction
        End Get
        Set(ByVal value As Axis)
            _AxisRestriction = value
        End Set
    End Property

    Private _ConnectByClass As Boolean = True

    ''' <summary>
    ''' Enables drag and drop to sortables with the same class name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables drag and drop to sortables with the same class name")> _
    Public Property ConnectByClass() As Boolean
        Get
            Return _ConnectByClass
        End Get
        Set(ByVal value As Boolean)
            _ConnectByClass = value
        End Set
    End Property

    Private _ConnectedCSS As String

    ''' <summary>
    ''' Class name by which multible sortables are connected
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Class name by which multible sortables are connected")> _
    Public Property ConnectedCSS() As String
        Get
            Return _ConnectedCSS
        End Get
        Set(ByVal value As String)
            _ConnectedCSS = value
        End Set
    End Property

    Private _ConnectToID As String

    ''' <summary>
    ''' Connects one sortable to another specificed by its id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Connects one sortable to another specificed by its id")> _
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
    ''' Class name for the Placeholder (Shows were sortable item can be dropped)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Class name for the Placeholder (Shows were sortable item can be dropped)")> _
    Public Property PlaceHolderClass() As String
        Get
            Return _PlaceHolderClass
        End Get
        Set(ByVal value As String)
            _PlaceHolderClass = value
        End Set
    End Property

    Public Enum Cursor As Integer
        move = 0
        pointer = 1
        crosshair = 2
    End Enum

    Private _cursortype As Cursor

    ''' <summary>
    ''' Type of curser when moving sortable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Type of curser when moving sortable")> _
    Public Property CursorType() As Cursor
        Get
            Return _cursortype
        End Get
        Set(ByVal value As Cursor)
            _cursortype = value
        End Set
    End Property

    Public Enum CursorAt As Integer
        [false] = 0
        top = 1
        left = 2
        right = 3
        bottom = 4
    End Enum

    Private _CursorPosition As CursorAt

    ''' <summary>
    ''' Changes where the cursor is located on a sortable item when it is being moved
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Changes where the cursor is located on a sortable item when it is being moved")> _
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
    ''' Offset of the cursor position
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Offset of the cursor position")> _
    Public Property CursorPositionValue() As Integer
        Get
            Return _CursorPositionValue
        End Get
        Set(ByVal value As Integer)
            _CursorPositionValue = value
        End Set
    End Property

    Private _DragOnEmpty As Boolean = False

    ''' <summary>
    ''' Disables dragging to an empty list
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Disables dragging to an empty list")> _
    Public Property NoDragOnEmpty() As Boolean
        Get
            Return _DragOnEmpty
        End Get
        Set(ByVal value As Boolean)
            _DragOnEmpty = value
        End Set
    End Property

    'Private _disable As Boolean
    '''' <summary>
    '''' Disables a specific item from being sortable :beta
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Property Disable() As Boolean
    '    Get
    '        Return _disable
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _disable = value
    '    End Set
    'End Property

    Public Enum Helper As Integer
        original = 0
        clone = 1
    End Enum

    Private _HelperType As Helper

    ''' <summary>
    ''' Original if the item is to be moved and clone if it is to be coppied when moving
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Original if the item is to be moved and clone if it is to be coppied when moving")> _
    Public Property HelperType() As Helper
        Get
            Return _HelperType
        End Get
        Set(ByVal value As Helper)
            _HelperType = value
        End Set
    End Property

    Private _HandleText As String = "Click to Drag"

    ''' <summary>
    ''' Text that goes into the handle of the sortable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text that goes into the handle of the sortable")> _
    Public Property HandleText() As String
        Get
            Return _HandleText
        End Get
        Set(ByVal value As String)
            _HandleText = value
        End Set
    End Property

    Private _HandleSelector As String

    ''' <summary>
    ''' Class name, id, or html tag of the handle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Class name, id, or html tag of the handle")> _
    Public Property HandleSelector() As String
        Get
            Return _HandleSelector
        End Get
        Set(ByVal value As String)
            _HandleSelector = value
        End Set
    End Property

    Private _zIndex As Integer

    ''' <summary>
    ''' specifies the stack order of an item
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("specifies the stack order of an item")> _
    Public Property ZIndex() As Integer
        Get
            Return _zIndex
        End Get
        Set(ByVal value As Integer)
            _zIndex = value
        End Set
    End Property

    Private _cssTheme As String = "default"

    ''' <summary>
    ''' Changes the css name so they can be overridden
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Changes the css name so they can be overridden")> _
    Public Property cssTheme() As String
        Get
            Return _cssTheme
        End Get
        Set(ByVal value As String)
            _cssTheme = value
        End Set
    End Property

    Private _deleteText As String = "X"

    ''' <summary>
    ''' Display text to delete any sortable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Display text to delete any sortable")> _
    Public Property DeleteText() As String
        Get
            Return _deleteText
        End Get
        Set(ByVal value As String)
            _deleteText = value
        End Set
    End Property

    Private _OuterStyle As String = "border: thin dashed red"

    ''' <summary>
    ''' Out line style of the sortable when in sorting mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Out line style of the sortable when in sorting mode")> _
    Public Property OuterStyle() As String
        Get
            Return _OuterStyle
        End Get
        Set(ByVal value As String)
            _OuterStyle = value
        End Set
    End Property

    Private _AutoAddEditPanel As Boolean = True

    ''' <summary>
    ''' Determines if an EditPanel is automatically added to an empty sortable.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines if an EditPanel is automatically added to an empty sortable.")> _
    Public Property AutoAddEditPanel() As Boolean
        Get
            Return _AutoAddEditPanel
        End Get
        Set(ByVal value As Boolean)
            _AutoAddEditPanel = value
        End Set
    End Property

    Private _AutoWrapInEditPanel As Boolean = True

    ''' <summary>
    ''' If sortable contains only literal controls They will be wrapped in a single edit panel otherwise
    ''' they are wrapped in ASP panels objects
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If sortable contains only literal controls They will be wrapped in a single edit panel otherwise   they are wrapped in ASP panels objects")> _
    Public Property AutoWrapInEditPanel() As Boolean
        Get
            Return _AutoWrapInEditPanel
        End Get
        Set(ByVal value As Boolean)
            _AutoWrapInEditPanel = value
        End Set
    End Property

    Private _ItemsClassName As String = ""

    ''' <summary>
    ''' Adds a class name to each item within a sortable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a class name to each item within a sortable")> _
    Public Property ItemsClassName() As String
        Get
            Return _ItemsClassName
        End Get
        Set(ByVal value As String)
            _ItemsClassName = value
        End Set
    End Property

    Private _controlist As New Collections.Specialized.OrderedDictionary
    Private Property ControlList() As Collections.Specialized.OrderedDictionary
        Get
            If _controlist Is Nothing Then
                _controlist = New OrderedDictionary
            End If
            Return _controlist
        End Get
        Set(ByVal value As Collections.Specialized.OrderedDictionary)
            _controlist = value
        End Set
    End Property

    Private ReadOnly Property ds() As dsDTISortable
        Get
            If Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") Is Nothing Then
                Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") = New dsDTISortable
            End If
            Return Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu")
        End Get
    End Property
#End Region

    Public Sub New()
    End Sub

    Public Sub New(ByVal ContentType As String, Optional ByVal width As Integer = -1)
        MyBase.New()
        Me.contentType = ContentType
        If width <> -1 Then _
            Me.Width = width
    End Sub

    Private Sub DTISortable_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If LayoutOn Then
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTISortable/Sortable.js")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTISortable/DTISortable.css", "text/css")
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(Me.Page)
        End If
        loadSortable()
    End Sub

    Private Sub loadSortable()
        addSQLCall()

        'adds sortbale if its not in the db
        Dim dvSortTable As New DataView(ds.DTISortable, "Content_Type='" & Me.contentType & "'", "", DataViewRowState.CurrentRows)
        If dvSortTable.Count > 0 Then
            sortableID = dvSortTable(0).Item("Id")
        Else
            Dim sortableRow As dsDTISortable.DTISortableRow = ds.DTISortable.NewDTISortableRow
            With sortableRow
                .Main_Id = MainID
                .Content_Type = contentType
            End With
            ds.DTISortable.AddDTISortableRow(sortableRow)
            sqlhelper.Update(ds.DTISortable)
            sortableID = sortableRow.Id
        End If

        Dim datachanged As Boolean = False
        Dim htPanels As New Collection
        Dim htPanelControls As Collection = Nothing

        'Checking any controls placed inside the sortable
        Dim onlyLiterals As Boolean = True
        For Each con As Control In Me.Controls
            If Not TypeOf con Is LiteralControl Then
                onlyLiterals = False
                Exit For
            End If
        Next

        'If control list contains only literals they will be wrapped in an edit panel
        If onlyLiterals AndAlso AutoWrapInEditPanel Then
            Dim s As String = ""
            For Each con As Control In Me.Controls
                s &= CType(con, LiteralControl).Text
                con.Visible = False
            Next
            Dim epConType As String = "InsertedDTIContentManagementEditPanel" & Me.contentType
            Dim ep As New DTIContentManagement.EditPanel
            Dim dtEditPanel As New DTIContentManagement.dsEditPanel.DTIContentManagerDataTable

            Dim dv As New DataView(ds.DTISortableItem, "Content_Type='" & epConType & "'", "", DataViewRowState.CurrentRows)
            If dv.Count = 0 Then
                sqlhelper.checkAndCreateTable(dtEditPanel)
                dtEditPanel.AddDTIContentManagerRow(s, MainID, epConType, Now)
                sqlhelper.Update(dtEditPanel)

                ds.DTISortableItem.AddDTISortableItemRow(epConType, 0, sortableID, "DTIContentManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#DTIContentManagement.EditPanel", False, Now, Nothing)
                datachanged = True
            ElseIf dv.Count = 1 Then
                'maybe replacing text if it has changed on the page
            End If
        Else
            'It will wrap any controls found in asp panels for sorting. unless it is already an asp panel
            For Each con As Control In Me.Controls
                If TypeOf con Is Panel Then
                    If htPanelControls IsNot Nothing Then
                        'If i've started a collection of controls to store I store that in a collection 
                        'which will represent my sortable panels, and start over the control collection
                        htPanels.Add(htPanelControls)
                        htPanelControls = New Collection
                    End If
                    htPanels.Add(con)
                Else
                    Dim s As String = ""
                    If TypeOf con Is LiteralControl Then
                        'getting rid of any blank white space in html so as not to save as sortable
                        s = CType(con, LiteralControl).Text.Replace(vbCrLf, "").Replace(vbTab, "").Replace(" ", "")
                    End If
                    If s <> "" OrElse Not TypeOf con Is LiteralControl Then
                        'starts a new control collection and adds the next non-panel control to that collection
                        If htPanelControls Is Nothing Then
                            htPanelControls = New Collection
                        End If
                        htPanelControls.Add(con)
                    End If
                End If
            Next
            'if there are any controls in the collection means we didn't hit a panel so I save it
            If htPanelControls IsNot Nothing Then
                htPanels.Add(htPanelControls)
            End If
        End If

        Dim i As Integer = 0
        'removes controls and wraps then in panels and adds the panel back
        'added panels will now be sortable
        For Each panColl As Object In htPanels
            If panColl.GetType Is GetType(Collection) AndAlso panColl.Count > 0 Then
                Dim panTheMan As New Panel
                panTheMan.ID = Me.ID & "_AspPanel_" & i
                For Each con As Control In panColl
                    Me.Controls.Remove(con)
                    panTheMan.Controls.Add(con)
                Next
                SaveSortablePanel(panTheMan, datachanged)
                Me.Controls.Add(panTheMan)
            ElseIf TypeOf panColl Is Panel Then
                Me.Controls.Remove(panColl)
                Me.Controls.Add(panColl)
                SaveSortablePanel(panColl, datachanged)
            End If
            i += 1
        Next
        If datachanged Then _
            sqlhelper.Update(ds.DTISortableItem)

        loadData()
        OutputControl()
    End Sub

    Private Sub SaveSortablePanel(ByRef con As Control, ByRef dataChanged As Boolean)
        'this fires for every static panel within a sortable
        Dim pan As Panel = CType(con, Panel)
        Dim id As String = con.ID '.ClientID
        'Dim dv As New DataView(ds.DTISortableItem, "Assembly_Name='DTIClient#" & id & "' AND DTISortable_ID = " & sortableID, "", DataViewRowState.CurrentRows)
        Dim dv As New DataView(ds.DTISortableItem, "Assembly_Name='DTIClient#" & id & "' AND Page_Id = '" & Page.Request.Path & "'", "", DataViewRowState.CurrentRows)
        If dv.Count = 0 Then
            Dim s As String = id & Guid.NewGuid.ToString
            If con.GetType Is GetType(DTIContentManagement.EditPanel) Then
                s = CType(con, DTIContentManagement.EditPanel).contentType
            End If
            ds.DTISortableItem.AddDTISortableItemRow(s.Replace("-", "").Replace(".", ""), ds.DTISortableItem.Count, sortableID, "DTIClient#" & id, False, Now, Page.Request.Path)
            dataChanged = True
        End If
        pan.Style.Add("display", "none")
        'pan.Visible = False
    End Sub

    Private Sub OutputControl()

        Dim hasHandle As Boolean = False

        SortableList.ID = Me.ClientID & "_SortableList"
        hiddenfield1.ID = Me.ClientID & "_HiddenField1"

        If LayoutOn Then
            hiddenfield1.Value = ""
            options = "cursor:'" & getEnumName(CursorType) & "',"
            If CursorPosition <> CursorAt.false OrElse CursorPositionValue > 0 Then
                options &= "cursorAt:{" & getEnumName(CursorPosition) & ":" & CursorPositionValue & "},"
            End If
            If CssClass = "" Then
                CssClass = "DTISortable-" & cssTheme
            End If
            If ConnectedCSS = "" Then
                ConnectedCSS = "DTIConnectedSortable-" & cssTheme
            End If

            SortableList.Attributes.Add("class", ConnectedCSS)

            If ConnectByClass = True Then
                options &= "connectWith:'." & ConnectedCSS & "',"
            ElseIf ConnectToID IsNot Nothing Then
                options &= "connectWith:'#" & ConnectToID & "',"
            End If
            If PlaceHolderClass Is Nothing Then
                PlaceHolderClass = "DTIPlaceHolder-" & cssTheme
            End If

            options &= "placeholder:'" & PlaceHolderClass & "',"

            If AxisRestriction <> Axis.none Then
                options &= "axis:'" & getEnumName(AxisRestriction) & "',"
            End If
            If NoDragOnEmpty = True Then
                options &= "dropOnEmpty:true,"
            End If
            If HandleText <> "" And String.IsNullOrEmpty(HandleSelector) Then
                hasHandle = True
                options &= "handle: '.DTIItemHandle-" & cssTheme & "',"
            ElseIf HandleSelector <> "" Then
                options &= "handle: '" & HandleSelector & "',"
            End If
            If ZIndex <> 0 Then
                options &= "zIndex: " & ZIndex & ","
            End If

            If HelperType <> Helper.original Then
                options &= "helper:'clone'"
            End If

            SortableList.Attributes.Add("class", ConnectedCSS)

            Try
                Dim style() As String = OuterStyle.Split(";")
                For Each st As String In style
                    Dim s() As String = st.Split(":")
                    SortableList.Style.Add(s(0), s(1))
                Next
            Catch ex As Exception

            End Try
            Me.Controls.Add(hiddenfield1)
        End If

        For Each item As DictionaryEntry In ControlList
            Dim name As String = item.Value.ToString
            If TypeOf item.Value Is Panel Then
                Dim x As Panel = item.Value
                If x.Controls.Count > 0 Then
                    If TypeOf x.Controls(0) Is UserControl Then
                        name = x.Controls(0).GetType().Name.Replace("_ascx", "")
                    End If
                End If
            End If
            Dim dotIndex As Integer = name.LastIndexOf(".") + 1
            Dim sName As String = name.Substring(dotIndex, name.Length - dotIndex)
            If LayoutOn Then
                SortableList.Controls.Add(New LiteralControl("<div id=""DTISortItem_" & item.Key & """ class=""DTISortableItem-" & cssTheme & """>" & vbCrLf))
                If hasHandle Then
                    SortableList.Controls.Add(New LiteralControl("     <div class=""DTIItemHandle-" & cssTheme & """>" & vbCrLf))
                    SortableList.Controls.Add(New LiteralControl("          <img id=""imgelem" & item.Key & """ style=""cursor:hand;right:2px;float:left;"" src=""~/res/BaseClasses/Scripts.aspx?f=DTISortable/collapse.jpg"" onclick=""ToggleDisplay(" & item.Key & ")"" />&nbsp;" & vbCrLf))
                    SortableList.Controls.Add(New LiteralControl("          <span style=""float:left;"">&nbsp;" & HandleText & " - " & sName & "</span>" & vbCrLf))
                    'End If
                    If isRecyclable Then
                        SortableList.Controls.Add(New LiteralControl("          <span style=""cursor:pointer;float:right"" onclick=""DeleteRecycledBinItem('DTISortItem_" & item.Key & "')"">" & DeleteText & "</span>" & vbCrLf))
                    End If
                    'If hasHandle Then
                    SortableList.Controls.Add(New LiteralControl("          <br clear=""both"" />" & vbCrLf))
                    SortableList.Controls.Add(New LiteralControl("     </div><!-- End handle div -->" & vbCrLf)) 'end handle div
                    SortableList.Controls.Add(New LiteralControl("     <div id=""divelem" & item.Key & """ style=""width:100%;"">" & vbCrLf)) 'start body container
                End If
            End If
            Try
                SortableList.Controls.Add(item.Value)
            Catch ex As Exception

            End Try
            If LayoutOn Then
                If hasHandle Then
                    SortableList.Controls.Add(New LiteralControl(vbCrLf & "     </div><!-- End bodydiv if handle -->" & vbCrLf)) 'end body container
                End If
                SortableList.Controls.Add(New LiteralControl("</div><!-- End SortableItem -->" & vbCrLf))
            End If
        Next
        If LayoutOn Then
            Me.Controls.Add(SortableList)
        Else
            Dim htcontrols As New Collection
            For Each con As Control In SortableList.Controls
                htcontrols.Add(con)
            Next
            For Each con As Control In htcontrols
                Dim pan As Panel = CType(con, Panel)
                If ItemsClassName <> "" Then
                    pan.CssClass = ItemsClassName
                End If
                Try
                    Me.Controls.Add(pan)
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub loadData()
        sotableServerTempList2.Insert(0, Me)
        'checks if it is a new sortable, if so inserts an edit panel by default
        Dim dv As New DataView(ds.DTISortableItem, "DTISortable_Id=" & sortableID, "Sort_Order asc", DataViewRowState.CurrentRows)
        If dv.Count = 0 AndAlso AutoAddEditPanel Then
            ds.DTISortableItem.AddDTISortableItemRow("DTIContentManagementEditPanel" & Guid.NewGuid.ToString.Replace("-", ""), 0, sortableID, "DTIContentManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#DTIContentManagement.EditPanel", False, Now, Nothing)
            doUpdate = True
        End If
        Dim dvSortableItem As New DataView(ds.DTISortableItem, "DTISortable_Id=" & sortableID & "AND isDeleted=0", "Sort_Order asc", DataViewRowState.CurrentRows)
        'Dim dvSortableItem As New DataView(ds.DTISortableItem, "DTISortable_Id=" & sortableID & "AND isDeleted=0 and (Page_Id is null or Page_Id = '" & Page.Request.Path & "')", "Sort_Order asc", DataViewRowState.CurrentRows)
        If dvSortableItem.Count > 0 Then
            For Each ctrlID As String In sotableServerTempList.Keys
                'Replaces temparary panels with ones found in this sortable
                Dim movedPanel As Panel = FindControl(ctrlID)
                If Not movedPanel Is Nothing Then
                    Dim movegroup As Object() = sotableServerTempList(ctrlID)
                    Dim tempPan As Panel = movegroup(0)
                    Dim destCtrl As Control = movegroup(1)
                    Dim isdeleted As Boolean = movegroup(2)
                    Dim itemclass As String = CType(destCtrl, DTISortable).ItemsClassName
                    'gets the correct new item class name
                    movedPanel.CssClass = itemclass

                    Dim foundctrl As Panel = destCtrl.FindControl(tempPan.ID)
                    If foundctrl IsNot Nothing Then
                        Dim reppanelindex = destCtrl.Controls.IndexOf(foundctrl)
                        If reppanelindex = -1 Then
                            'in layout mode items are wrapped in another div
                            If tempPan.Parent IsNot Nothing AndAlso tempPan.Parent.GetType Is GetType(Panel) Then
                                destCtrl = tempPan.Parent
                                reppanelindex = destCtrl.Controls.IndexOf(foundctrl)
                            End If
                        End If
                        If reppanelindex > -1 Then
                            destCtrl.Controls.RemoveAt(reppanelindex)
                            destCtrl.Controls.AddAt(reppanelindex, movedPanel)
                            If Not isdeleted Then
                                'movedPanel.Visible = True
                                movedPanel.Style.Remove("display")
                            End If
                        End If
                    End If

                End If
            Next
            Dim i As Integer = Me.ControlList.Count
            For Each row As DataRowView In dvSortableItem
                Dim assType() As String = CType(row.Item("Assembly_Name"), String).Split("#")
                Dim assName As String = assType(0)
                Dim ass As Assembly
                If assName = "DTIClient" Then
                    Dim ctrlID As String = assType(1)
                    Dim panTheMan As Panel = Nothing
                    For Each servctrl As DTISortable In sotableServerTempList2
                        panTheMan = servctrl.FindControl(ctrlID)
                        If panTheMan IsNot Nothing Then Exit For
                    Next

                    If panTheMan Is Nothing Then
                        'Temp panel for static items not found in this sortable
                        panTheMan = New Panel
                        panTheMan.ID = Me.ClientID & i
                        sotableServerTempList(ctrlID) = New Object() {panTheMan, Me, row("isDeleted")}
                    Else
                        panTheMan.Style.Remove("display")
                        'panTheMan.Visible = True
                    End If
                    Me.ControlList.Add(row("Id"), panTheMan)
                ElseIf assName = "UserControl" Then
                    Try
                        Dim ctrl As UserControl = Me.Page.LoadControl("~/" & assType(1))
                        Dim pnl As New Panel
                        pnl.Controls.Add(ctrl)
                        Me.ControlList.Add(row.Item("Id"), pnl)
                    Catch ex As Exception

                    End Try
                Else
                    Try
                        ass = Assembly.Load(assName)
                        If ass Is Nothing Then Throw New Exception
                    Catch ex As Exception
                        Try
                            assName = "DTIControls" & assName.Substring(assName.IndexOf(","))
                            ass = Assembly.Load(assName)
                            If ass Is Nothing Then Throw New Exception
                        Catch ex1 As Exception
                            Try
                                ass = Assembly.Load(assType(1) & assName.Substring(assName.IndexOf(",")))
                            Catch ex2 As Exception
                                Try
                                    ass = Assembly.Load(assType(1).Substring(0, assType(1).IndexOf(".")))
                                Catch ex3 As Exception

                                End Try
                            End Try
                        End Try
                    End Try
                    Dim servCont As DTIServerControls.DTIServerControl = ass.CreateInstance(assType(1))
                    If servCont IsNot Nothing Then
                        servCont.contentType = row.Item("Content_Type")
                        servCont.ID = Me.ClientID & "_Subcontrol_" & row.Item("Id")
                        Me.ControlList.Add(row.Item("Id"), servCont)
                    End If
                End If
                i += 1
            Next
        End If
    End Sub

    ''' <summary>
    ''' Saves the Order of the Current List's Items
    ''' </summary>
    ''' <param name="forceRedirect">Forces redirect on Pre-render</param>
    ''' <param name="SaveData">Push Data Changes to Database on Pre-render</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Saves the Order of the Current List's Items")> _
    Public Sub Save(Optional ByVal forceRedirect As Boolean = True, Optional ByVal SaveData As Boolean = True)

        Dim arr() As String = hiddenfield1.Value.Trim(",").Split(",")
        Dim order As Integer = 0

        If arr IsNot Nothing AndAlso arr.Length > 0 AndAlso Not String.IsNullOrEmpty(arr(0)) Then
            update = True
            For Each s As String In arr
                If Not String.IsNullOrEmpty(s) Then
                    If s.StartsWith("Menu_") AndAlso ds.WidgetMenuTemp.Count > 0 Then
                        Dim menId As Integer = Integer.Parse(s.Replace("Menu_", "").Replace("_x", ""))
                        Dim assName As String = ds.WidgetMenuTemp.FindById(menId).Assembly
                        Dim typeName As String = ds.WidgetMenuTemp.FindById(menId).Typename
                        If typeName.Contains("#") Then
                            typeName = typeName.Substring(0, typeName.IndexOf("#"))
                        End If
                        Dim str As String = typeName & Guid.NewGuid.ToString
                        Dim row As dsDTISortable.DTISortableItemRow = _
                            ds.DTISortableItem.AddDTISortableItemRow(str.Replace("-", "").Replace(".", ""), order, sortableID, assName & "#" & typeName, False, Now, Nothing)
                    ElseIf s.StartsWith("DTISortItem_") Then
                        Dim id As Integer = Integer.Parse(s.Replace("DTISortItem_", ""))
                        Dim sortItem As dsDTISortable.DTISortableItemRow = ds.DTISortableItem.FindById(id)
                        If sortItem Is Nothing Then
                            sqlhelper.SafeFillTable("select * from DTISortableItem where id = @id", ds.DTISortableItem, New Object() {id})
                            sortItem = ds.DTISortableItem.FindById(id)
                        End If
                        sortItem.Sort_Order = order
                        sortItem.DTISortable_Id = sortableID
                    ElseIf s.StartsWith("DTIRecycle_") Then
                        Dim id As Integer = Integer.Parse(s.Replace("DTIRecycle_", ""))
                        Dim dssortItem As dsDTISortable.DTISortableItemRow = ds.DTISortableItem.FindById(id)
                        If dssortItem IsNot Nothing Then
                            dssortItem.isDeleted = False
                            dssortItem.Sort_Order = order
                            dssortItem.DTISortable_Id = sortableID
                        End If
                    End If
                End If
                order += 1
            Next
        End If

        If update AndAlso SaveData Then _
            doUpdate = True
        If forceRedirect Then _
            doRedirect = True

    End Sub

    ''' <summary>
    ''' Spider's page for all DTISortableControls and calls their respective save method
    ''' </summary>
    ''' <param name="pg">Page to Spider</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Spider's page for all DTISortableControls and calls their respective save method")> _
    Public Sub saveAll(ByRef pg As Page)
        Dim col As Collection = BaseClasses.Spider.spiderPageforTypeArray(pg, GetType(DTISortable))
        For Each sort As DTISortable In col
            sort.Save()
        Next
    End Sub

    Private Sub addSQLCall()
        If (ds.DTISortable Is Nothing OrElse ds.DTISortable.Count = 0) AndAlso (ds.DTISortableItem Is Nothing OrElse ds.DTISortableItem.Count = 0) Then
            sqlhelper.checkAndCreateTable(ds.DTISortable)
            sqlhelper.checkAndCreateTable(ds.DTISortableItem)
            ds.DTISortable.Clear()
            ds.DTISortableItem.Clear()
            sqlhelper.FillDataTable("Select * from DTISortable where Main_Id = " & MainID, ds.DTISortable)
            sqlhelper.FillDataTable("Select * from DTISortableItem where DTISortable_Id in (Select id from DTISortable where Main_Id = " & MainID & ")", ds.DTISortableItem)
        End If
    End Sub

    Public Shared Function getEnumName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function

    Private Sub DTISortable_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If doUpdate Then
            sqlhelper.Update(ds.DTISortableItem)
            doUpdate = False
        End If
        If doRedirect Then _
            Page.Response.Redirect(Page.Request.Url.OriginalString)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If LayoutOn Then
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            writer.Write(vbCrLf & "<script type=""text/javascript""> " & vbCrLf & _
                        "	$(function() { " & vbCrLf & _
                        "		$('#" & SortableList.ClientID() & "').sortable({ " & vbCrLf & _
                        "			" & options & vbCrLf & _
                        "			update: function() {document.getElementById('" & hiddenfield1.ClientID() & "').value = $('#" & SortableList.ClientID() & "').sortable(""toArray"");}, " & vbCrLf & _
                        "           receive: function(event, ui){AddHeader(ui,'" & hiddenfield1.ClientID() & "','#" & SortableList.ClientID() & "');}" & vbCrLf & _
                        "		}); " & vbCrLf & _
                        "	});" & vbCrLf & _
                        "</script> " & vbCrLf)
        End If
        MyBase.Render(writer)
    End Sub

    Private Sub DTISortable_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        removeSortableServerList()
        removeSortableServerList2()
        sotableServerTempList.Clear()
    End Sub
End Class
