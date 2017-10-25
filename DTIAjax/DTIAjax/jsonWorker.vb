''' <summary>
''' The worker class recieving data from a json call.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class jsonWorker
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class jsonWorker
#End If
        Public sqlHelper As BaseClasses.BaseHelper
        Public ajaxID As String
        Public Session As System.Web.SessionState.HttpSessionState
        Public Page As DTIAjax.ajaxpg

    End Class
