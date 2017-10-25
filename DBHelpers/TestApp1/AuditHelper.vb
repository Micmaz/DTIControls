Public Class AuditHelper
    Inherits SQLiteHelper.SQLiteHelper

    Public Shadows Sub Update(ByRef dt As DataTable)
        MyBase.Update(dt)
    End Sub

End Class
