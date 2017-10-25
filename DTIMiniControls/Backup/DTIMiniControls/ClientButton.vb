Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Reflection
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class ClientButton
    Inherits HtmlInputButton
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("OnClick"), _
    ToolboxData("<{0}:ClientButton runat=""server""> </{0}:ClientButton>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ClientButton
        Inherits HtmlInputButton
#End If

        ''' <summary>
        ''' Property to get/set Text for the button
        ''' </summary>
        ''' <value>
        ''' Text string passed to the set method
        ''' </value>
        ''' <returns>
        ''' Text string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set Text for the button")> _
        Public Property Text() As String
            Get
                Return Me.Attributes("value")
            End Get
            Set(ByVal value As String)
                Me.Attributes("value") = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the On Click value for the button
        ''' </summary>
        ''' <value>
        ''' Onclick string passed to the set method
        ''' </value>
        ''' <returns>
        ''' Onclick string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the On Click value for the button")> _
        Public Property OnClick() As String
            Get
                Return Me.Attributes("onclick")
            End Get
            Set(ByVal value As String)
                Me.Attributes("onclick") = value
            End Set
        End Property
        ''' <summary>
        ''' Constructor for ClientButton class, sets default values for onclick and value
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            Me.Attributes.Add("onclick", "")
            Me.Attributes.Add("value", "")
        End Sub
    End Class
