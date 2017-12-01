Imports System.Web
Imports System.Web.SessionState
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Web.Configuration.WebConfigurationManager


''' <summary>
''' Data accessor object. This class is subclassed and used to store data access and application-wide functions. 
''' It is cached in the session and exists on a per-session bassis.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Data accessor object. This class is subclassed and used to store data access and application-wide functions.  It is cached in the session and exists on a per-session bassis."),ComponentModel.ToolboxItem(False)> _
Public Class DataBase
    Inherits System.ComponentModel.Component

    ''' <summary>
    ''' Raised when the default connection is changed.
    ''' </summary>
    ''' <remarks></remarks>
    Event defaultConnectionChanged()

    Private _session As HttpSessionState
	Private shared _sharedsession As HttpSessionState
    Public Shared ReadOnly defaultSqliteString As String = "Data Source=/Database/DTIContent.db3;Pooling=true;FailIfMissing=false;"

    ''' <summary>
    ''' The http session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The http session.")> _
    Public Shadows Property session() As HttpSessionState
        Get
            If httpSession Is Nothing Then
                If _session Is Nothing Then _session = createSession()
                Return _session
            End If
            Return httpSession
        End Get
        Set(ByVal Value As HttpSessionState)
        End Set
    End Property

    ''' <summary>
    ''' The http request.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The http request.")> _
    Public ReadOnly Property request() As HttpRequest
        Get
            If Not HttpContext.Current Is Nothing Then
                Return HttpContext.Current.Request
            Else : Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' The http request.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The http request.")> _
    Public ReadOnly Property response() As HttpResponse
        Get
            If Not HttpContext.Current Is Nothing Then
                Return HttpContext.Current.Response
            Else : Return Nothing
            End If
        End Get
    End Property


    ''' <summary>
    ''' The http session, either created or cached in a shared space.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The http session.")> _
    protected shared Property SharedSession() As HttpSessionState
        Get
            If httpSession Is Nothing Then
                If _sharedsession Is Nothing Then _sharedsession = createSession()
                Return _sharedsession
            End If
            Return httpSession
        End Get
        Set(ByVal Value As HttpSessionState)
        End Set
    End Property

	
	
    ''' <summary>
    ''' A helper method to create a dummy session for winforms applications and design-time use.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A helper method to create a dummy session for winforms applications and design-time use.")> _
    Public Shared Function createSession() As HttpSessionState
        If Not httpSession Is Nothing Then Return httpSession
        Dim container As New HttpSessionStateContainer(Guid.NewGuid.ToString("N"), New SessionStateItemCollection(), New HttpStaticObjectsCollection(), 5, True, HttpCookieMode.AutoDetect, SessionStateMode.InProc, False)
        Dim state As HttpSessionState = Activator.CreateInstance(GetType(HttpSessionState), _
            BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.CreateInstance, _
            Nothing, _
            New Object() {container}, _
            System.Globalization.CultureInfo.CurrentCulture _
        )
        Return state
    End Function

	''' <summary>
	''' The http session.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("The http session.")>
	Public Shared ReadOnly Property httpSession() As System.Web.SessionState.HttpSessionState
		Get
			If Not HttpContext.Current Is Nothing Then
				Return HttpContext.Current.Session
			Else : Return Nothing
			End If
		End Get
	End Property

	Dim _username As String

    ''' <summary>
    ''' The current user 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The current user")> _
    Public Property username() As String
        Get
            'Try
            '    If _username Is Nothing Then
            '        _username = ""
            '    End If
            '    Return _username
            'Catch
            '    Return ""
            'End Try
            Try
                If session("username") Is Nothing Then
                    session("username") = ""
                End If
                Return session("username")
            Catch
                Return ""
            End Try
        End Get
        Set(ByVal Value As String)
            Try

                session("username") = Value
            Catch
            End Try
            'Try

            '    _username = Value
            'Catch
            'End Try
        End Set
    End Property

	Public Shared Function endResponse() As Boolean
		Try
			Web.HttpContext.Current.Response.Flush() ' Sends all currently buffered output To the client.
			Web.HttpContext.Current.Response.SuppressContent = True   ' Gets Or sets a value indicating whether To send HTTP content To the client.
			Web.HttpContext.Current.ApplicationInstance.CompleteRequest() ' Causes ASP.NET To bypass all events And filtering In the HTTP pipeline chain Of execution And directly execute the EndRequest Event.
			Web.HttpContext.Current.Response.Close()
		Catch ex As Exception
			Return False
		End Try
		Return True
	End Function


#Region " Generic Methods"

	Public Shared Function GetWebAppPath() As String

        Dim strLocation As String = System.Reflection.Assembly.GetExecutingAssembly.CodeBase

        ' Remove "file" prefix for web page locations
        If Left(strLocation, 8) = "file:///" Then strLocation = Mid(strLocation, 9)
        strLocation = System.IO.Path.GetDirectoryName(strLocation)
        strLocation = Replace(strLocation, "/", "\")
        ' Make sure config file ends up in source directory
        If Right(strLocation, 4) = "\bin" Then strLocation = Left(strLocation, Len(strLocation) - 3)

        ' Make this a file path instead of a URL path
        Return strLocation

    End Function

    Public Shared Function parseID(ByVal input As String) As Integer
        Try
            Return Integer.Parse(input)
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Shared Sub setCssClasstoId(ByVal c As Object)
        Dim cssClass As String = AssemblyLoader.runM(c, "CssClass")
        If cssClass Is Nothing Then cssClass = ""
        Dim id As String = AssemblyLoader.runM(c, "ID")
		If Not id Is Nothing AndAlso Not cssClass.Contains(id) Then
			If cssClass = "" Then
				AssemblyLoader.runM(c, "CssClass", id)
			Else
				AssemblyLoader.runM(c, "CssClass", cssClass & " " & id)
			End If
		End If
		For Each c1 As Object In AssemblyLoader.runM(c, "Controls")
            setCssClasstoId(c1)
        Next
    End Sub

#End Region

#Region "Data Methods"

	'This assembly hash was put in place to prevent the Reflection.Assembly.LoadFrom call
	' from being called on every new session. 
	'Private Shared asmhashtable As Hashtable
	'This is where the helper object for any component, master component or controll is set
	Private Shared Function createHelper(ByVal ProviderName As String, Optional ByVal isretry As Boolean = False) As BaseHelper

		ProviderName = ProviderName.ToLower.Replace("system.data.", "")
		ProviderName = ProviderName.Replace("client", "helper").Trim
		If ProviderName.ToLower = "sqlhelper" OrElse ProviderName = "" Then
			Return New SQLHelper
		End If
		Dim helper As BaseHelper = Nothing
		'Try

		'BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
		Dim helpertype As Type = Nothing
		For Each t As Type In [Assembly].GetExecutingAssembly().GetTypes
			If t.Name = "BaseHelper" Then
				helpertype = t
				Exit For
			End If
		Next
		helper = AssemblyLoader.CreateInstance(ProviderName & "." & ProviderName, , helpertype)
		'if helper is nothing then
		'helper = AssemblyLoader.CreateInstance(ProviderName, , helpertype)
		'end if

		'If asmhashtable Is Nothing Then asmhashtable = New Hashtable
		'Dim asm As Reflection.Assembly
		'If Not asmhashtable.ContainsKey(ProviderName) Then
		'    asm = Assembly.Load(ProviderName & ", Version=0.0.0.0, PublicKeyToken=null,Culture=neutral")
		'    If Not asm Is Nothing Then asmhashtable.Add(ProviderName, asm)
		'End If
		'asm = asmhashtable(ProviderName)
		'Return asm.CreateInstance(ProviderName & "." & ProviderName, True)
		'Catch ex As Exception
		'    Throw New Exception("Could not initialize helper: " & ProviderName & ex.Message, ex)
		'    Return Nothing
		'End Try

		If helper Is Nothing Then
			If Not isretry Then
				BaseVirtualPathProvider.lastrebuild = Date.Today.AddMinutes(-60)
				BaseVirtualPathProvider.rebuildresources()
				Return createHelper(ProviderName, True)
			End If
			Dim empty As New EmptyHelper()
			empty.errorMessage = "Could not initialize helper: " & ProviderName & vbCrLf & "Please make sure the file: " & ProviderName & ".dll is referenced in your project or copied to your /bin folder."
			Return empty
			Throw New Exception("Could not initialize helper: " & ProviderName & vbCrLf & "Please make sure the file: " & ProviderName & ".dll is referenced in your project or copied to your /bin folder.")
		End If
		Return helper
	End Function

	Public ReadOnly Property sqlHelper() As BaseHelper
        Get
            If session("sqlHelper") Is Nothing Then
                session("sqlHelper") = getHelper()
            End If
            Return session("sqlHelper")
        End Get
    End Property

    Private Shared _defaultConnectionString As System.Configuration.ConnectionStringSettings = Nothing
    Public Shared Property defaultConnectionString() As System.Configuration.ConnectionStringSettings
        Get
            If _defaultConnectionString Is Nothing Then
                If Not ConnectionStrings("DTIConnection") Is Nothing Then
                    _defaultConnectionString = ConnectionStrings("DTIConnection")
                ElseIf Not ConnectionStrings("ConnectionString") Is Nothing Then
                    _defaultConnectionString = ConnectionStrings("ConnectionString")
                Else
                    _defaultConnectionString = New System.Configuration.ConnectionStringSettings("UnspecifiedConnection", defaultSqliteString, "SQLiteHelper")
                End If
            End If
            Return _defaultConnectionString
        End Get
        Set(ByVal value As System.Configuration.ConnectionStringSettings)
            _defaultConnectionString = value
            SharedSession("sqlHelper") = Nothing
            SharedSession("sqlHelper") = getHelper()
        End Set
    End Property

    Public Shared Function defaultSqliteConnection() As System.Data.Common.DbConnection
        Dim tmpHelper As BaseHelper = createHelper("SQLiteHelper", False)
        Return tmpHelper.createConnection(defaultSqliteString)
    End Function

    Public Shared Function createHelper(ByVal connection As System.Data.Common.DbConnection) As BaseHelper
        Dim helper As BaseHelper = createHelper(connection.GetType.Name.ToLower.Replace("connection", "") & "helper")
        helper.defaultConnection = helper.createConnection(connection.ConnectionString)
        Return helper
    End Function

    Public Overridable Function getHelperOverridale() As BaseHelper
        Return getHelper()
    End Function

    Public Shared Function getHelper() As BaseHelper
        If Not SharedSession("sqlHelper") Is Nothing Then Return SharedSession("sqlHelper")
        Dim _sqlhelper As BaseHelper
		SyncLock GetType(BaseHelper)
			'Setting the type
			If Not ConnectionStrings("ConnectionType") Is Nothing _
			 AndAlso Not ConnectionStrings("ConnectionType").ToString.ToLower = "sqlhelper" Then
				Dim classname As String = ConnectionStrings("ConnectionType").ConnectionString
				_sqlhelper = createHelper(classname)
			ElseIf Not defaultConnectionString Is Nothing Then
				_sqlhelper = createHelper(defaultConnectionString.ProviderName)
			Else
				_sqlhelper = New SQLHelper()
			End If
			'Setting the connection string
			If Not defaultConnectionString Is Nothing Then
				Dim constr As String = defaultConnectionString.ConnectionString
				Dim startndx As Integer = constr.IndexOf("Provider")
				If startndx >= 0 Then _
					constr = constr.Substring(0, startndx) & constr.Substring(constr.IndexOf(";", startndx) + 1)
				_sqlhelper.setDefaultConnectionString(constr)
			End If
		End SyncLock
		If TypeOf _sqlhelper Is EmptyHelper Then
			Dim h As EmptyHelper = _sqlhelper
			Throw New Exception(h.errorMessage)
		End If
		Return _sqlhelper
    End Function

    Private _parallelDataHelper As ParallelDataHelper
    Public ReadOnly Property parallelDataHelper() As ParallelDataHelper
        Get
            If _parallelDataHelper Is Nothing Then
                _parallelDataHelper = New ParallelDataHelper
                _parallelDataHelper.sqlHelper = sqlHelper
            End If
            Return _parallelDataHelper
        End Get
    End Property

    Public Overridable Property defaultConnection() As System.Data.Common.DbConnection
        Get
            Return sqlHelper.defaultConnection
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            sqlHelper.defaultConnection.ConnectionString = Value.ConnectionString
            RaiseEvent defaultConnectionChanged()
        End Set
    End Property

    Public Shared Property defaultConnectionAppWide() As System.Data.Common.DbConnection
        Get
            Return getHelper.defaultConnection
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            Dim provider As String = Value.GetType.Name
            provider = provider.ToLower
            provider = provider.Replace("connection", "helper")
            defaultConnectionString = New System.Configuration.ConnectionStringSettings("ConnectionString", Value.ConnectionString, provider)
        End Set
    End Property

    Public Shared Property defaultConnectionSessionWide() As System.Data.Common.DbConnection
        Get
            Return getHelper.defaultConnection
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            Dim provider As String = Value.GetType.Name
            provider = provider.ToLower
            provider = provider.Replace("connection", "helper")
            Dim helper As BaseHelper = createHelper(provider)
            helper.setDefaultConnectionString(Value.ConnectionString)
            SharedSession("sqlHelper") = helper
        End Set
    End Property

#End Region

    Public Sub New()
        Try
            Me.GetType.InvokeMember("InitializeComponent", BindingFlags.InvokeMethod Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public, Nothing, Me, Nothing)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub InitializeComponent()
    End Sub

	Public Enum enableSSlMail
        Auto = 2
        [True] = 1
        [False] = 0
    End Enum

	Public Sub mailhandler(ByVal body As String, ByVal toaddress As String, ByVal fromaddress As String, ByVal subject As String,
						Optional ByVal ishtml As Boolean = True, Optional ByVal enableSSl As enableSSlMail = enableSSlMail.Auto,
						Optional ByVal attachment As Net.Mail.Attachment = Nothing, Optional ByVal StrAttachment As String = Nothing, Optional ByVal StrAttachmentName As String = "", Optional requestRecipt As Boolean = False)




		Dim emailmsg As New Net.Mail.MailMessage()

		With emailmsg
			If Not fromaddress Is Nothing Then _
				.From = New Net.Mail.MailAddress(fromaddress)
			.Subject = subject
			.Body = body
			.IsBodyHtml = ishtml
		End With



		Dim tos() As String = toaddress.Split(New Char() {";"}, System.StringSplitOptions.RemoveEmptyEntries)
		For Each t As String In tos
			emailmsg.To.Add(t)
		Next

		If attachment IsNot Nothing Then
			emailmsg.Attachments.Add(attachment)
		End If

		If StrAttachment IsNot Nothing Then
			Dim ms As New IO.MemoryStream()
			Dim data As Byte() = Text.Encoding.ASCII.GetBytes(StrAttachment)
			ms.Write(data, 0, data.Length)
			If StrAttachmentName Is Nothing Then
				StrAttachmentName = Now.ToFileTimeUtc
			End If
			ms.Position = 0
			Dim attach As New Net.Mail.Attachment(ms, StrAttachmentName)
			emailmsg.Attachments.Add(attach)
		End If

		Dim client As New Net.Mail.SmtpClient
		If requestRecipt Then
			Try
				Dim smtpSec As System.Net.Configuration.SmtpSection = System.Web.Configuration.WebConfigurationManager.GetSection("system.net/mailSettings/smtp")
				fromaddress = smtpSec.Network.UserName
				emailmsg.Headers.Add("Disposition-Notification-To", "<" & fromaddress & ">")
			Catch ex As Exception

			End Try
		End If
		Select Case enableSSl
			Case enableSSlMail.Auto
				client.EnableSsl = client.Port = 465 OrElse client.Port = 587
			Case Else
				client.EnableSsl = Boolean.Parse(CType(enableSSl, Integer))
		End Select
		client.Send(emailmsg)

	End Sub

#Region "shared type helpers"

	Public Shared Function getBaseClassType(ByVal classname As String, ByRef referenceObj As Object) As Type
        Dim _currBaseClassType As Type
        _currBaseClassType = referenceObj.GetType
        Dim BaseSecurityPageType As Type = Type.GetType(classname)
        While Not _currBaseClassType.BaseType Is BaseSecurityPageType
            _currBaseClassType = _currBaseClassType.BaseType
        End While
        Return _currBaseClassType
    End Function

    Public Shared Function getBaseDataType(ByVal classname As String, ByRef referenceObj As Object) As Type
        Dim currBaseClassType As Type = getBaseClassType(classname, referenceObj)
        Return CType(currBaseClassType.GetMember("Data")(0), System.Reflection.PropertyInfo).PropertyType
    End Function

    Public Shared Function getDataObj() As BaseClasses.DataBase
        For Each key As String In httpSession.Keys
            If key.StartsWith("DataObj.") Then
                If GetType(DataBase).IsAssignableFrom(httpSession(key).GetType()) Then
                    Return httpSession(key)
                End If
            End If
        Next
        Return Nothing
    End Function

#End Region

End Class
