Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class EditLabel
    Inherits CompositeControl
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("Text"), _
    DefaultEvent("TextChanged"), _
    ToolboxData("<{0}:EditLabel runat=""server""> </{0}:EditLabel>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class EditLabel
        Inherits CompositeControl
#End If
        Private WithEvents lblEditLabel As Label
        Private WithEvents tbEditLabel As TextBox

#Region "Properties"

        ''' <summary>
        ''' Property to get/set the Label CSS Class, the class to apply to the label when displaying
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: ""
        ''' </value>
        ''' <returns>
        ''' lblEditLabel.CssClass string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <Category("Appearance"), _
        Bindable(True), _
        DefaultValue(""), _
        Description("Property to get/set the Label CSS Class, the class to apply to the label when displaying") _
        > _
        Public Property labelCssClass() As String
            Get
                EnsureChildControls()
                Return lblEditLabel.CssClass
            End Get
            Set(ByVal value As String)
                EnsureChildControls()
                lblEditLabel.CssClass = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the Text Box Class, the class to apply to the textbox when editing
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: ""
        ''' </value>
        ''' <returns>
        ''' tbEditLabel.CssClass string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <Category("Appearance"), _
        Bindable(True), _
        DefaultValue(""), _
        Description("Property to get/set the Text Box Class, the class to apply to the textbox when editing") _
        > _
        Public Property textboxCssClass() As String
            Get
                EnsureChildControls()
                Return tbEditLabel.CssClass
            End Get
            Set(ByVal value As String)
                EnsureChildControls()
                tbEditLabel.CssClass = value
            End Set
        End Property

        Private _labelOnly As Boolean

        ''' <summary>
        ''' Property to get/set LabelOnly, when enabled will only render the label
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' Default Value: False
        ''' </value>
        ''' <returns>
        ''' labelOnly boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <Category("Appearance"), _
        Bindable(True), _
        DefaultValue(False), _
        Description("Property to get/set LabelOnly, when enabled will only render the label") _
        > _
        Public Property LabelOnly() As Boolean
            Get
                Return _labelOnly
            End Get
            Set(ByVal value As Boolean)
                _labelOnly = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the Text of the control
        ''' </summary>
        ''' <value>
        ''' string passed to the set method
        ''' Default Value: "LabelText"
        ''' </value>
        ''' <returns>
        ''' tbEditLabel.Text string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <Category("Appearance"), _
        Bindable(True), _
        DefaultValue("LabelText"), _
        Description("Property to get/set the Text of the control") _
        > _
        Public Property Text() As String
            Get

                EnsureChildControls()
                Return tbEditLabel.Text
            End Get
            Set(ByVal value As String)
                EnsureChildControls()
                tbEditLabel.Text = value
                lblEditLabel.Text = value
            End Set
        End Property
#End Region

        Private Shared ReadOnly EventSubmitKey As New Object()

        Private Sub EditLabel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            EditLabelInclude.RegisterJs(Me.Page)
        End Sub

        ' The Submit event.

        ''' <summary>
        ''' The Submit event.
        ''' </summary>
        ''' <remarks></remarks>
        < _
        Category("Action"), _
        Description("The Submit event.") _
        > _
        Public Custom Event TextChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                Events.AddHandler(EventSubmitKey, value)
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                Events.RemoveHandler(EventSubmitKey, value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, _
                ByVal e As System.EventArgs)
                CType(Events(EventSubmitKey), _
                    EventHandler).Invoke(sender, e)
            End RaiseEvent
        End Event

        ' The method that raises the Submit event.
        Protected Overridable Sub OnTextChanged(ByVal e As EventArgs)
            Dim textChangedHandler As EventHandler = _
                CType(Events(EventSubmitKey), EventHandler)
            If textChangedHandler IsNot Nothing Then
                textChangedHandler(Me, e)
            End If
        End Sub

        ' Handles the Click event of the Button and raises
        ' the Submit event.
        Private Sub tbEditLabelTextChanged(ByVal source As Object, _
            ByVal e As EventArgs)
            Text = tbEditLabel.Text
            OnTextChanged(EventArgs.Empty)
        End Sub

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()

            lblEditLabel = New Label()
            lblEditLabel.ID = "Label"

            tbEditLabel = New TextBox()
            tbEditLabel.ID = "Textbox"
            AddHandler tbEditLabel.TextChanged, _
                AddressOf tbEditLabelTextChanged

            lblEditLabel.Attributes("onclick") = "DTIShowEdit('" & Me.ClientID & "_" & lblEditLabel.ClientID & "', '" & Me.ClientID & "_" & tbEditLabel.ClientID & "');"

            tbEditLabel.Attributes("onblur") = "DTIHideEdit('" & Me.ClientID & "_" & lblEditLabel.ClientID & "', '" & Me.ClientID & "_" & tbEditLabel.ClientID & "');"
            tbEditLabel.Attributes("onkeydown") = "DTIhandleKeydown(event, '" & Me.ClientID & "_" & lblEditLabel.ClientID & "', '" & Me.ClientID & "_" & tbEditLabel.ClientID & "');"
            tbEditLabel.Attributes("style") = "display:none;"

            Me.Controls.Add(lblEditLabel)
            Me.Controls.Add(tbEditLabel)
        End Sub

        Protected Overrides Sub RecreateChildControls()
            EnsureChildControls()
        End Sub

        Private Sub EditLabel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If LabelOnly Then
                Try
                    Me.Controls.Remove(tbEditLabel)
                    lblEditLabel.Attributes.Remove("onclick")
                Catch ex As Exception

                End Try
            End If
        End Sub

        Private Class EditLabelInclude
            Inherits WebControl

            Protected Overrides ReadOnly Property TagKey() _
                As HtmlTextWriterTag
                Get
                    Return HtmlTextWriterTag.Script
                End Get
            End Property

            ''' <summary>
            ''' Registers javascript on a given page
            ''' </summary>
            ''' <param name="page">
            ''' The page that the javascript is to be registered on
            ''' </param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Registers javascript on a given page")> _
            Public Shared Sub RegisterJs(ByRef page As Page)
                Dim jQueryIncludeHeader As EditLabelInclude = BaseClasses.Spider.spiderPageforType(page, GetType(EditLabelInclude))
                If jQueryIncludeHeader Is Nothing Then
                    BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
                    page.Header.Controls.Add(New EditLabelInclude("text/javascript", "EditLabelScript.js"))
                End If
            End Sub

            ''' <summary>
            ''' Constructor for the EditLabel class
            ''' </summary>
            ''' <param name="type">
            ''' String argument for the type
            ''' </param>
            ''' <param name="filename">
            ''' String argument for the src filename
            ''' </param>
            ''' <remarks></remarks>
            <System.ComponentModel.Description("Constructor for the EditLabel class")> _
            Public Sub New(ByVal type As String, ByVal filename As String)
                Me.Attributes.Add("type", type)
                Me.Attributes.Add("src", BaseClasses.Scripts.ScriptsURL(True) & "DTIMiniControls/" & filename)
            End Sub

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                writer.Write("<script type=""" & Me.Attributes("type") & """ src=""" & Me.Attributes("src") & """></script>")
            End Sub
        End Class
    End Class
