Imports System.Web
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web.Configuration
Imports System.Threading
Imports DTIVideoManager.dsDTIVideo

'Thanks to arinhere from CodeProject

''' <summary>
''' helper class to convert uploaded videos to Flash. Handles saving options, bitrates etc.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ffmpegHelper
#Else
            <Serializable()> _
            <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
            Public Class ffmpegHelper
#End If
#Region "Properties"

    ''' <summary>
    ''' Event raised once the video conversio is finished.
    ''' </summary>
    ''' <param name="video_id"></param>
    ''' <param name="conversionTime"></param>
    ''' <param name="errorLog"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Event raised once the video conversio is finished.")> _
    Public Event conversionDone(ByVal video_id As Integer, ByVal conversionTime As Double, ByVal errorLog As String)

    ''' <summary>
    ''' The internal executable that is used to convert the video file.
    ''' </summary>
    ''' <remarks></remarks>
    Enum VideoToolExecuteType
        FFMPeg = 0
        FLVTool2 = 1
    End Enum

    Private _outputFileNamingConvention As String

    ''' <summary>
    ''' The start of the saved output file. default is "dtivid_"
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The start of the saved output file. default is ""dtivid_""")> _
    Public Property OutputFileNamingConvention() As String
        Get
            If _outputFileNamingConvention Is Nothing Then
                _outputFileNamingConvention = "dtivid_"
            End If
            Return _outputFileNamingConvention
        End Get
        Set(ByVal value As String)
            If Not value.EndsWith("_") Then
                value &= "_"
            End If
            _outputFileNamingConvention = value
        End Set
    End Property

    Private _saveVideoInDataBase As Boolean = False
    Private privateSaveVarSet As Boolean = False

    ''' <summary>
    ''' Save the video in the database vs the file system. this defaults to false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Save the video in the database vs the file system. this defaults to false.")> _
    Public Property SaveVideoInDataBase() As Boolean
        Get
            If privateSaveVarSet Then
                Return _saveVideoInDataBase
            Else
                If Not WebConfigurationManager.AppSettings("SaveVideoInDataBase") Is Nothing Then
                    Return WebConfigurationManager.AppSettings("SaveVideoInDataBase")
                Else
                    Return False
                End If
            End If
        End Get
        Set(ByVal value As Boolean)
            _saveVideoInDataBase = value
            privateSaveVarSet = True
        End Set
    End Property

    Private _outputPath As String = System.IO.Path.GetTempPath

    ''' <summary>
    ''' The output path the video file is saved in.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The output path the video file is saved in.")> _
    Public Property OutputPath() As String
        Get
            If Not WebConfigurationManager.AppSettings("VideoSavePath") Is Nothing Then
                Return WebConfigurationManager.AppSettings("VideoSavePath")
            Else
                Return _outputPath
            End If
        End Get
        Set(ByVal value As String)
            _outputPath = value
        End Set
    End Property

    Private _ffmpegExePath As String

    ''' <summary>
    ''' the path to the ffmpeg executable. By default it is extracted from the controll dll and saved to the temp path.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("the path to the ffmpeg executable. By default it is extracted from the controll dll and saved to the temp path.")> _
    Public Property ffmpegExePath() As String
        Get
            Return _ffmpegExePath
        End Get
        Set(ByVal value As String)
            _ffmpegExePath = value
        End Set
    End Property

    Private _flvtool2ExePath As String

    ''' <summary>
    ''' the path to the Flvtool2 executable. By default it is extracted from the controll dll and saved to the temp path.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("the path to the Flvtool2 executable. By default it is extracted from the controll dll and saved to the temp path.")> _
    Public Property Flvtool2ExePath() As String
        Get
            Return _flvtool2ExePath
        End Get
        Set(ByVal value As String)
            _flvtool2ExePath = value
        End Set
    End Property

    Private _sqlhelper As BaseClasses.BaseHelper

    ''' <summary>
    ''' the SQL helper used by this object. Set automatically if not specified.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("the SQL helper used by this object. Set automatically if not specified.")> _
    Public Property sqlhelper() As BaseClasses.BaseHelper
        Get
            Return _sqlhelper
        End Get
        Set(ByVal value As BaseClasses.BaseHelper)
            _sqlhelper = value
        End Set
    End Property

    Private _videoSaveTable As DTIVideoManagerDataTable

    ''' <summary>
    ''' The datatable Object that the video info is saved to. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The datatable Object that the video info is saved to.")> _
    Public ReadOnly Property videoSaveTable() As DTIVideoManagerDataTable
        Get
            If _videoSaveTable Is Nothing Then
                _videoSaveTable = New DTIVideoManagerDataTable
            End If
            Return _videoSaveTable
        End Get
    End Property

    Private _userId As Integer = -1

    ''' <summary>
    ''' A userID field. Optional.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A userID field. Optional.")> _
    Public Property User_Id() As Integer
        Get
            Return _userId
        End Get
        Set(ByVal value As Integer)
            _userId = value
        End Set
    End Property

#End Region

    Private _bitrate As Integer = -1

    ''' <summary>
    ''' The bitrate the video is saved at.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The bitrate the video is saved at.")> _
    Public Property bitrate() As Integer
        Get
			if bitrate = -1 then
				If Not WebConfigurationManager.AppSettings("VideoBitrate") Is Nothing Then
					bitrate = WebConfigurationManager.AppSettings("VideoBitrate")
				else 
					bitrate = 850
				end if
			end if
            Return _bitrate
        End Get
        Set(ByVal value As Integer)
            _bitrate = value
        End Set
    End Property

    Private _otherArgs As String = ""

    ''' <summary>
    ''' Additional args of sent to the ffmpeg converter executable.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Additional args of sent to the ffmpeg converter executable.")> _
    Public Property otherArgs() As String
        Get
            Return _otherArgs
        End Get
        Set(ByVal value As String)
            _otherArgs = value
        End Set
    End Property

    ''' <summary>
    ''' Converts the video to a FLV file for streaming playback.
    ''' </summary>
    ''' <param name="myFile"></param>
    ''' <param name="title"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Converts the video to a FLV file for streaming playback.")> _
    Public Function convertToFLV(ByVal myFile As HttpPostedFile, Optional ByVal title As String = "", Optional ByVal description As String = "") As Integer
        Try
            Dim sTempFileName As String = System.IO.Path.GetTempPath() & myFile.FileName
            Dim fsTemp As New System.IO.FileStream(sTempFileName, IO.FileMode.Create)
            Dim filelen As Integer = myFile.ContentLength
            Dim _myByteArray(filelen) As Byte

            myFile.InputStream.Read(_myByteArray, 0, filelen)
            fsTemp.Write(_myByteArray, 0, filelen)
            fsTemp.Close()

            Dim id As Integer = convertToFLV(sTempFileName, title, description)

            Return id
        Catch ex As Exception
            Return -1
        End Try
    End Function


    Private Shared hasWriteAccess As Boolean = False

    ''' <summary>
    ''' Converts the video to a FLV file for streaming playback.
    ''' </summary>
    ''' <param name="inputPath"></param>
    ''' <param name="title"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Converts the video to a FLV file for streaming playback.")> _
    Public Function convertToFLV(ByVal inputPath As String, Optional ByVal title As String = "", Optional ByVal description As String = "") As Integer
        If Not hasWriteAccess Then
            Dim outputFileName As String = OutputPath & Guid.NewGuid().ToString()
            Try
                If Not Directory.Exists(OutputPath) Then
                    Directory.CreateDirectory(OutputPath)
                End If
                File.WriteAllText(outputFileName, "0")
                File.Delete(outputFileName)
            Catch ex As Exception
                Throw New Exception("The was an error copying the file: " & inputPath & " to the uploads folder " & OutputPath & vbCrLf & " To upload videos you  must create the path: " & OutputPath & " and make sure the IIS process has write permission to that folder.")
            End Try
            hasWriteAccess = True
        End If
        Try

            Dim newVideo As DTIVideoManagerRow = videoSaveTable.NewDTIVideoManagerRow

            'If title <> "" Then
            '    newVideo.Title = title
            'End If
            'If description <> "" Then
            '    newVideo.Description = description
            'End If

            newVideo.Converted = False
            newVideo.OriginalFileName = Path.GetFileName(inputPath)
            Dim returnValues = getInfo(inputPath)
            newVideo.Duration = returnValues(3)
            newVideo.width = returnValues(1)
            newVideo.height = returnValues(2)

            Dim duration As TimeSpan = TimeSpan.Parse(newVideo.Duration)
            If duration > New TimeSpan(0, 0, 8) Then
                newVideo.ScreenShot = createScreenshot(inputPath)
                newVideo.ScreenShotSecondMark = 8
            Else
                Dim newOffset As Integer = duration.Seconds - 1
                If newOffset > 0 Then
                    newVideo.ScreenShot = createScreenshot(inputPath, newOffset)
                    newVideo.ScreenShotSecondMark = newOffset
                Else
                    newVideo.ScreenShot = createScreenshot(inputPath, 1)
                    newVideo.ScreenShotSecondMark = 1
                End If
            End If

            videoSaveTable.AddDTIVideoManagerRow(newVideo)

            Try
                sqlhelper.Update(videoSaveTable)
            Catch ex As Exception
                If Not sqlhelper.checkDBObjectExists(videoSaveTable.TableName) Then
                    sqlhelper.createTable(videoSaveTable)

                    Try
                        sqlhelper.Update(videoSaveTable)
                    Catch innerex As Exception
                        Return -1
                    End Try
                End If
            End Try

            Dim conversionJob As New ParameterizedThreadStart(AddressOf doConversionProcess)
            Dim conversionThread = New Thread(conversionJob)
            conversionThread.Start(New Object() {inputPath, newVideo.Id})

            Return newVideo.Id

        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Sub doConversionProcess(ByVal params As Object)
        Try
            Dim parameterArray As Object() = DirectCast(params, Object())

            Dim inputFilePath As String = parameterArray(0)
            Dim videoId As Integer = parameterArray(1)
            Dim videoRow As DTIVideoManagerRow = videoSaveTable.FindById(videoId)
            If videoRow Is Nothing Then
                sqlhelper.FillDataTable("select * from " & videoSaveTable.TableName & " where Id = " & videoId, videoSaveTable)
                videoRow = videoSaveTable.FindById(videoId)
            End If

            Dim converionTime As Double = -1

            Try
                Dim outputFileName As String = OutputPath & OutputFileNamingConvention & videoRow.Id & ".flv.swf"

                'create flv file
                Dim bitrateStr As String = ""
                If bitrate > -1 Then
                    bitrateStr = " -b " & bitrate & " "
                End If
                Dim cmd As String = " -y -i """ & inputFilePath & """ -ar 22050 " & otherArgs & " " & bitrateStr & " -f flv """ & outputFileName & """"
                Dim returnValues As String() = executeFfmpegCommand(cmd)
                converionTime = returnValues(0)

                'If Not returnValues Is Nothing AndAlso Not returnValues(2) Is Nothing Then
                '    videoRow.width = returnValues(1)
                '    videoRow.height = returnValues(2)
                'End If

                Try
                    System.IO.File.Delete(inputFilePath)
                Catch ex As Exception
                End Try

                'create metaData
                returnValues = executeFfmpegCommand("-U """ & outputFileName & """", False, VideoToolExecuteType.FLVTool2)

                If SaveVideoInDataBase Then
                    videoRow.Video = getByteArrayFromFile(outputFileName)
                    videoRow.Length = videoRow.Video.Length
                Else
                    videoRow.Length = New FileInfo(outputFileName).Length
                End If

                'videoRow.ScreenShot = createScreenshot(outputFileName, videoRow.width, videoRow.height)

                videoRow.Converted = True

                sqlhelper.Update(videoRow.Table)

                If SaveVideoInDataBase Then
                    Try
                        System.IO.File.Delete(outputFileName)
                    Catch ex As Exception
                    End Try
                End If

                RaiseEvent conversionDone(videoRow.Id, converionTime, "")
            Catch ex As Exception
                RaiseEvent conversionDone(videoRow.Id, converionTime, ex.Message)
            End Try
        Catch ex As Exception
            RaiseEvent conversionDone(-1, -1, ex.Message)
        End Try
    End Sub

    Private Function getInfo(ByVal videoPath As String) As Array
        Dim durationArgs As String = " -i """ & videoPath & """"

        Dim returnValues As String() = executeFfmpegCommand(durationArgs, True)

        Return returnValues
    End Function

    Private Function createScreenshot(ByVal videoPath As String, Optional ByVal offset As Integer = 8) As Byte()
        Dim sTempFileName As String = Path.GetTempPath & Path.GetFileNameWithoutExtension(videoPath) & "_Screenshot.jpg"

        Path.GetFileNameWithoutExtension(videoPath)

        Dim imgargs As String = " -y -i """ & videoPath & """ -f image2 -ss " & offset & " -vframes 1 -an """ & _
            sTempFileName & """"

        executeFfmpegCommand(imgargs)

        Dim myBytes As Byte() = getByteArrayFromFile(sTempFileName)

        Try
            System.IO.File.Delete(sTempFileName)
        Catch ex As Exception
        End Try

        Return myBytes
    End Function

    ''' <summary>
    ''' Executes a convertion command
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <param name="grabVals"></param>
    ''' <param name="exePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Executes a convertion command")> _
    Private Function executeFfmpegCommand(ByVal cmd As String, Optional ByVal grabVals As Boolean = False, Optional ByVal exePath As VideoToolExecuteType = VideoToolExecuteType.FFMPeg) As String()


        Dim proc As New System.Diagnostics.Process()

        'Path of exe that will be executed, only for "filebuffer" it will be "flvtool2.exe"
        Dim filename As String = "ffmpeg.exe"
        If exePath = VideoToolExecuteType.FFMPeg Then
            proc.StartInfo.FileName = ffmpegExePath
        ElseIf exePath = VideoToolExecuteType.FLVTool2 Then
            proc.StartInfo.FileName = Flvtool2ExePath
            filename = "flvtool2.exe"
        End If
        If Not System.IO.File.Exists(proc.StartInfo.FileName) Then
            Dim tmpFile As String = Path.GetTempPath.Trim("\") & "\" & filename
            If Not File.Exists(tmpFile) Then
                Dim x As New DTIUploader.DTIUploaderControl()
                Dim str As Stream = x.GetType().Assembly.GetManifestResourceStream("DTIUploader.ffmpeg.exe")
                Dim fs As New System.IO.FileStream(tmpFile, FileMode.Create)
                Dim filebt(str.Length) As Byte
                str.Read(filebt, 0, str.Length)
                fs.Write(filebt, 0, str.Length)
                filebt = Nothing
                str.Close()
                fs.Close()
                str.Dispose()
                fs.Dispose()
            End If
            proc.StartInfo.FileName = tmpFile
        End If

        proc.StartInfo.Arguments = cmd 'The command which will be executed
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.CreateNoWindow = True
        proc.StartInfo.RedirectStandardOutput = False
        proc.StartInfo.RedirectStandardError = True

        proc.Start()

        Dim startTime As DateTime = proc.StartTime

        Dim out As String = proc.StandardError.ReadToEnd

        proc.WaitForExit()

        Dim endTime As DateTime = proc.ExitTime
        Dim elapsedTime As TimeSpan = endTime - startTime
        Dim returnValue(4) As String
        returnValue(0) = elapsedTime.TotalSeconds

        If grabVals Then
            Try
                Dim params As String() = out.Split(" ")

                Dim dimensionRegex As New Regex("\d+x\d+,?")
                Dim LengthRegex As New Regex("\d\d:\d\d:\d\d.\d\d,?")
                For Each param As String In params
                    If dimensionRegex.IsMatch(param) Then
                        Dim dims As String() = dimensionRegex.Match(param).Value.Split("x")
                        If dims.Length > 0 Then
                            If returnValue(1) Is Nothing OrElse dims(0) > returnValue(1) Then
                                returnValue(1) = dims(0)
                            End If
                            If returnValue(2) Is Nothing OrElse dims(1) > returnValue(2) Then
                                If dims(1).EndsWith(",") Then
                                    returnValue(2) = dims(1).Substring(0, dims(1).Length - 1)
                                Else
                                    returnValue(2) = dims(1)
                                End If
                            End If
                        End If
                    End If
                    If LengthRegex.IsMatch(param) Then
                        returnValue(3) = param.Substring(0, param.Length - 1)
                    End If
                Next
            Catch ex As Exception

            End Try
        End If

        Return returnValue
    End Function

    ''' <summary>
    ''' Deletes a video from the database.
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Deletes a video from the database.")> _
    Public Sub deleteVideo(ByVal id As Integer)
        Dim tmp As New DTIVideoManagerDataTable
        sqlhelper.SafeExecuteNonQuery("delete from " & tmp.TableName & " where Id = @videoId", New Object() {id})
    End Sub

    'Public Sub editVideoInfo(ByVal id As Integer, ByVal title As String, ByVal description As String, Optional ByVal sqlConn As SqlClient.SqlConnection = Nothing)
    '    If Not sqlConn Is Nothing Then
    '        sqlConnection = sqlConn
    '    End If

    '    Dim tmpRow As DTIVideoManagerRow = videoSaveTable.FindById(id)
    '    If Not tmpRow Is Nothing Then
    '        editVideoInfo(tmpRow, description, title)
    '    Else
    '        Dim tmp As New DTIVideoManagerDataTable
    '        sqlhelper.SafeExecuteNonQuery("update " & tmp.TableName & " set Title = @title, Description = @desc where Id = @videoId", New Object() {title, description, id})
    '    End If

    'End Sub

    'Public Sub editVideoInfo(ByVal videoRow As DTIVideoManagerRow, ByVal title As String, ByVal description As String, Optional ByVal sqlConn As SqlClient.SqlConnection = Nothing)
    '    If Not sqlConn Is Nothing Then
    '        sqlConnection = sqlConn
    '    End If

    '    videoRow.Description = description
    '    videoRow.Title = title

    '    sqlhelper.Update(videoRow.Table)
    'End Sub

    ''' <summary>
    ''' Updates the screenshot of a video row.
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="secondOffset"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Updates the screenshot of a video row.")> _
    Public Sub updateScreenShot(ByVal id As Integer, ByVal secondOffset As Integer)
        Dim tmpRow As DTIVideoManagerRow = videoSaveTable.FindById(id)
        If tmpRow Is Nothing Then
            sqlhelper.FillDataTable("select * from " & videoSaveTable.TableName & " where Id = " & id, videoSaveTable)
            tmpRow = videoSaveTable.FindById(id)
        End If

        updateScreenShot(tmpRow, secondOffset)

    End Sub

    ''' <summary>
    ''' Updates the screenshot of a video row.
    ''' </summary>
    ''' <param name="videoRow"></param>
    ''' <param name="secondOffset"></param>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Updates the screenshot of a video row.")> _
    Public Sub updateScreenShot(ByVal videoRow As DTIVideoManagerRow, ByVal secondOffset As Integer)
        Dim outputFileName As String = ""
        If Not SaveVideoInDataBase Then
            outputFileName = OutputPath & OutputFileNamingConvention & videoRow.Id & ".flv.swf"
        End If

        Try
            videoRow.ScreenShot = createScreenshot(outputFileName, secondOffset)
            videoRow.ScreenShotSecondMark = secondOffset
            sqlhelper.Update(videoRow.Table)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub New(ByVal myHelper As BaseClasses.BaseHelper, Optional ByVal ffmpegExePathArg As String = "C:\ffmpeg.exe", Optional ByVal flvtool2ExePathArg As String = "C:\flvtool2.exe")
        MyBase.New()
        sqlhelper = myHelper
        ffmpegExePath = ffmpegExePathArg
        Flvtool2ExePath = flvtool2ExePathArg
    End Sub

    ''' <summary>
    ''' Gets the byte array of the input file
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets the byte array of the input file")> _
    Private Function getByteArrayFromFile(ByVal path As String)
        Dim fsTemp As New System.IO.FileStream(path, IO.FileMode.Open)

        Dim filelen As Integer = fsTemp.Length
        Dim _byteArray(filelen) As Byte
        fsTemp.Read(_byteArray, 0, filelen)

        fsTemp.Close()

        Return _byteArray
    End Function
End Class
