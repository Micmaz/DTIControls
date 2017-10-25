Imports DTIServerControls

''' <summary>
''' simple control to display meta data of media objects
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class MediaInfo
    Inherits DTIServerControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class MediaInfo
        Inherits DTIServerControl
#End If
        Friend WithEvents _myInfoUC As MediaInfoUserControl
        Public ReadOnly Property MyInfoControl() As MediaInfoUserControl
            Get
                If _myInfoUC Is Nothing Then
                    _myInfoUC = DirectCast(Page.LoadControl("~/res/DTIMediaManager/MediaInfoUserControl.ascx"), _
                        MediaInfoUserControl)
                End If
                Return _myInfoUC
            End Get
        End Property

        Public Property ValuesPerStar() As Integer
            Get
                Return MyInfoControl.ValuesPerStar
            End Get
            Set(ByVal value As Integer)
                MyInfoControl.ValuesPerStar = value
            End Set
        End Property

        Public Property HighlightedWords() As String()
            Get
                Return MyInfoControl.HighlightedWords
            End Get
            Set(ByVal value As String())
                MyInfoControl.HighlightedWords = value
            End Set
        End Property

        Public Property MediaRow() As dsMedia.DTIMediaManagerRow
            Get
                Return MyInfoControl.MediaRow
            End Get
            Set(ByVal value As dsMedia.DTIMediaManagerRow)
                MyInfoControl.MediaRow = value
            End Set
        End Property

        Public Property TitleIsLink() As Boolean
            Get
                Return MyInfoControl.TitleIsLink
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.TitleIsLink = value
            End Set
        End Property

        Public Property ShowPubInfo() As Boolean
            Get
                Return MyInfoControl.ShowPubInfo
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowPubInfo = value
            End Set
        End Property

        Public Property ShowPubAuthor() As Boolean
            Get
                Return MyInfoControl.ShowPubAuthor
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowPubAuthor = value
            End Set
        End Property

        Public Property ShowPubDate() As Boolean
            Get
                Return MyInfoControl.ShowPubDate
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowPubDate = value
            End Set
        End Property

        Public Property ShowDescription() As Boolean
            Get
                Return MyInfoControl.ShowDescription
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowDescription = value
            End Set
        End Property

        Public Property ShowRating() As Boolean
            Get
                Return MyInfoControl.ShowRating
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowRating = value
            End Set
        End Property

        Public Property ShowSharing() As Boolean
            Get
                Return MyInfoControl.ShowSharing
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowSharing = value
            End Set
        End Property

        Public Property ShowSocialControls() As Boolean
            Get
                Return MyInfoControl.ShowSocialControls
            End Get
            Set(ByVal value As Boolean)
                MyInfoControl.ShowSocialControls = value
            End Set
        End Property

        Public ReadOnly Property ContentControls() As ControlCollection
            Get
                Return MyInfoControl.ContentControls
            End Get
        End Property

        Public ReadOnly Property SocialUtilitiesControls() As ControlCollection
            Get
                Return MyInfoControl.SocialUtilitiesControls
            End Get
        End Property

        Public ReadOnly Property CommentControls() As ControlCollection
            Get
                Return MyInfoControl.CommentControls
            End Get
        End Property

        Public Property DescriptionLength() As Integer
            Get
                Return MyInfoControl.DescriptionLength
            End Get
            Set(ByVal value As Integer)
                MyInfoControl.DescriptionLength = value
            End Set
        End Property

        Private Sub MediaInfo_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
            Controls.Add(MyInfoControl)
        End Sub
    End Class
