Imports DTIMiniControls
Imports DTIServerControls
Imports DTITagManager.dsTagger

''' <summary>
''' Control for associating string tags with a particular data row
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class TagManager
    Inherits DTIServerBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class TagManager
        Inherits DTIServerBase
#End If
        Protected WithEvents myTagger As New Tagger
        Public DataFetched As Boolean = False

#Region "Properties"

        Private ReadOnly Property popularTags() As DTI_Content_TagsDataTable
            Get
                If Session("popularTags") Is Nothing Then
                    Session("popularTags") = New DTI_Content_TagsDataTable
                End If
                Return Session("popularTags")
            End Get
        End Property


        Protected ReadOnly Property dsTagManagement() As dsTagger
            Get
                If DataSource Is Nothing Then
                    DataSource = New dsTagger
                End If
                Return DataSource
            End Get
        End Property

        Protected ReadOnly Property pivotTable() As DTI_Content_Tag_PivotDataTable
            Get
                Return dsTagManagement.DTI_Content_Tag_Pivot
            End Get
        End Property

        Protected ReadOnly Property tagTable() As DTI_Content_TagsDataTable
            Get
                Return dsTagManagement.DTI_Content_Tags
            End Get
        End Property

        Private _content_id As Integer = -1
        Public Property Content_Id() As Integer
            Get
                Return _content_id
            End Get
            Set(ByVal value As Integer)
                _content_id = value
                raiseDataChanged()
            End Set
        End Property

        Public Property ShowSubmit() As Boolean
            Get
                Return myTagger.ShowSubmit
            End Get
            Set(ByVal value As Boolean)
                myTagger.ShowSubmit = value
            End Set
        End Property

        Public Property AddTagText() As String
            Get
                Return myTagger.AddTagText
            End Get
            Set(ByVal value As String)
                myTagger.AddTagText = value
            End Set
        End Property

        Public Property CurrentTagText() As String
            Get
                Return myTagger.CurrentTagText
            End Get
            Set(ByVal value As String)
                myTagger.CurrentTagText = value
            End Set
        End Property

        Public Property SeparatorCharacter() As String
            Get
                Return myTagger.SeparatorCharacter
            End Get
            Set(ByVal value As String)
                myTagger.SeparatorCharacter = value
            End Set
        End Property

        Public Property ShowPopularTags() As Boolean
            Get
                Return myTagger.ShowPopularTags
            End Get
            Set(ByVal value As Boolean)
                myTagger.ShowPopularTags = value
            End Set
        End Property

        Public WriteOnly Property popularTagsSet() As List(Of String)
            Set(ByVal value As List(Of String))
                myTagger.popularTagsSet = value
            End Set
        End Property

        Private _maxPopularCount As Integer = 6
        Public Property MaxPopularCount() As Integer
            Get
                Return _maxPopularCount
            End Get
            Set(ByVal value As Integer)
                _maxPopularCount = value
            End Set
        End Property

        Public Property currentTagsList() As List(Of String)
            Get
                Return myTagger.currentTagsList
            End Get
            Set(ByVal value As List(Of String))
                myTagger.currentTagsList = value
            End Set
        End Property

        Protected ReadOnly Property rowFilter() As String
            Get
                Return "Content_Id = " & Content_Id & " and Component_Type = '" & Component_Type & "'"
            End Get
        End Property

        Public Property ValidateOnFormSubmit() As Boolean
            Get
                Return myTagger.ValidateOnFormSubmit
            End Get
            Set(ByVal value As Boolean)
                myTagger.ValidateOnFormSubmit = value
            End Set
        End Property

#End Region


#Region "Events"
        Private Sub TagManager_DataChanged() Handles Me.DataChanged
            addSQLCall()
        End Sub

        Private Sub TagManager_DataReady() Handles Me.DataReady
            DataFetched = True
        End Sub

        Private Sub TagManager_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not mypage.IsPostBack Then
                If tagTable.Count = 0 Then
                    parallelhelper.addFillDataTable("select * from DTI_Content_Tags where Main_Id = " & MainID, tagTable)
                End If
                If popularTags.Count = 0 Then
                    sqlhelper.checkAndCreateTable(popularTags)
                    sqlhelper.checkAndCreateTable(pivotTable)
                    sqlhelper.FillDataTable("select * from DTI_Content_Tags where Main_Id = " & MainID & " and Id in" & _
                        "(select top " & MaxPopularCount & " Tag_Id from DTI_Content_Tag_Pivot group by Tag_Id order by " & _
                        "count(*) desc)", popularTags)
                End If
                addSQLCall()
            End If
        End Sub

        Private Sub TagManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Controls.Add(myTagger)
            If Not mypage.IsPostBack Then
                If Not DataFetched Then parallelhelper.executeParallelDBCall()
                myTagger.currentTagsList.Clear()
            End If
        End Sub

        Private Sub TagManager_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim myTagPivots() As DTI_Content_Tag_PivotRow = pivotTable.Select(rowFilter)
            For Each tagPivot As DTI_Content_Tag_PivotRow In myTagPivots
                currentTagsList.Add(tagTable.FindById(tagPivot.Tag_Id).Tag_Name)
            Next

            Dim myPopTags As New List(Of String)
            If popularTags.Count < 1 Then
                Me.ShowPopularTags = False
            Else
                For Each tag As DTI_Content_TagsRow In popularTags
                    myPopTags.Add(tag.Tag_Name)
                Next
                myTagger.popularTagsSet = myPopTags
            End If
        End Sub
#End Region

#Region "Helper Functions"
        Private Sub addSQLCall()
            parallelhelper.addFillDataTable("select * from DTI_Content_Tag_Pivot where " & rowFilter, pivotTable)
        End Sub

        Public Sub saveTags()
            For Each tag As String In currentTagsList
                Dim tagId As Integer = -1
                For Each tagRow As DTI_Content_TagsRow In tagTable
                    If tag.Trim.ToLower = tagRow.Tag_Name.Trim.ToLower Then
                        tagId = tagRow.Id
                        Exit For
                    End If
                Next
                If tagId > -1 Then
                    Dim pivotExists As Boolean = False
                    For Each pivot As DTI_Content_Tag_PivotRow In pivotTable
                        If tag.Trim.ToLower = tagTable.FindById(pivot.Tag_Id).Tag_Name.Trim.ToLower AndAlso _
                            pivot.Content_Id = Content_Id Then
                            pivotExists = True
                            Exit For
                        End If
                    Next
                    If Not pivotExists Then
                        createPivot(tagId)
                    End If
                Else
                    Dim newTag As DTI_Content_TagsRow = tagTable.NewDTI_Content_TagsRow
                    newTag.Main_Id = MainID
                    newTag.Tag_Name = tag
                    tagTable.AddDTI_Content_TagsRow(newTag)
                    sqlhelper.Update(tagTable)
                    createPivot(newTag.Id)
                End If
            Next
            Dim myPivots As DTI_Content_Tag_PivotRow() = pivotTable.Select("Content_Id = " & Content_Id)
            For Each pivot As DTI_Content_Tag_PivotRow In myPivots
                If Not pivot.RowState = DataRowState.Deleted AndAlso Not pivot.RowState = DataRowState.Detached Then
                    Dim tagExists As Boolean = False
                    For Each tag As String In currentTagsList
                        If tag.Trim.ToLower = tagTable.FindById(pivot.Tag_Id).Tag_Name.Trim.ToLower AndAlso _
                            pivot.Content_Id = Content_Id Then
                            tagExists = True
                            Exit For
                        End If
                    Next
                    If Not tagExists Then
                        pivot.Delete()
                    End If
                End If
            Next
            sqlhelper.Update(pivotTable)
        End Sub

        Private Sub createPivot(ByVal tag_id As Integer)
            Dim newPivot As DTI_Content_Tag_PivotRow = pivotTable.NewDTI_Content_Tag_PivotRow
            newPivot.Component_Type = Component_Type
            newPivot.Content_Id = Content_Id
            newPivot.Tag_Id = tag_id
            pivotTable.AddDTI_Content_Tag_PivotRow(newPivot)
        End Sub
#End Region


    End Class
