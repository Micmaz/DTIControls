Imports DTIServerControls

''' <summary>
''' Contained in the admin panel, this displays deleted elements from a page. 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Contained in the admin panel, this displays deleted elements from a page."),ToolboxData("<{0}:DTIRecycleBin ID=""RecycleBin"" runat=""server""> </{0}:DTIRecycleBin>"),ComponentModel.ToolboxItem(False)> _
Public Class DTIRecycleBin
    Inherits DTIServerBase

#Region "Properties"

    Private Sub removeSortableServerList()
        Session.Remove("SortableServerTempList")
    End Sub

    ''' <summary>
    ''' Enables Layout mode for all sortables, Menu's and recyclebins
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enables Layout mode for all sortables, Menu's and recyclebins")> _
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
                _LayoutOn = True
            End If
            Return _LayoutOn
        End Get
        Set(ByVal value As Boolean)
            _LayoutOn = value
        End Set
    End Property

    Private Property isRecyclable() As Boolean
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

    Private _controlist As New Collections.Specialized.OrderedDictionary
    Public Property ControlList() As Collections.Specialized.OrderedDictionary
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

    Public Enum Axis As Integer
        none = 0
        x = 1
        y = 2
    End Enum

    Private _AxisRestriction As Axis
    Public Property AxisRestriction() As Axis
        Get
            Return _AxisRestriction
        End Get
        Set(ByVal value As Axis)
            _AxisRestriction = value
        End Set
    End Property

    Private _ConnectByClass As Boolean = True
    Public Property ConnectByClass() As Boolean
        Get
            Return _ConnectByClass
        End Get
        Set(ByVal value As Boolean)
            _ConnectByClass = value
        End Set
    End Property

    Private _ConnectedCSS As String
    Public Property ConnectedCSS() As String
        Get
            Return _ConnectedCSS
        End Get
        Set(ByVal value As String)
            _ConnectedCSS = value
        End Set
    End Property

    Private _ConnectToID As String
    Public Property ConnectToID() As String
        Get
            Return _ConnectToID
        End Get
        Set(ByVal value As String)
            _ConnectToID = value
        End Set
    End Property

    Private _PlaceHolderClass As String
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
    Public Property CursorPosition() As CursorAt
        Get
            Return _CursorPosition
        End Get
        Set(ByVal value As CursorAt)
            _CursorPosition = value
        End Set
    End Property

    Private _CursorPositionValue As Integer
    Public Property CursorPositionValue() As Integer
        Get
            Return _CursorPositionValue
        End Get
        Set(ByVal value As Integer)
            _CursorPositionValue = value
        End Set
    End Property

    Private _DragOnEmpty As Boolean
    Public Property DragOnEmpty() As Boolean
        Get
            Return _DragOnEmpty
        End Get
        Set(ByVal value As Boolean)
            _DragOnEmpty = value
        End Set
    End Property

    Public Enum Helper As Integer
        original = 0
        clone = 1
    End Enum

    Private _HelperType As Helper
    Public Property HelperType() As Helper
        Get
            Return _HelperType
        End Get
        Set(ByVal value As Helper)
            _HelperType = value
        End Set
    End Property

    Private _HandleText As String
    Public Property HandleText() As String
        Get
            Return _HandleText
        End Get
        Set(ByVal value As String)
            _HandleText = value
        End Set
    End Property

    Private _numberofItems As Integer = 10
    Public Property NumberOfItems() As Integer
        Get
            Return _numberofItems
        End Get
        Set(ByVal value As Integer)
            _numberofItems = value
        End Set
    End Property

    Private ReadOnly Property page_Id() As String
        Get
            Return Page.Request.RawUrl
        End Get
    End Property

    Private _cssTheme As String = "default"
    Public Property CssTheme() As String
        Get
            Return _cssTheme
        End Get
        Set(ByVal value As String)
            _cssTheme = value
        End Set
    End Property

    Private _recycleBinText As String = "Recycle Bin"
    Public Property RecycleBinText() As String
        Get
            Return _recycleBinText
        End Get
        Set(ByVal value As String)
            _recycleBinText = value
        End Set
    End Property

#End Region

    Private HiddenField1 As New HiddenField
    Private options As String = ""
    Private SortableList As New Panel
    Private doRedirect As Boolean = False

    Private Sub DTIWidgetMenu_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If LayoutOn Then
            loadWidgetMenu()
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(Me.Page)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTISortable/DTISortable.css", "text/css")
            isRecyclable = True
        End If
    End Sub

    Private Sub loadWidgetMenu()
        If LayoutOn Then
            loadData()

            Dim hasHandle As Boolean = False
            Dim OuterDiv As New Panel
            OuterDiv.CssClass = "DTIRecycleBin-" & CssTheme

            SortableList.ID = Me.ClientID & "_SortableList"
            HiddenField1.ID = Me.ClientID & "_HiddenField1"

            HiddenField1.Value = ""
            options = "cursor:'" & DTISortable.getEnumName(CursorType) & "',"
            If CursorPosition <> DTISortable.CursorAt.false OrElse CursorPositionValue > 0 Then
                options &= "cursorAt:{" & DTISortable.getEnumName(CursorPosition) & ":" & CursorPositionValue & "},"
            End If

            If CssClass = "" Then
                CssClass = "DTIRecycleBinOuter-" & CssTheme
            End If
            If ConnectedCSS = "" Then
                ConnectedCSS = "DTIConnectedSortable-" & CssTheme
            End If

            SortableList.CssClass = ConnectedCSS

            If ConnectByClass = True Then
                options &= "connectWith:'." & ConnectedCSS & "',"
            ElseIf ConnectToID IsNot Nothing Then
                options &= "connectWith:'#" & ConnectToID & "',"
            End If
            If PlaceHolderClass Is Nothing Then
                PlaceHolderClass = "DTIPlaceHolder-" & CssTheme
            End If

            options &= "placeholder:'" & PlaceHolderClass & "',"

            If AxisRestriction <> Axis.none Then
                options &= "axis:'" & DTISortable.getEnumName(AxisRestriction) & "',"
            End If
            If DragOnEmpty = True Then
                options &= "dropOnEmpty:true,"
            End If
            If HandleText <> "" Then
                hasHandle = True
                options &= "handle: '.DTIItemHandle-" & CssTheme & "',"
            End If
            If HelperType <> Helper.original Then
                options &= "helper:'clone'"
            End If

            Me.Controls.Add(HiddenField1)
            OuterDiv.Controls.Add(New LiteralControl("<div class=""DTIRecycleBinHandle-" & CssTheme & """ style=""cursor:move;width:100%"">" & RecycleBinText & "</div>"))
            For Each item As DictionaryEntry In ControlList
                SortableList.Controls.Add(New LiteralControl("<div id=""DTIRecycle_" & item.Key & """ class=""DTIRecycledItem-" & CssTheme & """>"))
                If hasHandle Then
                    SortableList.Controls.Add(New LiteralControl("<div class=""DTIItemHandle-" & CssTheme & """>" & HandleText & "</div>"))
                End If
                SortableList.Controls.Add(item.Value)
                SortableList.Controls.Add(New LiteralControl("</div>"))
            Next
            OuterDiv.Controls.Add(SortableList)
            Me.Controls.Add(OuterDiv)
        End If
    End Sub

    Private Sub loadData()
        Me.ControlList = Nothing

        If ds.DTISortable.Count > 0 Then
            Dim SortableFilter As String = "("
            Dim i As Integer = 1
            For Each sortrow As dsDTISortable.DTISortableRow In ds.DTISortable
                SortableFilter &= "DTISortable_Id=" & sortrow.Id
                If i <> ds.DTISortable.Count Then SortableFilter &= " Or "
                i += 1
            Next
            SortableFilter &= ")"

            Dim dvSortableItem As New DataView(ds.DTISortableItem, SortableFilter & "AND isDeleted=1 AND (Page_Id is null Or Page_Id = '" & Page.Request.RawUrl & "')", "DeleteDate desc", DataViewRowState.CurrentRows)
            Dim j As Integer = 0
            If dvSortableItem.Count > 0 Then
                For Each row As DataRowView In dvSortableItem
                    Dim name As String = row("Assembly_Name")
                    Dim disName As String = name.Substring(name.LastIndexOf(".") + 1)
                    Me.ControlList.Add(row.Item("Id"), New LiteralControl("Deleted: " & row("DeleteDate") & " " & disName & ""))
                    j += 1
                    If j = NumberOfItems Then Exit For
                Next
            End If
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
        Dim update As Boolean = False
        Dim arr() As String = HiddenField1.Value.Trim(",").Split(",")

        If arr IsNot Nothing AndAlso arr.Length > 0 AndAlso Not String.IsNullOrEmpty(arr(0)) Then
            For Each s As String In arr
                If Not String.IsNullOrEmpty(s) AndAlso ds.WidgetMenuTemp.Count > 0 Then
                    If s.StartsWith("DTISortItem_") Then
                        Dim id As Integer = Integer.Parse(s.Replace("DTISortItem_", ""))
                        Dim sortItem As dsDTISortable.DTISortableItemRow = ds.DTISortableItem.FindById(id)
                        If sortItem IsNot Nothing Then
                            sortItem.DeleteDate = Now
                            sortItem.isDeleted = True
                            update = True
                        End If
                    End If
                End If
            Next
        End If

        If update AndAlso SaveData Then _
           doUpdate = True
        If forceRedirect Then _
            doRedirect = True
    End Sub

    Private Sub DTISortable_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If doUpdate Then _
             sqlhelper.Update(ds.DTISortableItem)
        If doRedirect Then _
            Page.Response.Redirect(Page.Request.Url.OriginalString)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If LayoutOn Then
            writer.Write("<script type=""text/javascript""> " & vbCrLf & _
                        "	$(function() { " & vbCrLf & _
                        "		// here, we allow the user to sort the items " & vbCrLf & _
                        "		$('#" & SortableList.ClientID() & "').sortable({ " & vbCrLf & _
                        "			" & options & vbCrLf & _
                        "			update: function() {document.getElementById('" & HiddenField1.ClientID() & "').value = $('#" & SortableList.ClientID() & "').sortable(""toArray"");} " & vbCrLf & _
                        "		}); " & vbCrLf & _
                        "	});	 " & vbCrLf & _
                        "   function DeleteRecycledBinItem(DeleteItemId){" & vbCrLf & _
                        "       $('#' + DeleteItemId).remove().appendTo('#" & SortableList.ClientID & "');" & vbCrLf & _
                        "       ToggleDisplay(DeleteItemId.replace('DTISortItem_',''));" & vbCrLf & _
                        "       document.getElementById('" & HiddenField1.ClientID() & "').value = $('#" & SortableList.ClientID() & "').sortable(""toArray"");" & vbCrLf & _
                        "   }" & vbCrLf & _
                        "</script> ")
        End If

        MyBase.Render(writer)
    End Sub
End Class

