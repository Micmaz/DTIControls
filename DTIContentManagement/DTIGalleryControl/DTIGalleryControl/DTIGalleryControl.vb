Imports DTIServerControls
Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' base gallery control
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIGalleryControl
    Inherits DTIServerControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIGalleryControl
        Inherits DTIServerControl
#End If

#Region "Properties"

    ''' <summary>
    ''' URL of Image displayed while the gallery is loaded.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("URL of Image displayed while the gallery is loaded.")> _
        Public Property WaitingImageURL() As String
            Get
                Return MyGallery.WaitingImageURL
            End Get
            Set(ByVal value As String)
                MyGallery.WaitingImageURL = value
            End Set
        End Property

    ''' <summary>
    ''' Width or the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Width or the gallery.")> _
        Public Property GalleryWidth() As Unit
            Get
                Return MyGallery.GalleryWidth
            End Get
            Set(ByVal value As Unit)
                MyGallery.GalleryWidth = value
            End Set
        End Property

    ''' <summary>
    ''' Height of the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Height of the gallery.")> _
        Public Property GalleryHeight() As Unit
            Get
                Return MyGallery.GalleryHeight
            End Get
            Set(ByVal value As Unit)
                MyGallery.GalleryHeight = value
            End Set
        End Property

    Protected WithEvents _myGallery As Gallery

    ''' <summary>
    ''' The gallery user control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The gallery user control.")> _
    Protected ReadOnly Property MyGallery() As Gallery
        Get
            If _myGallery Is Nothing Then
                _myGallery = DirectCast(mypage.LoadControl("~/res/DTIGallery/Gallery.ascx"), Gallery)
                '_myGallery.ID = "galleryUC"
                Controls.Add(_myGallery)
            End If
            Return _myGallery
        End Get
    End Property

    ''' <summary>
    ''' The search panel control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The search panel control")> _
        Protected ReadOnly Property searchPanel() As Panel
            Get
                Return MyGallery.searchPanel
            End Get
        End Property

    Private _showSearch As Boolean = False

    ''' <summary>
    ''' Show the search control in a gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Show the search control in a gallery.")> _
        Public Property ShowSearching() As Boolean
            Get
                Return _showSearch
            End Get
            Set(ByVal value As Boolean)
                _showSearch = value
                If (_showSearch) Then
                    'searchPanel.Style("display") = ""
                    searchPanel.Visible = True
                Else
                    searchPanel.Visible = False
                    'searchPanel.Style("display") = "none"
                End If
            End Set
        End Property

    ''' <summary>
    ''' The ajax helper control used to load and page the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The ajax helper control used to load and page the gallery.")> _
        Public ReadOnly Property jsonControl() As DTIAjax.ajaxSeverConrol
            Get
                Return MyGallery.jsonControl
            End Get
        End Property

    Private _showPaging As Boolean = True

    ''' <summary>
    ''' Show the pages at the bottom of the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Show the pages at the bottom of the gallery.")> _
        Public Property ShowPaging() As Boolean
            Get
                Return _showPaging
            End Get
            Set(ByVal value As Boolean)
                _showPaging = value
            End Set
        End Property

    Private _showFirstAndLastButtons As Boolean = True

    ''' <summary>
    ''' Show the first and last page shortcuts at the bottom of the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Show the first and last page shortcuts at the bottom of the gallery.")> _
        Public Property ShowFirstAndLastButtons() As Boolean
            Get
                Return _showFirstAndLastButtons
            End Get
            Set(ByVal value As Boolean)
                _showFirstAndLastButtons = value
            End Set
        End Property

    Private _css_file As String

    ''' <summary>
    ''' The css file applied across the gallery
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css file applied across the gallery")> _
        Public Property CSS_file() As String
            Get
                Return _css_file
            End Get
            Set(ByVal value As String)
                _css_file = value
            End Set
        End Property

    ''' <summary>
    ''' Show the upload button on the gallery.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Show the upload button on the gallery.")> _
        Public Property ShowUpload() As Boolean
            Get
                'If AdminOn Then
                '    Return True
                'Else
                '    Return MyGallery.uploadPanel.Visible
                'End If

                Return MyGallery.uploadPanel.Visible
            End Get
            Set(ByVal value As Boolean)
                MyGallery.uploadPanel.Visible = value
            End Set
        End Property

    ''' <summary>
    ''' The number of pages in the gallery. Setting this only alters the label display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The number of pages in the gallery. Setting this only alters the label display.")> _
        Public Property TotalPages() As Integer
            Get
                Return MyGallery.TotalPages
            End Get
            Set(ByVal value As Integer)
                MyGallery.TotalPages = value
            End Set
        End Property

    ''' <summary>
    ''' The current page displayed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The current page displayed.")> _
        Public Property CurrentPage() As Integer
            Get
                Return MyGallery.CurrentPage
            End Get
            Set(ByVal value As Integer)
                MyGallery.CurrentPage = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the back button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the back button.")> _
        Public Property Back_Button_CSSClass() As String
            Get
                Return MyGallery.Back_Button.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Back_Button.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the forward button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the forward button.")> _
        Public Property Forward_Button_CSSClass() As String
            Get
                Return MyGallery.Forward_Button.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Forward_Button.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the first button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the first button.")> _
        Public Property First_Button_CSSClass() As String
            Get
                Return MyGallery.First_Button.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.First_Button.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the last button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the last button.")> _
        Public Property Last_Button_CSSClass() As String
            Get
                Return MyGallery.Last_Button.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Last_Button.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the page button.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the page button.")> _
        Public Property Page_Button_CSSClass() As String
            Get
                Return MyGallery.Page_Button.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Page_Button.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the page textbox.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the page textbox.")> _
        Public Property Page_Textbox_CSSClass() As String
            Get
                Return MyGallery.Page_Textbox.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Page_Textbox.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' The css class of the page label.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The css class of the page label.")> _
        Public Property Pages_Label_CSSClass() As String
            Get
                Return MyGallery.Pages_Label.CssClass
            End Get
            Set(ByVal value As String)
                MyGallery.Pages_Label.CssClass = value
            End Set
        End Property

    ''' <summary>
    ''' Back button image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Back button image url")> _
        Public Property Back_Button_Image_URL() As String
            Get
                Return MyGallery.Back_Button.ImageUrl
            End Get
            Set(ByVal value As String)
                MyGallery.Back_Button.ImageUrl = value
            End Set
        End Property

    ''' <summary>
    ''' Forward button image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Forward button image url")> _
        Public Property Forward_Button_Image_URL() As String
            Get
                Return MyGallery.Forward_Button.ImageUrl
            End Get
            Set(ByVal value As String)
                MyGallery.Forward_Button.ImageUrl = value
            End Set
        End Property

    ''' <summary>
    ''' First button image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("First button image url")> _
        Public Property First_Button_Image_URL() As String
            Get
                Return MyGallery.First_Button.ImageUrl
            End Get
            Set(ByVal value As String)
                MyGallery.First_Button.ImageUrl = value
            End Set
        End Property

    ''' <summary>
    ''' Last button image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Last button image url")> _
        Public Property Last_Button_Image_URL() As String
            Get
                Return MyGallery.Last_Button.ImageUrl
            End Get
            Set(ByVal value As String)
                MyGallery.Last_Button.ImageUrl = value
            End Set
        End Property

    ''' <summary>
    ''' Page button image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Page button image url")> _
        Public Property Page_Button_Image_URL() As String
            Get
                Return MyGallery.Page_Button.ImageUrl
            End Get
            Set(ByVal value As String)
                MyGallery.Page_Button.ImageUrl = value
            End Set
        End Property

    ''' <summary>
    ''' The place holder containing the gallery control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The place holder containing the gallery control.")> _
        Public ReadOnly Property PageHolder() As Panel
            Get
                Return MyGallery.PageHolder
            End Get
        End Property

#End Region

#Region "Event handlers"

    Private Sub DTIGalleryControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not mypage.IsPostBack Then
            CurrentPage = 1
        End If
    End Sub

    Private Sub DTIGalleryControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not CSS_file Is Nothing Then
            registerClientScriptBlock("galleryCSS", "<link rel=""stylesheet"" type=""text/css"" href=""" & CSS_file & """ />")
        End If

        jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIGallery/jquery-ui-1.8.1.gallery.min.js", , True) '"/jQueryLibrary/jquery-ui.custom.min.js"
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIGallery/jquery.batchImageLoad.js", , True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIGallery/jquery.dtiGallery.js", , True)
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "$.query = { prefix: false };", False)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/jQueryLibrary/jquery.query.js", , True)

        If ShowFirstAndLastButtons Then
            MyGallery.First_Button.Style("display") = "inline"
            MyGallery.Last_Button.Style("display") = "inline"
        Else
            MyGallery.First_Button.Style("display") = "none"
            MyGallery.Last_Button.Style("display") = "none"
        End If

        If ShowPaging Then
            MyGallery.Page_Button.Style("display") = "inline"
            MyGallery.slash_Label.Style("display") = "inline"
            MyGallery.Page_Textbox.Style("display") = "inline"
            MyGallery.Pages_Label.Style("display") = "inline"
        Else
            MyGallery.Page_Button.Style("display") = "none"
            MyGallery.slash_Label.Style("display") = "none"
            MyGallery.Page_Textbox.Style("display") = "none"
            MyGallery.Pages_Label.Style("display") = "none"
        End If

    End Sub

    Private Sub DTIGalleryControl_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        Dim ds As New dsGallery
        Dim ds1 As New DTIMediaManager.dsMedia
        sqlhelper.checkAndCreateAllTables(ds)
        sqlhelper.checkAndCreateAllTables(ds1)
        'sqlhelper.createGetSortedPage()
    End Sub

#End Region

#Region "Helper Functions"

    ''' <summary>
    ''' Sets the upload lik to a specific url and text value
    ''' </summary>
    ''' <param name="_displayText"></param>
    ''' <param name="_expandURL"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Sets the upload lik to a specific url and text value")> _
        Public Sub addUploadLink(ByVal _displayText As String, ByVal _expandURL As String)
            MyGallery.addUploadLink(_displayText, _expandURL)
        End Sub

    ''' <summary>
    ''' Clears pending uploads.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Clears pending uploads.")> _
        Public Sub clearUploads()
            MyGallery.clearUploads()
        End Sub

#End Region

    ''' <summary>
    ''' Ajax worker class
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Ajax worker class"),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public MustInherit Class GalleryWorkerBase
        Inherits DTIAjax.jsonWorker

        Public optionHash As New Hashtable
        Public total_pages As Integer = 1
        Public items_per_page As Integer = 4

        Private _parHelp As BaseClasses.ParallelDataHelper
        Protected ReadOnly Property parallelhelper() As BaseClasses.ParallelDataHelper
            Get
                If _parHelp Is Nothing Then
                    _parHelp = New BaseClasses.ParallelDataHelper
                    _parHelp.sqlHelper = Me.sqlHelper
                End If
                Return _parHelp
            End Get
        End Property


        Public Sub doExec(ByRef ds As DataSet, ByVal tableName As String, Optional ByVal primary_Key As String = "Id", Optional ByVal pageSize As Integer = 10, Optional ByRef pageNum As Integer = 1, Optional ByVal sort As String = "", Optional ByVal queryFilter As String = "", Optional ByVal cols As String = "")
            total_pages = sqlHelper.GetSortedPage(ds.tables(tableName),tableName, pageSize, pageNum, primary_Key, sort, queryFilter)
            'parallelhelper.addGetSortedPage(ds, tableName, tableName, primary_Key, pageSize, pageNum, _
            '    sort, queryFilter, cols)

            'parallelhelper.executeParallelDBCall()

            'total_pages = ds.Tables("PageCount").Rows(0)(0)
            Dim dt As New DataTable

            If pageNum > total_pages Then
                pageNum = total_pages

                ds.Clear()
                total_pages =  sqlHelper.GetSortedPage(ds.tables(tableName),tableName, pageSize, pageNum, primary_Key, sort, queryFilter)
                'parallelhelper.addGetSortedPage(ds, tableName, tableName, primary_Key, pageSize, pageNum, _
                'sort, queryFilter, cols)

                'parallelhelper.executeParallelDBCall()

                'total_pages = ds.Tables("PageCount").Rows(0)(0)
            End If

        End Sub

        Public Sub loadPage(ByVal options As Hashtable)
            optionHash = options
            getPage()
        End Sub

        Public MustOverride Sub getPage()

    End Class


    End Class
