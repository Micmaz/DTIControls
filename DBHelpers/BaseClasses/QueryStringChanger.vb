Imports System.Collections.Specialized
Imports System.Web
Imports System.Text

''' <summary>
''' reads the query string, as well as setting it's values. Then forwards the responce to an apropriate query string url.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class QueryStringChanger
    Inherits System.Collections.Specialized.NameValueCollection
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class QueryStringChanger
        Inherits System.Collections.Specialized.NameValueCollection
#End If

    ''' <summary>
    ''' Default constructor examins current request query string.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Default constructor examins current request query string.")> _
        Public Sub New()
            For Each key As String In HttpContext.Current.Request.QueryString.AllKeys
                Add(key, HttpContext.Current.Request.QueryString(key))
            Next
        End Sub

    ''' <summary>
    ''' Return the current query string.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Return the current query string.")> _
        Public Overrides Function ToString() As String
            Dim sb As StringBuilder = New StringBuilder
            For Each key As String In Me.Keys
                If Keys(0) = key Then
                    sb.Append("?" & key & "=" & HttpContext.Current.Server.UrlEncode(Me(key)))
                Else
                    sb.Append("&" & key & "=" & HttpContext.Current.Server.UrlEncode(Me(key)))
                End If
            Next
            Return sb.ToString
        End Function

    ''' <summary>
    ''' Add a key/value string pair to the query
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Add a key/value string pair to the query")> _
        Public Overrides Sub Add(ByVal key As String, ByVal value As String)
            Try
                If Me.GetValues(key).Length > 0 Then
                    Me(key) = value
                Else
                    MyBase.Add(key, value)
                End If
            Catch ex As Exception
                MyBase.Add(key, value)
            End Try
        End Sub

    ''' <summary>
    ''' Returns the full url with reformatted query.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Returns the full url with reformatted query.")> _
        Public ReadOnly Property FullUrl() As String
            Get
                Dim redirectstr As String
                Try
                    redirectstr = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.Query, "") & Me.ToString
                Catch ex As Exception
                    redirectstr = HttpContext.Current.Request.Url.AbsoluteUri & Me.ToString
                End Try
                Return redirectstr
            End Get
        End Property

    ''' <summary>
    ''' Redirects the responce to the formatted url/query
    ''' </summary>
    ''' <param name="endResponse"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Redirects the responce to the formatted url/query")> _
        Public Sub redirectToMyUrl(Optional ByVal endResponse As Boolean = False)
            HttpContext.Current.Response.Redirect(Me.FullUrl, endResponse)
        End Sub

    ''' <summary>
    ''' removes a given key
    ''' </summary>
    ''' <param name="name"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("removes a given key")> _
        Public Overrides Sub Remove(ByVal name As String)
            Try
                MyBase.Remove(name)
            Catch ex As Exception
            End Try
        End Sub

    ''' <summary>
    ''' replaces an existing query value and redirects the url immediatly.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <param name="endResponse"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("replaces an existing query value and redirects the url immediatly.")> _
        Public Shared Sub ReplaceQuery(ByVal key As String, ByVal value As String, Optional ByVal endResponse As Boolean = False)
            Dim mqs As New QueryStringChanger
            mqs.Add(key, value)
            HttpContext.Current.Response.Redirect(mqs.FullUrl, endResponse)
        End Sub

    ''' <summary>
    ''' replace a query value and returns the new full url.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("replace a query value and returns the new full url.")> _
        Public Shared Function ReturnReplaceQuery(ByVal key As String, ByVal value As String) As String
            Dim mqs As New QueryStringChanger
            mqs.Add(key, value)
            Return mqs.FullUrl
        End Function
    End Class
