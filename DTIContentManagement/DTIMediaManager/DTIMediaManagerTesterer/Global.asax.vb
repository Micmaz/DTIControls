Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        Try
            Dim session_cookie_name As String = "ASP.NET_SESSIONID"
            Dim session_value As String = Request.QueryString("sid")
            If session_value IsNot Nothing Then
                UpdateCookie(session_cookie_name, session_value)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub UpdateCookie(ByVal cookie_name As String, ByVal cookie_value As String)
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies.[Get](cookie_name)
        If cookie Is Nothing Then
            Dim cookie1 As New HttpCookie(cookie_name, cookie_value)
            Response.Cookies.Add(cookie1)
        Else
            cookie.Value = cookie_value
            HttpContext.Current.Request.Cookies.[Set](cookie)
        End If
    End Sub


    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class