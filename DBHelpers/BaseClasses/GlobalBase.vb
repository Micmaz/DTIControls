Imports System.Web
Imports System.Web.SessionState
Imports System.Data.SqlClient
'Imports Microsoft.ApplicationBlocks.Data

#If DEBUG Then
Public Class GlobalBase
    Inherits System.Web.HttpApplication
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class GlobalBase
        Inherits System.Web.HttpApplication
#End If

        Public Property username() As String
            Get
                Try
                    If Session.Item("username") Is Nothing Then
                        Session.Item("username") = ""
                    End If
                    Return Session.Item("username")
                Catch
                    Return ""
                End Try
            End Get
            Set(ByVal Value As String)
                Try
                    Session.Add("username", Value)
                Catch
                End Try
            End Set
        End Property

#Region " Generic Methods"

        Public Shared Function GetWebAppPath() As String

            Dim strLocation As String = System.Reflection.Assembly.GetExecutingAssembly.CodeBase

            ' Remove "file" prefix for web page locations
            If Left(strLocation, 8) = "file:///" Then strLocation = Mid(strLocation, 9)
            strLocation = System.IO.Path.GetDirectoryName(strLocation)
            strLocation = Replace(strLocation, "/", "\")
            ' Make sure config file ends up in source directory
            If Right(strLocation, 4) = "\bin" Then strLocation = Left(strLocation, Len(strLocation) - 3)

            ' Make this a file path instead of a URL path
            Return strLocation

        End Function

        Public Function GetUrlPath() As String
            Dim url As String = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.IndexOf("//") + 2)
            Return url.Substring(0, url.IndexOf("/")) & Request.ApplicationPath
            'Return strLocation & "tmp\"

        End Function


        Public Shared Function parseID(ByVal input As String) As Integer
            Try
                Return Integer.Parse(input)
            Catch ex As Exception
                Return -1
            End Try
        End Function

#End Region

#Region "Data Methods"
        Public Property sqlHelper() As SQLHelper
            Get
                Try
                    If Session.Item("sqlHelper") Is Nothing Then Session.Item("sqlHelper") = New SQLHelper(defaultConnection)
                    Return Session.Item("sqlHelper")
                Catch ex As Exception
                    Return New SQLHelper(defaultConnection)
                End Try
            End Get
            Set(ByVal Value As SQLHelper)
                Session.Item("sqlHelper") = Value
            End Set
        End Property
        Private _sqlHelper As New SQLHelper

        Public Property defaultConnection() As SqlClient.SqlConnection
            Get
                Return _defaultConnection
            End Get
            Set(ByVal Value As SqlClient.SqlConnection)
                _defaultConnection = Value
            End Set
        End Property
        Private _defaultConnection As SqlClient.SqlConnection

        '    Public Sub fillDataTable(ByVal selectStmt As String, ByVal table As DataTable, ByVal AdditionalStmts As Collection, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        If table.DataSet Is Nothing Then
        '            Dim ds As New DataSet
        '            ds.Tables.Add(table)
        '        End If
        '        selectStmt = getSQLStatement(selectStmt, AdditionalStmts)
        '        Me.fillDataSet(connection, CommandType.Text, selectStmt, table.DataSet, New String() {table.TableName})
        '    End Sub

        '    Public Function getSQLStatement(ByVal selectStmt As String, ByVal AdditionalStmts As Collection) As String
        '        If Not AdditionalStmts Is Nothing AndAlso AdditionalStmts.Count > 0 Then
        '            selectStmt = selectStmt.Trim
        '            If selectStmt.ToLower.IndexOf("where") = -1 Then
        '                selectStmt &= " where "
        '            Else
        '                selectStmt &= " AND"
        '            End If
        '            For Each stmt As String In AdditionalStmts
        '                If Not stmt.Trim = "" Then selectStmt &= " " & stmt & " AND"
        '            Next
        '            If selectStmt.EndsWith("AND") Then
        '                selectStmt = selectStmt.Substring(0, selectStmt.Length - 3)
        '            End If
        '        End If
        '        Return selectStmt
        '    End Function

        '    Public Sub fillDataSet(ByVal command As String, ByVal ds As DataSet, ByVal tableName As String, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        Me.fillDataSet(connection, CommandType.Text, command, ds, New String() {tableName})
        '    End Sub

        '    Public Sub fillDataSet(ByVal command As String, ByVal ds As DataSet, ByVal tableNames() As String, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        Me.fillDataSet(connection, CommandType.Text, command, ds, tableNames)
        '    End Sub


        '    Public Sub FillDataSet(ByVal connection As SqlConnection, ByVal commandType As CommandType, ByVal selectStmt As String, ByVal dataSet As DataSet, ByVal tblname() As String, Optional ByVal pararray As SqlParameter() = Nothing)
        '        Try
        '            Microsoft.ApplicationBlocks.Data.SqlHelper.FillDataset(connection, commandType, selectStmt, dataSet, tblname, pararray)
        '        Catch ex As Exception
        '            'try a 2nd time
        '            Try
        '                Microsoft.ApplicationBlocks.Data.SqlHelper.FillDataset(connection, commandType, selectStmt, dataSet, tblname, pararray)
        '            Catch ex1 As Exception
        '                Dim ex2 As New Exception("SQL command error: """ & selectStmt & """ " & ex.Message)
        '                Throw ex2
        '            End Try
        '        End Try
        '    End Sub

        '    Public Function createRecordSet(ByVal selectStmt As String, Optional ByVal connection As SqlConnection = Nothing) As SqlClient.SqlDataReader
        '        If connection Is Nothing Then connection = defaultConnection
        '        Return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(connection, CommandType.Text, selectStmt)
        '    End Function

        '    Public Sub ExecuteNonQuery(ByVal stmt As String, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connection, CommandType.Text, stmt)
        '    End Sub

        '    Public Sub fillDataTable(ByVal selectStmt As String, ByVal table As DataTable, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        fillDataTable(selectStmt, table, Nothing, connection)
        '    End Sub

        '    Public Sub fillDataTable(ByVal selectStmt As String, ByVal table As DataTable, ByVal colmnNames As Collection, ByVal Values As Collection, Optional ByVal additionalString As String = Nothing, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        Dim cols(colmnNames.Count - 1) As String
        '        Dim vals(Values.Count - 1) As Object
        '        Dim i As Integer
        '        For i = 0 To colmnNames.Count - 1
        '            cols(i) = colmnNames(i + 1)
        '            vals(i) = Values(i + 1)
        '        Next
        '        fillDataTable(selectStmt, table, cols, vals, additionalString, connection)
        '    End Sub

        '    'Main fillDataTable method. All similarly named methods execute here.

        '    Public Sub FillDataSetMultiSelect(ByVal selectStmt As String, ByVal ds As DataSet, ByVal tblNames As String(), Optional ByVal connection As SqlConnection = Nothing)
        '        Try
        '            If connection Is Nothing Then
        '                connection = defaultConnection
        '            End If
        '            Dim da As New SqlDataAdapter(selectStmt, connection)
        '            da.SelectCommand.CommandType = CommandType.Text
        '            For i As Integer = 0 To tblNames.Length - 1
        '                If i = 0 Then
        '                    da.TableMappings.Add("Table", tblNames(i))
        '                Else
        '                    da.TableMappings.Add("Table" & i, tblNames(i))
        '                End If
        '            Next
        '            da.Fill(ds)
        '        Catch ex As Exception
        '            Dim ex2 As New Exception("SQL command error: """ & selectStmt & """ " & ex.Message)
        '            Throw ex2
        '        End Try

        '    End Sub

        '    Public Sub fillDataTable(ByVal selectStmt As String, ByVal table As DataTable, ByVal colmnNames As String(), ByVal Values As Object(), Optional ByVal additionalString As String = Nothing, Optional ByVal connection As SqlConnection = Nothing)
        '        If connection Is Nothing Then connection = defaultConnection
        '        If table.DataSet Is Nothing Then
        '            Dim ds As New DataSet
        '            ds.Tables.Add(table)
        '        End If
        '        If colmnNames Is Nothing Then
        '            fillDataTable(selectStmt, table, New String() {}, New Object() {}, additionalString, connection)
        '            Return
        '        End If
        '        If Values Is Nothing Then
        '            fillDataTable(selectStmt, table, New String() {}, New Object() {}, additionalString, connection)
        '            Return
        '        End If
        '        If additionalString Is Nothing Then
        '            additionalString = ""
        '        End If
        '        additionalString = additionalString.Trim()
        '        Dim i As Integer = 0
        '        If colmnNames.Length > 0 Then
        '            Dim parm As String
        '            Dim pararray(colmnNames.Length - 1) As SqlClient.SqlParameter
        '            If selectStmt.ToLower.IndexOf("where") = -1 Then
        '                selectStmt &= " where "
        '            Else
        '                If Not selectStmt.ToLower.Trim.EndsWith("and") Then
        '                    selectStmt &= " and "
        '                End If
        '            End If
        '            For Each parm In colmnNames
        '                selectStmt &= " " & parm & " = " & "@" & parm & " AND"
        '                pararray(i) = New SqlClient.SqlParameter("@" & parm, Values(i))
        '                i += 1
        '            Next
        '            If selectStmt.EndsWith("AND") Then
        '                selectStmt = selectStmt.Substring(0, selectStmt.Length - 3)
        '            End If
        '            Me.fillDataSet(connection, CommandType.Text, selectStmt & additionalString, table.DataSet, New String() {table.TableName}, pararray)
        '        Else
        '            If additionalString.Trim.Length > 0 Then _
        '                selectStmt &= " " & additionalString
        '            Me.fillDataSet(connection, CommandType.Text, selectStmt, table.DataSet, New String() {table.TableName})
        '        End If
        '    End Sub

        '    Public Function fetchSingleValue(ByVal SQL As String, Optional ByVal connection As SqlConnection = Nothing) As String
        '        If connection Is Nothing Then connection = defaultConnection
        '        Dim dt As New DataTable
        '        fillDataTable(SQL, dt, connection)
        '        If dt.Rows.Count > 0 Then
        '            Return dt.Rows(0)(0)
        '        Else
        '            Return Nothing
        '        End If
        '    End Function
#End Region

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

    End Class


