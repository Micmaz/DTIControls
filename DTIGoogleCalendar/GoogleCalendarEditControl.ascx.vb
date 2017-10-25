#If DEBUG Then
Partial Public Class GoogleCalendarEditControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class GoogleCalendarEditControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private _cnt_type As String = ""
        Public Property contentType() As String
            Get
                Return _cnt_type
            End Get
            Set(ByVal value As String)
                _cnt_type = value
            End Set
        End Property

        Private _ctrlId As String = ""
        Public Property ControlId() As String
            Get
                Return _ctrlId
            End Get
            Set(ByVal value As String)
                _ctrlId = value
            End Set
        End Property

        Private _src As String = ""
        Public Property Source() As String
            Get
                Return _src
            End Get
            Set(ByVal value As String)
                _src = value
            End Set
        End Property

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Dim qscoll As NameValueCollection = HttpUtility.ParseQueryString(Source)

        End Sub
    End Class