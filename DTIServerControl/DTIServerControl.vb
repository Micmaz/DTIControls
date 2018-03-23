Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.SessionState
Imports BaseClasses
Imports DTIMiniControls
Imports HighslideControls

''' <summary>
''' Base class for DTIControls. Adds state awareness (read/write/layout) awareness as well as setup panel plugins to easily edit data.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Base class for DTIControls. Adds state awareness (read/write/layout) awareness as well as setup panel plugins to easily edit data."),ToolboxData("<{0}:DTIServerControl runat=server></{0}:DTIServerControl>"), _
Designer(GetType(DTIServerBaseDesigner)), ViewStateModeById()> _
Public MustInherit Class DTIServerControl
    Inherits DTIServerBase

	''' <summary>
	''' just a rename of the property cssClass to make it easier to switch between static and dynamic objects
	''' </summary>
	''' <value>
	''' The class list.
	''' </value>
<System.ComponentModel.Description("just a rename of the property cssClass to make it easier to switch between static and dynamic objects")> _
	Public Property [class] As String
		Get
			Return Me.CssClass
		End Get
		Set(value As String)
			Me.CssClass = value
		End Set
	End Property

	Protected Friend setupPanel As Panel

	Public Enum modes
		Read = 1
		Write = 2
	End Enum

	''' <summary>
	''' Fired when a control goes from read mode to write or layout mode.
	''' </summary>
	''' <remarks></remarks>
    <System.ComponentModel.Description("Fired when a control goes from read mode to write or layout mode.")> _
    Public Event ModeChanged()

    Public Event LoadControls(ByVal modeChanged As Boolean)


#Region "Properties"

    Private _settingsControlUrl = Nothing
    Protected Overridable Property settingsControlUrl() As String
        Get
            Return _settingsControlUrl
        End Get
        Set(ByVal value As String)
            _settingsControlUrl = value
        End Set
    End Property

    Private _settingsPageUrl = Nothing
    Protected Overridable Property settingsPageUrl() As String
        Get
            Return _settingsPageUrl
        End Get
        Set(ByVal value As String)
            If Not value.Contains("/") Then
                value = "/res/" & Me.GetType.FullName.Substring(0, Me.GetType.FullName.LastIndexOf(".")) & "/" & value
            End If
            _settingsPageUrl = value
        End Set
    End Property

    'Public ReadOnly Property setupControlList() As ControlCollection
    '    Get
    '        If setupPanel Is Nothing Then
    '            setupPanel = New Panel
    '            'setupPanel.ID = "setupPanel"
    '        End If
    '        Return setupPanel.Controls
    '    End Get
    'End Property

    Protected Overridable ReadOnly Property showSetup() As Boolean
        Get
            If Not Me.settingsControlUrl Is Nothing Then
                If setupPanel Is Nothing Then setupPanel = New Panel
                Return True
            End If

            If setupPanel Is Nothing OrElse setupPanel.Controls.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private _useDTIControlsProperties As Boolean = False
    Protected Friend Overridable Property useGenericDTIControlsProperties() As Boolean
        Get
            Return _useDTIControlsProperties
        End Get
        Set(ByVal value As Boolean)
            _useDTIControlsProperties = value
        End Set
    End Property



    Public Overridable ReadOnly Property Menu_Icon_Url() As String
        Get
            Return Nothing
        End Get
    End Property

    'Private _sortableId As Long = -1
    'Public Overridable Property SortableItemId() As String
    '    Get
    '        Return _sortableId
    '    End Get
    '    Set(ByVal value As String)
    '        _sortableId = value
    '    End Set
    'End Property


    'Private _staticMode As Boolean = False
    'Public Property StaticMode() As Boolean
    '    Get
    '        Return _staticMode
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _staticMode = value
    '    End Set
    'End Property

	
    Private _mode As modes = Nothing
	Private property modeSetProgramatically = false
    Public Property Mode() As modes
        Get
			if modeSetProgramatically then return _mode
            If _mode = Nothing Then
                _mode = modes.Read
                RaiseEvent ModeChanged()
            End If

            If DTISharedVariables.AdminOn AndAlso _mode = modes.Read Then
                If DTISharedVariables.siteEditMainID = Me.MainID Or DTISharedVariables.siteEditMainID = 0 Then
                    _mode = modes.Write
                    RaiseEvent ModeChanged()
                End If
            End If

            'If Not AdminOn AndAlso Not StaticMode AndAlso _mode <> modes.Read Then
			If Not DTISharedVariables.AdminOn AndAlso _mode <> modes.Read Then
                _mode = modes.Read
                RaiseEvent ModeChanged()
            End If

            Return _mode
        End Get
        Set(ByVal value As modes)
            _mode = value
			modeSetProgramatically = true
            RaiseEvent ModeChanged()
        End Set
    End Property

    'Public Property Mode() As modes
    '    Get
    '        If Session("ContentDisplayMode." & identifierString) Is Nothing Then
    '            Session("ContentDisplayMode." & identifierString) = modes.Read
    '            RaiseEvent ModeChanged()
    '        End If

    '        If AdminOn AndAlso Session("ContentDisplayMode." & identifierString) = modes.Read Then
    '            If DTISharedVariables.siteEditMainID = Me.MainID Or DTISharedVariables.siteEditMainID = 0 Then
    '                Session("ContentDisplayMode." & identifierString) = modes.Write
    '                RaiseEvent ModeChanged()
    '            End If
    '        End If

    '        If Not AdminOn AndAlso Not StaticMode AndAlso Session("ContentDisplayMode." & identifierString) <> modes.Read Then
    '            Session("ContentDisplayMode." & identifierString) = modes.Read
    '            RaiseEvent ModeChanged()
    '        End If

    '        Return Session("ContentDisplayMode." & identifierString)
    '    End Get
    '    Set(ByVal value As modes)
    '        Session("ContentDisplayMode." & identifierString) = value
    '        RaiseEvent ModeChanged()
    '    End Set
    'End Property

    Public Overrides Property Visible() As Boolean
        Get
            If Me.DesignMode Then Return MyBase.Visible
            If Mode = modes.Write Then
                Return True
            End If
            Return MyBase.Visible
        End Get
        Set(ByVal value As Boolean)
            MyBase.Visible = value
        End Set
    End Property

	private _showborder as boolean = false
    Public Overridable Property ShowBorder() As Boolean
        Get
			If Me.DesignMode Then Return True
			return _showborder
            'If Session("ShowBorder." & identifierString) Is Nothing Then
            '    Session("ShowBorder." & identifierString) = False
            'End If
            'Return Session("ShowBorder." & identifierString)
        End Get
        Set(ByVal value As Boolean)
            'Session("ShowBorder." & identifierString) = value
			_showborder = value
        End Set
    End Property




#End Region

#Region "Events"

    Private Sub DTIServerControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not DesignMode Then
            If Me.useGenericDTIControlsProperties Then
                If inittypeLst(GetType(DTIServerControl)) Then
                    sqlhelper.checkAndCreateAllTables(New dsDTIControls)
                End If

                ds = New dsDTIControls
                Me.parallelhelper.addFillDataTable("select * from DTIControl where mainID=@mainid and Content_Type= @contentType", ds.DTIControl, New Object() {Me.MainID, Me.contentType})
                Me.parallelhelper.addFillDataTable("select * from DTIControlProperty where DTIControlID in (select id from DTIControl where mainID=@mainid and Content_Type= @contentType)", ds.DTIControlProperty, New Object() {Me.MainID, Me.contentType})
            End If
            If Mode = modes.Read Then
                ShowBorder = False
            Else
                ShowBorder = True
            End If
            RaiseEvent LoadControls(False)
        End If
    End Sub


    Private Sub DTIServerControl_ModeChanged() Handles Me.ModeChanged
        If Mode = modes.Read Then
            ShowBorder = False
        Else
            ShowBorder = True
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If ShowBorder Then
			'setFreezer()
			' BorderStyle = UI.WebControls.BorderStyle.Dashed
			'BorderColor = Drawing.Color.Gray
			'BorderWidth = New WebControls.Unit("3px")
			'Attributes.Add("onmouseover", "style.borderColor='#FF2222'")
			'Attributes.Add("onmouseout", "style.borderColor='#777777'")
			If Not CssClass.Contains("DTIServerControlEdit") Then
				CssClass &= " DTIServerControlEdit"
			End If
		End If
        If Not settingsPageUrl Is Nothing AndAlso Mode <> modes.Read Then
            Session(uniqueIdentifier) = Me
        End If
        'Else
        '    If Not _freezer Is Nothing AndAlso Freezer.IsFrozen Then
        '        Freezer.BorderStyle = WebControls.BorderStyle.None
        '        Freezer.Attributes.Remove("onmouseover")
        '        Freezer.Attributes.Remove("onmouseout")
        '    Else
        '        BorderStyle = WebControls.BorderStyle.None
        '        Attributes.Remove("onmouseover")
        '        Attributes.Remove("onmouseout")
        '    End If



        MyBase.Render(writer)
        'nagRender(writer)

    End Sub

    Private Shared rand As New Random(0)
    Private Sub nagRender(ByRef writer As System.Web.UI.HtmlTextWriter)
        If rand.Next(0, 100) > 70 Then
            writer.Write("<a style='font-size:xx-small' href='http://www.DTIControls.net'>DTIControls</a>")
        End If
    End Sub


#End Region

    Private ds As dsDTIControls

    Private genericPropsSet As Boolean = False
    Private Sub DTIServerControl_DataReady() Handles Me.DataReady
        If Not DesignMode Then
            If Me.useGenericDTIControlsProperties Then
                If Not genericPropsSet Then
                    Dim dv As New DataView(ds.DTIControl, "mainid=" & Me.MainID & " AND Content_Type = '" & Me.contentType & "'", "", DataViewRowState.CurrentRows)
                    Dim controlRow As dsDTIControls.DTIControlRow
                    If dv.Count = Nothing Then
                        controlRow = ds.DTIControl.AddDTIControlRow(Me.GetType.ToString, contentType, MainID)
                        sqlhelper.Update(ds.DTIControl)
                    Else
                        controlRow = dv(0).Row
                    End If

                    Dim propview As New DataView(ds.DTIControlProperty, "DTIControlID = " & controlRow.Id, "", DataViewRowState.CurrentRows)
                    Comparator.setProperties(propview, Me)
                    genericPropsSet = True
                End If
            End If
        End If
    End Sub

    Private Sub DTIServerControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not DesignMode Then
            If Not settingsPageUrl Is Nothing AndAlso Mode <> modes.Read Then
                SetupHighslide.DisplayText = "<img class='serverctrlEditor' style='border-width:0px;padding:5px;float:right;margin-right:-19px;position:absolute;bottom:-19px;right:0px;' src='" & BaseClasses.Scripts.ScriptsURL & "DTIServerControls/settingTab.png' border='0' />"
                Me.Style.Add("position", "relative")
                SetupHighslide.HighslideDisplayMode = SharedHighslideVariables.HighslideDisplayModes.Iframe
                SetupHighslide.ExpandURL = settingsPageUrl & "?uniqueIdentifier=" & Me.uniqueIdentifier & "&parentID=" & Me.ClientID
                Controls.Add(SetupHighslide)
                enableFreezing()
            Else
                If showSetup AndAlso Mode <> modes.Read Then
                    If Mode = modes.Write Then

                    End If
                    'SetupHighslide.DisplayText = "<img style='border-width:0px;padding:5px;float:right;margin-top:-18px;margin-right:-19px;' src='" & BaseClasses.Scripts.ScriptsURL & "DTIServerControls/settingTab.png' border='0' />"
                    SetupHighslide.DisplayText = "<img style='border-width:0px;padding:5px;float:right;margin-right:-19px;position:absolute;bottom:-19px;right:0px;' src='" & BaseClasses.Scripts.ScriptsURL & "DTIServerControls/settingTab.png' border='0' />"
                    Me.Style.Add("position", "relative")
                    SetupHighslide.HighslideDisplayMode = SharedHighslideVariables.HighslideDisplayModes.HTML
                    setupPanel.CssClass = "highslide-maincontent"

                    Controls.Add(SetupHighslide)
                    If Not settingsControlUrl Is Nothing Then
                        Dim ctrl As DTISettingsControl = Me.Page.LoadControl(settingsControlUrl)
                        setupPanel.Controls.Add(ctrl)
                        ctrl.MainID = Me.MainID
                        ctrl.contentType = Me.contentType
                        ctrl.raiseSettingsCreated(Me)
                    End If
                    Controls.Add(setupPanel)

                    SetupHighslide.maincontentId = setupPanel.ClientID

                    'Dim script As String = "$(document).ready(function(){var ele = $('#" & _
                    '    Me.ClientID & "'); var offset = ele.offset(); $('#" & SetupHighslide.ClientID & _
                    '    "').css({position:'absolute',top:offset.top+'px',left:ele.width()+'px',zIndex:1001});}).children().fadeIn();"
                    'registerClientStartupScriptBlock(setupPanel.ClientID, script, True)
                End If
            End If

            If Me.useGenericDTIControlsProperties Then
                parallelhelper.executeParallelDBCall()
            End If
        End If

    End Sub

    Private Sub DTIServerControl_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        If Not DesignMode Then
            If Me.useGenericDTIControlsProperties Then
                sqlhelper.checkAndCreateAllTables(New dsDTIControls)
            End If
        End If
    End Sub


End Class
