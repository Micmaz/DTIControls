Public Class OverrideItem
    Inherits MenuItem

#Region "properties"


    Private _overrideTemplate As Boolean = False

    ''' <summary>
    ''' Allows this specific menuitem to be rendered differently based on it's Display name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Allows this specific menuitem to be rendered differently based on it's Display name")> _
    Public Property overrideTemplate() As Boolean
        Get
            Return _overrideTemplate
        End Get
        Set(ByVal value As Boolean)
            _overrideTemplate = value
        End Set
    End Property

    Private _name As String = ""

    ''' <summary>
    ''' Name of the page and display of menu item to add to data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Name of the page and display of menu item to add to data")> _
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private Property ds() As dsDTIAdminPanel
        Get
            If Page.Session("DTIAdminPanel.Pagelist") Is Nothing Then
                Page.Session("DTIAdminPanel.Pagelist") = New dsDTIAdminPanel
            End If
            Return Page.Session("DTIAdminPanel.Pagelist")
        End Get
        Set(ByVal value As dsDTIAdminPanel)
            Page.Session("DTIAdminPanel.Pagelist") = value
        End Set
    End Property
#End Region

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Sub SaveOverridableInfo(ByVal item As OverrideItem)
        Dim dv As New DataView(ds.DTIPageHeiarchy, "DisplayName = '" & item.Name & "'", "id asc", DataViewRowState.CurrentRows)
        Dim itemId As Integer = -1

        If Not dv.Count = 0 Then
            itemId = dv(0).Item("Id")
            Dim ht As Hashtable = CType(BaseClasses.Spider.spiderUpforType(Me, GetType(Menu)), Menu).ItemToData
            If Not ht.ContainsKey(itemId) Then
                ht.Add(itemId, item)
            End If
        End If
    End Sub
End Class
