Imports DTIServerControls
Imports System.Text.RegularExpressions

''' <summary>
''' control to navigate search of DTI-managed media objects.  Sorted by relevancy, submitted, or rating
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class MediaSearcher
    Inherits DTIServerControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class MediaSearcher
        Inherits DTIServerControl
#End If
        Public btnSearch As New HtmlInputButton
        Public tbSearch As New TextBox
        Friend tbSort As New TextBox
        Public btnNewest As New HtmlAnchor
        Public btnOldest As New HtmlAnchor
        Public btnRating As New HtmlAnchor
        Friend pnlButtons As New Panel
        Public commonWords As String() = {"the", "of", "to", "and", "a", "in", "is", "it"}
        Public commonWordsList As New ArrayList(commonWords)

        Protected TitleWeight As Double = 5
        Protected DescWeight As Double = 4
        Protected TagWeight As Double = 3
        Protected ContentWeight As Double = 2

        Private _words As String()
        Public ReadOnly Property SearchWords() As String()
            Get
                If _words Is Nothing Then
                    _words = Regex.Split(tbSearch.Text, "\W+")
                    Dim wordList As New List(Of String)
                    For Each word As String In _words
                        If word <> "" Then
                            wordList.Add(word)
                        End If
                    Next
                    _words = wordList.ToArray
                End If
                Return _words
            End Get
        End Property

        Private _word_soup As String()
        Public ReadOnly Property WordSoup() As String()
            Get
                If _word_soup Is Nothing Then
                    Dim _wordSoupList As New List(Of String)
                    For Each word As String In SearchWords
                        If Not commonWordsList.Contains(word) Then
                            _wordSoupList.Add("%" & word & "%")
                        End If
                    Next
                    _word_soup = _wordSoupList.ToArray
                End If
                Return _word_soup
            End Get
        End Property

        Private _use_search_rank As Boolean = True
        Public Property UseSearchRank() As Boolean
            Get
                Return _use_search_rank
            End Get
            Set(ByVal value As Boolean)
                _use_search_rank = value
                pnlButtons.Visible = Not value
            End Set
        End Property

        Public Property UseSortButtons() As Boolean
            Get
                Return pnlButtons.Visible
            End Get
            Set(ByVal value As Boolean)
                pnlButtons.Visible = value
                _use_search_rank = Not value
            End Set
        End Property

        Public Property ShowRatingSortButton() As Boolean
            Get
                Return btnRating.Visible
            End Get
            Set(ByVal value As Boolean)
                If value Then UseSortButtons = value
                btnRating.Visible = value
            End Set
        End Property

        Public Property ShowNewestOldestSortButtons() As Boolean
            Get
                Return btnNewest.Visible
            End Get
            Set(ByVal value As Boolean)
                If value Then UseSortButtons = value
                btnNewest.Visible = value
                btnOldest.Visible = value
            End Set
        End Property

        Public ReadOnly Property SortButtons() As ControlCollection
            Get
                Return pnlButtons.Controls
            End Get
        End Property

        Public Property SearchOnClick() As String
            Get
                Return btnSearch.Attributes("onclick")
            End Get
            Set(ByVal value As String)
                btnSearch.Attributes("onclick") = value
            End Set
        End Property

        Public Property SearchText() As String
            Get
                Return tbSearch.Text
            End Get
            Set(ByVal value As String)
                tbSearch.Text = value
            End Set
        End Property

        Private _add_query As String = ""
        Public Property AdditionalQuery() As String
            Get
                Return _add_query
            End Get
            Set(ByVal value As String)
                _add_query = value
            End Set
        End Property

        Public ReadOnly Property QueryFilter() As String
            Get
                For Each mediaType As mediaSearchTable In mediaSearchTableEntries
                    If Not Content_Types.Contains(mediaType.ContentType) Then
                        Content_Types.Add(mediaType.ContentType)
                    End If
                Next
                Dim returnStatement As String = " User_Id = '" & MainID & "' AND Published <> 0 and Removed <> 1 " & _
                        " and Component_Type = '" & Component_Type & "' and (" & _
                        comp_string("Content_Type", "=", Content_Types.ToArray) & ")"

                If UseSearchRank Then
                    returnStatement &= " and (" & CountExecString & " > 0)"
                ElseIf UseSortButtons AndAlso tbSearch.Text <> "" Then

                    returnStatement &= " and ((" & comp_string("Title", "like", WordSoup) & ") or (" & _
                        comp_string("Description", "like", WordSoup) & ") or Id in (select Content_Id from " & _
                        "DTI_Content_Tag_Pivot where Component_Type = '" & Component_Type & "' and Tag_Id in " & _
                        "(select Id from DTI_Content_Tags where " & comp_string("Tag_Name", "like", WordSoup) & "))"

                    For Each mediaSearch As mediaSearchTable In mediaSearchTableEntries
                        If Not mediaSearch.TableName Is Nothing Then
                            returnStatement &= " or (Content_Type = '" & mediaSearch.ContentType & "' and Content_Id" & _
                                "in (select Id from " & mediaSearch.TableName & " where " & _
                                comp_string(mediaSearch.VarCharColumnName, "like", WordSoup) & "))"
                        End If
                    Next
                    returnStatement &= ")"
                End If

                If AdditionalQuery <> "" Then
                    returnStatement &= " and (" & AdditionalQuery & ")"
                End If
                '" and User_Id in (select Id from from Users where MaindId = " & MainId & ")"

                Return returnStatement
            End Get
        End Property

        '***fixme database specific
        Private ReadOnly Property CountExecString() As String
            Get
                Dim returnValue As String = "1"
                If tbSearch.Text <> "" AndAlso UseSearchRank Then
                    Dim titleString As String = "0"
                    Dim descString As String = "0"
                    Dim tagString As String = "0"
                    Dim mediaString As String = "0"
                    For Each word As String In SearchWords
                        If Not commonWordsList.Contains(word) Then
                            titleString &= " + CountString(isnull(Title, ''), '" & word & "')"
                            descString &= " + CountString(isnull(Description, ''), '" & word & "')"
                            tagString &= " + HasTag(Id, '" & Component_Type & "', '" & word & "')"

                            For Each mediaSearch As mediaSearchTable In mediaSearchTableEntries
                                mediaString &= " + case when Content_Type = '" & mediaSearch.ContentType & _
                                "' then CountString((select CONVERT(varchar(8000), isnull(" & _
                                mediaSearch.VarCharColumnName & ",'')) from " & mediaSearch.TableName & _
                                " where Id = Content_Id), '" & word & "') else 0 end"
                            Next
                        End If
                    Next
                    titleString = "(" & titleString & ") * " & TitleWeight
                    descString = "(" & descString & ") * " & DescWeight
                    tagString = "(" & tagString & ") * " & TagWeight
                    mediaString = "(" & mediaString & ") * " & ContentWeight
                    returnValue = "isnull(Rating, 1) * (" & titleString & " + " & descString & " + " & tagString & _
                        " + " & mediaString & ")"
                End If
                Return returnValue
            End Get
        End Property

        Public Property Sort() As String
            Get
                If tbSort.Text = "" Then
                    tbSort.Text = "Date_added Desc"
                End If
                If UseSearchRank Then
                    Return CountExecString & " desc, " & tbSort.Text
                ElseIf UseSortButtons Then
                    Return tbSort.Text
                Else
                    Return tbSort.Text
                End If
            End Get
            Set(ByVal value As String)
                tbSort.Text = value
            End Set
        End Property

        Public ReadOnly Property SearchSelectStatement() As String
            Get
                Return "select " & SearchColumns & " from DTIMediaManager where " & QueryFilter & " order by " & Sort
            End Get
        End Property

        Public ReadOnly Property SearchColumns() As String
            Get
                Return "*"
            End Get
        End Property

        Public Class mediaSearchTable
            Public ContentType As String
            Public TableName As String
            Public VarCharColumnName As String

            Public Sub New(ByVal cont_type As String, ByVal table_name As String, ByVal var_char_col_name As String)
                ContentType = cont_type
                TableName = table_name
                VarCharColumnName = var_char_col_name
            End Sub
        End Class

        Public mediaSearchTableEntries As New List(Of mediaSearchTable)

        Private _cont_types As New List(Of String)
        Public ReadOnly Property Content_Types() As List(Of String)
            Get
                Return _cont_types
            End Get
        End Property

        Public ReadOnly Property comp_string(ByVal colName As String, ByVal op As String, ByVal values As String()) As String
            Get
                Dim retString As String = colName & " " & op & " '"
                Dim concatString As String = "' or " & colName & " " & op & " '"
                For Each comp As String In values
                    retString &= comp & concatString
                Next
                If retString.LastIndexOf(concatString) > -1 Then
                    Return retString.Substring(0, retString.LastIndexOf(" or"))
                Else
                    Return "1=1"
                End If
            End Get
        End Property

        Private Sub MediaSearcher_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            UseSortButtons = False
            tbSort.Text = "Date_added Desc"
        End Sub

        Private Sub MediaSearcher_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            tbSearch.ID = "tbMediaSearch"
            tbSearch.CssClass = "MediaSearchTextBox"
            btnSearch.Attributes("class") = "MediaSearchButton"
            tbSort.ID = "tbMediaSort"
            tbSort.CssClass = "MediaSortTextBox"
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            registerClientScriptBlock("queryInit", "$.query = { prefix: false };", True)
            registerClientScriptFile("JQQ", BaseClasses.Scripts.ScriptsURL(True) & "jQueryLibrary/jquery.query.js")

            If Page.Request.Params("SearchCompType") <> "" Then
                Component_Type = Page.Request.Params("SearchCompType")
            End If
            If Page.Request.Params("SearchContTypes") <> "" Then
                For Each contType As String In Page.Request.Params("SearchContTypes").Split("+")
                    Content_Types.Add(contType)
                Next
            End If
            If Page.Request.Params("SearchQuery") <> "" Then
                For Each contType As String In Page.Request.Params("SearchQuery").Split("+")
                    Content_Types.Add(contType)
                Next
            End If
        End Sub

        Private Sub MediaSearcher_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
            Controls.Add(tbSearch)
            Controls.Add(btnSearch)
            Controls.Add(New LiteralControl("<br />"))
            Controls.Add(tbSort)
            pnlButtons.Controls.Add(btnNewest)
            pnlButtons.Controls.Add(btnOldest)
            pnlButtons.Controls.Add(btnRating)
            Controls.Add(pnlButtons)
        End Sub

        Private Sub MediaSearcher_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            tbSort.Style("display") = "none"

            btnSearch.Attributes.Add("value", "Search")

            btnNewest.Attributes.Add("href", "#")
            btnNewest.InnerHtml = "Newest"
            btnNewest.Style("padding-right") = "30px"
            btnNewest.Attributes.Add("onclick", "$('#" & tbSort.ClientID & "').val('Date_Added Desc');$('#" & btnSearch.ClientID & "').click();return false;")

            btnOldest.Attributes.Add("href", "#")
            btnOldest.InnerHtml = "Oldest"
            btnOldest.Style("padding-right") = "30px"
            btnOldest.Attributes.Add("onclick", "$('#" & tbSort.ClientID & "').val('Date_Added Asc');$('#" & btnSearch.ClientID & "').click();return false;")

            btnRating.Attributes.Add("href", "#")
            btnRating.InnerHtml = "Rating"
            btnRating.Attributes.Add("onclick", "if($('#" & tbSort.ClientID & "').val().indexOf('Rating D') > -1){$('#" & tbSort.ClientID & "').val('Rating Asc');} else{$('#" & tbSort.ClientID & "').val('Rating Desc');}$('#" & btnSearch.ClientID & "').click();return false;")

            'Dim script As String = "$(function() { if($.query.load(window.location.href).get('" & Me.ClientID & _
            '    "') != '') { $('#" & Me.ClientID & "').val($.query.load(window.location.href).get('" & Me.ClientID & _
            '    "'));}});"
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "searcherInit_" & Me.ClientID, script, True)
        End Sub

        '***fixme database specific - Poss remove
        Public Shared Function getCreateSqlFunctionsScript() As Hashtable
            Dim script1 As String = "CREATE  FUNCTION [HasTag] " & vbCrLf & _
            "( @media_id INT, @comp_type VARCHAR(100), @tag_name VARCHAR(100) ) " & vbCrLf & _
            "RETURNS INT " & vbCrLf & _
            "BEGIN " & vbCrLf & _
            "    RETURN  (select count(*)  " & vbCrLf & _
            "	from DTI_Content_Tag_Pivot  " & vbCrLf & _
            "	where Content_Id = @media_id and Component_Type = @comp_type and Tag_Id in  " & vbCrLf & _
            "	(select Id from DTI_Content_Tags where Tag_Name like '%' + @tag_name + '%')) " & vbCrLf & _
            "END "
            Dim script2 As String = "CREATE FUNCTION [CountString] " & vbCrLf & _
            "( @pInput VARCHAR(8000), @pSearchString VARCHAR(100) ) " & vbCrLf & _
            "RETURNS INT " & vbCrLf & _
            "BEGIN " & vbCrLf & _
            "    RETURN (LEN(@pInput) -  " & vbCrLf & _
            "            LEN(REPLACE(@pInput, @pSearchString, ''))) / " & vbCrLf & _
            "            LEN(@pSearchString) " & vbCrLf & _
            "END "

            Dim ret As New Hashtable
            ret("HasTag") = script1
            ret("CountString") = script2
            Return ret

        End Function

        Public Sub createSqlFunctions()
            Dim hash As Hashtable = getCreateSqlFunctionsScript()
            For Each obj As String In hash.Keys
                Try
                    If Not sqlhelper.checkDBObjectExists(obj) Then
                        sqlhelper.ExecuteNonQuery(hash(obj))
                    End If
                Catch ex As Exception

                End Try
            Next
        End Sub

        Private Sub MediaSearcher_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
            Try
                createSqlFunctions()
            Catch ex As Exception

            End Try

        End Sub
    End Class
