Imports System.Reflection

''' <summary>
''' Utility class to load and cache assemblies. Caching is done in BaseVirtualPathProvider.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Utility class to load and cache assemblies. Caching is done in BaseVirtualPathProvider.")> _
Public Class AssemblyLoader

    Public Shared Function CreateInstance(ByVal classname As String, Optional ByVal isretry As Boolean = False, Optional ByVal baseType As Type = Nothing) As Object
        Dim obj As Object = Nothing
        classname = classname.Trim
        '    If Not BaseVirtualPathProvider.assemblyClassHash.ContainsKey(classname) Then
        '        Try
        '            Dim asmName As String = classname
        '            If classname.indexof(".") > -1 Then 
        '                obj = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(classname.Substring(0, classname.IndexOf(".")), True)
        'end if
        '            If obj IsNot Nothing Then
        '                Return obj
        '            End If
        '        Catch ex As Exception
        '        End Try
        '    End If

        If Not BaseVirtualPathProvider.assemblyClassHash.ContainsKey(classname) Then
        For i As Integer = 0 To assemblies.Count - 1
            Dim asm As Assembly = assemblies(i)
            'Try
            obj = asm.CreateInstance(classname, True)
            If Not obj Is Nothing Then
                If Not baseType Is Nothing Then
                    If obj.GetType.IsSubclassOf(baseType) Then
                        BaseVirtualPathProvider.assemblyClassHash.Add(classname, asm)
                        Exit For
                    End If
                Else
                    BaseVirtualPathProvider.assemblyClassHash.Add(classname, asm)
                    Exit For
                End If
            End If
            'Catch ex As Exception

            'End Try
            Next
        Try
            If Not isretry Then
                BaseVirtualPathProvider.buildLocalResources(classname.Substring(0, classname.IndexOf(".")))
                Return CreateInstance(classname, True)
            End If

        Catch ex As Exception

        End Try

        Return obj
        Else
			Return BaseVirtualPathProvider.assemblyClassHash(classname).CreateInstance(classname, True)
        End If

    End Function

    Public Shared ReadOnly Property assemblies() As Generic.List(Of Assembly)
        Get
            Return BaseVirtualPathProvider.assemblies
        End Get
    End Property

    Public Shared Function runM(ByVal o As Object, ByVal methodname As String, ByVal ParamArray args() As Object) As Object
        Dim method As System.Reflection.MethodInfo = o.GetType().GetMethod(methodname, BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.IgnoreCase)
        If Not method Is Nothing Then
            Return method.Invoke(o, args)
        Else
            Dim prop As System.Reflection.PropertyInfo = o.GetType().GetProperty(methodname, BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.IgnoreCase)
            If Not prop Is Nothing Then
                If args.Length = prop.GetIndexParameters().Length Then
                    Return prop.GetValue(o, args)
                Else
                    Dim args2() As Object = New Object() {}
                    If args.Length > 1 Then
                        args.CopyTo(args2, 1)
                    Else
                        args2 = Nothing
                    End If
                    prop.SetValue(o, args(0), args2)
                    Return Nothing
                End If
            End If
        End If
        Return Nothing
    End Function

End Class


