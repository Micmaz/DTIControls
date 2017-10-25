#Region "Includes"
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Reflection
Imports System.Collections
#End Region

Public Class Comparator

    Private _parents As New List(Of Object)()
    Private obj1hashtable As New List(Of Object)()
    Private obj2hashtable As New List(Of Object)()


#Region "Properties"

    Private _objectName As String = ""
    Public Property objectName() As String
        Get
            Return _objectName
        End Get
        Set(ByVal value As String)
            _objectName = value
        End Set
    End Property

    Private _maxDifferences As Integer = 100
    Public Property MaxDifferences() As Integer
        Get
            Return _maxDifferences
        End Get
        Set(ByVal value As Integer)
            _maxDifferences = value
        End Set
    End Property

    Private _excludeList As New Generic.List(Of String)
    Public ReadOnly Property excludeList() As Generic.List(Of String)
        Get
            Return _excludeList
        End Get
    End Property

    Public Sub addExclude(ByVal excludePropertyNames As String)
        For Each excludePropertyName As String In excludePropertyNames.Split(",")
            excludeList.Add(excludePropertyName.ToLower)
        Next
    End Sub

    Private _maxDepth As Integer = -1
    Public Property MaxDepth() As Integer
        Get
            Return _maxDepth
        End Get
        Set(ByVal value As Integer)
            _maxDepth = value
        End Set
    End Property

    Private _differences As New dsDTIControls.DTIControlPropertyDataTable
    Public Property differences() As dsDTIControls.DTIControlPropertyDataTable
        Get
            Return _differences
        End Get
        Set(ByVal value As dsDTIControls.DTIControlPropertyDataTable)
            _differences = value
        End Set
    End Property

    Private _DTIControlRow As dsDTIControls.DTIControlRow
    Public Property DTIControlRow() As dsDTIControls.DTIControlRow
        Get
            Return _DTIControlRow
        End Get
        Set(ByVal value As dsDTIControls.DTIControlRow)
            _DTIControlRow = value
            _differences = value.Table.DataSet.Tables("DTIControlProperty")
        End Set
    End Property

#End Region

#Region "Shared methods"

    Public Shared Function desearializeFromXMLString(ByVal row As dsDTIControls.DTIControlPropertyRow) As Object
        Try
            Return desearializeFromXMLString(row.PropertyValue, row.PropertyType)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Shared Function desearializeFromXMLString(ByVal xml As String, ByVal typename As String) As Object
        Try
            Dim x As New System.Xml.Serialization.XmlSerializer(Type.GetType(typename))
            Dim strm As New System.IO.StringReader(xml)
            Dim ret As Object = x.Deserialize(strm)
            Return ret
        Catch ex As Exception

        End Try
        Return Nothing
    End Function
    Public Shared Function searializeToXMLString(ByVal obj As Object) As String
        Try
            Dim x As New System.Xml.Serialization.XmlSerializer(obj.GetType)
            Dim strm As New System.IO.StringWriter
            x.Serialize(strm, obj)
            Return strm.ToString
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Public Shared Function desearializeFromBase64String(ByVal row As dsDTIControls.DTIControlPropertyRow) As Object
        Try
            Return desearializeFromBase64String(row.PropertyValue)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Shared Function desearializeFromBase64String(ByVal obj As String) As Object
        Try
            Using stream As New System.IO.MemoryStream(Convert.FromBase64String(obj))
                Dim formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                Return formatter.Deserialize(stream)
            End Using
        Catch
        End Try
        Return Nothing
    End Function
    Public Shared Function searializeToBase64String(ByVal obj As Object) As String
        Try
            Using stream As New System.IO.MemoryStream()
                Dim formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                formatter.Serialize(stream, obj)
                Return Convert.ToBase64String(stream.ToArray())
            End Using
        Catch
        End Try
        Return Nothing
    End Function

    Public Shared Function CompareObjects(ByVal obj1 As Object, ByVal obj2 As Object, Optional ByRef maximumDifferences As Integer = 100) As Comparator
        Dim c As New Comparator
        c.Compare(obj1, obj2)
        Return c
    End Function


    Public Shared Sub setProperties(ByVal dt As dsDTIControls.DTIControlPropertyDataTable, ByRef instance As Object, Optional ByRef session As System.Web.SessionState.HttpSessionState = Nothing)
        Dim propc As PropertyCreator = getPropertyCreator(session)

        For Each row As dsDTIControls.DTIControlPropertyRow In dt
            Try
                'setProperty(row, instance)
                propc.SetValue(instance, row.PropertyPath, desearializeFromBase64String(row))
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Shared Sub setProperties(ByVal dv As DataView, ByRef instance As Object, Optional ByRef session As System.Web.SessionState.HttpSessionState = Nothing)
        Dim propc As PropertyCreator = getPropertyCreator(session)

        For Each rv As DataRowView In dv
            Dim row As dsDTIControls.DTIControlPropertyRow = rv.Row
            Try
                'setProperty(row, instance)
                propc.SetValue(instance, row.PropertyPath, desearializeFromBase64String(row))
            Catch ex As Exception

            End Try
        Next
    End Sub


    '    Public Shared Sub setProperty(ByVal row As ComparatorDS.DifferencesRow, ByRef instance As Object, Optional ByVal session As System.Web.SessionState.HttpSessionState = Nothing)
    '        setProperty(row.PropertyPath, desearializeFromBase64String(row), instance)
    '    End Sub


    '    Public Shared Sub setProperty(ByVal propertyPath As String, ByVal newval As Object, ByRef instance As Object, Optional ByVal session As System.Web.SessionState.HttpSessionState = Nothing)
    '        Dim propc As PropertyCreator = getPropertyCreator(session)
    'propc.SetValue(instance,

    '    End Sub

    Private Shared Function getPropertyCreator(ByVal session As System.Web.SessionState.HttpSessionState) As PropertyCreator
        If session Is Nothing Then
            Return New PropertyCreator
        End If
        If session("propertyCreatorCache") Is Nothing Then
            session("propertyCreatorCache") = New PropertyCreator
        End If
        Return session("propertyCreatorCache")
    End Function


    'Public Shared Sub SetProperties(ByVal fromFields As PropertyInfo(), ByVal toFields As PropertyInfo(), ByVal fromRecord As Object, ByVal toRecord As Object)
    '    Dim fromField As PropertyInfo = Nothing
    '    Dim toField As PropertyInfo = Nothing
    '    Try
    '        If fromFields Is Nothing Then Exit Sub
    '        If toFields Is Nothing Then Exit Sub
    '        For f As Integer = 0 To fromFields.Length - 1
    '            fromField = DirectCast(fromFields(f), PropertyInfo)
    '            For t As Integer = 0 To toFields.Length - 1
    '                toField = DirectCast(toFields(t), PropertyInfo)
    '                If fromField.Name <> toField.Name Then
    '                    Continue For
    '                End If
    '                toField.SetValue(toRecord, fromField.GetValue(fromRecord, Nothing), Nothing)
    '                Exit For
    '            Next
    '        Next
    '    Catch generatedExceptionName As Exception
    '        Throw
    '    End Try
    'End Sub

    'Public Shared Function SetProperties(Of T)(ByVal fromRecord As T, ByVal toRecord As T) As T
    '    For Each field As PropertyInfo In GetType(T).GetProperties()
    '        field.SetValue(toRecord, field.GetValue(fromRecord, Nothing), Nothing)
    '    Next
    '    Return toRecord
    'End Function

    'Public Shared Function SetProperties(Of T, U)(ByVal fromRecord As U, ByVal toRecord As T) As T
    '    For Each fromField As PropertyInfo In GetType(U).GetProperties()
    '        If fromField.Name <> "Id" Then
    '            For Each toField As PropertyInfo In GetType(T).GetProperties()
    '                If fromField.Name = toField.Name Then
    '                    toField.SetValue(toRecord, fromField.GetValue(fromRecord, Nothing), Nothing)
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '    Next
    '    Return toRecord
    'End Function

#End Region

#Region "Public Methods"

    Public Sub New(ByVal objName As String)
        Me.objectName = objName
    End Sub

    Public Sub New()
    End Sub

    Public Function Compare(ByVal object1 As Object, ByVal object2 As Object) As Boolean
        Dim defaultBreadCrumb As String = String.Empty

        'differences.Clear()
        For Each row As DataRow In differences
            row.Delete()
        Next
        Compare(object1, object2, defaultBreadCrumb)

        Return differences.Count = 0
    End Function

    Public Function DiffList() As String
        Dim str As String = ""
        For Each row As dsDTIControls.DTIControlPropertyRow In differences
            Dim val As Object = desearializeFromXMLString(row)
            If val Is Nothing Then val = "{null}"
            str &= row.PropertyPath & " : " & val.ToString & vbCrLf
        Next
        Return str
    End Function

#End Region

#Region "Helper Methods"

    Private CurrentDepth As Integer = 0
    Private Sub Compare(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        If MaxDepth > -1 AndAlso MaxDepth < CurrentDepth Then
            Exit Sub
        End If
        CurrentDepth += 1
        'If both null return true
        If object1 Is Nothing AndAlso object2 Is Nothing Then
            CurrentDepth -= 1
            Exit Sub
        End If

        If object1 Is Nothing OrElse object2 Is Nothing Then
            addDifference(breadCrumb, object2)
            CurrentDepth -= 1
            Exit Sub
        End If


        Dim t1 As Type = object1.[GetType]()
        Dim t2 As Type = object2.[GetType]()

        If IsSimpleType(t1) Then
            CompareSimpleType(object1, object2, breadCrumb)
        ElseIf IsTimespan(t1) Then
            CompareTimespan(object1, object2, breadCrumb)
        ElseIf IsStruct(t1) Then
            CompareStruct(object1, object2, breadCrumb)
        Else

            If obj1hashtable.Contains(object1) Then
                CurrentDepth -= 1
                Exit Sub
            End If

            If obj2hashtable.Contains(object2) Then
                CurrentDepth -= 1
                Exit Sub
            End If

            obj1hashtable.Add(object1)
            obj2hashtable.Add(object2)


            'Objects must be the same type
            If t1 IsNot t2 Then
                addDifference(breadCrumb, object2)
                CurrentDepth -= 1
                Exit Sub
            End If

            If IsIList(t1) Then
                'This will do arrays, multi-dimensional arrays and generic lists
                CompareIList(object1, object2, breadCrumb)
            ElseIf IsIDictionary(t1) Then
                CompareIDictionary(object1, object2, breadCrumb)
            ElseIf IsEnum(t1) Then
                CompareEnum(object1, object2, breadCrumb)
            ElseIf IsClass(t1) Then
                CompareClass(object1, object2, breadCrumb)
            Else
                Throw New NotImplementedException("Cannot compare object of type " & t1.Name)

            End If
        End If

        CurrentDepth -= 1
    End Sub

#Region "Type Checks"

    Private Function IsTimespan(ByVal t As Type) As Boolean
        Return t Is GetType(TimeSpan)
    End Function

    Private Function IsEnum(ByVal t As Type) As Boolean
        Return t.IsEnum
    End Function

    Private Function IsStruct(ByVal t As Type) As Boolean
        Return t.IsValueType
    End Function

    Private Function IsSimpleType(ByVal t As Type) As Boolean

        Return t.IsPrimitive OrElse t Is GetType(DateTime) OrElse t Is GetType(Decimal) OrElse t Is GetType(String) OrElse t Is GetType(Guid)
    End Function

    Private Function ValidStructSubType(ByVal t As Type) As Boolean
        Return IsSimpleType(t) OrElse IsEnum(t) OrElse IsArray(t) OrElse IsClass(t) OrElse IsIDictionary(t) OrElse IsTimespan(t) OrElse IsIList(t)
    End Function

    Private Function IsArray(ByVal t As Type) As Boolean
        Return t.IsArray
    End Function

    Private Function IsClass(ByVal t As Type) As Boolean
        Return t.IsClass
    End Function

    Private Function IsIDictionary(ByVal t As Type) As Boolean
        Return t.GetInterface("System.Collections.IDictionary", True) IsNot Nothing
    End Function

    Private Function IsIList(ByVal t As Type) As Boolean
        Return t.GetInterface("System.Collections.IList", True) IsNot Nothing
    End Function

#End Region

#Region "Comparators"

    Private Sub CompareTimespan(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        If DirectCast(object1, TimeSpan).Ticks <> DirectCast(object2, TimeSpan).Ticks Then
            addDifference(breadCrumb, object2)
        End If
    End Sub

    Private Sub CompareEnum(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        If object1.ToString() <> object2.ToString() Then
            Dim currentBreadCrumb As String = AddBreadCrumb(breadCrumb, object1.[GetType]().Name, String.Empty, -1)
            addDifference(currentBreadCrumb, object2)
        End If
    End Sub

    Private Sub CompareSimpleType(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        Dim valOne As IComparable = TryCast(object1, IComparable)
        If valOne.CompareTo(object2) <> 0 Then
            addDifference(breadCrumb, object2)
        End If
    End Sub

    Private Sub CompareStruct(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        Dim currentCrumb As String
        Dim t1 As Type = object1.[GetType]()

        'Compare the fields
        Dim currentFields As FieldInfo() = t1.GetFields()
        If currentFields.Length > 1 Then
            For Each item As FieldInfo In currentFields
                'Only compare simple types within structs (Recursion Problems)
                If Not ValidStructSubType(item.FieldType) Then
                    Continue For
                End If
                currentCrumb = AddBreadCrumb(breadCrumb, item.Name, String.Empty, -1)
                Compare(item.GetValue(object1), item.GetValue(object2), currentCrumb)
                If differences.Count >= MaxDifferences Then
                    Exit Sub
                End If
            Next
        Else
            Try
                If Not object1 = object2 Then
                    addDifference(breadCrumb, object2)
                End If
            Catch ex As Exception

            End Try
        End If

    End Sub

    Private Sub CompareClass(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        Dim currentCrumb As String
        Dim objectValue1 As Object
        Dim objectValue2 As Object

        Try
            _parents.Add(object1)
            _parents.Add(object2)
            Dim t1 As Type = object1.[GetType]()

            'Compare the properties
            Dim currentProperties As PropertyInfo() = t1.GetProperties()
            'PropertyInfo[] currentProperties = t1.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
            Dim properties As ComponentModel.PropertyDescriptorCollection = ComponentModel.TypeDescriptor.GetProperties(object1)

            'For Each info As PropertyInfo In currentProperties
            For Each info As ComponentModel.PropertyDescriptor In properties
                Try
                    If info.IsBrowsable = False OrElse excludeList.Contains(info.Name.ToLower) Then
                        Continue For
                    End If

                    objectValue1 = info.GetValue(object1)
                    objectValue2 = info.GetValue(object2)

                    Dim object1IsParent As Boolean = objectValue1 IsNot Nothing AndAlso (objectValue1 Is object1 OrElse _parents.Contains(objectValue1))
                    Dim object2IsParent As Boolean = objectValue2 IsNot Nothing AndAlso (objectValue2 Is object2 OrElse _parents.Contains(objectValue2))

                    'Skip properties where both point to the corresponding parent
                    If IsClass(info.PropertyType) AndAlso (object1IsParent AndAlso object2IsParent) Then
                        Continue For
                    End If

                    currentCrumb = AddBreadCrumb(breadCrumb, info.Name, String.Empty, -1)

                    Compare(objectValue1, objectValue2, currentCrumb)

                    If differences.Count >= MaxDifferences Then
                        Exit Sub
                    End If
                Catch ex As Exception

                End Try
            Next

            Dim currentFields As FieldInfo() = t1.GetFields()
            'FieldInfo[] currentFields = t1.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

            For Each item As FieldInfo In currentFields
                objectValue1 = item.GetValue(object1)
                objectValue2 = item.GetValue(object2)
                Dim object1IsParent As Boolean = False
                Dim object2IsParent As Boolean = False

                Try
                    object1IsParent = objectValue1 IsNot Nothing AndAlso (objectValue1 = object1 OrElse _parents.Contains(objectValue1))
                Catch ex As Exception

                End Try
                Try
                    object2IsParent = objectValue2 IsNot Nothing AndAlso (objectValue2 = object2 OrElse _parents.Contains(objectValue2))
                Catch ex As Exception

                End Try

                'Skip fields that point to the parent
                If IsClass(item.FieldType) AndAlso (object1IsParent OrElse object2IsParent) Then
                    Continue For
                End If

                currentCrumb = AddBreadCrumb(breadCrumb, item.Name, String.Empty, -1)

                Compare(objectValue1, objectValue2, currentCrumb)

                If differences.Count >= MaxDifferences Then
                    Exit Sub
                End If

            Next
        Finally
            _parents.Remove(object1)
            _parents.Remove(object2)
        End Try
    End Sub


    Private Sub CompareIDictionary(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        Dim iDict1 As IDictionary = TryCast(object1, IDictionary)
        Dim iDict2 As IDictionary = TryCast(object2, IDictionary)

        'Objects must be the same length
        If iDict1.Count <> iDict2.Count Then
            addDifference(breadCrumb, object2)

            If differences.Count >= MaxDifferences Then
                Exit Sub
            End If
        End If

        Dim enumerator1 As IDictionaryEnumerator = iDict1.GetEnumerator()
        Dim enumerator2 As IDictionaryEnumerator = iDict2.GetEnumerator()
        Dim count As Integer = 0

        While enumerator1.MoveNext() AndAlso enumerator2.MoveNext()
            Dim currentBreadCrumb As String = AddBreadCrumb(breadCrumb, "Key", String.Empty, -1)

            Compare(enumerator1.Key, enumerator2.Key, currentBreadCrumb)

            If differences.Count >= MaxDifferences Then
                Exit Sub
            End If

            currentBreadCrumb = AddBreadCrumb(breadCrumb, "Value", String.Empty, -1)

            Compare(enumerator1.Value, enumerator2.Value, currentBreadCrumb)

            If differences.Count >= MaxDifferences Then
                Exit Sub
            End If

            count += 1

        End While
    End Sub



    Private Sub CompareIList(ByVal object1 As Object, ByVal object2 As Object, ByRef breadCrumb As String)
        Dim ilist1 As IList = TryCast(object1, IList)
        Dim ilist2 As IList = TryCast(object2, IList)

        'Objects must be the same length
        If ilist1.Count <> ilist2.Count Then
            addDifference(breadCrumb, object2)

            If differences.Count >= MaxDifferences Then
                Exit Sub
            End If
        End If

        Dim enumerator1 As IEnumerator = ilist1.GetEnumerator()
        Dim enumerator2 As IEnumerator = ilist2.GetEnumerator()
        Dim count As Integer = 0

        While enumerator1.MoveNext() AndAlso enumerator2.MoveNext()
            Dim currentBreadCrumb As String = AddBreadCrumb(breadCrumb, String.Empty, String.Empty, count)

            Compare(enumerator1.Current, enumerator2.Current, currentBreadCrumb)

            If differences.Count >= MaxDifferences Then
                Exit Sub
            End If

            count += 1
        End While
    End Sub

#End Region

    Private Function [cStr](ByVal obj As Object) As String
        Try
            If obj Is Nothing Then
                Return "(null)"
            ElseIf obj Is System.DBNull.Value Then
                Return "System.DBNull.Value"
            Else
                Return obj.ToString()
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Public Sub addDifference(ByVal location As String, ByVal value As Object, Optional ByVal oldval As Object = Nothing, Optional ByVal mainid As Integer = 0)
        If location Is Nothing Then Exit Sub
        location = location.Trim(New Char() {"."})
        If value Is Nothing Then
            differences.AddDTIControlPropertyRow(location, Nothing, Nothing, DTIControlRow)
        Else
            Dim base63EncodedValue As String = searializeToBase64String(value)
            If Not base63EncodedValue Is Nothing Then
                differences.AddDTIControlPropertyRow(base63EncodedValue, value.GetType.AssemblyQualifiedName, location, DTIControlRow)
            End If
        End If
    End Sub

    Private Function AddBreadCrumb(ByVal existing As String, ByVal name As String, ByVal extra As String, ByVal index As Integer) As String
        Dim useIndex As Boolean = index >= 0
        Dim useName As Boolean = name.Length > 0
        Dim sb As New StringBuilder()

        sb.Append(existing)

        If useName Then
            sb.AppendFormat(".")
            sb.Append(name)
        End If

        sb.Append(extra)

        If useIndex Then
            sb.AppendFormat("[{0}]", index)
        End If

        Return sb.ToString()
    End Function

#End Region


End Class
