Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports BaseClasses
Imports BaseClasses.MimeDecoder
Imports BaseClasses.BaseVirtualPathProvider
Imports System.Text.RegularExpressions
Imports System.Collections.Generic

''' <summary>
''' A helper page to load any images or javascript files from the virtual path provider.  Removes the need to alter a web config to view images and js files as embedded resources. 
''' Also Fixes path problems in css files as well as add client side caching.
''' Format is: http://localhost/res/Baseclasses/Scripts.aspx?f=baseclasses.TestResource.jpg
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Partial Public Class Scripts
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class Scripts
        Inherits System.Web.UI.Page
#End If

    Private Shared minScripts As New Hashtable
    Private Shared fixedCssFiles As New Hashtable
    Private Shared etags As New Hashtable
    Private Shared LastModified As Date = Nothing

	Private _filename As String = Nothing

	''' <summary>
	''' Filename read from query string. uses f=[resourceName]
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
        <System.ComponentModel.Description("Filename read from query string. uses f=[resourceName]")> _
        Private ReadOnly Property filename() As String
		Get
			If _filename Is Nothing Then
				Try
					_filename = Request.QueryString.Item("f")
					_filename = BaseVirtualPathProvider.getFilename(_filename)
				Catch ex As Exception
					_filename = Nothing
				End Try
			End If
			Return _filename
		End Get
	End Property

	Public ReadOnly Property fileExtension As String
		Get
			Return filename.Substring(filename.LastIndexOf(".")).ToLower()
		End Get
	End Property

	''' <summary>
	''' The load event. If it makes it here the item is either uncached on the client or the app is in debug mode.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
    <System.ComponentModel.Description("The load event. If it makes it here the item is either uncached on the client or the app is in debug mode.")> _
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		'registerVirtualPathProvider()
		If responseEnded Then Return
		Response.Clear()
		Response.ContentType = "application/octet-stream"
		If filename IsNot Nothing Then

			If fileExtension.EndsWith("js") Then
				Response.ContentType = "application/x-javascript"
			ElseIf fileExtension.EndsWith("css") Then
				Response.ContentType = "text/css"
			Else
				Response.ContentType = MimeType(fileExtension)
			End If
			If Response.ContentType = "application/x-javascript" Then
				Try
					If Not minScripts.Contains(filename) Then
						Dim strOut As String
						Using scriptReader As New StreamReader(BaseVirtualPathProvider.getResourceStream("/res/" & filename))
							strOut = scriptReader.ReadToEnd
						End Using
						'If filename.ToLower().StartsWith("jQueryLibrary/jquery-3.2.1.min.js".ToLower()) Then
						minScripts.Add(filename, strOut)
						'Else
						'	minScripts.Add(filename, "(function( $ ) {" & vbCrLf & strOut & vbCrLf & "})(jQuery);")
						'End If


					End If
						writeStringResponse(minScripts.Item(filename))
				Catch ex As Exception
					writeFileFromAssembly()
				End Try
			ElseIf Response.ContentType = "text/css" Then
				SyncLock fixedCssFiles
					If Not fixedCssFiles.Contains(filename) Then
						Dim strOut As String
						Try
							Using cssReader As New StreamReader(BaseVirtualPathProvider.getResourceStream("/res/" & filename))
								strOut = cssReader.ReadToEnd
							End Using

							Dim assemName As String = getResourcesName("/res/" & filename)(1)
							'remove the css file name to get the directory structure and
							assemName = assemName.Substring(0, assemName.LastIndexOf("."))
							assemName = assemName.Substring(0, assemName.LastIndexOf(".")).Replace(".", "/")
							'insert that into the css
							Dim replaced As New List(Of String)
							Dim findUrl As Regex = New Regex("url\(['""]?(?<filename>[^')""]*)['""]?\)", RegexOptions.IgnoreCase)
					For Each res As Match In findUrl.Matches(strOut)
								'find and ignore the crazy data:image/gif,base64 construct
								If res.Groups("filename").Value.IndexOf("data:") = -1 AndAlso Not replaced.Contains(res.Groups("filename").Value) Then
									replaced.Add(res.Groups("filename").Value)
									strOut = strOut.Replace(res.Groups("filename").Value, ScriptsURL() & assemName & "/" & res.Groups("filename").Value)
								End If
							Next

							fixedCssFiles.Add(filename, strOut)
						Catch ex As Exception

						End Try

					End If
				End SyncLock
				Try
					writeStringResponse(fixedCssFiles.Item(filename))
				Catch ex As Exception
					writeFileFromAssembly()
				End Try
			Else
				writeFileFromAssembly()
			End If
		End If

	End Sub

    ''' <summary>
    ''' Writes the string to the output stream.
    ''' </summary>
    ''' <param name="str"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Writes the string to the output stream.")> _
        Private Sub writeStringResponse(ByRef str As String)
            Using writer As New StreamWriter(Response.OutputStream)
                writer.Write(str)
            End Using
        End Sub

    ''' <summary>
    ''' Writes the file to the output stream.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Writes the file to the output stream.")> _
        Private Sub writeFileFromAssembly()
            Try
                Dim strm As Stream = BaseVirtualPathProvider.getResourceStream("/res/" & filename)
                Dim buff(strm.Length) As Byte
                Using strm
                    strm.Read(buff, 0, strm.Length)
                End Using
                Response.OutputStream.Write(buff, 0, buff.Length - 1)
                strm.Close()
            Catch ex As Exception

            End Try
        End Sub

    ''' <summary>
    ''' Minimizes a stream of javascript
    ''' </summary>
    ''' <param name="jsFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Minimizes a stream of javascript")> _
        Private Function minimizeFile(ByRef jsFile As Stream) As String
            Return JsMinimizer.SMinify(jsFile)
        End Function

	''' <summary>
	''' Returns the url to the scripts.aspx page. (e.g. "~/res/BaseClasses/Scripts.aspx?f=baseclasses/TestResource.jpg")
	''' </summary>
	''' <param name="debug">optional set to true to prevent compression of js files</param>
	''' <returns>The string to prepend to urls to utilize the Scripts.aspx resource</returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Returns the url to the scripts.aspx page. (e.g. ""~/res/BaseClasses/Scripts.aspx?f="")")>
	Shared Function ScriptsURL(Optional ByVal debug As Boolean = False) As String
		If scriptsURLHolder Is Nothing Then Return "~/res/BaseClasses/Scripts.aspx?f="
		Return scriptsURLHolder
	End Function
	Private Shared scriptsURLHolder As String = Nothing

	''' <summary>
	''' Determins weather resource should be gzipped on return. 
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
        <System.ComponentModel.Description("Determins weather resource should be gzipped on return.")> _
        Public Shared Function GZipSupported() As Boolean
			try
				Dim AcceptEncoding As String = System.Web.HttpContext.Current.Request.Headers("Accept-Encoding")
				If Not String.IsNullOrEmpty(AcceptEncoding) And (AcceptEncoding.Contains("gzip") Or AcceptEncoding.Contains("deflate")) Then
					Return True
				End If
            Catch ex As Exception
                Return False
            End Try				
            Return False
        End Function

    ''' <summary>
    ''' Determins weather requested item has been modified since it's last request. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determins weather requested item has been modified since it's last request.")> _
    Public Function isModified() As Boolean
        'Return True
#If DEBUG Then
        Return True
#End If
        Dim modSince As DateTime
        If Not String.IsNullOrEmpty(Request.Headers("If-None-Match")) Then
            If Request.Headers("If-None-Match") = etag Then Return False Else Return True
        End If
        If Not String.IsNullOrEmpty(("If-Modified-Since")) Then
            If Date.TryParse(Request.Headers("If-Modified-Since"), modSince) Then
                If LastModified.AddSeconds(-1) > modSince Then
                    Return True
                Else
                    Return False
                End If
            End If
        End If
        Return True
    End Function

	Private responseEnded As Boolean = False
	''' <summary>
	''' Handles the init event of the page. Will end the responce if the item is unmodified and therefore uses the client cache.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Handles the init event of the page. Will end the responce if the item is unmodified and therefore uses the client cache.")>
	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		If scriptsURLHolder Is Nothing Then
			scriptsURLHolder = Request.Url.AbsolutePath & "?f="
		End If
		If Request.Url.LocalPath.IndexOf("~/") <> Request.Url.LocalPath.LastIndexOf("~/") Then
			Response.Redirect(Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("~/")) & Request.Url.Query, False)
			responseEnded = True
			Return
		End If
		If Not Request.QueryString.Item("reset") Is Nothing AndAlso Request.QueryString.Item("reset").ToLower = "y" Then
			LastModified = Nothing
			ClearResources()
		End If
		If LastModified = Nothing Then LastModified = Date.Now
		Response.Cache.SetCacheability(Web.HttpCacheability.ServerAndPrivate)
		Response.Cache.SetLastModified(LastModified)
		Response.AppendHeader("Vary", "Content-Encoding")
		Response.Cache.SetETag(etag)

		If Not isModified() Then
			Response.Clear()
			Response.StatusCode = 304
			Response.ContentType = Nothing
			Response.StatusDescription = "Not Modified"
			Response.AddHeader("Content-Length", "0")
			'Response.Cache.SetCacheability(Web.HttpCacheability.Public)
			'Response.Cache.SetLastModified(LastModified)
			'Response.End()
			responseEnded = True
			Return
		End If

		If GZipSupported() Then
			Dim AcceptEncoding As String = System.Web.HttpContext.Current.Request.Headers("Accept-Encoding")
			If AcceptEncoding.Contains("deflate") Then
				Response.Filter = New System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress)
				Response.AppendHeader("Content-Encoding", "deflate")
			Else

				Response.Filter = New Compression.GZipStream(Response.Filter, Compression.CompressionMode.Compress)
				Response.AddHeader("Content-Encoding", "gzip")
			End If
		End If

	End Sub

	''' <summary>
	''' Generates a MD5 hash of a given input string
	''' </summary>
	''' <param name="input">String to hash</param>
	''' <returns>MD5 hash of String</returns>
	''' <remarks></remarks>
        <System.ComponentModel.Description("Generates a MD5 hash of a given input string")> _
        Public Shared Function GenerateHash(ByVal input As String) As String
            Dim md5Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()
            Dim hashedBytes As Byte()
            Dim encoder As New System.Text.UTF8Encoding()

            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(input))

            Dim strOutput As New System.Text.StringBuilder(hashedBytes.Length)

            For i As Integer = 0 To hashedBytes.Length - 1
                strOutput.Append(hashedBytes(i).ToString("X2"))
            Next

            Return strOutput.ToString()
        End Function

    ''' <summary>
    ''' Gets an etag for client caching control based on the date a resource was last read from the hard disk.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Gets an etag for client caching control based on the date a resource was last read from the hard disk.")> _
        Public ReadOnly Property etag() As String
            Get
                'Return GenerateHash(filename & LastModified).Replace("-", "")
                Return """" & GenerateHash(filename & LastModified).Replace("-", "") & """"
            End Get
        End Property


    End Class