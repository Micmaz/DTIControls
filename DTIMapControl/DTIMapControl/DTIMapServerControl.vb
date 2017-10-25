Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DTIServerControls
Imports HighslideControls
Imports DTIMapControl.SharedMapVariables

'#If DEBUG Then
Public Class DTIMapServerControl
    Inherits DTIServerControl
    '#Else
    '    <ParseChildren(True, "Address")> _
    '   <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    '  Public Class DTIMapServerControl
    'Inherits DTIServerControl
    '#End If
    Public mapViewControl As DTIMapUserControl
    'Public mapEditControl As DTIMapEditUserControl
    'Public DataFetched As Boolean = False
    'Public MapWidth As Integer = 400
    'Public MapHeight As Integer = 400
    Public DirectionWidth As Integer = 400
    Public DirectionHeight As Integer = 400

    Public Event AddressSet(ByVal address As String)

#Region "Properties"

    Public Overrides ReadOnly Property Menu_Icon_Url() As String
        Get
            Return BaseClasses.Scripts.ScriptsURL & "DTIMapControl/MapIcon.png"
        End Get
    End Property

    'Private _addressRow As dsMapControl.DTIMapControlRow
    '    Private Property addressRow() As dsMapControl.DTIMapControlRow
    '        Get
    '            Return _addressRow
    '        End Get
    '        Set(ByVal value As dsMapControl.DTIMapControlRow)
    '            _addressRow = value
    '        End Set
    '    End Property

    '    Private _keyRow As dsMapControl.DTIGoogleMapKeyRow
    '    Private Property googleKeyRow() As dsMapControl.DTIGoogleMapKeyRow
    '        Get
    '            Return _keyRow
    '        End Get
    '        Set(ByVal value As dsMapControl.DTIGoogleMapKeyRow)
    '            _keyRow = value
    '        End Set
    '    End Property

    Private _googleKey As String
    Public Property GoogleKey() As String
        Get
            If _googleKey Is Nothing Or _googleKey = "" Then
                If System.Web.Configuration.WebConfigurationManager.AppSettings("GOOGLE_MAP_KEY") IsNot Nothing Then
                    _googleKey = System.Web.Configuration.WebConfigurationManager.AppSettings("GOOGLE_MAP_KEY")
                Else
                    _googleKey = ""
                End If
            End If
            Return _googleKey
        End Get

        Set(ByVal value As String)
            _googleKey = value
        End Set
    End Property

    Private _address As String = "112 Pheasant Wood Ct., Morrisville, NC 27560"
    <PersistenceMode(PersistenceMode.InnerDefaultProperty)> Property Address() As String
        Get
            Return _address
        End Get
        Set(ByVal Value As String)
            _address = Value
        End Set
    End Property

    Private _title As String = ""
    Property AddressTitle() As String
        Get
            Return _title
        End Get
        Set(ByVal Value As String)
            _title = Value
        End Set
    End Property



#End Region

#Region "Events"

    'Private Sub DTIMapServerControl_DataChanged() Handles Me.DataChanged
    '    addSQLCall()
    'End Sub

    'Private Sub DTIMapServerControl_DataReady() Handles Me.DataReady
    '    DataFetched = True
    'End Sub

    Private Sub mypage_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles mypage.InitComplete
        If Not MyHighslideHeader.HighslideVariables.Contains("align") Then
            MyHighslideHeader.HighslideVariables.Add("align", "center")
        End If
        If Not MyHighslideHeader.HighslideVariables.Contains("wrapperClassName") Then
            MyHighslideHeader.HighslideVariables.Add("wrapperClassName", "draggable-header")
        End If
        If Not MyHighslideHeader.HighslideVariables.Contains("outlineType") Then
            MyHighslideHeader.HighslideVariables.Add("outlineType", "rounded-white")
        End If
    End Sub

    'Private Sub DTIMapServerControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    If Not mypage.IsPostBack Then
    '        addSQLCall()
    '    End If
    'End Sub

    Private Sub DTIMapServerControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not DataFetched AndAlso Not mypage.IsPostBack Then
        '    parallelhelper.executeParallelDBCall()
        'End If
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)

        'If Not Mode = modes.Read Then
        '    Me.Freezer.IsFrozen = True
        '    Me.Freezer.BackgroundColor = "transparent"
        'End If
    End Sub

    Private Sub DTIMapServerControl_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
        Controls.Clear()

        mapViewControl = DirectCast(mypage.LoadControl("~/res/DTIMapControl/DTIMapUserControl.ascx"), DTIMapUserControl)
        mapViewControl.ID = ClientID & "_mapViewControl"

        If Mode = modes.Read Then
            Controls.Add(mapViewControl)
        Else
            'mapEditControl = DirectCast(mypage.LoadControl("~/res/DTIMapControl/DTIMapEditUserControl.ascx"), DTIMapEditUserControl)
            'setupControlList.Add(mapEditControl)
            'mapEditControl.caller = Me
            Controls.Add(mapViewControl)
        End If
    End Sub

    Private Sub DTIMapServerControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        RenderMode = renderModes.Span
        'setAddress()
        setMode()
        If Not SetupHighslide.HighslideVariables.Contains("height") Then
            SetupHighslide.HighslideVariables.Add("height", 200)
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        setDimensions()
        MyBase.Render(writer)
    End Sub

#End Region

#Region "Helper Functions"

    Private Sub setDimensions()
        'If Me.Width.IsEmpty OrElse Me.Width.Value = 0 Then
        '    Me.Width = MapWidth
        'Else
        '    MapWidth = Me.Width.Value
        'End If
        'If Me.Height.IsEmpty OrElse Me.Height.Value = 0 Then
        '    Me.Height = MapHeight
        'Else
        '    MapHeight = Me.Height.Value
        'End If
        If Me.Width.IsEmpty OrElse Me.Width.Value = 0 Then
            mapViewControl.mapSpan.Width = 400
        Else
            mapViewControl.mapSpan.Width = Me.Width.Value
        End If
        If Me.Height.IsEmpty OrElse Me.Height.Value = 0 Then
            mapViewControl.mapSpan.Height = 400
        Else
            mapViewControl.mapSpan.Height = Me.Height.Value
        End If

    End Sub


    Private Sub setMode()

        If Mode = modes.Read Then
            ShowBorder = False
        Else
            ShowBorder = True

            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "$(document).ready(function() { $.query = { prefix: false }; });")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "jQueryLibrary/jquery.query.js")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIMapControl/mapFunctions.js")
        End If
        setMap()
    End Sub

    Private Sub setMap()
        'If Mode = modes.Write OrElse GoogleKey <> "" Then
        registerClientScriptFile("map_js", "http://maps.google.com/maps?file=api&v=2.x&sensor=false&key=" & GoogleKey)
        jQueryLibrary.jQueryInclude.addScriptFile(mypage, "DTIMapControl/mapFunctions.js")

        'Address, map_div_id, directions_div_id, client_id, directionWidth, directionHeight
        Dim script As String = "$(function() {initialize('" & Address.Replace("'", "\'") & "', '" & AddressTitle.Replace("'", "\'") & _
            "', '" & mapViewControl.mapSpan.ClientID & "', '" & mapViewControl.directionsSpan.ClientID & _
            "', '" & ClientID & "', " & DirectionWidth & ", " & DirectionHeight & ");});"
        registerClientStartupScriptBlock("initializeMap_" & ClientID, script, True)
        'End If
    End Sub

    'Public Overridable Sub setAddress()
    '    Dim rowSet As Boolean = setAddressRow()

    '    If rowSet Then
    '        If Not addressRow.IsAddressNull Then
    '            Address = addressRow.Address
    '            AddressTitle = addressRow.Title
    '        End If
    '    End If

    '    RaiseEvent AddressSet(Address)
    'End Sub


    'Private Function setAddressRow() As Boolean
    '    If addressRow Is Nothing Then
    '        For Each row As dsMapControl.DTIMapControlRow In addressTable
    '            If row.contentType = contentType Then
    '                addressRow = row
    '                Return True
    '            End If
    '        Next

    '    Else : Return True
    '    End If
    '    Return False
    'End Function

    'Private Function setGoogleKeyRow() As Boolean
    '    If googleKeyRow Is Nothing Then
    '        For Each row As dsMapControl.DTIGoogleMapKeyRow In googleKeyTable
    '            If row.MainId = MainID Then
    '                googleKeyRow = row
    '                Return True
    '            End If
    '        Next
    '    Else : Return True
    '    End If
    '    Return False
    'End Function

    'Public Sub saveAddress(Optional ByVal addr1 As String = "", Optional ByVal title As String = "")
    '    Dim rowSet As Boolean = setAddressRow()

    '    If addr1 <> "" Then
    '        Address = addr1
    '    End If

    '    If title <> "" Then
    '        AddressTitle = title
    '    End If

    '    If rowSet Then
    '        addressRow.Address = Address
    '        addressRow.Title = AddressTitle
    '    Else
    '        addressRow = addressTable.AddDTIMapControlRow(MainID, Address, AddressTitle, contentType)
    '    End If
    '    sqlhelper.Update(addressTable)
    'End Sub

    'Public Sub saveGoogleKey(ByVal key As String)
    '    Dim rowSet As Boolean = setGoogleKeyRow()

    '    If rowSet Then
    '        googleKeyRow.Google_Key = key
    '    Else
    '        googleKeyRow = googleKeyTable.AddDTIGoogleMapKeyRow(MainID, key)
    '    End If
    '    sqlhelper.Update(googleKeyTable)
    'End Sub

    'Private Sub addSQLCall()
    '    parallelhelper.addFillDataTable("select * from DTIMapControl where mainID = @mainId_" & contentType & " and contentType = @contentType_" & contentType, addressTable, New Object() {MainID, contentType})
    '    parallelhelper.addFillDataTable("select * from DTIGoogleMapKey where mainID = @mainId_" & contentType, googleKeyTable, New Object() {MainID})
    'End Sub

#End Region

    'Private Sub DTIMapServerControl_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
    '    sqlhelper.checkAndCreateAllTables(New dsMapControl)
    'End Sub

    Public Sub New()
        Me.settingsPageUrl = "settingsForm.aspx"
        Me.useGenericDTIControlsProperties = True
        'Me.ShowBorder = False
    End Sub

End Class
