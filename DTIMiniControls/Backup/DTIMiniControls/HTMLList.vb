Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class HTMLList
    Inherits Panel
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("OrderList"), _
    ToolboxData("<{0}:HTMLList runat=""server""> </{0}:HTMLList>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class HTMLList
        Inherits Panel
#End If
        Private _orderList As Boolean = False

        ''' <summary>
        ''' Property to get/set OrderList
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' Default Value: False
        ''' </value>
        ''' <returns>
        ''' orderList boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set OrderList")> _
        Public Property OrderList() As Boolean
            Get
                Return _orderList
            End Get
            Set(ByVal value As Boolean)
                _orderList = value
            End Set
        End Property

        Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
            Get
                If OrderList Then
                    Return HtmlTextWriterTag.Ol
                Else : Return HtmlTextWriterTag.Ul
                End If
            End Get
        End Property

        ''' <summary>
        ''' Adds an HTML list item 
        ''' </summary>
        ''' <param name="text">
        ''' Text string for the list item
        ''' </param>
        ''' <param name="className">
        ''' Class name string for the list item
        ''' </param>
        ''' <param name="href">
        ''' href string for the list item
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds an HTML list item")> _
        Public Overridable Sub addListItem(ByVal text As String, Optional ByVal className As String = Nothing, Optional ByVal href As String = Nothing)
            Me.Controls.Add(New HTMLListItem(text, className, href))
        End Sub

        ''' <summary>
        ''' Adds a new HTML list item
        ''' </summary>
        ''' <param name="text">
        ''' Text string for the list item
        ''' </param>
        ''' <param name="clickable">
        ''' Boolean to designate clickability
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds a new HTML list item")> _
        Public Overridable Sub addListItem(ByVal text As String, ByVal clickable As Boolean)
            Me.Controls.Add(New HTMLListItem(text, clickable))
        End Sub

        ''' <summary>
        ''' Adds an HTML list item
        ''' </summary>
        ''' <param name="li">
        ''' Reference to an HTMLListItem to be added to the list
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds an HTML list item")> _
        Public Overridable Sub addListItem(ByRef li As HTMLListItem)
            Me.Controls.Add(li)
        End Sub

        Public Overridable Sub addListItemRoot(ByRef li As HTMLListItem)
            Me.Controls.Add(li)
        End Sub

        ''' <summary>
        ''' Adds an HTML list
        ''' </summary>
        ''' <param name="ul">
        ''' Reference to an HTML List
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Adds an HTML list")> _
        Public Overridable Sub addListItem(ByRef ul As HTMLList)
            Me.Controls.Add(ul)
        End Sub

        ''' <summary>
        ''' Counts the number of controls
        ''' </summary>
        ''' <returns>
        ''' Returns the count as an iteger
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Counts the number of controls")> _
        Public Function Count() As Integer
            Return Me.Controls.Count
        End Function
    End Class

    <ComponentModel.ToolboxItem(False)> _
    Public Class HTMLListItem
        Inherits Panel

        Private _clickable As Boolean = True

        ''' <summary>
        ''' Property to get/set Clickability
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' </value>
        ''' <returns>
        ''' clickable boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set Clickability")> _
        Public Property Clickable() As Boolean
            Get
                Return _clickable
            End Get
            Set(ByVal value As Boolean)
                _clickable = value
            End Set
        End Property

        Protected _containerHTMLList As HTMLList
        Protected ReadOnly Property ContainerHTMLList() As HTMLList
            Get
                If _containerHTMLList Is Nothing Then
                    _containerHTMLList = New HTMLList
                    Me.Controls.Add(_containerHTMLList)
                End If
                Return _containerHTMLList
            End Get
        End Property

        Private _navigateURL As String = "#"

        ''' <summary>
        ''' Property to get/set NavigateURL
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' </value>
        ''' <returns>
        ''' navigateURL string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set NavigateURL")> _
        Public Property NavigateURL() As String
            Get
                Return _navigateURL
            End Get
            Set(ByVal value As String)
                _navigateURL = value
            End Set
        End Property

        Private _text As String = "LI_" & Me.ClientID

        ''' <summary>
        ''' Property to get/set Text
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' </value>
        ''' <returns>
        ''' text string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set Text")> _
        Public Overridable Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

        Protected Overrides ReadOnly Property TagKey() As System.Web.UI.HtmlTextWriterTag
            Get
                Return HtmlTextWriterTag.Li
            End Get
        End Property

        ''' <summary>
        ''' Constructor for HTMLList class
        ''' </summary>
        ''' <param name="_text">
        ''' String for text
        ''' </param>
        ''' <param name="className">
        ''' String for the class name
        ''' </param>
        ''' <param name="href">
        ''' String for the href
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Constructor for HTMLList class")> _
        Public Sub New(ByVal _text As String, Optional ByVal className As String = Nothing, Optional ByVal href As String = Nothing)
            Me.Text = _text
            If className IsNot Nothing Then
                CssClass = className
            End If
            If href IsNot Nothing Then
                NavigateURL = href
            End If
        End Sub

        ''' <summary>
        ''' Constructor for HTMLList class
        ''' </summary>
        ''' <param name="text">
        ''' String for text
        ''' </param>
        ''' <param name="clickable">
        ''' Boolean to set clickability
        ''' </param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Constructor for HTMLList class")> _
        Public Sub New(ByVal text As String, ByVal clickable As Boolean)
            Me.Text = text
            Me.Clickable = clickable
        End Sub

        ''' <summary>
        ''' Constructor for HTMLList class
        ''' </summary>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Constructor for HTMLList class")> _
        Public Sub New()

        End Sub

        Private Sub HTMLListItem_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Clickable Then
                Dim href As New HyperLink()
                href.NavigateUrl = Me.NavigateURL
                href.Text = Me.Text
                Me.Controls.AddAt(0, href)
            Else
                Dim litText As New LiteralControl()
                litText.Text = Me.Text
                Me.Controls.AddAt(0, litText)
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="items"></param>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("")> _
        Public Overridable Sub addItems(ByRef items As HTMLListItem())
            For Each item As HTMLListItem In items
                Me.ContainerHTMLList.addListItemRoot(item)
            Next
        End Sub
    End Class
