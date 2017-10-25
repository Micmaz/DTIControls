Imports BaseClasses
Imports DTIUploader.DTIUploaderControl

#If DEBUG Then
Partial Public Class DTIUploaderUserControl
    Inherits BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class DTIUploaderUserControl
        Inherits BaseSecurityUserControl
#End If
        Public Event handleFile(ByRef file As HttpPostedFile)
        Public Event handleWebFile(ByVal path As String)
        Public allowOver2GigUploads As Boolean = False
        Private queryChanger As New QueryStringChanger
        Friend WithEvents jsUploader As JSFileUploader
        Friend WithEvents javaUploader As JavaFileUploader
        Friend WithEvents webUploader As WebFileUploader

        Public Enum UploadModes
            JS
            Java
            Web
        End Enum

#Region "Properties"

        Protected Property _myuploadmode() As UploadModes
            Get
                If Session("_myuploadmode_" & Me.ClientID) Is Nothing Then
                    Session("_myuploadmode_" & Me.ClientID) = UploadModes.JS
                End If
                Return Session("_myuploadmode_" & Me.ClientID)
            End Get
            Set(ByVal value As UploadModes)
                Session("_myuploadmode_" & Me.ClientID) = value
            End Set
        End Property

        Public ReadOnly Property UploadMode() As UploadModes
            Get
                Return _myuploadmode
            End Get
        End Property

        Public Property selectFilesButtonCssClass() As String
            Get
                Return jsUploader.selectFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                jsUploader.selectFilesButtonCssClass = value
            End Set
        End Property

        Public Property uploadFilesButtonCssClass() As String
            Get
                Return jsUploader.uploadFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                jsUploader.uploadFilesButtonCssClass = value
            End Set
        End Property

        Public Property clearFilesButtonCssClass() As String
            Get
                Return jsUploader.clearFilesButtonCssClass
            End Get
            Set(ByVal value As String)
                jsUploader.clearFilesButtonCssClass = value
            End Set
        End Property

        Public Property selectFilesDivCssClass() As String
            Get
                Return jsUploader.selectFilesDivCssClass
            End Get
            Set(ByVal value As String)
                jsUploader.selectFilesDivCssClass = value
            End Set
        End Property

        'Public Property uploadFilesDivCssClass() As String
        '    Get
        '        Return jsUploader.uploadFilesDivCssClass
        '    End Get
        '    Set(ByVal value As String)
        '        jsUploader.uploadFilesDivCssClass = value
        '    End Set
        'End Property

        Public Property fileSizeWarningCssClass() As String
            Get
                Return fileSizeWarning.Attributes("class")
            End Get
            Set(ByVal value As String)
                fileSizeWarning.Attributes("class") = value
            End Set
        End Property

        Public ReadOnly Property fileSizeErrorId() As String
            Get
                Return errorMessage.ClientID
            End Get
        End Property

        Private _redirectURL As String
        Public Property RedirectURL() As String
            Get
                Return _redirectURL
            End Get
            Set(ByVal value As String)
                _redirectURL = value
            End Set
        End Property

        Private _maxfilesize As Integer = -1
        Public Property maxFileSize() As Integer
            Get
                If _maxfilesize = -1 Then
                    _maxfilesize = getMaxFileSize()
                End If
                Return _maxfilesize
            End Get
            Set(ByVal value As Integer)
                _maxfilesize = value
                setmaxUploadSize(value)
            End Set
        End Property

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            If Not IsPostBack Then
                _myuploadmode = UploadModes.JS
            End If
            jsUploader = DirectCast(Page.LoadControl("~/res/DTIUploader/JSFileUploader.ascx"), JSFileUploader)
            jsUploader.ID = "jsFileUploader"

            javaUploader = DirectCast(Page.LoadControl("~/res/DTIUploader/JavaFileUploader.ascx"), JavaFileUploader)
            javaUploader.ID = "javaFileUploader"

            webUploader = DirectCast(Page.LoadControl("~/res/DTIUploader/WebFileUploader.ascx"), WebFileUploader)
            webUploader.ID = "webFileUploader"
            webUploader.RedirectURL = RedirectURL

            pnlUploaderHolder.Controls.Add(jsUploader)
            pnlUploaderHolder.Controls.Add(javaUploader)
            pnlUploaderHolder.Controls.Add(webUploader)

            If UploadMode = UploadModes.Java Then
                jsUploader.Visible = False
                javaUploader.Visible = True
                webUploader.Visible = False
            ElseIf UploadMode = UploadModes.JS Then
                jsUploader.Visible = True
                javaUploader.Visible = False
                webUploader.Visible = False
            ElseIf UploadMode = UploadModes.Web Then
                jsUploader.Visible = False
                javaUploader.Visible = False
                webUploader.Visible = True
            End If
        End Sub

        Private Sub setmaxUploadSize(ByVal size As Integer)
            Dim config As System.Configuration.Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
            Dim httpruntime As Web.Configuration.HttpRuntimeSection = config.GetSection("system.web/httpRuntime")
            httpruntime.MaxRequestLength = size

            '        Dim maxRequestLength As Int32 = 0
            '        Dim untypedHttpRuntime As Object = System.Configuration.ConfigurationSettings.GetConfig("system.web/httpRuntime")
            '        If Not untypedHttpRuntime Is Nothing Then
            '            Dim type As Type = untypedHttpRuntime.GetType()
            'Dim maxRequestLengthField As FieldInfo =  type.GetField("_maxRequestLength",(BindingFlags.Instance | BindingFlags.Public |BindingFlags.NonPublic)) 
            '            If Not maxRequestLengthField Is Nothing Then
            '                Dim result As Object = maxRequestLengthField.GetValue(untypedHttpRuntime)
            '                If Not result Is Nothing Then
            '                    maxRequestLength = Convert.ToInt32(result)
            '                End If
            '            End If
            '        End If

        End Sub

        Private Function getMaxFileSize() As Double
            Dim config As System.Configuration.Configuration = Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
            Dim section As Web.Configuration.HttpRuntimeSection = CType(config.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)

            Dim maxFileSize As Double = Math.Round(section.MaxRequestLength, 1)
            Return maxFileSize
        End Function


        Private Function getMaxFileSizeStr() As String

            Dim maxsize As Double = getMaxFileSize()
            Dim maxFileSize As Double = Math.Round(maxsize, 1)
            If maxFileSize > 1000 Then
                maxFileSize = Math.Round(maxsize / 1024.0, 1)

                If maxFileSize > 1000 Then
                    maxFileSize = Math.Round((maxsize / 1024.0) / 1024.0, 1)

                    Return String.Format(" {0:0.#} GB.", maxFileSize)

                    If maxFileSize > 2 AndAlso allowOver2GigUploads Then
                        btnToggleMode.Visible = True
                        If UploadMode = UploadModes.Java OrElse UploadMode = UploadModes.Web Then
                            btnToggleMode.Text = "Go back to the default uploader"
                        ElseIf UploadMode = UploadModes.JS Then
                            btnToggleMode.Text = "If you have a file larger than 2GB, click here"
                        End If

                    Else
                        btnToggleMode.Visible = False
                    End If

                Else
                    Return String.Format(" {0:0.#} MB.", maxFileSize)
                End If

            Else
                Return String.Format(" {0:0.#} KB.", maxFileSize)
            End If
        End Function

        Private Sub jsUploader_handleFile(ByRef file As System.Web.HttpPostedFile) Handles jsUploader.handleFile
            RaiseEvent handleFile(file)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            FileSizeLimit.Text = getMaxFileSizeStr()
        End Sub

        Private Sub btnToggleMode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToggleMode.Click
            If UploadMode = UploadModes.Java OrElse UploadMode = UploadModes.Web Then
                _myuploadmode = UploadModes.JS
                jsUploader.Visible = True
                javaUploader.Visible = False
                webUploader.Visible = False
            ElseIf UploadMode = UploadModes.JS Then
                _myuploadmode = UploadModes.Java
                jsUploader.Visible = False
                javaUploader.Visible = True
                webUploader.Visible = False
            End If
        End Sub

        Private Sub webUploader_handleFile(ByVal path As String) Handles webUploader.handleFile
            RaiseEvent handleWebFile(path)
        End Sub

        Private Sub btnWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWeb.Click
            If UploadMode = UploadModes.Java OrElse UploadMode = UploadModes.JS Then
                _myuploadmode = UploadModes.Web
                jsUploader.Visible = False
                javaUploader.Visible = False
                webUploader.Visible = True
                btnWeb.Text = "Go back to the default uploader"
            ElseIf UploadMode = UploadModes.Web Then
                _myuploadmode = UploadModes.JS
                jsUploader.Visible = True
                javaUploader.Visible = False
                webUploader.Visible = False
                btnWeb.Text = "Upload a file from the web"
            End If
        End Sub
    End Class