Imports System.IO

#If DEBUG Then
Partial Public Class JQueryPageJS
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JQueryPageJS
        Inherits System.Web.UI.Page
#End If

        Private Sub JQueryPageJS_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Clear()
            Response.ContentType = "application/x-javascript"
            Dim strm As Stream = OpenFile("~/res/DTICalendar/JQueryPage.js")
            Dim ppJs As String
            Using cssReader As New StreamReader(strm)
                ppJs = cssReader.ReadToEnd
            End Using
            ppJs = ppJs.Replace("##URLKey##", Me.Request.UrlReferrer.AbsoluteUri)
            Using writer As New StreamWriter(Response.OutputStream)
                writer.Write(ppJs)
            End Using
        End Sub

    End Class