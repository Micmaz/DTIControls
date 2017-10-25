Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' Dialogue for custom tools in the editor.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class IframeDialog
    Inherits DTIMiniControls.ScriptBlock
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class IframeDialog
        Inherits DTIMiniControls.ScriptBlock
#End If
    Private _name As String

    ''' <summary>
    ''' The name of this dialogue. Also how it's accessed via the toolbar.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The name of this dialogue. Also how it's accessed via the toolbar.")> _
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _DialogTitle As String

    ''' <summary>
    ''' The title of the dialogue.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The title of the dialogue.")> _
    Public Property DialogTitle() As String
        Get
            Return _DialogTitle
        End Get
        Set(ByVal value As String)
            _DialogTitle = value
        End Set
    End Property

    Private _IconPath As String

    ''' <summary>
    ''' The icon for the toolbar button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The icon for the toolbar button.")> _
    Public Property IconPath() As String
        Get
            Return _IconPath
        End Get
        Set(ByVal value As String)
            _IconPath = value
        End Set
    End Property

    Private _IframeURL As String

    ''' <summary>
    ''' The url of the dialogue iframe.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The url of the dialogue iframe.")> _
    Public Property IframeURL() As String
        Get
            Return _IframeURL
        End Get
        Set(ByVal value As String)
            _IframeURL = value
        End Set
    End Property

    Private _DialogHeight As Integer = 300

    ''' <summary>
    ''' The height of the open dialogue in px.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The height of the open dialogue in px.")> _
    Public Property DialogHeight() As Integer
        Get
            Return _DialogHeight
        End Get
        Set(ByVal value As Integer)
            _DialogHeight = value
        End Set
    End Property

    Private _DialogWidth As Integer = 200

    ''' <summary>
    ''' The width of the open dialogue in px.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The width of the open dialogue in px.")> _
    Public Property DialogWidth() As Integer
        Get
            Return _DialogWidth
        End Get
        Set(ByVal value As Integer)
            _DialogWidth = value
        End Set
    End Property

    Private _DialogAction As String = ""


	''' <summary>
	''' Javascript command run when the dialogue opens.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Javascript command run when the dialogue opens.")> _
    Public Property DialogAction() As String
        Get
            Return _DialogAction
        End Get
        Set(ByVal value As String)
            _DialogAction = value
        End Set
    End Property

    Public Sub New(ByVal name As String, ByVal iconpath As String, ByVal dialogtitle As String, ByVal height As Integer, ByVal width As Integer, ByVal tooltip As String, ByVal url As String)
        Me.Name = name
        Me.IconPath = iconpath
        Me.DialogTitle = dialogtitle
        Me.IframeURL = url
        Me.DialogHeight = height
        Me.DialogWidth = width
        Me.ToolTip = tooltip
    End Sub

    Public Sub New()

    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        'If IframeURL.IndexOf("~") = 0 Then
        '    IframeURL = IframeURL.Substring(1)
        'End If
        'If IframeURL.IndexOf("/") = 0 Then
        '    _IframeURL = Me.Page.Request.Url.Host & IframeURL.Substring(1)
        'End If

        'If IconPath.IndexOf("~") = 0 Then
        '    IconPath = IconPath.Substring(1)
        'End If
        'If IconPath.IndexOf("/res/") > -1 Then
        '    IconPath = BaseClasses.Scripts.ScriptsURL & IconPath.Substring(1)
        'ElseIf IconPath.IndexOf("res/") > -1 Then
        '    IconPath = BaseClasses.Scripts.ScriptsURL & IconPath
        'End If

        Me.ScriptText = _
            "CKEDITOR.plugins.add('" & Name & "',{" & vbCrLf & _
            vbTab & "icons: '" & Name & "'," & vbCrLf & _
            vbTab & "init:function(a){" & vbCrLf & _
            vbTab & vbTab & "CKEDITOR.dialog.addIframe('" & Name & "_dialog', '" & DialogTitle & "','" & IframeURL & "'," & DialogWidth & "," & DialogHeight & ",function(){/*oniframeload*/})" & vbCrLf & _
            vbTab & vbTab & "var cmd = a.addCommand('" & Name & "', {exec:" & Name & "_onclick})" & vbCrLf & _
            vbTab & vbTab & "cmd.modes={wysiwyg:1,source:1}" & vbCrLf & _
            vbTab & vbTab & "cmd.canUndo = false" & vbCrLf & _
            vbTab & vbTab & "a.ui.addButton('" & Name & "',{ label:'" & ToolTip & "', command:'" & Name & "', icon:'" & IconPath & "' })" & vbCrLf & _
            vbTab & "}" & vbCrLf & _
            "})" & vbCrLf

        Me.ScriptText &= _
        "function " & Name & "_onclick(e)" & vbCrLf
        Me.ScriptText &= "{" & vbCrLf
        Me.ScriptText &= vbTab & "CKEDITOR.currentInstance.openDialog('" & Name & "_dialog');" & vbCrLf
        If Not String.IsNullOrEmpty(DialogAction) Then
            Me.ScriptText &= vbTab & DialogAction & vbCrLf
        End If
        Me.ScriptText &= "}" & vbCrLf

        Me.ScriptText &= _
        "CKEDITOR.config.extraPlugins += '" & Name & ",';"

        MyBase.Render(writer)
    End Sub
End Class
