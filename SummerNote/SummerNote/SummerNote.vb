Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web
Imports System.ComponentModel
Imports DTIMiniControls

''' <summary>
''' WYSIWYG html editor.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class SummerNote
	Inherits Panel
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SummerNote
        Inherits Panel
#End If

	''' <summary>
	''' The save button on the html editor is clicked.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The save button on the html editor is clicked.")>
	Public Event Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)

	''' <summary>
	''' the cancelbutton o the html editor is clicked.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	<System.ComponentModel.Description("the cancelbutton o the html editor is clicked.")>
	Public Event Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Private Controls"
	Private hfHTML As New HiddenField
	Private litHTML As New Literal
	Private phDialogues As New PlaceHolder
	Private WithEvents save As New Button
	Private WithEvents cancel As New Button
	Private htmlDiv As New Panel
	'Private scriptLit As New Literal

#End Region

#Region "Properties"

	''' <summary>
	''' Control id.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Control id.")>
	Public Overrides Property ID() As String
		Get
			Return MyBase.ID
		End Get
		Set(ByVal value As String)
			MyBase.ID = value
			changeInnerControlIds()
		End Set
	End Property

	Private attribs As New Hashtable

	''' <summary>
	''' Client side attributes added to control.
	''' </summary>
	''' <param name="key"></param>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Client side attributes added to control."), Browsable(False)>
	Public Property attribute(ByVal key As String) As Object
		Get
			Return attribs(key)
		End Get
		Set(ByVal value As Object)
			attribs(key) = value
		End Set
	End Property

	''' <summary>
	''' Edit area width.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Edit area width.")>
	Public Shadows Property Width() As Unit
		Get
			EnsureChildControls()
			Return MyBase.Width
			'Return divCell.Width
		End Get
		Set(ByVal value As Unit)
			EnsureChildControls()
			MyBase.Width = value
			'divCell.Width = value
			'MyBase.Width = value
			htmlDiv.Attributes("width") = value.Value
			If value.Type = UnitType.Percentage Then
				htmlDiv.Attributes("width") = value.Value & "%"
			End If
		End Set
	End Property

	''' <summary>
	''' Edit area Height.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Edit area Height.")>
	Public Shadows Property Height() As Unit
		Get
			EnsureChildControls()
			Return MyBase.Height 'divCell.Height
		End Get
		Set(ByVal value As Unit)
			EnsureChildControls()
			MyBase.Height = value
			'divCell.Height = value
			htmlDiv.Attributes("Height") = value.Value
			If value.Type = UnitType.Percentage Then
				htmlDiv.Attributes("Height") = value.Value & "%"
			End If
		End Set
	End Property

	'''' <summary>
	'''' The tag added to the htmlcode when enter is pressed.
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("The tag added to the htmlcode when enter is pressed.")> _
	'Public Shadows Property Entermode() As entermodetag
	'    Get
	'        EnsureChildControls()
	'        Return htmlDiv.Attributes("entermode")
	'    End Get
	'    Set(ByVal value As entermodetag)
	'        EnsureChildControls()
	'        htmlDiv.Attributes("entermode") = value
	'    End Set
	'End Property

	'''' <summary>
	'''' Enumeration of tag added to the htmlcode when enter is pressed.
	'''' </summary>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("Enumeration of tag added to the htmlcode when enter is pressed.")> _
	'Public Enum entermodetag
	'    p = 1
	'    br = 2
	'    div = 3
	'End Enum



	<Browsable(False)>
	Private ReadOnly Property literal() As System.Web.UI.LiteralControl
		Get
			If MyBase.Controls.Count = 0 Then
				MyBase.Controls.Add(New System.Web.UI.LiteralControl())
			End If
			For Each ctrl As Control In MyBase.Controls
				If ctrl.GetType Is GetType(LiteralControl) Then
					Return ctrl
				End If
			Next
			Return New System.Web.UI.LiteralControl()
		End Get
	End Property

	'Public Property ButtonCssClass() As String
	'    Get
	'        EnsureChildControls()
	'        Return save.CssClass
	'    End Get
	'    Set(ByVal value As String)
	'        EnsureChildControls()
	'        save.CssClass = value
	'        cancel.CssClass = value
	'    End Set
	'End Property

	'Public Property DivCssClass() As String
	'    Get
	'        EnsureChildControls()
	'        Return htmlDiv.CssClass
	'    End Get
	'    Set(ByVal value As String)
	'        EnsureChildControls()
	'        htmlDiv.CssClass = value
	'    End Set
	'End Property

	''' <summary>
	''' Disables the cancel and save buttons and removes them from the editor.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Disables the cancel and save buttons and removes them from the editor.")>
	Public Property DisableButtons() As Boolean
		Get
			EnsureChildControls()
			Return Not (save.Visible Or cancel.Visible)
		End Get
		Set(ByVal value As Boolean)
			EnsureChildControls()
			save.Visible = Not value
			cancel.Visible = Not value
		End Set
	End Property

	''' <summary>
	''' Disables the cancel button.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Disables the cancel button.")>
	Public Property DisableCancel() As Boolean
		Get
			EnsureChildControls()
			Return Not cancel.Visible
		End Get
		Set(ByVal value As Boolean)
			EnsureChildControls()
			cancel.Visible = Not value
		End Set
	End Property

	''' <summary>
	''' Disables the save button.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Disables the save button.")>
	Public Property DisableSave() As Boolean
		Get
			EnsureChildControls()
			Return Not save.Visible
		End Get
		Set(ByVal value As Boolean)
			EnsureChildControls()
			save.Visible = Not value
		End Set
	End Property

	Private _DefaultHTML As String = "Insert Content Here"

	''' <summary>
	''' The contents of the editor if the text property is empty. 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The contents of the editor if the text property is empty.")>
	Public Property DefaultHTML() As String
		Get
			Return _DefaultHTML
		End Get
		Set(ByVal value As String)
			If _DefaultHTML = value Then Return
			If litHTML.Text = _DefaultHTML Then litHTML.Text = value
			_DefaultHTML = value
		End Set
	End Property

	''' <summary>
	''' The html text contents of the editor.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The html text contents of the editor.")>
	Public Property Text() As String
		Get
			EnsureChildControls()
			If litHTML.Text = DefaultHTML Then Return ""
			Return litHTML.Text
		End Get
		Set(ByVal value As String)
			EnsureChildControls()
			If value = "" Then value = DefaultHTML
			litHTML.Text = value
		End Set
	End Property

	Private _BeforeReady As String

	''' <summary>
	''' Javascript function that is called before the editor is created.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Javascript function that is called before the editor is created.")>
	Public Property BeforeReady() As String
		Get
			If String.IsNullOrEmpty(_BeforeReady) Then
				Return ""
			Else
				Return _BeforeReady
			End If
		End Get
		Set(ByVal value As String)
			_BeforeReady = value
		End Set
	End Property

	Private _ClientReady As String

	''' <summary>
	''' Javascript function that is called once the editor is created
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Javascript function that is called once the editor is created")>
	Public Property ClientReady() As String
		Get
			If String.IsNullOrEmpty(_ClientReady) Then
				Return ""
			Else
				Return _ClientReady
			End If
		End Get
		Set(ByVal value As String)
			_ClientReady = value
		End Set
	End Property

	Private _BeforeClientDestroy As String

	''' <summary>
	''' Javascript function that is called once before the editor is destroyed.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Javascript function that is called once before the editor is destroyed.")>
	Public Property BeforeClientDestroyed() As String
		Get
			If String.IsNullOrEmpty(_BeforeClientDestroy) Then
				Return ""
			Else
				Return _BeforeClientDestroy
			End If
		End Get
		Set(ByVal value As String)
			_BeforeClientDestroy = value
		End Set
	End Property

	Private _ClientDestroyed As String

	''' <summary>
	''' Javascript function that is called after the client after the editor is destroyed.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Javascript function that is called after the client after the editor is destroyed.")>
	Public Property ClientDestroyed() As String
		Get
			If String.IsNullOrEmpty(_ClientDestroyed) Then
				Return ""
			Else
				Return _ClientDestroyed
			End If
		End Get
		Set(ByVal value As String)
			_ClientDestroyed = value
		End Set
	End Property

	'<Browsable(False)> _
	'Private _toolbar As Toolbar = New Toolbar

	'''' <summary>
	'''' The toolbar and buttns for this editor.
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("The toolbar and buttns for this editor.")> _
	'Public ReadOnly Property Toolbar() As Toolbar
	'    Get
	'        Return _toolbar
	'    End Get
	'End Property

	''' <summary>
	''' Enum of toolbar locations.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Enum of toolbar locations.")>
	Public Enum ToolbarModes
		Normal
		PageTop
		'Floating
	End Enum

	'Private _Toolbarlayout As Toolbar.ToolbarLayout = Toolbar.ToolbarLayout.FullToolbar

	'''' <summary>
	'''' Pre-made layout of toolbar tools and groups. 
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'<System.ComponentModel.Description("Pre-made layout of toolbar tools and groups.")> _
	'Public Property Toolbarlayout() As Toolbar.ToolbarLayout
	'    Get
	'        Return _Toolbarlayout
	'    End Get
	'    Set(ByVal value As Toolbar.ToolbarLayout)
	'        _Toolbarlayout = value
	'        _toolbar = Toolbar.ToolbarFromLayout(value)
	'    End Set
	'End Property

	Private _toolMode As ToolbarModes = ToolbarModes.PageTop

	''' <summary>
	''' Determines weather the toolbar is rendered over the editor or at the top of the screen.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Determines weather the toolbar is rendered over the editor or at the top of the screen.")>
	Public Property ToolbarMode() As ToolbarModes
		Get
			Return _toolMode
		End Get
		Set(ByVal value As ToolbarModes)
			_toolMode = value
		End Set
	End Property


#End Region


	Private Sub SummerNote_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

		'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.base64.js", , True)
		'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.form.js", , True)

		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/bootstrap-iso.css")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/bootstrap.js")

		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/codemirror.css")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/monokai.css")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/codemirror.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/xml.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/formatting.js")

		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/css/font-awesome.min.css")

		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/summernote.min.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/summernote.css")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/plugins/summernote-video-attributes.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/plugins/summernote-ext-dialogHelper.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/plugins/summernote-image-attributes.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/sumernoteInit.js")
		jQueryLibrary.jQueryInclude.addScriptFile(Page, "SummerNote/summernoteAddl.css")

		'jQueryLibrary.jQueryInclude.addScriptBlock(Page, "SummerNote.editorConfig = function (config) {config.enterMode = SummerNote.ENTER_BR; config.extraPlugins += 'onchange'; config.extraPlugins += 'imagemap'; }; ")

		changeInnerControlIds()
		'ResetToolbar()
		htmlDiv.CssClass &= " dtiContentEdit"

		save.Text = "Save"
		cancel.Text = "Cancel"
		save.CssClass &= " dtiCKEButton Save"
		cancel.CssClass &= " dtiCKEButton Cancel"
		save.Style("display") = "none"
		cancel.Style("display") = "none"

		changeInnerControlIds()
		'Me.Controls.Add(scriptLit)
		htmlDiv.Controls.Add(litHTML)
		Me.Controls.Add(htmlDiv)
		Me.Controls.Add(hfHTML)
		Me.Controls.Add(save)

		Me.Controls.Add(cancel)

	End Sub

	Private Sub SummerNote_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Private Sub SummerNote_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

		setScript()
		Try
			CType(htmlDiv.Controls(0), LiteralControl).Text = hfHTML.Value
		Catch ex As Exception
		End Try

		htmlDiv.Attributes.Add("contenteditable", "true")
		If Not DesignMode AndAlso Not Me.Page.IsPostBack Then
			If litHTML.Text = "" Then litHTML.Text = DefaultHTML
		End If
		If HttpContext.Current.Response.Headers("X-XSS-Protection") Is Nothing Then
			HttpContext.Current.Response.AddHeader("X-XSS-Protection", 0)
		End If
		hfHTML.Value = Base64Encode(litHTML.Text.ToCharArray())  'System.Web.HttpUtility.UrlEncode(litHTML.Text)
	End Sub

	Public Shared Function Base64Encode(plainText As String) As String
		Dim plainTextBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(plainText)
		Return System.Convert.ToBase64String(plainTextBytes)
	End Function

	Public Shared Function Base64Decode(base64EncodedData As String) As String
		Dim plainTextBytes As Byte() = System.Convert.FromBase64String(base64EncodedData)
		Return System.Text.Encoding.UTF8.GetString(plainTextBytes)
	End Function

	Private Sub changeInnerControlIds()
		If String.IsNullOrEmpty(Me.ID) Then
			Me.ID = Me.ClientID.Substring(Me.ClientID.LastIndexOf("_") + 1)
		End If
		save.ID = Me.ID & "_Save"
		cancel.ID = Me.ID & "_Cancel"
		htmlDiv.ID = Me.ID & "_HTML"
		hfHTML.ID = Me.ID & "_Hidden"
	End Sub

	Public dialogList As New List(Of IframeDialog)


	Private Sub setScript()
		Dim out As String = ""
		'out = "<script>"
		'scriptLit.ScriptText &= vbCrLf & "$(function() { " & Me.ClientID & "= $('#" & Me.ClientID & "_HTML').summernote({" & getAttribs() & "})  });"
		out &= "  $(function(){" & vbCrLf
		If attribs.Count > 0 Then
			out &= "  DTISummernote.setOptions($('#" & htmlDiv.ClientID & "'),{" & vbCrLf
			out &= "    " & getAttribs()
			out &= "});"
		End If
		For Each iframdlg As IframeDialog In dialogList
			out &= iframdlg.getScript(Me)
		Next
		out &= "  DTISummernote.addButton($('#" & htmlDiv.ClientID & "'),'<i class=""note-icon-save""/>','Save',function(){DTISummernote.setHtmlEditorValues(); $('#" & save.ClientID & "').click();});"
		'scriptLit.ScriptText &= "  setOptions($('#" & Me.ClientID & "'),{toolbar: [['style', ['style', 'bold', 'italic', 'underline', 'clear']]]});"
		out &= "})" & vbCrLf
		'out &= "</script>"
		jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, out)
	End Sub

	Private Function getAttribs() As String
		Dim out As String = ""
		For Each key As String In attribs.Keys
			out &= key & ": '" & attribs(key).ToString() & "',"
		Next
		Return out.Trim(",")
	End Function

	Public Sub setTextFromPage()
		litHTML.Text = Base64Decode(hfHTML.Value)
	End Sub

	Private Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
		Dim btnclientid As String = CType(sender, Button).ClientID
		If btnclientid.IndexOf(Me.ClientID) > -1 Then
			setTextFromPage()
			RaiseEvent Save_Clicked(sender, e)
		End If
	End Sub

	Private Sub cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel.Click
		Dim btnclientid As String = CType(sender, Button).ClientID
		If btnclientid.IndexOf(Me.ClientID) > -1 Then
			RaiseEvent Cancel_Clicked(sender, e)
		End If
	End Sub

	Public Class IframeDialog
		Public url As String
		Public title As String
		Public buttonClass As String
		Public buttonGroup As String = Nothing
		Public Sub New()
		End Sub

		Public Sub New(url As String, title As String, buttonClass As String, Optional ButtonGroup As String = Nothing)
			Me.title = title
			Me.url = url
			Me.buttonClass = buttonClass
			Me.buttonGroup = ButtonGroup
		End Sub
		Public Function getScript(editor As SummerNote) As String
			Return "DTISummernote.addIframeDialogButton($('#" & editor.htmlDiv.ClientID & "'),'<i class=""" & buttonClass & """/>','" & title & "','" & url & "');"
		End Function
	End Class

End Class
