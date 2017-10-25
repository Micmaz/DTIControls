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
Public Class ckEditor
    Inherits Panel
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ckEditor
        Inherits Panel
#End If

    ''' <summary>
    ''' The save button on the html editor is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The save button on the html editor is clicked.")> _
    Public Event Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)

    ''' <summary>
    ''' the cancelbutton o the html editor is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("the cancelbutton o the html editor is clicked.")> _
        Public Event Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)

#Region "Private Controls"
    Private hfHTML As New HiddenField
        Private litHTML As New Literal
        Private phDialogues As New PlaceHolder
        Private WithEvents save As New Button
        Private WithEvents cancel As New Button
    Private htmlDiv As New Panel
        Private scriptLit As New ScriptBlock

#End Region

#Region "Properties"

    ''' <summary>
    ''' Control id.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Control id.")> _
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
    <System.ComponentModel.Description("Client side attributes added to control."),Browsable(False)> _
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
    <System.ComponentModel.Description("Edit area width.")> _
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
            htmlDiv.Attributes("CKEDITOR.config.width") = value.Value
            If value.Type = UnitType.Percentage Then
                htmlDiv.Attributes("CKEDITOR.config.width") = value.Value & "%"
            End If
        End Set
    End Property

    ''' <summary>
    ''' Edit area Height.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Edit area Height.")> _
    Public Shadows Property Height() As Unit
        Get
            EnsureChildControls()
            Return MyBase.Height 'divCell.Height
        End Get
        Set(ByVal value As Unit)
            EnsureChildControls()
            MyBase.Height = value
            'divCell.Height = value
            htmlDiv.Attributes("CKEDITOR.config.Height") = value.Value
            If value.Type = UnitType.Percentage Then
                htmlDiv.Attributes("CKEDITOR.config.Height") = value.Value & "%"
            End If
        End Set
    End Property

    ''' <summary>
    ''' The tag added to the htmlcode when enter is pressed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The tag added to the htmlcode when enter is pressed.")> _
    Public Shadows Property Entermode() As entermodetag
        Get
            EnsureChildControls()
            Return htmlDiv.Attributes("entermode")
        End Get
        Set(ByVal value As entermodetag)
            EnsureChildControls()
            htmlDiv.Attributes("entermode") = value
        End Set
    End Property

    ''' <summary>
    ''' Enumeration of tag added to the htmlcode when enter is pressed.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enumeration of tag added to the htmlcode when enter is pressed.")> _
    Public Enum entermodetag
        p = 1
        br = 2
        div = 3
    End Enum



    <Browsable(False)> _
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
    <System.ComponentModel.Description("Disables the cancel and save buttons and removes them from the editor.")> _
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
    <System.ComponentModel.Description("Disables the cancel button.")> _
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
    <System.ComponentModel.Description("Disables the save button.")> _
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
    <System.ComponentModel.Description("The contents of the editor if the text property is empty.")> _
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
    <System.ComponentModel.Description("The html text contents of the editor.")> _
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
    <System.ComponentModel.Description("Javascript function that is called before the editor is created.")> _
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
    <System.ComponentModel.Description("Javascript function that is called once the editor is created")> _
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
    <System.ComponentModel.Description("Javascript function that is called once before the editor is destroyed.")> _
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
    <System.ComponentModel.Description("Javascript function that is called after the client after the editor is destroyed.")> _
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

    <Browsable(False)> _
    Private _toolbar As Toolbar = New Toolbar

    ''' <summary>
    ''' The toolbar and buttns for this editor.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The toolbar and buttns for this editor.")> _
    Public ReadOnly Property Toolbar() As Toolbar
        Get
            Return _toolbar
        End Get
    End Property

    ''' <summary>
    ''' Enum of toolbar locations.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enum of toolbar locations.")> _
    Public Enum ToolbarModes
        Normal
        PageTop
        'Floating
    End Enum

    Private _Toolbarlayout As Toolbar.ToolbarLayout = Toolbar.ToolbarLayout.FullToolbar

    ''' <summary>
    ''' Pre-made layout of toolbar tools and groups. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Pre-made layout of toolbar tools and groups.")> _
    Public Property Toolbarlayout() As Toolbar.ToolbarLayout
        Get
            Return _Toolbarlayout
        End Get
        Set(ByVal value As Toolbar.ToolbarLayout)
            _Toolbarlayout = value
            _toolbar = Toolbar.ToolbarFromLayout(value)
        End Set
    End Property

    Private _toolMode As ToolbarModes = ToolbarModes.PageTop

    ''' <summary>
    ''' Determines weather the toolbar is rendered over the editor or at the top of the screen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines weather the toolbar is rendered over the editor or at the top of the screen.")> _
    Public Property ToolbarMode() As ToolbarModes
        Get
            Return _toolMode
        End Get
        Set(ByVal value As ToolbarModes)
            _toolMode = value
        End Set
    End Property

    Private _myMngr As ckEditorManager

    ''' <summary>
    ''' The editorManager that is automatically added to the page for multiple editors.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The editorManager that is automatically added to the page for multiple editors."),Browsable(False)> _
    Public ReadOnly Property myManager() As ckEditorManager
        Get
            If _myMngr Is Nothing Then
                'Page.Items.Item("ckmanager") = _myMngr
                'Dim tmp As ckEditorManager = BaseClasses.Spider.spiderPageForType(Me.Page, GetType(ckEditorManager))
                If Page.Items.Item("ckmanager") Is Nothing Then
                    Page.Items.Item("ckmanager") = New ckEditorManager()
                    _myMngr = Page.Items.Item("ckmanager")
                    phDialogues.Controls.Add(_myMngr)
                    'Else
                    '    _myMngr = tmp
                End If
                _myMngr = Page.Items.Item("ckmanager")
            End If
            Return _myMngr
        End Get
    End Property

#End Region

    ''' <summary>
    ''' set toolbar to the default full toolbar.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("set toolbar to the default full toolbar.")> _
    Public Sub ResetToolbar()
        Toolbarlayout = Global.ckEditor.Toolbar.ToolbarLayout.FullToolbar
        'Toolbar.Clear()
        'Toolbar.Add(New ToolbarGroup(New String() {"Source"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Save"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"ShowBlocks"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Cut", "Copy", "Paste", "PasteText", "PasteFromWord", "RemoveFormat"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Undo", "Redo"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Find"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Styles", "Format", "Font", "FontSize"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"TextColor", "BGColor"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Bold", "Italic", "Underline", "Strike"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Subscript", "Superscript"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"NumberedList", "BulletedList"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Outdent", "Indent"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Link", "Unlink", "Anchor"}))
        'Toolbar.Add(New ToolbarGroup(New String() {"Image", "Table", "HorizontalRule", "SpecialChar", "Flash"}))
        ''Toolbar.Add(New ToolbarGroup(New String() {"uploader"}))
    End Sub

    Private Sub ckEditor_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.base64.js", , True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.form.js", , True)
        jQueryLibrary.jQueryInclude.addScriptBlock(Page, "CKEDITOR_BASEPATH = '~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/';")
        jQueryLibrary.jQueryInclude.addScriptFile(Page, "ckEditor/contents.css")
        jQueryLibrary.jQueryInclude.addScriptFile(Page, "ckEditor/ckeditor.js")
        jQueryLibrary.jQueryInclude.addScriptBlock(Page, "CKEDITOR.editorConfig = function (config) {config.enterMode = CKEDITOR.ENTER_BR; config.extraPlugins += 'onchange'; config.extraPlugins += 'imagemap'; }; ")
        'jQueryLibrary.jQueryInclude.addScriptFile(Page, "ckEditor/adapters.jquery.js")

        jQueryLibrary.jQueryInclude.addScriptFile(Page, "ckEditor/DTIckEditor.js")



        changeInnerControlIds()
        ResetToolbar()
        htmlDiv.CssClass &= " dtiContentEdit"

        save.Text = "Save"
        cancel.Text = "Cancel"
        save.CssClass &= " dtiCKEButton Save"
        cancel.CssClass &= " dtiCKEButton Cancel"
        save.Style("display") = "none"
        cancel.Style("display") = "none"

        changeInnerControlIds()
        htmlDiv.Controls.Add(litHTML)
        save.OnClientClick = "setHtmlEditorValue(CKEDITOR.instances[$(this).parent().parent().find('.dtiContentEdit').attr('id')]);"
        Me.Controls.Add(htmlDiv)
        Me.Controls.Add(phDialogues)
        Me.Controls.Add(hfHTML)
        Me.Controls.Add(save)

        Me.Controls.Add(scriptLit)
        Me.Controls.Add(cancel)

    End Sub

    Private Sub ckEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '    changeInnerControlIds()
        'If ToolbarMode = ToolbarModes.PageTop Then
        'Dim cssText As String = ".cke_toolbox {	position:fixed;_position:absolute;top:0px;left:0px;_top:expression(eval(document.body.scrollTop+0));margin-left:5px;margin-right:5px;width:98%;z-index:10001;}.cke_top{height:0px;}a.cke_toolbox_collapser_min{display:none;}"
        'Dim cssText As String = "#toolbardiv {-moz-border-radius:5px;-webkit-border-radius:5px;background-color:#F5F5F5;border: .5px solid #EBEBEB;position:fixed;_position:absolute;top:0px;left:-5px;_top:expression(eval(document.body.scrollTop+0));padding:0px;width:100%;z-index:10001;}.cke_top{height:0px;}a.cke_toolbox_collapser_min{display:none;}"
        'jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, cssText, False, "text/css")
        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "toolTopCSS", "<STYLE type=""text/css"">" & cssText & "</STYLE>", False)
        'End If
    End Sub

    Private Sub ckEditor_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'changeInnerControlIds()
        setScript()
        'hfHTML.ReferenceId = htmlDiv.ClientID
        'hfHTML.Value = System.Web.HttpUtility.UrlEncode( CType(htmlDiv.Controls(0), LiteralControl).Text)
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
		hfHTML.Value = System.Web.HttpUtility.UrlEncode(litHTML.Text)
	End Sub


    Private Sub changeInnerControlIds()
        If String.IsNullOrEmpty(Me.ID) Then
            Me.ID = Me.ClientID.Substring(Me.ClientID.LastIndexOf("_") + 1)
        End If
        save.ID = Me.ID & "_Save"
        cancel.ID = Me.ID & "_Cancel"
        htmlDiv.ID = Me.ID & "_HTML"
        scriptLit.ID = Me.ID & "_Script"
        hfHTML.ID = Me.ID & "_Hidden"
    End Sub

    Private Sub setScript()
        scriptLit.ScriptText =
        "CKEDITOR.config.toolbar_" & htmlDiv.ClientID & " = " & vbCrLf & _
        Toolbar.ToString & ";"
        If Not String.IsNullOrEmpty(BeforeReady) Then
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickBeforeCreate', function(e, divid){" & vbCrLf & BeforeReady & vbCrLf & "});"
        End If
        If Not String.IsNullOrEmpty(ClientReady) Then
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickCreated', function(e, divid, editor){" & vbCrLf & "dtiMoveInSaves(divid);" & vbCrLf & ClientReady & vbCrLf & "});"
        Else
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickCreated', function(e, divid, editor){" & vbCrLf & "dtiMoveInSaves(divid);" & vbCrLf & "});"
        End If
        If Not String.IsNullOrEmpty(BeforeClientDestroyed) Then
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickDestroyed', function(e, divid){" & vbCrLf & BeforeClientDestroyed & vbCrLf & "});"
        End If
        If Not String.IsNullOrEmpty(ClientDestroyed) Then
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickBeforeDestroy', function(e, divid){" & vbCrLf & "dtiMoveOutSaves(divid);" & vbCrLf & ClientDestroyed & vbCrLf & "});"
        Else
            scriptLit.ScriptText &= vbCrLf & "$('#" & Me.ClientID & "').bind('dtickBeforeDestroy', function(e, divid){" & vbCrLf & "dtiMoveOutSaves(divid);" & vbCrLf & "});"
        End If

        If ToolbarMode = ToolbarModes.PageTop Then
            scriptLit.ScriptText &= "CKEDITOR.inline( '" & htmlDiv.ClientID & "', {sharedSpaces: {top: 'topToolbar'}});"
        Else
            scriptLit.ScriptText &= "CKEDITOR.inline( '" & htmlDiv.ClientID & "', {});"
        End If

    End Sub

    Private Function getAttribs() As String
        Dim out As String = ""
        For Each key As String In attribs.Keys
            out &= key & "= '" & attribs(key) & "';"
        Next
        Return out
    End Function

    'Public Function ReplaceStr(ByVal inStr As String, ByVal ParamArray StringReplacements() As Object)
    '    Dim EndStr As String = ""
    '    Dim outs As String() = inStr.Split(New String() {"##"}, StringSplitOptions.None)

    '    For i As Integer = 0 To UBound(outs)
    '        EndStr &= outs(i)
    '        If StringReplacements.Length > i AndAlso i < UBound(outs) Then
    '            EndStr &= StringReplacements(i)
    '        End If
    '    Next
    '    Return EndStr
    'End Function

    Private Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        Dim btnclientid As String = CType(sender, Button).ClientID
        If btnclientid.IndexOf(Me.ClientID) > -1 Then
            litHTML.Text = System.Web.HttpUtility.UrlDecode(hfHTML.Value)
            RaiseEvent Save_Clicked(sender, e)
        End If
    End Sub

    Private Sub cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel.Click
        Dim btnclientid As String = CType(sender, Button).ClientID
        If btnclientid.IndexOf(Me.ClientID) > -1 Then
            RaiseEvent Cancel_Clicked(sender, e)
        End If
    End Sub

    ''' <summary>
    ''' Add a dialogue toolbar to the editor and optionaly open it with the button defined in the iframeDialogue object.
    ''' </summary>
    ''' <param name="dlg"></param>
    ''' <param name="addButtonToToolbar"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Add a dialogue toolbar to the editor and optionaly open it with the button defined in the iframeDialogue object.")> _
    Public Sub addIframeDialog(ByRef dlg As IframeDialog, Optional ByVal addButtonToToolbar As Boolean = True)
        'dlg.Name = dlg.Name & Me.ClientID
        If addButtonToToolbar Then
            Toolbar.Add(dlg.Name)
        End If
        For Each ctrl As Control In myManager.Controls()
            If TypeOf ctrl Is IframeDialog Then
                Dim ctrldlg As IframeDialog = CType(ctrl, IframeDialog)
                If ctrldlg.Name = dlg.Name Then
                    Exit Sub
                End If
            End If
        Next

        myManager.Controls.Add(dlg)
    End Sub

    ''' <summary>
    ''' Add a dialogue to the editor and bind it to a button in the editor.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="iconpath"></param>
    ''' <param name="dialogtitle"></param>
    ''' <param name="height"></param>
    ''' <param name="width"></param>
    ''' <param name="tooltip"></param>
    ''' <param name="url"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Add a dialogue to the editor and bind it to a button in the editor.")> _
    Public Sub addIframeDialog(ByVal name As String, ByVal iconpath As String, ByVal dialogtitle As String, ByVal height As Integer, ByVal width As Integer, ByVal tooltip As String, ByVal url As String)
        addIframeDialog(New IframeDialog(name, iconpath, dialogtitle, height, width, tooltip, url))
    End Sub
	
    Public Sub ParseToolbar(s As String) 
        Toolbar.ParseToolbar(s, Me.Toolbar)
    End Sub


End Class
