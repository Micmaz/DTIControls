Imports DTIServerControls

#If DEBUG Then
Partial Public Class MediaInfoUserControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class MediaInfoUserControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Protected WithEvents hlTitle As Global.System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblTitle As Global.System.Web.UI.WebControls.Label
        Protected WithEvents lblPubInfo As Global.System.Web.UI.WebControls.Label
        Protected WithEvents phSocialUtilities As Global.System.Web.UI.WebControls.PlaceHolder
        Protected WithEvents rater As Global.DTIMediaManager.MediaRater
        Protected WithEvents bookmarker As Global.DTIMiniControls.AddThisControl
        Protected WithEvents pnlContent As Global.System.Web.UI.WebControls.Panel
        Protected WithEvents litDesc As Global.System.Web.UI.WebControls.Literal
        Protected WithEvents phComments As Global.System.Web.UI.WebControls.PlaceHolder


        Public HighlightedWords As String()

        Private _media_row As dsMedia.DTIMediaManagerRow
        Public Property MediaRow() As dsMedia.DTIMediaManagerRow
            Get
                Return _media_row
            End Get
            Set(ByVal value As dsMedia.DTIMediaManagerRow)
                _media_row = value
            End Set
        End Property

        Public Property TitleIsLink() As Boolean
            Get
                Return hlTitle.Visible
            End Get
            Set(ByVal value As Boolean)
                hlTitle.Visible = value
                lblTitle.Visible = Not value
            End Set
        End Property

        Public Property LinkTarget() As String
            Get
                Return hlTitle.Target
            End Get
            Set(ByVal value As String)
                hlTitle.Target = value
            End Set
        End Property

        Public Property ShowPubInfo() As Boolean
            Get
                Return lblPubInfo.Visible
            End Get
            Set(ByVal value As Boolean)
                lblPubInfo.Visible = value
            End Set
        End Property

        Private _show_pub_author As Boolean = False
        Public Property ShowPubAuthor() As Boolean
            Get
                Return _show_pub_author
            End Get
            Set(ByVal value As Boolean)
                _show_pub_author = value
            End Set
        End Property

        Private _show_pub_date As Boolean = False
        Public Property ShowPubDate() As Boolean
            Get
                Return _show_pub_date
            End Get
            Set(ByVal value As Boolean)
                _show_pub_date = value
            End Set
        End Property

        Public Property ShowDescription() As Boolean
            Get
                Return litDesc.Visible
            End Get
            Set(ByVal value As Boolean)
                litDesc.Visible = value
            End Set
        End Property

        Public ReadOnly Property starRater() As MediaRater
            Get
                Return rater
            End Get
        End Property

        Public Property ShowRating() As Boolean
            Get
                Return rater.Visible
            End Get
            Set(ByVal value As Boolean)
                rater.Visible = value
            End Set
        End Property

        Public Property ValuesPerStar() As Integer
            Get
                Return rater.ValuesPerStar
            End Get
            Set(ByVal value As Integer)
                rater.ValuesPerStar = value
            End Set
        End Property

        Public Property ShowSharing() As Boolean
            Get
                Return bookmarker.Visible
            End Get
            Set(ByVal value As Boolean)
                bookmarker.Visible = value
            End Set
        End Property

        Public Property ShowSocialControls() As Boolean
            Get
                Return phSocialUtilities.Visible
            End Get
            Set(ByVal value As Boolean)
                phSocialUtilities.Visible = value
            End Set
        End Property

        Public ReadOnly Property ContentControls() As ControlCollection
            Get
                Return pnlContent.Controls
            End Get
        End Property

        Public ReadOnly Property SocialUtilitiesControls() As ControlCollection
            Get
                Return phSocialUtilities.Controls
            End Get
        End Property

        Public ReadOnly Property CommentControls() As ControlCollection
            Get
                Return phComments.Controls
            End Get
        End Property

        Private _descLength As Integer = 140
        Public Property DescriptionLength() As Integer
            Get
                Return _descLength
            End Get
            Set(ByVal value As Integer)
                _descLength = value
            End Set
        End Property

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not MediaRow Is Nothing Then
                If Not MediaRow.IsTitleNull AndAlso MediaRow.Title.Trim <> "" Then
                    lblTitle.Text = MediaRow.Title & " &ndash; "
                    hlTitle.Text = MediaRow.Title
                Else
                    lblTitle.Visible = False
                    If Not MediaRow.IsPermanent_URLNull Then
                        hlTitle.Text = MediaRow.Permanent_URL
                    End If
                End If

                If Not MediaRow.IsPermanent_URLNull Then
                    hlTitle.NavigateUrl = MediaRow.Permanent_URL
                Else
                    hlTitle.Visible = False
                End If

                If Not MediaRow.IsDescriptionNull Then
                    litDesc.Text = MediaRow.Description
                End If

                If ShowPubAuthor AndAlso Not MediaRow.IsUser_IdNull Then
                    'either user view or fetchsinglevalue to get name of user
                    'lblPubInfo.Text &= name  
                End If

                If ShowPubDate AndAlso Not MediaRow.IsDate_AddedNull Then
                    If lblPubInfo.Text <> "" Then
                        lblPubInfo.Text &= ", "
                    End If
                    lblPubInfo.Text &= "Added " & formatDate(MediaRow.Date_Added)
                End If

                rater.MediaRow = Me.MediaRow
            End If

            If Not HighlightedWords Is Nothing Then
                For Each word As String In HighlightedWords
                    Dim wordRegex As New Regex(word, RegexOptions.IgnoreCase)
                    For Each wordMatch As Match In wordRegex.Matches(hlTitle.Text)
                        hlTitle.Text = hlTitle.Text.Replace(wordMatch.Value, "<b>" & wordMatch.Value & "</b>")
                    Next
                    lblTitle.Text = hlTitle.Text
                    If Not litDesc Is Nothing Then
                        For Each wordMatch As Match In wordRegex.Matches(litDesc.Text)
                            litDesc.Text = litDesc.Text.Replace(wordMatch.Value, "<b>" & wordMatch.Value & "</b>")
                        Next
                    End If
                Next
            End If

            DTISharedVariables.truncateIt("#" & pnlContent.ClientID, Me.Page, DescriptionLength)
        End Sub

        Public Function formatDate(ByVal time As DateTime) As String
            Dim datediff As TimeSpan = Now.Subtract(time)

            Dim days As Integer = datediff.TotalDays

            If days < 1 Then
                Dim seconds As Integer = datediff.TotalSeconds
                Select Case seconds
                    Case 0 To 60
                        Return "just now"
                    Case 61 To 120
                        Return "1 minute ago"
                    Case 121 To 3600
                        Return Math.Floor(seconds / 60) & " minutes ago"
                    Case 3601 To 7200
                        Return "1 hour ago"
                    Case 7201 To 86400
                        Return Math.Floor(seconds / 3600) & " hours ago"
                End Select
            ElseIf days < 31 Then
                Select Case days
                    Case 1
                        Return "yesterday"
                    Case 2 To 7
                        Return days & " days ago"
                    Case Is > 7
                        Return Math.Ceiling(days / 7) & " weeks ago"
                End Select
            Else : Return time.ToString("MM/dd/yyyy")
            End If
        End Function

    End Class