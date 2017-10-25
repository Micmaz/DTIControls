Imports BaseClasses
Imports DTIMapControl

#If DEBUG Then
Partial Public Class DTIMapEditUserControl
    Inherits DTIServerControls.DTISettingsControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class DTIMapEditUserControl
        Inherits DTIServerControls.DTISettingsControl
#End If

        Public Property address() As String
            Get
                Return tbAddress.Text
            End Get
            Set(ByVal value As String)
                tbAddress.Text = value
            End Set
        End Property

        Public Property location() As String
            Get
                Return tbTitle.Text
            End Get
            Set(ByVal value As String)
                tbTitle.Text = value
            End Set
        End Property

        Public Property google_key() As String
            Get
                Return tbGoogleKey.Text
            End Get
            Set(ByVal value As String)
                tbGoogleKey.Text = value
            End Set
        End Property

        Private _caller As DTIMapControl.DTIMapServerControl
        Public Property caller() As DTIMapControl.DTIMapServerControl
            Get
                Return _caller
            End Get
            Set(ByVal value As DTIMapControl.DTIMapServerControl)
                _caller = value
            End Set
        End Property

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'caller.saveAddress(address, location)
        'caller.saveGoogleKey(google_key)
            'Response.Redirect(Request.Url.OriginalString)
        End Sub
    End Class