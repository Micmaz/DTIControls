Public Class DTISettingsControl
    Inherits BaseClasses.BaseSecurityUserControl

    Public Event settingsCreated(ByRef ParentControl As DTIServerControl)

    Friend Sub raiseSettingsCreated(ByRef ParentControl As DTIServerControl)
        RaiseEvent settingsCreated(ParentControl)
    End Sub

    Private _contentType As String = ""
    Public Overridable Property contentType() As String
        Get
            Return _contentType
        End Get
        Set(ByVal value As String)
            _contentType = value
        End Set
    End Property

    Private _MainID As Integer
    Public Overridable Property MainID() As Integer
        Get
            Return _MainID
        End Get
        Set(ByVal value As Integer)
            _MainID = value
        End Set
    End Property


End Class