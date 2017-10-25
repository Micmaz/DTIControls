Imports System.Text.RegularExpressions
Imports System.Data.Common

''' <summary>
''' A generic data access layer. This is subclassed for different database types. MSSql and SQLLite included in the base dll. 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("A generic data access layer. This is subclassed for different database types. MSSql and SQLLite included in the base dll.")> _
Public MustInherit Class BaseHelper
    Inherits System.ComponentModel.Component

    Private simultaniousConnections As Integer = 0

    ''' <summary>
    ''' This is a template constructor for derived classes. It is inherited by subclasses.
    ''' </summary>
    ''' <param name="connection">The default connection object used for the life of the object.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This is a template constructor for derived classes. It is inherited by subclasses.")> _
    Public Sub New(Optional ByRef connection As DbConnection = Nothing)
        MyBase.New()
        If Platform.isMono Then
            simultaniousConnections = 0
        End If
        If Not connection Is Nothing Then
            defaultConnection = createConnection(connection.ConnectionString)
        End If
    End Sub

    ''' <summary>
    ''' This is a template constructor for derived classes. It is inherited by subclasses.
    ''' </summary>
    ''' <param name="ConnectionString">The default connection String used for the life of the object.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This is a template constructor for derived classes. It is inherited by subclasses.")> _
    Public Sub New(ByVal ConnectionString As String)
        MyBase.New()
        If Not ConnectionString Is Nothing Then defaultConnection = createConnection(ConnectionString)
    End Sub


#Region "Properties"

    ''' <summary>
    ''' Sets the default connection used by any dataHelper object that is not explicitly set. 
    ''' This overrides any setting in the .config file.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the default connection used by any dataHelper object that is not explicitly set.    This overrides any setting in the .config file.")> _
    Public Shared Property defaultConnectionAppWide() As System.Data.Common.DbConnection
        Get
            Return DataBase.defaultConnectionAppWide
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            DataBase.defaultConnectionAppWide = Value
        End Set
    End Property

    ''' <summary>
    ''' The Connection used for all database comunication for the life of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The Connection used for all database comunication for the life of the object.")> _
    Public Overridable Property defaultConnection() As DbConnection
        Get
            Return defaultConnection_p
        End Get
        Set(ByVal Value As DbConnection)
            defaultConnection_p = Value
        End Set
    End Property
    Protected defaultConnection_p As DbConnection

    Public Event constraintError(ByVal ds As DataSet)

    ''' <summary>
    ''' When performing an update should errors be thrown in a constraint excepetion occures. 
    ''' </summary>
    ''' <remarks>Sets enforceconstraints=False if an error is encountered.</remarks>
    <System.ComponentModel.Description("When performing an update should errors be thrown in a constraint excepetion occures.")> _
    Private _enforceConstraints As Boolean = False
    Public Property enforceConstraints() As Boolean
        Get
            Return _enforceConstraints
        End Get
        Set(ByVal value As Boolean)
            _enforceConstraints = value
        End Set
    End Property

    Private _createAdaptorsWithoutPrimaryKeys As Boolean = True

    ''' <summary>
    ''' Should update and delete methods be added to adaptors if there is no primary key information.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Should update and delete methods be added to adaptors if there is no primary key information.")> _
    Public Property createAdaptorsWithoutPrimaryKeys() As Boolean
        Get
            Return _createAdaptorsWithoutPrimaryKeys
        End Get
        Set(ByVal value As Boolean)
            _createAdaptorsWithoutPrimaryKeys = value
        End Set
    End Property

#End Region

#Region "Mustinherit"

    ''' <summary>
    ''' Creates a typed connection from a string.
    ''' </summary>
    ''' <param name="ConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed connection from a string.")> _
    Public MustOverride Function createConnection(ByRef ConnectionString As String) As DbConnection

    ''' <summary>
    ''' Creates a DbDataAdapter from a command string.
    ''' </summary>
    ''' <param name="SQLcommand">Select command used to generate the DbDataAdapter</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns>a DbCommand typed to the base helper type.</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a DbDataAdapter from a command string.")> _
    Public MustOverride Function createAdaptor(Optional ByVal SQLcommand As String = Nothing, Optional ByVal connection As DbConnection = Nothing) As DbDataAdapter

    ''' <summary>
    ''' Creates a DbCommand from a command string.
    ''' </summary>
    ''' <param name="SQLcommand">Select command used to generate the DbCommand</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns>a DbCommand typed to the base helper type.</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a DbCommand from a command string.")> _
    Public MustOverride Function createCommand(Optional ByVal SQLcommand As String = Nothing, Optional ByVal connection As DbConnection = Nothing) As DbCommand

    ''' <summary>
    ''' Creates a typed dbParameter from a name and value
    ''' </summary>
    ''' <param name="name">the parm name.</param>
    ''' <param name="value">the parm value.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed dbParameter from a name and value")> _
    Public MustOverride Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As DbParameter

    ''' <summary>
    ''' Creates a typed parameter from a genric DbParameter
    ''' </summary>
    ''' <param name="parameter">the DbParameter</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed parameter from a genric DbParameter")> _
    Public MustOverride Function createParameter(ByRef parameter As DbParameter) As DbParameter

    ''' <summary>
    ''' Creates a typed DbCommandBuilder
    ''' </summary>
    ''' <param name="adaptor">The typed DbDataAdapter </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed DbCommandBuilder")> _
    Public MustOverride Function createCommandBuilder(ByRef adaptor As DbDataAdapter) As DbCommandBuilder

    ''' <summary>
    ''' Checks if a datatable exists in a database.
    ''' </summary>
    ''' <param name="tablename">The name of the table that may eexist in the database.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Checks if a datatable exists in a database.")> _
    Public MustOverride Function checkDBObjectExists(ByVal tablename As String) As Boolean

    ''' <summary>
    ''' Creates a table in the database based on the schema of the datatable passed in.
    ''' </summary>
    ''' <param name="dt">The datatable that is usedto createthe table in the database. Only schema is used, data is ignored.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a table in the database based on the schema of the datatable passed in.")> _
    Public Function createTable(ByVal dt As DataTable) As Boolean
        Try
            Dim createstr As String = getCreateTableString(dt)
            ExecuteNonQuery(createstr)
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Builds a create script for a table in the database based on the schema of the datatable passed in.
    ''' </summary>
    ''' <param name="dt">The datatable that is usedto build the create String. Only schema is used, data is ignored.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Builds a create script for a table in the database based on the schema of the datatable passed in.")> _
    Public MustOverride Function getCreateTableString(ByVal dt As DataTable) As String

#End Region

#Region "Generic Methods"

    ''' <summary>
    ''' Calls createConnection and using the supplied string and sets a new default connection. Also clears cached adaptors.
    ''' </summary>
    ''' <param name="connectionString">The ConnectionString as a string.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Calls createConnection and using the supplied string and sets a new default connection. Also clears cached adaptors.")> _
    Public Sub setDefaultConnectionString(ByVal connectionString As String)
        adaptorHash = Nothing
        defaultConnection_p = createConnection(connectionString)
    End Sub

    ''' <summary>
    ''' Shared function to return the current stored baseHelper used in a web application.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Shared function to return the current stored baseHelper used in a web application.")> _
    Public Shared Function getHelper() As BaseHelper
        Return DataBase.getHelper()
    End Function

    ''' <summary>
    ''' creates a typed connection from a config vaue.
    ''' </summary>
    ''' <param name="ConfigValueName">The name of the config value</param>
    ''' <returns>a typed DbConnection</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("creates a typed connection from a config vaue.")> _
    Public Function createConnectionFromConfig(ByVal ConfigValueName As String) As DbConnection
        Return Me.createConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings(ConfigValueName).ConnectionString)
    End Function

    ''' <summary>
    ''' Parse an integer from a string value. 
    ''' </summary>
    ''' <param name="input"></param>
    ''' <returns>Return the int value, or -1 if it fails.</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Parse an integer from a string value.")> _
    Public Shared Function parseID(ByVal input As String) As Integer
        Try
            Return Integer.Parse(input)
        Catch ex As Exception
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' Generates complete sql statement from a base statement and a collection additional strings. Statements are and-ed together.
    ''' </summary>
    ''' <param name="selectStmt">Sql Command Text</param>
    ''' <param name="AdditionalStmts">A collection of strings</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Generates complete sql statement from a base statement and a collection additional strings. Statements are and-ed together.")> _
    Public Function getSQLStatement(ByVal selectStmt As String, ByVal AdditionalStmts As Collection) As String
        If Not AdditionalStmts Is Nothing AndAlso AdditionalStmts.Count > 0 Then
            selectStmt = selectStmt.Trim
            If selectStmt.ToLower.IndexOf("where") = -1 Then
                selectStmt &= " where "
            Else
                selectStmt &= " AND"
            End If
            For Each stmt As String In AdditionalStmts
                If Not stmt.Trim = "" Then selectStmt &= " " & stmt & " AND"
            Next
            If selectStmt.EndsWith("AND") Then
                selectStmt = selectStmt.Substring(0, selectStmt.Length - 3)
            End If
        End If
        Return selectStmt
    End Function

    ''' <summary>
    ''' Fills a specific table in a dataset. Pass parms to command like the following:
    ''' FillDataSet("Select * from products where typeName =@type",ds,"products","Toys")
    ''' </summary>
    ''' <param name="SQLcommand">Sql Command Text</param>
    ''' <param name="ds"></param>
    ''' <param name="tableName"></param>
    ''' <param name="parms">Parameter values added to the SQLCommand.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fills a specific table in a dataset. Pass parms to command like the following:   FillDataSet(""Select * from products where typeName =@type"",ds,""products"",""Toys"")")> _
    Public Sub FillDataSet(ByVal SQLcommand As String, ByRef ds As DataSet, ByVal tableName As String, ByVal ParamArray parms As Object())
        SafeFillDataSet(SQLcommand, ds, tableName, parms)
    End Sub

    ''' <summary>
    ''' Fill multiple tables in a dataset. Example as following:
    ''' FillDataSetMultiSelect("Select * from products where typeName =@type; Select * from catagories where name = @catname",ds, new String (){"products","catagories"},"Toys","Childrens products")
    ''' </summary>
    ''' <param name="SQLcommand">Sql Command Text</param>
    ''' <param name="ds"></param>
    ''' <param name="tblNames"></param>
    ''' <param name="parms">Parameter values added to the SQLCommand.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill multiple tables in a dataset. Example as following:   FillDataSetMultiSelect(""Select * from products where typeName =@type; Select * from catagories where name = @catname"",ds, new String (){""products"",""catagories""},""Toys"",""Childrens products"")")> _
    Public Function FillDataSetMultiSelect(ByVal SQLcommand As String, ByRef ds As DataSet, ByVal tblNames As String(), ByVal ParamArray parms As Object()) As DataSet
        Return SafeFillDataSetMultiSelect(SQLcommand, ds, tblNames, parms)
    End Function

    ''' <summary>
    ''' Opens a DbCommand and waits for the connection execute. 
    ''' </summary>
    ''' <param name="command">typed dBCommand object</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <param name="transaction"></param>
    ''' <param name="commandType"></param>
    ''' <param name="commandText"></param>
    ''' <param name="commandParameters"></param>
    ''' <param name="mustCloseConnection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Opens a DbCommand and waits for the connection execute.")> _
    Protected Shared Sub PrepareCommand(ByVal command As DbCommand, ByVal connection As DbConnection, ByVal transaction As DbTransaction, ByVal commandType As CommandType, ByVal commandText As String, ByVal commandParameters As DbParameter(), ByRef mustCloseConnection As Boolean)
        If (command Is Nothing) Then
            Throw New ArgumentNullException("command")
        End If
        If ((commandText Is Nothing) OrElse (commandText.Length = 0)) Then
            Throw New ArgumentNullException("commandText")
        End If
        If (connection.State <> ConnectionState.Open) Then
            connection.Open()
            mustCloseConnection = True
        Else
            mustCloseConnection = False
        End If
        command.Connection = connection
        command.CommandText = commandText
        If (Not transaction Is Nothing) Then
            If (transaction.Connection Is Nothing) Then
                Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")
            End If
            command.Transaction = transaction
        End If
        command.CommandType = commandType
        If (Not commandParameters Is Nothing) Then
            AttachParameters(command, commandParameters)
        End If
    End Sub

    ''' <summary>
    ''' Adds paramters to a command.
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="commandParameters"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds paramters to a command.")> _
    Protected Shared Sub AttachParameters(ByVal command As DbCommand, ByVal commandParameters As DbParameter())
        If (command Is Nothing) Then
            Throw New ArgumentNullException("command")
        End If
        If (Not commandParameters Is Nothing) Then
            Dim parameter As DbParameter
            For Each parameter In commandParameters
                If (Not parameter Is Nothing) Then
                    If (((parameter.Direction = ParameterDirection.InputOutput) OrElse (parameter.Direction = ParameterDirection.Input)) AndAlso (parameter.Value Is Nothing)) Then
                        parameter.Value = DBNull.Value
                    End If
                    command.Parameters.Add(parameter)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Executes a Sql command string that does not return tabular data. 
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Executes a Sql command string that does not return tabular data.")> _
    Public Function ExecuteNonQuery(ByVal SQLcommand As String, Optional ByVal connection As DbConnection = Nothing) As Integer
        If connection Is Nothing Then connection = defaultConnection
        Dim command As DbCommand = createCommand()
        PrepareCommand(command, connection, Nothing, CommandType.Text, SQLcommand, Nothing, True)
        Dim num2 As Integer
        Try
            num2 = command.ExecuteNonQuery()
        Catch ex As Exception
            Throw SQLHelperException.sqlEx(command, ex)
        Finally
            connection.Close()
        End Try
        command.Parameters.Clear()
        Return num2
    End Function

    ''' <summary>
    ''' Fills a dataTable from a select string. Pass parms to command like the following:
    ''' FillDataTable("Select * from products where typeName=@type and department=@dept",dt,"Toys","children")
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="table"></param>
    ''' <param name="parms">Parameter values added to the SQLCommand.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fills a dataTable from a select string. Pass parms to command like the following:   FillDataTable(""Select * from products where typeName=@type and department=@dept"",dt,""Toys"",""children"")")> _
    Public Function FillDataTable(ByVal SQLcommand As String, ByRef table As DataTable, ByVal ParamArray parms As Object()) As DataTable
        Return SafeFillTable(SQLcommand, table, parms)
    End Function

    ''' <summary>
    ''' Fills a dataTable from a select string. Pass parms to command like the following:
    ''' FillDataTable("Select * from products where typeName=@type and department=@dept",dt,"Toys","children")
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="table"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fills a dataTable from a select string. Pass parms to command like the following:   FillDataTable(""Select * from products where typeName=@type and department=@dept"",dt,""Toys"",""children"")")> _
    Public Sub FillDataTable(ByVal SQLcommand As String, ByRef table As DataTable, ByRef connection As DbConnection)
        SafeFillTable(SQLcommand, table, , connection)
    End Sub

    ''' <summary>
    ''' Fills a dataTable from a select string. Pass parms to command like the following:
    ''' FillDataTable("Select * from products where typeName=@type and department=@dept",dt,"Toys","children")
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="parms">Parameter values added to the SQLCommand.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fills a dataTable from a select string. Pass parms to command like the following:   FillDataTable(""Select * from products where typeName=@type and department=@dept"",dt,""Toys"",""children"")")> _
    Public Function FillDataTable(ByVal SQLcommand As String, ByVal ParamArray parms As Object()) As DataTable
        Return SafeFillTable(SQLcommand, , parms)
    End Function


#Region "Fill table column filters"

    ''' <summary>
    ''' Fill a datatable with a value for each listed column. statements are anded together.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="table"></param>
    ''' <param name="colmnNames"></param>
    ''' <param name="Values"></param>
    ''' <param name="additionalString"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill a datatable with a value for each listed column. statements are anded together.")> _
    Public Sub ColumnFilterFillDataTable(ByVal SQLcommand As String, ByRef table As DataTable, ByVal colmnNames As Collection, ByVal Values As Collection, Optional ByVal additionalString As String = Nothing, Optional ByVal connection As DbConnection = Nothing)
        Dim cols(colmnNames.Count - 1) As String
        Dim vals(Values.Count - 1) As Object
        Dim i As Integer
        For i = 0 To colmnNames.Count - 1
            cols(i) = colmnNames(i + 1)
            vals(i) = Values(i + 1)
        Next
        FillDataTable(SQLcommand, table, cols, vals, additionalString, connection)
    End Sub

    ''' <summary>
    ''' Fill a datatable with a value for each listed column. statements are anded together.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="table"></param>
    ''' <param name="AdditionalStmts"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill a datatable with a value for each listed column. statements are anded together.")> _
    Public Sub ColumnFilterFillDataTable(ByVal SQLcommand As String, ByRef table As DataTable, ByVal AdditionalStmts As Collection, Optional ByVal connection As DbConnection = Nothing)
        SQLcommand = getSQLStatement(SQLcommand, AdditionalStmts)
        SafeFillTable(SQLcommand, table, Nothing, connection)
    End Sub

    ''' <summary>
    ''' Fill a datatable with a value for each listed column. statements are anded together.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="table"></param>
    ''' <param name="colmnNames"></param>
    ''' <param name="Values"></param>
    ''' <param name="additionalString"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill a datatable with a value for each listed column. statements are anded together.")> _
    Public Sub ColumnFilterFillDataTable(ByVal SQLcommand As String, ByRef table As DataTable, ByVal colmnNames As String(), ByVal Values As Object(), Optional ByVal additionalString As String = Nothing, Optional ByVal connection As DbConnection = Nothing)
        If colmnNames Is Nothing Then
            ColumnFilterFillDataTable(SQLcommand, table, New String() {}, New Object() {}, additionalString, connection)
            Return
        End If
        If Values Is Nothing Then
            ColumnFilterFillDataTable(SQLcommand, table, New String() {}, New Object() {}, additionalString, connection)
            Return
        End If
        If additionalString Is Nothing Then
            additionalString = ""
        End If
        additionalString = additionalString.Trim()
        Dim i As Integer = 0
        If colmnNames.Length > 0 Then
            Dim parm As String
            Dim parms(colmnNames.Length - 1) As Object
            If SQLcommand.ToLower.IndexOf("where") = -1 Then
                SQLcommand &= " where "
            Else
                If Not SQLcommand.ToLower.Trim.EndsWith("and") Then
                    SQLcommand &= " and "
                End If
            End If
            For Each parm In colmnNames
                SQLcommand &= " " & parm & " = " & "@" & parm & " AND"
                i += 1
            Next
            If SQLcommand.EndsWith("AND") Then
                SQLcommand = SQLcommand.Substring(0, SQLcommand.Length - 3)
            End If
            SafeFillTable(SQLcommand & additionalString, table, Values, connection)
        Else
            If additionalString.Trim.Length > 0 Then _
                SQLcommand &= " " & additionalString
            SafeFillTable(SQLcommand, table, Nothing, connection)
        End If
    End Sub


#End Region

    ''' <summary>
    ''' Execute a stored procedure.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="ds"></param>
    ''' <param name="pararray"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Execute a stored procedure.")> _
    Public Sub ExecuteSproc(ByVal SQLcommand As String, ByRef ds As DataSet, Optional ByRef pararray As DbParameter() = Nothing, Optional ByVal connection As DbConnection = Nothing)
        If isUsingDifferentHelperType(pararray, connection) Then
            getHelperExternalHelper(pararray, connection).ExecuteSproc(SQLcommand, ds, pararray, connection)
            Exit Sub
        End If
        Dim da As DbDataAdapter = prepareSafeFillAdaptor(SQLcommand, Nothing, connection)
        da.SelectCommand.CommandType = CommandType.StoredProcedure
        da.SelectCommand.Parameters.AddRange(pararray)
        SafeFillErrorCatcher(da, ds)
    End Sub

    ''' <summary>
    ''' Fetches a single value from a datastore.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fetches a single value from a datastore.")> _
    Public Function FetchSingleValue(ByVal SQLcommand As String, Optional ByVal connection As DbConnection = Nothing) As String
        Return SafeFetchSingleValue(SQLcommand, Nothing, connection)
    End Function

    ''' <summary>
    ''' Fetches a single value from a datastore.
    ''' </summary>
    ''' <param name="SQLcommand">Sql command string</param>
    ''' <param name="parms">optional parameters to pass to the select statement. Last one may be a connection.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fetches a single value from a datastore.")> _
    Public Function FetchSingleValue(ByVal SQLcommand As String, ByVal ParamArray parms() As Object) As String
        If parms.Length = 0 OrElse Not TypeOf parms.GetValue(parms.Length - 1) Is DbConnection Then
            Return SafeFetchSingleValue(SQLcommand, parms)
        Else
            Dim newparms(parms.Length - 2) As Object
            Dim c As DbConnection = parms(parms.Length - 1)
            Array.ConstrainedCopy(parms, 0, newparms, 0, parms.Length - 2)
            Return SafeFetchSingleValue(SQLcommand, newparms, c)
        End If
    End Function

#End Region

#Region "Safe Fill Area"

    ''' <summary>
    ''' Fetches a single value from a database.
    ''' </summary>
    ''' <param name="SQLcommand">Sql Command Text</param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fetches a single value from a database.")> _
    Public Function SafeFetchSingleValue(ByVal SQLcommand As String, Optional ByVal parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As String
        Dim dt As New DataTable
        SafeFillTable(SQLcommand, dt, parmValueArray, connection)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0)(0).ToString
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Executes a Sql command string that does not return tabular data. Takes a parm list as arguments.
    ''' SafeExecuteNonQuery("delete from useres where id = @id",7)
    ''' </summary>
    ''' <param name="SQLcommand">Sql Command Text</param>
    ''' <param name="parmValueArray">Array or parameters</param>
    ''' <param name="connection">dBConnection that will override the default conection.</param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Executes a Sql command string that does not return tabular data. Takes a parm list as arguments.   SafeExecuteNonQuery(""delete from useres where id = @id"",7)")> _
    Public Sub SafeExecuteNonQuery(ByVal SQLcommand As String, Optional ByVal parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing)
        If isUsingDifferentHelperType(parmValueArray, connection) Then
            getHelperExternalHelper(parmValueArray, connection).SafeExecuteNonQuery(SQLcommand, parmValueArray, connection)
            Exit Sub
        End If
        Dim da As DbDataAdapter = prepareSafeFillAdaptor(SQLcommand, parmValueArray, connection)
        Try
            da.SelectCommand.Connection.Open()
            da.SelectCommand.ExecuteNonQuery()
        Catch ex1 As Exception
            Throw SQLHelperException.sqlEx(da.SelectCommand, ex1)
        Finally
            da.SelectCommand.Connection.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Protected error catcher that will send the select command up to error stack for easier debugging.
    ''' </summary>
    ''' <param name="da"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Protected error catcher that will send the select command up to error stack for easier debugging.")> _
    Protected Sub SafeFillErrorCatcher(ByRef da As DbDataAdapter, ByRef ds As DataSet)
        If ds Is Nothing Then ds = New DataSet
        If ds.Tables.Count = 0 Then ds.Tables.Add()
        Try
            da.FillLoadOption = LoadOption.PreserveChanges
            da.MissingMappingAction = MissingMappingAction.Passthrough
            da.MissingSchemaAction = MissingSchemaAction.Add
            Dim enf As Boolean = ds.EnforceConstraints
            Dim haserr As Boolean = ds.HasErrors
            If Not enforceConstraints Then _
                ds.EnforceConstraints = False
            da.Fill(ds)
            If Not enforceConstraints AndAlso Not ds.HasErrors AndAlso enf Then
                Try
                    ds.EnforceConstraints = enf
                Catch ex As Exception
                    RaiseEvent constraintError(ds)
                End Try
            End If
        Catch ex1 As Exception
            Throw SQLHelperException.sqlEx(da.SelectCommand, ex1)
        Finally
            da.SelectCommand.Connection.Close()
        End Try
    End Sub

    Private _connectionHash As New Hashtable

    ''' <summary>
    ''' If connection are qued this will return the next available one. 
    ''' </summary>
    ''' <param name="connection"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If connection are qued this will return the next available one.")> _
    Private ReadOnly Property connectionFromStack(ByVal connection As DbConnection) As DbConnection
        Get
            If Not _connectionHash.ContainsKey(connection.ConnectionString) Then
                Dim que As New Queue
                que.Enqueue(connection)
                For i As Integer = 0 To (simultaniousConnections - 2)
                    que.Enqueue(createConnection(connection.ConnectionString))
                Next
                _connectionHash.Add(connection.ConnectionString, que)
            End If
            Dim conn As DbConnection
            Dim myque As Collections.Queue = _connectionHash(connection.ConnectionString)
            SyncLock myque
                conn = myque.Dequeue
                myque.Enqueue(conn)
            End SyncLock
            Return conn
        End Get
    End Property

    ''' <summary>
    ''' An overidable method for subclasses to add formatting and utility code to derived classes
    ''' </summary>
    ''' <param name="SQLcommand"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("An overidable method for subclasses to add formatting and utility code to derived classes")> _
    Public Function prepareAdaptor(ByVal SQLcommand As String, Optional ByRef connection As DbConnection = Nothing) As DbDataAdapter
        If connection Is Nothing Then
            connection = defaultConnection
        Else
            connection = Me.createConnection(connection.ConnectionString)
        End If
        Dim da As DbDataAdapter = createAdaptor(SQLcommand, connectionFromStack(connection))
        da.SelectCommand.CommandType = CommandType.Text
        Return da
    End Function

    ''' <summary>
    ''' An overidable method for subclasses to add formatting and utility code to derived classes
    ''' </summary>
    ''' <param name="SQLcommand"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("An overidable method for subclasses to add formatting and utility code to derived classes")> _
    Protected Overridable Function processSelectCommand(ByVal SQLcommand As String) As String
        Return SQLcommand
    End Function

    ''' <summary>
    ''' Creates an adaptor froma sql string
    ''' </summary>
    ''' <param name="SQLcommand"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates an adaptor froma sql string")> _
    Public Function prepareSafeFillAdaptor(ByVal SQLcommand As String, Optional ByVal parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As System.Data.Common.DbDataAdapter
        If Not parmValueArray Is Nothing AndAlso parmValueArray.Length > 0 AndAlso TypeOf parmValueArray.GetValue(parmValueArray.Length - 1) Is DbConnection Then
            Dim newparms(parmValueArray.Length - 2) As Object
            Array.ConstrainedCopy(parmValueArray, 0, newparms, 0, parmValueArray.Length - 1)
            connection = parmValueArray.GetValue(parmValueArray.Length - 1)
            parmValueArray = newparms
        End If
        SQLcommand = processSelectCommand(SQLcommand)
        Dim da As DbDataAdapter = prepareAdaptor(SQLcommand, connection)
        If Not parmValueArray Is Nothing AndAlso parmValueArray.Length > 0 Then
            Dim regex1 As Regex = New Regex("\x40\w+", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant _
            Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled)
            Dim i As Integer = 0
            For Each singleMatch As Match In regex1.Matches(SQLcommand)
                If Not da.SelectCommand.Parameters.Contains(singleMatch.ToString) Then
                    If TypeOf parmValueArray(i) Is DbParameter Then
                        da.SelectCommand.Parameters(singleMatch.ToString) = parmValueArray(i)
                    Else
                        da.SelectCommand.Parameters.Add(createParameter(singleMatch.ToString, parmValueArray(i)))
                    End If
                    i = i + 1
                End If
            Next
        End If
        Return da
    End Function

    ''' <summary>
    ''' Fill multiple tables in a dataset. Example as following:
    ''' FillDataSetMultiSelect("Select * from products where typeName =@type; Select * from catagories where name = @catname",ds, new String (){"products","catagories"},"Toys","Childrens products")
    ''' </summary>
    ''' <param name="SQLcommand"></param>
    ''' <param name="ds"></param>
    ''' <param name="tblNames"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill multiple tables in a dataset. Example as following:   FillDataSetMultiSelect(""Select * from products where typeName =@type; Select * from catagories where name = @catname"",ds, new String (){""products"",""catagories""},""Toys"",""Childrens products"")")> _
    Public Function SafeFillDataSetMultiSelect(ByVal SQLcommand As String, ByRef ds As DataSet, ByVal tblNames As String(), Optional ByRef parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As DataSet
        If isUsingDifferentHelperType(parmValueArray, connection) Then
            Return getHelperExternalHelper(parmValueArray, connection).SafeFillDataSetMultiSelect(SQLcommand, ds, tblNames, parmValueArray, connection)
        End If
        If ds Is Nothing Then ds = New DataSet
        If ds.Tables.Count = 0 Then ds.Tables.Add()
        If tblNames Is Nothing Then tblNames = New String() {ds.Tables(0).TableName}
        Dim da As DbDataAdapter = prepareSafeFillAdaptor(SQLcommand, parmValueArray, connection)
        For j As Integer = 0 To tblNames.Length - 1
            If tblNames(j) IsNot Nothing AndAlso Not tblNames(j).Trim = "" Then
                If j = 0 Then
                    da.TableMappings.Add("Table", tblNames(j))
                Else
                    da.TableMappings.Add("Table" & j, tblNames(j))
                End If
            End If
        Next
        SafeFillErrorCatcher(da, ds)
        Return ds
    End Function

    Private Shared ExernalHelperHash As Hashtable
    Private Function getHelperExternalHelper(ByRef parmValueArray As Object(), ByVal connection As DbConnection) As BaseHelper
        If ExernalHelperHash Is Nothing Then ExernalHelperHash = New Hashtable
        Dim conn As DbConnection = getConnectionFromParms(parmValueArray, connection)
        If Not ExernalHelperHash.ContainsKey(conn.ConnectionString) Then
            ExernalHelperHash.Add(conn.ConnectionString, DataBase.createHelper(conn))
        End If
        Return ExernalHelperHash(conn.ConnectionString)
    End Function

    Private Function getConnectionFromParms(ByRef parmValueArray As Object(), ByVal connection As DbConnection) As DbConnection
        If Not connection Is Nothing Then Return connection
        If Not parmValueArray Is Nothing AndAlso parmValueArray.Length > 0 AndAlso TypeOf parmValueArray.GetValue(parmValueArray.Length - 1) Is DbConnection Then
            connection = parmValueArray.GetValue(parmValueArray.Length - 1)
        End If
        If Not connection Is Nothing Then Return connection
        Return defaultConnection
    End Function


    Private Function isUsingDifferentHelperType(ByRef parmValueArray As Object(), ByVal connection As DbConnection) As Boolean
        If Not parmValueArray Is Nothing AndAlso parmValueArray.Length > 0 AndAlso TypeOf parmValueArray.GetValue(parmValueArray.Length - 1) Is DbConnection Then
            connection = parmValueArray.GetValue(parmValueArray.Length - 1)
        End If
        If Not connection Is Nothing AndAlso Not connection.GetType Is Me.defaultConnection.GetType Then
            Return True
        End If
        Return False
    End Function

    Public tableFromSelect As Regex = New Regex( _
        "select.*?\s+?from\s+?(?<table>([a-zA-Z]|[0-9]|\x2E|\x2D|\x5F)+?)(\s+?|$)", _
        RegexOptions.IgnoreCase _
        Or RegexOptions.CultureInvariant _
        Or RegexOptions.IgnorePatternWhitespace _
        Or RegexOptions.Compiled _
        )

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SQLcommand"></param>
    ''' <param name="dt"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("")> _
    Public Function SafeFillTable(ByVal SQLcommand As String, Optional ByRef dt As DataTable = Nothing, Optional ByRef parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As DataTable
        If dt Is Nothing Then dt = New DataTable
        If Not parmValueArray Is Nothing AndAlso parmValueArray.Length > 0 AndAlso TypeOf parmValueArray.GetValue(0) Is DataTable Then
            Dim newparms(parmValueArray.Length - 2) As Object
            Array.ConstrainedCopy(parmValueArray, 1, newparms, 0, parmValueArray.Length - 1)
            dt = parmValueArray.GetValue(0)
            parmValueArray = newparms
        End If
        If dt.DataSet Is Nothing Then
            Dim ds As New DataSet
            ds.Tables.Add(dt)
        End If

        If dt.TableName = "Table1" Then
            Dim mtc As MatchCollection = tableFromSelect.Matches(SQLcommand)
            If mtc.Count > 0 Then
                If Not mtc(0).Groups("table") Is Nothing Then
                    Dim tblname As String = mtc(0).Groups("table").Value
                    If tblname.Contains(".") Then tblname = tblname.Substring(tblname.LastIndexOf("."))
                    dt.TableName = tblname
                End If
            End If
        End If
        SafeFillDataSetMultiSelect(SQLcommand, dt.DataSet, New String() {dt.TableName}, parmValueArray, connection)
        Return dt
    End Function

    ''' <summary>
    ''' Fill multiple tables in a dataset. Example as following:
    ''' SafeFillDataSetMultiSelect("Select * from products where typeName =@type; Select * from catagories where name = @catname",ds, new String (){"products","catagories"},new Object(){"Toys","Childrens products"})
    ''' </summary>
    ''' <param name="SQLcommand">Sql Command Text</param>
    ''' <param name="ds"></param>
    ''' <param name="tablename"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fill multiple tables in a dataset. Example as following:   SafeFillDataSetMultiSelect(""Select * from products where typeName =@type; Select * from catagories where name = @catname"",ds, new String (){""products"",""catagories""},new Object(){""Toys"",""Childrens products""})")> _
    Public Function SafeFillDataSet(ByVal SQLcommand As String, Optional ByRef ds As DataSet = Nothing, Optional ByVal tablename As String = Nothing, Optional ByRef parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As DataSet
        Return SafeFillDataSetMultiSelect(SQLcommand, ds, Nothing, parmValueArray, connection)
    End Function

#End Region

#Region "Update"

    ''' <summary>
    ''' Checks for and creates a datatable if one does not exist in the database.
    ''' </summary>
    ''' <param name="dt">Datatable schema to create in the database.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Checks for and creates a datatable if one does not exist in the database.")> _
    Public Function checkAndCreateTable(ByVal dt As DataTable) As Boolean
        If Not checkDBObjectExists(dt.TableName) Then
            Dim retval As Boolean = False
            Dim priCol As DataColumn = Nothing
            If dt.PrimaryKey.Length = 1 Then
                For Each col As DataColumn In dt.Columns
                    If col.AutoIncrement AndAlso col.AutoIncrementSeed = -1 AndAlso col.AutoIncrementStep = -1 Then
                        priCol = col
                        priCol.AutoIncrementSeed = 0
                        priCol.AutoIncrementStep = 1
                    End If
                Next
            End If

            retval = createTable(dt)

            If Not priCol Is Nothing Then
                priCol.AutoIncrementSeed = -1
                priCol.AutoIncrementStep = -1
            End If

            Return retval
        End If
        Return False
    End Function

    ''' <summary>
    ''' Cheacks and createsevery dataTable in a dataSet.
    ''' </summary>
    ''' <param name="ds">The dataSet to add to the current database.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Cheacks and createsevery dataTable in a dataSet.")> _
    Public Function checkAndCreateAllTables(ByVal ds As DataSet) As Boolean
        Dim retval As Boolean = True
        For Each tbl As DataTable In ds.Tables
            If checkAndCreateTable(tbl) Then
                retval = False
            End If
        Next
        Return retval
    End Function

    ''' <summary>
    ''' Called on creation of a new DbAdaptor
    ''' </summary>
    ''' <param name="da"></param>
    ''' <param name="TableName"></param>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Called on creation of a new DbAdaptor")> _
    Protected Overridable Function ProcessDataAdaptor(ByRef da As DbDataAdapter, ByVal TableName As String, Optional ByVal dt As DataTable = Nothing) As DbDataAdapter
        If Not dt Is Nothing Then
            If Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length = 1 AndAlso dt.PrimaryKey(0).AutoIncrement Then
                Dim col As DataColumn = dt.PrimaryKey(0)
                da.UpdateCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & " = @Original_" & col.ColumnName & "  )"
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.Both
            End If
        End If
        Return da
    End Function

    Protected adaptorHash As Hashtable

    Public Sub removeCachedAdaptor(ByVal tablename As String)
        If adaptorHash Is Nothing Then adaptorHash = New Hashtable
        If adaptorHash.ContainsKey(tablename) Then
            adaptorHash.Remove(tablename)
        End If
    End Sub

    ''' <summary>
    ''' Property that returns a data adaptor for the passed in tale name. Caches the adaptor after the first call for the life of this object.
    ''' </summary>
    ''' <param name="TableName"></param>
    ''' <param name="connection"></param>
    ''' <param name="dt"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property that returns a data adaptor for the passed in tale name. Caches the adaptor after the first call for the life of this object.")> _
    Public ReadOnly Property Adaptor(ByVal TableName As String, Optional ByVal connection As DbConnection = Nothing, Optional ByVal dt As DataTable = Nothing) As DbDataAdapter
        Get
            'Dim d As Date = Date.Now
            If Not TableName.StartsWith("[") Then
                TableName = "[" & TableName & "]"
            End If
            connection = IIf(connection Is Nothing, defaultConnection, connection)
            If adaptorHash Is Nothing Then adaptorHash = New Hashtable
            If adaptorHash(TableName) Is Nothing Then
                Dim que As New Collections.Queue
                Dim builtDataAdapter As DbDataAdapter = createAdaptor()
                builtDataAdapter.SelectCommand = createCommand("Select * from " & TableName, connection)
                Dim cb As DbCommandBuilder = createCommandBuilder(builtDataAdapter)
                cb.ConflictOption = ConflictOption.OverwriteChanges
                builtDataAdapter.InsertCommand = cb.GetInsertCommand(True)
                Try
                    builtDataAdapter.UpdateCommand = cb.GetUpdateCommand(True)
                Catch ex As Exception
                    If dt Is Nothing Then
                        dt = New DataTable
                        builtDataAdapter.FillSchema(dt, SchemaType.Mapped)
                    End If
                    If createAdaptorsWithoutPrimaryKeys Then
                        Dim cmdStr As String = "update " & TableName & " set "
                        builtDataAdapter.UpdateCommand = Me.createCommand(cmdStr, connection)

                        For Each parm As DbParameter In builtDataAdapter.InsertCommand.Parameters
                            cmdStr &= "[" & parm.SourceColumn & "]=@new" & parm.SourceColumn & ","
                            Dim p As DbParameter = createParameter(parm)
                            p.ParameterName = "@new" & parm.SourceColumn
                            p.Direction = ParameterDirection.Input
                            p.SourceVersion = DataRowVersion.Current
                            builtDataAdapter.UpdateCommand.Parameters.Add(p)
                        Next
                        cmdStr = cmdStr.Trim(",")
                        Dim wherestr As String = " where "
                        For Each parm As DbParameter In builtDataAdapter.InsertCommand.Parameters
                            wherestr &= String.Format("([{0}]=@Original_{0} OR (@Original_{0} IS NULL AND [{0}] IS NULL) ) AND ", parm.SourceColumn)
                            Dim p As DbParameter = createParameter(parm)
                            p.ParameterName = "@Original_" & parm.SourceColumn
                            p.Direction = ParameterDirection.Input
                            p.SourceVersion = DataRowVersion.Original
                            builtDataAdapter.UpdateCommand.Parameters.Add(p)
                        Next

                        wherestr = wherestr.Substring(0, wherestr.Length - 4)
                        If Not dt Is Nothing Then
                            If dt.PrimaryKey.Length > 0 Then
                                For Each col As DataColumn In dt.PrimaryKey
                                    wherestr &= String.Format(" AND ([{0}]=@Original_{0} OR (@Original_{0} IS NULL AND [{0}] IS NULL) )", col.ColumnName)
                                    If Not builtDataAdapter.UpdateCommand.Parameters.Contains("@Original_" & col.ColumnName) Then
                                        Dim p As DbParameter = createParameter("@Original_" & col.ColumnName)
                                        p.ParameterName = "@Original_" & col.ColumnName
                                        p.Direction = ParameterDirection.Input
                                        p.SourceColumn = col.ColumnName
                                        p.SourceVersion = DataRowVersion.Original
                                        builtDataAdapter.UpdateCommand.Parameters.Add(p)
                                    End If
                                Next
                            End If
                        End If
                        cmdStr = cmdStr & wherestr & ";"
                        If Not dt Is Nothing AndAlso Not (Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length = 1 AndAlso dt.PrimaryKey(0).AutoIncrement) Then
                            cmdStr &= " select * from " & TableName & wherestr.Replace("@Original_", "@new")
                        End If
                        builtDataAdapter.UpdateCommand.CommandText = cmdStr
                    End If
                End Try
                Try
                    builtDataAdapter.DeleteCommand = cb.GetDeleteCommand(True)
                Catch ex As Exception
                    Dim cmdStr As String = "delete from " & TableName & " where "
                    builtDataAdapter.DeleteCommand = Me.createCommand(cmdStr, connection)
                    For Each parm As DbParameter In builtDataAdapter.InsertCommand.Parameters
                        cmdStr &= "[" & parm.SourceColumn & "]=@" & parm.SourceColumn & " AND "
                        Dim p As DbParameter = createParameter(parm)
                        p.ParameterName = "@" & parm.SourceColumn
                        p.Direction = ParameterDirection.Input
                        p.SourceVersion = DataRowVersion.Original
                        builtDataAdapter.DeleteCommand.Parameters.Add(p)
                    Next
                    cmdStr = cmdStr.Substring(0, cmdStr.Length - 4)
                    If Not dt Is Nothing Then
                        If dt.PrimaryKey.Length > 0 Then
                            For Each col As DataColumn In dt.PrimaryKey
                                cmdStr &= String.Format(" AND ([{0}]=@Original_{0} OR (@Original_{0} IS NULL AND [{0}] IS NULL) )", col.ColumnName)
                                If Not builtDataAdapter.UpdateCommand.Parameters.Contains("@Original_" & col.ColumnName) Then
                                    Dim p As DbParameter = createParameter("@Original_" & col.ColumnName)
                                    p.ParameterName = "@Original_" & col.ColumnName
                                    p.Direction = ParameterDirection.Input
                                    p.SourceColumn = col.ColumnName
                                    p.SourceVersion = DataRowVersion.Original
                                    builtDataAdapter.UpdateCommand.Parameters.Add(p)
                                End If
                            Next
                        End If
                    End If
                    builtDataAdapter.DeleteCommand.CommandText = cmdStr
                End Try

                Dim myDataAdapter As DbDataAdapter = createAdaptor("Select * from " & TableName, connection)
                myDataAdapter.InsertCommand = builtDataAdapter.InsertCommand
                myDataAdapter.UpdateCommand = builtDataAdapter.UpdateCommand
                myDataAdapter.DeleteCommand = builtDataAdapter.DeleteCommand

                'myDataAdapter.DeleteCommand.NotificationAutoEnlist = False

                myDataAdapter = ProcessDataAdaptor(myDataAdapter, TableName, dt)
                que.Enqueue(myDataAdapter)
                'If Not Platform.isMono Then
                For i As Integer = 0 To (simultaniousConnections - 2)
                    que.Enqueue(cloneSQLAdaptor(myDataAdapter, connection))
                Next
                'End If
                adaptorHash(TableName) = que
            End If
            Dim da As DbDataAdapter
            Dim mystack As Collections.Queue = adaptorHash(TableName)
            SyncLock mystack
                da = mystack.Dequeue
                mystack.Enqueue(da)
            End SyncLock
            'Dim s As String = Date.Now.Subtract(d).TotalMilliseconds
            'Debug.WriteLine(s)
            If Not da.SelectCommand.Connection.ConnectionString = connection.ConnectionString Then
                da.SelectCommand.Connection = connection
                da.UpdateCommand.Connection = connection
                da.InsertCommand.Connection = connection
                da.DeleteCommand.Connection = connection
            End If
            Return da
        End Get
    End Property

    ''' <summary>
    ''' Utility function to create a deep copy of a DbDataAdapter object.
    ''' </summary>
    ''' <param name="da"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Utility function to create a deep copy of a DbDataAdapter object.")> _
    Protected Function cloneSQLAdaptor(ByVal da As DbDataAdapter, ByVal connection As DbConnection) As DbDataAdapter
        Dim newConn As DbConnection = createConnection(connection.ConnectionString)
        Dim da1 As DbDataAdapter = createAdaptor(, newConn)
        For Each mapping As Data.Common.DataTableMapping In da.TableMappings
            da1.TableMappings.Add(mapping.SourceTable, mapping.DataSetTable)
        Next
        da1.MissingMappingAction = da.MissingMappingAction
        da1.MissingSchemaAction = da.MissingSchemaAction
        da1.FillLoadOption = da.FillLoadOption
        da1.UpdateBatchSize = da.UpdateBatchSize
        da1.SelectCommand = cloneDbCommand(da.SelectCommand)
        da1.InsertCommand = cloneDbCommand(da.InsertCommand)
        da1.UpdateCommand = cloneDbCommand(da.UpdateCommand)
        da1.DeleteCommand = cloneDbCommand(da.DeleteCommand)
        Return da1
    End Function

    ''' <summary>
    ''' Utility function to create a deep copy of a DbCommand object.
    ''' </summary>
    ''' <param name="dc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Utility function to create a deep copy of a DbCommand object.")> _
    Protected Function cloneDbCommand(ByVal dc As DbCommand) As DbCommand
        Dim dcnew As DbCommand = createCommand(dc.CommandText, createConnection(dc.Connection.ConnectionString))
        dcnew.CommandType = dc.CommandType
        dcnew.CommandTimeout = dc.CommandTimeout
        dcnew.UpdatedRowSource = dc.UpdatedRowSource
        For Each parm As DbParameter In dc.Parameters
            'dcnew.Parameters.Add(New DbParameter(parm.ParameterName, parm.SqlDbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, parm.SourceVersion, parm.Value))
            dcnew.Parameters.Add(createParameter(parm))
        Next
        Return dcnew
    End Function

    ''' <summary>
    ''' Update a given datatable, and create it if it dosen't exist in the database.
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="TableName"></param>
    ''' <param name="ContinueUpdateOnError"></param>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Update a given datatable, and create it if it dosen't exist in the database.")> _
    Public Sub UpdateCreateDB(ByRef dt As DataTable, Optional ByVal TableName As String = Nothing, Optional ByVal ContinueUpdateOnError As Boolean = False, Optional ByRef connection As DbConnection = Nothing)
        Try
            Update(dt, TableName, ContinueUpdateOnError, connection)
        Catch ex As Exception
            If Not TableName Is Nothing Then
                dt.TableName = TableName
            End If
            If checkAndCreateTable(dt) Then
                UpdateCreateDB(dt, TableName, ContinueUpdateOnError, connection)
            Else
                Throw ex
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Performs all inserts,updates and deletes on a database froma provided datatable. Uses the table name from the dataTable and pushes the changes to the table in the database with the same name. 
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Performs all inserts,updates and deletes on a database froma provided datatable. Uses the table name from the dataTable and pushes the changes to the table in the database with the same name.")> _
    Public Sub Update(ByRef dt As DataTable)
        Update(dt, Nothing)
    End Sub

    ''' <summary>
    ''' The same as Update, only the table does not need to be passed byref and is returned by the call. Helps passing in C#
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateAndReturn(ByVal dt As DataTable) As DataTable
        Update(dt, Nothing)
        Return dt
    End Function

    ''' <summary>
    ''' Performs all inserts,updates and deletes on a database froma provided datatable. Uses the table name from the dataTable and pushes the changes to the table in the database with the same name. 
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="TableName"></param>
    ''' <param name="ContinueUpdateOnError"></param>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Performs all inserts,updates and deletes on a database froma provided datatable. Uses the table name from the dataTable and pushes the changes to the table in the database with the same name.")> _
    Public Sub Update(ByRef dt As DataTable, ByVal TableName As String, Optional ByVal ContinueUpdateOnError As Boolean = False, Optional ByRef connection As DbConnection = Nothing)
        connection = IIf(connection Is Nothing, defaultConnection, connection)
        TableName = IIf(TableName Is Nothing, dt.TableName, TableName)
        If TableName.Contains(" ") Then
            If Not (TableName.StartsWith("[") AndAlso TableName.EndsWith("]")) Then _
                TableName = "[" & TableName & "]"
        End If
        Dim da As DbDataAdapter = Adaptor(TableName, connection, dt)
        SyncLock da
            Try
                If Not da.UpdateCommand.Parameters.Count = dt.Columns.Count Then
                    da.FillSchema(dt, SchemaType.Mapped)
                End If
            Catch ex As Exception

            End Try

            'dt.BeginLoadData()
            da.ContinueUpdateOnError = ContinueUpdateOnError
            'da.ContinueUpdateOnError = True
            Try
                da.Update(dt)
            Catch ex As Exception
                Throw SQLHelperException.sqlEx(da, dt, ex)
            End Try

            'dt.EndLoadData()
        End SyncLock
    End Sub

#End Region

#Region "Get Sorted Page"

    '''' <summary>
    '''' Returns a paged result for a given table with a seperate query.
    '''' </summary>
    '''' <param name="tablename"></param>
    '''' <param name="itemsPerPage"></param>
    '''' <param name="pageNum"></param>
    '''' <param name="primaryKey"></param>
    '''' <param name="sort"></param>
    '''' <param name="query"></param>
    '''' <param name="ds"></param>
    '''' <param name="connection"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Returns a paged result for a given table with a seperate query."), Obsolete("use GetSortedPAge instead. This method is not safe.")> _
    'Protected Overridable Function SafeGetSortedPageWrapper( _
    '    ByVal tablename As String, _
    '    ByVal itemsPerPage As Integer, _
    '    ByVal pageNum As Integer, _
    '    Optional ByVal primaryKey As String = "Id", _
    '    Optional ByVal sort As String = "", _
    '    Optional ByVal query As String = "", _
    '    Optional ByRef ds As DataSet = Nothing, _
    '    Optional ByVal connection As DbConnection = Nothing) As Integer

    '    If ds Is Nothing Then
    '        ds = New DataSet
    '    End If
    '    If Not ds.Tables.Contains("PageCount") Then
    '        ds.Tables.Add("PageCount")
    '        ds.Tables("PageCount").Columns.Add("PageCount")
    '    End If
    '    ds.Tables("PageCount").Clear()

    '    If pageNum < 1 Then
    '        pageNum = 1
    '    End If
    '    Dim SizeString As Integer = itemsPerPage
    '    Dim PrevString As Integer = itemsPerPage * (pageNum - 1)
    '    Dim sortdirection As String = "DESC"

    '    If String.IsNullOrEmpty(sort) Then
    '        sort = primaryKey
    '    End If
    '    If String.IsNullOrEmpty(query) Then
    '        query = "1=1"
    '    End If
    '    Dim ret As Integer = 0
    '    'tablename = stripDBNameOfDanger(tablename)
    '    Dim searchStr As String = "SELECT TOP " & SizeString & " * FROM " & tablename & " WHERE " & primaryKey & " IN" & _
    '    "(SELECT TOP " & SizeString & " " & primaryKey & " FROM " & tablename & " WHERE " & query & " AND " & primaryKey & " NOT IN" & _
    '    "(SELECT TOP " & PrevString & " " & primaryKey & " FROM " & tablename & " WHERE " & query & " ORDER BY " & sort & ")" & _
    '    "ORDER BY " & sort & ")" & _
    '    "ORDER BY " & sort
    '    Dim pageStr As String = "SELECT (COUNT(*) - 1)/" & SizeString & " + 1 AS PageCount FROM " & tablename & " WHERE " & query
    '    SafeFillDataSetMultiSelect(searchStr & ";" & pageStr, ds, New String() {tablename, "PageCount"}, New Object() {primaryKey, itemsPerPage, pageNum, sort, query})
    '    ret = ds.Tables("PageCount").Rows(0).Item(0)
    '    Return ret
    'End Function

    <System.ComponentModel.Description("Returns a paged result for a given table with a seperate query.")> _
    Public Overridable Function GetSortedPage( _
        ByRef dt As DataTable, _
        Optional ByVal tablename As String = Nothing, _
        Optional ByVal itemsPerPage As Integer = 15, _
        Optional ByVal pageNum As Integer = 1, _
        Optional ByVal primaryKey As String = Nothing, _
        Optional ByVal sort As String = "", _
        Optional ByVal query As String = "", _
        Optional ByRef parmValueArray As Object() = Nothing, _
        Optional ByVal connection As DbConnection = Nothing) As Integer
        'sort = BaseSecurityPage.RemoveSpecialCharacters(sort, "\s_\,")
        Dim pagecountTable As String = "DTIPageCount1"
        Dim ds As DataSet = dt.DataSet
        If ds Is Nothing Then
            ds = New DataSet
            ds.Tables.Add(dt)
        End If
        If Not ds.Tables.Contains(pagecountTable) Then
            ds.Tables.Add(pagecountTable)
            ds.Tables(pagecountTable).Columns.Add(pagecountTable)
        End If
        ds.Tables(pagecountTable).Clear()
        If tablename Is Nothing Then tablename = dt.TableName
        If Not tablename.ToLower.Contains("select ") Then
            If Not tablename.Contains("(") Then
                If Not tablename.StartsWith("[") Then
                    tablename = "[" & tablename & "]"
                End If
            End If
        End If
        If pageNum < 1 Then
            pageNum = 1
        End If
        Dim SizeString As Integer = itemsPerPage
        Dim PrevString As Integer = itemsPerPage * (pageNum - 1)
        Dim sortdirection As String = "DESC"
        If primaryKey Is Nothing Then
            If Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length > 0 Then
                If dt.PrimaryKey.Length > 1 Then
                    Throw New Exception("Get Sorted page needs a primary key specified for a table with a multi-column primary key.")
                End If
                primaryKey = dt.PrimaryKey(0).ColumnName
            Else
                primaryKey = "Id"
            End If
        End If
        If String.IsNullOrEmpty(sort) Then
            sort = primaryKey
        End If
        If String.IsNullOrEmpty(query) Then
            query = "1=1"
        End If
        query = "(" & query & ")"
        Dim ret As Integer = 0
        'tablename = stripDBNameOfDanger(tablename)
        Dim searchStr As String = "SELECT TOP " & SizeString & " * FROM " & tablename & " WHERE " & primaryKey & " IN" & _
        "(SELECT TOP " & SizeString & " " & primaryKey & " FROM " & tablename & " WHERE " & query & " AND " & primaryKey & " NOT IN" & _
        "(SELECT TOP " & PrevString & " " & primaryKey & " FROM " & tablename & " WHERE " & query & " ORDER BY " & sort & ")" & _
        "ORDER BY " & sort & ")" & _
        "ORDER BY " & sort
        Dim pageStr As String = "SELECT (COUNT(*) - 1)/" & SizeString & " + 1 AS " & pagecountTable & " FROM " & tablename & " WHERE " & query
        SafeFillDataSetMultiSelect(searchStr & ";" & pageStr, ds, New String() {dt.TableName, pagecountTable}, parmValueArray, connection)
        ret = ds.Tables(pagecountTable).Rows(0).Item(0)
        ds.Tables.Remove(pagecountTable)
        Return ret

    End Function

    ''' <summary>
    ''' A preventatie mesure to make sure getSortedPage is not vulnerable to sql injection.
    ''' </summary>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A preventatie mesure to make sure getSortedPage is not vulnerable to sql injection.")> _
    Private Function stripSQLStringOfDanger(ByVal tablename As String) As String
        Return tablename.Replace(" ", "").Replace(",", "").Replace("(", "").Replace(")", "").Replace("'", "").Replace("""", "").Replace("{", "").Replace("}", "")
    End Function


    '''' <summary>
    ''''  Returns a paged result for a given table with a seperate query.
    '''' </summary>
    '''' <param name="tablename"></param>
    '''' <param name="itemsPerPage"></param>
    '''' <param name="pageNum"></param>
    '''' <param name="primaryKey"></param>
    '''' <param name="sort"></param>
    '''' <param name="query"></param>
    '''' <param name="ds"></param>
    '''' <param name="connection"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Returns a paged result for a given table with a seperate query.")> _
    'Private Function SafeGetSortedPage(ByVal tablename As String, ByVal itemsPerPage As Integer, ByVal pageNum As Integer, Optional ByVal primaryKey As String = "Id", Optional ByVal sort As String = "", Optional ByVal query As String = "", Optional ByRef ds As DataSet = Nothing, Optional ByVal connection As DbConnection = Nothing) As Integer
    '    Return SafeGetSortedPageWrapper(tablename, itemsPerPage, pageNum, primaryKey, sort, query, ds, connection)
    'End Function

#End Region

#Region "Import Data"

    'Public Overridable Sub importTableData(ByVal SourceDt As DataTable, Optional ByVal createIfMissing As Boolean = True, Optional ByVal connection As DbConnection = Nothing)
    '    If connection Is Nothing Then connection = defaultConnection
    '    If createIfMissing Then checkAndCreateTable(SourceDt)
    '    Dim dt As DataTable = SourceDt.Clone
    '    For Each row As DataRow In dt.Rows
    '        row.SetAdded()
    '    Next
    '    Dim da As DbDataAdapter = cloneSQLAdaptor(createAdaptor("select * from [" & dt.TableName & "]"), connection)
    '    da.InsertCommand.Parameters.Clear()
    '    Dim command As String = "insert into [" & dt.TableName & "] ("
    '    Dim vals As String = " VALUES ("
    '    Dim i As Integer = 1
    '    For Each col As DataColumn In dt.Columns
    '        command &= "[" & col.ColumnName & "],"
    '        Dim parm As DbParameter = Me.createParameter(col.ColumnName)
    '        parm.ParameterName = "parm" & i
    '        parm.SourceColumn = col.ColumnName
    '        parm.SourceVersion = DataRowVersion.Current
    '        da.InsertCommand.Parameters.Item("parm" & i) = parm
    '        vals += "@parm" & i & ","
    '        i += 1
    '    Next
    '    command = command.Trim(",")
    '    vals = vals.Trim(",")
    '    da.InsertCommand.CommandText = command & ") " & vals & ")"
    '    da.Update(dt)
    'End Sub

    'Public Overridable Sub importDataSet(ByVal ds As DataSet, Optional ByVal createIfMissing As Boolean = True, Optional ByVal SourceConnection As DbConnection = Nothing, Optional ByVal DestinationConnection As DbConnection = Nothing)
    '    Dim c As New Collection
    '    For Each t As DataTable In ds.Tables
    '        c.Add(t, t.TableName)
    '    Next
    '    For Each r As DataRelation In ds.Relations
    '        c.Remove(r.ChildTable.TableName)
    '        c.Add(r.ChildTable, r.ChildTable.TableName, , c(r.ParentTable))
    '    Next
    '    For Each t As DataTable In c
    '        importTableData(t, True)
    '    Next

    'End Sub

    Public Overridable Sub ExportData(ByVal ds As DataSet, ByVal DestinationConnection As DbConnection, Optional ByVal autofilldataset As Boolean = True)
        Dim destHelper As BaseHelper = DataBase.createHelper(DestinationConnection)
        ExportData(ds, destHelper, autofilldataset)
    End Sub

    Private Function findindex(ByVal item As Object, ByVal enu As IEnumerable) As Integer
        Dim pos As Integer = 0
        For Each o As Object In enu
            If o = item Then Return pos
            pos += 1
        Next
    End Function

    Public Overridable Sub ExportData(ByVal ds As DataSet, ByVal destHelper As BaseHelper, Optional ByVal autofilldataset As Boolean = True)

        Dim c As New Collection
        'This section arranges the tables with parents first.
        For Each t As DataTable In ds.Tables
            c.Add(t, t.TableName)
        Next
        Dim noChange As Boolean = True
        For i As Integer = 0 To 20
            noChange = True
            For Each r As DataRelation In ds.Relations
                If findindex(r.ChildTable, c) < findindex(r.ParentTable, c) Then
                    c.Remove(r.ChildTable.TableName)
                    c.Add(r.ChildTable, r.ChildTable.TableName, Nothing, r.ParentTable.TableName)
                    noChange = False
                End If
            Next
            If noChange Then Exit For
        Next

        destHelper.checkAndCreateAllTables(ds)

        If autofilldataset Then
            ds.Clear()
            Dim query As String = ""
            Dim i As Integer = 0
            Dim tables(ds.Tables.Count) As String
            For Each t As DataTable In c
                query &= "select * from [" & t.TableName & "];"
                tables(i) = t.TableName
                i += 1
            Next
            Me.FillDataSetMultiSelect(query, ds, tables)
        End If

        For Each table As DataTable In c
            For Each row As DataRow In table.Rows
                row.AcceptChanges()
                row.SetAdded()
            Next
            destHelper.Update(table, table.TableName)
        Next
    End Sub

#End Region

#Region "Exception Class"

    ''' <summary>
    ''' An exception class used to give more complete information when database errors occure. It will proved all SQL commands plus all parameters that were provided. Use caution as this can expose SQL statements to the end user.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("An exception class used to give more complete information when database errors occure. It will proved all SQL commands plus all parameters that were provided. Use caution as this can expose SQL statements to the end user.")> _
    Public Class SQLHelperException
        Inherits Exception

        ''' <summary>
        ''' SQL Exception helper classes
        ''' </summary>
        ''' <param name="cmd"></param>
        ''' <param name="innerException"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("SQL Exception helper classes")> _
        Public Shared Function sqlEx(ByVal cmd As DbCommand, ByVal innerException As Exception) As Exception
            Dim errorStr As String = innerException.Message & vbCrLf & cmd.CommandText() & " "
            For Each parm As DbParameter In cmd.Parameters
                errorStr &= parm.ParameterName & "='" & parm.Value & "',"
            Next
            Return sqlEx(errorStr, innerException)
        End Function

        ''' <summary>
        ''' SQL Exception helper classes
        ''' </summary>
        ''' <param name="adaptor"></param>
        ''' <param name="dt"></param>
        ''' <param name="innerException"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("SQL Exception helper classes")> _
        Public Shared Function sqlEx(ByVal adaptor As DbDataAdapter, ByVal dt As DataTable, ByVal innerException As Exception) As Exception
            Dim errorStr As String = innerException.Message & vbCrLf & "Insert: " & adaptor.InsertCommand.CommandText() & " " & vbCrLf
            errorStr &= "Update: " & adaptor.UpdateCommand.CommandText() & " " & vbCrLf
            errorStr &= "Delete: " & adaptor.DeleteCommand.CommandText() & " " & vbCrLf
            For Each row As DataRow In dt.Rows
                If row.HasErrors Then
                    errorStr &= row.RowState.ToString() & " Row With Errors: "
                    For Each col As DataColumn In dt.Columns
                        errorStr &= "     " & col.ColumnName & " = "
                        Try
                            errorStr &= row(col.ColumnName) & vbCrLf
                        Catch ex As Exception
                            errorStr &= "Unable to cast. " & vbCrLf
                        End Try
                    Next
                End If
            Next

            Return sqlEx(errorStr, innerException)
        End Function

        ''' <summary>
        ''' SQL Exception helper classes
        ''' </summary>
        ''' <param name="slqString"></param>
        ''' <param name="innerException"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("SQL Exception helper classes")> _
        Public Shared Function sqlEx(ByVal slqString As String, ByVal innerException As Exception) As Exception
            Dim errorStr As String = innerException.Message & vbCrLf & vbCrLf & "SQL command : """ & slqString
            Return New Exception(errorStr, innerException)
        End Function

        Public Sub New(ByVal errorMessage As String, ByVal innerException As Exception)
            MyBase.new(errorMessage, innerException)

        End Sub
    End Class

#End Region

End Class

