Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.Caching
Imports System.Web.Hosting
Imports System.Reflection

''' <summary>
''' Allows embedded resources to be loaded as direct web calls. Format is http://localhost/res/[AssemblyName]/[Embedded_Resource]
''' a web.config change is needed for files not normally handeled by .NET. By default *.aspx  and *.ascx files are handeled automatically. 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Allows embedded resources to be loaded as direct web calls. Format is http://localhost/res/[AssemblyName]/[Embedded_Resource] a web.config change is needed for files not normally handeled by .NET. By default *.aspx and *.ascx files are handeled automatically.")> _
Public Class BaseVirtualPathProvider
    Inherits System.Web.Hosting.VirtualPathProvider

#Region "Shared Attributes"

    ''' <summary>
    ''' The shortest time in seconds between two assembly fetches. Prevents too-frequent resource retrieval.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The shortest time in seconds between two assembly fetches. Prevents too-frequent resource retrieval.")> _
    Public Shared rebuildWait As Integer = 30

    ''' <summary>
    ''' The date the assemblies were last fetched from disk to memory.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The date the assemblies were last fetched from disk to memory.")> _
    Public Shared lastrebuild As Date

    ''' <summary>
    ''' a shared hash of the embedded resources and their common name.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("a shared hash of the embedded resources and their common name.")> _
    Public Shared resources As Hashtable

    ''' <summary>
    ''' Cached assemblies in the BaseVirtualPathProvider. Is kept in memory for the lifetime of the application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Cached assemblies in the BaseVirtualPathProvider. Is kept in memory for the lifetime of the application.")> _
    Public Shared ReadOnly Property assemblies() As Generic.List(Of Assembly)
        Get
            If _assemblies Is Nothing Then _assemblies = New Generic.List(Of Assembly)
            Return _assemblies
        End Get
    End Property
    Private Shared _assemblies As Generic.List(Of Assembly)

    ''' <summary>
    ''' a hashtable linking to the resources in the assemblies list. Links a cached assembly with it's name as a string.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("a hashtable linking to the resources in the assemblies list. Links a cached assembly with it's name as a string.")> _
    Private Shared _assemblyClassHash As Generic.Dictionary(Of String, Assembly)
    Public Shared ReadOnly Property assemblyClassHash() As Generic.Dictionary(Of String, Assembly)
        Get
            If _assemblyClassHash Is Nothing Then _assemblyClassHash = New Generic.Dictionary(Of String, Assembly)
            Return _assemblyClassHash
        End Get
    End Property

	''' <summary>
	''' Returns weather the baseVirtualPathProvider has been initialized or not. 
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Returns weather the baseVirtualPathProvider has been initialized or not.")>
	Public Shared initialized As Boolean = False
	'   Public Shared Function initialized() As Boolean
	'	Return Not resources Is Nothing AndAlso resources.Count > 0
	'End Function

	''' <summary>
	''' Registers the virtual path provider in the application. Ideally this call is added to the global.asax file on application start.
	''' </summary>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Registers the virtual path provider in the application. Ideally this call is added to the global.asax file on application start.")>
	Public Shared Sub registerVirtualPathProvider()
		If Not BaseVirtualPathProvider.initialized Then
			System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(New BaseVirtualPathProvider())
			Debug.WriteLine("Initialized virtual path provider.")
			testVirtualPathProvider()
			'BaseVirtualPathProvider.initialized = True
		End If
		Debug.WriteLine("Path Provider registered. all done.")
	End Sub

	''' <summary>
	''' Spiders available dlls and caches all embedded resources for fast retrieval by a webserver. 
	''' </summary>
	''' <param name="assemblyname"></param>
	''' <remarks></remarks>
    <System.ComponentModel.Description("Spiders available dlls and caches all embedded resources for fast retrieval by a webserver.")> _
    Public Shared Sub buildLocalResources(Optional ByVal assemblyname As String = "")
        If resources Is Nothing Then
            resources = New Hashtable
        End If
		If assemblyname = "" Then
			'For Each asm As System.Reflection.Assembly In Assembly.GetEntryAssembly().GetReferencedAssemblies
			'Try
			'    For Each dllfile As String In System.IO.Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll")
			'        cacheAssembly(Assembly.LoadFile(dllfile))
			'    Next
			'Catch ex As Exception

			'End Try
			For Each asm As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies
				cacheAssembly(asm)
			Next
		Else
			Try
				cacheAssembly(Assembly.Load(assemblyname))
			Catch ex As Exception
				Dim shortasmname As String = assemblyname.ToLower
				If shortasmname.IndexOf(".") > -1 Then shortasmname = shortasmname.Substring(0, shortasmname.IndexOf("."))
				Dim location As String = AppDomain.CurrentDomain.RelativeSearchPath
				For Each dllfile As String In System.IO.Directory.GetFiles(location, "*.dll")
					Dim justfile As String = dllfile.Substring(location.Length).ToLower
					If justfile.Contains(shortasmname) Then
						cacheAssembly(Assembly.LoadFile(dllfile))
					End If
				Next
			End Try
		End If
		Debug.WriteLine("Finished caching all assemblies.")
	End Sub

    ''' <summary>
    ''' Adds assembly to the cache.
    ''' </summary>
    ''' <param name="asm"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds assembly to the cache.")> _
    Private Shared Sub cacheAssembly(ByVal asm As Assembly)
        If Not assemblies.Contains(asm) Then assemblies.Add(asm)
        Dim localpath As String = Assembly.GetExecutingAssembly.CodeBase
        localpath = localpath.Replace("file:///", "")
        localpath = localpath.Replace("\", "/")
        If localpath.LastIndexOf("/") > -1 Then
            localpath = localpath.Substring(0, localpath.LastIndexOf("/")).ToLower
        End If
        Try
            Dim loc As String = asm.CodeBase.Replace("\", "/").ToLower
            If loc.Contains(localpath) Then
				For Each rname As String In asm.GetManifestResourceNames
					resources(rname.ToLower.Replace("_", "")) = New Object() {asm, rname}
					'resources.Add(rname.ToLower.Replace("_", ""), New Object() {asm, rname})
				Next
			End If
        Catch ex As Exception
            Dim x As Integer
            x = 2
        End Try
    End Sub

	''' <summary>
	''' Formats the filename of a requested file.
	''' </summary>
	''' <param name="_filename"></param>
	''' <returns></returns>
<System.ComponentModel.Description("Formats the filename of a requested file.")> _
	Public Shared Function getFilename(_filename As String) As String
		_filename = _filename.Replace("/./", "/")
		If _filename.LastIndexOf("f=") > -1 Then
			_filename = _filename.Substring(_filename.LastIndexOf("f=") + 2)
		End If
		_filename = _filename.Substring(_filename.LastIndexOf("~") + 1)
		If _filename.IndexOf("/res/") > -1 Then
			_filename = _filename.Replace("/res/", "")
		ElseIf _filename.IndexOf("res/") > -1 Then
			_filename = _filename.Replace("res/", "")
		ElseIf _filename.IndexOf("/") = 0 Then
			_filename = _filename.Substring(1)
		End If
		If _filename.Contains("?") Then
			_filename = _filename.Substring(0, _filename.IndexOf("?"))
		End If
		If _filename.Contains("/../") Then
			Dim pathparts As String() = _filename.Split("/")
			_filename = ""
			For Each foldername As String In pathparts
				If foldername = ".." Then
					_filename = _filename.Substring(0, _filename.LastIndexOf("/"))
				Else
					_filename &= "/" & foldername
				End If
			Next

		End If
		Return _filename
	End Function

	''' <summary>
	''' Clears the current cache of all assemblies. Assemblies will be recached on next access.
	''' </summary>
	''' <remarks></remarks>
    <System.ComponentModel.Description("Clears the current cache of all assemblies. Assemblies will be recached on next access.")> _
    Public Shared Sub ClearResources()
        If Not resources Is Nothing Then resources.Clear()
        resources = Nothing
        resources = New Hashtable
    End Sub

	Public Shared Function testVirtualPathProvider() As Boolean
		If Not BaseVirtualPathProvider.initialized Then
			Dim loaded As Boolean = True
			'Dim approot As String = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) & System.Web.VirtualPathUtility.ToAbsolute("~/")
			Dim req As HttpRequest = Nothing
			Try
				req = HttpContext.Current.Request
			Catch ex As Exception

			End Try
			BaseVirtualPathProvider.initialized = HostingEnvironment.VirtualPathProvider.FileExists("~/res/BaseClasses/TestUC.ascx")
			'If HttpContext.Current IsNot Nothing AndAlso req IsNot Nothing Then
			'	Dim page As System.Web.UI.Page = HttpContext.Current.Handler
			'	loaded = False
			'	If page IsNot Nothing Then
			'		Dim count As Integer = 0

			'		While Not loaded AndAlso count < 20
			'			count += 1
			'			Try
			'				Dim i = page.LoadControl("~/res/BaseClasses/TestUC.ascx")
			'				loaded = True
			'			Catch ex As Exception
			'				Threading.Thread.Sleep(500)
			'			End Try
			'		End While
			'	End If
			'	BaseVirtualPathProvider.initialized = loaded
			'	'I hate doing this but the virtual path provider isn't active until the next load..
			'	HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri, False)
			'Else

			'End If
		End If

		Return BaseVirtualPathProvider.initialized
	End Function

	''' <summary>
	''' Gets the file stream of an embedded resource.
	''' </summary>
	''' <param name="virtualPath"></param>
	''' <returns></returns>
	''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the file stream of an embedded resource.")> _
    Public Shared Function getResourceStream(ByVal virtualPath As String) As System.IO.Stream
        Dim itms As Object() = BaseVirtualPathProvider.getResourcesName(virtualPath)
        If itms Is Nothing Then Return Nothing
        Dim asm As Assembly = itms(0)
        Return asm.GetManifestResourceStream(itms(1))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="virtualPath"></param>
    ''' <param name="retryOnFail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Shared Function getResourcesName(ByVal virtualPath As String, Optional ByVal retryOnFail As Boolean = True) As Object()
        If resources Is Nothing Then resources = New Hashtable
        Dim vp As String = virtualPath.Replace("/", ".").Replace("\", ".").Replace("_", "").ToLower
        If vp.LastIndexOf(".res.") > -1 Then
            If vp.EndsWith(".x.aspx") Then
                vp = vp.Replace(".x.aspx", "")
            End If
            vp = vp.Substring(vp.LastIndexOf(".res."))
            vp = vp.Replace(".res.", "")
            If vp.StartsWith(".") Then
                vp = vp.Substring(1)
            End If
            If vp = "resetembeddedresources.aspx" Then
                ClearResources()
                'rebuildresources()
                Return getResourcesName("/res/baseclasses/ListResources.aspx")
            End If
            If Not resources.ContainsKey(vp) Then
                If vp.LastIndexOf(".") = -1 Then Return Nothing
                Dim asmname As String = vp.Substring(0, vp.LastIndexOf("."))
                If asmname.LastIndexOf(".") = -1 Then Return Nothing
                asmname = asmname.Substring(0, asmname.LastIndexOf("."))
                buildLocalResources(asmname)
            End If
            If resources.ContainsKey(vp) Then
                Return resources(vp)
            End If
        End If
        If retryOnFail Then
            rebuildresources()
            Return getResourcesName(virtualPath, False)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Clears the current resource and assembly caches and refetches all assemblies from disk.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared Sub rebuildresources()
        If lastrebuild = Nothing Then lastrebuild = Date.Today.AddDays(-1)
		If lastrebuild.AddSeconds(rebuildWait) < Date.Now Then
			BaseVirtualPathProvider.initialized = False
			lastrebuild = Date.Now
			If resources Is Nothing Then resources = New Hashtable
			resources.Clear()
			assemblies.Clear()
			_assemblyClassHash = Nothing
			buildLocalResources()
			lastrebuild = Date.Now
		End If
	End Sub

#End Region

    ''' <summary>
    ''' Overridden function to indicate if a file has been found by a web request.
    ''' </summary>
    ''' <param name="virtualPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Overridden function to indicate if a file has been found by a web request.")> _
    Public Overrides Function FileExists(ByVal virtualPath As String) As Boolean
        'If virtualPath.Contains("~/") Then virtualPath = virtualPath.Substring(virtualPath.LastIndexOf("~/"))
        Dim vp As String = virtualPath.Replace("/", ".").Replace("\", ".").Replace("_", "").ToLower
        If vp.Contains(".res.") Then
            Dim itm As Object() = getResourcesName(virtualPath)
            If Not itm Is Nothing Then Return True
        ElseIf vp.StartsWith(".page.") Then
            Return True
        End If
        Return MyBase.FileExists(virtualPath)
    End Function

    ''' <summary>
    ''' Overridden function to return the contents of a web request.
    ''' </summary>
    ''' <param name="virtualPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Overridden function to return the contents of a web request.")> _
    Public Overrides Function GetFile(ByVal virtualPath As String) As System.Web.Hosting.VirtualFile
        'If virtualPath.Contains("~/") Then virtualPath = virtualPath.Substring(virtualPath.LastIndexOf("~/"))
        Dim vp As String = virtualPath.Replace("/", ".").Replace("\", ".").Replace("_", "").ToLower
        If vp.StartsWith(".page.") Then
            If System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory & virtualPath) Then Return MyBase.GetFile(virtualPath)
            Return New BaseVirtualFile(virtualPath)
        Else
            If getResourcesName(virtualPath) Is Nothing Then Return MyBase.GetFile(virtualPath)
            Return New BaseVirtualFile(virtualPath)
        End If
    End Function

    'Public Overrides Function DirectoryExists(ByVal virtualDir As String) As Boolean
    '    If virtualDir.Replace("/", ".").Replace("\", ".").ToLower.Contains(".res.") Then Return True
    '    Return MyBase.DirectoryExists(virtualDir)
    'End Function

    'Public Overrides Function GetDirectory(ByVal virtualDir As String) As System.Web.Hosting.VirtualDirectory
    '    If virtualDir.Replace("/", ".").Replace("\", ".").ToLower.Contains(".res.") Then
    '        Dim vdir As System.Web.Hosting.VirtualDirectory = MyBase.GetDirectory("/res/")
    '    End If
    '    Return MyBase.GetDirectory(virtualDir)
    'End Function

    ''' <summary>
    ''' Returnss the cachability of a given resource.
    ''' </summary>
    ''' <param name="virtualPath"></param>
    ''' <param name="virtualPathDependencies"></param>
    ''' <param name="utcStart"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returnss the cachability of a given resource.")> _
    Public Overrides Function GetCacheDependency(ByVal virtualPath As String, ByVal virtualPathDependencies As System.Collections.IEnumerable, ByVal utcStart As Date) As System.Web.Caching.CacheDependency
        Return Nothing
    End Function

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class BaseVirtualFile
        Inherits System.Web.Hosting.VirtualFile
        'Private str As System.IO.Stream

        Public Overrides Function Open() As System.IO.Stream
            Dim itms As Object() = BaseVirtualPathProvider.getResourcesName(vpath)
            If itms Is Nothing Then Return Nothing
            Dim asm As Assembly = itms(0)
			return asm.GetManifestResourceStream(itms(1))
            'Me.str = asm.GetManifestResourceStream(itms(1))
            'Return str
        End Function

        Private vpath As String = ""
        Public Sub New(ByVal vpath As String)
            MyBase.New(vpath)
            If vpath.Contains("~/") Then vpath = vpath.Substring(vpath.LastIndexOf("~/"))

            Dim vp As String = vpath.Replace("/", ".").Replace("\", ".").Replace("_", "").ToLower
            If vp.StartsWith(".page.") Then
                Me.vpath = "~/res/DTIAdminPanel/page.aspx"
            Else
                Me.vpath = vpath
            End If


            'MyBase.New(vpath.Replace("//", "/"))
            'Me.vpath = vpath.Replace("//", "/")
        End Sub

        Protected Overrides Sub Finalize()
            'Try
            '    Me.str.Close()
            'Catch ex As Exception

            'End Try
            MyBase.Finalize()
        End Sub
    End Class

    Public Sub New()
        rebuildresources()
    End Sub
End Class

