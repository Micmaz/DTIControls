''' <summary>
''' Simplified Ajax enbaled panel
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIupdatepanel
    Inherits System.Web.UI.WebControls.Panel
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIupdatepanel
        Inherits System.Web.UI.WebControls.Panel
#End If

#Region "Properties"

        Private ht As New Hashtable
        Public Property JsFunctionSuccess() As String
            Get
                Return ht("success")
            End Get
            Set(ByVal value As String)
                ht("success") = value
            End Set
        End Property

        Public Property JsFunctionBeforeSubmit() As String
            Get
                Return ht("BeforeSubmit")
            End Get
            Set(ByVal value As String)
                ht("BeforeSubmit") = value
            End Set
        End Property

        Public Property JsFunctionBeforeSerialize() As String
            Get
                Return ht("BeforeSerialize")
            End Get
            Set(ByVal value As String)
                ht("BeforeSerialize") = value
            End Set
        End Property

#End Region

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Page.IsPostBack Then
                Page.Response.Clear()
                MyBase.Render(writer)
                Page.Response.End()
            Else

                MyBase.Render(writer)
                Dim script As New DTIMiniControls.ScriptBlock
                script.ScriptText = "var options" & Me.ClientID & " = {target: '#" & Me.ClientID & "',success: function() { $('#" & Me.ClientID & "').fadeIn('slow');  } "
                For Each key As String In ht.Keys
                    script.ScriptText = "," & key & ":" & ht(key)
                Next
                script.ScriptText &= "}" & vbCrLf

                Dim setEncodedScript As String = ""
                script.ScriptText &= _
                    "        $(document).ready(function() {  " & vbCrLf & _
                    "            $('form').ajaxForm(options" & Me.ClientID & ");  " & vbCrLf & _
                    "        });  " & vbCrLf
                script.ScriptText &= _
                    "       function __doPostBack(eventTarget, eventArgument) { " & vbCrLf & _
                    "        if (!theForm.onsubmit || (theForm.onsubmit() != false)) { " & vbCrLf & _
                    "        theForm.__EVENTTARGET.value = eventTarget; " & vbCrLf & _
                    "        theForm.__EVENTARGUMENT.value = eventArgument; " & vbCrLf & _
                    "        $('form').ajaxSubmit(options" & Me.ClientID & "); " & vbCrLf & _
                    "        } " & vbCrLf & _
                    "       }  " & vbCrLf
                script.RenderControl(writer)
            End If

        End Sub

        Private Sub jsonSeverConrol_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIAjax/jquery.form.js")
        End Sub

    End Class
