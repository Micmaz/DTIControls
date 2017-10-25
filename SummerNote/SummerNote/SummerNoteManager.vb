Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web
Imports DTIMiniControls

''' <summary>
''' manager class for keeping multiple SummerNote instances on one page.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class SummerNoteManager
    Inherits PlaceHolder
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
   Public  Class SummerNoteManager
        Inherits PlaceHolder
#End If
    Private script As New ScriptBlock
    Private hiddenField As New HiddenField
    Private curEditor As String = Nothing
    Private _useLastActive As Boolean = False

    ''' <summary>
    ''' The last editor activated on the page.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The last editor activated on the page."),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public ReadOnly Property lastActiveEditor() As String
        Get
            Return Me.Page.Request.Params("dtiCurrentFocusedEditor")
        End Get
    End Property

    ''' <summary>
    ''' The id of the editor that is currently active on the page.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The id of the editor that is currently active on the page.")> _
    Public Property ActiveEditor() As String
        Get
            Return curEditor
        End Get
        Set(ByVal value As String)
            curEditor = value
        End Set
    End Property


    'Public Property useLastActiveEditor() As Boolean
    '    Get
    '        Return _useLastActive
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _useLastActive = value
    '    End Set
    'End Property

    'Public Function addDialogue()

    'End Function

    'Public _dialogues As New Generic.Dictionary(Of String, IframeDialog)
    'Public ReadOnly Property dialogues() As Generic.Dictionary(Of String, IframeDialog)
    '    Get

    '    End Get
    'End Property


    Private Sub SummerNoteManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Controls.Add(hiddenField)
        hiddenField.ID = "dtiCurrentFocusedEditor"
    End Sub

    Private Sub setScript()
		'      Dim str As String = "addLoadEvent(getPageCss);" & vbCrLf
		'      str &= "addLoadEvent(addClickListener);" & vbCrLf
		'If _useLastActive AndAlso Me.lastActiveEditor IsNot Nothing Then
		'	str &= "addLoadEvent(function()" & vbCrLf
		'	str &= "{" & vbCrLf
		'	str &= vbTab & "ckeReplaceDiv(""" & Me.lastActiveEditor & """);" & vbCrLf
		'	str &= "});" & vbCrLf
		'ElseIf ActiveEditor IsNot Nothing Then
		'	str &= "addLoadEvent(function()" & vbCrLf
		'	str &= "{" & vbCrLf
		'	str &= vbTab & "ckeReplaceDiv(""" & Me.ActiveEditor & """);" & vbCrLf
		'	str &= "});" & vbCrLf
		'End If

	End Sub

    ''' <summary>
    ''' Overrides the render function of the panel.
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Overrides the render function of the panel.")> _
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        setScript()
        Me.script.RenderControl(writer)
        'For Each ctrl As Control In Me.Controls
        '    ctrl.RenderControl(writer)
        'Next
        MyBase.Render(writer)
    End Sub
End Class
