Imports DTIImageManager
Imports DTIMediaManager
Imports System.IO

<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Partial Public Class Uploader
    Inherits BaseClasses.BaseSecurityPage
    Sub Page_Load(ByVal o As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Clear()
        Response.Write(processUpload())
    End Sub

	Private Function processChunkedUpload() As String
		Dim uploadpath As String = Request.MapPath(DTIServerControls.DTISharedVariables.UploadFolderDefault)
		Dim fileName As String = uploadpath & Request.Params("fileName")
		Dim chunk As Integer = Request.Params("chunk")
		Dim chunkcount As Integer = Request.Params("chunkCount")
		Dim offset As Integer = Request.Params("offset")

		Dim infile As HttpPostedFile = Request.Files(0)
		Dim filelen As Integer = infile.ContentLength
		'Dim fileData(filelen) As Byte
		'infile.InputStream.Read(fileData, 0, filelen)

		If chunk = 0 AndAlso System.IO.File.Exists(fileName) Then File.Delete(fileName)
		If Not Directory.Exists(uploadpath) Then Directory.CreateDirectory(uploadpath)

		If System.IO.File.Exists(fileName) Then
			Using fs As New FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None)
				Dim buffer(infile.InputStream.Length - 1) As Byte
				infile.InputStream.Read(buffer, 0, infile.InputStream.Length)
				fs.Write(buffer, 0, buffer.Length)
				fs.Close()
			End Using
		Else
			infile.SaveAs(fileName)
		End If



		'Using fs As New FileStream(fileName, FileMode.Append, FileAccess.Write)
		'	Using bw As New BinaryWriter(fs)
		'		bw.Write(fileData)
		'	End Using
		'End Using
		If chunk = chunkcount Then
			Dim imgrow = DTIImageManager.ImageUploaderControl.saveImage(fileName, deletefile:=True)
			Return getimgTag(imgrow)
		End If
		Return ""
	End Function

	Private Function processUpload() As String

		If Not String.IsNullOrEmpty(Request.Params("chunkCount")) Then
			Return processChunkedUpload()
		End If

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

		'Dim name As String = System.IO.Path.Combine(basePath, sFileName)

		Dim imgrow As dsImageManager.DTIImageManagerRow = SharedImageVariables.myImages.NewDTIImageManagerRow
		ImageUploaderControl.pushImage(theFile, imgrow, 0)
		SharedImageVariables.myImages.AddDTIImageManagerRow(imgrow)

		Dim dt As New dsImageManager.DTIImageManagerDataTable
		' and original_filename = @origfn
		Try
			sqlHelper.FillDataTable("select * from DTIImageManager where width= @w and height = @ht and image_content_type = @ct", dt,
								imgrow.Width, imgrow.Height, imgrow.Image_Content_Type, imgrow.Original_Filename)
		Catch ex As Exception
			sqlHelper.checkAndCreateTable(dt)
			sqlHelper.FillDataTable("select * from DTIImageManager where width= @w and height = @ht and image_content_type = @ct", dt,
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
		Return getimgTag(imgrow)

	End Function

	Public Shared Function getimgTag(imgrow As DTIImageManager.dsImageManager.DTIImageManagerRow) As String
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
		Return "<scr" + "ipt type='text/javascript'> window.parent.callFunction(" + funcNum + ", '', '" + msg + "')</scr" + "ipt>"
	End Function

End Class