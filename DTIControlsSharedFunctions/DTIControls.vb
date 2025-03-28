Imports DTIServerControls

''' <summary>
''' Shared helper module for easy access to utility methods
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Shared helper module for easy access to utility methods")> _
Public Module Share

    ''' <summary>
    ''' A generic session object. Handles design mode as well as runtime. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A generic session object. Handles design mode as well as runtime.")> _
    Public ReadOnly Property Session() As DTISharedVariables.HttpSessionState
        Get
            Return DTISharedVariables.Session
        End Get
    End Property

    ''' <summary>
    ''' This is the secondary identifier of the current session. 
    ''' This propery can be overridden by any DTIControl, but changing this property here will make all editpanels and sortables use a different default value.
    ''' This is for display purposes. All rendered controls without a set mainID will use this site wide one.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("This is the secondary identifier of the current session.    This propery can be overridden by any DTIControl, but changing this property here will make all editpanels and sortables use a different default value.   This is for display purposes. All rendered controls without a set mainID will use this site wide one.")> _
    Public Property MainId() As Long
        Get
            Return DTISharedVariables.MasterMainId
        End Get
        Set(ByVal value As Long)
            DTISharedVariables.MasterMainId = value
        End Set
    End Property

    ''' <summary>
    ''' A language seperator. usable for multilingual sites. This is the current LanguageId visable for the site.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A language seperator. usable for multilingual sites. This is the current LanguageId visable for the site.")> _
    Public Property LanguageId() As Integer
        Get
            Return DTISharedVariables.MasterLanguageId
        End Get
        Set(ByVal value As Integer)
            DTISharedVariables.MasterLanguageId = value
        End Set
    End Property

    ''' <summary>
    ''' If set to true, this will turn edit mode on for the current session.
    ''' You will need to set logged in to true if you have not already.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, this will turn edit mode on for the current session.   You will need to set logged in to true if you have not already.")> _
    Public Property EditModeOn() As Boolean
        Get
            Return DTISharedVariables.AdminOn
        End Get
        Set(ByVal value As Boolean)
            DTISharedVariables.AdminOn = value
            If value = True Then
                AdminPanelOn = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' If set to true, this will log the current session in and turn edit mode on.
    ''' This causes a responce.redirect to occure so no code after this property is set will be run. 
    ''' The redirect only occures if the state changes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, this will log the current session in and turn edit mode on.   This causes a responce.redirect to occure so no code after this property is set will be run.    The redirect only occures if the state changes.")> _
    Public Property AdminPanelOn() As Boolean
        Get
            Return DTISharedVariables.LoggedIn
        End Get
        Set(ByVal value As Boolean)
            If Not DTISharedVariables.LoggedIn = value Then
                DTISharedVariables.LoggedIn = value
                If value Then
                    DTISharedVariables.AdminOn = value
                End If
                If Not Session.isdesign Then
                    Session.httpResponce.Redirect(Session.httpRequest.Url.OriginalString)
                End If
            End If
        End Set
    End Property

	''' <summary>
    ''' The mainID that the current administrator has edit access to.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The mainID that the current administrator has edit access to.")> _
    Public Property siteEditMainID() As Long
        Get
            Return DTISharedVariables.siteEditMainID
        End Get
        Set(ByVal value As Long)
            DTISharedVariables.siteEditMainID = value
        End Set
    End Property

    ''' <summary>
    ''' If set to true, this will turn layout mode on for the current session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, this will turn layout mode on for the current session.")> _
    Public Property LayoutOn() As Boolean
        Get
            Return DTISharedVariables.LayoutOn
        End Get
        Set(ByVal value As Boolean)
            DTISharedVariables.LayoutOn = value
            If value = True Then
                AdminPanelOn = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' If set to true, this will set the user as logged in for the current session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If set to true, this will set the user as logged in for the current session.")> _
    Public Property LoggedIn() As Boolean
        Get
            Return DTISharedVariables.LoggedIn
        End Get
        Set(ByVal value As Boolean)
            DTISharedVariables.LoggedIn = value
            If value = True Then
                AdminPanelOn = True
            End If
        End Set
    End Property

	
	
    ''' <summary>
    ''' Default upload folder. The webserver must have write access to this folder. 
    ''' </summary>
    ''' <value>"/uploads/"</value>
    ''' <returns>The currently selected upload folder.</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Default upload folder. The webserver must have write access to this folder.")> _
    Public Property UploadFolderDefault() As String
        Get
            Return DTISharedVariables.UploadFolderDefault
        End Get
        Set(ByVal value As String)
            DTISharedVariables.UploadFolderDefault = value
        End Set
    End Property

    '''' <summary>
    '''' This dataset contains a cached version of the page heiarchy for the current user. 
    '''' If set to nothing it will refresh at the next page load.
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public ReadOnly Property siteData() As dsSite
    '    Get
    '        Return DTISharedVariables.dsSite
    '    End Get
    'End Property

    ''' <summary>
    ''' A convience helper for getting the current baseHelper
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A convience helper for getting the current baseHelper")> _
    Public ReadOnly Property sqlHelper() As BaseClasses.BaseHelper
        Get
            Return BaseClasses.SQLHelper.getHelper()
        End Get
    End Property

    ''' <summary>
    ''' The default connection for the entire application. This will effect all logged in users.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The default connection for the entire application. This will effect all logged in users.")> _
    Public Property defaultConnection() As System.Data.Common.DbConnection
        Get
            Return BaseClasses.DataBase.defaultConnectionAppWide
        End Get
        Set(ByVal value As System.Data.Common.DbConnection)
            BaseClasses.DataBase.defaultConnectionAppWide = value
        End Set
    End Property

    ''' <summary>
    ''' The default connection for the current session state only.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The default connection for the current session state only.")> _
    Public Property defaultConnectionSessionWide() As System.Data.Common.DbConnection
        Get
            Return BaseClasses.DataBase.defaultConnectionSessionWide
        End Get
        Set(ByVal value As System.Data.Common.DbConnection)
            BaseClasses.DataBase.defaultConnectionSessionWide = value
        End Set
    End Property

    ''' <summary>
    ''' Call this at application.init()
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Call this at application.init()")> _
    Public Sub initializePathProvider()
        BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
    End Sub

	
	''' <summary>
    ''' Returns a default value if the item is not an app property in the application config.
	''' Usefull for overideable propertys EX:
	''' Public Property redirectURL As String = vmDbOperations.getDefaultValue("redirectURL", "www.google.com")
    ''' </summary>
    ''' <param name="configName">The name of the key in the app.config or web.config</param>
    ''' <param name="defaultValue">Default value if the key is not found</param>
    ''' <remarks></remarks>
<System.ComponentModel.Description("Returns a default value if the item is not an app property in the application config. 	 Usefull for overideable propertys EX: 	 Public Property redirectURL As String = vmDbOperations.getDefaultValue(""redirectURL"", ""www.google.com"")")> _
	Public Function getDefaultValue(configName As String, defaultValue As String) As String
        If Not Configuration.ConfigurationManager.AppSettings(configName) Is Nothing Then Return Configuration.ConfigurationManager.AppSettings(configName)
        Return defaultValue
    End Function

	
    ''' <summary>
    ''' Adds a user control to the admin panel tool menu. Appears in layout mode.
    ''' </summary>
    ''' <param name="Path_to_ascx_Control"></param>
    ''' <param name="Menu_Item_title"></param>
    ''' <param name="iconUrl"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Adds a user control to the admin panel tool menu. Appears in layout mode.")> _
    Public Sub add_UserControl_to_Menu(ByVal Path_to_ascx_Control As String, Optional ByVal Menu_Item_title As String = Nothing, Optional ByVal iconUrl As String = Nothing)
        DTISortable.DTIWidgetMenu.addUserControlToMenu(Path_to_ascx_Control, Menu_Item_title, iconUrl)
    End Sub


End Module

