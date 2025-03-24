Imports System.Text.RegularExpressions
Imports System.Data.Common
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Reflection

''' <summary>
''' Data layer class used to execute multiple sql statements in once command. Usefull for component systems where many components execute a select independant of one another.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Data layer class used to execute multiple sql statements in once command. Usefull for component systems where many components execute a select independant of one another."),ComponentModel.ToolboxItem(False)> _
Public Class ParallelDataHelper
    Inherits System.ComponentModel.Component

    Private Enum CommandTypes
        Table
        NonQuery
        SingleValue
    End Enum

    ''' <summary>
    ''' All commands in sql que have been run and the data has been set to it's target object.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("All commands in sql que have been run and the data has been set to it's target object.")> _
    Public Event DataReady()
    Public Event handleInvalidObject(ByVal dt As DataTable)
    Public paramCount As Integer = 0


#Region "Properties"

    'Private _parallelDataSet As DataSet
    'Private ReadOnly Property ParallelDataSet() As DataSet
    '    Get
    '        If _parallelDataSet Is Nothing Then
    '            _parallelDataSet = New DataSet
    '        End If
    '        Return _parallelDataSet
    '    End Get
    'End Property

    Private _nonQueryDummyTable As DataTable
    Private ReadOnly Property NonQueryDummyTable() As DataTable
        Get
            If _nonQueryDummyTable Is Nothing Then
                _nonQueryDummyTable = New DataTable
            End If
            Return _nonQueryDummyTable
        End Get
    End Property

    'Private _nonQueryRowsAffected As ArrayList
    'Private ReadOnly Property NonQueryRowsAffected() As ArrayList
    '    Get
    '        If _nonQueryRowsAffected Is Nothing Then
    '            _nonQueryRowsAffected = New ArrayList
    '        End If
    '        Return _nonQueryRowsAffected
    '    End Get
    'End Property

    Private _fetchSingleValueTable As DataTable
    Private ReadOnly Property FetchSingleValueTable() As DataTable
        Get
            If _fetchSingleValueTable Is Nothing Then
                _fetchSingleValueTable = New DataTable
            End If
            Return _fetchSingleValueTable
        End Get
    End Property

    Private Shared asmhashtable As Hashtable
    Private _sqlHelper As BaseHelper
    Public Property sqlHelper() As BaseHelper
        Get
            If _sqlHelper Is Nothing Then _
                    _sqlHelper = BaseHelper.getHelper()
            Return _sqlHelper
        End Get
        Set(ByVal value As BaseHelper)
            _sqlHelper = value
        End Set
    End Property

    Private _defaultConnection As DbConnection
    Private ReadOnly Property DefaultConnection() As DbConnection
        Get
            If _defaultConnection Is Nothing Then
                _defaultConnection = sqlHelper.defaultConnection
            End If
            Return _defaultConnection
        End Get
    End Property

    Private _dbConnectionManager As Hashtable
    Private ReadOnly Property DBConnectionManager() As Hashtable
        Get
            If _dbConnectionManager Is Nothing Then
                _dbConnectionManager = New Hashtable
            End If
            Return _dbConnectionManager
        End Get
    End Property

    Private _pageCountManager As ArrayList
    Private ReadOnly Property PageCountManager() As ArrayList
        Get
            If _pageCountManager Is Nothing Then
                _pageCountManager = New ArrayList
            End If
            Return _pageCountManager
        End Get
    End Property

    Private _dataFetched As Boolean = False
    Public Property DataFetched() As Boolean
        Get
            Return _dataFetched
        End Get
        Set(ByVal value As Boolean)
            _dataFetched = value
        End Set
    End Property

#End Region

#Region "Public Functions"

    ''' <summary>
    ''' adds a table select to the sql que that will be filled befor the dataready event.
    ''' </summary>
    ''' <param name="stmt"></param>
    ''' <param name="dt"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <param name="createTableOnError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("adds a table select to the sql que that will be filled befor the dataready event.")> _
    Public Function addFillDataTable(ByVal stmt As String, Optional ByRef dt As DataTable = Nothing, Optional ByRef parmValueArray As Object() = Nothing, Optional ByRef connection As DbConnection = Nothing, Optional ByVal createTableOnError As Boolean = True) As DataTable
        If connection Is Nothing Then connection = DefaultConnection
        If dt Is Nothing Then dt = New DataTable
        If dt.DataSet Is Nothing Then
            Dim ds As New DataSet
            ds.Tables.Add(dt)
        End If
        If dt.TableName = "Table1" Then
            Dim mtc As MatchCollection = sqlHelper.tableFromSelect.Matches(stmt)
            If mtc.Count > 0 Then
                If Not mtc(0).Groups("table") Is Nothing Then
                    Dim tblname As String = mtc(0).Groups("table").Value
                    If tblname.Contains(".") Then tblname = tblname.Substring(tblname.LastIndexOf("."))
                    dt.TableName = tblname
                End If
            End If
        End If

        addCommand(dt, stmt, parmValueArray, connection, , createTableOnError)

        Return dt
    End Function


    '''' <summary>
    '''' Adds a pages fill to the sql que that will be filled befor the dataready event.
    '''' </summary>
    '''' <param name="ds"></param>
    '''' <param name="tableName"></param>
    '''' <param name="destinationTableName"></param>
    '''' <param name="primary_Key"></param>
    '''' <param name="pageSize"></param>
    '''' <param name="pageNum"></param>
    '''' <param name="sort"></param>
    '''' <param name="queryFilter"></param>
    '''' <param name="cols"></param>
    '''' <param name="connection"></param>
    '''' <remarks></remarks>
    '<System.ComponentModel.Description("Adds a pages fill to the sql que that will be filled befor the dataready event.")> _
    'Public Sub addGetSortedPage(ByRef ds As DataSet, ByVal tableName As String, Optional ByVal destinationTableName As String = "", Optional ByVal primary_Key As String = "Id", Optional ByVal pageSize As Integer = 10, Optional ByVal pageNum As Integer = 1, Optional ByVal sort As String = "", Optional ByVal queryFilter As String = "", Optional ByVal cols As String = "", Optional ByRef connection As DbConnection = Nothing)
    '    sqlHelper.SafeGetSortedPage(tableName, pageSize, pageNum, primary_Key, sort, queryFilter, ds, connection)
    '    'If connection Is Nothing Then connection = DefaultConnection
    '    'If destinationTableName = "" Then destinationTableName = tableName
    '    'If Not PageCountManager.Contains(ds) Then
    '    '    PageCountManager.Add(ds)
    '    'End If
    '    'addCommand(ds.Tables(destinationTableName), _
    '    '    "exec getsortedpage @Table, @Primarykey, @Itemsper, @Pagenum, @Sort, @Query, @Cols", _
    '    '    New Object() {tableName, primary_Key, pageSize, pageNum, sort, queryFilter, cols}, connection)
    'End Sub

    ''' <summary>
    ''' Adds a non-query to the sql que.
    ''' </summary>
    ''' <param name="stmt"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a non-query to the sql que.")> _
    Public Sub addExecuteNonQuery(ByVal stmt As String, Optional ByRef parmValueArray As Object() = Nothing, Optional ByRef connection As DbConnection = Nothing)
        If connection Is Nothing Then connection = DefaultConnection
        addCommand(NonQueryDummyTable, stmt, parmValueArray, connection)
    End Sub

    ''' <summary>
    ''' Adds a single value fetch to the sql que.
    ''' </summary>
    ''' <param name="stmt"></param>
    ''' <param name="parmValueArray"></param>
    ''' <param name="connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a single value fetch to the sql que.")> _
    Public Function addFetchSingleValue(ByVal stmt As String, Optional ByRef parmValueArray As Object() = Nothing, Optional ByVal connection As DbConnection = Nothing) As String
        If connection Is Nothing Then connection = DefaultConnection

        Dim returnVal As String
        addCommand(FetchSingleValueTable, stmt, parmValueArray, connection, returnVal)
        Return returnVal
    End Function

    ''' <summary>
    ''' performs all sql calls in the current sql que.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("performs all sql calls in the current sql que.")> _
    Public Sub executeParallelDBCall()
        Dim conn_keys As ICollection = DBConnectionManager.Keys
        Dim dbError As Exception = Nothing

        For Each conn As DbConnection In conn_keys
            Dim ds As New DataSet
            Dim stmt As String = ""
            Dim params As New ArrayList
            Dim tableNameList As New ArrayList

            Dim myDBTableManager As ParallelCallTableManager
            myDBTableManager = CType(DBConnectionManager.Item(conn), ParallelCallTableManager)

            Dim table_keys As ICollection = myDBTableManager.Keys
            For Each table As DataTable In table_keys
                Dim commandManager As ParallelCallStatementManager = myDBTableManager.Item(table)

                'If Not table.DataSet Is Nothing Then
                'commandManager.orignalDataSet = table.DataSet
                'table.DataSet.Tables.Remove(table)
                'End If
                If ds.Tables(table.tablename) Is Nothing Then
                    ds.Tables.Add(table.Clone)
                End If

                For Each statement As ParallelCallStatement In commandManager.StatementManager
                    stmt &= statement.queryCommand & "; "
                    If Not statement.paramsArray Is Nothing Then
                        For Each param As Object In statement.paramsArray
                            params.Add(param)
                        Next
                    End If

                    tableNameList.Add(table.TableName)
                    If statement.queryCommand.ToLower.Contains("getsortedpage") Then
                        tableNameList.Add("PageCount" & PageCountManager.IndexOf(table.DataSet))
                    End If
                Next
            Next

            Dim tableNames(tableNameList.Count) As String
            For index As Integer = 0 To tableNameList.Count - 1
                tableNames(index) = tableNameList(index)
            Next

			sqlHelper.SafeFillDataSetMultiSelect(stmt, ds, tableNames, params.ToArray, conn)


            For Each table As DataTable In table_keys
                Dim commandManager As ParallelCallStatementManager = myDBTableManager.Item(table)

                'ds.Tables.Remove(table)
                'If Not commandManager.orignalDataSet Is Nothing Then
                '    commandManager.orignalDataSet.Tables.Add(table)
                'End If

                If Not table.DataSet Is Nothing Then
                    table.BeginLoadData()
                    table.Merge(ds.Tables(table.TableName), False, MissingSchemaAction.Add)
                End If

                Dim cmdIndex As Integer = 0
                For Each statement As ParallelCallStatement In commandManager.StatementManager
                    If statement.queryCommand.ToLower.Contains("getsortedpage") Then
                        If Not table.DataSet.Tables.Contains("PageCount") Then
                            table.DataSet.Tables.Add("PageCount")
                        Else
                            table.DataSet.Tables("PageCount").Clear()
                        End If
                        table.DataSet.Tables("PageCount").BeginLoadData()
                        table.DataSet.Tables("PageCount").Merge(ds.Tables("PageCount" & _
                            PageCountManager.IndexOf(table.DataSet)), True, MissingSchemaAction.Add)
                        table.DataSet.Tables("PageCount").EndLoadData()
                    End If
                    If Not statement.returnObject Is Nothing Then
                        If table.Equals(FetchSingleValueTable) Then
                            statement.returnObject = FetchSingleValueTable.Rows(cmdIndex)(0)
                        End If
                    End If

                    cmdIndex += 1
                Next
            Next

            For Each table As DataTable In table_keys
                If Not table.DataSet Is Nothing Then
                    table.EndLoadData()
                End If
            Next
        Next

        DBConnectionManager.Clear()
        paramCount = 0
        If Not dbError Is Nothing Then Throw dbError
        RaiseEvent DataReady()
    End Sub
#End Region

#Region "Private Functions"

    Private Sub addCommand(ByRef myKey As DataTable, ByVal cmd As String, ByRef paramValueArray As Object(), ByRef connection As DbConnection, Optional ByRef returnVal As String = Nothing, Optional ByVal createTableOnError As Boolean = True)
        Dim myDBTableManager As ParallelCallTableManager

        Dim regex1 As Regex = New Regex("\x40\w+", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant _
            Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled)
        For Each singleMatch As Match In regex1.Matches(cmd)
            cmd = cmd.Replace(singleMatch.ToString, "@var" & paramCount)
            paramCount += 1
        Next

        If DBConnectionManager.Contains(connection) Then

            myDBTableManager = CType(DBConnectionManager.Item(DefaultConnection), ParallelCallTableManager)

            If myDBTableManager.Contains(myKey) Then
                Dim callMan As ParallelCallStatementManager = CType(myDBTableManager.Item(myKey), _
                ParallelCallStatementManager)
                callMan.StatementManager.Add(New ParallelCallStatement(cmd, paramValueArray, returnVal))
                callMan.createTableOnError = createTableOnError
            Else
                Dim myStatements As New ParallelCallStatementManager
                myStatements.StatementManager.Add(New ParallelCallStatement(cmd, paramValueArray, returnVal))
                myStatements.createTableOnError = createTableOnError
                myDBTableManager.Add(myKey, myStatements)
            End If

        Else
            myDBTableManager = New ParallelCallTableManager
            Dim myStatements As New ParallelCallStatementManager
            myStatements.StatementManager.Add(New ParallelCallStatement(cmd, paramValueArray, returnVal))
            myStatements.createTableOnError = createTableOnError
            myDBTableManager.Add(myKey, myStatements)
            DBConnectionManager.Add(DefaultConnection, myDBTableManager)
        End If
    End Sub

#End Region

End Class

Friend Class ParallelCallTableManager
    Inherits Hashtable
End Class

''' <summary>
''' Manages parallel data calls in a pages request/responce loop.
''' </summary>
''' <remarks></remarks>
Friend Class ParallelCallStatementManager

    Private _statementsManager As ArrayList
    Public ReadOnly Property StatementManager() As ArrayList
        Get
            If _statementsManager Is Nothing Then
                _statementsManager = New ArrayList
            End If
            Return _statementsManager
        End Get
    End Property

    Public createTableOnError As Boolean = True
End Class

''' <summary>
''' A container class for an individual parallel sql call.
''' </summary>
''' <remarks></remarks>
Friend Class ParallelCallStatement
    Public returnObject As Object
    Public paramsArray() As Object
    Public queryCommand As String

    Public Sub New(ByVal cmd As String, Optional ByVal paramsArrayArg() As Object = Nothing, Optional ByVal returnObjectArg As Object = Nothing)
        queryCommand = cmd
        returnObject = returnObjectArg
        paramsArray = paramsArrayArg
    End Sub
End Class
