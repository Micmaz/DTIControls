Imports System.ComponentModel
Imports System.Web.UI
#If DEBUG Then
Public Class TextBoxValidated
    Inherits System.Web.UI.WebControls.TextBox
#Else
<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
<DefaultProperty("Text"), ToolboxData("<{0}:TextBoxValidated runat=server></{0}:TextBoxValidated>")> Public Class TextBoxValidated
    Inherits System.Web.UI.WebControls.TextBox
#End If
    Private WithEvents Validator As WebControls.RegularExpressionValidator

    Public Property ValidationExpression() As String
        Get
            Return Validator.ValidationExpression
        End Get
        Set(ByVal Value As String)
            Validator.ValidationExpression = Value
        End Set
    End Property
    Public Property ValidationText() As String
        Get
            Return Validator.ErrorMessage
        End Get
        Set(ByVal Value As String)
            Validator.ErrorMessage = Value
        End Set
    End Property

    Protected Overrides Sub Render(ByVal output As System.Web.UI.HtmlTextWriter)

        'Validator.CopyBaseAttributes(Me)

        MyBase.Render(output)
        'Validator.RenderControl(output)
        'Validator.RenderEndTag(output)

        'output.Write([Text])
    End Sub

    Public Sub New()
        Validator = New WebControls.RegularExpressionValidator
    End Sub

    Private Sub TextBoxValidated_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Init
        Validator.Page = Me.Page
        Me.Page.Controls.Add(Validator)

        Validator.ControlToValidate = Me.ID
        Validator.ErrorMessage = "Invalid Data"
        Validator.Display = WebControls.ValidatorDisplay.Dynamic
        Validator.EnableClientScript = True
    End Sub
End Class
