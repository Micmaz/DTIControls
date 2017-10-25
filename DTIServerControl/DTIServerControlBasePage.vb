Imports BaseClasses
Imports System.Web.Services
Imports System.Web
Imports System.Web.SessionState
Imports System.Reflection
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.UI.WebControls

Imports System.Text
Imports System.IO
Imports System.Web.UI

Public Class DTIServerControlBasePage
    Inherits BaseClasses.BaseSecurityPage
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		If DTISharedVariables.LoggedIn = False Then Response.End()
	End Sub
End Class