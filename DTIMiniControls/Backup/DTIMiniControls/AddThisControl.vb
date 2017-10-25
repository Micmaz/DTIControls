#If DEBUG Then
Public Class AddThisControl
    Inherits Literal
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class AddThisControl
        Inherits Literal
#End If
        Public DefaultButtonIcons As New List(Of String)

        Private _username As String = "xa-4b944e241ce5319f"

        ''' <summary>
        ''' Property to get/set the Username
        ''' </summary>
        ''' <value>
        ''' Username string passed to the set method
        ''' </value>
        ''' <returns>
        ''' Username string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Username")> _
        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property

        Private _text_insturction As String = "Share"

        ''' <summary>
        ''' Property to get/set the Text Instruction
        ''' </summary>
        ''' <value>
        ''' Text instruction string passed to the set method
        ''' </value>
        ''' <returns>
        ''' Text instruction string returned by the get method
        ''' </returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Property to get/set the Text Instruction")> _
        Public Property TextInstruction() As String
            Get
                Return _text_insturction
            End Get
            Set(ByVal value As String)
                _text_insturction = value
            End Set
        End Property

        Private Sub AddThisControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            DefaultButtonIcons.Add("email")
            DefaultButtonIcons.Add("print")
            DefaultButtonIcons.Add("facebook")
            DefaultButtonIcons.Add("twitter")
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            Me.Text = "<!-- AddThis Button BEGIN --> " & vbCrLf & _
                "<div class=""addthis_toolbox addthis_default_style""> " & vbCrLf & _
                "<a href=""http://www.addthis.com/bookmark.php?v=250&amp;username=" & Username & """ " & _
                "class=""addthis_button_compact"">" & TextInstruction & "</a> " & vbCrLf & _
                "<span class=""addthis_separator"">|</span> " & vbCrLf

            For Each service As String In DefaultButtonIcons
                Me.Text &= "<a class=""addthis_button_" & service & """></a> " & vbCrLf
            Next

            Me.Text &= "</div> " & vbCrLf & "<!-- AddThis Button END --> " & vbCrLf
            '_
            '    "<script type=""text/javascript"" src=""" & _
            '    "http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4b944e241ce5319f""></script> " & _
            '    vbCrLf &
            MyBase.Render(writer)
        End Sub

        Private Sub AddThisControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Page.ClientScript.RegisterClientScriptInclude("addthis", "http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4b944e241ce5319f")
        End Sub

    End Class
