Public Class Scroller
    Inherits Panel

    Private _top As Unit = New Unit(0, UnitType.Pixel)

    ''' <summary>
    ''' Height from top of page where the scroller will become static.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Height from top of page where the scroller will become static.")> _
    Public Property Top As Unit
        Get
            Return _top
        End Get
        Set(value As Unit)
            _top = value
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

    Private Sub Scroller_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, _
            "  var a = function() { " & _
            "    var b = $(window).scrollTop() + " & Top.Value & "; " & _
            "    var d = $(""#" & Me.ClientID & "-anchor"").offset().top; " & _
            "    var c = $(""#" & Me.ClientID & """); " & _
            "    if (b>d) { " & _
            "      c.css({position:""fixed"",top:""" & Top.ToString & """}) " & _
            "    } else { " & _
            "      if (b<=d) { " & _
            "        c.css({position:""relative"",top:""""}) " & _
            "      } " & _
            "    } " & _
            "  }; " & _
            "  $(window).scroll(a);a() ")
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write("<div id=""" & Me.ClientID & "-anchor""></div>")
        MyBase.Render(writer)
    End Sub

End Class
