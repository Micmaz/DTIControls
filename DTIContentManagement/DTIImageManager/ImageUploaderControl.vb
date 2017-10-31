Imports DTIUploader
Imports DTIImageManager.dsImageManager
Imports DTIImageManager.SharedImageVariables

''' <summary>
''' control to upload images to DTI-managed media library
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ImageUploaderControl
    Inherits DTIUploaderControl
#Else
        <ComponentModel.ToolboxItem(False)> _
        Public Class ImageUploaderControl
            Inherits DTIUploaderControl
#End If
    Private _user_id As Integer = -1
    Public Property User_Id() As Integer
        Get
            Return _user_id
        End Get
        Set(ByVal value As Integer)
            _user_id = value
        End Set
    End Property

    Private Sub ImageUploaderControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        FileFilter = FileFilters.Images
    End Sub

    Private Sub ImageUploaderControl_handleWebFile(ByVal path As String) Handles Me.handleWebFile
        Dim imgRow As DTIImageManagerRow = myImages.NewDTIImageManagerRow
        pushImage(path, imgRow, User_Id)
        myImages.AddDTIImageManagerRow(imgRow)
        sqlhelper.Update(myImages)
    End Sub

    Private Sub ImageUploaderControl_handleFile(ByRef file As System.Web.HttpPostedFile) Handles Me.handleFile
        Dim imgRow As DTIImageManagerRow = myImages.NewDTIImageManagerRow
        pushImage(file, imgRow, User_Id)
        myImages.AddDTIImageManagerRow(imgRow)
        sqlhelper.Update(myImages)
    End Sub

    Public Shared Function saveImage(fileContents As Byte(), filename As String, Optional catagory As String = "", Optional siteID As Integer = 0) As DTIImageManagerRow
        Dim sqlhelper As BaseClasses.BaseHelper = BaseClasses.DataBase.getHelper()
        Dim images As New DTIImageManagerDataTable
        Dim imgrow As DTIImageManagerRow = images.NewDTIImageManagerRow()
        pushImage(fileContents, filename, imgrow, siteID)
        images.AddDTIImageManagerRow(imgrow)
        sqlhelper.Update(images)

        If catagory <> "" Then
            Dim catagories As New dsImageManager.DTIImageCategoriesDataTable()
            catagories.AddDTIImageCategoriesRow(siteID, imgrow.Id.ToString(), catagory, DateTime.Now)
            sqlhelper.Update(catagories)
        End If

        Return imgrow
    End Function

    Public Shared Function saveImage(ByRef infile As HttpPostedFile, Optional catagory As String = "", Optional siteID As Integer = 0) As DTIImageManagerRow
        Dim filelen As Integer = infile.ContentLength
        Dim _image(filelen) As Byte
        infile.InputStream.Read(_image, 0, filelen)
        Return saveImage(_image, infile.FileName, catagory, siteID)
    End Function

	Private Shared checkImageTable = False
	Private Shared checkImageCatTable = False
	Public Shared Function saveImage(filename As String, Optional catagory As String = "", Optional siteID As Integer = 0, Optional deletefile As Boolean = True, Optional addToMediaManager As Boolean = True) As DTIImageManagerRow
		Dim sqlhelper As BaseClasses.BaseHelper = BaseClasses.DataBase.getHelper()
		Dim images As New DTIImageManagerDataTable

		If Not checkImageTable Then
			sqlhelper.checkAndCreateTable(images)
			checkImageTable = True
		End If

		Dim imgrow As DTIImageManagerRow = images.NewDTIImageManagerRow()
		pushImage(filename, imgrow, siteID, deletefile)
		images.AddDTIImageManagerRow(imgrow)
		sqlhelper.Update(images)

		If addToMediaManager Then
			Dim dtImgMan As New DTIImageManager.dsMedaManagerTbl.DTIMediaManagerDataTable
			dtImgMan.AddDTIMediaManagerRow("Image", imgrow.Id, True, False, "", 0, 0, "", "", "ContentManagement", Date.Now, "DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id, 0, 0)
			sqlhelper.Update(dtImgMan)
		End If

		If catagory <> "" Then
			If Not checkImageCatTable Then
				sqlhelper.checkAndCreateTable(New dsImageManager.DTIImageCategoriesDataTable)
				checkImageCatTable = True
			End If

			Dim catagories As New dsImageManager.DTIImageCategoriesDataTable()
			catagories.AddDTIImageCategoriesRow(siteID, imgrow.Id.ToString(), catagory, DateTime.Now)
			sqlhelper.Update(catagories)
		End If

		Return imgrow
	End Function

	Public Shared Sub pushImage(ByVal path As String, ByRef imgRow As DTIImageManagerRow, ByVal user_id As Integer, Optional deletefile As Boolean = True)
        Dim oFile As System.IO.FileInfo
        oFile = New System.IO.FileInfo(path)

        Dim oFileStream As System.IO.FileStream = oFile.OpenRead()
        Dim lBytes As Long = oFileStream.Length

        If (lBytes > 0) Then
            Dim fileData(lBytes - 1) As Byte

            ' Read the file into a byte array
            oFileStream.Read(fileData, 0, lBytes)
            oFileStream.Close()

            setImgRow(imgRow, fileData, path, user_id)

            Try
                If deletefile Then oFile.Delete()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Public Shared Sub pushImage(_image As Byte(), filename As String, ByRef imgRow As DTIImageManagerRow, ByVal user_id As Integer)
        setImgRow(imgRow, _image, filename, user_id)
    End Sub
    Public Shared Sub pushImage(ByRef infile As HttpPostedFile, ByRef imgRow As DTIImageManagerRow, ByVal user_id As Integer)
        Dim filelen As Integer = infile.ContentLength
        Dim _image(filelen) As Byte
        infile.InputStream.Read(_image, 0, filelen)

        setImgRow(imgRow, _image, infile.FileName, user_id)
    End Sub

    Private Shared imgext As String = "jpg,gif,png,tif,tiff,bmp,jpeg,tga"
	Public Shared Sub setImgRow(ByRef imgRow As DTIImageManagerRow, ByVal _image As Byte(), ByVal fileName As String, ByVal user_id As Integer, Optional maxHeight As Integer = -1, Optional maxWidth As Integer = -1)
		imgRow.Original_Filename = fileName.Replace("\", "/")
		If imgRow.Original_Filename.LastIndexOf("/") > -1 Then
			imgRow.Original_Filename = imgRow.Original_Filename.Substring(imgRow.Original_Filename.LastIndexOf("/") + 1)
		End If

		Dim ext As String = imgRow.Original_Filename.Substring(imgRow.Original_Filename.LastIndexOf(".") + 1).ToLower()

		If imgext.Contains(ext) Then
			imgRow.Image_Content_Type = "image/" & ext 'infile.ContentType.ToLower
		Else
			imgRow.Image_Content_Type = "application/" & ext 'infile.ContentType.ToLower
		End If

		imgRow.Height = 0
		imgRow.Width = 0
		If imgRow.Image_Content_Type.IndexOf("image") > -1 Then
			ViewImage.getHeightWidth(_image, imgRow.Height, imgRow.Width)
			If imgRow.Original_Filename.ToLower.EndsWith(".gif") OrElse imgRow.Height = 0 OrElse imgRow.Width = 0 Then
				imgRow.Image = _image
				'ElseIf imgRow.Original_Filename.ToLower.EndsWith(".png") Then
				'	imgRow.Image = _image
			Else
				If maxWidth = -1 Then
					Try
						maxWidth = ConfigurationManager.AppSettings("maxImageWidth")
					Catch ex As Exception
						maxWidth = 2048
					End Try
				End If

				If maxHeight = -1 Then
					Try
						maxHeight = ConfigurationManager.AppSettings("maxImageHeight")
					Catch ex As Exception
						maxHeight = 2048
					End Try
				End If
				If maxHeight = 0 Then maxHeight = 2048
				If maxWidth = 0 Then maxWidth = 2048
				ViewImage.getDimention(imgRow.Height, imgRow.Width, imgRow.Height, imgRow.Width, 0, 0, maxHeight, maxWidth)
				imgRow.Image = ViewImage.processImageArr(_image, imgRow.Image_Content_Type, imgRow.Height, imgRow.Width)
			End If

		Else
			imgRow.Image = _image
		End If
	End Sub

End Class
