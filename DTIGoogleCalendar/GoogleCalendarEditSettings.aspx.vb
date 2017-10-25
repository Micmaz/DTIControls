Imports System.Web.Services
Imports DTIGoogleCalendar.dsGoogleCal
Imports DTIGoogleCalendar.SharedGoogleCalendarVariables
Imports DTIServerControls.DTISharedVariables

#If DEBUG Then
Partial Public Class GoogleCalendarEditSettings
    Inherits DTIServerControls.SettingsEditorPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class GoogleCalendarEditSettings
        Inherits DTIServerControls.SettingsEditorPage
#End If

        <WebMethod()> _
        Public Shared Function saveGoogleCalendarSettings(ByVal cont_type As String, ByVal src As String, ByVal width As String, ByVal height As String) As Integer
            Try
                If LoggedIn AndAlso AdminOn Then
                    Dim calendarRow As DTIGoogleCalendarRow
                    For Each row As DTIGoogleCalendarRow In calendarTable
                        If row.Content_Type = cont_type Then
                            calendarRow = row
                            Exit For
                        End If
                    Next
                    Dim newRow As Boolean = False
                    If calendarRow Is Nothing Then
                        sharedSqlHelper.SafeFillTable("select * from " & calendarTable.TableName & _
                            " where Main_Id = @mainid and Content_Type = @contType", calendarTable, _
                            New Object() {MasterMainId, cont_type})

                        For Each row As DTIGoogleCalendarRow In calendarTable
                            If row.Content_Type = cont_type Then
                                calendarRow = row
                            End If
                        Next

                        If calendarRow Is Nothing Then
                            calendarRow = calendarTable.NewDTIGoogleCalendarRow
                            newRow = True
                        End If
                    End If

                    If Not src Is Nothing AndAlso src <> "" Then
                        calendarRow.Iframe_Source = src
                    End If
                    If Not width Is Nothing AndAlso Integer.TryParse(width, New Integer) Then
                        calendarRow.Width = width
                    End If
                    If Not height Is Nothing AndAlso Integer.TryParse(height, New Integer) Then
                        calendarRow.Height = height
                    End If
                    If newRow Then
                        If Not cont_type Is Nothing AndAlso cont_type <> "" Then
                            calendarRow.Content_Type = cont_type
                            calendarRow.Main_Id = MasterMainId
                        End If
                        calendarTable.AddDTIGoogleCalendarRow(calendarRow)
                    End If

                    sharedSqlHelper.Update(calendarRow.Table)

                    Return 1
                Else
                    Return -1
                End If
            Catch ex As Exception
                Return -1
            End Try
        End Function


        <WebMethod()> _
        Public Shared Function restoreGoogleCalendar(ByVal cont_type As String) As String
            Try
                Dim calendarRow As DTIGoogleCalendarRow
                For Each row As DTIGoogleCalendarRow In calendarTable
                    If row.Content_Type = cont_type Then
                        calendarRow = row
                        Exit For
                    End If
                Next
                Dim newRow As Boolean = False
                If calendarRow Is Nothing Then
                    sharedSqlHelper.SafeFillTable("select * from " & calendarTable.TableName & _
                        " where Main_Id = @mainid and Content_Type = @contType", calendarTable, _
                        New Object() {MasterMainId, cont_type})

                    For Each row As DTIGoogleCalendarRow In calendarTable
                        If row.Content_Type = cont_type Then
                            calendarRow = row
                            Exit For
                        End If
                    Next
                End If

                If calendarRow Is Nothing Then
                    Return "https://www.google.com/calendar/embed"
                Else
                    Return calendarRow.Iframe_Source
                End If
            Catch ex As Exception
                Return "https://www.google.com/calendar/embed"
            End Try
        End Function
    End Class