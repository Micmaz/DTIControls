#If DEBUG Then
Public Class TabSlider
    Inherits Panel
#Else
    <ToolboxData("<{0}:TabSlider ID=""tabSlider"" runat=""server""> </{0}:TabSlider>")> _
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class TabSlider
        Inherits Panel
#End If

#Region "Properties"
        Private _ConnectByClass As Boolean

        ''' <summary>
        ''' True to select the class rather than ID for the tabslider
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("True to select the class rather than ID for the tabslider")> _
        Public Property ConnectByClass() As Boolean
            Get
                Return _ConnectByClass
            End Get
            Set(ByVal value As Boolean)
                _ConnectByClass = value
            End Set
        End Property
        Private _HandleCssClass As String

        ''' <summary>
        ''' class of the element that will be your tab: an anchor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("class of the element that will be your tab: an anchor")> _
        Public Property HandleCssClass() As String
            Get
                Return _HandleCssClass
            End Get
            Set(ByVal value As String)
                _HandleCssClass = value
            End Set
        End Property

        Private _tabLocation As Position

        ''' <summary>
        ''' side of screen where tab lives
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("side of screen where tab lives")> _
        Public Property TabLocation() As Position
            Get
                Return _tabLocation
            End Get
            Set(ByVal value As Position)
                _tabLocation = value
            End Set
        End Property

        Private _action As MouseAction

        ''' <summary>
        ''' action to trigger animation
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("action to trigger animation")> _
        Public Property Action() As MouseAction
            Get
                Return _action
            End Get
            Set(ByVal value As MouseAction)
                _action = value
            End Set
        End Property

        Private _speed As Integer

        ''' <summary>
        ''' speed of animation in ms
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("speed of animation in ms")> _
        Public Property Speed() As Integer
            Get
                Return _speed
            End Get
            Set(ByVal value As Integer)
                _speed = value
            End Set
        End Property

        Private _pathToTabImage As String

        ''' <summary>
        ''' path to the image for the tab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("path to the image for the tab")> _
        Public Property ImageUrl() As String
            Get
                Return _pathToTabImage
            End Get
            Set(ByVal value As String)
                _pathToTabImage = value
            End Set
        End Property

        Private _imageHeight As Web.UI.WebControls.Unit = New Unit(-1, UnitType.Pixel)

        ''' <summary>
        ''' height of tab image
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("height of tab image")> _
        Public Property ImageHeight() As Web.UI.WebControls.Unit
            Get
                Return _imageHeight
            End Get
            Set(ByVal value As Web.UI.WebControls.Unit)
                _imageHeight = value
            End Set
        End Property

        Private _imageWidth As Web.UI.WebControls.Unit = New Unit(-1, UnitType.Pixel)

        ''' <summary>
        ''' width of tab image
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("width of tab image")> _
        Public Property ImageWidth() As Web.UI.WebControls.Unit
            Get
                Return _imageWidth
            End Get
            Set(ByVal value As Web.UI.WebControls.Unit)
                _imageWidth = value
            End Set
        End Property

        Private _topPos As Web.UI.WebControls.Unit

        ''' <summary>
        ''' position from the top. use if tabLocation is left or right
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("position from the top. use if tabLocation is left or right")> _
        Public Property TopPosition() As Web.UI.WebControls.Unit
            Get
                Return _topPos
            End Get
            Set(ByVal value As Web.UI.WebControls.Unit)
                _topPos = value
            End Set
        End Property

        Private _leftPos As Web.UI.WebControls.Unit

        ''' <summary>
        ''' position from left. use if tabLocation is bottom or top
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("position from left. use if tabLocation is bottom or top")> _
        Public Property LeftPosition() As Web.UI.WebControls.Unit
            Get
                Return _leftPos
            End Get
            Set(ByVal value As Web.UI.WebControls.Unit)
                _leftPos = value
            End Set
        End Property

        Private _fixedPosition As Boolean

        ''' <summary>
        ''' true makes it stick(fixed position) on scroll
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("true makes it stick(fixed position) on scroll")> _
        Public Property FixedScrollPosition() As Boolean
            Get
                Return _fixedPosition
            End Get
            Set(ByVal value As Boolean)
                _fixedPosition = value
            End Set
        End Property

        Private _onloadSlideOut As Boolean

        ''' <summary>
        ''' True makes the tab slide out after loading
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("True makes the tab slide out after loading")> _
        Public Property OnLoadSlideOut() As Boolean
            Get
                Return _onloadSlideOut
            End Get
            Set(ByVal value As Boolean)
                _onloadSlideOut = value
            End Set
        End Property
#End Region

#Region "Enums"
        Public Enum MouseAction As Integer
            click = 0
            hover = 1
        End Enum

        Public Enum Position As Integer
            left = 0
            right = 1
            top = 2
            bottom = 3
        End Enum
#End Region

        Private options As String = ""

        Private Sub DTITabSlider_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIMiniControls/jquery.tabSlideOut.v1.3.js")

            If String.IsNullOrEmpty(Me.CssClass) AndAlso Not ConnectByClass Then
                Me.CssClass = Me.ClientID
            ElseIf String.IsNullOrEmpty(Me.CssClass) Then
                Me.CssClass = "Default"
            End If
            If ImageWidth.Value = -1.0 Then
                ImageWidth = New Unit(16, UnitType.Pixel)
            End If
            If ImageHeight.Value = -1.0 Then
                ImageHeight = New Unit(75, UnitType.Pixel)
            End If
            If HandleCssClass Is Nothing Then
                HandleCssClass = "DTITabSliderHandle-" & Me.CssClass
            End If
            If ImageUrl Is Nothing AndAlso (TabLocation = Position.top OrElse TabLocation = Position.bottom) Then
			ImageUrl = BaseClasses.Scripts.ScriptsURL() & "DTIMiniControls/SlideHorz.png"
		ElseIf ImageUrl Is Nothing AndAlso (TabLocation = Position.left OrElse TabLocation = Position.right) Then
			ImageUrl = BaseClasses.Scripts.ScriptsURL() & "DTIMiniControls/SlideVert.png"
		End If
            If Speed = 0 Then
                Speed = 300
            End If

            Me.Controls.Add(New LiteralControl(" <a class=""" & HandleCssClass & """ href=""#"">Content</a> "))
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim selector As String = ""
            Dim opts As String = ""

            If ConnectByClass Then
                selector = "." & Me.CssClass
            Else
                selector = "#" & Me.ClientID
            End If

            opts &= "<script type=""text/javascript""> " & vbCrLf
            opts &= "        $(function(){ " & vbCrLf
            opts &= "            $('" & selector & "').tabSlideOut({ " & vbCrLf
            opts &= "                pathToTabImage: '" & ImageUrl & "'," & vbCrLf
            opts &= "                imageHeight: '" & ImageHeight.ToString & "'," & vbCrLf
            opts &= "                imageWidth: '" & ImageWidth.ToString & "'," & vbCrLf
            If TabLocation <> Position.left Then
                opts &= "                tabLocation: '" & getEnumName(TabLocation) & "'," & vbCrLf
            End If
            If Speed <> 300 Then
                opts &= "                speed: " & Speed & "," & vbCrLf
            End If
            If Action <> MouseAction.click Then
                opts &= "                action: '" & getEnumName(Action) & "'," & vbCrLf
            End If
            If TopPosition.Value > 0 Then
                opts &= "                topPos: '" & TopPosition.ToString & "'," & vbCrLf
            End If
            If LeftPosition.Value > 0 Then
                opts &= "                leftPos: '" & LeftPosition.ToString & "'," & vbCrLf
            End If
            If FixedScrollPosition Then
                opts &= "                fixedPosition: " & FixedScrollPosition.ToString.ToLower & "," & vbCrLf
            End If
            If OnLoadSlideOut Then
                opts &= "                onLoadSlideOut: " & OnLoadSlideOut.ToString.ToLower & "," & vbCrLf
            End If
            opts &= "                tabHandle: '." & HandleCssClass & "'" & vbCrLf
            opts &= "        }); " & vbCrLf
            opts &= "    }); " & vbCrLf
            opts &= "</script>" & vbCrLf

            writer.Write(opts)
            MyBase.Render(writer)
        End Sub

        Private Function getEnumName(ByVal enumeration As Object) As String
            Return [Enum].GetName(enumeration.GetType, enumeration)
        End Function
    End Class

