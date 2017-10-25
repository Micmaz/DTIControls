Imports System.Web.UI
Imports System.Reflection
Imports System.Web.Caching

Public Module PageCache

#Region "Page Level Object Cache"

    '<Extension()> _
    Public Function getPageCache(ByRef c As Control, ByVal key As Object) As Object
        Return pageHT(c)(key)
    End Function
    '<Extension()> _
    Public Sub setPageCache(ByRef c As Control, ByVal key As Object, ByVal value As Object)
        pageHT(c)(key) = value
    End Sub

    '<Extension()> _
    Private Function pageHT(ByRef c As Control) As Hashtable
        If c.Page.Cache(pageKey(c)) Is Nothing Then
            c.Page.Cache.Insert(pageKey(c), New Hashtable, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10))
        End If
        Return c.Page.Cache(pageKey(c))
    End Function

    '<Extension()> _
    Public Function refViewstate(ByRef c As Control) As StateBag
        Return runM(c, "ViewState")
    End Function

    Public Function runM(ByVal o As Object, ByVal methodname As String, ByVal ParamArray args() As Object) As Object
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

    '<Extension()> _
    Public Function pageKey(ByRef c As Control) As String
        If refViewstate(c)("pageKey") Is Nothing Then
            refViewstate(c)("pageKey") = Guid.NewGuid.ToString
        End If
        Return refViewstate(c)("pageKey")
    End Function

#End Region

End Module
