Imports System.Web.UI.WebControls

#If DEBUG Then
Partial Public Class ajaxpg
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ajaxpg
        Inherits BaseClasses.BaseSecurityPage
#End If

        Public ReadOnly Property ajaxId() As String
            Get
                Return Request.Params("id")
            End Get
        End Property

        Public ReadOnly Property action() As String
            Get
                Return Request.Params("action")
            End Get
        End Property

        Public ReadOnly Property workerClass() As String
            Get
                Return Session(ajaxId)
            End Get
        End Property

        Public ReadOnly Property ResultPanel() As Panel
            Get
                Return result
            End Get
        End Property

        Private ht As Hashtable
        Public ReadOnly Property input() As Object
            Get
                If Not Request.Params("input") Is Nothing Then Return (Request.Params("input"))
                If ht Is Nothing Then
                    ht = New Hashtable
                    For Each parm As String In Request.Params
                        If parm.StartsWith("input[") Then
                            Dim key As String = parm.Substring(6, parm.Length - 7)
                            ht.Add(key, Request.Params(parm))
                        End If
                    Next
                End If
                If ht.Count = 0 Then Return Nothing
                Return ht
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ajaxresponce()
        End Sub

        'Public ReadOnly Property input() As String
        '    Get
        '        Return Request.Params("input")
        '    End Get
        'End Property

        'Private Sub jsonpg_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        '    ajaxresponce()
        'End Sub

        Public Overridable Sub ajaxresponce()
            If ajaxId Is Nothing OrElse workerClass Is Nothing Then Response.End()
            Dim classnm As String = workerClass '.Replace("+", ".")
            Dim obj As jsonWorker = BaseClasses.AssemblyLoader.CreateInstance(classnm)
            If obj Is Nothing Then
                Debug.WriteLine("Could not find the class: " & classnm)
                Throw New UnfoundActionMethod("Could not find the class: " & classnm, New Exception)
            Else
                Dim parms() As Object = New Object() {input}
                obj.sqlHelper = Me.sqlHelper
                obj.Session = Me.Session
                obj.ajaxID = ajaxId
                obj.Page = Me
                Dim meth As System.Reflection.MethodInfo = obj.GetType().GetMethod(action)
                If meth Is Nothing Then
                    Debug.WriteLine("There was no method in called: " & action & " in class: " & workerClass)
                    Throw New UnfoundActionMethod("There was no method in called: " & action & " in class: " & workerClass, New Exception)
                End If

                Dim ret As Object
                Try
                    ret = obj.GetType().GetMethod(action).Invoke(obj, parms)
                Catch ex2 As System.Reflection.TargetParameterCountException
                    Try
                        ret = obj.GetType().GetMethod(action).Invoke(obj, New Object() {})
                        If ret Is Nothing Then ret = ""
                    Catch ex3 As System.ArgumentException
                        Debug.WriteLine("There was no method in called: " & action & " in class: " & workerClass & "with calling types:" & input.GetType.Name)
                        Debug.WriteLine(ex3.Message & ex3.StackTrace)
                        Throw New UnfoundActionMethod("There was no method in called: " & action & " in class: " & workerClass & "with calling types:" & input.GetType.Name, ex3)
                    End Try
                Catch ex As System.ArgumentException
                    Debug.WriteLine("There was no method in called: " & action & " in class: " & workerClass & "with calling types:" & input.GetType.Name)
                    Debug.WriteLine(ex.Message & ex.StackTrace)
                    Throw New UnfoundActionMethod("There was no method in called: " & action & " in class: " & workerClass & "with calling types:" & input.GetType.Name, ex)
                End Try
            End If

        End Sub

        Public Class UnfoundActionMethod
            Inherits Exception

            Public Sub New(ByVal message As String, ByVal innerex As Exception)
                MyBase.New(message, innerex)
            End Sub
        End Class

    End Class