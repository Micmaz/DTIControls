Imports DTIServerControls
Imports System.Reflection

''' <summary>
''' Menu used in admin panel to add dynamic elements to sortable.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Menu used in admin panel to add dynamic elements to sortable."),ToolboxData("<{0}:DTIWidgetMenu ID='WidgetMenu' runat=""server""> </{0}:DTIWidgetMenu>"),ComponentModel.ToolboxItem(False)> _
Public Class DTIWidgetMenu
    Inherits DTIServerBase

#Region "Properties"

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

    Public Property ControlList() As Specialized.OrderedDictionary
        Get
            If Session("ControlList_" & Page.Request.RawUrl & Me.UniqueID) Is Nothing Then
                Session("ControlList_" & Page.Request.RawUrl & Me.UniqueID) = New Specialized.OrderedDictionary
            End If
            Return Session("ControlList_" & Page.Request.RawUrl & Me.UniqueID)
        End Get
        Set(ByVal value As Specialized.OrderedDictionary)
            Session("ControlList_" & Page.Request.RawUrl & Me.UniqueID) = value
        End Set
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

    Private _Class As String
    Public Property [Class]() As String
        Get
            Return _Class
        End Get
        Set(ByVal value As String)
            _Class = value
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

    Private _ConnectToID As String
    Public Property ConnectToID() As String
        Get
            Return _ConnectToID
        End Get
        Set(ByVal value As String)
            _ConnectToID = value
        End Set
    End Property

    Private _placeHolderClass As String
    Public Property PlaceHolderClass() As String
        Get
            Return _placeHolderClass
        End Get
        Set(ByVal value As String)
            _placeHolderClass = value
        End Set
    End Property

    Public Enum Cursor As Integer
        move = 0
        pointer = 1
        crosshair = 2
    End Enum

    Private _CursorType As Cursor
    Public Property CursorType() As Cursor
        Get
            Return _CursorType
        End Get
        Set(ByVal value As Cursor)
            _CursorType = value
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
        original = 1
        clone = 0
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

    Private ReadOnly Property ds() As dsDTISortable
        Get
            If Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") Is Nothing Then
                Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") = New dsDTISortable
            End If
            Return Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu")
        End Get
    End Property

    Private _isMenu As Boolean
    Public Property isMenu() As Boolean
        Get
            Return _isMenu
        End Get
        Set(ByVal value As Boolean)
            _isMenu = value
        End Set
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

    Private _menuText As String = "Menu"
    Public Property MenuText() As String
        Get
            Return _menuText
        End Get
        Set(ByVal value As String)
            _menuText = value
        End Set
    End Property
#End Region

    Private Sub DTIWidgetMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If LayoutOn Then
            loadWidgetMenu()
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(Me.Page)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTISortable/DTISortable.css", "text/css")
        End If
    End Sub

    Private Sub loadWidgetMenu()
        Me.CssClass = "DTIMenuOuter-" & CssTheme
        loadData(Page.IsPostBack)
        loatSortable()
    End Sub

    Private Sub loatSortable()
        Dim widgetMenu As New Panel
        Dim options As String = ""
        widgetMenu.CssClass = "DTIMenu-" & CssTheme
        widgetMenu.Controls.Add(New LiteralControl("<span class=""DTIMenuText-" & CssTheme & """>" & MenuText & "</span>"))

        Dim hasHandle As Boolean = False
        options = "cursor:'" & getEnumName(CursorType) & "',"

        If CursorPosition <> CursorAt.false OrElse CursorPositionValue > 0 Then
            options &= "cursorAt:{" & getEnumName(CursorPosition) & ":" & CursorPositionValue & "},"
        End If
        If [Class] = "" Then
            [Class] = "DTIConnectedSortable-" & CssTheme
        End If
        If ConnectByClass = True Then
            options &= "connectToSortable:'." & [Class] & "',"
        ElseIf ConnectToID IsNot Nothing Then
            options &= "connectToSortable:'#" & ConnectToID & "',"
        End If
        If PlaceHolderClass IsNot Nothing Then
            options &= "placeholder:'" & PlaceHolderClass & "',"
        End If

        If AxisRestriction <> Axis.none Then
            options &= "axis:'" & getEnumName(AxisRestriction) & "',"
        End If
        If DragOnEmpty = True Then
            options &= "dropOnEmpty:true,"
        End If
        If HelperType <> Helper.original Then
            options &= "helper:'clone',"
        End If
        If HandleText <> "" Then
            hasHandle = True
            options &= "handle: '.DTIItemHandle-" & CssTheme & "',"
        End If
        options &= "stop: function(event, ui){ui.helper.append('<div>Test This</div>');}"

        options = options.TrimEnd(",")

        Me.Controls.Add(New LiteralControl("<script type=""text/javascript"">  " & vbCrLf & _
                            "	$(function() { " & vbCrLf & _
                            "		$('.DTIMenuItem-" & CssTheme & "').draggable({ " & vbCrLf & _
                            "			" & options & vbCrLf & _
                            "		}); " & vbCrLf & _
                            "	}); " & vbCrLf & _
                            "</script>  "))

        For Each row As dsDTISortable.WidgetMenuTempRow In ds.WidgetMenuTemp
            With widgetMenu
                Dim title As String = row.Typename
                If title.IndexOf("#") > -1 Then
                    title = title.Substring(title.LastIndexOf("#") + 1)
                Else
                    title = title.Substring(title.LastIndexOf(".") + 1)
                End If
                .Controls.Add(New LiteralControl("<div id=""Menu_" & row.Id & """ class=""DTIMenuItem-" & CssTheme & """>"))
                If hasHandle Then
                    .Controls.Add(New LiteralControl("<div class=""DTIItemHandle-" & CssTheme & """>" & HandleText & "</div>"))
                End If
                .Controls.Add(New LiteralControl("<img src=""" & row.Icon_url & """ />"))
                .Controls.Add(New LiteralControl("<span class=""DTIItemText-" & CssTheme & """>" & title & "</span>"))
                .Controls.Add(New LiteralControl("</div>"))

            End With
        Next

        Me.Controls.Add(widgetMenu)
    End Sub

    Private Sub loadData(ByVal postback As Boolean)
        If Not postback Then
            BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
            'ds.WidgetMenuTemp.Clear()
            ControlList.Clear()
            Dim location As String = AppDomain.CurrentDomain.RelativeSearchPath
            For Each asm As Assembly In AppDomain.CurrentDomain.GetAssemblies()
                addassembly(asm)
            Next
            For Each dllfile As String In System.IO.Directory.GetFiles(location, "*.dll")
                Dim filename As String = dllfile.Substring(dllfile.LastIndexOf("\"))
                If (filename.StartsWith("\DTI") OrElse filename.StartsWith("\ContentHolder")) AndAlso filename <> "\DTIVideoManager.dll" Then
                    Dim ass As Assembly = Assembly.LoadFile(dllfile)
                    addassembly(ass)
                End If
            Next
        End If
    End Sub

    Private Sub addassembly(ByVal asm As Assembly)
        Dim basetype As Type = GetType(DTIServerControl)
        Try
            For Each typ As Type In asm.GetExportedTypes
                Dim s As String = typ.ToString
                If basetype.IsAssignableFrom(typ) Then
                    If typ.FullName <> "DTIGallery.DTIDataGallery" Then
                        Try
                            Dim x As DTIServerControls.DTIServerControl = typ.Assembly.CreateInstance(typ.FullName)
                            'Dim name As String = x.GetType.Name
                            'If name <> "DTISortableGeneric" AndAlso name <> "DTISortable" AndAlso name <> "DTIWidgetMenu" AndAlso name <> "DTIDataGallery" Then
                            If Not x.Menu_Icon_Url Is Nothing Then
                                If ds.WidgetMenuTemp.Select("Typename='" & typ.FullName & "'").Length = 0 Then
                                    x.contentType = "test"
                                    ds.WidgetMenuTemp.AddWidgetMenuTempRow(asm.FullName, typ.FullName, x.Menu_Icon_Url)
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Next
        Catch ex As Exception
            'Lets just say we don't need this throwing exceptions
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Function save(ByRef pg As Page) As Boolean
        Dim sort As DTISortable = BaseClasses.Spider.spiderPageforType(pg, GetType(DTISortable))
        sort.saveAll(pg)
    End Function

    Private Function getEnumName(ByVal enumeration As Object) As String
        Return [Enum].GetName(enumeration.GetType, enumeration)
    End Function

    Public Shared Sub addUserControlToMenu(ByVal Path_To_Ascx_Control As String, Optional ByVal menu_Item_Title As String = Nothing, Optional ByVal iconURL As String = Nothing)
        If iconURL Is Nothing Then iconURL = BaseClasses.Scripts.ScriptsURL & "DTISortable/usercontrol.png"
        If menu_Item_Title Is Nothing Then
            menu_Item_Title = Path_To_Ascx_Control.Substring(Path_To_Ascx_Control.LastIndexOf("/") + 1)
            menu_Item_Title = menu_Item_Title.Substring(0, menu_Item_Title.LastIndexOf("."))
        End If
        If DTISharedVariables.Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") Is Nothing Then
            DTISharedVariables.Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") = New dsDTISortable
        End If
        Dim ds As dsDTISortable = DTISharedVariables.Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu")

        If ds.WidgetMenuTemp.Select("Typename like '" & Path_To_Ascx_Control & "#%'").Length = 0 Then
            ds.WidgetMenuTemp.AddWidgetMenuTempRow("UserControl", Path_To_Ascx_Control & "#" & menu_Item_Title, iconURL)
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If LayoutOn Then
            writer.Write("<script type=""text/javascript""> " & vbCrLf & _
                        "   function DeleteMenuItem(id, hidid,sortid){" & vbCrLf & _
                        "       $(id).remove();" & vbCrLf & _
                        "       document.getElementById(hidid).value = $(sortid).sortable(""toArray""); " & vbCrLf & _
                        "   }" & vbCrLf & _
                        "</script> ")
        End If
        MyBase.Render(writer)
    End Sub
End Class
