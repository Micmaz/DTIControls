Imports System.Web
Imports System.Web.UI

Public Class DTISharedVariables

    ''Should change all of these to be initially settable via Server.MapPath(ConfigurationManager.AppSettings("propName"))

    Public Shared siteEditOnDefaultKey As String = "DTIServerControl.siteEditOn"
    Public Shared siteAdminLoginOnDefaultKey As String = "DTIServerControl.siteLoggedIn"
    Public Shared siteMainID As String = "DTIServerControl.MainID"
    Public Shared siteLanguageID As String = "DTIServerControl.LanguageID"
    Public Shared siteLayoutOn As String = "IsLayoutOn_ForDTISortableDTIMenuAndDTIRecycleBin"

	''' <summary>
	''' A subclassed session state to provide design time support for any property that uses session.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	<System.ComponentModel.Description("A subclassed session state to provide design time support for any property that uses session.")>
	Public Shared ReadOnly Property Session() As HttpSessionState
		Get
			If _httpsessionstate Is Nothing Then
				_httpsessionstate = New HttpSessionState
				Dim sess As SessionState.HttpSessionState = Nothing
				Try
					sess = _httpsessionstate.httpSession
				Catch ex As Exception
					sess = Nothing
				End Try
				If sess Is Nothing Then _httpsessionstate.isdesign = True
			End If
			Return _httpsessionstate
		End Get
	End Property
	Private Shared _httpsessionstate As HttpSessionState

	''' <summary>
	''' The default mainID. 
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
    <System.ComponentModel.Description("The default mainID.")> _
    Public Shared Property MasterMainId() As Long
        Get
            If Session(siteMainID) Is Nothing Then Session(siteMainID) = 0
            'language id is bit shifted to the top 10 bits and then ORed into the MasterMainId
            Dim shifted_language_id As Long = MasterLanguageId * Math.Pow(2, 55)
            Return shifted_language_id Or Session(siteMainID)
        End Get
        Set(ByVal value As Long)
            Session(siteMainID) = value
        End Set
    End Property

    ''' <summary>
    ''' The LanguageID that the current administrator has edit access to.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The LanguageID that the current administrator has edit access to.")> _
    Public Shared Property siteEditMainID() As Long
        Get
            If Session(siteMainID & "_Edit") Is Nothing Then Session(siteMainID & "_Edit") = 0
            Return Session(siteMainID & "_Edit")
        End Get
        Set(ByVal value As Long)
            Session(siteMainID & "_Edit") = value
        End Set
    End Property

    ''' <summary>
    ''' The Default Language id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The Default Language id")> _
    Public Shared Property siteEditLanguageId() As Integer
        Get
            If Session(siteLanguageID & "_Edit") Is Nothing Then Session(siteLanguageID & "_Edit") = 0
            Return Session(siteLanguageID & "_Edit")
        End Get
        Set(ByVal value As Integer)
            Session(siteLanguageID & "_Edit") = value
        End Set
    End Property

    ''' <summary>
    ''' The Default Language id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The Default Language id")> _
    Public Shared Property MasterLanguageId() As Integer
        Get
            If Session(siteLanguageID) Is Nothing Then Session(siteLanguageID) = 0
            Return Session(siteLanguageID)
        End Get
        Set(ByVal value As Integer)
            Session(siteLanguageID) = value
        End Set
    End Property



    Public Shared Property AdminOn() As Boolean
        Get
            If Session(siteEditOnDefaultKey) Is Nothing Then Session(siteEditOnDefaultKey) = False
            If LoggedIn Then
                Return Session(siteEditOnDefaultKey)
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then LoggedIn = True
            Session(siteEditOnDefaultKey) = value
        End Set
    End Property

    Public Shared Property LoggedIn() As Boolean
        Get
            If Session(siteAdminLoginOnDefaultKey) Is Nothing Then Session(siteAdminLoginOnDefaultKey) = False
            Return Session(siteAdminLoginOnDefaultKey)
        End Get
        Set(ByVal value As Boolean)
            Session(siteAdminLoginOnDefaultKey) = value
            If Not value Then
                AdminOn = False
				LayoutOn = False
            End If
        End Set
    End Property
	
	Public Shared Property LayoutOn() As Boolean
        Get
            If Session(siteLayoutOn) Is Nothing Then Session(siteLayoutOn) = False
            Return Session(siteLayoutOn)
        End Get
        Set(ByVal value As Boolean)
            Session(siteLayoutOn) = value
        End Set
    End Property

    ''' <summary>
    ''' Default upload folder. The webserver must have write access to this folder. 
    ''' </summary>
    ''' <value>"/uploads/"</value>
    ''' <returns>The currently selected upload folder.</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Default upload folder. The webserver must have write access to this folder.")> _
    Public Shared Property UploadFolderDefault() As String
        Get
            If Session("defaultUploadFolder") Is Nothing Then  
				Dim appPath as String = HttpContext.Current.Request.ApplicationPath
				Session("defaultUploadFolder") = appPath & "/uploads/"
			end if
			
            Return Session("defaultUploadFolder")
        End Get
        Set(ByVal value As String)
            Session("defaultUploadFolder") = "/" & value.Replace("\", "/").Trim.Trim("/") & "/"
            Dim physicalPath As String = HttpContext.Current.Request.MapPath(DTIServerControls.DTISharedVariables.UploadFolderDefault)
            Try
                If Not System.IO.Directory.Exists(physicalPath) Then
                    System.IO.Directory.CreateDirectory(physicalPath)
                End If
            Catch ex As Exception
                Throw New Exception("The web server does not have acces to create the upload folder at: " & physicalPath & " Please ensure that the iis user has write permission to that directory.")
            End Try
        End Set
    End Property

    'Public Shared Property CurrentUser() As dsSite.UsersRow
    '    Get
    '        Return Session("DTICurrentSiteUser")
    '    End Get
    '    Set(ByVal value As dsSite.UsersRow)
    '        Session("DTICurrentSiteUser") = value
    '    End Set
    'End Property

    'Public Shared ReadOnly Property dsSite() As dsSite
    '    Get
    '        If Session("DTIdsSite") Is Nothing Then
    '            Session("DTIdsSite") = New dsSite
    '        End If
    '        Return Session("DTIdsSite")
    '    End Get
    'End Property

    Public Shared Sub moveIt(ByVal sourceSelector As String, ByVal destSelector As String, ByRef pageRef As Page)
        jQueryLibrary.jQueryInclude.RegisterJQuery(pageRef)
        Dim script As String = "$(document).ready(function(){$('" & sourceSelector & "').remove().appendTo('" & destSelector & "');});"
        jQueryLibrary.jQueryInclude.addScriptBlock(pageRef, script)
    End Sub

    Public Shared Sub truncateIt(ByVal sourceSelector As String, ByRef pageRef As Page, Optional ByVal max_length As Integer = 140, Optional ByVal more_text As String = ". . . more", Optional ByVal less_text As String = "less")
        jQueryLibrary.jQueryInclude.RegisterJQuery(pageRef)
        jQueryLibrary.jQueryInclude.addScriptFile(pageRef, "/jQueryLibrary/jquery.truncator.js", , True)
        Dim script As String = "$(function() {$('" & sourceSelector & "').truncate({max_length: " & _
            max_length & ", more: '" & more_text & "', less: '" & less_text & "' });});"
        pageRef.ClientScript.RegisterStartupScript(pageRef.GetType, sourceSelector & "_truncate", script, True)
    End Sub



    'Private Shared _ds As dsDTIControls
    'Private Shared hasinit As Boolean = False
    'Private Shared ReadOnly Property ds() As dsDTIControls
    '    Get
    '        If _ds Is Nothing Then
    '            ds = New dsDTIControls
    '            Dim sqlhelper As BaseClasses.BaseHelper = BaseClasses.BaseHelper.getHelper()
    '            If Not hasinit Then
    '                sqlhelper.checkAndCreateAllTables(ds)
    '                hasinit = True
    '            End If
    '            sqlhelper.FillDataSetMultiSelect( _
    '            "select * from DTIControl " & _
    '                "where Component_type = 'SiteWideSettings' " & _
    '                "and Content_Type = 'SiteWideSettings' and Mainid = '-1'; " & _
    '            "select id from DTIControlProperty " & _
    '                "where DTIControlID in " & _
    '                    "(select * from DTIControl where Component_type = 'SiteWideSettings' " & _
    '                        "and Content_Type = 'SiteWideSettings' and Mainid = '-1') ", _
    '            _ds, New String() {"DTIControl", "DTIControlProperty"})
    '            If ds.DTIControl.Count = 0 Then
    '                ds.DTIControl.AddDTIControlRow("SiteWideSettings", "SiteWideSettings", "-1")
    '                sqlhelper.Update(ds.DTIControl)
    '            End If
    '        End If
    '        For Each row As dsDTIControls.DTIControlPropertyRow In _ds.DTIControlProperty
    '            If System.Configuration.ConfigurationManager.AppSettings(row.PropertyPath) Is Nothing Then
    '                cachedSettings(row.PropertyPath) = row.PropertyValue
    '            End If
    '        Next
    '        Return _ds
    '    End Get
    'End Property

    'Private Shared Sub saveSetting(ByVal key As String, ByVal value As String)
    'End Sub

    Private Shared cachedSettings As New Hashtable
    Public Shared Property SiteWideSettings(ByVal key As String) As String
        Get
            'If ds Is Nothing Then
            '    Return Nothing
            'End If
            If System.Configuration.ConfigurationManager.AppSettings(key) Is Nothing Then
                Return cachedSettings(key)
            Else : Return System.Configuration.ConfigurationManager.AppSettings(key)
            End If
        End Get
        Set(ByVal value As String)
            If System.Configuration.ConfigurationManager.AppSettings(key) Is Nothing Then
                cachedSettings(key) = value
            End If
            '    cachedSettings(key) = value
            '    If savesetting() Then
            '    End If
        End Set
    End Property


	Public Class HttpSessionState

		Public Sub Remove(ByVal name As String)
			If isdesign Then
				designProperty.Remove(name)
			Else
				httpSession.Remove(name)
			End If
		End Sub

		Public ReadOnly Property SessionID() As String
			Get
				If isdesign Then
					Return ""
				Else
					Return httpSession.SessionID
				End If
			End Get
		End Property

		Public ReadOnly Property httpResponce() As System.Web.HttpResponse
			Get
				If isdesign Then Return Nothing
				Return HttpContext.Current.Response
			End Get
		End Property

		Public ReadOnly Property httpRequest() As System.Web.HttpRequest
			Get
				If isdesign Then Return Nothing
				Return HttpContext.Current.Request
			End Get
		End Property

		Public ReadOnly Property httpSession() As System.Web.SessionState.HttpSessionState
			Get
				Return HttpContext.Current.Session
			End Get
		End Property

		Private _isdesign As Boolean = False
		Private designProperty As Hashtable
		Public Property isdesign() As Boolean
			Get
				Return _isdesign
			End Get
			Set(ByVal value As Boolean)
				_isdesign = value
				If _isdesign Then designProperty = New Hashtable
			End Set
		End Property

		Default Public Property Item(ByVal name As String) As Object
			Get
				If isdesign Then
					Return designProperty(name)
				Else
					Return httpSession(name)
				End If
			End Get
			Set(ByVal value As Object)
				If isdesign Then
					designProperty(name) = value
				Else
					httpSession(name) = value
				End If
			End Set
		End Property

	End Class

End Class
