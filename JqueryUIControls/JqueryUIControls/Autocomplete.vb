Public Class Autocomplete
    Inherits TextBox

#Region "properties"
    Private _delay As Integer = 300

    ''' <summary>
    ''' The delay in milliseconds the Autocomplete waits after a keystroke to activate itself. 
    ''' A zero-delay makes sense for local data (more responsive), but can produce a lot of 
    ''' load for remote data, while being less responsive
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The delay in milliseconds the Autocomplete waits after a keystroke to activate itself.    A zero-delay makes sense for local data (more responsive), but can produce a lot of    load for remote data, while being less responsive")> _
    Public Property Delay() As Integer
        Get
            Return _delay
        End Get
        Set(ByVal value As Integer)
            _delay = value
        End Set
    End Property

    Private _autopostback As Boolean = False
    Public Shadows Property AutoPostBack() As Boolean
        Get
            Return _autopostback
        End Get
        Set(ByVal value As Boolean)
            _autopostback = value
            MyBase.AutoPostBack = False
        End Set
    End Property

    Private _minLength As Integer = 1

    ''' <summary>
    ''' The minimum number of characters a user has to type before the Autocomplete activates. 
    ''' Zero is useful for local data with just a few items. Should be increased when there are 
    ''' a lot of items, where a single character would match a few thousand items
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The minimum number of characters a user has to type before the Autocomplete activates.    Zero is useful for local data with just a few items. Should be increased when there are    a lot of items, where a single character would match a few thousand items")> _
    Public Property MinLength() As Integer
        Get
            Return _minLength
        End Get
        Set(ByVal value As Integer)
            _minLength = value
        End Set
    End Property

    Private _autofocus As Boolean = False

    ''' <summary>
    ''' If set to true the first item will be automatically focused
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true the first item will be automatically focused")> _
    Public Property AutoFocus() As Boolean
        Get
            Return _autofocus
        End Get
        Set(ByVal value As Boolean)
            _autofocus = value
        End Set
    End Property

    Private _position As String = ""

    ''' <summary>
    ''' Identifies the position of the Autocomplete widget in relation to the associated input 
    ''' element. The "of" option defaults to the input element, but you can specify another 
    ''' element to position against.
    ''' <see>http://docs.jquery.com/UI/Position</see>
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Identifies the position of the Autocomplete widget in relation to the associated input    element. The ""of"" option defaults to the input element, but you can specify another    element to position against.   <see>http://docs.jquery.com/UI/Position</see>")> _
    Public Property Position() As String
        Get
            Return _position
        End Get
        Set(ByVal value As String)
            _position = value
        End Set
    End Property

    Private _returnedParms As Hashtable

    ''' <summary>
    ''' Stores values from other controls for the search event
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Stores values from other controls for the search event")> _
    Public ReadOnly Property returnedParms() As Hashtable
        Get
            If _returnedParms Is Nothing Then _returnedParms = New Hashtable
            Return _returnedParms
        End Get
    End Property

    Private _watchedControlList As New List(Of Control)
    Public ReadOnly Property watchedControlList() As List(Of Control)
        Get
            Return _watchedControlList
        End Get
    End Property


    Private _myvalue As String = Nothing

    ''' <summary>
    ''' Gets the value of the selected item
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the value of the selected item")> _
    Public Property Value() As String
        Get
            If Me.DesignMode Then Return Nothing
            If _myvalue is Nothing and Not Me.Page.Request.Params(Me.ClientID & "_hidden") Is Nothing Then
                _myvalue = Me.Page.Request.Params(Me.ClientID & "_hidden")
            End If
            Return _myvalue
        End Get
        Set(ByVal value As String)
            _myvalue = value
        End Set
    End Property

    Private _source As String = ""
    Public Property Source() As String
        Get
            If Me.DesignMode Then Return _source
            If _source = "" Then
                If Page.Request.QueryString.Count > 0 AndAlso Page.Request.QueryString("ajaxCtrl") Is Nothing Then
                    _source = """" & Page.Request.Url.PathAndQuery & "&ajaxCtrl=" & Me.ClientID & """"
                Else
                    _source = """" & Page.Request.Url.AbsolutePath & "?ajaxCtrl=" & Me.ClientID & """"
                End If
            End If
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property

    Private _EnableCategories As Boolean = False

    ''' <summary>
    ''' Allows categories to be shwon in search results
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Allows categories to be shwon in search results")> _
    Public Property EnableCategories As Boolean
        Get
            Return _EnableCategories
        End Get
        Set(value As Boolean)
            _EnableCategories = value
        End Set
    End Property

    Private ReadOnly Property query() As String
        Get
            If Me.DesignMode Then Return Nothing
            If Page Is Nothing Then Return Nothing
            Return Page.Request.QueryString("term")
        End Get
    End Property

    Private ReadOnly Property ctrlid() As String
        Get
            If Me.DesignMode Then Return Nothing
            If Page Is Nothing Then Return Nothing
            Return Page.Request.QueryString("ajaxCtrl")
        End Get
    End Property

Public ReadOnly Property tableName as String
        Get
            Return acTables(Me.UniqueID & "_Table")
        End Get
End Property
Public ReadOnly Property selectStmt as String
        Get
            Return acTables(Me.UniqueID & "_SelectStmt")
        End Get
End Property
Public ReadOnly Property columnName as String
        Get
            Return acTables(Me.UniqueID & "_Col") 
        End Get
End Property
Public ReadOnly Property numberReturned as Integer
        Get
            Return acTables(Me.UniqueID & "_Num")
        End Get
End Property


#End Region

#Region "callbacks"
    Private _searchCallback As String

    ''' <summary>
    ''' Before a request (source-option) is started, after minLength and delay are met. 
    ''' Can be canceled (return false), then no request will be started and no items suggested.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Before a request (source-option) is started, after minLength and delay are met.    Can be canceled (return false), then no request will be started and no items suggested.")> _
    Public Property SearchCallback() As String
        Get
            Return _searchCallback
        End Get
        Set(ByVal value As String)
            _searchCallback = value
        End Set
    End Property

    Private _openCallback As String

    ''' <summary>
    ''' Triggered when the suggestion menu is opened.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Triggered when the suggestion menu is opened.")> _
    Public Property OpenCallback() As String
        Get
            Return _openCallback
        End Get
        Set(ByVal value As String)
            _openCallback = value
        End Set
    End Property

    Private _focusCallback As String

    ''' <summary>
    ''' Before focus is moved to an item (not selecting), ui.item refers to the focused item. 
    ''' The default action of focus is to replace the text field's value with the value of 
    ''' the focused item, though only if the focus event was triggered by a keyboard interaction. 
    ''' Canceling this event prevents the value from being updated, but does not prevent the menu 
    ''' item from being focused
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Before focus is moved to an item (not selecting), ui.item refers to the focused item.    The default action of focus is to replace the text field's value with the value of    the focused item, though only if the focus event was triggered by a keyboard interaction.    Canceling this event prevents the value from being updated, but does not prevent the menu    item from being focused")> _
    Public Property FocusCallback() As String
        Get
            Return _focusCallback
        End Get
        Set(ByVal value As String)
            _focusCallback = value
        End Set
    End Property

    Private _selectCallback As String

    ''' <summary>
    ''' Triggered when an item is selected from the menu; ui.item refers to the selected item. 
    ''' The default action of select is to replace the text field's value with the value of 
    ''' the selected item. Canceling this event prevents the value from being updated, but does 
    ''' not prevent the menu from closing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Triggered when an item is selected from the menu; ui.item refers to the selected item.    The default action of select is to replace the text field's value with the value of    the selected item. Canceling this event prevents the value from being updated, but does    not prevent the menu from closing")> _
    Public Property SelectCallback() As String
        Get
            Return _selectCallback
        End Get
        Set(ByVal value As String)
            _selectCallback = value
        End Set
    End Property

    Private _changeCallback As String

    ''' <summary>
    ''' After an item was selected; ui.item refers to the selected item. Always triggered
    '''  after the close event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("After an item was selected; ui.item refers to the selected item. Always triggered    after the close event.")> _
    Public Property ChangeCallback() As String
        Get
            Return _changeCallback
        End Get
        Set(ByVal value As String)
            _changeCallback = value
        End Set
    End Property

    Private _closeCallback As String

    ''' <summary>
    ''' When the list is hidden - doesn't have to occur together with a change.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("When the list is hidden - doesn't have to occur together with a change.")> _
    Public Property CloseCallback() As String
        Get
            Return _closeCallback
        End Get
        Set(ByVal value As String)
            _closeCallback = value
        End Set
    End Property
#End Region

    Public Event Search(ByVal sender As Autocomplete, ByVal query As String)
    Public Event Selected(ByVal sender As Autocomplete)

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.autoCompleteHelper.js")
        End If
    End Sub

    Private Sub Autocomplete_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not acTablesHashTmp Is Nothing Then
            For Each key As String In acTablesHashTmp.Keys
                Dim newkey As String = key
                If key.StartsWith("_") Then
                    newkey = Me.UniqueID & key
                End If
                acTablesHash(newkey) = acTablesHashTmp(key)
            Next
        End If
        checkpost()
        registerControl(Me.Page)
    End Sub

    Public Sub addControlsToWatchList(ByVal ParamArray controls As Control())
        For Each ctrl As Control In controls
            Me.watchedControlList.Add(ctrl)
        Next
    End Sub

    Private Sub checkpost()
        If Me.DesignMode Then Return
        If Not Page.IsPostBack AndAlso Not query Is Nothing AndAlso ctrlid = Me.ClientID Then
            For Each key As String In Me.Page.Request.QueryString.Keys
                If Not key = "ajaxCtrl" AndAlso Not key = "term" andalso key IsNot Nothing Then
                    Me.returnedParms.Add(key, Me.Page.Request.QueryString(key))
                    Try
                        Dim ctrl As Control = Me.FindControl(key)
                        If ctrl.GetType().GetProperty("Text") IsNot Nothing Then
                            ctrl.GetType().GetProperty("Text").SetValue(ctrl, Me.Page.Request.QueryString(key), Nothing)
                        End If
                        If ctrl.GetType().GetProperty("Value") IsNot Nothing Then
                            ctrl.GetType().GetProperty("Value").SetValue(ctrl, Me.Page.Request.QueryString(key), Nothing)
                        End If
                        If ctrl.GetType().GetProperty("SelectedValue") IsNot Nothing Then
                            ctrl.GetType().GetProperty("SelectedValue").SetValue(ctrl, Me.Page.Request.QueryString(key), Nothing)
                        End If
                        If TypeOf ctrl Is DropDownList Then
                            Dim ctrl1 As DropDownList = ctrl
                            ctrl1.Items.Add(Me.Page.Request.QueryString(key))
                            ctrl1.SelectedValue = Me.Page.Request.QueryString(key)
                        End If
                        'Dim ctrl As Control = Me.Page.GetType().GetMember(key).GetValue(Nothing, Nothing)
                        'ctrl.GetType().GetMember("text").SetValue(Me.Page.Request.QueryString(key), 0)
                    Catch ex As Exception

                    End Try
                End If
            Next
            If acTables(Me.uniqueID & "_Table") Is Nothing Then
                RaiseEvent Search(Me, query)
            Else
                genericAutocompleteHandler(Me, query)
            End If

            Page.Response.End()
        End If
    End Sub

    Private Function renderparams() As String
        Dim outstr As String = ""
        outstr = "source: " & Source & ","
        If Not Enabled Then
            outstr &= "disabled: true,"
        End If
        If AutoFocus Then
            outstr &= "autoFocus: true,"
        End If
        If Delay <> 300 AndAlso Delay >= 0 Then
            outstr &= "delay: " & Delay & ","
        End If
        If MinLength <> 1 AndAlso MinLength >= 0 Then
            outstr &= "minLength: " & MinLength & ","
        End If
        If Position <> "" Then
            outstr &= "position: " & Position & ","
        End If

        Dim autopostStr As String = ""
        If AutoPostBack = True Then
            autopostStr = "$('#" & Me.ClientID & "').val(ui.item.value); $('#" & Me.ClientID & "').closest('form').submit();"
        End If
        For Each ctrl As Control In Me.watchedControlList
            SearchCallback &= "addvalueToAutoComplete('" & ctrl.ClientID & "','" & Me.ClientID & "','" & ctrl.ID & "');"
        Next
        'callbacks
        If SearchCallback <> "" Then
            outstr &= "search: function( event, ui ) {" & SearchCallback & "},"
        End If
        If OpenCallback <> "" Then
            outstr &= "open: function( event, ui ) {" & OpenCallback & "},"
        End If
        If FocusCallback <> "" Then
            outstr &= "focus: function( event, ui ) {" & FocusCallback & "},"
        End If
        If CloseCallback <> "" Then
            outstr &= "close: function( event, ui ) {" & CloseCallback & "},"
        End If
        If ChangeCallback <> "" Then
            outstr &= "change: function( event, ui ) {" & ChangeCallback & "},"
        End If


        outstr &= "select: function( event, ui ) {$('#" & Me.ClientID & "_hidden').val(ui.item.id);$('#" & Me.ClientID & "_hidden2').val('1');" & SelectCallback & ";" & autopostStr & "}"
        outstr = "{" & outstr & "}"
        Return outstr
    End Function

    Private Sub Autocomplete_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.DesignMode Then Return
        If Me.Page.Request.Params(Me.ClientID & "_hidden2") = "1" Then
            RaiseEvent Selected(Me)
        End If
    End Sub

    Private Sub Autocomplete_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim id As String = "" & Me.ID
        If id = "" Then id = ClientID
		Dim s As String = ""

		If EnableCategories Then
            s &= "$.widget( 'custom.catcomplete', $.ui.autocomplete, {"
            s &= "     _renderMenu: function( ul, items ) {"
            s &= "          var that = this, currentCategory = '';"
            s &= "          $.each( items, function( index, item ) {"
            s &= "               if ( item.category != currentCategory ) {"
            s &= "                    ul.append( ""<li class='ui-autocomplete-category'>"" + item.category + ""</li>"" );"
            s &= "                    currentCategory = item.category;"
            s &= "               }"
            s &= "               that._renderItemData( ul, item );"
            s &= "          });"
            s &= "     }"
            s &= "});"
        End If

		If Not EnableCategories Then
			s &= "window." & id & " = $('#" & Me.ClientID & "').autocomplete("
		Else
			s &= "window." & id & " = $('#" & Me.ClientID & "').catcomplete("
		End If
        s &= renderparams()
        s &= "      );"

		jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, s)
	End Sub

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="nameColumn"></param>
    ''' <param name="valuecolumn"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByVal dt As DataTable, ByVal nameColumn As String, Optional ByVal valuecolumn As String = Nothing, _
                       Optional ByVal categorycolumn As String = Nothing)
        Dim outstr As String = "["
        For Each row As DataRow In dt.Rows
            outstr &= "{"
            If valuecolumn IsNot Nothing Then
                outstr &= """value"": """ & row(nameColumn) & ""","
                outstr &= """id"": """ & row(valuecolumn) & """"
            Else
                outstr &= """value"": """ & row(nameColumn) & ""","
                outstr &= """id"": """ & row(nameColumn) & """"
            End If
            If categorycolumn IsNot Nothing Then
                outstr &= ",""category"": """ & row(categorycolumn) & """"
            End If
            outstr &= "},"
        Next
        outstr = outstr.TrimEnd(",") & "]"
        respond(outstr)
    End Sub

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="dv"></param>
    ''' <param name="nameColumn"></param>
    ''' <param name="valuecolumn"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByVal dv As DataView, ByVal nameColumn As String, Optional ByVal valuecolumn As String = Nothing, _
                       Optional ByVal categorycolumn As String = Nothing)
        respond(dv.ToTable, nameColumn, valuecolumn, categorycolumn)
    End Sub

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByVal dt As DataTable)
        Dim outstr As String = "["
        For Each row As DataRow In dt.Rows
            For Each col As DataColumn In dt.Columns
                outstr &= """" & row(col.ColumnName) & ""","
            Next
            outstr = outstr.TrimEnd(",")
        Next
        outstr = outstr.TrimEnd(",") & "]}"
        respond(outstr)
    End Sub

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="dv"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByVal dv As DataView)
        respond(dv.ToTable)
    End Sub

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="JSONDataString"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByRef JSONDataString As String)
        Page.Response.Clear()
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Page.Response.Cache.SetAllowResponseInBrowserHistory(False)
        Page.Response.Write(JSONDataString)
        Page.Response.End()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<input type=""hidden"" value=""" & Value & """ name=""" & Me.ClientID & "_hidden"" id=""" & Me.ClientID & "_hidden"" />")
        writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_hidden2"" id=""" & Me.ClientID & "_hidden2"" />")
        MyBase.Render(writer)
    End Sub

#Region "Setup Autocomplete on a single column"

    Private acTablesHashTmp As Hashtable
    Protected ReadOnly Property acTablesHash() As Hashtable
        Get
            If page Is Nothing Then
                If acTablesHashTmp Is Nothing Then acTablesHashTmp = New hashtable
                Return acTablesHashTmp
            End If
            If Page.Session("acTablesHash") Is Nothing Then
                Page.Session("acTablesHash") = New Hashtable
            End If
            Return Page.Session("acTablesHash")
        End Get
    End Property

    Protected Property acTables(ByVal id As String) As Object
        Get
            Return acTablesHash(id)
        End Get
        Set(ByVal value As Object)
            acTablesHash(id) = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the autocomplete to display the distinct text from the column provided.
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="columnName"></param>
    ''' <param name="numberReturned"></param>
    ''' <param name="searchParmFormat"></param>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the autocomplete to display the distinct text from the column provided.")> _
    Public Sub setDistinctAutocomplete(ByVal tableName As String, ByVal columnName As String, Optional ByVal numberReturned As Integer = 20, Optional ByVal searchParmFormat As String = "{0}%", Optional ByVal connection As System.Data.Common.DbConnection = Nothing)
        acTables(Me.UniqueID & "_Table") = tableName
        acTables(Me.UniqueID & "_Col") = columnName
        acTables(Me.UniqueID & "_Num") = numberReturned
        acTables(Me.UniqueID & "_searchParmFormat") = searchParmFormat
        If Not connection Is Nothing Then _
            acTables(Me.UniqueID & "_connection") = connection
        'acConnection = connection
        'AddHandler Me.Search, AddressOf genericAutocompleteHandler
    End Sub

	''' <summary>
    ''' Sets the connection for setDistinctAutocomplete and setSelectAutocomplete. 
    ''' </summary>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
	public sub setAutocompleteConnection(ByVal connection As System.Data.Common.DbConnection)
		acTables(Me.UniqueID & "_connection") = connection
	end sub

	
    ''' <summary>
    ''' Sets the autocomplete to display results from the provided select statement.
    ''' </summary>
    ''' <param name="SelectStmt">A select statement with a single query parameter.</param>
    ''' <param name="Displaycolumn">Column to display in the autocomplete</param>
    ''' <param name="valueColumn">Value to return once clicked (autopostback must = true)</param>
    ''' <param name="searchParmFormat">{0}%</param>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the autocomplete to display results from the provided select statement.")> _
    Public Sub setSelectAutocomplete(ByVal SelectStmt As String, ByVal displayColumn As String, Optional ByVal valueColumn As String = Nothing, Optional ByVal searchParmFormat As String = "{0}%", Optional ByVal connection As System.Data.Common.DbConnection = Nothing)
        acTables(Me.UniqueID & "_SelectStmt") = SelectStmt
        acTables(Me.UniqueID & "_Table") = "select"
        acTables(Me.UniqueID & "_Col") = displayColumn
        acTables(Me.UniqueID & "_valueColumn") = valueColumn
        acTables(Me.UniqueID & "_searchParmFormat") = searchParmFormat
        If Not connection Is Nothing Then _
            acTables(Me.UniqueID & "_connection") = connection
        'acConnection = connection
        'AddHandler Me.Search, AddressOf genericAutocompleteHandler

    End Sub

    Private acConnection As System.Data.Common.DbConnection

    Protected Sub genericAutocompleteHandler(ByVal ac As JqueryUIControls.Autocomplete, ByVal query As String)
        Dim selectstr As String = ""
        Dim displayColumn As String = acTables(Me.UniqueID & "_Col")
        Dim searchParmFormat As String = acTables(ac.UniqueID & "_searchParmFormat")
        If Not acTables(Me.UniqueID & "_SelectStmt") Is Nothing Then
            selectstr = acTables(Me.UniqueID & "_SelectStmt")
        Else
            Dim tablename As String = acTables(ac.UniqueID & "_Table")
            Dim numberReturned As Integer = acTables(ac.UniqueID & "_Num")
            selectstr = "select distinct top {0} [{1}] from [{2}] where [{1}] like @parm"
            selectstr = String.Format(selectstr, numberReturned, displayColumn.Trim("[").Trim("]"), tablename.Trim("[").Trim("]"))
        End If
        Dim parm As String = String.Format(searchParmFormat, query)
        acConnection = acTables(Me.UniqueID & "_connection")
        Dim dt As DataTable = BaseClasses.DataBase.getHelper().FillDataTable(selectstr, parm, acConnection)
        ac.respond(dt, displayColumn, acTables(Me.UniqueID & "_valueColumn"))
    End Sub

#End Region


End Class
