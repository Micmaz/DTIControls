Imports DTIMediaManager.dsMedia

''' <summary>
''' control to edit meta data of media objects
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class EditMediaControl
    Inherits DTIServerControls.DTIServerBase
#Else
    <ToolboxData("<{0}:EditMediaControl runat=""server"" />")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class EditMediaControl
        Inherits DTIServerControls.DTIServerBase
#End If
        Protected _myEditor As EditMediaUserControl
        Protected ReadOnly Property myEditor() As EditMediaUserControl
            Get
                If _myEditor Is Nothing Then
                    _myEditor = DirectCast(mypage.LoadControl("~/res/DTIMediaManager/EditMediaUserControl.ascx"), EditMediaUserControl)
                End If
                Return _myEditor
            End Get
        End Property

        Public ReadOnly Property myControlHolder() As ControlCollection
            Get
                Return myEditor.myControlHolder
            End Get
        End Property

        Public Property MyMediaRow() As DTIMediaManagerRow
            Get
                Return myEditor.MyMediaRow
            End Get
            Set(ByVal value As DTIMediaManagerRow)
                myEditor.MyMediaRow = value
            End Set
        End Property

        Public ReadOnly Property Title() As String
            Get
                Return myEditor.Title
            End Get
        End Property

        Public ReadOnly Property Description() As String
            Get
                Return myEditor.Description
            End Get
        End Property

        Public Sub saveTags(Optional ByVal contentId As Integer = -1)
            myEditor.saveTags(contentId)
        End Sub

        Private Sub EditMediaControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Controls.Add(myEditor)
        End Sub
    End Class
