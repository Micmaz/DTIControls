Imports System.Data.Common
Imports System.Text.RegularExpressions

Public Class EmptyHelper
	Inherits BaseClasses.BaseHelper
	Public errorMessage As String = ""
	''' <summary>
	''' Creates a SQLiteDataAdapter from a select command. This adaptor is for filling a datatable and may not contain insert,update, or delete commands.
	''' </summary>
	''' <param name="command">Select command used to generate Adaptor</param>
	''' <param name="connection">optional connection object. If ommited it uses the helper's default connection.</param>
	''' <returns>a SQLiteDataAdapter typed to the base helper</returns>
	''' <remarks>The default connection uses web config connection string named 'DTIConnection' or 'ConnectionString'</remarks>
	<System.ComponentModel.Description("Creates a SQLiteDataAdapter from a select command. This adaptor is for filling a datatable and may not contain insert,update, or delete commands.")>
	Public Overrides Function createAdaptor(Optional ByVal command As String = Nothing, Optional ByVal connection As System.Data.Common.DbConnection = Nothing) As System.Data.Common.DbDataAdapter
		Return New System.Data.SqlClient.SqlDataAdapter()
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
		Return New System.Data.SqlClient.SqlCommand()
	End Function

	''' <summary>
	''' Creates a typed DbCommandBuilder
	''' </summary>
	''' <param name="adaptor">The typed DbDataAdapter </param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Creates a typed DbCommandBuilder")>
	Public Overrides Function createCommandBuilder(ByRef adaptor As System.Data.Common.DbDataAdapter) As System.Data.Common.DbCommandBuilder
		Return New SqlClient.SqlCommandBuilder()
	End Function

	''' <summary>
	''' Creates a typed connection from a string.
	''' </summary>
	''' <param name="ConnectionString"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Creates a typed connection from a string.")>
	Public Overrides Function createConnection(ByVal ConnectionString As String) As System.Data.Common.DbConnection
		Return New System.Data.SqlClient.SqlConnection()
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
		Return New System.Data.SqlClient.SqlParameter()
	End Function

	''' <summary>
	''' Creates a typed parameter from a genric DbParameter
	''' </summary>
	''' <param name="parameter">the DbParameter</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Creates a typed parameter from a genric DbParameter")>
	Public Overloads Overrides Function createParameter(ByRef parameter As System.Data.Common.DbParameter) As System.Data.Common.DbParameter
		Return New System.Data.SqlClient.SqlParameter()
	End Function

	Public Sub New(ByRef ConnectionString As String)

	End Sub

	Public Sub New()

	End Sub

	''' <summary>
	''' Called on creation of a new DbAdaptor
	''' </summary>
	''' <param name="da"></param>
	''' <param name="TableName"></param>
	''' <param name="dt"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Called on creation of a new DbAdaptor")>
	Protected Overrides Function ProcessDataAdaptor(ByRef da As DbDataAdapter, ByVal TableName As String, Optional ByVal dt As DataTable = Nothing) As DbDataAdapter
		Return da
	End Function

	''' <summary>
	''' Checks if a datatable exists in a database.
	''' </summary>
	''' <param name="tablename">The name of the table that may eexist in the database.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Checks if a datatable exists in a database.")>
	Public Overrides Function checkDBObjectExists(ByVal tablename As String) As Boolean
		Return True
	End Function

	''' <summary>
	''' Builds a create script for a table in the database based on the schema of the datatable passed in.
	''' </summary>
	''' <param name="dt">The datatable that is usedto build the create String. Only schema is used, data is ignored.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("Builds a create script for a table in the database based on the schema of the datatable passed in.")>
	Public Overrides Function getCreateTableString(ByVal dt As System.Data.DataTable) As String
		Return ""
	End Function

End Class
