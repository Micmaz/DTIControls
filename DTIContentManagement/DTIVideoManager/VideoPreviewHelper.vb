''' <summary>
''' AJAX worker class to assist the video preview control in determining if the Flash conversion completed,
'''  or the title screenshot has changed
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class VideoPreviewHelper
    Inherits DTIAjax.jsonWorker
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class VideoPreviewHelper
            Inherits DTIAjax.jsonWorker
#End If

    ''' <summary>
    ''' Fetches a new screenshot from the file.
    ''' </summary>
    ''' <param name="inputHash"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Fetches a new screenshot from the file.")> _
    Public Function resetThumbnail(ByVal inputHash As Hashtable) As Integer
        Try
            Dim videoHelper As New VideoBase
            videoHelper.MyFlashWrapper.updateScreenShot(inputHash("vid_id"), inputHash("sec_mark"))

            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Check if video file was converted to flash.
    ''' </summary>
    ''' <param name="vid_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Check if video file was converted to flash.")> _
    Public Function checkConversion(ByVal vid_id As String) As Integer
        Try
            If sqlHelper.SafeFetchSingleValue("select Converted from DTIVideoManager where Id = @id", New Object() {Integer.Parse(vid_id)}) Then
                Return 1
            Else : Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

End Class
