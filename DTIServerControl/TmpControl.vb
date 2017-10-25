Imports System.Web.UI.WebControls
Public Class TmpControl
    Inherits DTIServerControlBasePage
    Protected WithEvents ph1 As PlaceHolder

    Public ReadOnly Property uniqueIdentifier() As String
        Get
            Return Me.Request.QueryString("uniqueIdentifier")
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

    Dim newctrl As DTIServerControl
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me)
        Dim t As Type = DTISeverControlTarget.GetType()
        newctrl = t.Assembly.CreateInstance(t.FullName)
        newctrl.MainID = DTISeverControlTarget.MainID
        newctrl.contentType = DTISeverControlTarget.contentType
        newctrl.ID = DTISeverControlTarget.ID
        'newctrl.disableFreezing()
        newctrl.Mode = DTIServerControl.modes.Read
        Me.ph1.Controls.Add(newctrl)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'newctrl.disableFreezing()
        newctrl.Mode = DTIServerControl.modes.Read
    End Sub
End Class