Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.SessionState
Imports BaseClasses
Imports DTIMiniControls
Imports HighslideControls

''' <summary>
''' Design time support class. 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Design time support class."),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Public Class DTIServerBaseDesigner
    Inherits System.Web.UI.Design.ControlDesigner

    Protected Overrides Function GetErrorDesignTimeHtml(ByVal e As Exception) As String
        'Return error text in a placeholder
        'Return Me.CreatePlaceHolderDesignTimeHtml("DTIServerControl: The control could not be loaded:<br>" & e.ToString() & "<br>" & e.StackTrace)
        Return "<div><div style='Position:relative;text-align:right;margin-bottom:-12px;margin-right:3px;font-size: 11px;font-weight:bold;'><span style='background-color: gray;color:white;'>" & CType(Component, DTIServerBase).GetType.FullName & "</span></div></div>"
    End Function

    Public Overrides Function GetDesignTimeHtml(ByVal regions As System.Web.UI.Design.DesignerRegionCollection) As String
        Return GetDesignTimeHtml()
    End Function


    Public Overrides Function GetDesignTimeHtml() As String

        Dim tmp As String = ""
        Dim tp As String = ""
        Try

            tp = CType(Component, DTIServerBase).GetType.FullName
            If CType(Component, DTIServerBase).getDesignTimeHtml Is Nothing Then
                tmp = MyBase.GetDesignTimeHtml
            Else
                tmp = CType(Component, DTIServerBase).getDesignTimeHtml
            End If
            tp = tp.Substring(tp.LastIndexOf(".") + 1)

        Catch ex As Exception
            tmp = ex.Message & ex.StackTrace
        End Try

        Return "<div><div style='Position:relative;text-align:right;margin-bottom:-12px;margin-right:3px;font-size: 11px;font-weight:bold;'><span style='background-color: gray;color:white;'>" & tp & "</span></div>" & tmp & "</div>"

    End Function



End Class