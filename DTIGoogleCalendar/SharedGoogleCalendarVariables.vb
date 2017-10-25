Imports System.Web.SessionState
Imports System.Web
Imports DTIGoogleCalendar.dsGoogleCal
Imports DTIServerControls.DTISharedVariables

#If DEBUG Then
Public Class SharedGoogleCalendarVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SharedGoogleCalendarVariables
#End If
        Public Shared ReadOnly Property calendarTable() As DTIGoogleCalendarDataTable
            Get
                If Session("DTIGoogleCalendarDataTable") Is Nothing Then
                    Session("DTIGoogleCalendarDataTable") = New DTIGoogleCalendarDataTable
                End If
                Return Session("DTIGoogleCalendarDataTable")
            End Get
        End Property
    End Class
