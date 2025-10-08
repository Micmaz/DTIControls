Imports System.Data.Common
Imports System.Data.SQLite
Imports System.Text
Imports System.Text.RegularExpressions

Public Class SQLiteHelper
    Inherits BaseClasses.BaseHelper

    ''' <summary>
    ''' Creates a SQLiteDataAdapter from a select command. This adaptor is for filling a datatable and may not contain insert,update, or delete commands.
    ''' </summary>
    ''' <param name="command">Select command used to generate Adaptor</param>
    ''' <param name="connection">optional connection object. If ommited it uses the helper's default connection.</param>
    ''' <returns>a SQLiteDataAdapter typed to the base helper</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a SQLiteDataAdapter from a select command. This adaptor is for filling a datatable and may not contain insert,update, or delete commands.")>
    Public Overrides Function createAdaptor(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
        If command Is Nothing Then Return New SQLiteDataAdapter()
        Return New SQLiteDataAdapter(command, connection)
    End Function

    ''' <summary>
    ''' Creates a SQLiteCommand from a sqlite command string.
    ''' </summary>
    ''' <param name="command">Select command used to generate SQLiteCommand</param>
    ''' <param name="connection">optional connection object. If ommited it uses the helper's default connection.</param>
    ''' <returns>a SQLiteCommand typed to the base helper</returns>
    ''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
    <System.ComponentModel.Description("Creates a SQLiteCommand from a sqlite command string.")>
    Public Overrides Function createCommand(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbCommand
        If command Is Nothing Then Return New SQLiteCommand()
        If connection Is Nothing Then Return New SQLiteCommand(command)
        Return New SQLiteCommand(command, connection)
    End Function

    ''' <summary>
    ''' Creates a typed DbCommandBuilder
    ''' </summary>
    ''' <param name="adaptor">The typed DbDataAdapter </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed DbCommandBuilder")>
    Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
        Return New SQLiteCommandBuilder(adaptor)
    End Function

    ''' <summary>
    ''' Creates a typed connection from a string.
    ''' </summary>
    ''' <param name="ConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed connection from a string.")>
    Public Overrides Function createConnection(ByVal ConnectionString As String) As System.Data.Common.DbConnection
        Dim vars() As String = ConnectionString.Split(";")
        Dim dslash As String = "\"
        If BaseClasses.Platform.isMono Then dslash = "/"
        Dim filename As String = ""
        For Each var As String In vars
            If var.ToLower.StartsWith("data source", StringComparison.OrdinalIgnoreCase) Then
                Dim keyval() As String = var.Split("=")
                If keyval.Length = 2 Then
                    filename = keyval(1).Replace("/", dslash)
                    Dim makelocal As Boolean = False
                    If filename.Contains(dslash) Then
                        If Not System.IO.Directory.Exists(filename.Substring(0, filename.LastIndexOf(dslash))) Then
                            If filename.Contains(":") Then
                                System.IO.Directory.CreateDirectory(filename.Substring(0, filename.LastIndexOf(dslash)))
                            Else
                                makelocal = True
                            End If
                        End If
                    Else
                        makelocal = True
                    End If
                    If makelocal Then
                        filename = AppDomain.CurrentDomain.BaseDirectory & filename.Replace("/", dslash).Trim(dslash)
                        If Not System.IO.File.Exists(filename) Then
                            Dim e As Exception = Nothing
                            If Not System.IO.Directory.Exists(filename.Substring(0, filename.LastIndexOf(dslash))) Then
                                Try
                                    System.IO.Directory.CreateDirectory(filename.Substring(0, filename.LastIndexOf(dslash)))
                                Catch ex As Exception
                                    e = ex
                                    'Throw New Exception("There was an error creating the directory: " & filename.Substring(0, filename.LastIndexOf(dslash)) & ". This directory is needed for the SQLite database. Please be sure it exists and the user: " & System.Threading.Thread.CurrentPrincipal.Identity.Name & " has full access to that folder.", ex)
                                End Try
                            Else
                                Try
                                    Dim str As System.IO.Stream = System.IO.File.Create(filename)
                                    str.Close()
                                Catch ex As Exception
                                    e = ex
                                End Try
                            End If
                            If Not e Is Nothing Then
                                Throw New Exception("There was an error creating the file: " & filename & ". This file is needed for the SQLite database. Please be sure it exists and the user: " & System.Threading.Thread.CurrentPrincipal.Identity.Name & " has full access to that folder." & vbCrLf & "The error that occured was: " & e.Message)
                            End If
                        End If

                        ConnectionString = ConnectionString.Replace(keyval(1), filename)
                    End If
                    Exit For
                End If
            End If
        Next
        'If checkConnection = Nothing OrElse DateDiff(DateInterval.Minute, checkConnection, Date.Now) > 5 Then
        '    checkConnection = Date.Now
        '    Dim connection As New SQLiteConnection(ConnectionString)
        '    If (connection.State <> ConnectionState.Open) Then
        '        connection.Open()
        '        connection.Close()
        '    End If
        '    Return connection
        'Else
        Return New SQLiteConnection(ConnectionString)
        'End If

    End Function

    '''' <summary>
    '''' 
    '''' </summary>
    '''' <remarks></remarks>
    'Public Shared checkConnection As Date = Nothing

    ''' <summary>
    ''' Creates a typed dbParameter from a name and value
    ''' </summary>
    ''' <param name="name">the parm name.</param>
    ''' <param name="value">the parm value.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed dbParameter from a name and value")>
    Public Overloads Overrides Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As System.Data.Common.DbParameter
        If name Is Nothing Then Return New SQLiteParameter()
        Return New SQLiteParameter(name, value)
    End Function

    ''' <summary>
    ''' Creates a typed parameter from a genric DbParameter
    ''' </summary>
    ''' <param name="parameter">the DbParameter</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates a typed parameter from a genric DbParameter")>
    Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
        Dim parm As SQLiteParameter = parameter
        Return New SQLiteParameter(parm.ParameterName, parm.DbType, parm.Size, parm.Direction, parm.IsNullable, Byte.MaxValue, Byte.MaxValue, parm.SourceColumn, parm.SourceVersion, parm.Value)
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


#Region "Convert from T-SQL"

    Private topregex As Regex = New Regex(
  "top\s+(?<num>\d+)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.IgnorePatternWhitespace _
Or RegexOptions.Compiled
)

    ' CAST conversions - MS SQL to SQLite type mapping
    Private castDateRegex As Regex = New Regex(
  "CAST\s*\(\s*(?<expr>[^)]+)\s+AS\s+DATE\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private castDateTimeRegex As Regex = New Regex(
  "CAST\s*\(\s*(?<expr>[^)]+)\s+AS\s+DATETIME\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private castIntRegex As Regex = New Regex(
  "CAST\s*\(\s*(?<expr>[^)]+)\s+AS\s+(?:INT|INTEGER|BIGINT|SMALLINT|TINYINT)\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private castDecimalRegex As Regex = New Regex(
  "CAST\s*\(\s*(?<expr>[^)]+)\s+AS\s+(?:DECIMAL|NUMERIC|FLOAT|REAL|MONEY|SMALLMONEY)\s*(?:\([^)]+\))?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private castVarcharRegex As Regex = New Regex(
  "CAST\s*\(\s*(?<expr>[^)]+)\s+AS\s+(?:VARCHAR|NVARCHAR|CHAR|NCHAR|TEXT|NTEXT)\s*(?:\([^)]+\))?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    ' CONVERT conversions - MS SQL to SQLite
    ' CONVERT has syntax: CONVERT(data_type, expression [, style])
    Private convertDateRegex As Regex = New Regex(
  "CONVERT\s*\(\s*DATE\s*,\s*(?<expr>[^,)]+)(?:\s*,\s*\d+)?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private convertDateTimeRegex As Regex = New Regex(
  "CONVERT\s*\(\s*(?:DATETIME|DATETIME2|SMALLDATETIME)\s*,\s*(?<expr>[^,)]+)(?:\s*,\s*\d+)?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private convertIntRegex As Regex = New Regex(
  "CONVERT\s*\(\s*(?:INT|INTEGER|BIGINT|SMALLINT|TINYINT)\s*,\s*(?<expr>[^,)]+)(?:\s*,\s*\d+)?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private convertDecimalRegex As Regex = New Regex(
  "CONVERT\s*\(\s*(?:DECIMAL|NUMERIC|FLOAT|REAL|MONEY|SMALLMONEY)\s*(?:\([^)]+\))?\s*,\s*(?<expr>[^,)]+)(?:\s*,\s*\d+)?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private convertVarcharRegex As Regex = New Regex(
  "CONVERT\s*\(\s*(?:VARCHAR|NVARCHAR|CHAR|NCHAR|TEXT|NTEXT)\s*(?:\([^)]+\))?\s*,\s*(?<expr>[^,)]+)(?:\s*,\s*\d+)?\s*\)",
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.Compiled
)

    Private Function moveToptoEnd(ByVal cmd As String, ByVal top As String, ByVal topnum As String) As String
        Dim ndx As Integer = cmd.IndexOf(top)
        Dim newtop As String = " limit " & topnum
        Dim parenct As Integer = 0
        Dim i As Integer = 0
        For i = 0 To ndx
            If cmd(i) = "(" Then parenct += 1
            If cmd(i) = ")" Then parenct -= 1
        Next

        i = cmd.Length
        While parenct > 0
            i -= 1
            If cmd(i) = "(" Then parenct += 1
            If cmd(i) = ")" Then parenct -= 1
        End While
        cmd = cmd.Substring(0, ndx) & cmd.Substring(ndx + top.Length, i - (ndx + top.Length)) & newtop & cmd.Substring(i)
        Return cmd
    End Function

    Private Shared regOpts As RegexOptions = RegexOptions.Compiled Or RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.Singleline
    Private Const OuterApplyPattern As String = "(?ims)^(?<i>[ \t]*)OUTER\s+APPLY\s*\(\s*SELECT\s+(?<sel>.*?)\s+FROM\s+(?<from>\S+)\s+WHERE\s+(?<fk>\S+)\s*=\s*(?<outer>\S+)\s*\)\s*(?<a>\w+)"
    Shared ReadOnly OuterApplyRegex As Regex = New Regex(OuterApplyPattern, regOpts)

    Public Shared Function RewriteOuterApplyToLeftJoin(ByVal sql As String) As String
        If String.IsNullOrEmpty(sql) Then Return sql
        Return OuterApplyRegex.Replace(sql, Function(m)
                                                Dim i = m.Groups("i").Value
                                                Dim sel = m.Groups("sel").Value.Trim()
                                                Dim src = m.Groups("from").Value.Trim()
                                                Dim fk = m.Groups("fk").Value.Trim()
                                                Dim outer = m.Groups("outer").Value.Trim()
                                                Dim a = m.Groups("a").Value.Trim()
                                                Dim nl = Environment.NewLine
                                                Return i & "LEFT JOIN (" + nl + i & "    SELECT " + sel & ", " + fk + nl + i & "    FROM " + src + nl + i & "    GROUP BY " + fk + nl + i & ") " + a & " ON " + fk & " = " + outer
                                            End Function)
    End Function


    Private Shared StuffPattern As String = "(?is)\bSTUFF\s*\(\s*\(\s*SELECT\s+(?<select_list>.+?)\s+FROM\s+(?<from_where>.+?)\s*FOR\s+XML\s+PATH\(\s*''\s*\)\s*,\s*TYPE\)\s*\.value\(\s*'\.'\s*,\s*'N?VARCHAR\(\s*(MAX|\d*)?\s*\)'\s*\)\s*,\s*\d+\s*,\s*\d+\s*,\s*''\s*\)\s*(?:AS\s+(?<alias>\w+))"
    Private Shared StuffRegex As Regex = New Regex(StuffPattern, regOpts)
    Private Shared StuffRegexReplacement As String = "GROUP_CONCAT(" + vbCrLf + "${select_list}" + vbCrLf + ")" + vbCrLf + "FROM ${from_where}" + vbCrLf + ""

    Private Shared IsNullSmartPattern As String = "(?i)(?<str>'(?:''|[^'])*')|(?<line>--[^\r\n]*)|(?<block>/\*.*?\*/)|(?<isnull>\bISNULL\s*\()"
    Private Shared IsNullRegex As Regex = New Regex(IsNullSmartPattern, regOpts)
    ''' <summary>
    ''' Converts select from TSQL to SQLite. Top, isnull, cast, convert, and other MS SQL syntax are made SQLite compliant.
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Converts select from TSQL to SQLite. Top, isnull, cast, convert, and other MS SQL syntax are made SQLite compliant.")>
    Protected Overrides Function processSelectCommand(ByVal commandString As String) As String
        Dim outstr As String = ""
        'commandString = IsNullRegex.Replace(commandString, Function(m) If(m.Groups("isnull").Success, "IFNULL(", m.Value))
        commandString = Replace(commandString, "isnull(", "ifnull(")
        commandString = Replace(commandString, "getdate()", "datetime('now','localtime')")

        ' Convert STUFF function to SQLite syntax
        commandString = StuffRegex.Replace(commandString, StuffRegexReplacement)

        ' Convert CAST to DATE - SQLite uses DATE() function
        commandString = castDateRegex.Replace(commandString, Function(m)
                                                                 Dim expr = m.Groups("expr").Value.Trim()
                                                                 Return "DATE(" & expr & ")"
                                                             End Function)

        ' Convert CAST to DATETIME - SQLite uses DATETIME() function
        commandString = castDateTimeRegex.Replace(commandString, Function(m)
                                                                     Dim expr = m.Groups("expr").Value.Trim()
                                                                     Return "DATETIME(" & expr & ")"
                                                                 End Function)

        ' Convert CAST to INT/INTEGER types - SQLite uses CAST(expr AS INTEGER)
        commandString = castIntRegex.Replace(commandString, Function(m)
                                                                Dim expr = m.Groups("expr").Value.Trim()
                                                                Return "CAST(" & expr & " AS INTEGER)"
                                                            End Function)

        ' Convert CAST to DECIMAL/NUMERIC/FLOAT types - SQLite uses CAST(expr AS REAL)
        commandString = castDecimalRegex.Replace(commandString, Function(m)
                                                                    Dim expr = m.Groups("expr").Value.Trim()
                                                                    Return "CAST(" & expr & " AS REAL)"
                                                                End Function)

        ' Convert CAST to VARCHAR/CHAR types - SQLite uses CAST(expr AS TEXT)
        commandString = castVarcharRegex.Replace(commandString, Function(m)
                                                                    Dim expr = m.Groups("expr").Value.Trim()
                                                                    Return "CAST(" & expr & " AS TEXT)"
                                                                End Function)

        ' Convert CONVERT to DATE - SQLite uses DATE() function
        commandString = convertDateRegex.Replace(commandString, Function(m)
                                                                    Dim expr = m.Groups("expr").Value.Trim()
                                                                    Return "DATE(" & expr & ")"
                                                                End Function)

        ' Convert CONVERT to DATETIME - SQLite uses DATETIME() function
        commandString = convertDateTimeRegex.Replace(commandString, Function(m)
                                                                        Dim expr = m.Groups("expr").Value.Trim()
                                                                        Return "DATETIME(" & expr & ")"
                                                                    End Function)

        ' Convert CONVERT to INT/INTEGER types - SQLite uses CAST(expr AS INTEGER)
        commandString = convertIntRegex.Replace(commandString, Function(m)
                                                                   Dim expr = m.Groups("expr").Value.Trim()
                                                                   Return "CAST(" & expr & " AS INTEGER)"
                                                               End Function)

        ' Convert CONVERT to DECIMAL/NUMERIC/FLOAT types - SQLite uses CAST(expr AS REAL)
        commandString = convertDecimalRegex.Replace(commandString, Function(m)
                                                                       Dim expr = m.Groups("expr").Value.Trim()
                                                                       Return "CAST(" & expr & " AS REAL)"
                                                                   End Function)

        ' Convert CONVERT to VARCHAR/CHAR types - SQLite uses CAST(expr AS TEXT)
        commandString = convertVarcharRegex.Replace(commandString, Function(m)
                                                                       Dim expr = m.Groups("expr").Value.Trim()
                                                                       Return "CAST(" & expr & " AS TEXT)"
                                                                   End Function)

        For Each command As String In commandString.Split(";")
            command = RewriteOuterApplyToLeftJoin(command)
            Dim m As Match = topregex.Match(command)
            While m.Success
                command = moveToptoEnd(command, topregex.Match(command).Value, m.Groups("num").Value)
                m = topregex.Match(command)
            End While
            outstr &= command & ";"
        Next
        Return outstr

    End Function

#End Region

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
        If Not dt Is Nothing Then
            If Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length = 1 AndAlso dt.PrimaryKey(0).AutoIncrement Then
                Dim col As DataColumn = dt.PrimaryKey(0)
                da.InsertCommand.CommandText &= "; SELECT last_insert_rowid() as [" & dt.PrimaryKey(0).ColumnName & "] "
                da.InsertCommand.UpdatedRowSource = UpdateRowSource.Both
                da.UpdateCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & " = @Original_" & col.ColumnName & "  )"
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.Both
            End If
        End If
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
            Return Me.FetchSingleValue("select count(*) from sqlite_master where name='" & tablename & "'") > 0
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
        Dim createstr As String = "CREATE TABLE [" & dt.TableName & "] (  "
        For Each col As DataColumn In dt.Columns
            createstr &= vbCrLf & "[" & col.ColumnName & "] "
            If col.DataType Is GetType(Integer) Then
                createstr &= "[integer]"
            ElseIf col.DataType Is GetType(Long) Then
                createstr &= "[bigint]"
            ElseIf col.DataType Is GetType(Double) Then
                createstr &= "[Decimal]"
            ElseIf col.DataType Is GetType(Guid) Then
                createstr &= "[uniqueidentifier]"
            ElseIf col.DataType Is GetType(System.Byte()) Then
                createstr &= "[Image]"
            ElseIf col.DataType Is GetType(Date) Then
                createstr &= "[DateTime]"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT 'getDate()'"
                End If
            ElseIf col.DataType Is GetType(Boolean) Then
                createstr &= "[Bit]"
            Else  'assume string if nothing else
                If col.MaxLength > 8000 Then
                    createstr &= "[text] "
                ElseIf col.MaxLength <= 0 Then  'default to 200 char string
                    createstr &= "[varchar] (200) "
                ElseIf col.MaxLength < 20 Then
                    createstr &= "[char] (" & col.MaxLength & ")"
                Else
                    createstr &= "[varchar] (" & col.MaxLength & ") "
                End If
            End If
            If dt.PrimaryKey.Length = 1 AndAlso dt.PrimaryKey(0) Is col Then
                createstr &= " PRIMARY KEY "
                If col.AutoIncrement = True Then
                    createstr &= " autoincrement "
                End If
            End If
            If Not col.DefaultValue Is DBNull.Value Then
                createstr &= " DEFAULT '" & col.DefaultValue & "'"
            End If
            If Not col.AllowDBNull Then createstr &= " NOT NULL"
            createstr &= " ,"
        Next
        If dt.PrimaryKey.Length > 1 Then
            createstr &= vbCrLf & " PRIMARY KEY ("
            For Each pk As DataColumn In dt.PrimaryKey
                createstr &= "[" & pk.ColumnName & "] ,"
            Next
            createstr = createstr.Trim(",") & ")"
        End If
        createstr = createstr.Trim(",") & ");  " & vbCrLf

        For Each cons As System.Data.Constraint In dt.Constraints
            If cons.GetType Is GetType(UniqueConstraint) Then
                Dim ucons As UniqueConstraint = cons
                createstr &= vbCrLf & "CREATE UNIQUE INDEX " & dt.TableName.Replace(" ", "_") & "_" & ucons.ConstraintName
                createstr &= vbCrLf & " ON [" & dt.TableName & "] (  "
                For Each col As DataColumn In ucons.Columns
                    createstr &= "[" & col.ColumnName & "], "
                Next
                createstr = createstr.Substring(0, createstr.Length - 2)
                createstr &= ");  " & vbCrLf
            ElseIf cons.GetType Is GetType(ForeignKeyConstraint) Then
                Dim inCurrentTable As Boolean = False
                Dim index As String = ""
                Dim ucons As ForeignKeyConstraint = cons
                index &= vbCrLf & "CREATE INDEX " & dt.TableName & "_" & ucons.ConstraintName
                index &= vbCrLf & " ON [" & dt.TableName & "] (  "
                For Each col As DataColumn In ucons.Columns
                    If col.Table Is dt Then
                        inCurrentTable = True
                        index &= "[" & col.ColumnName & "], "
                    End If
                Next
                index = index.Substring(0, index.Length - 2)
                index &= ");  " & vbCrLf
                If inCurrentTable Then
                    createstr &= index
                End If
            End If

            'For Each col As DataColumn in
        Next

        Return createstr
    End Function

End Class
