''' <summary>
''' Control to provide simple site searches of pages
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class SearchControl
    Inherits DTIServerControls.DTIServerControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SearchControl
        Inherits DTIServerControls.DTIServerControl
#End If
        'Public Overrides ReadOnly Property Menu_Icon_Url() As String
        '    Get
        '        Return BaseClasses.Scripts.ScriptsURL & "DTIContentManagement/searchIcon.png"
        '    End Get
        'End Property

        Private _searcher As SearchUserControl
        Public ReadOnly Property MySearchUserControl() As SearchUserControl
            Get
                If _searcher Is Nothing Then
                    _searcher = DirectCast(Page.LoadControl("~/res/DTIContentManagement/SearchUserControl.ascx"), SearchUserControl)
                End If
                Return _searcher
            End Get
        End Property

        Private Sub SearchControl_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
            Controls.Add(MySearchUserControl)
        End Sub

        Private Sub SearchControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Style.Add("text-align", "justified")
        End Sub
    End Class
