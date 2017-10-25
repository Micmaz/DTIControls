Public Class ucImage
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

    Private _uploadOnly As Boolean = False
    Public Property UploadOnly As Boolean
        Get
            Return _uploadOnly
        End Get
        Set(value As Boolean)
            _uploadOnly = value
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

    Public Event Delete(ByVal ImageID As String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        divImg.ID = "divImg_" & ImageID
        Dim checked As String = ""
        If Selected Then
            checked = " checked=""true"""
            divImg.Attributes.Remove("class")
            divImg.Attributes.Add("class", "img selected")
        End If
        If _MulitSelect Then
            litButtons.Text = "<input id=""b_" & ImageID & """ class=""btnImageSelect"" type=""checkbox"" " & _
                "name=""imageSelect"" value=""" & ImageID & """ onclick=""cbrbselect(this.id)"" " & _
                checked & ">" & _
                "<label id=""lbl_" & ImageID & """ for=""b_" & ImageID & """ title=""" & Name & """>" & Name & "</label>"
        ElseIf UploadOnly Then
            litButtons.Text = "<label id=""lbl_" & ImageID & """ for=""b_" & ImageID & """ title=""" & Name & """>" & Name & "</label>"
        Else
            litButtons.Text = "<input id=""b_" & ImageID & """ class=""btnImageSelect"" type=""radio"" " & _
                "name=""imageSelect"" value=""" & ImageID & """ onclick=""cbrbselect(this.id)"" " & _
                 checked & ">" & _
                "<label id=""lbl_" & ImageID & """ for=""b_" & ImageID & """ title=""" & Name & """>" & Name & "</label>"
        End If
        LazyImgLoad1.ImageUrl = "~/res/DTIImageManager/ViewImage.aspx?maxHeight=100&maxWidth=100&id=" & ImageID
        LazyImgLoad1.Container = "$("".innerdiv"")"
        LazyImgLoad1.ID = "LazyLoad_" & ImageID
        lkImage.HRef = "~/res/DTIImageManager/ManipulateImage.aspx?Id=" & ImageID & "&f=" & Name
        lkImage.Title = Name
        LazyImgLoad1.SkipInvisible = False
        lbDelete.OnClientClick = "return parent.jqconfirm(this,'Are you sure you want to delete " & Name & "?','Delete Image');"
    End Sub

    Private Sub lbDelete_Click(sender As Object, e As System.EventArgs) Handles lbDelete.Click
        RaiseEvent Delete(ImageID)
    End Sub
End Class