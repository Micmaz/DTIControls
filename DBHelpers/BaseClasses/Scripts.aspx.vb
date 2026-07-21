Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Web
Imports BaseClasses
Imports BaseClasses.MimeDecoder
Imports BaseClasses.BaseVirtualPathProvider
Imports System.Text.RegularExpressions
Imports System.Collections.Generic

''' <summary>
''' A lightweight IHttpHandler that loads any images / javascript / css files from the virtual path provider.
''' Removes the need to alter a web.config to view embedded resources, fixes path problems in css files, and
''' adds client side caching. Served (drop-in, via BaseVirtualPathProvider) at:
'''   http://localhost/~/res/BaseClasses/Scripts.ashx?f=baseclasses/TestResource.jpg
''' Was a System.Web.UI.Page; converted to a handler so each asset request no longer pays the full page
''' lifecycle. The legacy "Scripts.aspx" URL still works via ScriptsLegacyPage (below) for back-compat.
''' </summary>
''' <remarks></remarks>
<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)>
Public Class Scripts
	Implements System.Web.IHttpHandler

	Private Shared minScripts As New Hashtable
	Private Shared fixedCssFiles As New Hashtable
	Private Shared LastModified As Date = Nothing

	' Per-request state. IsReusable = False, so a fresh instance is created per request (matches the old Page).
	Private _context As HttpContext
	Private responseEnded As Boolean = False

	Private ReadOnly Property Request() As HttpRequest
		Get
			Return _context.Request
		End Get
	End Property

	Private ReadOnly Property Response() As HttpResponse
		Get
			Return _context.Response
		End Get
	End Property

	Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
		Get
			Return False
		End Get
	End Property

	''' <summary>
	''' Handler entry point. Runs the header/caching logic (formerly Page_Init), then streams the resource
	''' (formerly Page_Load) unless the request already completed (304 / redirect).
	''' </summary>
	''' <param name="context"></param>
	''' <remarks></remarks>
	Public Sub ProcessRequest(ByVal context As HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
		_context = context
		prepareResponse()
		If responseEnded Then Return
		writeResource()
	End Sub

	Private _filename As String = Nothing

	''' <summary>
	''' Filename read from query string. uses f=[resourceName]
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Filename read from query string. uses f=[resourceName]")>
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
	''' Streams the requested resource. If we get here the item is either uncached on the client or the app is
	''' in debug mode. (Formerly Page_Load.)
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Streams the requested resource to the client.")>
	Private Sub writeResource()
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
					SyncLock minScripts
					If Not minScripts.Contains(filename) Then
						Dim strOut As String
						Using scriptReader As New StreamReader(BaseVirtualPathProvider.getResourceStream("/res/" & filename))
							strOut = scriptReader.ReadToEnd
						End Using
						minScripts.Add(filename, strOut)
					End If
					End SyncLock
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
	<System.ComponentModel.Description("Writes the string to the output stream.")>
	Private Sub writeStringResponse(ByRef str As String)
		Using writer As New StreamWriter(Response.OutputStream)
			writer.Write(str)
		End Using
	End Sub

	''' <summary>
	''' Writes the file to the output stream.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Writes the file to the output stream.")>
	Private Sub writeFileFromAssembly()
		Try
			Dim strm As Stream = BaseVirtualPathProvider.getResourceStream("/res/" & filename)
			If strm IsNot Nothing Then
				Dim buff(strm.Length) As Byte
				Using strm
					strm.Read(buff, 0, strm.Length)
				End Using
				Response.OutputStream.Write(buff, 0, buff.Length - 1)
				strm.Close()
			End If
		Catch ex As Exception

		End Try
	End Sub

	''' <summary>
	''' Minimizes a stream of javascript
	''' </summary>
	''' <param name="jsFile"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Minimizes a stream of javascript")>
	Private Function minimizeFile(ByRef jsFile As Stream) As String
		Return JsMinimizer.SMinify(jsFile)
	End Function

	''' <summary>
	''' web.config appSettings key used to opt out of root-relative resource URLs.
	''' Set &lt;add key="DTIScriptsRootRelative" value="false"/&gt; for apps NOT hosted at a site root
	''' where a root-relative ("/~/res/...") URL would resolve outside the application.
	''' </summary>
	Private Const RootRelativeSettingKey As String = "DTIScriptsRootRelative"

	''' <summary>
	''' Returns the url to the scripts handler. (e.g. "/~/res/BaseClasses/Scripts.ashx?f=baseclasses/TestResource.jpg")
	''' The prefix is root-relative so the browser caches each resource once across every page in the site
	''' (the leading "~" is stripped server-side by BaseVirtualPathProvider.getFilename).
	''' </summary>
	''' <param name="debug">optional set to true to prevent compression of js files</param>
	''' <returns>The string to prepend to urls to utilize the Scripts handler resource</returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Returns the url to the scripts handler. (e.g. ""/~/res/BaseClasses/Scripts.ashx?f="")")>
	Shared Function ScriptsURL(Optional ByVal debug As Boolean = False) As String
		If scriptsURLHolder IsNot Nothing Then Return scriptsURLHolder
		scriptsURLHolder = buildScriptsURL()
		Return scriptsURLHolder
	End Function

	''' <summary>
	''' Builds the resource-url prefix. Root-relative and application-aware so the emitted &lt;script&gt;/&lt;link&gt;/img
	''' urls are identical on every page (enabling cross-page client caching), and correct for sub-applications.
	''' </summary>
	''' <remarks></remarks>
	Private Shared Function buildScriptsURL() As String
		' Opt-out for odd hosting (reverse proxy, externally-handled nested vdir, etc.): keep legacy app-relative behavior.
		Try
			Dim cfg As String = System.Configuration.ConfigurationManager.AppSettings(RootRelativeSettingKey)
			If cfg IsNot Nothing AndAlso cfg.Trim().ToLower() = "false" Then
				Return "~/res/BaseClasses/Scripts.ashx?f="
			End If
		Catch ex As Exception
		End Try

		' Auto-detect the application's virtual root so sub-apps work without config:
		'   root app  -> ""        -> "/~/res/..."
		'   "/myapp"  -> "/myapp"  -> "/myapp/~/res/..."
		Dim appPath As String = ""
		Try
			appPath = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath
		Catch ex As Exception
		End Try
		If appPath Is Nothing OrElse appPath = "/" Then appPath = ""
		If appPath.EndsWith("/") Then appPath = appPath.Substring(0, appPath.Length - 1)

		Return appPath & "/~/res/BaseClasses/Scripts.ashx?f="
	End Function
	Private Shared scriptsURLHolder As String = Nothing

	''' <summary>
	''' Determins weather resource should be gzipped on return.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Determins weather resource should be gzipped on return.")>
	Public Shared Function GZipSupported() As Boolean
		Try
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
	<System.ComponentModel.Description("Determins weather requested item has been modified since it's last request.")>
	Public Function isModified() As Boolean
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

	''' <summary>
	''' Sets caching headers and handles 304 / redirect short-circuits. (Formerly Page_Init.)
	''' Sets responseEnded = True when the request is already complete.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Sets caching headers and handles 304 / redirect short-circuits.")>
	Private Sub prepareResponse()
		If Request.Url.LocalPath.IndexOf("~/") <> Request.Url.LocalPath.LastIndexOf("~/") Then
			Response.Redirect(Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("~/")) & Request.Url.Query, False)
			responseEnded = True
			Return
		End If
		If Not Request.QueryString.Item("reset") Is Nothing AndAlso Request.QueryString.Item("reset").ToLower = "y" Then
			LastModified = Nothing
			ClearResources()
		End If
		If LastModified = Nothing Then
			Try
				LastModified = System.IO.File.GetLastWriteTime(GetType(Scripts).Assembly.Location)
			Catch ex As Exception
				LastModified = Date.Now
			End Try
		End If

		If Not isModified() Then
			Response.Clear()
			Response.StatusCode = 304
			Response.ContentType = Nothing
			Response.StatusDescription = "Not Modified"
			Response.AddHeader("Content-Length", "0")
			responseEnded = True
			Return
		End If

		Response.Cache.SetCacheability(Web.HttpCacheability.Public)
		Response.Cache.SetMaxAge(TimeSpan.FromDays(7))
		Response.Cache.SetExpires(DateTime.UtcNow.AddDays(7))
		Response.Cache.SetValidUntilExpires(True)
		Response.Cache.SetLastModified(LastModified)
		Response.AppendHeader("Vary", "Content-Encoding")
		Response.Cache.SetETag(etag)

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
	<System.ComponentModel.Description("Generates a MD5 hash of a given input string")>
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
	''' Gets an etag for client caching control based on the requested file and the assembly version.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Gets an etag for client caching control based on assembly version for stable caching.")>
	Public ReadOnly Property etag() As String
		Get
			Dim version As String = GetType(Scripts).Assembly.GetName().Version.ToString()
			Return """" & GenerateHash(filename & version).Replace("-", "") & """"
		End Get
	End Property

End Class

''' <summary>
''' Backward-compatibility shim for the legacy "~/res/BaseClasses/Scripts.aspx?f=..." URL. New code emits the
''' lighter-weight ".ashx" handler via Scripts.ScriptsURL(); this Page just delegates to that handler so any
''' previously-rendered / hard-coded ".aspx" links keep working.
''' </summary>
''' <remarks></remarks>
<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)>
Public Class ScriptsLegacyPage
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim handler As New Scripts()
		handler.ProcessRequest(Me.Context)
	End Sub
End Class
