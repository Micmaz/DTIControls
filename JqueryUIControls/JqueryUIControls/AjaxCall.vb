

''' <summary>
''' An ajax call back to current page. Page is terminated after init. Elements can be cahnged on the page but must be added to the watched control list. The call can be made from javascript by calling the control id
''' ex:   ajaxcall(StringData);
''' The event is raised on the server as the event "callback(ByVal sender As JqueryUIControls.AjaxCall, ByVal query As String)" 
''' The js return method recieves data back via sender.respond("ok")
''' ex:   function jsReturn(dataFromServer){ alert(dataFromServer); }
''' 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("An ajax call back to current page. Page is terminated after init. Elements can be cahnged on the page but must be added to the watched control list. The call can be made from javascript by calling the control id ex:  ajaxcall(StringData); The event is raised on the server as the event ""callback(ByVal sender As JqueryUIControls.AjaxCall, ByVal query As String)""  The js return method recieves data back via sender.respond(""ok"") ex:  function jsReturn(dataFromServer){ alert(dataFromServer); }"), ToolboxData("<{0}:AjaxCall ID=""ajaxcall"" runat=""server""/>")> _
Public Class AjaxCall
    Inherits Control

#Region "Properties"

    Enum returnCall
        PageChanges
        Javascript
        CustomText
    End Enum

    Private returnFormatValue As returnCall
    Public Property returnFormat() As returnCall
        Get
            If renderControlsBack Then Return returnCall.PageChanges
            Return returnFormatValue
        End Get
        Set(ByVal value As returnCall)
            returnFormatValue = value
            If value = returnCall.PageChanges Then
                Me.renderControlsBack = True
            End If
        End Set
    End Property

    Public ReadOnly Property postURL()
        Get
            Return Source.Trim("""")
        End Get
    End Property

    Private _source As String = ""
    Private Property Source() As String
        Get
            If Me.DesignMode Then Return _source
            If _source = "" Then
                Try
                    If Page.Request.QueryString.Count > 0 AndAlso Page.Request.QueryString("ajaxCtrl") Is Nothing Then
                        _source = """" & Page.Request.Url.PathAndQuery & "&ajaxCtrl=" & Me.ClientID & "&term="""
                    Else
                        _source = """" & Page.Request.Url.AbsolutePath & "?ajaxCtrl=" & Me.ClientID & "&term="""
                    End If
                Catch ex As Exception
                End Try
            End If
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property

    Private ReadOnly Property query() As String
        Get
            If Page Is Nothing Then Return Nothing
            If Me.DesignMode Then Return Nothing
            Return DecodeString(Page.Request.QueryString("term"))
        End Get
    End Property

    Private ReadOnly Property ctrlid() As String
        Get
            If Me.DesignMode Then Return Nothing
            If Page Is Nothing Then Return Nothing
            Return Page.Request.QueryString("ajaxCtrl")
        End Get
    End Property

    Private _javascriptCallFunction As String
    Public Property jsFunction() As String
        Get
            If _javascriptCallFunction = "" Then
                If Me.ID Is Nothing Then
                    Return "ajax"
                Else
                    _javascriptCallFunction = Me.ID
                End If
            End If
            Return _javascriptCallFunction
        End Get
        Set(ByVal value As String)
            _javascriptCallFunction = value
        End Set
    End Property

    Private _javascriptCallCompleteFunction As String
    Public Property jsReturnFunction() As String
        Get
            Return _javascriptCallCompleteFunction
        End Get
        Set(ByVal value As String)
            _javascriptCallCompleteFunction = value
        End Set
    End Property

    Private _watchedControlList As New List(Of Control)
    Public ReadOnly Property watchedControlList() As List(Of Control)
        Get
            Return _watchedControlList
        End Get
    End Property

    Private _returnedParms As Hashtable

    ''' <summary>
    ''' Stores values from other controls for the search event
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Stores values from other controls for the search event")> _
    Public ReadOnly Property returnedParms() As Hashtable
        Get
            If _returnedParms Is Nothing Then _returnedParms = New Hashtable
            Return _returnedParms
        End Get
    End Property

    Private _renderControlsBack As Boolean = False
    Public Property renderControlsBack() As Boolean
        Get
            Return _renderControlsBack
        End Get
        Set(ByVal value As Boolean)
            _renderControlsBack = value
        End Set
    End Property


    Private javascriptCallTimerValue As Integer = -1
    Public Property javascriptCallTimer() As Integer
        Get
            Return javascriptCallTimerValue
        End Get
        Set(ByVal value As Integer)
            javascriptCallTimerValue = value
        End Set
    End Property

    Private _isAsync As Boolean = True
    Public Property isAsync As Boolean
        Get
            Return _isAsync
        End Get
        Set(value As Boolean)
            _isAsync = value
        End Set
    End Property
#End Region

#Region "Render Controls Back"

    Private ep As emptypage
    Private Function RenderControltoString(ByVal ctrl As Control) As String
        If ep Is Nothing Then ep = New emptypage
        Return ep.RenderUserControlToString(ctrl)
    End Function

    Private Class emptypage
        Inherits System.Web.UI.Page
        Public Function RenderUserControlToString(ByVal ctrl As Control) As String
            setcontrol(ctrl)

            Dim sb As StringBuilder = New StringBuilder()
            Dim tw As IO.StringWriter = New IO.StringWriter(sb)
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)

            ctrl.RenderControl(hw)
            Return sb.ToString()
        End Function

        Private Sub setcontrol(ByVal ctrl As Control)
            ctrl.Page = Me
            For Each c As Control In ctrl.Controls
                setcontrol(c)
            Next
        End Sub

        Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        End Sub

        Public Overrides Property EnableEventValidation() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
                '         	  Do nothing */
            End Set
        End Property
    End Class

#End Region

    Public Event callBack(ByVal sender As AjaxCall, ByVal value As String)

    ''' <summary>
    ''' Registers all necessary javascript and css files for this control to function on the page.
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Registers all necessary javascript and css files for this control to function on the page.")> _
    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.autoCompleteHelper.js")
        End If
    End Sub

    Private Sub Control_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        checkpost()
        registerControl(Me.Page)
    End Sub

    Public Sub addControlsToWatchList(ByVal ParamArray controls As Control())
        For Each ctrl As Control In controls
            If ctrl IsNot Nothing Then
                Me.watchedControlList.Add(ctrl)
                For Each innerctl As Control In ctrl.Controls
                    addControlsToWatchList(innerctl)
                Next
            End If
        Next
    End Sub

    Public Shared Function DecodeString(ByVal input As String) As String
        If input Is Nothing Then Return Nothing
        Return HttpUtility.UrlDecode(input)
    End Function

    Private Sub checkpost()
        If Me.DesignMode Then Return
        If Not Page.IsPostBack AndAlso Not query Is Nothing AndAlso ctrlid = Me.ClientID Then
            For Each key As String In Me.Page.Request.QueryString.Keys
                If Not key = "ajaxCtrl" AndAlso Not key = "term" AndAlso Not key = "_" Then
                    Dim val As String = DecodeString(Me.Page.Request.QueryString(key))
                    Me.returnedParms.Add(key, val)
                    Try
                        Dim ctrl As Control = Me.FindControl(key)
                        If ctrl IsNot Nothing Then
                            watchedControlList.Add(ctrl)
                            If ctrl.GetType().GetProperty("Text") IsNot Nothing Then
                                ctrl.GetType().GetProperty("Text").SetValue(ctrl, val, Nothing)
                            End If
                            If ctrl.GetType().GetProperty("Value") IsNot Nothing Then
                                ctrl.GetType().GetProperty("Value").SetValue(ctrl, val, Nothing)
                            End If
                            If ctrl.GetType().GetProperty("SelectedValue") IsNot Nothing Then
                                ctrl.GetType().GetProperty("SelectedValue").SetValue(ctrl, val, Nothing)
                            End If
                            If ctrl.GetType().GetProperty("Checked") IsNot Nothing Then
                                ctrl.GetType().GetProperty("Checked").SetValue(ctrl, Boolean.Parse(val), Nothing)
                            End If
                            If GetType(DropDownList).IsAssignableFrom(ctrl.GetType()) Then
                                Dim ctrl1 As DropDownList = ctrl
                                ctrl1.Items.Add(val)
                                ctrl1.SelectedValue = val
                            ElseIf GetType(Autocomplete).IsAssignableFrom(ctrl.GetType()) Then
                                Dim ctrl1 As Autocomplete = ctrl
                                ctrl1.Value = DecodeString(Me.Page.Request.QueryString(key & "_hidden"))
                            End If
                            'Dim ctrl As Control = Me.Page.GetType().GetMember(key).GetValue(Nothing, Nothing)
                            'ctrl.GetType().GetMember("text").SetValue(Me.Page.Request.QueryString(key), 0)
                        End If

                    Catch ex As Exception

                    End Try
                End If
            Next
            RaiseEvent callBack(Me, query)
            Page.Response.End()
        End If
    End Sub

    Private Sub javascriptAjaxCall_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim completeFunction As String = ""
        If Not jsReturnFunction = "" Then
            completeFunction = jsReturnFunction
            If completeFunction.IndexOf("(") = -1 Then
                completeFunction &= "(msg.split('####')[0])"
            End If
        End If

        Dim ctrlList As String = ""
        For Each ctrl As Control In Me.watchedControlList
            Dim checked As String = ""
            If GetType(CheckBox).IsAssignableFrom(ctrl.GetType()) Then
                checked = ",true"
            ElseIf GetType(Autocomplete).IsAssignableFrom(ctrl.GetType()) Then
                ctrlList &= " src=addvalueToSrc('" & ctrl.ClientID & "_hidden','" & ctrl.ID & "_hidden',src" & checked & ");" & vbCrLf
            End If
            ctrlList &= " src=addvalueToSrc('" & ctrl.ClientID & "','" & ctrl.ID & "',src" & checked & ");" & vbCrLf
        Next

        'Renders controls back that are in the watch list.
        If renderControlsBack OrElse Me.returnFormat = returnCall.Javascript Then
            completeFunction = ";if(msg.length > 0)eval(msg.split('####')[1]);" & completeFunction
        End If

        Dim asyncStr As String = ""
        If Not isAsync Then
            asyncStr = "   async: false, " & vbCrLf
        End If

        'src+=parmName+"="+$("#"+elementID).val();
        Dim s As String = ""
		s = "window." & Me.jsFunction & " = function(input){ " & vbCrLf &
		" var str=escapestr(input); " & vbCrLf &
		" var src=" & Source & "+str; " & vbCrLf &
		ctrlList &
		" $.ajax({ " & vbCrLf &
		"   url: src, " & vbCrLf &
		"   cache: false, " & vbCrLf &
		asyncStr &
		"   success:  function(msg){ " & completeFunction & "; }" & vbCrLf &
		"  }); " & vbCrLf &
		"} " & vbCrLf
		jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, s)
		If javascriptCallTimerValue > 0 Then
            If javascriptCallTimerValue < 1000 Then javascriptCallTimerValue = 1000
			jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, "setInterval( '" & Me.jsFunction & "()'," & Me.javascriptCallTimerValue & "); ")
		End If

	End Sub

    ''' <summary>
    ''' Decodes a given string from URL encoding 
    ''' </summary>
    ''' <param name="value">
    ''' String to be decoded
    ''' </param>
    ''' <returns>
    ''' Decoded string
    ''' </returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Decodes a given string from URL encoding")> _
    Public Shared Function decodeFromURL(ByVal value As String) As String
        Return HttpUtility.UrlDecode(value, System.Text.Encoding.Default)
    End Function

    ''' <summary>
    ''' Text to display matching the users current query
    ''' </summary>
    ''' <param name="returnValue"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Text to display matching the users current query")> _
    Public Sub respond(ByVal returnValue As String, Optional ByVal javascript As String = "")
        'Page.Response.Clear()
        If renderControlsBack Then

            For Each ctrl As Control In Me.watchedControlList
                If Not ctrl Is Nothing Then
					javascript &= jQueryLibrary.jQueryInclude.jqueryVar & "('#" & ctrl.ClientID & "').replaceWith(unescape('" & BaseClasses.BaseSecurityPage.JavaScriptEncode(RenderControltoString(ctrl)) & "'));"
				End If
            Next
            'returnValue = ret
        End If
        If javascript = "" AndAlso returnValue = "" Then
        Else
            Page.Response.Write(returnValue & "####" & javascript)
        End If

        'Page.Response.End()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

    End Sub
End Class
