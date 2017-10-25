Imports System.Web.Configuration
Imports System.IO

''' <summary>
''' HTTP handler for Flash movies for pseudo streaming
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Partial Public Class ViewMovie
    Implements IHttpHandler
#Else
                <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
                Partial Public Class ViewMovie
                    Implements IHttpHandler
#End If
    'Protected Shared ReadOnly Property Session() As HttpSessionState
    '    Get
    '        Return HttpContext.Current.Session
    '    End Get
    'End Property

    'Private ReadOnly Property MyFlashWrapper() As ffmpegHelper
    '    Get
    '        If Session("MyFlashWrapper") Is Nothing Then
    '            Session("MyFlashWrapper") = New ffmpegHelper
    '            Dim _myFlashWrapper As ffmpegHelper = CType(Session("MyFlashWrapper"), ffmpegHelper)
    '            Dim utl As HttpServerUtility
    '            _myFlashWrapper.OutputPath = HttpContext.Current.Server.MapPath("/")
    '        End If
    '        Return Session("MyFlashWrapper")
    '    End Get
    'End Property

    Public ReadOnly Property SaveVideoInDataBase() As Boolean
        Get
            Return False 'MyFlashWrapper.SaveVideoInDataBase
        End Get
    End Property

    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Private Shared ReadOnly _flvheader As Byte() = HexToByte("464C5601010000000900000009")
    Private sqlHelper As BaseClasses.BaseHelper

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim Response As HttpResponse = context.Response
        Dim Request As HttpRequest = context.Request
        sqlHelper = BaseClasses.BaseHelper.getHelper

        Response.Clear()
        Response.ClearHeaders()

        Dim vid_id As Integer
        Dim tmpVidDt As New dsDTIVideo.DTIVideoManagerDataTable
        Dim vid_row As dsDTIVideo.DTIVideoManagerRow

        Try
            Dim filename As String = Path.GetFileNameWithoutExtension(context.Request.FilePath)
            Dim splitString() As String = filename.Split("_")
            vid_id = splitString(splitString.Length - 1)

            sqlHelper.SafeFillTable("select Id, width, height, Length from DTIVideoManager where id = @vid_id", tmpVidDt, New Object() {vid_id})
            vid_row = tmpVidDt.FindById(vid_id)

            If Not vid_row Is Nothing Then
                Try
                    Dim startPos As String = Request("start")
                    Dim pos As Long
                    Dim partLengthPlusHeader As Long
                    Dim totalLength As Long = Convert.ToInt64(vid_row.Length)

                    If String.IsNullOrEmpty(startPos) Then
                        pos = 0
                        partLengthPlusHeader = totalLength
                    Else
                        pos = Convert.ToInt64(startPos)
                        partLengthPlusHeader = Convert.ToInt64(vid_row.Length - pos) + _flvheader.Length
                    End If

                    Response.Cache.SetCacheability(HttpCacheability.[Public])
                    Response.Cache.SetLastModified(DateTime.Now)
                    Response.ContentType = "video/x-flv"
                    Response.AppendHeader("Content-Length", partLengthPlusHeader.ToString())

                    If pos > 0 Then
                        Response.OutputStream.Write(_flvheader, 0, _flvheader.Length)
                    End If

                    Const buffersize As Integer = 16384

                    'If CType(context.Session("MyFlashWrapper"), ffmpegHelper).SaveVideoInDataBase Then
                    If SaveVideoInDataBase Then
                        Dim currentPosition As Long = pos

                        While currentPosition < totalLength
                            If Response.IsClientConnected Then
                                Dim mytempVidTable As New DataTable
                                Dim partLength As Long = totalLength - currentPosition

                                sqlHelper.FillDataTable("Select Left(Right(Cast(convert(varchar(max),Video), " & partLength & _
                                    "), " & buffersize & ") as FLVVideo from " & tmpVidDt.TableName & _
                                    " where Id = " & vid_id, mytempVidTable)

                                Dim buffer As Byte() = mytempVidTable.Rows(0).Item("FLVVideo")

                                Response.OutputStream.Write(buffer, 0, buffersize)

                                currentPosition += buffersize
                            End If
                        End While
                    Else
                        Using fs As New FileStream(context.Server.MapPath(context.Request.FilePath), FileMode.Open, FileAccess.Read, FileShare.Read)
                            Dim buffer As Byte() = New Byte(buffersize - 1) {}
                            Dim count As Integer = fs.Read(buffer, 0, buffersize)

                            While count > 0
                                If context.Response.IsClientConnected Then
                                    context.Response.OutputStream.Write(buffer, 0, count)
                                    count = fs.Read(buffer, 0, buffersize)
                                Else
                                    count = -1
                                End If
                            End While
                        End Using
                    End If

                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
            If Not sqlHelper.checkDBObjectExists(tmpVidDt.TableName) Then
                sqlHelper.createTable(tmpVidDt)
            End If
        End Try
    End Sub

    Private Shared Function HexToByte(ByVal hexString As String) As Byte()
        Dim returnBytes As Byte() = New Byte(hexString.Length / 2 - 1) {}
        For i As Integer = 0 To returnBytes.Length - 1
            returnBytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
        Next
        Return returnBytes
    End Function
End Class