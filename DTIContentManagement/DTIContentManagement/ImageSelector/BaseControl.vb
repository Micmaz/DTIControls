Public Class BaseControl
    Inherits BaseClasses.BaseSecurityUserControl

    'Public Shadows ReadOnly Property Data() As Data
    '    Get
    '        Return MyBase.Data
    '    End Get
    'End Property

    'Private _currentSite As dsSites.SitesRow
    'Public ReadOnly Property currentSite As dsSites.SitesRow
    '    Get
    '        If _currentSite Is Nothing Then
    '            Dim dt As New dsSites.SitesDataTable
    '            _currentSite = dt.AddSitesRow(1, "", "", "", "", "", "", 0, 0, 0, "", 0, False, False, "", "", 0, 0, "", "", Date.Now, True)
    '        End If
    '        Return _currentSite
    '    End Get
    'End Property

    'Public ReadOnly Property dsCurrent As dsSites
    '    Get
    '        Return Data.dsCurrent
    '    End Get
    'End Property

    Private _dsCurrent As New DTIImageManager.dsImageManager
    Public ReadOnly Property dsCurrent As DTIImageManager.dsImageManager
        Get
            Return _dsCurrent
        End Get
    End Property

    Private _mainID As Long = 0
    Public ReadOnly Property MainId() As Long
        Get
            If _mainID = -1 Then
                Long.TryParse(Request.QueryString("m"), _mainID)
            End If
            Return _mainID
        End Get
    End Property

    'Public Shared Function renderTable(ByVal dt As DataTable, Optional ByVal cols() As String = Nothing, Optional ByVal colTitles() As String = Nothing, Optional ByVal Cellprops() As String = Nothing, Optional ByVal includetabletag As Boolean = True, Optional ByVal renderheader As Boolean = True) As String
    '    If cols Is Nothing Then
    '        Dim arr As New List(Of String)
    '        For Each col As DataColumn In dt.Columns
    '            arr.Add(col.ColumnName)
    '        Next
    '        cols = arr.ToArray
    '    End If
    '    If colTitles Is Nothing Then colTitles = New String() {}
    '    If Cellprops Is Nothing Then Cellprops = New String() {}
    '    Dim outstr As String = ""
    '    If includetabletag Then outstr &= "<table>"
    '    Dim i As Integer = 0
    '    If renderheader Then
    '        outstr &= "<tr>"
    '        For Each col As String In cols
    '            Dim title As String = col.Replace("_", " ")
    '            If i < colTitles.Length Then title = colTitles(i)
    '            Dim props As String = ""
    '            If Cellprops.Length > i Then props = Cellprops(i)
    '            outstr &= String.Format("<td class='tablehead' {0}>{1}</td>", props, title)
    '            i += 1
    '        Next
    '        outstr &= "</tr>"
    '    End If

    '    For Each r As DataRow In dt.Rows
    '        i = 0
    '        outstr &= "<tr class='tablerow'>"
    '        For Each col As String In cols
    '            Dim props As String = ""
    '            If Cellprops.Length > i Then props = Cellprops(i)
    '            outstr &= String.Format("<td class='tablecell' {0}>{1}</td>", props, Binding.getValueString(r, col))
    '        Next
    '        outstr &= "</tr>"
    '    Next
    '    If includetabletag Then outstr &= "</table>"
    '    Return outstr
    'End Function

End Class
