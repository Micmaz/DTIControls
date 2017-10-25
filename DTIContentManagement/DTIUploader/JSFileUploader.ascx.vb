Imports DTIUploader.DTIUploaderControl

#If DEBUG Then
Partial Public Class JSFileUploader
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JSFileUploader
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Public Event handleFile(ByRef file As HttpPostedFile)

#Region "Properties"

        Public Property selectFilesButtonCssClass() As String
            Get
                Return selectLink.Attributes("class")
            End Get
            Set(ByVal value As String)
                selectLink.Attributes("class") = value
            End Set
        End Property

        Public Property uploadFilesButtonCssClass() As String
            Get
                Return uploadLink.Attributes("class")
            End Get
            Set(ByVal value As String)
                uploadLink.Attributes("class") = value
            End Set
        End Property

        Public Property clearFilesButtonCssClass() As String
            Get
                Return clearLink.Attributes("class")
            End Get
            Set(ByVal value As String)
                clearLink.Attributes("class") = value
            End Set
        End Property

        Public Property selectFilesDivCssClass() As String
            Get
                Return selectFilesLink.Attributes("class")
            End Get
            Set(ByVal value As String)
                selectFilesLink.Attributes("class") = value
            End Set
        End Property

        'Public Property uploadFilesDivCssClass() As String
        '    Get
        '        Return uploadFilesLink.Attributes("class")
        '    End Get
        '    Set(ByVal value As String)
        '        uploadFilesLink.Attributes("class") = value
        '    End Set
        'End Property

        Public ReadOnly Property dataTableContainerId() As String
            Get
                Return dataTableContainer.ClientID
            End Get
        End Property

        Public ReadOnly Property uploadOverlayId() As String
            Get
                Return uploaderOverlay.ClientID
            End Get
        End Property

        Public ReadOnly Property selectFilesButtonId() As String
            Get
                Return selectLink.ClientID
            End Get
        End Property

        Public ReadOnly Property uploadFilesButtonId() As String
            Get
                Return uploadLink.ClientID
            End Get
        End Property

        Public ReadOnly Property clearFilesButtonId() As String
            Get
                Return clearLink.ClientID
            End Get
        End Property

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            If Request.QueryString("fp") = uploadOverlayId Then
                For Each fileKey As String In Request.Files.Keys
                    If fileKey IsNot Nothing AndAlso fileKey <> "" Then
                        RaiseEvent handleFile(Request.Files(fileKey))
                    End If
                Next
            End If

        End Sub





    End Class