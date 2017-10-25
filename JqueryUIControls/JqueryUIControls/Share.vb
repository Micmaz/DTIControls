Public Module Share
    Friend Function jsPropString(ByVal membername As String, ByVal value As Object) As String
        If value Is Nothing Then Return ""
        'If value = Nothing Then Return ""
        Dim out As String = membername & ": "
        If GetType(String).IsAssignableFrom(value.GetType) Then
            out &= "'" & BaseClasses.BaseSecurityPage.JavaScriptEncode(value) & "'"
        ElseIf GetType(System.Drawing.Color).IsAssignableFrom(value.GetType) Then
            If value = Nothing Then Return ""
            out &= "'" & System.Drawing.ColorTranslator.ToHtml(value) & "'"
        ElseIf GetType(Boolean).IsAssignableFrom(value.GetType) Then
            out &= value.ToString.ToLower
        ElseIf GetType(Date).IsAssignableFrom(value.GetType) Then
            If value = Nothing Then Return ""
            Dim dt As Date = value
            out &= String.Format("new Date ({0},{1},{2},{3},{4})", dt.Year, dt.Month - 1, dt.Day, dt.Hour, dt.Minute, dt.Second)
        ElseIf IsNumeric(value) Then
            out &= value
        Else
            If value = Nothing Then Return ""
            out &= value
        End If
        Return out & ","
    End Function

    Friend Function jsonPropString(ByVal membername As String, ByVal value As Object) As String
        If value Is Nothing Then Return ""
        'If value = Nothing Then Return ""
        Dim q As String = """"
        Dim out As String = q & membername & q & ":"
        If GetType(String).IsAssignableFrom(value.GetType) Then
            out &= q & value.ToString.Replace(q, "\" & q) & q
        ElseIf GetType(System.Drawing.Color).IsAssignableFrom(value.GetType) Then
            If value = Nothing Then Return ""
            out &= q & System.Drawing.ColorTranslator.ToHtml(value) & q
        ElseIf GetType(Boolean).IsAssignableFrom(value.GetType) Then
            out &= q & value.ToString.ToLower & q
        ElseIf GetType(Date).IsAssignableFrom(value.GetType) Then
            If value = Nothing Then Return ""
            Dim dt As Date = value
            out &= q & String.Format("{0}-{1}-{2} {3}:{4}:{5}", dt.Year, z(dt.Month), z(dt.Day), z(dt.Hour), z(dt.Minute), z(dt.Second)) & q
        ElseIf IsNumeric(value) Then
            out &= value
        Else
            If value = Nothing Then Return ""
            out &= q & value & q
        End If
        Return out & ","
    End Function

    Private Function z(ByVal val As Integer) As String
        If val < 10 Then
            Return "0" & val
        End If
        Return val
    End Function

    Friend Function jsPropFunctionString(ByVal membername As String, ByVal jsFunctionName As String)
        If jsFunctionName Is Nothing OrElse jsFunctionName.Trim = "" Then Return ""
        Dim out As String = membername & ": "
        If jsFunctionName.Trim().StartsWith("function(") Then
            out &= jsFunctionName
        ElseIf jsFunctionName.Trim().EndsWith(";") Then
            out &= "function(){" & jsFunctionName & "}"
        Else
            out &= jsFunctionName
        End If
        Return out & ","
    End Function

    Public Sub BindToEnum(ByVal enumType As Type, ByVal lc As ListControl)
        ' get the names from the enumeration
        Dim names As String() = [Enum].GetNames(enumType)
        ' get the values from the enumeration
        Dim values As Array = [Enum].GetValues(enumType)
        ' turn it into a hash table
        Dim ht As New Hashtable()
        For i As Integer = 0 To names.Length - 1
            ' note the cast to integer here is important
            ' otherwise we'll just get the enum string back again
            ht.Add(names(i), CInt(values.GetValue(i)))
        Next
        ' return the dictionary to be bound to
        lc.DataSource = ht
        lc.DataTextField = "Key"
        lc.DataValueField = "Value"
        lc.DataBind()
    End Sub

End Module
