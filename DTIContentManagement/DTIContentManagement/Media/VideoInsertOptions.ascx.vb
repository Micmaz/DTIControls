Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports DTIMediaManager.SharedMediaVariables
Imports DTIMediaManager.dsMedia

#If DEBUG Then
Partial Public Class VideoInsertOptions
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class VideoInsertOptions
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private _media_row As DTIMediaManagerRow
        Public Property MediaRow() As DTIMediaManagerRow
            Get
                Return _media_row
            End Get
            Set(ByVal value As DTIMediaManagerRow)
                _media_row = value
            End Set
        End Property

        Public ReadOnly Property VideoID() As Integer
            Get
                Return MediaRow.Content_Id
            End Get
        End Property

        Private _videoWidth As Integer = 500
        Public Property VideoWidth() As Integer
            Get
                Return _videoWidth
            End Get
            Set(ByVal value As Integer)
                _videoWidth = value
            End Set
        End Property

        Private _videoHeight As Integer = 400
        Public Property VideoHeight() As Integer
            Get
                Return _videoHeight
            End Get
            Set(ByVal value As Integer)
                _videoHeight = value
            End Set
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAdminPanel/iframe-default.css", "text/css")
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Dim vidPreview As String = "<div id=""DTIVidDiv_" & VideoID & _
            '    """><img src=""~/res/DTIVideoManager/ViewVideoScreenShot.aspx?Id=" & VideoID & _
            '    "&showPlayOverlay=1"" border=""0"" width=""" & VideoWidth & """ height=""" & VideoHeight & """ /></div>"
            Dim vidPreview As String = "<img src=""~/res/DTIVideoManager/ViewVideoScreenShot.aspx?insertvideo&Id=" & VideoID & _
                "&showPlayOverlay=1"" border=""0"" width=""" & VideoWidth & """ height=""" & VideoHeight & """ />"

            hlInsertVideo.Attributes.Add("onclick", "insertMedia('" & vidPreview & "'); return false;")

            Dim myVidThumb As New VideoThumb
            myVidThumb.VideoId = VideoID
            myVidThumb.VideoWidth = VideoWidth
            myVidThumb.VideoHeight = VideoHeight
            hlInsertVideoThumb.Attributes.Add("onclick", "insertMedia('" & myVidThumb.outputValue.Replace("'", "\'") & "'); return false;")

            hsEdit.ExpandURL = "~/res/DTIMediaManager/EditMediaItem.aspx?mId=" & MediaRow.Id
            hsEdit.width = 650

            hlDeleteVideo.Attributes.Add("onclick", "deleteMedia(" & MediaRow.Id & "); return false;")
        End Sub


    End Class