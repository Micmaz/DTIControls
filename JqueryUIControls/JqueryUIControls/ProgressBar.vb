Public Class ProgressBar
    Inherits Panel

    Private _Value As Integer

    ''' <summary>
    ''' Percentage out of 100 of bar that is filled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Percentage out of 100 of bar that is filled")> _
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            _Value = value
        End Set
    End Property

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Sub ProgressBar_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim str As String = "$(function(){"
        str &= "$(""#" & Me.ClientID & """).progressbar({value: " & Value & "});" & vbCrLf
        str &= "     });" & vbCrLf
        jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, str)
    End Sub

    Public Sub New(ByVal Value As Integer)
        Me.Value = Value
    End Sub

    Public Sub New()

    End Sub
End Class
