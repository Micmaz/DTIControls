#If DEBUG Then
Public Class jsonServerControlTimer
    Inherits jsonSeverConrol
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class jsonServerControlTimer
        Inherits jsonSeverConrol
#End If

        Private Sub jsonServerControlTimer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAjax/jquery.timers-1.2.js")
            jQueryLibrary.jQueryInclude.addScriptBlock(Page, "$(document).everyTime(" & interval & ",function(){" & Me.jsCallFunc & "('" & action & "','');})", False)
        End Sub

        Private _action As String
        Public Property action() As String
            Get
                Return _action
            End Get
            Set(ByVal value As String)
                _action = value
            End Set
        End Property

        Private _interval As Integer = 5000
        Public Property interval() As Integer
            Get
                Return _interval
            End Get
            Set(ByVal value As Integer)
                _interval = value
            End Set
        End Property

    End Class
