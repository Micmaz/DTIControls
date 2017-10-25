Imports VistaDB.Provider
Imports System.Data.Common
Imports System.Text.RegularExpressions

Public Class vistaDBHelper
    Inherits BaseClasses.BaseHelper

    Public Overrides Function createAdaptor(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
        If command Is Nothing Then Return New VistaDBDataAdapter()
        Return New VistaDBDataAdapter(command, connection)
    End Function

    Public Overrides Function createCommand(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbCommand
        If command Is Nothing Then Return New VistaDBCommand()
        If connection Is Nothing Then Return New VistaDBCommand(command)
        Return New VistaDBCommand(command, connection)
    End Function

    Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
        Return New VistaDBCommandBuilder(adaptor)
    End Function

    Public Overrides Function createConnection(ByRef ConnectionString As String) As System.Data.Common.DbConnection
        Return New VistaDBConnection(ConnectionString)
    End Function

    Public Overloads Overrides Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As System.Data.Common.DbParameter
        If name Is Nothing Then Return New VistaDBParameter()
        Return New VistaDBParameter(name, value)
    End Function

    Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
        Dim parm As VistaDBParameter = parameter
        Return New VistaDBParameter(parm.ParameterName, parm.VistaDBType, parm.Size, parm.Direction, parm.IsNullable, parm.SourceColumn, parm.SourceVersion, parm.Value)
    End Function

    Public Overrides Function checkDBObjectExists(ByVal objName As String) As Boolean
        Return Me.FetchSingleValue("select count(*) from [Database schema] where name='" & objName & "'") > 0
    End Function

    Public Sub New(ByRef defaultconnection As VistaDBConnection)
        MyBase.New()
        If Not defaultconnection Is Nothing Then
            defaultConnection_p = createConnection(defaultconnection.ConnectionString)
        End If
    End Sub

    Public Sub New(ByRef defaultconnection As String)
        MyBase.New()
        defaultConnection_p = createConnection(defaultconnection)
    End Sub
    Protected Overrides Function ProcessDataAdaptor(ByRef da As DbDataAdapter, ByVal TableName As String, Optional ByVal dt As DataTable = Nothing) As DbDataAdapter
        If Not dt Is Nothing Then
            If Not dt.PrimaryKey Is Nothing AndAlso dt.PrimaryKey.Length = 1 AndAlso dt.PrimaryKey(0).AutoIncrement Then
                Dim col As DataColumn = dt.PrimaryKey(0)
                da.InsertCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & "  = @@Identity) "
                da.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
                da.UpdateCommand.CommandText &= ";" & vbCrLf & "select * from " & TableName & " where (" & col.ColumnName & " = @Original_" & col.ColumnName & "  )"
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord
                'myDataAdapter.InsertCommand.Parameters
            End If
        End If

        Return da
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

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
            ElseIf col.DataType Is GetType(Double) Then
                createstr &= "[Decimal]"
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
            Else  'assume string if nothing else
                If col.MaxLength > 1000 Then
                    createstr &= "[text] "
                ElseIf col.MaxLength <= 0 Then  'default to 200 char string
                    createstr &= "[varchar] (200) "
                ElseIf col.MaxLength < 20 Then
                    createstr &= "[char] (" & col.MaxLength & ")"
                Else
                    createstr &= "[varchar] (" & col.MaxLength & ") "
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
            createstr &= "	CONSTRAINT [PK_" & dt.TableName & "] PRIMARY KEY  CLUSTERED	( "
            For Each col As DataColumn In dt.PrimaryKey
                createstr &= vbCrLf & "    [" & col.ColumnName & "] ,"
            Next
            createstr = createstr.Substring(0, createstr.Length - 2)
            createstr &= "	) ,"
        End If
        createstr = createstr.Substring(0, createstr.Length - 2)
        createstr &= ")  " & vbCrLf
		return createstr
        'ExecuteNonQuery(createstr)
    End Function


End Class
