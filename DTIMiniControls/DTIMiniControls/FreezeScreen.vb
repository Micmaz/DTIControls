Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.IO

<ComponentModel.ToolboxItem(False)> _
Public Class FreezeScreen
    Inherits Panel

    Protected innerTable As New Table
    Protected innerRow As New TableRow
    Protected innerCell As New TableCell

#Region "Properties"

    Private _displayOnAnyPostback As Boolean = True

    ''' <summary>
    ''' Property to get/set DisplayOnAnyPostback
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' displayOnAnyPostback boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set DisplayOnAnyPostback")> _
    Public Property DisplayOnAnyPostback() As Boolean
        Get
            Return _displayOnAnyPostback
        End Get
        Set(ByVal value As Boolean)
            _displayOnAnyPostback = value
        End Set
    End Property

    Private _displayOnPartialPostback As Boolean = True

    ''' <summary>
    ''' Property to get/set DisplayOnPartialPostback
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' displayOnPartialPostback boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set DisplayOnPartialPostback")> _
    Public Property DisplayOnPartialPostback() As Boolean
        Get
            Return _displayOnPartialPostback
        End Get
        Set(ByVal value As Boolean)
            _displayOnPartialPostback = value
        End Set
    End Property

    Private _bgColor As String = "#000000"

    ''' <summary>
    ''' Property to get/set BackgroundColor
    ''' </summary>
    ''' <value>
    ''' String passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' bgColor string returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set BackgroundColor")> _
    Public Property BackgroundColor() As String
        Get
            Return _bgColor
        End Get
        Set(ByVal value As String)
            _bgColor = value
        End Set
    End Property

    Private _opacity As Double = 0.7

    ''' <summary>
    ''' Property to get/set BackgroundOpacity
    ''' </summary>
    ''' <value>
    ''' Double passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' opacity double returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set BackgroundOpacity")> _
    Public Property BackgroundOpacity() As Double
        Get
            Return _opacity
        End Get
        Set(ByVal value As Double)
            _opacity = value
        End Set
    End Property
#End Region

    Private Sub FreezeScreen_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        innerTable.ID = "overlayInnerTable_" & Me.ID
    End Sub

    Private Function getJS() As String
        Dim outputJS As String = vbCrLf & _
        "function FreezeScreen" & Me.ID & "() { FreezeScreenEl('" & Me.ClientID & "')}" & vbCrLf & _
        "function UnfreezeScreen" & Me.ID & "() { UnfreezeScreenEl('" & Me.ClientID & "')}" & vbCrLf & _
        "addLoadEvent(function(){UnfreezeScreen" & Me.ID & "();})" & vbCrLf
        If DisplayOnAnyPostback Then
            outputJS &= vbCrLf & "function FreezeScreen(){FreezeScreen" & Me.ID & "();}" & _
                           vbCrLf & "function UnfreezeScreen(){UnfreezeScreen" & Me.ID & "();}"
        End If
        Return outputJS
    End Function

    Private Sub FreezeScreen_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim opacityPercentage As Integer = BackgroundOpacity * 100
        Me.Style.Value = "display:none; position:absolute; z-index:1000; left:-1px; top:-1px; width:100%; background-color:" & BackgroundColor & "; opacity: " & BackgroundOpacity & "; -ms-filter:'progid:DXImageTransform.Microsoft.Alpha(Opacity=" & opacityPercentage & ")'; filter: alpha(opacity=" & opacityPercentage & ");"
        innerTable.Style("width") = "100%"
        innerCell.VerticalAlign = VerticalAlign.Middle
        innerCell.HorizontalAlign = HorizontalAlign.Center

        innerRow.Cells.Add(innerCell)
        innerTable.Rows.Add(innerRow)
        For Each ctrl As Control In Me.Controls
            innerCell.Controls.Add(ctrl)
        Next
        Me.Controls.Clear()
        Me.Controls.Add(innerTable)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.centerdiv.js", , True)
        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/FreezeScreen.js", , True)
        Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "freezeScript" & Me.ID, getJS(), True)
        If DisplayOnAnyPostback Then
            Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, "freezeScriptSubmit" & Me.ID & "", "FreezeScreen" & Me.ClientID & "()")
        End If

        If DisplayOnPartialPostback Then
            Dim ajaxScript As String = "if(window.Sys){var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();  " & vbCrLf & _
                    "pageRequestManager.add_endRequest(UnfreezeScreen" & Me.ID & ");  " & vbCrLf & _
                    "pageRequestManager.add_initializeRequest(FreezeScreen" & Me.ID & ");  }"
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "AJAXFreezeScript" & Me.ID, ajaxScript, True)
        End If

    End Sub
End Class
