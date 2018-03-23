Public Class jsPlumb
    Inherits Panel

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		'Dim str As String = "<script type=""text/javascript"">$(function(){"
		MyBase.Render(writer)
	End Sub

	Private Sub Calendar_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
		jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, getScript())
	End Sub

	Public Function getScript() As String
		Dim str As String = ""
		Dim id As String = "" & Me.ID
		If id = "" Then id = ClientID

		str &= "var " & Me.ID & " = jsPlumb.getInstance();" & vbCrLf
		str &= id & ".Defaults.Container = """ & Me.ClientID & """;"
		For Each c As Connection In connections
			str &= Me.ID & ".connect({ source:""" & c.sourceID & """, target:""" & c.destinationID & """ });" & vbCrLf
		Next
		'str &= "});</script>"
		Return str
	End Function

	''' <summary>
	''' Registers all necessary javascript and css files for this control to function on the page.
	''' </summary>
	''' <param name="page"></param>
	''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.jsPlumb-1.3.3-all-min.js", , True)
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Public connections As New Generic.List(Of Connection)
    Public endPoints As New Generic.List(Of EndPoint)


    Public Class EndPoint
        Inherits propertyListItem
        Public Sub New()

        End Sub

        Public Sub New(ByVal t As EndPointType, Optional ByVal width As Integer = 15, Optional ByVal Height As Integer = 15, Optional ByVal cssClass As String = "")
            Me.endpoint = t
            Me.width = width
            Me.height = Height
            Me.cssClass = cssClass
        End Sub

        Public Sub New(ByVal imgurl As String)
            Me.endpoint = EndPointType.Image
            Me.src = imgurl
        End Sub

        Public Enum EndPointType
            Dot
            Rectangle
            Image
        End Enum

        Public Enum AnchorPosition
            TopCenter
            TopRight
            RightMiddle
            BottomRight
            BottomCenter
            BottomLeft
            LeftMiddle
            TopLeft
            Center
        End Enum

        Public anchor As List(Of AnchorPosition)
        Public endpoint As EndPointType = EndPointType.Dot
        Public width As Integer
        Public height As Integer
        Public src As String
        Public cssClass As String
        Public isSource As Boolean = True
        Public isTarget As Boolean = True
        Public dragAllowedWhenFull As Boolean = True
        Private elementID As String = Nothing

        'Public Function options() As String
        '    Dim str As String = ""
        '    If pointType = EndPointType.Dot Then
        '        str &= """Dot"", { radius:" & width & " }"
        '    ElseIf pointType = EndPointType.Rectangle Then
        '        str &= """Rectangle"", { width:" & width & ", height:" & height & " }"
        '    ElseIf pointType = EndPointType.Image Then
        '        str &= """Image"", { src:""" & imageSrc & """ }"
        '    End If
        '    If Not cssClass = "" Then
        '        str &= ", cssClass:""" & cssClass & """"
        '    End If
        'End Function

    End Class

    Public Class Overlay
        Inherits propertyListItem
        Public Enum OverlayType
            None
            Arrow
            Label
            PlainArrow
            Diamond
        End Enum

        Public overlayStyle As OverlayType = OverlayType.None
        Public id As String
        Public width As Integer = -1
        Public length As Integer = -1
        Public location As Integer = -1
        Public label As String = ""
        Public sourceToDestination As Boolean = True
        Public foldback As Double = 0.623
        Public paint As PaintStyle


    End Class

    Public Class Connector
        Inherits propertyListItem
        Public Enum connectorType
            Bezier
            Straight
            Flowchart
        End Enum

    End Class

    Public Class PaintStyle
        Inherits propertyListItem
        Public width As Integer
        Public height As Integer
        Public fillStyle As String
        Public lineWidth As Integer
        Public outlineWidth As Integer
        Public strokeStyle As String
    End Class

    Public Class Connection

        Private _source As String
        Public Property sourceID() As String
            Get
                Return _source
            End Get
            Set(ByVal value As String)
                _source = value
            End Set
        End Property

        Private _destination As String
        Public Property destinationID() As String
            Get
                Return _destination
            End Get
            Set(ByVal value As String)
                _destination = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal source As String, ByVal destination As String)
            Me.sourceID = source
            Me.destinationID = destination
        End Sub

        Public Sub New(ByVal source As Control, ByVal destination As Control)
            Me.sourceID = source.ClientID
            Me.destinationID = destination.ClientID
        End Sub

    End Class


End Class

Public Class propertyListItem
 
    Public Overrides Function toString() As String
        Return "[" & options() & "]"
    End Function

    Public Function getFieldVal(ByVal value As Object) As String
        If TypeOf value Is String Then
            Return """" & value & """"
        ElseIf TypeOf value Is propertyListItem Then
            Return "{" & CType(value, propertyListItem).options() & "}"
        ElseIf TypeOf value Is [Enum] Then
            Return """" & [Enum].GetName(value.GetType, value) & """"
        ElseIf TypeOf value Is IList Then
            Dim str As String = "["
            For Each listval As Object In CType(value, IList)
                str &= getFieldVal(listval) & ","
            Next
            Return str.Trim(",") & "]"
        Else
            Return value.ToString
        End If
        Return ""
    End Function

    Public Function options() As String
        Dim str As String = ""
        For Each f As Reflection.FieldInfo In Me.GetType().GetFields()
            Dim key As String = f.Name
            Dim val As Object = f.GetValue(Me)
            If Not val Is Nothing Then
                str &= key & ":" & getFieldVal(val) & ","
            End If
        Next
        str = str.Trim(",")
        Return str
    End Function



End Class
