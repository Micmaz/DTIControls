Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Text.RegularExpressions

Public Class SQLHelper
    Inherits BaseHelper

    ''' <summary>
    ''' Creates a DbDataAdapter from a command string.
    ''' </summary>
    ''' <param name="SQLcommand">Select command used to generate the DbDataAdapter</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns>a DbCommand typed to the base helper type.</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a DbDataAdapter from a command string.")> _
    Public Overrides Function createAdaptor(Optional ByVal SQLcommand As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
        If SQLcommand Is Nothing Then Return New SqlDataAdapter()
        Dim da As New SqlDataAdapter(SQLcommand, connection)
        da.SelectCommand.CommandTimeout = connection.ConnectionTimeout
        Return da
    End Function

    ''' <summary>
    ''' Creates a DbCommand from a command string.
    ''' </summary>
    ''' <param name="SQLcommand">Select command used to generate the DbCommand</param>
    ''' <param name="connection">optional dBConnection that will override the default conection.</param>
    ''' <returns>a DbCommand typed to the base helper type.</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a DbCommand from a command string.")> _
    Public Overrides Function createCommand(Optional ByVal SQLcommand As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbCommand
        If SQLcommand Is Nothing Then Return New SqlCommand()
        If connection Is Nothing Then Return New SqlCommand(SQLcommand)
        Dim cmd As New SqlCommand(SQLcommand, connection)
        cmd.CommandTimeout = connection.ConnectionTimeout
        Return cmd
    End Function

    ''' <summary>
    ''' Creates a typed DbCommandBuilder
    ''' </summary>
    ''' <param name="adaptor">The typed DbDataAdapter </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed DbCommandBuilder")> _
    Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
        Return New SqlCommandBuilder(adaptor)
    End Function

    ''' <summary>
    ''' Creates a typed connection from a string.
    ''' </summary>
    ''' <param name="ConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed connection from a string.")> _
    Public Overrides Function createConnection(ByRef ConnectionString As String) As System.Data.Common.DbConnection
        Dim cs As String = Regex.Replace(ConnectionString, "persist\s*security\s*info\s*\=\s*(true|false)", "")
        Return New SqlConnection(cs & ";Persist Security Info=True;")
    End Function

    ''' <summary>
    ''' Creates a typed dbParameter from a name and value
    ''' </summary>
    ''' <param name="name">the parm name.</param>
    ''' <param name="value">the parm value.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed dbParameter from a name and value")> _
    Public Overloads Overrides Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As System.Data.Common.DbParameter
        If name Is Nothing Then Return New SqlParameter()
        Return New SqlParameter(name, value)
    End Function

    ''' <summary>
    ''' Creates a typed parameter from a genric DbParameter
    ''' </summary>
    ''' <param name="parameter">the DbParameter</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed parameter from a genric DbParameter")> _
    Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
        Dim parm As SqlParameter = parameter
        Return New SqlParameter(parm.ParameterName, parm.SqlDbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, parm.SourceVersion, parm.Value)
    End Function

    Public Sub New(ByRef connection As SqlClient.SqlConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New(ByRef ConnectionString As String)
        MyBase.New(ConnectionString)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Called on creation of a new DbAdaptor
    ''' </summary>
    ''' <param name="da"></param>
    ''' <param name="TableName"></param>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Called on creation of a new DbAdaptor")> _
    Protected Overrides Function ProcessDataAdaptor(ByRef da As DbDataAdapter, ByVal TableName As String, Optional ByVal dt As DataTable = Nothing) As DbDataAdapter
        'If Not dt Is Nothing Then
        Dim dtemp As DataTable = Me.FillDataTable("select top 1 * from " & TableName)
        da.FillSchema(dtemp, SchemaType.Mapped)
        If Not dtemp.PrimaryKey Is Nothing AndAlso dtemp.PrimaryKey.Length = 1 AndAlso dtemp.PrimaryKey(0).AutoIncrement Then
            Dim col As DataColumn = dtemp.PrimaryKey(0)
            If Not da.InsertCommand.CommandText.ToLower.Contains("select ") Then
                da.InsertCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & "  = SCOPE_IDENTITY()) "
                da.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
            End If
            If Not da.UpdateCommand.CommandText.ToLower.Contains("select ") Then
                da.UpdateCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & " = @Original_" & col.ColumnName & "  )"
                If Not da.UpdateCommand.Parameters.Contains("@Original_" & col.ColumnName) Then
                    Dim p As DbParameter = createParameter()
                    p.ParameterName = "@Original_" & col.ColumnName
                    p.SourceColumn = col.ColumnName
                    p.Direction = ParameterDirection.Input
                    p.SourceVersion = DataRowVersion.Original
                    da.UpdateCommand.Parameters.Add(p)
                End If
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
            End If
        End If
        ' End If
        da.UpdateCommand.CommandTimeout = da.UpdateCommand.Connection.ConnectionTimeout
        da.InsertCommand.CommandTimeout = da.InsertCommand.Connection.ConnectionTimeout
        da.DeleteCommand.CommandTimeout = da.DeleteCommand.Connection.ConnectionTimeout
        If Not da.SelectCommand Is Nothing Then _
        da.SelectCommand.CommandTimeout = da.SelectCommand.Connection.ConnectionTimeout
        Return da
    End Function

    ''' <summary>
    ''' Checks if a datatable exists in a database.
    ''' </summary>
    ''' <param name="tablename">The name of the table that may eexist in the database.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Checks if a datatable exists in a database.")> _
    Public Overrides Function checkDBObjectExists(ByVal tablename As String) As Boolean
        Try
            Return Me.FetchSingleValue("select count(*) from sysobjects where name='" & tablename & "'") > 0
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
    Public Overrides Function getCreateTableString(ByVal dt As System.Data.DataTable) As String
        Dim createstr As String = "CREATE TABLE [" & dt.TableName & "] (  " & vbCrLf
        For Each col As DataColumn In dt.Columns
            createstr &= vbCrLf & "[" & col.ColumnName & "] "
            If col.DataType Is GetType(Integer) Then
                createstr &= "[int]"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT " & col.DefaultValue
                End If
                If col.AutoIncrement = True Then
                    createstr &= " IDENTITY (" & col.AutoIncrementSeed & ", " & col.AutoIncrementStep & ") "
                End If
            ElseIf col.DataType Is GetType(Long) Then
                createstr &= "[bigint]"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT " & col.DefaultValue
                End If
                If col.AutoIncrement = True Then
                    createstr &= " IDENTITY (" & col.AutoIncrementSeed & ", " & col.AutoIncrementStep & ") "
                End If
            ElseIf col.DataType Is GetType(Double) Then
                createstr &= "[Decimal](18, 5)"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT " & col.DefaultValue
                End If
            ElseIf col.DataType Is GetType(Guid) Then
                createstr &= "[UniqueIdentifier]"
            ElseIf col.DataType Is GetType(System.Byte()) Then
                createstr &= "[image]"
            ElseIf col.DataType Is GetType(Date) Then
                createstr &= "[DateTime]"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT 'getDate()'"
                End If
            ElseIf col.DataType Is GetType(Boolean) Then
                createstr &= "[Bit]"
                If Not col.DefaultValue Is DBNull.Value Then
                    Dim i As Integer = col.DefaultValue
                    createstr &= " DEFAULT " & i
                End If
            Else  'assume string if nothing else
                If col.MaxLength > 8000 Then
                    createstr &= "[text] COLLATE SQL_Latin1_General_CP1_CI_AS"
                ElseIf col.MaxLength <= 0 Then  'default to 200 char string
                    createstr &= "[varchar] (200) "
                ElseIf col.MaxLength < 20 Then
                    createstr &= "[char] (" & col.MaxLength & ")"
                Else
                    createstr &= "[varchar] (" & col.MaxLength & ") COLLATE SQL_Latin1_General_CP1_CI_AS"
                End If
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT '" & col.DefaultValue & "'"
                End If
            End If
            If Not col.AllowDBNull Then createstr &= " NOT "
            createstr &= " NULL ,"
        Next
        'createstr = createstr.Substring(0, createstr.Length - 2)
        If Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length > 0 Then
            createstr &= "	CONSTRAINT [PK_" & dt.TableName.Replace(" ", "_") & "] PRIMARY KEY  CLUSTERED	(   "
            For Each col As DataColumn In dt.PrimaryKey
                createstr &= vbCrLf & "    [" & col.ColumnName & "] ,"
            Next
            createstr = createstr.Substring(0, createstr.Length - 2)
            createstr &= "	)  ON [PRIMARY] ,"
        End If
        createstr = createstr.Substring(0, createstr.Length - 2)
        createstr &= ")  ON [PRIMARY]  " & vbCrLf
        Return createstr
    End Function

    ''' <summary>
    ''' Normalize sql statements in case non-compliant syntax works it's way in.
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Normalize sql statements in case non-compliant syntax works it's way in.")> _
    Protected Overrides Function processSelectCommand(ByVal commandString As String) As String
        commandString = commandString.Replace("||", "+")
        Return commandString
    End Function

    '''' <summary>
    '''' Used if some setup is needed by the database to return paged results. Depricated.
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Used if some setup is needed by the database to return paged results. Depricated.")> _
    'Public Overrides Function createGetSortedPage() As Boolean
    '    Dim creationString As String = "CREATE      PROCEDURE dbo.GetSortedPage(  " & vbCrLf & _
    '        "  @TableName VARCHAR(50),  " & vbCrLf & _
    '        "  @PrimaryKey VARCHAR(25),  " & vbCrLf & _
    '        "  @PageSize INT,  " & vbCrLf & _
    '        "  @PageIndex INT = 1,  " & vbCrLf & _
    '        "  @SortField VARCHAR(8000) = NULL,  " & vbCrLf & _
    '        "  @QueryFilter VARCHAR(8000) = NULL,  " & vbCrLf & _
    '        "  @Columns VARCHAR(1000) = NULL  " & vbCrLf & _
    '        ") AS  " & vbCrLf & _
    '        "SET NOCOUNT ON  " & vbCrLf & _
    '        "DECLARE @SizeString AS VARCHAR(5)  " & vbCrLf & _
    '        "DECLARE @PrevString AS VARCHAR(5)  " & vbCrLf & _
    '        "DECLARE @sortDirection AS VARCHAR(5)  " & vbCrLf & _
    '        "SET @SizeString = CONVERT(VARCHAR, @PageSize) " & vbCrLf & _
    '        "IF @PageIndex < 1 " & vbCrLf & _
    '        "BEGIN " & vbCrLf & _
    '        "Set @PageIndex = 1 " & vbCrLf & _
    '        "END " & vbCrLf & _
    '        "SET @PrevString = CONVERT(VARCHAR, @PageSize * (@PageIndex - 1))  " & vbCrLf & _
    '        "SET @sortDirection = 'DESC'  " & vbCrLf & _
    '        "IF @SortField IS NULL OR @SortField = ''  " & vbCrLf & _
    '        "BEGIN  " & vbCrLf & _
    '        "Set @SortField = @PrimaryKey  " & vbCrLf & _
    '        "END  " & vbCrLf & _
    '        "IF @QueryFilter IS NULL OR @QueryFilter = ''  " & vbCrLf & _
    '        "BEGIN  " & vbCrLf & _
    '        "Set @QueryFilter= ' WHERE 1=1'  " & vbCrLf & _
    '        "END  " & vbCrLf & _
    '        "ELSE  " & vbCrLf & _
    '        "BEGIN  " & vbCrLf & _
    '        "Set @QueryFilter= ' WHERE ' + @QueryFilter  " & vbCrLf & _
    '        "END  " & vbCrLf & _
    '        "IF @Columns IS NULL OR @Columns = ''  " & vbCrLf & _
    '        "BEGIN  " & vbCrLf & _
    '        "Set @Columns= '*'  " & vbCrLf & _
    '        "END  " & vbCrLf & _
    '        "  EXEC(  " & vbCrLf & _
    '        "  'SELECT TOP ' + @SizeString + ' ' + @Columns + ' FROM ' + @TableName + ' WHERE ' + @PrimaryKey + ' IN  " & vbCrLf & _
    '        "    (SELECT TOP ' + @SizeString + ' ' + @PrimaryKey + ' FROM ' + @TableName + @QueryFilter + ' AND ' + @PrimaryKey + ' NOT IN  " & vbCrLf & _
    '        "      (SELECT TOP ' + @PrevString + ' ' + @PrimaryKey + ' FROM ' + @TableName + @QueryFilter + ' ORDER BY ' + @SortField + ')  " & vbCrLf & _
    '        "    ORDER BY ' + @SortField + ')  " & vbCrLf & _
    '        "  ORDER BY ' + @SortField  " & vbCrLf & _
    '        "  )  " & vbCrLf & _
    '        "  EXEC('SELECT (COUNT(*) - 1)/' + @SizeString + ' + 1 AS PageCount FROM ' + @TableName + @QueryFilter)  " & vbCrLf & _
    '        "print(  " & vbCrLf & _
    '        "  'SELECT TOP ' + @SizeString + ' ' + @Columns + ' FROM ' + @TableName + ' WHERE ' + @PrimaryKey + ' IN  " & vbCrLf & _
    '        "    (SELECT TOP ' + @SizeString + ' ' + @PrimaryKey + ' FROM ' + @TableName + @QueryFilter + ' AND ' + @PrimaryKey + ' NOT IN  " & vbCrLf & _
    '        "      (SELECT TOP ' + @PrevString + ' ' + @PrimaryKey + ' FROM ' + @TableName + @QueryFilter + ' ORDER BY ' + @SortField + ')  " & vbCrLf & _
    '        "    ORDER BY ' + @SortField + ')  " & vbCrLf & _
    '        "  ORDER BY ' + @SortField  " & vbCrLf & _
    '        "  )  " & vbCrLf & _
    '        "RETURN 0  "
    '    Try
    '        ExecuteNonQuery(creationString)
    '    Catch ex As Exception
    '        Return False
    '    End Try
    '    Return True
    'End Function


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
    '''' <param name="tryAgain"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Returns a paged result for a given table with a seperate query.")> _
    'Public Overrides Function SafeGetSortedPageWrapper(ByVal tablename As String, ByVal itemsPerPage As Integer, ByVal pageNum As Integer, Optional ByVal primaryKey As String = "Id", Optional ByVal sort As String = "", Optional ByVal query As String = "", Optional ByRef ds As DataSet = Nothing, Optional ByVal connection As DbConnection = Nothing, Optional ByVal tryAgain As Boolean = True) As Integer
    '    Try
    '        SafeFillDataSetMultiSelect("exec getsortedpage @table, @primarykey, @itemsper, @pagenum, @sort, @query ", ds, New String() {tablename, "PageCount"}, New Object() {tablename, primaryKey, itemsPerPage, pageNum, sort, query})
    '    Catch ex As Exception
    '        If tryAgain AndAlso TypeOf ex.InnerException Is SqlClient.SqlException _
    '        AndAlso ex.InnerException.Message = "Could not find stored procedure 'getsortedpage'." Then
    '            createGetSortedPage()
    '            SafeGetSortedPageWrapper(tablename, itemsPerPage, pageNum, primaryKey, sort, query, ds, connection, False)
    '        End If
    '    End Try
    '    Return ds.Tables("PageCount").Rows(0).Item(0)
    'End Function
End Class