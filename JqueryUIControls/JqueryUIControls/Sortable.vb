Public Class Sortable
    Inherits Panel

#Region "Properties"

    Private _rendertag As HtmlTextWriterTag = HtmlTextWriterTag.Div

    ''' <summary>
    ''' Change the html tag used for this sortable (div, ul, p, etc.)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Change the html tag used for this sortable (div, ul, p, etc.)")> _
    Public WriteOnly Property RenderTag As HtmlTextWriterTag
        Set(value As HtmlTextWriterTag)
            _rendertag = value
        End Set
    End Property

    Protected Overrides ReadOnly Property TagKey As HtmlTextWriterTag
        Get
            Return _rendertag
        End Get
    End Property

    Private _appendto As String = ""

    ''' <summary>
    ''' Defines where the helper that moves with the mouse is being appended to during the drag (for example, to resolve overlap/zIndex issues).
    ''' Multiple types supported:
    '''   jQuery: A jQuery object containing the element to append the helper to.
    '''   Element: The element to append the helper to.
    '''    Selector: A selector specifying which element to append the helper to.
    '''    String: The string "parent" will cause the helper to be a sibling of the sortable item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Defines where the helper that moves with the mouse is being appended to during the drag (for example, to resolve overlap/zIndex issues).   Multiple types supported:    jQuery: A jQuery object containing the element to append the helper to.    Element: The element to append the helper to.     Selector: A selector specifying which element to append the helper to.     String: The string ""parent"" will cause the helper to be a sibling of the sortable item.")> _
    Public Property AppendTo As String
        Get
            Return _appendto
        End Get
        Set(value As String)
            _appendto = value
        End Set
    End Property

    Public Enum Axes
        none
        x
        y
    End Enum

    Private _axis As Axes = Axes.none

    ''' <summary>
    ''' If defined, the items can be dragged only horizontally or vertically.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If defined, the items can be dragged only horizontally or vertically.")> _
    Public Property Axis As Axes
        Get
            Return _axis
        End Get
        Set(value As Axes)
            _axis = value
        End Set
    End Property

    Private _Cancel As String = ""

    ''' <summary>
    ''' Prevents sorting if you start on elements matching the selector.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Prevents sorting if you start on elements matching the selector.")> _
    Public Property Cancel As String
        Get
            Return _Cancel
        End Get
        Set(value As String)
            _Cancel = value
        End Set
    End Property

    Private _connectWith As String = ""

    ''' <summary>
    ''' A selector of other sortable elements that the items from this list should be connected to. 
    ''' This is a one-way relationship, if you want the items to be connected in both directions, 
    ''' the connectWith option must be set on both sortable elements.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A selector of other sortable elements that the items from this list should be connected to.    This is a one-way relationship, if you want the items to be connected in both directions,    the connectWith option must be set on both sortable elements.")> _
    Public Property ConnectWith As String
        Get
            Return _connectWith
        End Get
        Set(value As String)
            _connectWith = value
        End Set
    End Property

    Private _containment As String = ""

    ''' <summary>
    ''' Defines a bounding box that the sortable items are contrained to while dragging.
    ''' Note: The element specified for containment must have a calculated width and height 
    ''' (though it need not be explicit). For example, if you have float: left sortable children and 
    ''' specify containment: "parent" be sure to have float: left on the sortable/parent 
    ''' container as well or it will have height: 0, causing undefined behavior.
    ''' 
    ''' Multiple types supported:
    '''      Element: An element to use as the container.
    '''      Selector: A selector specifying an element to use as the container.
    '''      String: A string identifying an element to use as the container. Possible values: "parent", "document", "window".
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Defines a bounding box that the sortable items are contrained to while dragging.   Note: The element specified for containment must have a calculated width and height    (though it need not be explicit). For example, if you have float: left sortable children and    specify containment: ""parent"" be sure to have float: left on the sortable/parent    container as well or it will have height: 0, causing undefined behavior.      Multiple types supported:      Element: An element to use as the container.      Selector: A selector specifying an element to use as the container.      String: A string identifying an element to use as the container. Possible values: ""parent"", ""document"", ""window"".")> _
    Public Property Containment As String
        Get
            Return _containment
        End Get
        Set(value As String)
            _containment = value
        End Set
    End Property

    Private _cursor As String = ""

    ''' <summary>
    ''' Defines the cursor that is being shown while sorting.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Defines the cursor that is being shown while sorting.")> _
    Public Property Cursor As String
        Get
            Return _cursor
        End Get
        Set(value As String)
            _cursor = value
        End Set
    End Property

    Public Enum CursorPosition
        none
        top
        left
        right
        bottom
    End Enum

    ''' <summary>
    ''' Moves the sorting element or helper so the 
    ''' cursor always appears to drag from the same position. 
    ''' Coordinates can be given as a hash using a combination of one or two keys
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Moves the sorting element or helper so the    cursor always appears to drag from the same position.    Coordinates can be given as a hash using a combination of one or two keys")> _
    Public ReadOnly CursorAt As New List(Of CursorPosition)(2)

    Private _delay As Integer = 0

    ''' <summary>
    ''' Time in milliseconds to define when the sorting should start. 
    ''' Adding a delay helps preventing unwanted drags when clicking on an element.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Time in milliseconds to define when the sorting should start.    Adding a delay helps preventing unwanted drags when clicking on an element.")> _
    Public Property Delay As Integer
        Get
            Return _delay
        End Get
        Set(value As Integer)
            _delay = value
        End Set
    End Property

    Private _distance As Integer = 1

    ''' <summary>
    ''' Tolerance, in pixels, for when sorting should start. If specified, 
    ''' sorting will not start until after mouse is dragged beyond distance. 
    ''' Can be used to allow for clicks on elements within a handle.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Tolerance, in pixels, for when sorting should start. If specified,    sorting will not start until after mouse is dragged beyond distance.    Can be used to allow for clicks on elements within a handle.")> _
    Public Property Distance As Integer
        Get
            Return _distance
        End Get
        Set(value As Integer)
            _distance = value
        End Set
    End Property

    Private _droponempty As Boolean = True

    ''' <summary>
    ''' If false, items from this sortable can't be dropped on an empty 
    ''' connect sortable (see the connectWith option.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If false, items from this sortable can't be dropped on an empty    connect sortable (see the connectWith option.")> _
    Public Property DropOnEmpty As Boolean
        Get
            Return _droponempty
        End Get
        Set(value As Boolean)
            _droponempty = value
        End Set
    End Property

    Private _forceHelperSize As Boolean = False

    ''' <summary>
    ''' If true, forces the placeholder to have a size.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If true, forces the placeholder to have a size.")> _
    Public Property ForceHelperSize As Boolean
        Get
            Return _forceHelperSize
        End Get
        Set(value As Boolean)
            _forceHelperSize = value
        End Set
    End Property

    Private _forcePlaceholderSize As Boolean = False

    ''' <summary>
    ''' If true, forces the placeholder to have a size.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If true, forces the placeholder to have a size.")> _
    Public Property ForcePlaceholderSize As Boolean
        Get
            Return _forcePlaceholderSize
        End Get
        Set(value As Boolean)
            _forcePlaceholderSize = value
        End Set
    End Property

    ''' <summary>
    ''' Snaps the sorting element or helper to a grid, every x and y pixels. Array values: [ x, y ].
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Snaps the sorting element or helper to a grid, every x and y pixels. Array values: [ x, y ].")> _
    Public ReadOnly Grid As New List(Of Integer)(2)

    Private _handle As String = ""

    ''' <summary>
    ''' Restricts sort start click to the specified element.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Restricts sort start click to the specified element.")> _
    Public Property Handle As String
        Get
            Return _handle
        End Get
        Set(value As String)
            _handle = value
        End Set
    End Property

    Private _helper As String = ""

    ''' <summary>
    ''' Allows for a helper element to be used for dragging display.
    ''' 
    ''' Multiple types supported:
    '''      String: If set to "clone", then the element will be cloned and the clone will be dragged.
    '''      Function: A function that will return a DOMElement to use while dragging. The function receives the event and the element being sorted.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Allows for a helper element to be used for dragging display.      Multiple types supported:      String: If set to ""clone"", then the element will be cloned and the clone will be dragged.      Function: A function that will return a DOMElement to use while dragging. The function receives the event and the element being sorted.")> _
    Public Property Helper As String
        Get
            Return _helper
        End Get
        Set(value As String)
            _helper = value
        End Set
    End Property

    Private _items As String = ""

    ''' <summary>
    ''' Specifies which items inside the element should be sortable.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies which items inside the element should be sortable.")> _
    Public Property ItemsSelector As String
        Get
            Return _items
        End Get
        Set(value As String)
            _items = value
        End Set
    End Property

    Private _opacity As Double = 0

    ''' <summary>
    ''' Defines the opacity of the helper while sorting. From 0.01 to 1.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Defines the opacity of the helper while sorting. From 0.01 to 1.")> _
    Public Property Opacity As Double
        Get
            Return Math.Round(_opacity, 2)
        End Get
        Set(value As Double)
            If value >= 0 AndAlso value <= 1 Then _opacity = value
        End Set
    End Property

    Private _placeHolder As String = ""

    ''' <summary>
    ''' A class name that gets applied to the otherwise white space.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A class name that gets applied to the otherwise white space.")> _
    Public Property PlaceHolder As String
        Get
            Return _placeHolder
        End Get
        Set(value As String)
            _placeHolder = value
        End Set
    End Property

    Private _revert As Integer = -1

    ''' <summary>
    ''' The duration, in milliseconds, of sortable items reverting to their new positions using a smooth animation.
    ''' -1 to disable, 0 for default
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The duration, in milliseconds, of sortable items reverting to their new positions using a smooth animation.   -1 to disable, 0 for default")> _
    Public Property Revert As Integer
        Get
            Return _revert
        End Get
        Set(value As Integer)
            _revert = value
        End Set
    End Property

    Private _scroll As Boolean = True

    ''' <summary>
    ''' If set to true, the page scrolls when coming to an edge.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, the page scrolls when coming to an edge.")> _
    Public Property Scroll As Boolean
        Get
            Return _scroll
        End Get
        Set(value As Boolean)
            _scroll = value
        End Set
    End Property

    Private _scrollSensitivity As Integer = 20

    ''' <summary>
    ''' Defines how near the mouse must be to an edge to start scrolling.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Defines how near the mouse must be to an edge to start scrolling.")> _
    Public Property ScrollSensitivity As Integer
        Get
            Return _scrollSensitivity
        End Get
        Set(value As Integer)
            _scrollSensitivity = value
        End Set
    End Property

    Private _ScrollSpeed As Integer = 20

    ''' <summary>
    ''' The speed at which the window should scroll once the mouse pointer gets within the scrollSensitivity distance.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The speed at which the window should scroll once the mouse pointer gets within the scrollSensitivity distance.")> _
    Public Property ScrollSpeed As Integer
        Get
            Return _ScrollSpeed
        End Get
        Set(value As Integer)
            _ScrollSpeed = value
        End Set
    End Property

    Public Enum ToleranceMode
        ''' <summary>
        ''' The item overlaps the other item by at least 50%.
        ''' </summary>
        ''' <remarks></remarks>
        intersect

        ''' <summary>
        ''' The mouse pointer overlaps the other item.
        ''' </summary>
        ''' <remarks></remarks>
        pointer
    End Enum

    Private _tolerance As ToleranceMode = ToleranceMode.intersect

    ''' <summary>
    ''' Specifies which mode to use for testing whether the item being moved is hovering over another item. Possible values:
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies which mode to use for testing whether the item being moved is hovering over another item. Possible values:")> _
    Public Property Tolerance As ToleranceMode
        Get
            Return _tolerance
        End Get
        Set(value As ToleranceMode)
            _tolerance = value
        End Set
    End Property

    Private _zIndex As Integer = 1000

    ''' <summary>
    ''' Z-index for element/helper while being sorted.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Z-index for element/helper while being sorted.")> _
    Public Property zIndex As Integer
        Get
            Return _zIndex
        End Get
        Set(value As Integer)
            _zIndex = value
        End Set
    End Property
#End Region

#Region "callbacks"
    Private _onactivate As String = ""

    ''' <summary>
    ''' This event is triggered when using connected lists, 
    ''' every connected list on drag start receives it.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when using connected lists,    every connected list on drag start receives it.")> _
    Public Property onActivateCallback As String
        Get
            Return _onactivate
        End Get
        Set(value As String)
            _onactivate = value
        End Set
    End Property

    Private _onbeforestop As String = ""

    ''' <summary>
    ''' This event is triggered when sorting stops, but when the placeholder/helper is still available.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when sorting stops, but when the placeholder/helper is still available.")> _
    Public Property onBeforeStopCallback As String
        Get
            Return _onbeforestop
        End Get
        Set(value As String)
            _onbeforestop = value
        End Set
    End Property

    Private _onchange As String = ""

    ''' <summary>
    ''' This event is triggered during sorting, but only when the DOM position has changed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered during sorting, but only when the DOM position has changed.")> _
    Public Property onChangeCallback As String
        Get
            Return _onchange
        End Get
        Set(value As String)
            _onchange = value
        End Set
    End Property

    Private _oncreate As String = ""

    ''' <summary>
    ''' Triggered when the sortable is created.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Triggered when the sortable is created.")> _
    Public Property onCreateCallback As String
        Get
            Return _oncreate
        End Get
        Set(value As String)
            _oncreate = value
        End Set
    End Property

    Private _ondeactivate As String = ""

    ''' <summary>
    ''' his event is triggered when sorting was stopped, is propagated to all possible connected lists.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("his event is triggered when sorting was stopped, is propagated to all possible connected lists.")> _
    Public Property onDeactivateCallback As String
        Get
            Return _ondeactivate
        End Get
        Set(value As String)
            _ondeactivate = value
        End Set
    End Property

    Private _onout As String = ""

    ''' <summary>
    ''' This event is triggered when a sortable item is moved away from a connected list.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when a sortable item is moved away from a connected list.")> _
    Public Property onOutCallback As String
        Get
            Return _onout
        End Get
        Set(value As String)
            _onout = value
        End Set
    End Property

    Private _onover As String = ""

    ''' <summary>
    ''' This event is triggered when a sortable item is moved into a connected list.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when a sortable item is moved into a connected list.")> _
    Public Property onOverCallback As String
        Get
            Return _onover
        End Get
        Set(value As String)
            _onover = value
        End Set
    End Property

    Private _onrecieve As String = ""

    ''' <summary>
    ''' This event is triggered when a connected sortable list has received an item from another list.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when a connected sortable list has received an item from another list.")> _
    Public Property onReceiveCallback As String
        Get
            Return _onrecieve
        End Get
        Set(value As String)
            _onrecieve = value
        End Set
    End Property

    Private _onRemove As String = ""

    ''' <summary>
    ''' This event is triggered when a sortable item has been dragged out from the list and into another.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when a sortable item has been dragged out from the list and into another.")> _
    Public Property onRemoveCallback As String
        Get
            Return _onRemove
        End Get
        Set(value As String)
            _onRemove = value
        End Set
    End Property

    Private _onsort As String = ""

    ''' <summary>
    ''' This event is triggered during sorting.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered during sorting.")> _
    Public Property onSortCallback As String
        Get
            Return _onsort
        End Get
        Set(value As String)
            _onsort = value
        End Set
    End Property

    Private _onstart As String = ""

    ''' <summary>
    ''' This event is triggered when sorting starts.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when sorting starts.")> _
    Public Property onstartCallback As String
        Get
            Return _onstart
        End Get
        Set(value As String)
            _onstart = value
        End Set
    End Property

    Private _onstopcallback As String = ""

    ''' <summary>
    ''' This event is triggered when sorting has stopped.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when sorting has stopped.")> _
    Public Property onStopCallback As String
        Get
            Return _onstopcallback
        End Get
        Set(value As String)
            _onstopcallback = value
        End Set
    End Property

    Private _onupdate As String = ""

    ''' <summary>
    ''' This event is triggered when the user stopped sorting and the DOM position has changed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This event is triggered when the user stopped sorting and the DOM position has changed.")> _
    Public Property onUpdateCallback As String
        Get
            Return _onupdate
        End Get
        Set(value As String)
            _onupdate = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""

        'Properties
        If AppendTo <> "" Then
            outstr &= "appendTo: '" & AppendTo & "',"
        End If
        If Axis <> Axes.none Then
            outstr &= "axis: '" & DatePicker.getEnumName(Axis) & "',"
        End If
        If Cancel <> "" Then
            outstr &= "cancel: '" & Cancel & "',"
        End If
        If ConnectWith <> "" Then
            outstr &= "connectWith: '" & ConnectWith & "',"
        End If
        If Containment <> "" Then
            outstr &= "containment: '" & Containment & "',"
        End If
        If Cursor <> "" Then
            outstr &= "cursor: '" & Cursor & "',"
        End If
        If CursorAt.Count > 0 Then
            outstr &= "cursorAt: {"
            For Each cur As CursorPosition In CursorAt
                outstr &= DatePicker.getEnumName(cur) & ","
            Next
            outstr = outstr.TrimEnd(",")
            outstr &= "},"
        End If
        If Delay > 0 Then
            outstr &= "delay: " & Delay & ","
        End If
        If Not Enabled Then
            outstr &= "diabled: true,"
        End If
        If Distance > 1 Then
            outstr &= "distance: " & Distance & ","
        End If
        If Not DropOnEmpty Then
            outstr &= "dropOnEmpty: false,"
        End If
        If ForceHelperSize Then
            outstr &= "forceHelperSize: true,"
        End If
        If ForcePlaceholderSize Then
            outstr &= "forcePlaceholderSize: true,"
        End If
        If Grid.Count > 0 Then
            outstr &= "grid: ["
            For Each ax As Axes In Grid
                outstr &= DatePicker.getEnumName(ax) & ","
            Next
            outstr = outstr.TrimEnd(",")
            outstr &= "],"
        End If
        If Handle <> "" Then
            outstr &= "handle: '" & Handle & "',"
        End If
        If Helper <> "" Then
            outstr &= "helper: '" & Helper & "',"
        End If
        If ItemsSelector <> "" Then
            outstr &= "items: '" & ItemsSelector & "',"
        End If
        If Opacity > 0 Then
            outstr &= "opacity: " & Opacity & ","
        End If
        If PlaceHolder <> "" Then
            outstr &= "placeholder: '" & PlaceHolder & "',"
        End If
        If Revert > -1 Then
            outstr &= "revert: "
            If Revert = 0 Then
                outstr &= "true"
            Else
                outstr &= Revert
            End If
            outstr &= ","
        End If
        If Not Scroll Then
            outstr &= "scroll: false,"
        End If
        If ScrollSensitivity <> 20 Then
            outstr &= "scrollSensitivity: " & ScrollSensitivity & ","
        End If
        If ScrollSpeed <> 20 Then
            outstr &= "scrollSpeed: " & ScrollSpeed & ","
        End If
        If Tolerance <> ToleranceMode.intersect Then
            outstr &= "tolerance: 'pointer',"
        End If
        If zIndex <> 1000 Then
            outstr &= "zIndex: " & zIndex & ","
        End If

        'Callbacks
        If onActivateCallback <> "" Then
            outstr &= "activate: function(event,ui){" & onActivateCallback & "},"
        End If
        If onBeforeStopCallback <> "" Then
            outstr &= "beforeStop: function(event,ui){" & onBeforeStopCallback & "},"
        End If
        If onChangeCallback <> "" Then
            outstr &= "change: function(event,ui){" & onChangeCallback & "},"
        End If
        If onCreateCallback <> "" Then
            outstr &= "create: function(event,ui){" & onCreateCallback & "},"
        End If
        If onDeactivateCallback <> "" Then
            outstr &= "deactivate: function(event,ui){" & onDeactivateCallback & "},"
        End If
        If onOutCallback <> "" Then
            outstr &= "out: function(event,ui){" & onOutCallback & "},"
        End If
        If onOverCallback <> "" Then
            outstr &= "over: function(event,ui){" & onOverCallback & "},"
        End If
        If onReceiveCallback <> "" Then
            outstr &= "receive: function(event,ui){" & onReceiveCallback & "},"
        End If
        If onRemoveCallback <> "" Then
            outstr &= "remove: function(event,ui){" & onRemoveCallback & "},"
        End If
        If onSortCallback <> "" Then
            outstr &= "sort: function(event,ui){" & onSortCallback & "},"
        End If
        If onstartCallback <> "" Then
            outstr &= "start: function(event,ui){" & onstartCallback & "},"
        End If
        If onStopCallback <> "" Then
            outstr &= "stop: function(event,ui){" & onStopCallback & "},"
        End If
        If onUpdateCallback <> "" Then
            outstr &= "update: function(event,ui){" & onUpdateCallback & "},"
        End If

        outstr = outstr.TrimEnd(",")
        If outstr <> "" Then
            outstr = "{" & outstr & "}"
        End If
        Return outstr
    End Function

    Private Sub Sortable_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim s As String = ""
        s &= "$(function(){"
        s &= "$('#" & Me.ClientID & "').sortable("
        s &= renderparams()
        s &= "      );"
        s &= "});"
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub
End Class