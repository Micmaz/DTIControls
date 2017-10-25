Imports System.Web.UI.WebControls

Public Class JsonTest
    Inherits Panel

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim res As HttpResponse = Me.Page.Response
        If Me.Page.Request.QueryString("JsonTest") IsNot Nothing _
            AndAlso Me.Page.Request.QueryString("JsonTest") = Me.ClientID Then
            Dim str As String
            Using strR As New IO.StreamReader(Me.Page.Request.InputStream)
                str = strR.ReadToEnd
            End Using
            res.Clear()
            res.ContentType = "application/json"
            res.Cache.SetMaxAge(New TimeSpan(0))
            If str.IndexOf("Yesterday") > -1 Then
                res.Write("""" & DateTime.Now.AddDays(-1).ToString & """")
            ElseIf str.IndexOf("Today") > -1 Then
                res.Write("""" & DateTime.Now.ToString & """")
            Else
                res.Write("""" & DateTime.Now.AddDays(1).ToString & """")
            End If
            res.End()
        Else
            MyBase.Render(writer)
        End If
    End Sub
End Class
