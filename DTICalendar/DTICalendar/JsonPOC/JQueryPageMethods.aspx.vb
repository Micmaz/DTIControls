Imports System.Web.Services
#If DEBUG Then
Partial Public Class JQueryPageMethods
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JQueryPageMethods
        Inherits BaseClasses.BaseSecurityPage
#End If

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        <WebMethod()> _
        Public Shared Function GetDate(ByVal [when] As String) As String
            Dim str As String
            Using strR As New IO.StreamReader(HttpContext.Current.Request.InputStream)
                str = strR.ReadToEnd
            End Using
            Select Case [when]
                Case "Yesterday"
                    Return DateTime.Now.AddDays(-1).ToString()
                Case "Today"
                    Return DateTime.Now.ToString()
                Case "Tomorrow"
                    Return DateTime.Now.AddDays(1).ToString()
            End Select
            Return DateTime.Now.ToString()
        End Function
    End Class