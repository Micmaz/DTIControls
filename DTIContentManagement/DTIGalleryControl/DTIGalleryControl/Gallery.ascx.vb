Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables

#If DEBUG Then
Partial Public Class Gallery
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class Gallery
        Inherits BaseClasses.BaseSecurityUserControl
#End If

#Region "Design File"

        '''<summary>
        '''AjaxSeverConrol1 control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("AjaxSeverConrol1 control.")> _
        Protected WithEvents AjaxSeverConrol1 As Global.DTIAjax.ajaxSeverConrol

        '''<summary>
        '''pnlGalleryHolder control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("pnlGalleryHolder control.")> _
        Protected WithEvents pnlGalleryHolder As Global.System.Web.UI.HtmlControls.HtmlGenericControl

        '''<summary>
        '''pnlSearch control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("pnlSearch control.")> _
        Protected WithEvents pnlSearch As Global.System.Web.UI.WebControls.Panel

        '''<summary>
        '''freezePanel control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("freezePanel control.")> _
        Protected WithEvents freezePanel As Global.DTIMiniControls.FreezeIt

        '''<summary>
        '''imgWait control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("imgWait control.")> _
        Protected WithEvents imgWait As Global.System.Web.UI.WebControls.Image

        '''<summary>
        '''pnlThumbHolder control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("pnlThumbHolder control.")> _
        Protected WithEvents pnlThumbHolder As Global.System.Web.UI.WebControls.Panel

        '''<summary>
        '''pnlUploadLinks control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("pnlUploadLinks control.")> _
        Protected WithEvents pnlUploadLinks As Global.System.Web.UI.WebControls.Panel

        '''<summary>
        '''Gallery_Button_Div control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("Gallery_Button_Div control.")> _
        Protected WithEvents Gallery_Button_Div As Global.System.Web.UI.HtmlControls.HtmlGenericControl

        '''<summary>
        '''btnGalleryFirst control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("btnGalleryFirst control.")> _
        Protected WithEvents btnGalleryFirst As Global.System.Web.UI.WebControls.ImageButton

        '''<summary>
        '''btnGalleryBack control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("btnGalleryBack control.")> _
        Protected WithEvents btnGalleryBack As Global.System.Web.UI.WebControls.ImageButton

        '''<summary>
        '''tbGalleryPage control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("tbGalleryPage control.")> _
        Protected WithEvents tbGalleryPage As Global.System.Web.UI.WebControls.TextBox

        '''<summary>
        '''lblSlash control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("lblSlash control.")> _
        Protected WithEvents lblSlash As Global.System.Web.UI.WebControls.Label

        '''<summary>
        '''lblGalleryTotalPages control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("lblGalleryTotalPages control.")> _
        Protected WithEvents lblGalleryTotalPages As Global.System.Web.UI.WebControls.Label

        '''<summary>
        '''btnGalleryPage control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("btnGalleryPage control.")> _
        Protected WithEvents btnGalleryPage As Global.System.Web.UI.WebControls.ImageButton

        '''<summary>
        '''btnGalleryFwd control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("btnGalleryFwd control.")> _
        Protected WithEvents btnGalleryFwd As Global.System.Web.UI.WebControls.ImageButton

        '''<summary>
        '''btnGalleryLast control.
        '''</summary>
        '''<remarks>
        '''Auto-generated field.
        '''To modify move field declaration from designer file to code-behind file.
        '''</remarks>
        <System.ComponentModel.Description("btnGalleryLast control.")> _
        Protected WithEvents btnGalleryLast As Global.System.Web.UI.WebControls.ImageButton
#End Region



#Region "properties"

        Public Property WaitingImageURL() As String
            Get
                Return imgWait.ImageUrl
            End Get
            Set(ByVal value As String)
                imgWait.ImageUrl = value
            End Set
        End Property

        Public ReadOnly Property ThumbFreezer() As DTIMiniControls.FreezeIt
            Get
                Return freezePanel
            End Get
        End Property

        Public Property GalleryWidth() As Unit
            Get
                Return pnlThumbHolder.Width
            End Get
            Set(ByVal value As Unit)
                pnlThumbHolder.Width = value
            End Set
        End Property

        Public Property GalleryHeight() As Unit
            Get
                Return pnlThumbHolder.Height
            End Get
            Set(ByVal value As Unit)
                pnlThumbHolder.Height = value
            End Set
        End Property

        Friend ReadOnly Property searchPanel() As Panel
            Get
                Return pnlSearch
            End Get
        End Property

        Friend ReadOnly Property uploadPanel() As Panel
            Get
                Return pnlUploadLinks
            End Get
        End Property

        Friend ReadOnly Property jsonControl() As DTIAjax.ajaxSeverConrol
            Get
                Return AjaxSeverConrol1
            End Get
        End Property

        Public Property TotalPages() As Integer
            Get
                If lblGalleryTotalPages.Text = "" Then
                    lblGalleryTotalPages.Text = 1
                End If
                Return lblGalleryTotalPages.Text
            End Get
            Set(ByVal value As Integer)
                lblGalleryTotalPages.Text = value
            End Set
        End Property

        Public Property CurrentPage() As Integer
            Get
                Return tbGalleryPage.Text
            End Get
            Set(ByVal value As Integer)
                tbGalleryPage.Text = value
            End Set
        End Property

        Public ReadOnly Property PageHolder() As Panel
            Get
                Return pnlThumbHolder
            End Get
        End Property

        Public ReadOnly Property Back_Button() As ImageButton
            Get
                Return btnGalleryBack
            End Get
        End Property

        Public ReadOnly Property Forward_Button() As ImageButton
            Get
                Return btnGalleryFwd
            End Get
        End Property

        Public ReadOnly Property First_Button() As ImageButton
            Get
                Return btnGalleryFirst
            End Get
        End Property

        Public ReadOnly Property Last_Button() As ImageButton
            Get
                Return btnGalleryLast
            End Get
        End Property

        Public ReadOnly Property Page_Button() As ImageButton
            Get
                Return btnGalleryPage
            End Get
        End Property

        Public ReadOnly Property Page_Textbox() As TextBox
            Get
                Return tbGalleryPage
            End Get
        End Property

        Public ReadOnly Property Pages_Label() As Label
            Get
                Return lblGalleryTotalPages
            End Get
        End Property

        Public ReadOnly Property slash_Label() As Label
            Get
                Return lblSlash
            End Get
        End Property

#End Region

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            freezePanel.FreezeId = pnlThumbHolder.ClientID
            freezePanel.BackgroundOpacity = 0.4
            If pnlSearch.Visible AndAlso pnlSearch.Controls.Count = 0 Then
                pnlSearch.Visible = False
            End If
            If pnlUploadLinks.Visible AndAlso pnlUploadLinks.Controls.Count = 0 Then
                pnlUploadLinks.Visible = False
            End If
        End Sub

        Public Sub addUploadLink(ByVal _displayText As String, ByVal _expandURL As String)
            Dim newHSUpload As New Highslider
            With newHSUpload
                .ID = "hsUpload_" & pnlUploadLinks.Controls.Count + 1
                .DisplayText = _displayText
                .HighslideVariablesString = "width: '650', height: '450', preserveContent: false"
                .HighslideDisplayMode = HighslideDisplayModes.Iframe
                .ExpandURL = _expandURL
            End With
            pnlUploadLinks.Controls.Add(newHSUpload)
        End Sub

        Public Sub clearUploads()
            pnlUploadLinks.Controls.Clear()
        End Sub



    End Class