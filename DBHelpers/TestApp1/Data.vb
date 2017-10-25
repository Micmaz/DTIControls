Imports BaseClasses

Public Class Data
    Inherits BaseClasses.DataBase

    Public Overrides Function getHelperOverridale() As BaseHelper
        Dim h As New AuditHelper
        h.setDefaultConnectionString(Data.defaultConnectionString.ConnectionString)
        Return h
    End Function




End Class
