Imports System.Web.Configuration
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Web
Imports System.Configuration

''' <summary>
''' Generic error handler class. Can be overridden to handel errors from all classes derived from a baseclass type.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ErrorHandlerBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ErrorHandlerBase
#End If
        Private _application As Web.HttpApplication
        Public Property application() As Web.HttpApplication
            Get
                Return _application
            End Get
            Set(ByVal value As Web.HttpApplication)
                _application = value
                AddHandler value.Error, AddressOf errorEvent
            End Set
        End Property

        'Public Overridable Sub errorEvent(ByVal sender As Object, ByVal e As EventArgs)
        '    Dim x As Integer = 0
        '    Dim y As String = x + 1

        'End Sub



        Private _erroremail As String = Nothing
        Public Property erroremail() As String
            Get
                If _erroremail Is Nothing Then
                    Return WebConfigurationManager.AppSettings("ErrorEmail")
                Else : Return _erroremail
                End If
            End Get
            Set(ByVal value As String)
                _erroremail = value
            End Set
        End Property

        Protected Overridable Sub errorEvent(ByVal sender As Object, ByVal e As System.EventArgs)
            'Catch the exception
            Try
                If Not WebConfigurationManager.AppSettings("ErrorEmail") Is Nothing Then
                    Dim ex As Exception = application.Server.GetLastError()
                    Dim errorto As String = WebConfigurationManager.AppSettings("ErrorEmail")
                    Dim appname As String = "Myapp"
                    Try
                        'ugly. just gets the application directory. should probably get the assembly name.
                        appname = application.Request.PhysicalApplicationPath.Substring(application.Request.PhysicalApplicationPath.Substring(0, application.Request.PhysicalApplicationPath.Length - 1).LastIndexOf("\")).Replace("\", "")
                    Catch ex1 As Exception
                    End Try
                    If appname.Length = 0 Then
                        appname = "Myapp"
                    End If
                    Dim smtpcli As System.Net.Mail.SmtpClient
                    If WebConfigurationManager.AppSettings("ErrorEmailServer") Is Nothing Then
                        smtpcli = New System.Net.Mail.SmtpClient("localhost")
                    Else
                        smtpcli = New System.Net.Mail.SmtpClient(WebConfigurationManager.AppSettings("ErrorEmailServer"))
                    End If
                    If Not WebConfigurationManager.AppSettings("ErrorEmailServerUser") Is Nothing AndAlso Not WebConfigurationManager.AppSettings("ErrorEmailServerPassword") Is Nothing Then
                        smtpcli.Credentials = New System.Net.NetworkCredential( _
                            WebConfigurationManager.AppSettings("ErrorEmailServerUser"), WebConfigurationManager.AppSettings("ErrorEmailServerPassword"))
                    End If

                    Dim Subject As String = "Error in Application: " & appname & " Page: " & application.Request.RawUrl & " Error: " & ex.GetType.Name
                    Dim body As String = "There was an error in: <br>" & appname & "<br><br>"
                    ' request:" & application.Request.RawUrl & "<br> from: <br> ip: " & application.Request.UserHostAddress & "(" & application.Request.UserHostName & ") <br> error: <br> "
                    'body &= ex.Message & "<br>Stacktrace:<br> " & ex.StackTrace & "<br>Source:<br>" & ex.Source & "<br>"
                    body &= ExceptionToString(ex)
                    Dim i As Integer
                    body &= "<br><br>---- Stack Trace ----<br>"
                    body &= ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
                    For i = 0 To 5
                        If Not ex.InnerException Is Nothing Then
                            ex = ex.InnerException
                            body &= "<br><br>---- Stack Trace ----<br>"
                            body &= ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
                        End If
                    Next
                    'System.Web.Mail.SmtpMail.Send("error@" & appname & ".com", errorto, Subject, body)
                    Dim from As String = WebConfigurationManager.AppSettings("ErrorFromEmail")
                    If from Is Nothing Then
                        If Not HttpContext.Current.Request.ServerVariables("SERVER_NAME") Is Nothing Then
                            Dim servernm As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
                            Dim parts() As String = servernm.Split(".")
                            If parts.Length > 1 Then
                                from = "error@" & parts(parts.Length - 2) & parts(parts.Length - 1)
                            Else
                                from = "error@" & appname & ".com"
                            End If
                        Else
                            from = "error@" & appname & ".com"
                        End If
                    End If

                    body = body.Replace("<br>", vbCrLf)
                    'body = body.Replace("<br>", "<br>" & vbCrLf)
                    smtpcli.Send(from, errorto, Subject, body)
                End If
            Catch ex2 As Exception
            End Try

            'If WebConfigurationManager.AppSettings("hideErrors") = "Y" Then
            '    Dim ex As Exception = Server.GetLastError()

            '    If Not IsNothing(ex) Then
            '        If Not Session Is Nothing Then _
            '        Session.Add("LastException", ex)

            '        'We must now manually render the page. When an unhandled
            '        'exception occurs, the Page_Render method is not fired. The
            '        'following code renders manually.

            '        Dim sb As New System.Text.StringBuilder
            '        Dim sw As New System.IO.StringWriter(sb)
            '        Dim htmltw As New HtmlTextWriter(sw)
            '        'fire the PreRender event
            '        Me.OnPreRender(New EventArgs)
            '        'Render the actual page controls
            '        Dim body As String = ex.Message & "<br>" & ex.StackTrace & "<br>" & ex.Source & "<br>"
            '        htmltw.Write("<!--" & vbCrLf & _
            '        body & _
            '        "// -->" & vbCrLf)
            '        'htmltw.Write("<script language=""JavaScript""> " & vbCrLf & _
            '        '"<!--" & vbCrLf & _
            '        '"window.open('" & Request.ApplicationPath & "/Popuperror.aspx','ErrorWindow','height=320,width=320,scrollbars,resizable');  " & _
            '        '"// -->" & vbCrLf & _
            '        '"</script>")
            '        Me.Render(htmltw)
            '        Response.Write(sb.ToString)
            '        Context.ClearError()
            '    End If
            'End If

        End Sub

#Region "Error Formatters"

        Private Const _strViewstateKey As String = "__VIEWSTATE"
        Private Const _strRootException As String = "System.Web.HttpUnhandledException"
        Private Const _strRootWsException As String = "System.Web.Services.Protocols.SoapException"

    ''' <summary>
    ''' turns a single stack frame object into an informative string
    ''' </summary>
    ''' <param name="sf"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("turns a single stack frame object into an informative string")> _
        Private Function StackFrameToString(ByVal sf As StackFrame) As String
            Dim sb As New StringBuilder
            Dim intParam As Integer
            Dim mi As MemberInfo = sf.GetMethod

            With sb
                '-- build method name
                .Append("   ")
                .Append(mi.DeclaringType.Namespace)
                .Append(".")
                .Append(mi.DeclaringType.Name)
                .Append(".")
                .Append(mi.Name)

                '-- build method params
                .Append("(")
                intParam = 0
                For Each param As ParameterInfo In sf.GetMethod.GetParameters()
                    intParam += 1
                    If intParam > 1 Then .Append(", ")
                    .Append(param.Name)
                    .Append(" As ")
                    .Append(param.ParameterType.Name)
                Next
                .Append(")")
                .Append(Environment.NewLine)

                '-- if source code is available, append location info
                .Append("       ")
                If sf.GetFileName Is Nothing OrElse sf.GetFileName.Length = 0 Then
                    .Append("(unknown file)")
                    '-- native code offset is always available
                    .Append(": N ")
                    .Append(String.Format("{0:#00000}", sf.GetNativeOffset))

                Else
                    .Append(System.IO.Path.GetFileName(sf.GetFileName))
                    .Append(": line ")
                    .Append(String.Format("{0:#0000}", sf.GetFileLineNumber))
                    .Append(", col ")
                    .Append(String.Format("{0:#00}", sf.GetFileColumnNumber))
                    '-- if IL is available, append IL location info
                    If sf.GetILOffset <> StackFrame.OFFSET_UNKNOWN Then
                        .Append(", IL ")
                        .Append(String.Format("{0:#0000}", sf.GetILOffset))
                    End If
                End If
                .Append(Environment.NewLine)
            End With
            Return sb.ToString
        End Function


    ''' <summary>
    ''' enhanced stack trace generator
    ''' </summary>
    ''' <param name="st"></param>
    ''' <param name="strSkipClassName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("enhanced stack trace generator")> _
        Private Overloads Function EnhancedStackTrace(ByVal st As StackTrace, _
            Optional ByVal strSkipClassName As String = "") As String
            Dim intFrame As Integer

            Dim sb As New StringBuilder

            sb.Append(Environment.NewLine)
            sb.Append("---- Stack Trace ----")
            sb.Append(Environment.NewLine)

            For intFrame = 0 To st.FrameCount - 1
                Dim sf As StackFrame = st.GetFrame(intFrame)
                Dim mi As MemberInfo = sf.GetMethod

                If strSkipClassName <> "" AndAlso mi.DeclaringType.Name.IndexOf(strSkipClassName) > -1 Then
                    '-- don't include frames with this name
                Else
                    sb.Append(StackFrameToString(sf))
                End If
            Next
            sb.Append(Environment.NewLine)

            Return sb.ToString
        End Function

    ''' <summary>
    ''' enhanced stack trace generator, using existing exception as start point
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("enhanced stack trace generator, using existing exception as start point")> _
        Private Overloads Function EnhancedStackTrace(ByVal ex As Exception) As String
            Return EnhancedStackTrace(New StackTrace(ex, True))
        End Function

    ''' <summary>
    ''' enhanced stack trace generator, using current execution as start point
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("enhanced stack trace generator, using current execution as start point")> _
        Private Overloads Function EnhancedStackTrace() As String
            Return EnhancedStackTrace(New StackTrace(True), "ASPUnhandledException")
        End Function

    ''' <summary>
    ''' returns current URL http://localhost:85/mypath/mypage.aspx?test=1&apples=bear
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("returns current URL http://localhost:85/mypath/mypage.aspx?test=1&apples=bear")> _
        Private Function WebCurrentUrl() As String
            Dim strUrl As String
            With HttpContext.Current.Request.ServerVariables
                strUrl = "http://" & .Item("server_name")
                If .Item("server_port") <> "80" Then
                    strUrl &= ":" & .Item("server_port")
                End If
                strUrl &= .Item("url")
                If .Item("query_string").Length > 0 Then
                    strUrl &= "?" & .Item("query_string")
                End If
            End With
            Return strUrl
        End Function

    ''' <summary>
    ''' returns brief summary info for all assemblies in the current AppDomain
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("returns brief summary info for all assemblies in the current AppDomain")> _
        Private Function AllAssemblyDetailsToString() As String
            Dim sb As New StringBuilder
            Dim nvc As Specialized.NameValueCollection
            Const strLineFormat As String = "    {0, -30} {1, -15} {2}"

            sb.Append(Environment.NewLine)
            sb.Append(String.Format(strLineFormat, _
                "Assembly", "Version", "BuildDate"))
            sb.Append(Environment.NewLine)
            sb.Append(String.Format(strLineFormat, _
                "--------", "-------", "---------"))
            sb.Append(Environment.NewLine)

            For Each a As Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies()
                nvc = AssemblyAttribs(a)
                '-- assemblies without versions are weird (dynamic?)
                If nvc("Version") <> "0.0.0.0" Then
                    sb.Append(String.Format(strLineFormat, _
                        IO.Path.GetFileName(nvc("CodeBase")), _
                        nvc("Version"), _
                        nvc("BuildDate")))
                    sb.Append(Environment.NewLine)
                End If
            Next

            Return sb.ToString
        End Function

        ''' <summary>
        ''' returns more detailed information for a single assembly
        ''' </summary>
        <System.ComponentModel.Description("returns more detailed information for a single assembly")> _
        Private Function AssemblyDetailsToString(ByVal a As Reflection.Assembly) As String
            Dim sb As New StringBuilder
            Dim nvc As Specialized.NameValueCollection = AssemblyAttribs(a)

            With sb
                .Append("Assembly Codebase:     ")
                Try
                    .Append(nvc("CodeBase"))
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Assembly Full Name:    ")
                Try
                    .Append(nvc("FullName"))
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Assembly Version:      ")
                Try
                    .Append(nvc("Version"))
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Assembly Build Date:   ")
                Try
                    .Append(nvc("BuildDate"))
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)
            End With

            Return sb.ToString
        End Function

        ''' <summary>
        ''' retrieve relevant assembly details for this exception, if possible
        ''' </summary>
        <System.ComponentModel.Description("retrieve relevant assembly details for this exception, if possible")> _
        Private Function AssemblyInfoToString(ByVal ex As Exception) As String
            '-- ex.source USUALLY contains the name of the assembly that generated the exception
            '-- at least, according to the MSDN documentation..
            Dim a As System.Reflection.Assembly = GetAssemblyFromName(ex.Source)
            If a Is Nothing Then
                Return AllAssemblyDetailsToString()
            Else
                Return AssemblyDetailsToString(a)
            End If
        End Function

        ''' <summary>
        ''' exception-safe WindowsIdentity.GetCurrent retrieval; returns "domain\username"
        ''' </summary>
        ''' <remarks>
        ''' per MS, this can sometimes randomly fail with "Access Denied" on NT4
        ''' </remarks>
        <System.ComponentModel.Description("exception-safe WindowsIdentity.GetCurrent retrieval; returns ""domain\username""")> _
        Private Function CurrentWindowsIdentity() As String
            Try
                Return System.Security.Principal.WindowsIdentity.GetCurrent.Name()
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' exception-safe System.Environment "domain\username" retrieval
        ''' </summary>
        <System.ComponentModel.Description("exception-safe System.Environment ""domain\username"" retrieval")> _
        Private Function CurrentEnvironmentIdentity() As String
            Try
                Return System.Environment.UserDomainName + "\" + System.Environment.UserName
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' retrieve Process identity with fallback on error to safer method
        ''' </summary>
        <System.ComponentModel.Description("retrieve Process identity with fallback on error to safer method")> _
        Private Function ProcessIdentity() As String
            Dim strTemp As String = CurrentWindowsIdentity()
            If strTemp = "" Then
                Return CurrentEnvironmentIdentity()
            End If
            Return strTemp
        End Function

        ''' <summary>
        ''' gather some system information that is helpful in diagnosing exceptions
        ''' </summary>
        <System.ComponentModel.Description("gather some system information that is helpful in diagnosing exceptions")> _
        Private Function SysInfoToString(Optional ByVal blnIncludeStackTrace As Boolean = False) As String
            Dim sb As New StringBuilder

            With sb
                .Append("Date and Time:         ")
                .Append(DateTime.Now)
                .Append(Environment.NewLine)

                .Append("Machine Name:          ")
                Try
                    .Append(Environment.MachineName)
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Process User:          ")
                .Append(ProcessIdentity)
                .Append(Environment.NewLine)

                .Append("Remote User:           ")
                .Append(HttpContext.Current.Request.ServerVariables("REMOTE_USER"))
                .Append(Environment.NewLine)

                .Append("Remote Address:        ")
                .Append(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"))
                .Append(Environment.NewLine)

                .Append("Remote Host:           ")
                .Append(HttpContext.Current.Request.ServerVariables("REMOTE_HOST"))
                .Append(Environment.NewLine)

                .Append("URL:                   ")
                .Append(WebCurrentUrl)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)

                .Append("NET Runtime version:   ")
                .Append(System.Environment.Version.ToString)
                .Append(Environment.NewLine)

                .Append("Application Domain:    ")
                Try
                    .Append(System.AppDomain.CurrentDomain.FriendlyName())
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                If blnIncludeStackTrace Then
                    .Append(EnhancedStackTrace())
                End If

            End With

            Return sb.ToString
        End Function

        ''' <summary>
        ''' translate an exception object to a formatted string, with additional system info
        ''' </summary>
        <System.ComponentModel.Description("translate an exception object to a formatted string, with additional system info")> _
        Private Function ExceptionToString(ByVal ex As Exception) As String
            Dim sb As New StringBuilder

            With sb
                .Append(ExceptionToStringPrivate(ex))
                '-- get ASP specific settings
                Try
                    .Append(GetASPSettings())
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)
            End With

            Return sb.ToString
        End Function


        ''' <summary>
        ''' private version, called recursively for nested exceptions (inner, outer, etc)
        ''' </summary>
        <System.ComponentModel.Description("private version, called recursively for nested exceptions (inner, outer, etc)")> _
        Private Function ExceptionToStringPrivate(ByVal ex As Exception, _
            Optional ByVal blnIncludeSysInfo As Boolean = True) As String

            Dim sb As New StringBuilder

            If Not (ex.InnerException Is Nothing) Then
                '-- sometimes the original exception is wrapped in a more relevant outer exception
                '-- the detail exception is the "inner" exception
                '-- see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/exceptdotnet.asp

                '-- don't return the outer root ASP exception; it is redundant.
                If ex.GetType.ToString = _strRootException Or ex.GetType.ToString = _strRootWsException Then
                    Return ExceptionToStringPrivate(ex.InnerException)
                Else
                    With sb
                        .Append(ExceptionToStringPrivate(ex.InnerException, False))
                        .Append(Environment.NewLine)
                        .Append("(Outer Exception)")
                        .Append(Environment.NewLine)
                    End With
                End If
            End If

            With sb
                '-- get general system and app information
                '-- we only really want to do this on the outermost exception in the stack
                If blnIncludeSysInfo Then
                    .Append(SysInfoToString)
                    .Append(AssemblyInfoToString(ex))
                    .Append(Environment.NewLine)
                End If

                '-- get exception-specific information

                .Append("Exception Type:        ")
                Try
                    .Append(ex.GetType.FullName)
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Exception Message:     ")
                Try
                    .Append(ex.Message)
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Exception Source:      ")
                Try
                    .Append(ex.Source)
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                .Append("Exception Target Site: ")
                Try
                    .Append(ex.TargetSite.Name)
                Catch e As Exception
                    .Append(e.Message)
                End Try
                .Append(Environment.NewLine)

                'Try
                '    .Append(EnhancedStackTrace(ex))
                'Catch e As Exception
                '    .Append(e.Message)
                'End Try
                .Append(Environment.NewLine)

            End With

            Return sb.ToString
        End Function

        ''' <summary>
        ''' exception-safe file attrib retrieval; we don't care if this fails
        ''' </summary>
        <System.ComponentModel.Description("exception-safe file attrib retrieval; we don't care if this fails")> _
        Private Function AssemblyLastWriteTime(ByVal a As System.Reflection.Assembly) As DateTime
            Try
                Return IO.File.GetLastWriteTime(a.Location)
            Catch ex As Exception
                Return DateTime.MaxValue
            End Try
        End Function


        ''' <summary>
        ''' returns build datetime of assembly, using calculated build time if possible, or filesystem time if not
        ''' </summary>
        <System.ComponentModel.Description("returns build datetime of assembly, using calculated build time if possible, or filesystem time if not")> _
        Private Function AssemblyBuildDate(ByVal a As System.Reflection.Assembly, _
            Optional ByVal blnForceFileDate As Boolean = False) As DateTime

            Dim v As System.Version = a.GetName.Version
            Dim dt As DateTime

            If blnForceFileDate Then
                dt = AssemblyLastWriteTime(a)
            Else
                dt = CType("01/01/2000", DateTime). _
                    AddDays(v.Build). _
                    AddSeconds(v.Revision * 2)
                If TimeZone.IsDaylightSavingTime(dt, TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)) Then
                    dt = dt.AddHours(1)
                End If
                If dt > DateTime.Now Or v.Build < 730 Or v.Revision = 0 Then
                    dt = AssemblyLastWriteTime(a)
                End If
            End If

            Return dt
        End Function

        ''' <summary>
        ''' returns string name / string value pair of all attribs for the specified assembly
        ''' </summary>
        ''' <remarks>
        ''' note that Assembly* values are pulled from AssemblyInfo file in project folder
        '''
        ''' Trademark       = AssemblyTrademark string
        ''' Debuggable      = True
        ''' GUID            = 7FDF68D5-8C6F-44C9-B391-117B5AFB5467
        ''' CLSCompliant    = True
        ''' Product         = AssemblyProduct string
        ''' Copyright       = AssemblyCopyright string
        ''' Company         = AssemblyCompany string
        ''' Description     = AssemblyDescription string
        ''' Title           = AssemblyTitle string
        ''' </remarks>
        <System.ComponentModel.Description("returns string name / string value pair of all attribs for the specified assembly")> _
        Private Function AssemblyAttribs(ByVal a As System.Reflection.Assembly) As Specialized.NameValueCollection
            Dim Name As String
            Dim Value As String
            Dim nvc As New Specialized.NameValueCollection

            For Each attrib As Object In a.GetCustomAttributes(False)
                Name = attrib.GetType().ToString()
                Value = ""
                Select Case Name
                    Case "System.Diagnostics.DebuggableAttribute"
                        Name = "Debuggable"
                        Value = CType(attrib, System.Diagnostics.DebuggableAttribute).IsJITTrackingEnabled.ToString
                    Case "System.CLSCompliantAttribute"
                        Name = "CLSCompliant"
                        Value = CType(attrib, System.CLSCompliantAttribute).IsCompliant.ToString
                    Case "System.Runtime.InteropServices.GuidAttribute"
                        Name = "GUID"
                        Value = CType(attrib, System.Runtime.InteropServices.GuidAttribute).Value.ToString
                    Case "System.Reflection.AssemblyTrademarkAttribute"
                        Name = "Trademark"
                        Value = CType(attrib, AssemblyTrademarkAttribute).Trademark.ToString
                    Case "System.Reflection.AssemblyProductAttribute"
                        Name = "Product"
                        Value = CType(attrib, AssemblyProductAttribute).Product.ToString
                    Case "System.Reflection.AssemblyCopyrightAttribute"
                        Name = "Copyright"
                        Value = CType(attrib, AssemblyCopyrightAttribute).Copyright.ToString
                    Case "System.Reflection.AssemblyCompanyAttribute"
                        Name = "Company"
                        Value = CType(attrib, AssemblyCompanyAttribute).Company.ToString
                    Case "System.Reflection.AssemblyTitleAttribute"
                        Name = "Title"
                        Value = CType(attrib, AssemblyTitleAttribute).Title.ToString
                    Case "System.Reflection.AssemblyDescriptionAttribute"
                        Name = "Description"
                        Value = CType(attrib, AssemblyDescriptionAttribute).Description.ToString
                    Case Else
                        'Console.WriteLine(Name)
                End Select
                If Value <> "" Then
                    If nvc.Item(Name) = "" Then
                        nvc.Add(Name, Value)
                    End If
                End If
            Next

            '-- add some extra values that are not in the AssemblyInfo, but nice to have
            With nvc
                .Add("CodeBase", a.CodeBase.Replace("file:///", ""))
                .Add("BuildDate", AssemblyBuildDate(a).ToString)
                .Add("Version", a.GetName.Version.ToString)
                .Add("FullName", a.FullName)
            End With

            Return nvc
        End Function

        ''' <summary>
        ''' matches assembly by Assembly.GetName.Name; returns nothing if no match
        ''' </summary>
        <System.ComponentModel.Description("matches assembly by Assembly.GetName.Name; returns nothing if no match")> _
        Private Function GetAssemblyFromName(ByVal strAssemblyName As String) As System.Reflection.Assembly
            For Each a As [Assembly] In AppDomain.CurrentDomain.GetAssemblies()
                If a.GetName.Name = strAssemblyName Then
                    Return a
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' returns formatted string of all ASP.NET collections (QueryString, Form, Cookies, ServerVariables)
        ''' </summary>
        <System.ComponentModel.Description("returns formatted string of all ASP.NET collections (QueryString, Form, Cookies, ServerVariables)")> _
        Private Function GetASPSettings() As String
            Dim sb As New StringBuilder

            Const strSuppressKeyPattern As String = _
                "^ALL_HTTP|^ALL_RAW|VSDEBUGGER"

            With sb
                .Append("---- ASP.NET Collections ----")
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(HttpVarsToString(HttpContext.Current.Request.QueryString, "QueryString"))
                .Append(HttpVarsToString(HttpContext.Current.Request.Form, "Form"))
                .Append(HttpVarsToString(HttpContext.Current.Request.Cookies))
                .Append(HttpVarsToString(HttpContext.Current.Session))
                .Append(HttpVarsToString(HttpContext.Current.Cache))
                .Append(HttpVarsToString(HttpContext.Current.Application))
                .Append(HttpVarsToString(HttpContext.Current.Request.ServerVariables, "ServerVariables", True, strSuppressKeyPattern))
            End With

            Return sb.ToString
        End Function

        ''' <summary>
        ''' returns formatted string of all ASP.NET Cookies
        ''' </summary>
        <System.ComponentModel.Description("returns formatted string of all ASP.NET Cookies")> _
        Private Function HttpVarsToString(ByVal c As HttpCookieCollection) As String
            If c.Count = 0 Then Return ""

            Dim sb As New StringBuilder
            sb.Append("Cookies")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)

            For Each strKey As String In c
                AppendLine(sb, strKey, c.Item(strKey).Value)
            Next

            sb.Append(Environment.NewLine)
            Return sb.ToString
        End Function

        ''' <summary>
        ''' returns formatted summary string of all ASP.NET app vars
        ''' </summary>
        <System.ComponentModel.Description("returns formatted summary string of all ASP.NET app vars")> _
        Private Function HttpVarsToString(ByVal a As HttpApplicationState) As String
            If a.Count = 0 Then Return ""

            Dim sb As New StringBuilder
            sb.Append("Application")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)

            For Each strKey As String In a
                AppendLine(sb, strKey, a.Item(strKey))
            Next

            sb.Append(Environment.NewLine)
            Return sb.ToString
        End Function

        ''' <summary>
        ''' returns formatted summary string of all ASP.NET Cache vars
        ''' </summary>
        <System.ComponentModel.Description("returns formatted summary string of all ASP.NET Cache vars")> _
        Private Function HttpVarsToString(ByVal c As Caching.Cache) As String
            If c.Count = 0 Then Return ""

            Dim sb As New StringBuilder
            sb.Append("Cache")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)

            For Each de As DictionaryEntry In c
                AppendLine(sb, Convert.ToString(de.Key), de.Value)
            Next

            sb.Append(Environment.NewLine)
            Return sb.ToString
        End Function

        ''' <summary>
        ''' returns formatted summary string of all ASP.NET Session vars
        ''' </summary>
        <System.ComponentModel.Description("returns formatted summary string of all ASP.NET Session vars")> _
        Private Function HttpVarsToString(ByVal s As SessionState.HttpSessionState) As String
            '-- sessions can be disabled
            If s Is Nothing Then Return ""
            If s.Count = 0 Then Return ""

            Dim sb As New StringBuilder
            sb.Append("Session")
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)

            For Each strKey As String In s
                AppendLine(sb, strKey, s.Item(strKey))
            Next

            sb.Append(Environment.NewLine)
            Return sb.ToString
        End Function


        ''' <summary>
        ''' returns formatted string of an arbitrary ASP.NET NameValueCollection
        ''' </summary>
        <System.ComponentModel.Description("returns formatted string of an arbitrary ASP.NET NameValueCollection")> _
        Private Function HttpVarsToString(ByVal nvc As Specialized.NameValueCollection, ByVal strTitle As String, _
            Optional ByVal blnSuppressEmpty As Boolean = False, _
            Optional ByVal strSuppressKeyPattern As String = "") As String

            If Not nvc.HasKeys Then Return ""

            Dim sb As New StringBuilder
            sb.Append(strTitle)
            sb.Append(Environment.NewLine)
            sb.Append(Environment.NewLine)

            Dim blnDisplay As Boolean

            For Each strKey As String In nvc
                blnDisplay = True

                If blnSuppressEmpty Then
                    blnDisplay = nvc(strKey) <> ""
                End If

                If strKey = _strViewstateKey Then
                    blnDisplay = False
                End If

                If blnDisplay AndAlso strSuppressKeyPattern <> "" Then
                    blnDisplay = Not Regex.IsMatch(strKey, strSuppressKeyPattern)
                End If

                If blnDisplay Then
                    AppendLine(sb, strKey, nvc(strKey))
                End If
            Next

            sb.Append(Environment.NewLine)
            Return sb.ToString
        End Function


        ''' <summary>
        ''' attempts to coerce the value object using the .ToString method if possible, 
        ''' then appends a formatted key/value string pair to a StringBuilder. 
        ''' will display the type name if the object cannot be coerced.
        ''' </summary>
        <System.ComponentModel.Description("attempts to coerce the value object using the .ToString method if possible,      then appends a formatted key/value string pair to a StringBuilder.      will display the type name if the object cannot be coerced.")> _
        Private Sub AppendLine(ByVal sb As StringBuilder, _
            ByVal Key As String, ByVal Value As Object)

            Dim strValue As String
            If Value Is Nothing Then
                strValue = "(Nothing)"
            Else
                Try
                    strValue = Value.ToString
                Catch ex As Exception
                    strValue = "(" & Value.GetType.ToString & ")"
                End Try
            End If

            AppendLine(sb, Key, strValue)
        End Sub


        ''' <summary>
        ''' appends a formatted key/value string pair to a StringBuilder
        ''' </summary>
        <System.ComponentModel.Description("appends a formatted key/value string pair to a StringBuilder")> _
        Private Sub AppendLine(ByVal sb As StringBuilder, _
            ByVal Key As String, ByVal strValue As String)

            sb.Append(String.Format("    {0, -30}{1}", Key, strValue))
            sb.Append(Environment.NewLine)
        End Sub

#End Region

    End Class
