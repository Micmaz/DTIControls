Imports System.Web.UI.WebControls
Imports BaseClasses

#If DEBUG Then
Public Class DTICalendar
    Inherits BaseAsyncControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTICalendar
        Inherits BaseAsyncControl
#End If

        Public Event DateRangeChanged(ByVal starttime As DateTime, ByVal endtime As DateTime)
        Public Event EventChanged(ByVal id As String, ByVal name As String, ByVal isAllDay As Boolean, ByVal starttime As DateTime, ByVal endtime As DateTime)
        Public Event EventCreated(ByVal name As String, ByVal isAllDay As Boolean, ByVal starttime As DateTime, ByVal endtime As DateTime)
        Public Event EventDeleted(ByVal id As String)

        Private Sub DTICalendar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            DTICalendar.DTICalendarInclude.RegisterJs(Me.Page)
            jQueryLibrary.ThemeAdder.AddTheme(Me.Page)
            jQueryLibrary.Plugins.Center.Register(Me.Page)
            jQueryLibrary.Plugins.TimePicker.Register(Me.Page)
        End Sub

        Protected Overrides Sub ActionCalled()
            Select Case Action
                Case "fetchEvents"
                    Dim startDate As DateTime = UnixToDate(Page.Request.Params("start"))
                    Dim endDate As DateTime = UnixToDate(Page.Request.Params("end"))
                    RaiseEvent DateRangeChanged(startDate, endDate)
                Case "changeEvent"
                Case "datechangeEvent"
                Case "deleteEvent"
                Case "createEvent"
            End Select
        End Sub

        Private Shared Function DateToUnix(ByVal stamp As DateTime) As Long
            Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Dim diff As TimeSpan = stamp - origin
            Return Math.Floor(diff.TotalSeconds)
        End Function

        Private Shared Function UnixToDate(ByVal timestamp As Long) As DateTime
            Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Return origin.AddSeconds(timestamp)
        End Function

        Private Class DTICalendarInclude
            Inherits WebControl

            Private _tagKey As HtmlTextWriterTag = HtmlTextWriterTag.Script

            Protected Overrides ReadOnly Property TagKey() _
                As HtmlTextWriterTag
                Get
                    Return _tagKey
                End Get
            End Property

            Public Shared Sub RegisterJs(ByRef page As Page)
                BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
                jQueryLibrary.Plugins.ContextMenu.Register(page)
                Dim jQueryIncludeHeader As DTICalendarInclude = BaseClasses.Spider.spiderPageForType(page, GetType(DTICalendarInclude))
                If jQueryIncludeHeader Is Nothing Then
                    page.Header.Controls.Add(New DTICalendarInclude("text/javascript", "fullcalendar.js"))
                    page.Header.Controls.Add(New DTICalendarInclude("text/css", "fullcalendar.css"))
                End If
            End Sub

            Public Sub New(ByVal type As String, ByVal filename As String, Optional ByVal noscripts As Boolean = False)
                Dim src As String = "src"
                If type = "text/css" Then
                    _tagKey = HtmlTextWriterTag.Link
                    src = "href"
                    Me.Attributes.Add("rel", "stylesheet")
                End If
                Me.Attributes.Add("type", type)
                If noscripts Then
                    Me.Attributes.Add(src, "~/res/DTICalendar/" & filename)
                Else
                    Me.Attributes.Add(src, BaseClasses.Scripts.ScriptsURL(type <> "text/css") & "DTICalendar/" & filename)
                End If
            End Sub
        End Class
    End Class
