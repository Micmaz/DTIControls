Public Class TreeListItemsCollection
    Inherits System.Collections.ObjectModel.Collection(Of TreeListItem)

    Public Function Find(ByVal value As Integer) As TreeListItem
        Dim it As TreeListItem = Nothing
        For Each itm As TreeListItem In Me
            it = Find(value, itm)
            If it IsNot Nothing Then Exit For
        Next
        Return it
    End Function

    Private Function Find(ByVal value As Integer, ByVal itm As TreeListItem) As TreeListItem
        Dim it As TreeListItem = Nothing
        If itm.Value = value Then
            it = itm
        Else
            For Each i As TreeListItem In itm.Items
                it = Find(value, i)
                If it IsNot Nothing Then Exit For
            Next
        End If
        Return it
    End Function

    'Public Function Find(ByVal value As Integer) As TreeListItem
    '    For Each itm As TreeListItem In Me
    '        If itm.Value = value Then Return itm
    '    Next
    '    Return Nothing
    'End Function

    'Private Function Find(ByVal value As Integer, ByVal itm As TreeListItem) As TreeListItem
    '    If itm.Value = value Then Return itm
    '    For Each i As TreeListItem In itm.Items
    '        Find(value, i)
    '    Next
    '    Return Nothing
    'End Function
End Class
