Imports DTIServerControls

''' <summary>
''' A control to handle file uploads.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIUploaderControl
    Inherits DTIServerBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIUploaderControl
        Inherits DTIServerBase
#End If
        Public Event handleFile(ByRef file As HttpPostedFile)
        Public Event handleWebFile(ByVal path As String)
        Friend WithEvents myUploaderControl As DTIUploaderUserControl

        Public Enum FileFilters
            All
            Images
            Videos
            ImagesAndVideo
        End Enum

#Region "Properties"

        'Public Property maxFileSize() As Integer
        '    Get
        '        Return myUploaderControl.maxFileSize
        '    End Get
        '    Set(ByVal value As Integer)
        '        myUploaderControl.maxFileSize = value
        '    End Set
        'End Property

        Private _fileFilter As FileFilters = FileFilters.All
        Public Property FileFilter() As FileFilters
            Get
                Return _fileFilter
            End Get
            Set(ByVal value As FileFilters)
                _fileFilter = value
            End Set
        End Property

        Private _cssFile As String = ""
        Public Property CssFile() As String
            Get
                Return _cssFile
            End Get
            Set(ByVal value As String)
                _cssFile = value
            End Set
        End Property

        Public Property selectFilesButtonCssClass() As String
            Get
                Return myUploaderControl.selectFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                myUploaderControl.selectFilesButtonCssClass = value
            End Set
        End Property

        Public Property uploadFilesButtonCssClass() As String
            Get
                Return myUploaderControl.uploadFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                myUploaderControl.uploadFilesButtonCssClass = value
            End Set
        End Property

        Public Property clearFilesButtonCssClass() As String
            Get
                Return myUploaderControl.clearFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                myUploaderControl.clearFilesButtonCssClass = value
            End Set
        End Property

        Public Property selectFilesDivCssClass() As String
            Get
                Return myUploaderControl.selectFilesDivCssClass
            End Get
            Set(ByVal value As String)
                myUploaderControl.selectFilesDivCssClass = value
            End Set
        End Property

        'Public Property uploadFilesDivCssClass() As String
        '    Get
        '        Return myUploaderControl.uploadFilesDivCssClass
        '    End Get
        '    Set(ByVal value As String)
        '        myUploaderControl.uploadFilesDivCssClass = value
        '    End Set
        'End Property

        Public Property fileSizeWarningCssClass() As String
            Get
                Return myUploaderControl.fileSizeWarningCssClass
            End Get
            Set(ByVal value As String)
                myUploaderControl.fileSizeWarningCssClass = value
            End Set
        End Property

        'Private _mySkin As Skins = Skins.VirtuConnect
        'Public Property Skin() As Skins
        '    Get
        '        Return _mySkin
        '    End Get
        '    Set(ByVal value As Skins)
        '        _mySkin = value
        '    End Set
        'End Property

        Public Property AllowUploadsOver2Gigs() As Boolean
            Get
                Return myUploaderControl.allowOver2GigUploads
            End Get
            Set(ByVal value As Boolean)
                myUploaderControl.allowOver2GigUploads = value
            End Set
        End Property

        Private _redirectURL As String = ""
        Public Property RedirectURL() As String
            Get
                Return _redirectURL
            End Get
            Set(ByVal value As String)
                _redirectURL = value
            End Set
        End Property

#End Region

        'Public Class applicationlistener
        '    Shared listener As New applicationlistener

        '    Public Shared Sub intialize()
        '        SyncLock listener
        '            'If listener Is Nothing Then
        '            'listener = New applicationlistener
        '            listener.app = HttpContext.Current.ApplicationInstance
        '            'End If
        '        End SyncLock
        '    End Sub

        '    Public WithEvents app As HttpApplication

        '    Private Sub app_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs) Handles app.BeginRequest
        '        Try
        '            Dim session_cookie_name As String = "ASP.NET_SESSIONID"
        '            Dim session_value As String = HttpContext.Current.Request.QueryString("sid")
        '            If session_value IsNot Nothing Then
        '                UpdateCookie(session_cookie_name, session_value)
        '            End If
        '        Catch ex As Exception
        '        End Try
        '    End Sub

        '    Private Sub UpdateCookie(ByVal cookie_name As String, ByVal cookie_value As String)
        '        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies.[Get](cookie_name)
        '        If cookie Is Nothing Then
        '            Dim cookie1 As New HttpCookie(cookie_name, cookie_value)
        '            HttpContext.Current.Response.Cookies.Add(cookie1)
        '        Else
        '            cookie.Value = cookie_value
        '            HttpContext.Current.Request.Cookies.[Set](cookie)
        '        End If
        '    End Sub

        'End Class

        Public Shared Sub correctCookie()
            Try
                Dim session_cookie_name As String = "ASP.NET_SESSIONID"
                Dim session_value As String = HttpContext.Current.Request.QueryString("sid")
                If session_value IsNot Nothing Then
                    UpdateCookie(session_cookie_name, session_value)
                End If
            Catch ex As Exception
            End Try
        End Sub

        Private Shared Sub UpdateCookie(ByVal cookie_name As String, ByVal cookie_value As String)
            Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies.[Get](cookie_name)
            If cookie Is Nothing Then
                Dim cookie1 As New HttpCookie(cookie_name, cookie_value)
                HttpContext.Current.Response.Cookies.Add(cookie1)
            Else
                cookie.Value = cookie_value
                HttpContext.Current.Request.Cookies.[Set](cookie)
            End If
        End Sub

        Private Sub DTIUploaderControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            correctCookie()
            'this is to fix a bug in Flash where it only sends IE cookies

            'applicationlistener.intialize()
            Me.Controls.Clear()
            myUploaderControl = DirectCast(Page.LoadControl("~/res/DTIUploader/DTIUploaderUserControl.ascx"), DTIUploaderUserControl)
            myUploaderControl.ID = "jsFileUploader_" & ClientID
            myUploaderControl.RedirectURL = RedirectURL
            Me.Controls.Add(myUploaderControl)
        End Sub


        Private Sub DTIUploaderControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not CssFile Is Nothing AndAlso CssFile <> "" Then
                registerClientScriptBlock("interfacestyle", "<link rel=""stylesheet"" type=""text/css"" href=""" & CssFile & """ />")
                'Else
                '    jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/interfacestyle.css", "text/css")
                'registerClientScriptBlock("interfacestyle", "<link rel=""stylesheet"" type=""text/css"" href=""/res/BaseClasses/Scripts.aspx?f=res/DTIUploader/interfacestyle.css"" />")
            End If

            If myUploaderControl.UploadMode = DTIUploaderUserControl.UploadModes.JS Then
                'jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/fonts-min.css", "text/css")
                'jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/datatable.css", "text/css")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/yahoo-dom-event.js")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/element-min.js")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/uploader.js")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/datasource-min.js")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/datatable-min.js")
                jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIUploader/YahooUploader.js")

                'registerClientScriptBlock("yahooFonts", "<link rel=""stylesheet"" type=""text/css"" href=""/res/DTIUploader/fonts-min.css"" />")
                'registerClientScriptBlock("yahooSkins", "<link rel=""stylesheet"" type=""text/css"" href=""/res/DTIUploader/datatable.css"" />")
                'registerClientScriptFile("yahooEvents", "/res/DTIUploader/yahoo-dom-event.js")
                'registerClientScriptFile("yahooElement", "/res/DTIUploader/element-min.js")
                'registerClientScriptFile("yahooUploader", "/res/DTIUploader/uploader.js")
                'registerClientScriptFile("yahooDatasource", "/res/DTIUploader/datasource-min.js")
                'registerClientScriptFile("yahooDatatable", "/res/DTIUploader/datatable-min.js")
                'registerClientScriptFile("localUploaderHandler", "/res/DTIUploader/YahooUploader.js")

                Dim uploadOverlayId As String = myUploaderControl.jsUploader.uploadOverlayId
                Dim dataTableContainerId As String = myUploaderControl.jsUploader.dataTableContainerId
                Dim selectFilesButtonId As String = myUploaderControl.jsUploader.selectFilesButtonId
                Dim uploadFilesId As String = myUploaderControl.jsUploader.uploadFilesButtonId
                Dim clearFilesId As String = myUploaderControl.jsUploader.clearFilesButtonId
                Dim errorMessageId As String = myUploaderControl.fileSizeErrorId
                Dim queryChanger As New BaseClasses.QueryStringChanger
                queryChanger.Add("fp", uploadOverlayId)
                queryChanger.Add("sid", Session.SessionID)
                Dim uploadPath = queryChanger.FullUrl
                Dim initializeScript As String = "initialize('" & uploadOverlayId & "', '" & uploadPath & _
                    "', '" & dataTableContainerId & "', '" & selectFilesButtonId & "', '" & uploadFilesId & "', '" & _
                    FileFilter.ToString & "', '" & clearFilesId & "', '" & RedirectURL & "', '" & errorMessageId & "');"
                registerClientStartupScriptBlock("initializeUploader_" & ClientID, initializeScript, True)
            End If
        End Sub

        Private Sub myUploaderControl_handleFile(ByRef file As System.Web.HttpPostedFile) Handles myUploaderControl.handleFile
            RaiseEvent handleFile(file)
        End Sub

        Private Sub myUploaderControl_handleWebFile(ByVal path As String) Handles myUploaderControl.handleWebFile
            RaiseEvent handleWebFile(path)
        End Sub
    End Class
