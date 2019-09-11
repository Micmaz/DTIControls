Imports System.Data.SQLite
Imports System.Data.Common
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
	<System.ComponentModel.Description("Creates a SQLiteDataAdapter from a select command. This adaptor is for filling a datatable and may not contain insert,update, or delete commands.")> _
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
    <System.ComponentModel.Description("Creates a SQLiteCommand from a sqlite command string.")> _
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
    <System.ComponentModel.Description("Creates a typed DbCommandBuilder")> _
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
	<System.ComponentModel.Description("Creates a typed dbParameter from a name and value")> _
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
    <System.ComponentModel.Description("Creates a typed parameter from a genric DbParameter")> _
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

    Private topregex As Regex = New Regex( _
  "top\s+(?<num>\d+)", _
RegexOptions.IgnoreCase _
Or RegexOptions.CultureInvariant _
Or RegexOptions.IgnorePatternWhitespace _
Or RegexOptions.Compiled _
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

    ''' <summary>
    ''' Converts select from TSQL to SQLite. Top and isnull are made SQLite compliant.
    ''' </summary>
    ''' <param name="commandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Converts select from TSQL to SQLite. Top and isnull are made SQLite compliant.")> _
    Protected Overrides Function processSelectCommand(ByVal commandString As String) As String
        Dim outstr As String = ""
        commandString = commandString.Replace(" isnull(", " ifnull(")
        For Each command As String In commandString.Split(";")
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
