<ComponentModel.Designer(GetType(DTIServerControls.DTIServerBaseDesigner)), ViewStateModeById()> _
Public Class serverControl
    Inherits DTIServerControls.DTIServerControl


    Private lbl As New Label
    Private lbl2 As New Label


    Public Sub New()
        Me.settingsPageUrl = "settingsForm.aspx"
        Me.useGenericDTIControlsProperties = True
    End Sub

    Public Property something() As String
        Get
            Return lbl.Text
        End Get
        Set(ByVal value As String)
            lbl.Text = value
        End Set
    End Property

    Public Property sessionProp() As String
        Get
            Return Session("Hi")
        End Get
        Set(ByVal value As String)
            Session("Hi") = value
        End Set
    End Property

    Private Sub serverControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lbl2.Text = "Dynamic text entered below:<br>"
        Me.Controls.Add(lbl2)
        Me.Controls.Add(lbl)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        'Throw New Exception("AAAA")
        MyBase.Render(writer)
    End Sub

End Class
