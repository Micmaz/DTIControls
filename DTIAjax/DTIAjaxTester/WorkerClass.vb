Public Class WorkerClass
    Inherits DTIAjax.jsonWorker

    Public Function returnStr(ByVal input As String) As String
        Return "You entered String: " & input
    End Function

    Public Function returnStr2(ByVal input As Hashtable) As String
        Return "Did IT 2!"
    End Function

    Public Function returnStuff(ByVal input As Hashtable) As Hashtable
        Dim ht As New Hashtable
        ht("stuff") = "more stuff"
        Return ht
    End Function

    Shared a As Integer = 0
    Public Function doit(ByVal data As String) As String
        a += 1
        If a > 5 Then
            a = 0
            Return "YAY!"
        End If
        Return ""
    End Function

    Public Function returnHTML() As String
        Dim retList As New PlaceHolder

        Dim retButton As New Button
        retButton.Text = "Submit"

        Dim lblTest As New Label
        lblTest.Text = "BLAAAAH"

        retList.Controls.Add(retButton)
        retList.Controls.Add(lblTest)

        Return "ASDASDASD"

    End Function


    Public Function returnHTMLWithString(ByVal input As Hashtable) As ControlCollection
        Dim retList As New PlaceHolder

        Dim retButton As New Button
        retButton.Text = input("some")

        Dim lblTest As New Label
        lblTest.Text = input("another")

        retList.Controls.Add(retButton)
        retList.Controls.Add(lblTest)

        Return retList.Controls

    End Function

    Public Function returnHTMLWithString1(ByVal input As String) As ControlCollection
        Dim retList As New PlaceHolder

        Dim retButton As New Button
        retButton.Text = "Submit"

        Dim lblTest As New Label
        lblTest.Text = input

        retList.Controls.Add(lblTest)

        Return retList.Controls

    End Function

End Class
