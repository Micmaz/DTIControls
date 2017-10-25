Imports DTIImageManager
Imports DTIMediaManager

<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Partial Public Class Uploader
    Inherits BaseClasses.BaseSecurityPage
    Sub Page_Load(ByVal o As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Clear()
        Response.Write(processUpload())
    End Sub

    ' Upload script for CKEditor.
    ' Use at your own risk, no warranty provided. Be careful about who is able to access this file
    ' The upload folder shouldn't be able to upload any kind of script, just in case.
    ' If you're not sure, hire a professional that takes care of adjusting the server configuration as well as this script for you.
    ' (I am not such professional)

    Private Function processUpload() As String
        ' Step 1: change the true for whatever condition you use in your environment to verify that the user
        ' is logged in and is allowed to use the script
        'If (True) Then
        '    Return sendError("You're not allowed to upload files")
        'End If


        ' Step 2: Put here the full absolute path of the folder where you want to save the files:
        ' You must set the proper permissions on that folder


        ' Step 3: Put here the Url that should be used for the upload folder (it the URL to access the folder that you have set in $basePath
        ' you can use a relative url "/images/", or a path including the host "http://example.com/images/"
        ' ALWAYS put the final slash (/)

        ' Done. Now test it!

        ' No need to modify anything below this line
        '----------------------------------------------------

        ' ------------------------
        ' Input parameters: optional means that you can ignore it, and required means that you
        ' must use it to provide the data back to CKEditor.
        ' ------------------------

        ' Optional: instance name (might be used to adjust the server folders for example)
        Dim CKEditor As String = HttpContext.Current.Request("CKEditor")

        ' Required: Function number as indicated by CKEditor.
        Dim funcNum As String = HttpContext.Current.Request("CKEditorFuncNum")

        ' Optional: To provide localized messages
        Dim langCode As String = HttpContext.Current.Request("langCode")

        ' ------------------------
        ' Data processing
        ' ------------------------

        Dim total As Integer
        Try
            total = HttpContext.Current.Request.Files.Count
        Catch ex As Exception
            Return sendError("Error uploading the file")
        End Try

        If (total = 0) Then
            Return sendError("No file has been sent")
        End If

        'Grab the file name from its fully qualified path at client
        Dim theFile As HttpPostedFile = HttpContext.Current.Request.Files(0)

        Dim strFileName As String = theFile.FileName
        If (strFileName = "") Then
            Return sendError("File name is empty")
        End If

        Dim sFileName As String = System.IO.Path.GetFileName(strFileName)

        'Dim name As String = System.IO.Path.Combine(basePath, sFileName)

        Dim imgrow As dsImageManager.DTIImageManagerRow = SharedImageVariables.myImages.NewDTIImageManagerRow
        ImageUploaderControl.pushImage(theFile, imgrow, 0)
        SharedImageVariables.myImages.AddDTIImageManagerRow(imgrow)

        Dim dt As New dsImageManager.DTIImageManagerDataTable
        ' and original_filename = @origfn
        Try
            sqlHelper.FillDataTable("select * from DTIImageManager where width= @w and height = @ht and image_content_type = @ct", dt, _
                                imgrow.Width, imgrow.Height, imgrow.Image_Content_Type, imgrow.Original_Filename)
        Catch ex As Exception
            sqlHelper.checkAndCreateTable(dt)
            sqlHelper.FillDataTable("select * from DTIImageManager where width= @w and height = @ht and image_content_type = @ct", dt, _
                                imgrow.Width, imgrow.Height, imgrow.Image_Content_Type, imgrow.Original_Filename)
        End Try
        
        Dim newImage As Boolean = True
        For Each row As dsImageManager.DTIImageManagerRow In dt
            If getHash(imgrow.Image) = getHash(row.Image) Then
                newImage = False
                imgrow = row
                Exit For
            End If
        Next
        If newImage Then
            sqlHelper.Update(SharedImageVariables.myImages)
            Dim dtImgMan As New dsMedia.DTIMediaManagerDataTable
            dtImgMan.AddDTIMediaManagerRow("Image", imgrow.Id, True, False, "", 0, 0, "", "", "ContentManagement", Date.Now, "DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id, 0, 0)
            sqlHelper.Update(dtImgMan)
        End If


		'theFile.SaveAs(name)

		'Dim url As String = baseUrl + sFileName.Replace("'", "\'")
		' ------------------------
		' Write output
		' ------------------------
		If Not imgrow.IsHeightNull AndAlso imgrow.Height > 200 Then
			Return "~/res/DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id & "&Height=200"
			'Return "<scr" & "ipt type='text/javascript'> window.parent.CKEDITOR.tools.callFunction(" & funcNum & ", '~/res/DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id & "&Height=200', '')</scr" & "ipt>"
		Else
			Return "~/res/DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id
			'Return "<scr" & "ipt type='text/javascript'> window.parent.CKEDITOR.tools.callFunction(" & funcNum & ", '~/res/DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id & "', '')</scr" & "ipt>"
		End If


    End Function

    Public Function getHash(content As Byte()) As String
        Dim SHA As New System.Security.Cryptography.SHA512Managed
        Dim data As Byte() = SHA.ComputeHash(content)
        Dim sBuilder As New System.Text.StringBuilder
        For i As Integer = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i
        Return sBuilder.ToString()
    End Function

    Private Function sendError(ByVal msg As String) As String
        Dim funcNum As String = HttpContext.Current.Request("CKEditorFuncNum")
        Return "<scr" + "ipt type='text/javascript'> window.parent.CKEDITOR.tools.callFunction(" + funcNum + ", '', '" + msg + "')</scr" + "ipt>"
    End Function

End Class