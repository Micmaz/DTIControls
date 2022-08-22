Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.Configuration
Imports System.Configuration
Imports System.Net.Configuration

''' <summary>
''' Base class for System.Web.UI.Page class, adding data accessors, error handeling and basic security. 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Base class for System.Web.UI.Page class, adding data accessors, error handeling and basic security.")> _
Public Class BaseSecurityPage
    Inherits System.Web.UI.Page

    Private Const classname As String = "BaseClasses.BaseSecurityPage"
    Private WithEvents myPageTracker As PageTracker

    ''' <summary>
    ''' Fires when page tracking is enabled and a new page is visited
    ''' </summary>
    ''' <param name="trackerRow"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fires when page tracking is enabled and a new page is visited")> _
    Public Event pageTracked(ByVal trackerRow As dsBaseClasses.DTIPageTrackerRow)

    ''' <summary>
    ''' Fires when the load event occures with a postback
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fires when the load event occures with a postback")> _
    Public Event PostBack(ByVal sender As System.Object, ByVal e As System.EventArgs)

    ''' <summary>
    ''' fires after the load event when there is no postback data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("fires after the load event when there is no postback data")> _
    Public Event LoadNoPostBack(ByVal sender As System.Object, ByVal e As System.EventArgs)

    ''' <summary>
    ''' Fires when data from a parallel data helper call has data available to it.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fires when data from a parallel data helper call has data available to it.")> _
    Public Event DataReady()

    Public Sub New()
		BaseVirtualPathProvider.registerVirtualPathProvider()
	End Sub

    ''' <summary>
    ''' Returns the current error handler for this application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the current error handler for this application.")> _
    Public ReadOnly Property errorHandler() As ErrorHandlerBase
        Get
            Return Application.Get("ErrorHandler")
        End Get
    End Property

    ''' <summary>
    ''' Init event handeler. Sets the default connection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Init event handeler. Sets the default connection.")> _
    Private Sub BasePageInit_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'This may seem redudant but keep it to ensure that data has been instantiated in this base class.
        If Not Data Is Nothing Then Me.sqlHelper.defaultConnection = Data.sqlHelper.defaultConnection
    End Sub

    ''' <summary>
    ''' Preload event handeler. Calls the parallel datahelper so data will be available at load.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Preload event handeler. Calls the parallel datahelper so data will be available at load.")> _
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If _parallelDataHelper IsNot Nothing Then _
            parallelDataHelper.executeParallelDBCall()
    End Sub

    ''' <summary>
    ''' Load event handeler. Checks security and begins page tracking.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Load event handeler. Checks security and begins page tracking.")> _
    Private Sub baseSecuityPage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If SecurePage AndAlso Not checkSecurity() Then
            If Not securityFailedPage Is Nothing Then
                Response.Redirect(securityFailedPage)
            Else
                Response.Write("You do not have access to view this page.")
                Response.End()
            End If
        End If
        If AdminPage AndAlso Not checkAdmin() Then
            If Not AdminFailedPage Is Nothing Then
                Response.Redirect(AdminFailedPage)
            Else
                Response.Write("You do not have access to view this page.")
                Response.End()
            End If
        End If

        trackPage()

        If IsPostBack Then RaiseEvent PostBack(sender, e) Else RaiseEvent LoadNoPostBack(sender, e)
    End Sub

    ''' <summary>
    ''' Prerenter event handeler. evecutes the parallel data helper so prerender and render should have all data availalbe.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Prerenter event handeler. evecutes the parallel data helper so prerender and render should have all data availalbe.")> _
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If _parallelDataHelper IsNot Nothing Then _
            parallelDataHelper.executeParallelDBCall()
        'Response.Filter = New IO.Compression.GZipStream(Response.Filter, IO.Compression.CompressionMode.Compress)
        'Response.AddHeader("Content-Encoding", "gzip")
    End Sub

#Region "Data Accessors"
    Private _currBaseClassType As Type
    Private ReadOnly Property currBaseClassType() As Type
        Get
            If _currBaseClassType Is Nothing Then
                _currBaseClassType = Me.GetType
                Dim BaseSecurityPageType As Type = Type.GetType(classname)
                While Not _currBaseClassType.BaseType Is BaseSecurityPageType
                    _currBaseClassType = _currBaseClassType.BaseType
                End While
            End If
            Return _currBaseClassType
        End Get
    End Property

    Private _currBaseDataType As Type
    Private ReadOnly Property currBaseDataType() As Type
        Get
            If _currBaseDataType Is Nothing Then
                _currBaseDataType = CType(currBaseClassType.GetMember("Data")(0), System.Reflection.PropertyInfo).PropertyType
            End If
            Return _currBaseDataType
        End Get
    End Property

    ''' <summary>
    ''' Returns a subclass of Database. The loacl assembly is searched, otherwise a cached version of Database is returned. Database is cached in the session so it will be user-isolated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns a subclass of Database. The loacl assembly is searched, otherwise a cached version of Database is returned. Database is cached in the session so it will be user-isolated.")> _
    Public ReadOnly Property Data() As BaseClasses.DataBase
        Get
            If DesignMode Then
                Return Nothing
            End If
            If Session("DataObj." & currBaseDataType.FullName) Is Nothing Then
                Dim methinfo As System.Reflection.PropertyInfo = CType(currBaseClassType.GetMember("Data")(0), System.Reflection.PropertyInfo)
                Dim dataType As Type = methinfo.PropertyType
                Dim data1 As DataBase
                data1 = dataType.Assembly.CreateInstance(dataType.FullName, False, Reflection.BindingFlags.ExactBinding, Nothing, Nothing, Nothing, Nothing)
                data1.session = Session
                If Application.Get("ErrorHandler") Is Nothing Then
                    methinfo = CType(currBaseClassType.GetMember("errorHandler")(0), System.Reflection.PropertyInfo)
                    dataType = methinfo.PropertyType
                    Dim perrorHandler As ErrorHandlerBase = dataType.Assembly.CreateInstance(dataType.FullName, False, Reflection.BindingFlags.ExactBinding, Nothing, Nothing, Nothing, Nothing)
                    perrorHandler.application = Me.Context.ApplicationInstance
                    Application.Add("ErrorHandler", perrorHandler)
                    'addhandler Application.e
                End If
                Session("DataObj." & currBaseDataType.FullName) = data1
            End If
            Return Session("DataObj." & currBaseDataType.FullName)
        End Get

    End Property

    ''' <summary>
    ''' Convienece method, returns the BaseHelper in the dataObject. This is the default BaseHelper for a web application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convienece method, returns the BaseHelper in the dataObject. This is the default BaseHelper for a web application.")> _
    Public ReadOnly Property sqlHelper() As BaseHelper
        Get
            Return Data.sqlHelper
        End Get
    End Property

    Private WithEvents _parallelDataHelper As ParallelDataHelper

    ''' <summary>
    ''' Convienece method, returns the ParallelDataHelper in the dataObject. This is the default ParallelDataHelper for a web application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convienece method, returns the ParallelDataHelper in the dataObject. This is the default ParallelDataHelper for a web application.")> _
    Public ReadOnly Property parallelDataHelper() As ParallelDataHelper
        Get
            'If _parallelDataHelper Is Nothing Then
            '    If Session("DTIParallelDataHelper") Is Nothing Then
            '        Session("DTIParallelDataHelper") = New ParallelDataHelper
            '    End If
            '    _parallelDataHelper = Session("DTIParallelDataHelper")
            '    _parallelDataHelper.sqlHelper = sqlHelper
            'End If
            'Return _parallelDataHelper
            If _parallelDataHelper Is Nothing Then
                _parallelDataHelper = Data.parallelDataHelper
            End If
            Return _parallelDataHelper
        End Get
    End Property

    Private Sub _parallelDataHelper_DataReady() Handles _parallelDataHelper.DataReady
        RaiseEvent DataReady()
    End Sub

    ''' <summary>
    ''' returns the default connection used in the web application. This is set in the web config as a connection string called either DTIConnection or ConnectionString.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("returns the default connection used in the web application. This is set in the web config as a connection string called either DTIConnection or ConnectionString.")> _
    Public Property defaultConnection() As System.Data.Common.DbConnection
        Get
            Return Data.defaultConnection()
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            Data.defaultConnection = Value
        End Set
    End Property

    ''' <summary>
    ''' Convience method to <see cref="BaseHelper.FillDataSet">BaseHelper.FillDataSet</see>
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="tableName"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convience method to <see cref=""BaseHelper.FillDataSet"">BaseHelper.FillDataSet</see>")> _
    Public Sub FillDataSet(ByVal command As String, ByVal tableName As String, ByVal ds As DataSet)
        sqlHelper.FillDataSet(command, ds, tableName)
    End Sub

    ''' <summary>
    ''' Convience method to <see cref="BaseHelper.FillDataTable">BaseHelper.FillDataTable</see>
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="table"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convience method to <see cref=""BaseHelper.FillDataTable"">BaseHelper.FillDataTable</see>")> _
    Public Sub FillDataTable(ByVal command As String, ByVal table As DataTable)
        sqlHelper.FillDataTable(command, table)
    End Sub

    ''' <summary>
    ''' Returns the object or nothing if it is dbnull.value
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the object or nothing if it is dbnull.value")> _
    Public Shared Function getValue(ByVal o As Object) As Object
        If o Is DBNull.Value Then
            Return Nothing
        Else
            Return o
        End If
    End Function

    ''' <summary>
    ''' Returns the object or "" if it is null or dbnull.value 
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the object or """" if it is null or dbnull.value")> _
    Public Shared Function getString(ByVal o As Object) As Object
        If o Is DBNull.Value OrElse o Is Nothing Then
            Return ""
        Else
            Return o
        End If
    End Function

#End Region

#Region "Security"

    ''' <summary>
    ''' If this is set to true then the page will not render past load unless the Username value has been set.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If this is set to true then the page will not render past load unless the Username value has been set.")> _
    Public Property SecurePage() As Boolean
        Get
            Return _SecurePage
        End Get
        Set(ByVal Value As Boolean)
            _SecurePage = Value
        End Set
    End Property
    Private _SecurePage As Boolean = False

	
	''' <summary>
    ''' If this is set to true then the page will not render past load unless the checkAdmin function returns true.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If this is set to true then the page will not render past load unless the checkAdmin function returns true.")> _
    Public Property AdminPage() As Boolean
        Get
            Return _AdminPage
        End Get
        Set(ByVal Value As Boolean)
            _AdminPage = Value
        End Set
    End Property
    Private _AdminPage As Boolean = False

    ''' <summary>
    ''' The current users name. Must be set programatically.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The current users name. Must be set programatically.")> _
    Public Property UserName() As String
        Get
            Try
                Return Session("username")
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(ByVal Value As String)
            Session("username") = Value
        End Set
    End Property

    ''' <summary>
    ''' If the currentuser is an admin. Must be set programatically.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If the currentuser is an admin. Must be set programatically.")> _
    Public Property isAdmin() As Boolean
		Get
            Try
                Return Session("SetsifCurrentUserisAnAdminorNOt")
            Catch ex As Exception
                Return false
            End Try
        End Get
        Set(ByVal Value As Boolean)
            Session("SetsifCurrentUserisAnAdminorNOt") = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the username from Page.User.Identity.Name
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the username from Page.User.Identity.Name")> _
    Protected Overridable Function getUsername() As String
        Dim sUsername As String = ""
        Dim nUsernameIndex As Integer = InStrRev(Page.User.Identity.Name, "\")
        If nUsernameIndex > 0 Then
            sUsername = Mid(Page.User.Identity.Name, nUsernameIndex + 1)
        End If
        sUsername = sUsername.Trim.ToUpper
        Return sUsername
    End Function

    ''' <summary>
    ''' Returns true if UserName has been set to a non-empty string. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns true if UserName has been set to a non-empty string.")> _
    Protected Overridable Function checkSecurity() As Boolean
        Return Not UserName Is Nothing AndAlso Not UserName.Trim = ""
    End Function

    ''' <summary>
    ''' Returns true if user is an Admin. 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns true if user is an Admin.")> _
    Protected Overridable Function checkAdmin() As Boolean
        Return isAdmin
    End Function

    ''' <summary>
    ''' This is the url of the page the request is forwarded to if the security check fails.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This is the url of the page the request is forwarded to if the security check fails.")> _
    Public Property securityFailedPage() As String
        Get
            Return _securityFailedPage
        End Get
        Set(ByVal Value As String)
            _securityFailedPage = Value
        End Set
    End Property
    Private _securityFailedPage As String

    ''' <summary>
    ''' This is the url of the page the request is forwarded to if the admin check fails.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This is the url of the page the request is forwarded to if the admin check fails.")> _
    Public Property AdminFailedPage() As String
        Get
            Return _adminFailedPage
        End Get
        Set(ByVal Value As String)
            _adminFailedPage = Value
        End Set
    End Property
    Private _adminFailedPage As String

#End Region

#Region "Error handeler"

    Private _erroremail As String = Nothing

    ''' <summary>
    ''' The email address error reports are sent to. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The email address error reports are sent to.")> _
    Public Property erroremail() As String
        Get
            If _erroremail Is Nothing Then
                Return WebConfigurationManager.AppSettings("ErrorEmail")
            Else : Return _erroremail
            End If
        End Get
        Set(ByVal value As String)
            _erroremail = value
        End Set
    End Property

    ''' <summary>
    ''' Handles an error on the current page. This is overidable.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Handles an error on the current page. This is overidable.")> _
    Protected Overridable Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        'Catch the exception
        Try

            Dim ex As Exception = Server.GetLastError()
            Dim errorto As String = WebConfigurationManager.AppSettings("ErrorEmail")
            Dim appname As String = "Myapp"
            Try
                'ugly. just gets the application directory. should probably get the assembly name.
                appname = Request.PhysicalApplicationPath.Substring(Request.PhysicalApplicationPath.Substring(0, Request.PhysicalApplicationPath.Length - 1).LastIndexOf("\")).Replace("\", "")
            Catch ex1 As Exception
            End Try
            If appname.Length = 0 Then
                appname = "Myapp"
            End If

            Dim body As String = "There was an error from request:" & Request.RawUrl & "<br>" & ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
            Dim Subject As String = "Error in: " & appname & " : " & ex.Message
            If Not ex.InnerException Is Nothing Then
                ex = ex.InnerException
                body &= ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
            End If
            If Not ex.InnerException Is Nothing Then
                ex = ex.InnerException
                body &= ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
            End If
            'System.Web.Mail.SmtpMail.Send("error@" & appname & ".com", errorto, Subject, body)


            'logEvent(body)
            If Not WebConfigurationManager.AppSettings("ErrorEmail") Is Nothing Then
                Dim configurationFile As Configuration = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath & "web.config")

                Dim mailSettings As MailSettingsSectionGroup = configurationFile.GetSectionGroup("system.net/mailSettings")

                Dim host As String = ""
                Dim port As Integer = 25
                Dim password As String = ""
                Dim smtpusername As String = ""
                Dim from As String = ""

                Dim smtpcli As New System.Net.Mail.SmtpClient
                If Not mailSettings Is Nothing Then
                    port = mailSettings.Smtp.Network.Port
                    password = mailSettings.Smtp.Network.Password
                    smtpusername = mailSettings.Smtp.Network.UserName
                    host = mailSettings.Smtp.Network.Host

                    smtpcli = New System.Net.Mail.SmtpClient(host, port)
                    Dim creds As New System.Net.NetworkCredential(smtpusername, password)
                    smtpcli.Credentials = creds

                    If Not WebConfigurationManager.AppSettings("ErrorFromEmail") Is Nothing Then
                        from = WebConfigurationManager.AppSettings("ErrorFromEmail")
                    Else
                        from = "error@" & appname & ".com"
                    End If
                End If

                smtpcli.Send(from, errorto, Subject, body)

            End If

        Catch ex2 As Exception
        End Try
        'If WebConfigurationManager.AppSettings("hideErrors") = "Y" Then
        '    Dim ex As Exception = Server.GetLastError()

        '    If Not IsNothing(ex) Then
        '        If Not Session Is Nothing Then _
        '        Session.Add("LastException", ex)

        '        'We must now manually render the page. When an unhandled
        '        'exception occurs, the Page_Render method is not fired. The
        '        'following code renders manually.

        '        Dim sb As New System.Text.StringBuilder
        '        Dim sw As New System.IO.StringWriter(sb)
        '        Dim htmltw As New HtmlTextWriter(sw)
        '        'fire the PreRender event
        '        Me.OnPreRender(New EventArgs)
        '        'Render the actual page controls
        '        Dim body As String = ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
        '        htmltw.Write("<!--" & vbCrLf & _
        '        body & _
        '        "// -->" & vbCrLf)
        '        'htmltw.Write("<script language=""JavaScript""> " & vbCrLf & _
        '        '"<!--" & vbCrLf & _
        '        '"window.open('" & Request.ApplicationPath & "/Popuperror.aspx','ErrorWindow','height=320,width=320,scrollbars,resizable');  " & _
        '        '"// -->" & vbCrLf & _
        '        '"</script>")
        '        Me.Render(htmltw)
        '        Response.Write(sb.ToString)
        '        Context.ClearError()
        '    End If
        'End If

    End Sub

    ''' <summary>
    ''' Adds an entry into the event log. By default the event group Web Event is added if permissions allow.
    ''' </summary>
    ''' <param name="anevent"></param>
    ''' <param name="sourcename"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds an entry into the event log. By default the event group Web Event is added if permissions allow.")> _
    Public Sub logEvent(ByVal anevent As String, Optional ByVal sourcename As String = Nothing)
        Dim logname As String = "Web Event"
        If sourcename Is Nothing Then
            Dim appname As String = "Myapp"
            Try
                'ugly. just gets the application directory. should probably get the assembly name.
                appname = Request.PhysicalApplicationPath.Substring(Request.PhysicalApplicationPath.Substring(0, Request.PhysicalApplicationPath.Length - 1).LastIndexOf("\")).Replace("\", "")
            Catch ex1 As Exception
            End Try
            If appname.Length = 0 Then
                appname = "Myapp"
            End If
            sourcename = appname
        End If

        Dim evt As New System.Diagnostics.EventSourceCreationData(sourcename, logname)

        If Not EventLog.SourceExists(sourcename, ".") Then
            EventLog.CreateEventSource(evt)
        End If

        Dim ELog As New EventLog(logname, ".", sourcename)
        ELog.WriteEntry(anevent, EventLogEntryType.Error, 234, CType(3, Short))
    End Sub

#End Region

#Region "helpers"

    ''' <summary>
    ''' Returns true if a column is null or dbnull.value
    ''' </summary>
    ''' <param name="row"></param>
    ''' <param name="colname"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns true if a column is null or dbnull.value")> _
    Public Shared Function isNull(ByVal row As DataRow, ByVal colname As String) As Boolean
        If row(colname) Is DBNull.Value OrElse row(colname).ToString.Length = 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    '''  Performs JavaScript encoding on given string
    ''' </summary>
    ''' <param name="Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Performs JavaScript encoding on given string")> _
    Public Shared Function JavaScriptEncode(ByVal Str As String) As String

        Str = Replace(Str, "\", "\\")
        Str = Replace(Str, "'", "\'")
        Str = Replace(Str, """", "\""")
        Str = Replace(Str, Chr(8), "\b")
        Str = Replace(Str, Chr(9), "\t")
        Str = Replace(Str, Chr(10), "\r")
        Str = Replace(Str, Chr(12), "\f")
        Str = Replace(Str, Chr(13), "\n")

        Return Str + ""

    End Function

    ''' <summary>
    ''' Unencodes JavaScript characters from given string
    ''' </summary>
    ''' <param name="Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Unencodes JavaScript characters from given string")> _
    Public Shared Function JavaScriptUnencode(ByVal Str As String) As String

        Str = Replace(Str, "\\", "\")
        Str = Replace(Str, "\'", "'")
        Str = Replace(Str, "\""", """")
        Str = Replace(Str, "\b", Chr(8))
        Str = Replace(Str, "\t", Chr(9))
        Str = Replace(Str, "\r", Chr(10))
        Str = Replace(Str, "\f", Chr(12))
        Str = Replace(Str, "\n", Chr(13))

        Return Str

    End Function

    ''' <summary>
    ''' Ensures a string is compliant with cookie name requirements
    ''' </summary>
    ''' <param name="Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Ensures a string is compliant with cookie name requirements")> _
    Public Shared Function ValidCookieName(ByVal Str As String) As String

        Str = Replace(Str, "=", "")
        Str = Replace(Str, ";", "")
        Str = Replace(Str, ",", "")
        Str = Replace(Str, Chr(9), "")
        Str = Replace(Str, Chr(10), "")
        Str = Replace(Str, Chr(13), "")

        Return Str

    End Function

    ''' <summary>
    ''' Returns the query string and excludes some of the parameters if needed.
    ''' </summary>
    ''' <param name="excludeKeys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the query string and excludes some of the parameters if needed.")> _
    Public Function getQueryString(Optional ByVal excludeKeys() As String = Nothing) As String
        Dim query As String = "?"
        If excludeKeys Is Nothing Then
            excludeKeys = New String() {}
        End If
        Dim keycol As New Hashtable
        For Each key As String In excludeKeys
            keycol.Add(key, key)
        Next
        For Each str As String In Request.QueryString.Keys
            If Not keycol.ContainsKey(str) Then _
                query &= str & "=" & Request.QueryString.Item(str) & "&"
        Next
        query = query.Substring(0, query.Length - 1)
        Return query
    End Function

    ''' <summary>
    ''' Returns the full path of the current url. ex: /test/some/location.aspx
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the full path of the current url. ex: /test/some/location.aspx")> _
    Public Function GetUrlPath() As String
        Dim url As String = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.IndexOf("//") + 2)
        Return url.Substring(0, url.IndexOf("/")) & Request.ApplicationPath
        'Return strLocation & "tmp\"

    End Function

    ''' <summary>
    ''' Ensures a string is compliant with cookie value requirements
    ''' </summary>
    ''' <param name="Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Ensures a string is compliant with cookie value requirements")> _
    Public Shared Function ValidCookieValue(ByVal Str As String) As String

        Str = Replace(Str, ";", "")
        Str = Replace(Str, ",", "")

        Return Str
    End Function

    ''' <summary>
    ''' Creates a datatable from a csv file posted to a web form.
    ''' </summary>
    ''' <param name="myFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a datatable from a csv file posted to a web form.")> _
    Public Shared Function makeTableFromCSV(ByVal myFile As Web.HttpPostedFile) As DataTable
        Dim dt As DataTable = New DataTable
        If (Not (myFile Is Nothing)) AndAlso (myFile.ContentLength > 0) Then
            Try

                Dim lBytes As Long = myFile.InputStream.Length
                Dim fileData(lBytes) As Byte
                myFile.InputStream.Read(fileData, 0, lBytes)

                Dim csvString As String
                Dim enc As New System.Text.ASCIIEncoding
                csvString = enc.GetString(fileData)

                Dim splitString() As String = csvString.Split(vbCrLf)

                Dim vals() As String = splitString(0).Split(Char.Parse(","))
                Dim colNum As Integer = vals.Length

                For index As Integer = 0 To colNum - 1
                    vals(index) = vals(index).Replace(vbCr, "")
                    vals(index) = vals(index).Replace(vbCrLf, "")
                    vals(index) = vals(index).Replace(vbFormFeed, "")
                    vals(index) = vals(index).Replace(vbLf, "")
                    vals(index) = vals(index).Replace(vbNewLine, "")
                    vals(index) = vals(index).Replace(vbTab, "")
                    If vals(index).StartsWith("""") AndAlso vals(index).EndsWith("""") Then
                        vals(index) = vals(index).Substring(1)
                        vals(index) = vals(index).Substring(0, vals(index).Length - 1)
                    End If
                Next index

                For Each col As String In vals
                    dt.Columns.Add(col)
                Next

                For i As Integer = 1 To splitString.Length - 1
                    vals = splitString(i).Split(Char.Parse(","))

                    Dim list As New Collections.Generic.List(Of String)

                    For index As Integer = 0 To vals.Length - 1
                        vals(index) = vals(index).Trim
                        vals(index) = vals(index).Replace(vbCr, "")
                        vals(index) = vals(index).Replace(vbCrLf, "")
                        vals(index) = vals(index).Replace(vbFormFeed, "")
                        vals(index) = vals(index).Replace(vbLf, "")
                        vals(index) = vals(index).Replace(vbNewLine, "")
                        vals(index) = vals(index).Replace(vbTab, "")
                        If vals(index).StartsWith("""") AndAlso Not vals(index).EndsWith("""") Then
                            Dim str As String = vals(index)
                            Do
                                index += 1
                                str &= ", " & vals(index)
                            Loop While (Not vals(index).EndsWith(""""))
                            vals(index) = str
                        End If
                        vals(index) = vals(index).Trim("""")
                        vals(index) = vals(index).Replace("""""", """")
                        list.Add(vals(index))
                    Next index

                    If list.Count = colNum Then
                        dt.Rows.Add(list.ToArray)
                    End If
                Next

            Catch ex As Exception

            End Try

        End If

        Return dt
    End Function

    ''' <summary>
    ''' Converts a datatable to csv format and writes csv data out to the responce stream.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="colNames"></param>
    ''' <param name="asAttachment"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Converts a datatable to csv format and writes csv data out to the responce stream.")> _
    Public Sub writeCSV(ByVal dt As DataTable, Optional ByVal colNames() As String = Nothing, Optional ByVal asAttachment As Boolean = True)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = ""

        If Not asAttachment Then
            Response.AddHeader("content-disposition", "inline; filename=Document.csv")
        Else
            Response.AddHeader("content-disposition", "attachment; filename=Document.csv")
        End If

        Response.Charset = ""
        Me.EnableViewState = False

        If colNames Is Nothing Then
            For Each col As DataColumn In dt.Columns
                Response.Write(col.ColumnName & ",")
            Next
            Response.Write(vbCrLf)
            For Each row As DataRow In dt.Rows
                For Each Val As Object In row.ItemArray
                    'Response.Write(Val & ",")
                    Response.Write("""" & Val.ToString.Replace("""", """""") & """,")
                Next
                Response.Write(vbCrLf)
            Next
        Else
            For i As Integer = 0 To colNames.Length - 1
                Response.Write(colNames(i) & ",")
            Next
            Response.Write(vbCrLf)
            For Each row As DataRow In dt.Rows
                For i As Integer = 0 To colNames.Length - 1
                    'Response.Write(row.Item(colNames(i)) & ",")
                    Response.Write("""" & row.Item(colNames(i)).ToString.Replace("""", """""") & """,")
                Next
                Response.Write(vbCrLf)
            Next
        End If
        Response.End()
    End Sub

	''' <summary>
	''' Converts a datatable to excel format and writes csv data out to the responce stream.
	''' </summary>
	''' <param name="dt"></param>
	''' <param name="colNames"></param>
	''' <param name="asAttachment"></param>
	''' <remarks></remarks>
<System.ComponentModel.Description("Converts a datatable to excel format and writes csv data out to the responce stream.")> _
	Public Sub writeexcel(ByVal dt As DataTable, Optional ByVal colNames() As String = Nothing, Optional ByVal asAttachment As Boolean = True, Optional ByVal filename As String = "Document.xls")
		Me.EnableViewState = False
		excelExport(dt, colNames, asAttachment, filename)
	End Sub

	''' <summary>
	''' Converts a datatable to excel format and writes csv data out to the responce stream.
	''' </summary>
	''' <param name="dt"></param>
	''' <param name="colNames"></param>
	''' <param name="asAttachment"></param>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Converts a datatable to excel format and writes csv data out to the responce stream.")>
	Public Shared Sub excelExport(ByVal dt As DataTable, Optional ByVal colNames() As String = Nothing, Optional ByVal asAttachment As Boolean = True, Optional ByVal filename As String = "Document.xls")
		Dim response As Web.HttpResponse = Web.HttpContext.Current.Response
		response.Clear()
		response.ClearHeaders()
		response.Buffer = True
		'Response.Charset = Text.Encoding.UTF8.WebName
		'Response.ContentType = "application/vnd.ms-excel"
		'Response.ContentType = "application/ms-excel; charset=utf-8"
		'Response.ContentType = "application/ms-excel"
		'Response.ContentType = "application/xls"
		If Not asAttachment Then
			response.AddHeader("content-disposition", "inline; filename=" & filename)
		Else
			response.AddHeader("content-disposition", "attachment; filename=" & filename)
		End If
		response.Write("<!DOCTYPE html PUBLIC " & Chr(34) & "-//W3C//DTD XHTML 1.0 Transitional//EN" & Chr(34) & " " & Chr(34) &
	   "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" & Chr(34) & ">" & vbCrLf &
	   "<html xmlns=" & Chr(34) & "http://www.w3.org/1999/xhtml" & Chr(34) & ">" & vbCrLf &
	   "<head>" & vbCrLf &
	   "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>" & vbCrLf &
	   "</head><body>")

		'Me.EnableViewState = False
		response.Write("<table><tr>")
		If colNames Is Nothing Then
			For Each col As DataColumn In dt.Columns
				response.Write("<td>" & col.ColumnName & "</td>")
			Next
			response.Write("</tr>")
			For Each row As DataRow In dt.Rows
				response.Write("<tr>")
				For Each Val As Object In row.ItemArray
					response.Write("<td>" & Val.ToString() & "</td>")
					'Response.Write("<td>" & Val.ToString & "</td>")
				Next
				response.Write("</tr>")
			Next
		Else
			For i As Integer = 0 To colNames.Length - 1
				response.Write("<td>" & colNames(i) & "</td>")
			Next
			response.Write("</tr>")
			For Each row As DataRow In dt.Rows
				response.Write("<tr>")
				For i As Integer = 0 To colNames.Length - 1
					response.Write("<td>" & row.Item(colNames(i)).ToString() & "</td>")
					'Response.Write("<td>" & row.Item(colNames(i)).ToString & "</td>")
				Next
				response.Write("</tr>")
			Next
		End If
		response.Write("</table></body></html>")
		response.End()
	End Sub



	Public Sub mailhandler(ByVal body As String, ByVal toaddress As String, ByVal fromaddress As String, ByVal subject As String, _
                        Optional ByVal ishtml As Boolean = True, Optional ByVal enableSSl As DataBase.enableSSlMail = DataBase.enableSSlMail.Auto, _
                        Optional ByVal attachment As Net.Mail.Attachment = Nothing, Optional ByVal StrAttachment As String = Nothing, Optional ByVal StrAttachmentName As String = "")
		data.mailhandler(body ,  toaddress ,  fromaddress ,  subject ,ishtml ,  enableSSl ,  attachment ,  StrAttachment ,  StrAttachmentName)
    End Sub

    Public Shared Function RemoveSpecialCharacters(ByVal input As String, Optional ByVal excludeCharsRegex As String = Nothing) As String
        Return Text.RegularExpressions.Regex.Replace(input, "[^A-Za-z0-9" & excludeCharsRegex & "]", String.Empty)
    End Function
#End Region

#Region "Encryption"

    ''' <summary>
    ''' Encrypts and urlEncodes text string using a specific key.
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Encrypts and urlEncodes text string using a specific key.")> _
    Public Shared Function EncryptText(ByVal strText As String, ByVal key As String) As String
        'Return EncryptionHelper.Encrypt(strText, WebConfigurationManager.AppSettings("EncryptionKey"))
        Return System.Web.HttpUtility.UrlEncode(EncryptionHelper.Encrypt(strText, key), System.Text.Encoding.Default)
    End Function

    ''' <summary>
    ''' Decrypts and urlDecodes text string using a specific key.
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Decrypts and urlDecodes text string using a specific key.")> _
    Public Shared Function DecryptText(ByVal strText As String, ByVal key As String) As String
        'Return EncryptionHelper.Decrypt(strText, WebConfigurationManager.AppSettings("EncryptionKey"))
        Return System.Web.HttpUtility.UrlDecode(EncryptionHelper.Decrypt(strText, key))
    End Function
#End Region

#Region "Page Tracking"

    ''' <summary>
    ''' Referring url within this domain.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Will return Nothing on first page.  Must be set in code for all pages to track</remarks>
    <System.ComponentModel.Description("Referring url within this domain.")> _
    Public Property UrlReferrer As String
        Get
            Return Session("UrlReferrerObject2")
        End Get
        Set(value As String)
            If Session("UrlReferrerObject") <> value Then
                Session("UrlReferrerObject2") = Session("UrlReferrerObject")
                Session("UrlReferrerObject") = value
            End If
        End Set
    End Property

    Private _enablePageTracking As Boolean = False

    ''' <summary>
    ''' Enable tracking on this page. Default is false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Enable tracking on this page. Default is false.")> _
    Public Property EnablePageTracking() As Boolean
        Get
            Return _enablePageTracking
        End Get
        Set(ByVal value As Boolean)
            _enablePageTracking = value
        End Set
    End Property

    Private Sub trackPage()
        If EnablePageTracking AndAlso Not IsPostBack Then
            myPageTracker = New PageTracker
            myPageTracker.trackPage(Request, Session.SessionID, sqlHelper.defaultConnection)
        End If
    End Sub

    Private Sub myPageTracker_PageTracked(ByVal trackerRow As dsBaseClasses.DTIPageTrackerRow) Handles myPageTracker.PageTracked
        RaiseEvent pageTracked(trackerRow)
    End Sub

#End Region
End Class
