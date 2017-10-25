Imports DTIVideoManager.dsDTIVideo
Imports HighslideControls.SharedHighslideVariables

#If DEBUG Then
Partial Public Class VideoPreview
    Inherits BaseClasses.BaseSecurityUserControl
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Partial Public Class VideoPreview
            Inherits BaseClasses.BaseSecurityUserControl
#End If
    Public MyFlashWrapper As ffmpegHelper

    Private _stepAndMax As Integer = 1
    Public Property StepAndMax() As Integer
        Get
            Return _stepAndMax
        End Get
        Set(ByVal value As Integer)
            _stepAndMax = value
        End Set
    End Property

    Private _sliderValue As Integer = 1
    Public Property SliderValue() As Integer
        Get
            Return _sliderValue
        End Get
        Set(ByVal value As Integer)
            _sliderValue = value
        End Set
    End Property

    Public Property VideoOutline() As Highslide_Outline_Scheme
        Get
            Return VideoThumb1.Outline_Scheme
        End Get
        Set(ByVal value As Highslide_Outline_Scheme)
            VideoThumb1.Outline_Scheme = value
        End Set
    End Property

    Public isThumbChange As Boolean = False
    Private ReadOnly Property MyThumbNum() As Integer
        Get
            If Session("MyThumbNailNum_" & myVideoRow.Id) Is Nothing OrElse isThumbChange Then
                Dim rdmNumGen As New Random
                Session("MyThumbNailNum_" & myVideoRow.Id) = rdmNumGen.Next
            End If
            Return Session("MyThumbNailNum_" & myVideoRow.Id)
        End Get
    End Property

    Private _myVideoRow As DTIVideoManagerRow
    Public Property myVideoRow() As DTIVideoManagerRow
        Get
            Return _myVideoRow
        End Get
        Set(ByVal value As DTIVideoManagerRow)
            _myVideoRow = value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If myVideoRow Is Nothing OrElse myVideoRow.RowState = DataRowState.Deleted Then
            Me.Visible = False
        Else
            Me.Visible = True

            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/jQueryLibrary/jquery-ui.1.8.1.min.js", , True)
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/DTIAjax/jquery.timers-1.2.js", , True)
            jQueryLibrary.jQueryInclude.addScriptBlock(Me.Page, "$.query = { prefix: false };")
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "jQueryLibrary/jquery.query.js")
            jQueryLibrary.ThemeAdder.AddTheme(Me.Page, jQueryLibrary.ThemeAdder.themes.ui_lightness)
            Dim myTimeSpan As TimeSpan = TimeSpan.Parse(myVideoRow.Duration)
            StepAndMax = myTimeSpan.TotalSeconds

            lblMax.Text = myVideoRow.Duration

            SliderValue = myVideoRow.ScreenShotSecondMark

            If myVideoRow.IswidthNull Then
                myVideoRow.width = 0
            End If
            If myVideoRow.IsheightNull Then
                myVideoRow.height = 0
            End If
            lblDimensionsValue.Text = myVideoRow.width & " x " & myVideoRow.height

            If Not myVideoRow.IsOriginalFileNameNull Then
                lblRawFileValue.Text = myVideoRow.OriginalFileName
            End If

            If Not myVideoRow.IsDurationNull Then
                lblTimeValue.Text = myVideoRow.Duration
            End If

            VideoThumb1.VideoId = myVideoRow.Id
            VideoThumb1.VideoWidth = myVideoRow.width
            VideoThumb1.VideoHeight = myVideoRow.height
            VideoThumb1.align = "center"
            VideoThumb1.ThumbURL = "~/res/DTIVideoManager/ViewVideoScreenShot.aspx?Id=" & myVideoRow.Id & _
                            "&maxHeight=100&maxWidth=100&showPlayOverlay=1&r=" & MyThumbNum

            If Not myVideoRow.IsConvertedNull AndAlso Not myVideoRow.Converted Then
                lblStatusValue.Text = "Processing . . . "
                lblStatusValue.Font.Bold = False
                videoHolder.Style("display") = "none"

                imgScreenshot.ImageUrl = "~/res/DTIVideoManager/ViewVideoScreenShot.aspx?Id=" & myVideoRow.Id & _
                "&maxHeight=100&maxWidth=100&showPlayOverlay=1&r=" & MyThumbNum
            Else
                lblStatusValue.Text = "Processing Complete"
                lblStatusValue.Font.Bold = True
                imgStatus.Visible = False
                imgScreenshot.Visible = False
                checkConversionScript.Visible = False

                pnlScreenshot.Attributes.Add("onmouseover", "$('#" & pnlScreenShotEditor.ClientID & "').show();")
                pnlScreenshot.Attributes.Add("onmouseout", "$('#" & pnlScreenShotEditor.ClientID & "').hide();")
            End If

        End If
    End Sub
End Class