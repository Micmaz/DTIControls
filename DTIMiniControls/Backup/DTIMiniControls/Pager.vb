<ToolboxData("<{0}:Pager ID=""Pager1"" runat=""server"" />"),ComponentModel.ToolboxItem(False)> _
Public Class Pager
    Inherits Panel
    Private WithEvents hid As New HiddenField
    Private WithEvents postback As New Button

    Public Event pagechanged(ByVal pageNum As Integer)


#Region "properties"
    Private _PageCount As Integer = 1

    ''' <summary>
    ''' By default the page is reloaded with query String "page" set to the current page.
    ''' If True a postback event is called and the pageChange event is raised.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("By default the page is reloaded with query String ""page"" set to the current page.   If True a postback event is called and the pageChange event is raised.")> _
    Private _postBackPageChanges As Boolean = False
    Public Property RaiseEventOnPageChange() As Boolean
        Get
            Return _postBackPageChanges
        End Get
        Set(ByVal value As Boolean)
            _postBackPageChanges = True
        End Set
    End Property

    ''' <summary>
    ''' Total number of pages
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Total number of pages")> _
    Public Property PageCount() As Integer
        Get
            Return _PageCount
        End Get
        Set(ByVal value As Integer)
            _PageCount = value
        End Set
    End Property





    Private _currentPage As Integer = 0

    ''' <summary>
    ''' Page currently being viewed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Page currently being viewed")> _
    Public Property CurrentPage() As Integer
        Set(ByVal value As Integer)
            _currentPage = value
        End Set
        Get
            Try
                Return Integer.Parse(Page.Request.QueryString("page"))
            Catch ex As Exception
                If _currentPage > 0 Then Return _currentPage
                Return 1
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Number of pages to display in pager.  Will always be an odd number
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Number of pages to display in pager. Will always be an odd number")> _
    Private _PagesToDisplay As Integer = 11
    Public Property PagesToDisplay() As Integer
        Get
            Return Math.Floor(_PagesToDisplay / 2)
        End Get
        Set(ByVal value As Integer)
            _PagesToDisplay = value
        End Set
    End Property

    Private _PagesToSkip As Integer = PagesToDisplay + 1

    ''' <summary>
    ''' Number of pages from the current page the skip link skip to
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Number of pages from the current page the skip link skip to")> _
    Public Property PagesToSkip() As Integer
        Get
            Return _PagesToSkip
        End Get
        Set(ByVal value As Integer)
            _PagesToSkip = value
        End Set
    End Property
#End Region

    Private Sub Pager_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        postback.Style.Add("display", "none")
        Me.Controls.Add(postback)
        Me.Controls.Add(hid)
    End Sub


    Private Sub Pager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim divLeft As New Panel
        Dim divRight As New Panel

        divLeft.Style.Add("width", "80%")
        divLeft.Style.Add("float", "left")
        divRight.Style.Add("width", "20%")
        divRight.Style.Add("text-align", "right")
        divRight.Style.Add("float", "right")

        divLeft.Controls.Add(New LiteralControl(writePager()))
        divRight.Controls.Add(New LiteralControl(CurrentPage & " / " & PageCount))

        Me.Controls.Add(divLeft)
        Me.Controls.Add(divRight)
        Me.Controls.Add(New LiteralControl("<br style=""clear:both;"" />"))
    End Sub

    Private Function writePager() As String
        Return writePrev() & "&nbsp;&nbsp;" & writePages() & "&nbsp;&nbsp;" & writeNext()
    End Function

    Private Function writePages() As String
        Dim pages As String = ""
        Dim i As Integer
        Dim min As Integer = 1
        Dim max As Integer = PageCount

        If CurrentPage - PagesToDisplay >= 1 Then
            min = CurrentPage - PagesToDisplay
        End If
        If CurrentPage + PagesToDisplay <= PageCount Then
            max = CurrentPage + PagesToDisplay
        End If

        For i = min To max Step 1
            pages &= writePage(i)
        Next
        Return pages
    End Function

    Private Function writeNext() As String
        Dim nextTxt As String = ""
        If CurrentPage + PagesToSkip < PageCount Then
            nextTxt &= buildLink(CurrentPage + PagesToSkip, "&gt;")
        End If
        If CurrentPage < PageCount - PagesToDisplay Then
            nextTxt &= "&nbsp;&nbsp;" & buildLink(PageCount, "&gt;&gt;")
        End If
        Return nextTxt
    End Function

    Private Function writePrev() As String
        Dim prevTxt As String = ""
        If CurrentPage - PagesToDisplay > 1 Then
            prevTxt &= buildLink(1, "&lt;&lt;") & "&nbsp;&nbsp;"
        End If
        If CurrentPage - PagesToSkip > 1 Then
            prevTxt &= buildLink(CurrentPage - PagesToSkip, "&lt;")
        End If
        Return prevTxt
    End Function

    Private Function writePage(ByVal pageIndex As Integer) As String
        If Not pageIndex = CurrentPage Then
            Return "&nbsp;" & buildLink(pageIndex, pageIndex)
        Else
            Return "&nbsp;" & pageIndex
        End If
    End Function

    Private Function buildLink(ByVal pageNum As Integer, ByVal text As String) As String
        Dim Link As String = Page.Request.Url.LocalPath & "?page=" & pageNum

        For Each key As String In Page.Request.QueryString.Keys
            If key.ToLower <> "page" Then
                Link &= "&" & key & "=" & Page.Request.QueryString(key)
            End If
        Next
        If RaiseEventOnPageChange Then
            Return "<a href=""javascript:;"" onclick=""document.getElementById('" & hid.ClientID & "').value=" & pageNum & ";document.getElementById('" & postback.ClientID & "').click();"">" & text & "</a>"
        Else
            Return "<a href=""" & Link & """>" & text & "</a>"
        End If

    End Function

    Private Sub postback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles postback.Click
        _currentPage = hid.Value
        RaiseEvent pagechanged(hid.Value)
    End Sub
End Class
