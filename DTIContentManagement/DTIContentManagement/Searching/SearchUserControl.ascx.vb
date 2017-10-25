#If DEBUG Then
Partial Public Class SearchUserControl
    Inherits System.Web.UI.UserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class SearchUserControl
        Inherits System.Web.UI.UserControl
#End If
        Private searchResults As SearchResultsUserControl

        Private _searchTextBoxClientId As String = ""
        Public Property SearchTextBoxClientId() As String
            Get
                Return _searchTextBoxClientId
            End Get
            Set(ByVal value As String)
                _searchTextBoxClientId = value
            End Set
        End Property

        Private _searhcButtonClientId As String = ""
        Public Property SearchButtonClientId() As String
            Get
                Return _searhcButtonClientId
            End Get
            Set(ByVal value As String)
                _searhcButtonClientId = value
            End Set
        End Property


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            hsSearch.DisplayText = "<img style='border-width:0px;' src='" & BaseClasses.Scripts.ScriptsURL & "/DTIContentManagement/zoom.png' border='0' />"
            searchResults = DirectCast(Page.LoadControl("~/res/DTIContentManagement/SearchResultsUserControl.ascx"), SearchResultsUserControl)
            hsSearch.ContentControls.Add(searchResults)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            _searhcButtonClientId = searchResults.SearchButtonId
            _searchTextBoxClientId = searchResults.SearchTextBoxId
        End Sub
    End Class