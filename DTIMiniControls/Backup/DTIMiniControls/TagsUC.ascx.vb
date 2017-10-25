#If DEBUG Then
Partial Public Class TagsUC
    Inherits System.Web.UI.UserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class TagsUC
        Inherits System.Web.UI.UserControl
#End If

        Public Property ShowSubmit() As Boolean
            Get
                Return btnSubmit.Visible
            End Get
            Set(ByVal value As Boolean)
                btnSubmit.Visible = value
            End Set
        End Property

        Public Property AddTagText() As String
            Get
                Return lblAddTags.Text
            End Get
            Set(ByVal value As String)
                lblAddTags.Text = value
            End Set
        End Property

        Public Property CurrentTagText() As String
            Get
                Return lblCurrTags.Text
            End Get
            Set(ByVal value As String)
                lblCurrTags.Text = value
            End Set
        End Property

        Private _separatorCharacter As String = ";"
        Public Property SeparatorCharacter() As String
            Get
                Return _separatorCharacter
            End Get
            Set(ByVal value As String)
                _separatorCharacter = value
            End Set
        End Property

        Public Property ShowPopularTags() As Boolean
            Get
                Return DTIPopularTagHolder.Visible
            End Get
            Set(ByVal value As Boolean)
                DTIPopularTagHolder.Visible = value
            End Set
        End Property

        Private _popularTags As List(Of String) = New List(Of String)
        'Protected ReadOnly Property popularTags() As String
        '    Get
        '        Return String.Join(""", """, _popularTags.ToArray())
        '    End Get
        'End Property

        Public WriteOnly Property popularTagsSet() As List(Of String)
            Set(ByVal value As List(Of String))
                _popularTags = value
            End Set
        End Property

        Private _currentTags As List(Of String) = New List(Of String)
        'Protected ReadOnly Property currentTags() As String
        '    Get
        '        Return String.Join(""", """, _currentTags.ToArray())
        '    End Get
        'End Property

        Public Property currentTagsList() As List(Of String)
            Get
                Return _currentTags
            End Get
            Set(ByVal value As List(Of String))
                _currentTags = value
            End Set
        End Property

        Dim _newTagsList As List(Of String)
        Private ReadOnly Property newTagsList() As List(Of String)
            Get
                If _newTagsList Is Nothing Then
                    _newTagsList = New List(Of String)
                    Try
                        For Each str As String In Request.Params(hfTags.UniqueID).Split(SeparatorCharacter)
                            If _newTagsList.IndexOf(str) < 0 AndAlso str.Trim <> "" Then
                                _newTagsList.Add(str)
                            End If
                        Next
                    Catch ex As Exception
                    End Try
                End If
                Return _newTagsList
            End Get
        End Property

        Friend ReadOnly Property DTICurrTagDiv() As Panel
            Get
                Return DTICurrTags
            End Get
        End Property

        Friend ReadOnly Property DTIPopTagDiv() As Panel
            Get
                Return DTIPopularTags
            End Get
        End Property

        Private _validateOnFormSubmit As Boolean = True
        Public Property ValidateOnFormSubmit() As Boolean
            Get
                Return _validateOnFormSubmit
            End Get
            Set(ByVal value As Boolean)
                _validateOnFormSubmit = value
            End Set
        End Property

        Public Event CurrentTagsChanged(ByVal tagsList As List(Of String))

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                If hasCurrentTagsChanged() Then
                    currentTagsList = newTagsList
                    RaiseEvent CurrentTagsChanged(Me.currentTagsList)
                End If
            End If
        End Sub

        Private Function hasCurrentTagsChanged() As Boolean
            If currentTagsList.Count <> newTagsList.Count Then Return True
            newTagsList.Sort()
            currentTagsList.Sort()
            Try
                For i As Integer = 0 To newTagsList.Count - 1
                    If currentTagsList(i) <> newTagsList(i) Then Return True
                Next
            Catch ex As Exception
                Return True
            End Try
            Return False
        End Function

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If ValidateOnFormSubmit Then
                Me.Page.Form.Attributes.Add("onsubmit", "prepareCurrentTags();")
            End If
            If Not ShowSubmit Then
                btnSubmit.Visible = False
            End If
        End Sub
    End Class