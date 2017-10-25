<ComponentModel.ToolboxItem(False)> _
Public Partial Class postback
    Inherits BaseClasses.BaseSecurityPage

    Public ReadOnly Property query() As String
        Get
            Return Request.QueryString("q")
        End Get
    End Property

    Public Sub respond(ByVal dt As DataTable, ByVal nameColumn As String, Optional ByVal valuecolumn As String = Nothing)
        Dim outstr As String = ""
        For Each row As DataRow In dt.Rows
            If Not valuecolumn Is Nothing Then
                outstr &= row(nameColumn) & "|" & row(valuecolumn) & vbCrLf
            Else
                outstr &= row(nameColumn) & vbCrLf
            End If
        Next
        Response.Write(outstr)
        Response.End()
    End Sub

End Class