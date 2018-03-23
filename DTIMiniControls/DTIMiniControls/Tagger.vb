Imports System.Text
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class Tagger
    Inherits Panel
#Else
    <AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("ShowSubmit"), _
    DefaultEvent("CurrentTagsChanged"), _
    ToolboxData("<{0}:Tagger runat=""server""> </{0}:Tagger>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Tagger
        Inherits Panel
#End If
        Public Event CurrentTagsChanged()
        Protected WithEvents _tagsUC1 As TagsUC
        Protected ReadOnly Property TagsUC1() As TagsUC
            Get
                If _tagsUC1 Is Nothing Then
                    _tagsUC1 = Me.Page.LoadControl("~/res/DTIMiniControls/TagsUC.ascx")
                End If
                Return _tagsUC1
            End Get
        End Property

        ''' <summary>
        ''' Property to get/set the Show Submit toggle
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' ShowSubmit boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Show Submit toggle")> _
        Public Property ShowSubmit() As Boolean
            Get
                Return TagsUC1.ShowSubmit
            End Get
            Set(ByVal value As Boolean)
                TagsUC1.ShowSubmit = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the tag text to be added
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' AddTagText string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the tag text to be added")> _
        Public Property AddTagText() As String
            Get
                Return TagsUC1.AddTagText
            End Get
            Set(ByVal value As String)
                TagsUC1.AddTagText = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the Current Tag Text
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' CurrentTagText string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Current Tag Text")> _
        Public Property CurrentTagText() As String
            Get
                Return TagsUC1.CurrentTagText
            End Get
            Set(ByVal value As String)
                TagsUC1.CurrentTagText = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set the Separator Character
        ''' </summary>
        ''' <value>
        ''' String passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' SeparatorCharacter string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Separator Character")> _
        Public Property SeparatorCharacter() As String
            Get
                Return TagsUC1.SeparatorCharacter
            End Get
            Set(ByVal value As String)
                TagsUC1.SeparatorCharacter = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set whether or not Popular Tags is shown.
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' ShowPopularTags boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set whether or not Popular Tags is shown.")> _
        Public Property ShowPopularTags() As Boolean
            Get
                Return TagsUC1.ShowPopularTags
            End Get
            Set(ByVal value As Boolean)
                TagsUC1.ShowPopularTags = value
            End Set
        End Property

        Private _popularTags As List(Of String) = New List(Of String)

        ''' <summary>
        ''' Property to set the list of popular tags
        ''' </summary>
        ''' <value>
        ''' String List passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to set the list of popular tags")> _
        Public WriteOnly Property popularTagsSet() As List(Of String)
            Set(ByVal value As List(Of String))
                _popularTags = value
            End Set
        End Property

        Protected ReadOnly Property popularTags() As String
            Get
                Return String.Join("', '", _popularTags.ToArray())
            End Get
        End Property

        Protected ReadOnly Property currentTags() As String
            Get
                Return String.Join("', '", currentTagsList.ToArray())
            End Get
        End Property

        Private ReadOnly Property Session() As System.Web.SessionState.HttpSessionState
            Get
                Return HttpContext.Current.Session
            End Get
        End Property

        ''' <summary>
        ''' Property to get/set the list of current tags
        ''' </summary>
        ''' <value>
        ''' String List passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' String list returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the list of current tags")> _
        Public Property currentTagsList() As List(Of String)
            Get
                If Session("DTITagger" & Me.ClientID) Is Nothing Then
                    Session("DTITagger" & Me.ClientID) = New List(Of String)
                End If
                Return Session("DTITagger" & Me.ClientID)
            End Get
            Set(ByVal value As List(Of String))
                Session("DTITagger" & Me.ClientID) = value
            End Set
        End Property

        ''' <summary>
        ''' Property to get/set whether or not the form will be validated on submit
        ''' </summary>
        ''' <value>
        ''' Boolean passed to the set method
        ''' Default Value: 
        ''' </value>
        ''' <returns>
        ''' ValidateOnFormSubmit boolean returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set whether or not the form will be validated on submit")> _
        Public Property ValidateOnFormSubmit() As Boolean
            Get
                Return TagsUC1.ValidateOnFormSubmit
            End Get
            Set(ByVal value As Boolean)
                TagsUC1.ValidateOnFormSubmit = value
            End Set
        End Property

        Private Sub Tagger_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            TaggerInclude.RegisterJs(Me.Page)
            With TagsUC1
                .currentTagsList = Me.currentTagsList
                .popularTagsSet = Me._popularTags
                '.ID = "myTaggerUserControl"
            End With
            Me.Controls.Add(TagsUC1)
        End Sub

        Private Sub TagsUC1_CurrentTagsChanged(ByVal tagsList As System.Collections.Generic.List(Of String)) Handles _tagsUC1.CurrentTagsChanged
            Me.currentTagsList = tagsList
            RaiseEvent CurrentTagsChanged()
        End Sub

        Private Sub Tagger_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim registerScript As String = "currIdArray.push('" & TagsUC1.DTICurrTagDiv.ClientID & _
                "'); currentTagsArray.push(['" & currentTags & "']); popularTagsArray['" & _
                TagsUC1.DTIPopTagDiv.ClientID & "'] = ['" & popularTags & "']; separatorArray['" & _
                TagsUC1.DTICurrTagDiv.ClientID & "'] = '" & SeparatorCharacter & "';"
            Page.ClientScript.RegisterStartupScript(Me.GetType, "registerTagger_" & Me.ClientID, registerScript, True)
        End Sub

        Private Class TaggerInclude
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
		<System.ComponentModel.Description("Registers javascript on a given page")>
		Public Shared Sub RegisterJs(ByRef page As Page)
			jQueryLibrary.jQueryInclude.addScriptFile(page, "DtiminiControls/DTITagManagement.js", id:="tagger")
		End Sub

		''' <summary>
		''' Constructor for Tagger class
		''' </summary>
		''' <param name="type">
		''' Type of script
		''' </param>
		''' <param name="filename"
		''' Filename of script
		''' ></param>
		''' <remarks></remarks>
            <System.ComponentModel.Description("Constructor for Tagger class")> _
            Public Sub New(ByVal type As String, ByVal filename As String)
                Me.Attributes.Add("type", type)
                Me.Attributes.Add("src", BaseClasses.Scripts.ScriptsURL(True) & "DTIMiniControls/" & filename)
            End Sub

            Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
                writer.Write("<script type=""" & Me.Attributes("type") & """ src=""" & Me.Attributes("src") & """></script>")
            End Sub
        End Class
    End Class
