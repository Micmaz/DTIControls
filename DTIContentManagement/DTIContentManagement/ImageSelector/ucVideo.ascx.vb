Public Class ucVideo
    Inherits BaseControl

    Private _ImageID As String = "-1"
    Public Property ImageID As String
        Get
            Return _ImageID
        End Get
        Set(value As String)
            _ImageID = value
        End Set
    End Property

    Private _Name As String = "Select"
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property

    Private _MulitSelect As Boolean = False
    Public WriteOnly Property MultiSelect As Boolean
        Set(value As Boolean)
            _MulitSelect = value
        End Set
    End Property

    Private _selected As Boolean = False
    Public Property Selected As Boolean
        Get
            Return _selected
        End Get
        Set(value As Boolean)
            _selected = value
        End Set
    End Property

    Private Enum VideoTypes
        youtube
        vimeo
    End Enum

    Private _VideoType As VideoTypes = VideoTypes.youtube

    Public Event Delete(ByVal ImageID As String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        divImg.ID = "divImg_" & ImageID
        Dim divClass As String = "img"
        Dim checked As String = ""

        If Selected Then
            checked = " checked=""true"""
            divClass &= " selected"
        End If

        If ImageID.StartsWith("Y") Then
            _VideoType = VideoTypes.youtube
        Else
            _VideoType = VideoTypes.vimeo
        End If
        divClass &= " video"
        divImg.Attributes.Remove("class")
        divImg.Attributes.Add("class", divClass)

        If _MulitSelect Then
            litButtons.Text = "<input id=""b_" & ImageID & """ class=""btnImageSelect"" type=""checkbox"" " & _
                "name=""imageSelect"" value=""" & ImageID & """ onclick=""cbrbselect(this.id)"" " & _
                checked & ">" & _
                "<label id=""lbl_" & ImageID & """ for=""b_" & ImageID & """ title=""" & Name & """>" & Name & "</label>"
        Else
            litButtons.Text = "<input id=""b_" & ImageID & """ class=""btnImageSelect"" type=""radio"" " & _
                "name=""imageSelect"" value=""" & ImageID & """ onclick=""cbrbselect(this.id)"" " & _
                 checked & ">" & _
                "<label id=""lbl_" & ImageID & """ for=""b_" & ImageID & """ title=""" & Name & """>" & Name & "</label>"
        End If
        LazyImgLoad1.ImageUrl = ""
        LazyImgLoad1.ID = "LazyLoad_" & ImageID
        LazyImgLoad1.Container = "$("".innerdiv"")"
        If _VideoType = VideoTypes.youtube Then
            lkImage.HRef = "http://www.youtube.com/embed/" & ImageID.Substring(1) & "?autoplay=1"
        Else
            lkImage.HRef = "http://player.vimeo.com/video/" & ImageID.Substring(1) & "?autoplay=1"
        End If

        lkImage.Title = Name
        LazyImgLoad1.SkipInvisible = False
        lbDelete.OnClientClick = "return deleteVid(this,'" & ImageID & "');"
    End Sub

    Private Sub lbDelete_Click(sender As Object, e As System.EventArgs) Handles lbDelete.Click
        RaiseEvent Delete(ImageID)
    End Sub
End Class