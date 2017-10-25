<ComponentModel.ToolboxItem(False)> _
Public Class FreezeIt
    Inherits Panel

    Protected innerTable As New Table
    Protected innerRow As New TableRow
    Protected innerCell As New TableCell

#Region "Properties"

    Private _useInnerWidthAndHeight As Boolean = False

    ''' <summary>
    ''' Property to get/set UseInnerWidthAndHeight
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' useInnerWidthAndHeight boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set UseInnerWidthAndHeight")> _
    Public Property UseInnerWidthAndHeight() As Boolean
        Get
            Return _useInnerWidthAndHeight
        End Get
        Set(ByVal value As Boolean)
            _useInnerWidthAndHeight = value
        End Set
    End Property

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
    ''' BackgroundOpacity double returned by the get method
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

    Private _freezeId As String

    ''' <summary>
    ''' Property to get/set FreezeId
    ''' </summary>
    ''' <value>
    ''' Double passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' freezeId double returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set FreezeId")> _
    Public Property FreezeId() As String
        Get
            Return _freezeId
        End Get
        Set(ByVal value As String)
            _freezeId = value
        End Set
    End Property

    Private _isFrozen As Boolean = False

    ''' <summary>
    ''' Property to get/set IsFrozen
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' isFrozen boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set IsFrozen")> _
    Public Property IsFrozen() As Boolean
        Get
            Return _isFrozen
        End Get
        Set(ByVal value As Boolean)
            _isFrozen = value
            If _isFrozen Then freezeit()
        End Set
    End Property

    Private _useCenter As Boolean = False

    ''' <summary>
    ''' Property to get/set UseCentering
    ''' </summary>
    ''' <value>
    ''' Boolean passed to the set method
    ''' Default Value: 
    ''' </value>
    ''' <returns>
    ''' useCenter boolean returned by the get method
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Property to get/set UseCentering")> _
    Public Property UseCentering() As Boolean
        Get
            Return _useCenter
        End Get
        Set(ByVal value As Boolean)
            _useCenter = value
        End Set
    End Property
#End Region

    Private Sub FreezeScreen_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim opacityPercentage As Integer = BackgroundOpacity * 100
        Me.Style.Value = "display:none; position:absolute; z-index:1000; background-color:" & BackgroundColor & "; opacity: " & BackgroundOpacity & "; -ms-filter:'progid:DXImageTransform.Microsoft.Alpha(Opacity=" & opacityPercentage & ")'; filter: alpha(opacity=" & opacityPercentage & ");"

        If UseCentering Then
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
        End If

        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/FreezeIt.js", , True)
        Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "freezeScript" & Me.ClientID, getJS(), True)
        If DisplayOnAnyPostback Then
            Page.ClientScript.RegisterOnSubmitStatement(Me.GetType, "freezeScriptSubmit" & Me.ClientID, "FreezeIt" & Me.ClientID & "();")
        End If
    End Sub

    Private Function getJS() As String
        Dim outputJS As String = vbCrLf & _
            "function FreezeIt" & Me.ClientID & "() { FreezeItEl('" & FreezeId & "', '" & Me.ClientID & "')}" & vbCrLf & _
            "function UnfreezeIt" & Me.ClientID & "() { UnfreezeItEl('" & Me.ClientID & "')}"
        Return outputJS
    End Function

    Private Sub freezeit()
        Page.ClientScript.RegisterStartupScript(Me.GetType, "freezeOnStartupScript" & Me.ClientID, _
            "$(document).ready(function(){FreezeIt" & Me.ClientID & "();});", True)
    End Sub

    Public Shared Sub addFreezitScript(ByVal page As Web.UI.Page)
        jQueryLibrary.jQueryInclude.addScriptFile(page, "DTIMiniControls/FreezeIt.js", , True)
    End Sub

    Public Shared Sub freezeOnLoad(ByVal ctrl As Control)
        jQueryLibrary.jQueryInclude.addScriptFile(ctrl.Page, "DTIMiniControls/FreezeIt.js", , True)
        jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(ctrl.Page, "FreezeIt('" & ctrl.ClientID & "');")
    End Sub

End Class