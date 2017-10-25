<ComponentModel.ToolboxItem(False)> _
Public Class TreeListUL
    Inherits Panel
    Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Ul
        End Get
    End Property

    Public Function addListItem(ByVal text As String, ByVal value As String, Optional ByVal nodeType As String = Nothing, Optional ByVal className As String = Nothing) As TreeListItem
        Dim newguy As TreeListItem = New TreeListItem(text, value, nodeType, className)
        Controls.Add(newguy)
        Return newguy
    End Function

    Public Shadows Sub addListItemRoot(ByRef li As TreeListItem)
        Controls.Add(li)
    End Sub
End Class