#If DEBUG Then
Public Class StarRater
    Inherits RadioButtonList
#Else
    <ToolboxData("<{0}:StarRater runat=""server""> </{0}:StarRater>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class StarRater
        Inherits RadioButtonList
#End If
        Private _callback As String = ""

        ''' <summary>
        ''' Property to get/set the Callback Function
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' callback string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Callback Function")> _
        Public Property CallbackFunction() As String
            Get
                Return _callback
            End Get
            Set(ByVal value As String)
                _callback = value
            End Set
        End Property


        Private _number_of_stars As Integer = 5

        ''' <summary>
        ''' Property to get/set the number of stars
        ''' </summary>
        ''' <value>
        ''' Integer passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' number_of_stars integer returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the number of stars")> _
        Public Property NumberOfStars() As Integer
            Get
                Return _number_of_stars
            End Get
            Set(ByVal value As Integer)
                _number_of_stars = value
            End Set
        End Property

        Private Sub StarRater_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.rating.js", , True)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.MetaData.js", , True)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jqueryStar.css", "text/css", True)

            Dim splitVal As Integer = Items.Count / NumberOfStars
            Dim script As String = "function initStarRaters(ancestor) {if(!ancestor){ancestor='';}$(ancestor +' .DTIStarRater input:radio').attr('class', 'star {split:" & _
                splitVal & "}').rating("

            If CallbackFunction <> "" Then
                script &= "{callback: " & CallbackFunction & "}"
            End If
            script &= ");$(ancestor +' .DTIStarRater').fadeIn();} $(function() {initStarRaters();});"

            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "rate_class_init", script, True)
        End Sub

        Protected Overrides Sub RenderItem(ByVal itemType As System.Web.UI.WebControls.ListItemType, ByVal repeatIndex As Integer, ByVal repeatInfo As System.Web.UI.WebControls.RepeatInfo, ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim radioButton As New RadioButton()
            radioButton.Page = Me.Page
            radioButton.GroupName = Me.UniqueID
            radioButton.ID = (Me.ClientID & "_") + repeatIndex.ToString()
            radioButton.Attributes("value") = Me.Items(repeatIndex).Value
            radioButton.Checked = Me.Items(repeatIndex).Selected
            radioButton.TextAlign = Me.TextAlign
            radioButton.AutoPostBack = Me.AutoPostBack
            radioButton.TabIndex = Me.TabIndex
            radioButton.Enabled = Me.Enabled

            radioButton.Style.Add(HtmlTextWriterStyle.BackgroundColor, Me.Items(repeatIndex).Text)

            radioButton.RenderControl(writer)
        End Sub


        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            Me.RepeatLayout = WebControls.RepeatLayout.Flow
            Me.RepeatDirection = WebControls.RepeatDirection.Horizontal
            Me.Style("display") = "none"
            Me.CssClass = "DTIStarRater"
            MyBase.Render(writer)
        End Sub
    End Class
