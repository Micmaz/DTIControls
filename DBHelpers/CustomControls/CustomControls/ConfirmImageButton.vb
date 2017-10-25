#If DEBUG Then
Public Class ConfirmImageButton
    Inherits System.Web.UI.WebControls.ImageButton
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ConfirmImageButton
        Inherits System.Web.UI.WebControls.ImageButton
#End If
        'Some things to note on this button class
        '1) Causes validation property on button must be set to false!
        '2) Instead, in the buttons server side click event use the following:
        '3)         Me.Validate()
        '4)         If Me.IsValid Then ... (Response.Redirect("Default.aspx"))
        '5) This will cause validation prior to button action

        Private _ButtonMessage As String

        'Public Shadows ReadOnly Property CausesValidation() As Boolean
        '    Get
        '        Return True
        '    End Get
        'End Property

        Public Property Message() As String
            Get
                Return _ButtonMessage
            End Get
            Set(ByVal Value As String)
                _ButtonMessage = Value
            End Set
        End Property

        Public Property DisableOnSubmit() As Boolean
            Get
                Return _DisableOnSubmit
            End Get
            Set(ByVal Value As Boolean)
                _DisableOnSubmit = Value
            End Set
        End Property
        Private _DisableOnSubmit As Boolean = False

        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            'Dim onclick As String
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ConfirmButton", _
                    "<script language=""JavaScript1.2"">" & vbCrLf & _
                    "function __doConfirm(btn,msg,dodisable){" & vbCrLf & _
                        "var answer; " & vbCrLf & _
                        "if(msg == ''){answer = true;}else{answer = confirm(msg);}" & vbCrLf & _
                        "if (answer){" & vbCrLf & _
                        "if (dodisable){" & vbCrLf & _
                            "var frm = btn.form; var value = btn.getAttribute('name');" & vbCrLf & _
                            "btn.disabled = true;" & vbCrLf & _
                            "frm.innerHTML += ""<input type='hidden' name='"" + value +""' />"";" & vbCrLf & _
                            "submitform(frm);" & vbCrLf & "}else{return true;}" & vbCrLf & "}" & vbCrLf & _
                        "return false;" & vbCrLf & _
                        "}" & vbCrLf & _
                    "function submitform(frm){" & vbCrLf & _
                    " if(frm.onsubmit()) { frm.submit(); }" & vbCrLf & _
                    "}" & vbCrLf & _
                    "</script>")

            MyBase.CausesValidation = False
            Dim dis As String = "false"
            If DisableOnSubmit Then
                dis = "true"
            End If
            Me.Attributes("onclick") = "return __doConfirm(this,'" & _ButtonMessage & "'," & dis & ");"
            'The MyBase keyword behaves like an object variable referring 
            'to the base class of the current instance of a class
            MyBase.OnPreRender(e)

        End Sub

        Private Function getScriptString() As String
            If Not Message Is Nothing AndAlso Message.Length > 0 Then
                Dim msg As String = Message.Replace("""", "\""")
                msg = msg.Replace("'", "\'")
                If DisableOnSubmit Then
                    Return "this.disabled = true; return confirm('" & msg & "');"
                Else
                    Return "return confirm('" & msg & "');"
                End If
            Else
                Return "this.disabled = true; "
            End If

        End Function

        Private Sub ConfirmButton_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init
            If Me.Attributes.Item("onClick") Is Nothing Then
                Me.Attributes.Add("onClick", getScriptString())
            End If

        End Sub

    End Class
