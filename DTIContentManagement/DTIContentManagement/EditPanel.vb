Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text.RegularExpressions
Imports BaseClasses
Imports DTIServerControls
Imports HighslideControls
Imports DTIMediaManager
Imports System.Web.Configuration
Imports SummerNote.SummerNote

''' <summary>
''' The edit panel user control, adds editable content areas to any page.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("The edit panel user control, adds editable content areas to any page."), ToolboxData("<{0}:EditPanel runat=""server""> </{0}:EditPanel>"), ComponentModel.Designer(GetType(DTIServerControls.DTIServerBaseDesigner)), ViewStateModeById()> _
Public Class EditPanel
    Inherits DTIServerControl

    Protected litText As New LiteralControl
    Private DataFetched As Boolean = False

    ''' <summary>
    ''' The text value of the control has been set.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The text value of the control has been set.")> _
    Public Event textSet()

    ''' <summary>
    ''' The save button on the html editor has been clicked.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The save button on the html editor has been clicked.")> _
    Public Event SaveClicked()

    ''' <summary>
    ''' The cancel button on the html editor has been clicked.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The cancel button on the html editor has been clicked.")> _
    Public Event CancelClicked()
    Private _defaultText As String = " <br />"
    Public Property defaultText As String
        Get
            Return _defaultText
        End Get
        Set(value As String)
            _defaultText = value
            If _htmlEdit IsNot Nothing Then _htmlEdit.DefaultHTML = _defaultText
        End Set
    End Property

#Region "Properties"

    'Public Overrides Property ShowBorder() As Boolean
    '    Get
    '        Return False
    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    ''' <summary>
    ''' Icon of the control int the layout toolbar.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Icon of the control int the layout toolbar.")> _
    Public Overrides ReadOnly Property Menu_Icon_Url() As String
        Get
            Return BaseClasses.Scripts.ScriptsURL & "DTIContentManagement/editPanelIcon.png"
        End Get
    End Property

    ''' <summary>
    ''' Determines weather the toolbar is rendered over the editor or at the top of the screen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines weather the toolbar is rendered over the editor or at the top of the screen.")> _
    Public Property ToolbarMode() As ToolbarModes
        Get
            Return htmlEdit.ToolbarMode
        End Get
        Set(ByVal value As ToolbarModes)
            htmlEdit.ToolbarMode = value
        End Set
    End Property

    Public ReadOnly Property htmlEditUniqueID As String
        Get
            Return htmlEdit.UniqueID
        End Get
    End Property


    Private medialListInstValue As String = Nothing
    Public Property mediaListURL As String
        Get
            If Not medialListInstValue Is Nothing Then Return medialListInstValue
            If Not WebConfigurationManager.AppSettings("mediaListURL") Is Nothing Then Return WebConfigurationManager.AppSettings("mediaListURL")
            'Return "~/res/DTIContentManagement/MediaList.aspx"
            Return "~/res/DTIContentManagement/ImageSelectorDlg.aspx"
        End Get
        Set(ByVal value As String)
            medialListInstValue = value
        End Set
    End Property


	Private WithEvents _htmlEdit As SummerNote.SummerNote

	'Public Property Skin() As String
	'    Get
	'        Return htmlEdit.SkinID
	'    End Get
	'    Set(ByVal value As String)
	'        htmlEdit.SkinID = value
	'    End Set
	'End Property

	'Private _imageDir As String() = {"~/userdata/"}
	'Public Property ImageDirectories() As String()
	'    Get
	'        Return _imageDir
	'    End Get
	'    Set(ByVal value As String())
	'        _imageDir = value
	'    End Set
	'End Property

	Protected _text As String

	''' <summary>
	''' The text contained in the html editor.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The text contained in the html editor.")>
	Public Property Text() As String
		Get
			If _text Is Nothing Then
				_text = defaultText
			End If
			Return _text
		End Get
		Set(ByVal value As String)
			_text = value
		End Set
	End Property

	'Private _saveClickReturnsToWriteMode As Boolean = True
	'Public Property SaveClickReturnsToWriteMode() As Boolean
	'    Get
	'        Return _saveClickReturnsToWriteMode
	'    End Get
	'    Set(ByVal value As Boolean)
	'        _saveClickReturnsToWriteMode = value
	'    End Set
	'End Property

	''' <summary>
	''' Show save and cancel buttons.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Show save and cancel buttons.")>
	Public Property ShowCancelButton() As Boolean
		Get
			Return Not htmlEdit.DisableCancel
		End Get
		Set(ByVal value As Boolean)
			htmlEdit.DisableCancel = Not value
		End Set
	End Property

	''' <summary>
	''' Readonly property with the contents data table for all editors on this page.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Readonly property with the contents data table for all editors on this page.")>
	Protected Overridable ReadOnly Property contentTable() As dsEditPanel.DTIContentManagerDataTable
		Get
			If DataSource Is Nothing Then
				DataSource = New dsEditPanel.DTIContentManagerDataTable
				CType(DataSource, dsEditPanel.DTIContentManagerDataTable).TableName = Me.GetType.Name
			End If
			Return CType(DataSource, dsEditPanel.DTIContentManagerDataTable)
		End Get
	End Property

	Private _contentRow As dsEditPanel.DTIContentManagerRow

	''' <summary>
	''' DataRow for the current content.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("DataRow for the current content.")>
	Protected Property ContentRow() As dsEditPanel.DTIContentManagerRow
		Get
			Return _contentRow
		End Get
		Set(ByVal value As dsEditPanel.DTIContentManagerRow)
			_contentRow = value
		End Set
	End Property






	'''' <summary>
	'''' The media row that controls the layout of the panel instance.
	'''' </summary>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("The media row that controls the layout of the panel instance.")> _
	'Private _mediaRow As dsMedia.DTIMediaManagerRow
	'Public Property MediaRow() As dsMedia.DTIMediaManagerRow
	'    Get
	'        Return _mediaRow
	'    End Get
	'    Set(ByVal value As dsMedia.DTIMediaManagerRow)
	'        _mediaRow = value
	'    End Set
	'End Property

	'Private _saveMediaRow As Boolean = True





	'''' <summary>
	'''' If false layout informatio is not saved back to the mediaRow.
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("If false layout informatio is not saved back to the mediaRow.")> _
	'Public Property SaveMediaRow() As Boolean
	'    Get
	'        Return _saveMediaRow
	'    End Get
	'    Set(ByVal value As Boolean)
	'        _saveMediaRow = value
	'    End Set
	'End Property

#End Region

#Region "Events"

	Private Sub EditPanel_DataChanged() Handles Me.DataChanged
		_contentRow = Nothing
		addSQLCall()
	End Sub

	Private Sub EditPanel_DataReady() Handles Me.DataReady
		DataFetched = True
	End Sub


	Private Sub EditPanel_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.urlencode.js", , True)
		Me.EnableViewState = False
		If Not mypage.IsPostBack Then
			addSQLCall()
		End If
		MyBase.ShowBorder = False
		If Mode = modes.Write Then
			ShowBorder = True
		Else
			ShowBorder = False
		End If

		'myHighslideHeader = HighslideControls.HighslideHeaderControl.addToPage(mypage)
		'If Not MyHighslideHeader.HighslideVariables.Contains("wrapperClassName") Then
		'    MyHighslideHeader.HighslideVariables.Add("wrapperClassName", "draggable-header")
		'End If
		'If Not MyHighslideHeader.HighslideVariables.Contains("outlineType") Then
		'    MyHighslideHeader.HighslideVariables.Add("outlineType", "rounded-white")
		'End If

		registerHighslide()


		If Me.Controls.Count > 0 AndAlso TypeOf Controls(0) Is LiteralControl Then
			litText = CType(Controls(0), LiteralControl)
		End If

		Me.Controls.Clear()

		If Mode = modes.Write Then
			If Not htmlEdit() Is Nothing Then
				Me.Controls.Add(htmlEdit)
			End If
			setMode()
			'htmlEdit.BorderStyle = UI.WebControls.BorderStyle.Dashed
			'htmlEdit.BorderColor = Drawing.Color.Gray
			'htmlEdit.BorderWidth = New WebControls.Unit("3px")
			'htmlEdit.Attributes.Add("onmouseover", "style.borderColor='#FF2222'")
			'htmlEdit.Attributes.Add("onmouseout", "style.borderColor='#777777'")
		Else
			Me.Controls.Add(litText)
		End If

	End Sub

	Private Sub EditPanel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not DataFetched AndAlso mypage IsNot Nothing AndAlso Not mypage.IsPostBack Then
			parallelhelper.executeParallelDBCall()
		End If
	End Sub

	Private Sub EditPanel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		If dosave Then
			saveInstance()
		End If
		setText()
		setMode()
		If Mode = modes.Write Then

		Else

			litText.Text = litText.Text.Replace("<protected>", "").Replace("</protected>", "")
		End If
	End Sub

	Private Sub _htmlEdit_Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles _htmlEdit.Cancel_Clicked
		RaiseEvent CancelClicked()
	End Sub


	Private Sub _htmlEdit_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles _htmlEdit.Save_Clicked
		If (Mode = modes.Write) Then
			For Each ep As EditPanel In BaseClasses.Spider.spidercontrolforTypeArray(Me.Page, GetType(EditPanel))
				If ep IsNot Nothing AndAlso ep.Mode = modes.Write Then
					ep.setTextFromPage()
					If ep._text IsNot Nothing AndAlso ep._text.ToLower.Trim = htmlEdit.DefaultHTML.ToLower.Trim Then ep._text = ""
					ep.dosave = True
				End If
			Next
			RaiseEvent SaveClicked()
			'If SaveClickReturnsToWriteMode Then Mode = modes.Write
		End If
	End Sub

	''' <summary>
	''' Saves the current instance and makes all other editors obn the page to be saved on pre-render.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Saves the current instance and makes all other editors obn the page to be saved on pre-render.")>
	Public Sub saveAllInstances()
		For Each ep As EditPanel In BaseClasses.Spider.spidercontrolforTypeArray(Me.Page, GetType(EditPanel))
			If ep IsNot Nothing AndAlso ep.Mode = modes.Write Then
				ep.setTextFromPage()
				If ep._text IsNot Nothing AndAlso ep._text.ToLower.Trim = htmlEdit.DefaultHTML.ToLower.Trim Then ep._text = ""
				ep.dosave = True
			End If
		Next
		saveInstance()
	End Sub

	Private dosave As Boolean = False

	''' <summary>
	''' Saves just the current instance of the editor
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Saves just the current instance of the editor")>
	Public Sub saveInstance()
		If (Mode = modes.Write) Then
			'_text = htmlEdit.Text 'Web.HttpUtility.UrlDecode(Page.Request.Form(htmlEdit.UniqueID))
			setTextFromPage()
			_text = replaceimg(_text)
			saveContent()
			'If SaveClickReturnsToWriteMode Then Mode = modes.Write
		End If
	End Sub

	Public Sub setTextFromPage()
		_text = SummerNote.SummerNote.Base64Decode(Page.Request.Form(Me.htmlEditUniqueID & "_Hidden"))
	End Sub

#End Region

#Region "Helper Functions"

	''' <summary>
	''' The html editor instance. 
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The html editor instance.")>
	Public Function htmlEdit() As SummerNote.SummerNote
		If _htmlEdit Is Nothing Then
			_htmlEdit = New SummerNote.SummerNote
			_htmlEdit.ID = "editor_" & identifierString
			_htmlEdit.DefaultHTML = defaultText
			_htmlEdit.Text = ""
		End If

		Return _htmlEdit
	End Function

	''' <summary>
	''' SQL call information
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("SQL call information")>
	Protected Overridable Sub addSQLCall()
		parallelhelper.addFillDataTable("select top 1 * from DTIContentManager where mainID = @mainId and areaName = @contentType order by dateModified desc", contentTable, New Object() {MainID, contentType})
	End Sub

	''' <summary>
	''' Sets the contents row of the data objects.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Sets the contents row of the data objects.")>
	Protected Function setContentRow(Optional useDefaultMainIDIfNotFound As Boolean = True) As Boolean
		If ContentRow Is Nothing Then
			For Each row As dsEditPanel.DTIContentManagerRow In contentTable
				If row.areaName = contentType AndAlso row.MainID = MainID Then
					ContentRow = row
					Return True
				End If
			Next
			If useDefaultMainIDIfNotFound Then
				For Each row As dsEditPanel.DTIContentManagerRow In contentTable
					If row.areaName = contentType AndAlso row.MainID = 0 Then
						ContentRow = row
						Return True
					End If
				Next
			End If

		Else : Return True
		End If
		Return False
	End Function

	''' <summary>
	''' Sets the text of the datarow to the text value of the control.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Sets the text of the datarow to the text value of the control.")>
	Public Overridable Sub setText()
		Dim rowSet As Boolean = setContentRow()

		If rowSet Then
			If ContentRow.IscontentNull Then
				_text = ""
			Else
				_text = ContentRow.content
			End If
		Else
			If Not litText.Text Is Nothing AndAlso litText.Text.Trim <> "" Then
				_text = litText.Text
			Else
				_text = defaultText
			End If
		End If

		RaiseEvent textSet()
	End Sub

	''' <summary>
	''' Saves the contents of the control.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Saves the contents of the control.")>
	Protected Overridable Sub saveContent()
		Dim rowSet As Boolean = setContentRow(False)

		Dim dh As New dsEditPanel.DTIContentManagerHistoryDataTable
		Dim historyRow As dsEditPanel.DTIContentManagerHistoryRow = Nothing
		Dim historyRowDefTxt As dsEditPanel.DTIContentManagerHistoryRow = Nothing
		Dim Change As Boolean = True

		If rowSet Then
			If Not ContentRow.content = Text Then
				If ContentRow.IsDateModifiedNull Then
					ContentRow.DateModified = Date.Today
				End If
				historyRow = dh.AddDTIContentManagerHistoryRow(ContentRow, Text, ContentRow.MainID, ContentRow.areaName, Date.Now, False)
				ContentRow.content = Text
				ContentRow.DateModified = Date.Now
			Else
				Change = False
			End If
		Else
			If Not Text = defaultText Then
				historyRow = dh.AddDTIContentManagerHistoryRow(Nothing, Me.defaultText, MainID, contentType, Date.Now.AddSeconds(-1), False)
				historyRow = dh.AddDTIContentManagerHistoryRow(Nothing, Me.Text, MainID, contentType, Date.Now, False)
				ContentRow = contentTable.AddDTIContentManagerRow(Text, CType(MainID, Integer), contentType, Date.Now)
				'If SaveMediaRow Then
				'    MediaRow = SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow
				'    With MediaRow
				'        .Component_Type = "ContentManagement"
				'        .Content_Type = "editPanel"
				'        .Date_Added = Now
				'        .Permanent_URL = Me.Page.Request.Url.PathAndQuery
				'        .Published = True
				'        .Removed = False
				'        .User_Id = "-1" 'currentUser.Id
				'    End With
				'End If
			Else
				Change = False
			End If
		End If
		If Change Then
			sqlhelper.Update(contentTable, "DTIContentManager")
			If historyRowDefTxt IsNot Nothing Then historyRowDefTxt.DTIContentManagerID = ContentRow.id
			historyRow.DTIContentManagerID = ContentRow.id
			saveContentHistory(dh)
			'If SaveMediaRow AndAlso MediaRow IsNot Nothing Then
			'    MediaRow.Content_Id = ContentRow.id
			'    SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(MediaRow)
			'    sqlhelper.Update(MediaRow.Table)
			'End If
		End If

	End Sub

	Protected Sub saveContentHistory(ByVal dh As dsEditPanel.DTIContentManagerHistoryDataTable)
		Try
			sqlhelper.Update(CType(dh, DataTable))
		Catch ex As Exception
			If Not sqlhelper.checkDBObjectExists("DTIContentManagerHistory") Then
				sqlhelper.createTable(dh)
				sqlhelper.Update(CType(dh, DataTable))
			Else
				Throw ex
			End If
		End Try
	End Sub

	''' <summary>
	''' Changes the sizes of any dynamic images in the string to the size on the html page so the image size is the same as it appears to the client. 
	''' </summary>
	''' <param name="instring"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Changes the sizes of any dynamic images in the string to the size on the html page so the image size is the same as it appears to the client.")>
	Private Shared Function replaceimg(ByVal instring As String) As String
		If instring Is Nothing Then Return ""
		Dim regexopt As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled
		Dim imgList As Regex = New Regex("\<img.*?res\/DTIImageManager\/ViewImage\.aspx.*?\>", regexopt)
		Dim imgid As Regex = New Regex("(id\=(?<imageid>\d+))", regexopt)
		Dim imgheight As Regex = New Regex("(height\s?\:\s?(?<height>\d+))", regexopt)
		Dim imgwidth As Regex = New Regex("(width\s?\:\s?(?<width>\d+))", regexopt)
		Dim imgheight2 As Regex = New Regex("(height\=\p{P}(?<height>\d+))\p{P}", regexopt)
		Dim imgwidth2 As Regex = New Regex("(width\=\p{P}(?<height>\d+))\p{P}", regexopt)
		Dim imgsrc As Regex = New Regex("(\s+src\=\""(?<src>.*?)\"")", regexopt)
		Dim outstr As String = instring

		For Each imgm As Match In imgList.Matches(instring)
			Try
				Dim img As String = imgm.Value
				Dim height As Integer = -1
				Dim width As Integer = -1
				Dim id As Integer
				Dim src As String = imgsrc.Match(img).Groups("src").Value
				id = CType(imgid.Match(img).Groups("imageid").Value, Integer)
				If Not Integer.TryParse(imgheight.Match(img).Groups("height").Value, height) Then
					Integer.TryParse(imgheight2.Match(img).Groups("height").Value, height)
				End If
				If Not Integer.TryParse(imgwidth.Match(img).Groups("width").Value, width) Then
					Integer.TryParse(imgwidth2.Match(img).Groups("width").Value, width)
				End If
				Dim repSrc As String = "~/res/DTIImageManager/ViewImage.aspx?id=" & id
				If width > 0 Then repSrc &= "&Width=" & width
				If height > 0 Then repSrc &= "&Height=" & height
				If height > 0 OrElse width > 0 Then img = img.Replace(src, repSrc)
				outstr = outstr.Replace(imgm.Value, img)
			Catch ex As Exception
			End Try

		Next
		Return outstr
	End Function

	Private Sub setMode()
		If mypage Is Nothing Then mypage = Me.Page
		If Mode = modes.Write Then
			'ShowBorder = True

			Dim vidMgmt As New DTIVideoManager.VideoViewerControl
			vidMgmt.registerSWFJSFile(Me.Page)

			If Not MyHighslideHeader.HighslideVariables.Contains("zIndexCounter") Then
				MyHighslideHeader.HighslideVariables.Add("zIndexCounter", 20000)
			Else
				MyHighslideHeader.HighslideVariables.Item("zIndexCounter") = 20000
			End If

			If _text <> defaultText Then htmlEdit.Text = _text
			Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
			setuphtmleditor()
			If litText IsNot Nothing Then litText.Visible = False

		Else
			'ShowBorder = False

			registerVideos()
			litText.Visible = True
			litText.Text = Text
			If litText.Parent Is Nothing Then
				Me.Controls.Add(litText)
			End If


			If htmlEdit() IsNot Nothing Then htmlEdit.Visible = False
		End If

	End Sub


	'add video replacer for read mode
	Private Sub registerVideos()
		If Text.Contains("ViewVideoScreenShot.aspx?insertvideo") Then
			Dim vidMgmt As New DTIVideoManager.VideoViewerControl
			vidMgmt.registerSWFJSFile(mypage)
			Dim vidDivRegex As Regex = New Regex(
				  "<img[\x00-\x7E]*?ViewVideoScreenShot\.aspx\?insertvideo[\x00-\x7E]*?>",
				RegexOptions.IgnoreCase _
				Or RegexOptions.Multiline _
				Or RegexOptions.Compiled
				)

			Dim widthRegex As New Regex("width\s*\:\s*(?<cap>\d*)", RegexOptions.IgnoreCase)
			Dim heightRegex As New Regex("height\s*\:\s*(?<cap>\d*)", RegexOptions.IgnoreCase)
			Dim idRegex As New Regex("ViewVideoScreenShot\.aspx\?insertvideo.*?id\=(?<cap>\d*)", RegexOptions.IgnoreCase)

			Dim i As Integer = 0
			For Each vid As Match In vidDivRegex.Matches(Text)
				Try
					i += 1
					Dim id As Integer = idRegex.Match(vid.Value).Groups("cap").Value
					Dim height As Integer = -1
					Dim width As Integer = -1
					Try
						width = widthRegex.Match(vid.Value).Groups("cap").Value
					Catch ex As Exception
					End Try
					Try
						height = heightRegex.Match(vid.Value).Groups("cap").Value
					Catch ex As Exception

					End Try

					vidMgmt.VideoId = id
					Dim idstr As String = "Video_" & Me.ClientID & "_" & i
					Text = Text.Replace(vid.Value, "<span id='" & idstr & "'>" & vid.Value & "</span>")
					vidMgmt.registerInitScript(idstr, mypage, width, height, id)
				Catch ex As Exception

				End Try

			Next
		End If
	End Sub

	Private Sub setuphtmleditor()
		Dim dlg2 As New SummerNote.SummerNote.IframeDialog()
		dlg2.buttonClass = "fa fahistory"
		dlg2.title = "History Tool"
		'dlg2.IframeURL = "~/res/DTIContentManagement/History.aspx?c=' + $(a.element.$).parent('.dtiClickEdit').attr('contentType') + '&m=' + $(a.element.$).parent('.dtiClickEdit').attr('mainId') + '"
		dlg2.url = "~/res/DTIContentManagement/History.aspx?c=" & Me.contentType & "&m=" & Me.MainID
		dlg2.buttonClass = "fa fa-repeat"

		'dlg2.DialogHeight = 400
		'dlg2.DialogWidth = 620
		'dlg2.ToolTip = "History Tool"
		'htmlEdit.Toolbar.Add("history")
		If htmlEdit.Page Is Nothing Then
			Me.Controls.Add(htmlEdit)
		End If
		htmlEdit.dialogList.Add(dlg2)

		Dim dlg As New SummerNote.SummerNote.IframeDialog()
		dlg.buttonClass = "fa fa-picture-o"
		dlg.title = "Media Uploader"
		'dlg.IframeURL = "~/res/DTIContentManagement/MediaList.aspx"
		dlg.url = mediaListURL
		'htmlEdit.Toolbar.Add("uploader")
		htmlEdit.dialogList.Add(dlg)

		'htmlEdit.myManager.useLastActiveEditor = False

		'htmlEdit.ToolbarMode = Summernote.Summernote.ToolbarModes.PageTop

		'If ShowBorder Then
		'    htmlEdit.BorderStyle = UI.WebControls.BorderStyle.Dashed
		'    htmlEdit.BorderColor = Drawing.Color.Gray
		'    htmlEdit.BorderWidth = New WebControls.Unit("3px")
		'    htmlEdit.Attributes.Add("onmouseover", "style.borderColor='#FF2222'")
		'    htmlEdit.Attributes.Add("onmouseout", "style.borderColor='#777777'")
		'End If
		htmlEdit.Attributes("style") &= "cursor:hand"
        'htmlEdit.Attributes("style") &= "cursor:pointer;cursor:hand"
        htmlEdit.Attributes.Add("contentType", contentType)
        htmlEdit.Attributes.Add("mainId", MainID)
		'      If ShowBorder Then
		'          htmlEdit.ClientReady = "$($(this).parent()).css('border','none');" 'setPageCss($(this).get(0));"
		'	htmlEdit.ClientDestroyed = "$($(this).parent()).css('border','2px dashed #777777');"
		'End If

		Dim clearDiv As New Panel
        clearDiv.Attributes.Add("style", "clear:both;")
        htmlEdit.Controls.Add(clearDiv)
        MyHighslideHeader.isOuterFrame = True

    End Sub

#End Region

    Private Sub EditPanel_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        Dim ds As New dsEditPanel
		Dim mediaDs As New DTIMediaManager.dsMedia
		Dim imageDs As New DTIImageManager.dsImageManager
		sqlhelper.checkAndCreateTable(ds.DTIContentManager)
        sqlhelper.checkAndCreateTable(ds.DTIContentManagerHistory)
        sqlhelper.checkAndCreateTable(mediaDs.DTIMediaManager)
		sqlhelper.checkAndCreateTable(mediaDs.DTIMediaTypes)
		sqlhelper.checkAndCreateTable(imageDs.DTIImageManager)
	End Sub

    'Private Sub EditPanel_ModeChanged() Handles Me.ModeChanged
    '    If Mode = modes.Write Then
    '        ShowBorder = True
    '    Else
    '        ShowBorder = False
    '    End If
    'End Sub

End Class