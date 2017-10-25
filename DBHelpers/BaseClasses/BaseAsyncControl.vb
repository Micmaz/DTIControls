Imports System.Web.UI.WebControls
Imports System.Web
Imports System.Web.Configuration.WebConfigurationManager

Public Class BaseAsyncControl
    Inherits Panel

    'example ajax url: CalendarTest.aspx?IsAsyncKey=&DTICalendar=DTICalendar1&action=fetchEvents

    Public Const IsAsyncKey As String = "DTIBaseAsyncCall"

    Public Event ActionCalled(ByVal eventName As String)

    Public Shared Function IsAsyncCall(ByRef page As UI.Page) As Boolean
        Return page.Request.QueryString(IsAsyncKey) IsNot Nothing
    End Function

    Protected ReadOnly Property Instance() As String
        Get
            If Me.Page.Request.QueryString(Me.Parent.GetType.Name) Is Nothing Then
                Return ""
            Else
                Return Me.Page.Request.QueryString(Me.Parent.GetType.Name)
            End If
        End Get
    End Property

    Protected ReadOnly Property Action() As String
        Get
            If Me.Page.Request.QueryString("action") Is Nothing Then
                Return ""
            Else
                Return Me.Page.Request.QueryString("action")
            End If
        End Get
    End Property

    Private _asyncText As String = ""
    Public Property AsyncText() As String
        Get
            Return _asyncText
        End Get
        Set(ByVal value As String)
            _asyncText = value
        End Set
    End Property

    Private _asyncInput As String = ""
    Public ReadOnly Property AsyncInput() As String
        Get
            Return _asyncInput
        End Get
    End Property

    Public Sub EndAction()
        Me.Page.Response.Write(AsyncText)
        Me.Page.Response.End()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        CallActions()
        MyBase.Render(writer)
    End Sub

    Public Sub CallActions()
        If IsAsyncCall(Me.Page) AndAlso Instance = Me.Parent.ClientID Then
            Dim res As HttpResponse = Me.Page.Response
            Try
                Using strR As New IO.StreamReader(HttpContext.Current.Request.InputStream)
                    _asyncInput = strR.ReadToEnd
                End Using
            Catch ex As Exception
                _asyncInput = ""
            End Try
            res.Clear()
            res.ContentType = "application/json"
            RaiseEvent ActionCalled(Action)
            res.End()
        End If
    End Sub

    Public Function getActionURL(ByVal actionName As String) As String
        'CalendarTest.aspx?IsAsyncKey=&DTICalendar=DTICalendar1&action=fetchEvents
        Return Me.Page.Request.Url.ToString & "?" & IsAsyncKey & "=&" & Me.Parent.GetType.Name & "=" & Me.Parent.ClientID & "&action=" & actionName
    End Function

    Public Function getComponentURL() As String
        Return Me.Page.Request.Url.ToString & "?" & IsAsyncKey & "=&" & Me.Parent.GetType.Name & "=" & Me.Parent.ClientID &
    End Function
End Class
