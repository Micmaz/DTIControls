Public Class BasePage
    Inherits BaseClasses.BaseSecurityPage

    Public Property SiteTemplate() As String
        Get
            If Session("MasterPageUsedForTemplatingWebsite") Is Nothing Then
                Session("MasterPageUsedForTemplatingWebsite") = "~/Puzzled.Master"
            End If
            Return Session("MasterPageUsedForTemplatingWebsite")
        End Get
        Set(ByVal value As String)
            Session("MasterPageUsedForTemplatingWebsite") = value
        End Set
    End Property

    Public Property TemplateChanged() As Boolean
        Get
            If Session("HastheTemplateBeenChanged") Is Nothing Then
                Session("HastheTemplateBeenChanged") = False
            End If
            Return Session("HastheTemplateBeenChanged")
        End Get
        Set(ByVal value As Boolean)
            Session("HastheTemplateBeenChanged") = True
        End Set
    End Property

    Private Sub BasePage_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Me.MasterPageFile = SiteTemplate
    End Sub

    
End Class
