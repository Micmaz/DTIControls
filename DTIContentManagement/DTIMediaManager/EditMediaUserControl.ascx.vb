Imports DTIMediaManager.dsMedia
Imports DTITagManager.dsTagger

#If DEBUG Then
Partial Public Class EditMediaUserControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class EditMediaUserControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private _myMediaRow As DTIMediaManagerRow
        Friend Property MyMediaRow() As DTIMediaManagerRow
            Get
                Return _myMediaRow
            End Get
            Set(ByVal value As DTIMediaManagerRow)
                _myMediaRow = value
            End Set
        End Property

        Friend Property Content_Id() As Integer
            Get
                Return tagger.Content_Id
            End Get
            Set(ByVal value As Integer)
                tagger.Content_Id = value
            End Set
        End Property

        Friend ReadOnly Property myControlHolder() As ControlCollection
            Get
                Return phContentEditor.Controls
            End Get
        End Property

        Private ReadOnly Property previewList() As ArrayList
            Get
                Return SharedMediaVariables.PreviewList
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not MyMediaRow Is Nothing Then
                Content_Id = MyMediaRow.Id
                tagger.Component_Type = MyMediaRow.Component_Type
            End If
            setTextBoxes()
        End Sub

        Friend ReadOnly Property Title() As String
            Get
                Return tbTitle.Text
            End Get
        End Property

        Friend ReadOnly Property Description() As String
            Get
                Return tbDesc.Text
            End Get
        End Property

        Public Sub setTextBoxes()
            If Not IsPostBack Then
                If Not MyMediaRow Is Nothing Then
                    If Not MyMediaRow.IsDescriptionNull Then tbDesc.Text = MyMediaRow.Description
                    If Not MyMediaRow.IsTitleNull Then tbTitle.Text = MyMediaRow.Title
                    If Not MyMediaRow.IsDate_AddedNull Then
                        lblDateAddedValue.Text = MyMediaRow.Date_Added.ToShortDateString & ", " & MyMediaRow.Date_Added.ToShortTimeString
                    End If
                End If
            End If
        End Sub

        Public Sub saveTags(Optional ByVal contentId As Integer = -1)
            If Not MyMediaRow Is Nothing AndAlso contentId = -1 Then
                contentId = MyMediaRow.Id
            End If
            Content_Id = contentId
            tagger.saveTags()
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'If MyMediaRow Is Nothing OrElse MyMediaRow.RowState = DataRowState.Deleted OrElse _
            '    MyMediaRow.RowState = DataRowState.Detached Then

            tagger.ValidateOnFormSubmit = False

            'If Not previewList.Contains(MyMediaRow) OrElse MyMediaRow.Removed Then
            If Not MyMediaRow.IsRemovedNull AndAlso MyMediaRow.Removed Then
                Me.Visible = False
            End If

            If Not IsPostBack Then
                tagger.AddTagText &= "<br />"
            End If
            tagger.ShowSubmit = False
        End Sub
    End Class