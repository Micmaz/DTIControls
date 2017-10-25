Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.Configuration
Imports System.Configuration

''' <summary>
''' Base class for System.Web.UI.MasterPage class, adding data accessors, error handeling and basic security. 
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class MasterBase
    Inherits MasterPage
#Else
    <ComponentModel.ToolboxItem(False)> _
    Public Class MasterBase
        Inherits MasterPage
#End If
        Private Const classname As String = "BaseClasses.MasterBase"

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

    ''' <summary>
    ''' Init event handeler. Sets the default connection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Init event handeler. Sets the default connection.")> _
        Private Sub BasePageInit_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            If Not Data Is Nothing Then Me.sqlHelper.defaultConnection = Data.sqlHelper.defaultConnection
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

            'parallelDataHelper.executeParallelDBCall()

            If IsPostBack Then RaiseEvent PostBack(sender, e) Else RaiseEvent LoadNoPostBack(sender, e)
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

#End Region


        'Protected Overridable Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
        '    'Catch the exception
        '    If General.Config.Common.Variables("hideErrors") = "Y" Then
        '        Dim ex As Exception = Server.GetLastError()

        '        If Not IsNothing(ex) Then
        '            If Not Session Is Nothing Then _
        '            Session.Add("LastException", ex)

        '            'We must now manually render the page. When an unhandled
        '            'exception occurs, the Page_Render method is not fired. The
        '            'following code renders manually.

        '            Dim sb As New System.Text.StringBuilder
        '            Dim sw As New System.IO.StringWriter(sb)
        '            Dim htmltw As New HtmlTextWriter(sw)
        '            'fire the PreRender event
        '            Me.OnPreRender(New EventArgs)
        '            'Render the actual page controls
        '            htmltw.Write("<script language=""JavaScript""> " & vbCrLf & _
        '            "<!--" & vbCrLf & _
        '            "window.open('" & Request.ApplicationPath & "/Popuperror.aspx','ErrorWindow','height=320,width=320,scrollbars,resizable');  " & _
        '            "// -->" & vbCrLf & _
        '            "</script>")
        '            Me.Render(htmltw)
        '            Response.Write(sb.ToString)
        '            Context.ClearError()

        '        End If
        '    End If

        'End Sub

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

        Return Str

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

                    For index As Integer = 0 To vals.Length - 1
                        vals(index) = vals(index).Trim
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

                    If vals.Length = colNum Then
                        dt.Rows.Add(vals)
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
    Public Sub writeexcel(ByVal dt As DataTable, Optional ByVal colNames() As String = Nothing, Optional ByVal asAttachment As Boolean = True)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        'Response.Charset = Text.Encoding.UTF8.WebName
        'Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/ms-excel; charset=utf-8"
        'Response.ContentType = "application/ms-excel"
        'Response.ContentType = "application/xls"
        If Not asAttachment Then
            Response.AddHeader("content-disposition", "inline; filename=Document.xls")
        Else
            Response.AddHeader("content-disposition", "attachment; filename=Document.xls")
        End If
        Response.Write("<!DOCTYPE html PUBLIC " & Chr(34) & "-//W3C//DTD XHTML 1.0 Transitional//EN" & Chr(34) & " " & Chr(34) & _
       "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" & Chr(34) & ">" & vbCrLf & _
       "<html xmlns=" & Chr(34) & "http://www.w3.org/1999/xhtml" & Chr(34) & ">" & vbCrLf & _
       "<head>" & vbCrLf & _
       "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>" & vbCrLf & _
       "</head><body>")

        Me.EnableViewState = False
        Response.Write("<table><tr>")
        If colNames Is Nothing Then
            For Each col As DataColumn In dt.Columns
                Response.Write("<td>" & col.ColumnName & "</td>")
            Next
            Response.Write("</tr>")
            For Each row As DataRow In dt.Rows
                Response.Write("<tr>")
                For Each Val As Object In row.ItemArray
                    Response.Write("<td>" & Val & "</td>")
                    'Response.Write("<td>" & Val.ToString & "</td>")
                Next
                Response.Write("</tr>")
            Next
        Else
            For i As Integer = 0 To colNames.Length - 1
                Response.Write("<td>" & colNames(i) & "</td>")
            Next
            Response.Write("</tr>")
            For Each row As DataRow In dt.Rows
                Response.Write("<tr>")
                For i As Integer = 0 To colNames.Length - 1
                    Response.Write("<td>" & row.Item(colNames(i)) & "</td>")
                    'Response.Write("<td>" & row.Item(colNames(i)).ToString & "</td>")
                Next
                Response.Write("</tr>")
            Next
        End If
        Response.Write("</table></body></html>")
        Response.End()
    End Sub
#End Region

        Public Sub New()
            If Not BaseVirtualPathProvider.initialized Then
                System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(New BaseVirtualPathProvider())
            End If
        End Sub

    End Class
