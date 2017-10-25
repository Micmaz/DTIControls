<ComponentModel.ToolboxItem(False), Obsolete("This Method is Deprecated, use JqueryUIControls.Autocomplete instead.")> _
Public Class AutocompleteDropDown
    Inherits System.Web.UI.WebControls.TextBox

    Private parms As New Hashtable

#Region "Properties"

    Public Property onItemSelect() As String
        Get
            If parms("onItemSelect") Is Nothing Then Return 0
            Return parms("onItemSelect")
        End Get
        Set(ByVal value As String)
            parms("onItemSelect") = value
        End Set
    End Property

    Public Property minChars() As Integer
        Get
            If parms("minChars") Is Nothing Then Return 0
            Return parms("minChars")
        End Get
        Set(ByVal value As Integer)
            parms("minChars") = value
        End Set
    End Property

    Public Property cacheLength() As Integer
        Get
            If parms("cacheLength") Is Nothing Then Return 1
            Return parms("cacheLength")
        End Get
        Set(ByVal value As Integer)
            parms("cacheLength") = value
        End Set
    End Property

    Public Property delay() As Integer
        Get
            If parms("delay") Is Nothing Then Return 400
            Return parms("delay")
        End Get
        Set(ByVal value As Integer)
            parms("delay") = value
        End Set
    End Property

    Public Property widthDropDown() As Integer
        Get
            If parms("width") Is Nothing Then Return 100
            Return parms("width")
        End Get
        Set(ByVal value As Integer)
            parms("width") = value
        End Set
    End Property

    Public ReadOnly Property Value() As String
        Get
            Return Me.Page.Request.Params(Me.ClientID & "_hidden")
        End Get
    End Property

    Private _url As String = Nothing
    Public Property postbackUrl() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

    Private ReadOnly Property query() As String
        Get
            If Page Is Nothing Then Return Nothing
            Return Page.Request.QueryString("q")
        End Get
    End Property

    Private ReadOnly Property ctrlid() As String
        Get
            If Page Is Nothing Then Return Nothing
            Return Page.Request.QueryString("ctrl")
        End Get
    End Property
#End Region

    Public Event search(ByVal query As String)
    Public Event search2(ByVal sender As Object, ByVal query As String)

    Private Sub checkpost()
        If Not Page.IsPostBack AndAlso postbackUrl Is Nothing AndAlso Not query Is Nothing AndAlso ctrlid = Me.ID Then
            RaiseEvent search(query)
            RaiseEvent search2(Me, query)
            Page.Response.End()
        End If
    End Sub

    Private Sub AutocompleteDropDown_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        checkpost()
    End Sub

    Private Sub AutocompleteDropDown_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkpost()
        BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.autocomplete.js")
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.autocomplete.css")
    End Sub

    Private Function renderparms() As String
        Dim outstr As String = ""
        For Each key As String In parms.Keys
            If key <> "onItemSelect" Then
                outstr &= key & ":" & parms(key) & "," & vbCrLf
            End If
        Next
        Return outstr
    End Function

    Private Sub AutocompleteDropDown_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim url As String = postbackUrl
        If url Is Nothing OrElse url.Length = 0 Then
            url = Page.Request.Url.PathAndQuery
        End If
        Dim s As String = ""
        s &= "$(function(){" & vbCrLf
        s &= "     $('#" & Me.ClientID & "').autocomplete(" & vbCrLf
        s &= "          '" & url & "',{" & vbCrLf
        s &= "               " & renderparms() & vbCrLf
        s &= "               onItemSelect : function(li){" & vbCrLf
        s &= "                    $('#" & Me.ClientID & "_hidden').val(li.extra);" & vbCrLf
        s &= "                    " & onItemSelect & "(li);" & vbCrLf
        s &= "               }," & vbCrLf
        s &= "               extraParams :{" & vbCrLf
        s &= "                    ctrl:'" & Me.ID & "'" & vbCrLf
        s &= "               }" & vbCrLf
        s &= "      });" & vbCrLf
        s &= "});" & vbCrLf
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
    End Sub

    Public Sub respond(ByVal dt As DataTable, ByVal nameColumn As String, Optional ByVal valuecolumn As String = Nothing)
        Dim outstr As String = ""
        For Each row As DataRow In dt.Rows
            If Not valuecolumn Is Nothing Then
                outstr &= row(nameColumn) & "|" & row(valuecolumn) & vbCrLf
            Else
                outstr &= row(nameColumn) & vbCrLf
            End If
        Next
        respond(outstr)
    End Sub

    Public Sub respond(ByVal dt As DataTable)
        Dim outstr As String = ""
        For Each row As DataRow In dt.Rows
            For Each col As DataColumn In dt.Columns
                outstr &= row(col.ColumnName) & "|"
            Next
            outstr.Trim("|")
        Next
        respond(outstr)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<input type=""hidden"" name=""" & Me.ClientID & "_hidden"" id=""" & Me.ClientID & "_hidden"" />")
        MyBase.Render(writer)
    End Sub

    Public Sub respond(ByRef CRLF_DelimitedList As String)
        Page.Response.Clear()
        Page.Response.Write(CRLF_DelimitedList)
        Page.Response.End()
    End Sub
End Class
