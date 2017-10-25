Imports BaseClasses
Imports System.Web.Services
Imports System.Web
Imports System.Web.SessionState
Imports System.Reflection
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.UI.WebControls

Imports System.Text
Imports System.IO
Imports System.Web.UI

Public Class SettingsEditorPage
    Inherits BaseClasses.BaseSecurityPage
    Public Event saveClicked()
    Friend WithEvents save As New Button
    'Private hidbox As New DTIMiniControls.HiddenFieldEncoded

    Private _suppress As Boolean = False
    Public Property supressWritingToHiddenDiv() As Boolean
        Get
            Return _suppress
        End Get
        Set(ByVal value As Boolean)
            _suppress = value
        End Set
    End Property

    Public ReadOnly Property uniqueIdentifier() As String
        Get
            Return Me.Request.QueryString("uniqueIdentifier")
        End Get
    End Property

    Public Shared ReadOnly Property SharedSession() As HttpSessionState
        Get
            Return HttpContext.Current.Session
        End Get
    End Property

    Public Shared ReadOnly Property sharedSqlHelper() As BaseHelper
        Get
            Return BaseClasses.DataBase.getHelper
        End Get
    End Property

    Public ReadOnly Property DTISeverControlTarget() As DTIServerControl
        Get
            Try
                Return Session(uniqueIdentifier)
            Catch ex As Exception
                Return Nothing
            End Try
        End Get

    End Property


    Public Function RenderServerControl(ByVal ctrl As Control) As String
        If supressWritingToHiddenDiv Then Return ""
        Dim sb As StringBuilder = New StringBuilder()
        Dim tw As StringWriter = New StringWriter(sb)
        Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)

        ctrl.RenderControl(hw)
        Return sb.ToString()
    End Function

    'Dim ctrlstr As String
    Dim iframeliteral As New LiteralControl()
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.DTISeverControlTarget Is Nothing Then Response.End()
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me)
        jQueryLibrary.jQueryInclude.addScriptFile(Me, "DTIServerControls/setParent.js")
        jQueryLibrary.jQueryInclude.addScriptFile(Me, "/DTIServerControls/default.css", "text/css")
        save.CssClass = "saveButton"
        Me.Form.Controls.Add(save)
        'Me.Form.Controls.Add(hidbox)
        save.Text = "Save"
        'Dim freezeID As String = Me.DTISeverControlTarget.Freezer.ClientID
        Dim parentID As String = Me.Request.QueryString("parentID")
        save.Attributes.Add("onclick", "freezeParent(); return true;")
        If IsPostBack Then

            'jQueryLibrary.jQueryInclude.addScriptBlock(Me, "$(function(){setParentContent('" & parentID & "','clientCtrl');})")
            'jQueryLibrary.jQueryInclude.addScriptBlock(Me, "$(function(){ parent.$('#" & parentID & "').replaceWith($('#" & hidbox.ClientID & "').val()); " & _
            '    "parent.unfreeze_" & Me.DTISeverControlTarget.ID & "(); " & _
            '    " });")
        End If

        If Not DTISharedVariables.LoggedIn Then
            Response.End()

        End If
        Me.Form.Controls.Add(iframeliteral)
    End Sub

    Public Sub saveControl()
        'ctrlstr = RenderServerControl(Me.DTISeverControlTarget)
        If DTISeverControlTarget.useGenericDTIControlsProperties Then
            Dim blankctrl As DTIServerControl = BaseClasses.AssemblyLoader.CreateInstance(DTISeverControlTarget.GetType.FullName)
            Dim ds As New dsDTIControls
            Dim controlRow As dsDTIControls.DTIControlRow

            sqlHelper.FillDataTable("select * from DTIControl where mainID=@mainid and Content_Type= @contentType", ds.DTIControl, DTISeverControlTarget.MainID, DTISeverControlTarget.contentType)
            sqlHelper.FillDataTable("select * from DTIControlProperty where DTIControlID in (select id from DTIControl where mainID=@mainid and Content_Type= @contentType)", ds.DTIControlProperty, DTISeverControlTarget.MainID, DTISeverControlTarget.contentType)
            If ds.DTIControl.Count = 0 Then
                controlRow = ds.DTIControl.AddDTIControlRow(Me.GetType.ToString, DTISeverControlTarget.contentType, DTISeverControlTarget.MainID)
                sqlHelper.Update(ds.DTIControl)
            Else
                controlRow = ds.DTIControl(0)
            End If
            Dim cmp As New DTIServerControls.Comparator
            cmp.MaxDepth = maxPropertySearchDepth
            cmp.DTIControlRow = controlRow
            cmp.differences = ds.DTIControlProperty
            cmp.addExclude(excludeProperties)
            cmp.Compare(blankctrl, DTISeverControlTarget)
            sqlHelper.Update(ds.DTIControlProperty)
            iframeliteral.Text = "<IFRAME ID=""clientCtrl"" SRC=""~/res/DTIServerControls/TmpControl.aspx?" & Request.QueryString.ToString & """ style=""position:absolute; visibility:hidden; left:0; top:0;height:5px;""></IFRAME>"
        End If
    End Sub

    Private Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        RaiseEvent saveClicked()
        saveControl()
    End Sub

    Private _maxPropertySearchDepth As Integer = 5
    Public Property maxPropertySearchDepth() As Integer
        Get
            Return _maxPropertySearchDepth
        End Get
        Set(ByVal value As Integer)
            _maxPropertySearchDepth = value
        End Set
    End Property

    Private _excludeProperties As String = "page,freezer,Mode,MyHighslideHeader,BorderStyle,useGenericDTIControlsProperties,SetupHighslide,ID,contentType,identifierString,contentType,MainID,showborder,borderwidth,borderheight"
    Public Property excludeProperties() As String
        Get
            Return _excludeProperties
        End Get
        Set(ByVal value As String)
            _excludeProperties = value
        End Set
    End Property

    'Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    If IsPostBack Then
    '        hidbox.Value = ctrlstr
    '    End If
    'End Sub
End Class