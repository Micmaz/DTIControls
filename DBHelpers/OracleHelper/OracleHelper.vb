Imports Oracle.ManagedDataAccess.Client
Imports System.Data.Common
Imports System.Text.RegularExpressions
#If DEBUG Then
Public Class OracleHelper
    Inherits BaseClasses.BaseHelper
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class OracleHelper
        Inherits BaseClasses.BaseHelper
#End If

        Public Overrides Function createAdaptor(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
            If command Is Nothing Then Return New OracleDataAdapter()
            Return New OracleDataAdapter(command, connection)
        End Function

        Public Overrides Function createCommand(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbCommand
            If command Is Nothing Then Return New OracleCommand()
            If connection Is Nothing Then Return New OracleCommand(command)
            Return New OracleCommand(command, connection)
        End Function

        Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
            Return New OracleCommandBuilder(adaptor)
        End Function

    Public Overrides Function createConnection(ByVal ConnectionString As String) As System.Data.Common.DbConnection
        Return New OracleConnection(ConnectionString)
    End Function

    Public Overloads Overrides Function createParameter(Optional ByVal name As String = Nothing, Optional ByVal value As Object = Nothing) As System.Data.Common.DbParameter
            If name Is Nothing Then Return New OracleParameter()
            Return New OracleParameter(name, value)
        End Function

        Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
            Dim parm As OracleParameter = parameter
            Return New OracleParameter(parm.ParameterName, parm.OracleDbType, parm.Size, parm.Direction, parm.IsNullable, parm.Precision, parm.Scale, parm.SourceColumn, parm.SourceVersion, parm.Value)
        End Function

        'Needs Testing
        Public Overrides Function checkDBObjectExists(ByVal tablename As String) As Boolean
            Return Me.FetchSingleValue("select count(*) from dba_objects where OBJECT_NAME='" & tablename & "'") > 0
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

	'Public Overrides Function createGetSortedPage() As Boolean

	'End Function

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

	''' <summary>
	''' Builds a create script for a table in the database based on the schema of the datatable passed in.
	''' </summary>
	''' <param name="dt">The datatable that is usedto build the create String. Only schema is used, data is ignored.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Creates a table in the database ased on the schema of the datatable passed in.")> _
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
