Imports System.Web
''' <summary>
''' Tracks where a user has been on a web application. Usefull for adding a breadcrumb or other navigation stack to an application.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class PageTracker
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class PageTracker
#End If

        Dim sqlHelper As New SQLHelper
        Public Event PageTracked(ByVal trackerRow As dsBaseClasses.DTIPageTrackerRow)

        Public Sub New(Optional ByVal sqlConn As SqlClient.SqlConnection = Nothing)
            If Not sqlConn Is Nothing Then sqlHelper.defaultConnection = sqlConn
        End Sub

        Public Property DefaultConnection() As SqlClient.SqlConnection
            Get
                Return sqlHelper.defaultConnection
            End Get
            Set(ByVal value As SqlClient.SqlConnection)
                sqlHelper.defaultConnection = value
            End Set
        End Property

        Private Property LastPageTrackingId() As Long
            Get
                If HttpContext.Current.Session("LastPageTrackingId") Is Nothing Then
                    HttpContext.Current.Session("LastPageTrackingId") = -1
                End If
                Return HttpContext.Current.Session("LastPageTrackingId")
            End Get
            Set(ByVal value As Long)
                HttpContext.Current.Session("LastPageTrackingId") = value
            End Set
        End Property

        Public Sub trackPage(ByVal request As HttpRequest, ByVal sessionId As String, Optional ByVal sqlConn As SqlClient.SqlConnection = Nothing)
            If Not sqlConn Is Nothing Then sqlHelper.defaultConnection = sqlConn

            Dim ip As String = request.ServerVariables("REMOTE_ADDR")
            Dim urlFile As String = request.Url.AbsolutePath
            Dim queryString As String = request.Url.Query

            Try
                Dim refererURL As New Uri(request.ServerVariables("HTTP_REFERER"))

                If Not refererURL.Host = request.Url.Host Then
                    insertTrackerRow(ip, refererURL.Query, sessionId, refererURL.OriginalString.Replace(refererURL.Query, ""), -1)
                End If
            Catch ex As Exception
            End Try

            insertTrackerRow(ip, queryString, sessionId, urlFile, LastPageTrackingId)

        End Sub

        Private Sub insertTrackerRow(ByVal ip As String, ByVal q As String, ByVal sessId As String, ByVal url As String, ByVal lastTrackId As Long)
            Dim myTrackerTable As New dsBaseClasses.DTIPageTrackerDataTable
            Dim currentPage As dsBaseClasses.DTIPageTrackerRow = myTrackerTable.NewDTIPageTrackerRow

            With currentPage
                .clientIP = ip
                .Query = q
                .SessionId = sessId
                .URL = url
                .lastPageTrackId = lastTrackId
            End With

            myTrackerTable.AddDTIPageTrackerRow(currentPage)

            Try
                sqlHelper.Update(myTrackerTable)
            Catch ex As Exception
                sqlHelper.checkAndCreateTable(myTrackerTable)
                Try
                    sqlHelper.Update(myTrackerTable)
                Catch innerException As Exception

                End Try
            End Try

            LastPageTrackingId = currentPage.Id
            RaiseEvent PageTracked(currentPage)
        End Sub

    End Class
