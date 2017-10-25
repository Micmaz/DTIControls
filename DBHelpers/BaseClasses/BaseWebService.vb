

''' <summary>
''' Base class for System.Web.Services.WebService class, adding data accessors and error handeling.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Base class for System.Web.Services.WebService class, adding data accessors and error handeling."),ComponentModel.ToolboxItem(False)> _
Public Class BaseWebService
    Inherits System.Web.Services.WebService
    Private Const classname As String = "BaseClasses.BaseWebService"

#Region "Data Accessors"
    Private _currBaseClassType As Type
    Private ReadOnly Property currBaseClassType() As Type
        Get
            If _currBaseClassType Is Nothing Then
                _currBaseClassType = Me.GetType
                Dim BaseSecurityPageType As Type = Type.GetType(classname)
                While Not _currBaseClassType.BaseType Is BaseSecurityPageType
                    _currBaseClassType = _currBaseClassType.BaseType
                End While
            End If
            Return _currBaseClassType
        End Get
    End Property

    Private _currBaseDataType As Type
    Private ReadOnly Property currBaseDataType() As Type
        Get
            If _currBaseDataType Is Nothing Then
                _currBaseDataType = CType(currBaseClassType.GetMember("Data")(0), System.Reflection.PropertyInfo).PropertyType
            End If
            Return _currBaseDataType
        End Get
    End Property

    ''' <summary>
    ''' Returns a subclass of Database. The loacl assembly is searched, otherwise a cached version of Database is returned. Database is cached in the session so it will be user-isolated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns a subclass of Database. The loacl assembly is searched, otherwise a cached version of Database is returned. Database is cached in the session so it will be user-isolated.")> _
    Public ReadOnly Property Data() As BaseClasses.DataBase
        Get
            If DesignMode Then
                Return Nothing
            End If
            If Session Is Nothing Then
                Return getdata(False)
            End If
            If Session("DataObj." & currBaseDataType.FullName) Is Nothing Then
                Dim data1 As DataBase = getdata(False)
                Session("DataObj." & currBaseDataType.FullName) = data1
            End If
            Return Session("DataObj." & currBaseDataType.FullName)
        End Get

    End Property

    Private privData As DataBase
    Private Function getdata(Optional ByVal adderrorhandeler As Boolean = True) As DataBase
        If Not privData Is Nothing Then
            Return privData
        Else
            privData = New DataBase
            Dim methinfo As System.Reflection.PropertyInfo = CType(currBaseClassType.GetMember("Data")(0), System.Reflection.PropertyInfo)
            Dim dataType As Type = methinfo.PropertyType

            privData = dataType.Assembly.CreateInstance(dataType.FullName, False, Reflection.BindingFlags.ExactBinding, Nothing, Nothing, Nothing, Nothing)
            privData.session = Session
            If adderrorhandeler AndAlso Application.Get("ErrorHandler") Is Nothing Then
                methinfo = CType(currBaseClassType.GetMember("errorHandler")(0), System.Reflection.PropertyInfo)
                dataType = methinfo.PropertyType
                Dim perrorHandler As ErrorHandlerBase = dataType.Assembly.CreateInstance(dataType.FullName, False, Reflection.BindingFlags.ExactBinding, Nothing, Nothing, Nothing, Nothing)
                perrorHandler.application = Me.Context.ApplicationInstance
                Application.Add("ErrorHandler", perrorHandler)
                'addhandler Application.e
            End If
            Return privData
        End If

    End Function

    ''' <summary>
    ''' Convienece method, returns the BaseHelper in the dataObject. This is the default BaseHelper for a web application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convienece method, returns the BaseHelper in the dataObject. This is the default BaseHelper for a web application.")> _
    Public ReadOnly Property sqlHelper() As BaseHelper
        Get
            Return Data.sqlHelper
        End Get
    End Property

    ''' <summary>
    ''' returns the default connection used in the web application. This is set in the web config as a connection string called either DTIConnection or ConnectionString.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("returns the default connection used in the web application. This is set in the web config as a connection string called either DTIConnection or ConnectionString.")> _
    Public Property defaultConnection() As System.Data.Common.DbConnection
        Get
            Return Data.defaultConnection()
        End Get
        Set(ByVal Value As System.Data.Common.DbConnection)
            Data.defaultConnection = Value
        End Set
    End Property

    ''' <summary>
    ''' Convience method to <see cref="BaseHelper.FillDataSet">BaseHelper.FillDataSet</see>
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="tableName"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convience method to <see cref=""BaseHelper.FillDataSet"">BaseHelper.FillDataSet</see>")> _
    Public Sub FillDataSet(ByVal command As String, ByVal tableName As String, ByVal ds As DataSet)
        sqlHelper.FillDataSet(command, ds, tableName)
    End Sub

    ''' <summary>
    ''' Convience method to <see cref="BaseHelper.FillDataTable">BaseHelper.FillDataTable</see>
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="table"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Convience method to <see cref=""BaseHelper.FillDataTable"">BaseHelper.FillDataTable</see>")> _
    Public Sub FillDataTable(ByVal command As String, ByVal table As DataTable)
        sqlHelper.FillDataTable(command, table)
    End Sub

    ''' <summary>
    ''' Returns the object or nothing if it is dbnull.value
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the object or nothing if it is dbnull.value")> _
    Public Shared Function getValue(ByVal o As Object) As Object
        If o Is DBNull.Value Then
            Return Nothing
        Else
            Return o
        End If
    End Function

    ''' <summary>
    ''' Returns the object or "" if it is null or dbnull.value 
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the object or """" if it is null or dbnull.value")> _
    Public Shared Function getString(ByVal o As Object) As Object
        If o Is DBNull.Value OrElse o Is Nothing Then
            Return ""
        Else
            Return o
        End If
    End Function

#End Region

    ''' <summary>
    ''' Returns the current error handler for this application.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Returns the current error handler for this application.")> _
    Public ReadOnly Property errorHandler() As ErrorHandlerBase
        Get
            Return Application.Get("ErrorHandler")
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        If Not Data Is Nothing Then Me.sqlHelper.defaultConnection = Data.sqlHelper.defaultConnection
    End Sub
End Class
