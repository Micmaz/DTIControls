Public Class Dialog
    Inherits Panel

#Region "properties"
    Private _autoOpen As Boolean
    Public Property AutoOpen() As Boolean
        Get
            Return _autoOpen
        End Get
        Set(ByVal value As Boolean)
            _autoOpen = value
        End Set
    End Property

    Private _effect As Effect = JqueryUIControls.Effect.fade
    Public Property ShowEffect() As Effect
        Get
            Return _effect
        End Get
        Set(ByVal value As Effect)
            _effect = value
        End Set
    End Property

    Private _openerText As String = ""
    Public Property OpenerText() As String
        Get
            Return _openerText
        End Get
        Set(ByVal value As String)
            _openerText = value
        End Set
    End Property

    Private _openerAdditionalText As String = ""
    Public Property OpenerAttributes() As String
        Get
            Return _openerAdditionalText
        End Get
        Set(ByVal value As String)
            _openerAdditionalText = value
        End Set
    End Property

    Public Enum DialogOpener
        Link
        Button
    End Enum

    Private _openerType As DialogOpener
    Public Property OpenerType() As DialogOpener
        Get
            Return _openerType
        End Get
        Set(ByVal value As DialogOpener)
            _openerType = value
        End Set
    End Property

    Private _draggable As Boolean = True
    Public Property Draggable() As Boolean
        Get
            Return _draggable
        End Get
        Set(ByVal value As Boolean)
            _draggable = value
        End Set
    End Property

    Private _modal As Boolean
    Public Property Modal() As Boolean
        Get
            Return _modal
        End Get
        Set(ByVal value As Boolean)
            _modal = value
        End Set
    End Property

    Private _resizable As Boolean = True
    Private Property Resizable() As Boolean
        Get
            Return _resizable
        End Get
        Set(ByVal value As Boolean)
            _resizable = value
        End Set
    End Property

    Private _title As String = ""
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _url As String = ""
    Public Property Url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
            If value IsNot Nothing AndAlso value.Length > 0 Then
                Me.Style.Add("overflow", "hidden")
            End If
        End Set
    End Property

    Private _buttons As Collection
    Private ReadOnly Property Buttons() As Collection
        Get
            If _buttons Is Nothing Then
                _buttons = New Collection
            End If
            Return _buttons
        End Get
    End Property

    Private _maximizeOnDoubleClick As Boolean = True
    Public Property maximizeOnDoubleClick() As Boolean
        Get
            Return _maximizeOnDoubleClick
        End Get
        Set(ByVal value As Boolean)
            _maximizeOnDoubleClick = value
        End Set
    End Property

#End Region

#Region "callbacks"
    Private _onOpenCallback As String = ""
    Public Property onOpenCallback() As String
        Get
            Return _onOpenCallback
        End Get
        Set(ByVal value As String)
            _onOpenCallback = value
        End Set
    End Property

    Private _onCreateCallback As String = ""
    Public Property onCreateCallback() As String
        Get
            Return _onCreateCallback
        End Get
        Set(ByVal value As String)
            _onCreateCallback = value
        End Set
    End Property

    Private _onCloseCallback As String = ""
    Public Property OnCloseCallback() As String
        Get
            Return _onCloseCallback
        End Get
        Set(ByVal value As String)
            _onCloseCallback = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.dialogHelper.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.dialogextend.pack.js")
        End If
    End Sub

    Private Sub control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Function renderparamsExtended() As String
        Dim outstr As String = ""
        If maximizeOnDoubleClick Then
            outstr = ".dialogExtend({'dblclick' : 'maximize' })"
        End If
        Return outstr
    End Function


    Private Function renderparams() As String
        Dim outstr As String = ""
        outstr &= "autoOpen: " & AutoOpen.ToString.ToLower & ","

        If Url <> "" OrElse onOpenCallback <> "" OrElse Modal Then
            outstr &= "open: function(event,ui){"
            If Modal Then
                outstr &= "$('body').first().css('overflow', 'hidden');"
            End If
            If Url <> "" Then
                outstr &= "openDialogHelper($(this),'" & Me.ClientID & "','" & Url & "');"
                If Height = Nothing Then
                    Height = 400
                End If
            End If
            outstr &= onOpenCallback & "},"
        End If

        If onCreateCallback <> "" Then
            outstr &= "create: function(event,ui){" & onCreateCallback & "},"
        End If

        If OnCloseCallback <> "" OrElse Modal Then
            outstr &= "close: function(event,ui){"
            If Modal Then
                outstr &= "$('body').first().css('overflow', '');"
            End If
            outstr &= OnCloseCallback & "},"
        End If

        If Not Draggable Then
            outstr &= "draggable:false,"
        End If
        If Modal Then
            outstr &= "modal:true,"
        End If
        If Not Resizable Then
            outstr &= "resizable:false,"
        End If
        If Width <> Nothing Then
            outstr &= "width:" & Width.Value.ToString & ","
        End If
        If Height <> Nothing Then
            outstr &= "height:" & Height.Value.ToString & ","
        End If
        If Not ShowEffect = JqueryUIControls.Effect.none Then
            outstr &= "show:""" & [Enum].GetName(GetType(Effect), ShowEffect) & ""","
            outstr &= "hide:""" & [Enum].GetName(GetType(Effect), ShowEffect) & ""","
        End If

        If Buttons.Count > 0 Then
            outstr &= "buttons:{"
            For Each button As DialogButton In Buttons
                outstr &= button.renderButton() & ","
            Next
            outstr = outstr.TrimEnd(",")
            outstr &= "},"
        End If

        outstr = outstr.TrimEnd(",")
        Return outstr
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If OpenerText <> "" Then
            If OpenerType = DialogOpener.Link Then
                writer.Write(OpenerLink(OpenerText))
            Else
                writer.Write(OpenerButton(OpenerText))
            End If
        End If
        MyBase.Render(writer)
    End Sub


    Public Function OpenerButton(ByVal text As String) As String
        Return "<input type=""button"" " & OpenerAttributes & " onclick=""" & OpenerJavascript() & ";return false;"" value=""" & text & """ />"
    End Function

    Public Function OpenerLink(ByVal text As String) As String
        Return "<a href=""javascript:void(0)"" " & OpenerAttributes & " onclick=""" & OpenerJavascript() & """>" & text & "</a>"
    End Function

    Public Function OpenerJavascript() As String
		Return "$$('#" & Me.ClientID & "').dialog( 'open' );"
	End Function

    Public Sub addButton(ByRef button As Button, Optional ByVal CloseDialog As Boolean = True, Optional ByVal BeforeClickJavascript As String = "", Optional ByVal AfterClickJavascript As String = "")
        'Javascript = Javascript.TrimEnd(";") & ";"
        Dim script As String = "$('#" & button.ClientID & "').click();"
        button.Style.Add("display", "none")
        Me.Buttons.Add(New DialogButton(button.Text, BeforeClickJavascript & script & AfterClickJavascript, CloseDialog))
    End Sub

    Public Sub addButton(ByVal name As String, Optional ByVal CloseDialog As Boolean = True, Optional ByVal javascript As String = "")
        Me.Buttons.Add(New DialogButton(name, javascript, CloseDialog))
    End Sub

	'Public Sub addButtonFromIframe(ByVal ButtonID As String, ByVal ButtonName As String, Optional ByVal CloseDialog As Boolean = True, Optional ByVal javascript As String = "", Optional ByVal IframeName As String = "")
	'    If IframeName = "" Then
	'        IframeName = Me.ClientID & "_iframe"
	'    End If
	'    javascript &= ";$('#" & IframeName & "').contents().find('#" & ButtonID & "').click()"
	'    Me.onOpenCallback = "$('#" & IframeName & "').load(function(){$('#" & IframeName & "').contents().find('#" & ButtonID & "').css('display','none');});"
	'    Me.Buttons.Add(New DialogButton(ButtonName, javascript, CloseDialog))
	'End Sub

	Public Sub addOKButton(Optional ByVal closeDialog As Boolean = True, Optional ByVal javascript As String = "")
        Me.Buttons.Add(New DialogButton("Ok", javascript, closeDialog))
    End Sub

    Public Sub addCancelButton(Optional ByVal closeDialog As Boolean = True, Optional ByVal javascript As String = "")
        Me.Buttons.Add(New DialogButton("Cancel", javascript, closeDialog))
    End Sub

    Private Sub Dialog_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Title IsNot Nothing AndAlso Title <> "" Then
            Me.Attributes.Add("Title", Title)
        End If
		Dim s As String = ""
		s &= "     $('#" & Me.ClientID & "').dialog({"
        s &= renderparams()
        s &= "      })" & renderparamsExtended() & ";"
		s &= "     $('#" & Me.ClientID & "').parent().appendTo($('form:first'));"
		jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, s, False)
	End Sub

    Shared Function CreateDialogueUrl(ByVal url As String, ByVal linktext As String, Optional ByVal openType As DialogOpener = DialogOpener.Link, Optional ByVal width As Integer = 0, Optional ByVal height As Integer = 0, Optional ByVal openerAdditionalText As String = "") As Dialog
        Dim dlg As New Dialog
        dlg.Url = url
        dlg.OpenerText = linktext
        dlg.OpenerType = openType
        If width > 0 Then
            dlg.Width = width
        End If
        If height > 0 Then
            dlg.Height = height
        End If
        dlg.OpenerAttributes = openerAdditionalText
        Return dlg
    End Function

    Shared Function CreateDialogue(Optional ByVal linktext As String = "", Optional ByVal openType As DialogOpener = DialogOpener.Link, Optional ByVal width As Integer = 0, Optional ByVal height As Integer = 0, Optional ByVal openerAdditionalText As String = "") As Dialog
        Dim dlg As New Dialog
        dlg.OpenerText = linktext
        dlg.OpenerType = openType
        If width > 0 Then
            dlg.Width = width
        End If
        If height > 0 Then
            dlg.Height = height
        End If
        dlg.OpenerAttributes = openerAdditionalText
        Return dlg
    End Function


    Private Class DialogButton
        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _Javascript As String
        Public Property Javascript() As String
            Get
                Return _Javascript
            End Get
            Set(ByVal value As String)
                _Javascript = value
            End Set
        End Property

        Private _CloseDialog As Boolean = True
        Public Property CloseDialog() As Boolean
            Get
                Return _CloseDialog
            End Get
            Set(ByVal value As Boolean)
                _CloseDialog = value
            End Set
        End Property

        Public Sub New(ByVal name As String, Optional ByVal javascript As String = "", Optional ByVal closeDialog As Boolean = True)
            Me.Name = name
            Me.Javascript = javascript
            Me.CloseDialog = closeDialog
        End Sub

        Public Function renderButton() As String
            Dim close As String = ""
            If CloseDialog Then
                close = "$(this).dialog(""close"");"
            End If
            If Name.ToLower = "ok" Then
                Name = "Ok"
            ElseIf Name.ToLower = "cancel" Then
                Name = "Cancel"
            Else
                Name = """" & Name & """"
            End If
            If Not Javascript = "" Then
                Javascript = Javascript.Trim.TrimEnd(";") & ";"
            End If
            Return Name & ": function(){" & Javascript & close & "}"
        End Function
    End Class
End Class

