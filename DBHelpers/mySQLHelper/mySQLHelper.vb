Imports MySql.Data.MySqlClient
Imports System.Data.Common
Imports System.Text.RegularExpressions

Public Class mySQLHelper
    Inherits BaseClasses.BaseHelper


    Public Overrides Function createAdaptor(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
        If command Is Nothing Then Return New MySqlDataAdapter()
        Return New MySqlDataAdapter(command, connection)
    End Function

    Public Overrides Function createCommand(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbCommand
        If command Is Nothing Then Return New MySqlCommand()
        If connection Is Nothing Then Return New MySqlCommand(command)
        Return New MySqlCommand(command, connection)
    End Function

    Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
        Return New MySqlCommandBuilder(adaptor)
    End Function

    Public Overrides Function createConnection(ByRef ConnectionString As String) As System.Data.Common.DbConnection
        If Not ConnectionString.ToLower.Contains("allow zero datetime") Then
            ConnectionString &= ";Allow Zero Datetime=True;"
        End If
        Return New MySqlConnection(ConnectionString)
    End Function

    Public Overloads Overrides Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As System.Data.Common.DbParameter
        If name Is Nothing Then Return New MySqlParameter()
        Return New MySqlParameter(name, value)
    End Function

    Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
        Dim parm As MySqlParameter = parameter
        Return New MySqlParameter(parm.ParameterName, parm.MySqlDbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, parm.SourceVersion, parm.Value)
    End Function

    Public Overrides Function checkDBObjectExists(ByVal objName As String) As Boolean
        Return Me.FetchSingleValue("SELECT count(*) FROM information_schema.`COLUMNS` C WHERE table_name = '" & objName & "'") > 0
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

	
	Public Overrides Function getCreateTableString(ByVal dt As System.Data.DataTable) As String
        Dim createstr As String = "CREATE TABLE " & dt.TableName & " (  " & vbCrLf
        For Each col As DataColumn In dt.Columns
            createstr &= vbCrLf & col.ColumnName & " "
            If col.DataType Is GetType(Integer) Then
                createstr &= "int"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT " & col.DefaultValue
                End If
                If col.AutoIncrement = True Then
                    createstr &= " AUTO_INCREMENT "
                End If
            ElseIf col.DataType Is GetType(Double) Then
                createstr &= "DOUBLE"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT " & col.DefaultValue
                End If
            ElseIf col.DataType Is GetType(Guid) Then
                createstr &= "CHAR"
            ElseIf col.DataType Is GetType(System.Byte()) Then
                createstr &= "BINARY "
            ElseIf col.DataType Is GetType(Date) Then
                createstr &= "DateTime"
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT 'getDate()'"
                End If
            ElseIf col.DataType Is GetType(Boolean) Then
                createstr &= "TINYINT"
            Else  'assume string if nothing else
                If col.MaxLength > 1000 Then
                    createstr &= "text "
                ElseIf col.MaxLength < 20 Then
                    createstr &= "char(" & col.MaxLength & ")"
                Else
                    createstr &= "varchar(" & col.MaxLength & ") "
                End If
                If Not col.DefaultValue Is DBNull.Value Then
                    createstr &= " DEFAULT '" & col.DefaultValue & "'"
                End If
            End If
            If Not col.AllowDBNull Then createstr &= " NOT "
            createstr &= " NULL ,"
        Next
        'createstr = createstr.Substring(0, createstr.Length - 2)
        If Not dt.PrimaryKey Is Nothing Then
            createstr &= " PRIMARY KEY ( "
            For Each col As DataColumn In dt.PrimaryKey
                createstr &= vbCrLf & "    " & col.ColumnName & " ,"
            Next
            createstr = createstr.Substring(0, createstr.Length - 2)
            createstr &= "	) ,"
        End If
        createstr = createstr.Substring(0, createstr.Length - 2)
        createstr &= ")  " & vbCrLf
		return createstr
        'ExecuteNonQuery(createstr)
    End Function

#Region "Convert from T-SQL"

    Private Function moveToptoEnd(ByVal cmd As String, ByVal top As String) As String
        Dim ndx As Integer = cmd.IndexOf(top)
        Dim newtop As String = top.ToLower.Replace("top", " limit ") & " "
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

    Protected Overrides Function processSelectCommand(ByVal command As String) As String
        Dim top As New System.Text.RegularExpressions.Regex("top\s+\d+")
        While top.Match(command).Success
            command = moveToptoEnd(command, top.Match(command).Value)
        End While

        Return command

    End Function

#End Region

End Class
