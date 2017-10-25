Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables
Imports DTIMediaManager.dsMedia
Imports DTIMediaManager.SharedMediaVariables

''' <summary>
''' base media gallery with built-in support for Highslide dialogs
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTISlideGallery
    Inherits DTIMediaGallery
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTISlideGallery
        Inherits DTIMediaGallery
#End If

    ''' <summary>
    ''' Sets to true if the gallery is in a highslide frame.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Sets to true if the gallery is in a highslide frame.")> _
        Public Property IsInnerFrame() As Boolean
            Get
                Return MyHighslideHeader.isInnerFrame
            End Get
            Set(ByVal value As Boolean)
                MyHighslideHeader.isInnerFrame = value
            End Set
        End Property

    ''' <summary>
    ''' Sets to true if the gallery has a highslide frame in it.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Sets to true if the gallery has a highslide frame in it.")> _
        Public Property IsOuterFrame() As Boolean
            Get
                Return MyHighslideHeader.isOuterFrame
            End Get
            Set(ByVal value As Boolean)
                MyHighslideHeader.isOuterFrame = value
            End Set
        End Property

        Private Sub DTISlideGallery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            options.Add("isInnerFrame", New GalleryOptionObject(GalleryOptionObject.OptionTypes.boolean_option, _
                IsInnerFrame))
            callbacks.Add("endRequestHS")
        End Sub

        Private Sub DTISlideGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            MyHighslideHeader.HighslideVariablesString = _
                "var found = false;" & vbCrLf & _
                "for (var i = 0; i < hs.slideshows.length; i++) {" & vbCrLf & _
                "   if(hs.slideshows[i].slideshowGroup == '" & Me.ClientID & "') {found = true; break;}" & vbCrLf & _
                "} if(!found) { hs.addSlideshow({" & vbCrLf & _
                "   slideshowGroup: '" & Me.ClientID & "'," & vbCrLf & _
                "   interval: 5000," & vbCrLf & _
                "   repeat: false," & vbCrLf & _
                "   useControls: true," & vbCrLf & _
                "   fixedControls: false," & vbCrLf & _
                "   overlayOptions: {" & vbCrLf & _
                "      opacity: .6," & vbCrLf & _
                "      position:  'bottom center'," & vbCrLf & _
                "      hideOnMouseOut: true" & vbCrLf & _
                "   }" & vbCrLf & _
                "});}" & vbCrLf

            registerClientStartupScriptBlock("gallerySlideScript", "<script src=""" & _
                BaseClasses.Scripts.ScriptsURL(True) & "/DTIGallery/dtiSlideGallery.js"" type=""text/javascript""></script>")
            If ShowUpload AndAlso Not IsInnerFrame Then
                IsOuterFrame = True
            End If
        End Sub

    ''' <summary>
    ''' Ajax worker class
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Ajax worker class"),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public MustInherit Class SlideGalleryAjaxWorker
        Inherits MediaGalleryAjaxWorker

        Public MaximumThumbnailWidth As Integer = 120
        Public MaximumThumbnailHeight As Integer = 140
        Public ThumbSpanWidth As Integer = 130
        Public ThumbSpanHeight As Integer = 150
        Public RefreshImages As Boolean = False
        Public HighSlide_Color_Mode As Highslide_Outline_Scheme = Highslide_Outline_Scheme.RoundedWhite

        Public Overrides Sub getPage()
            Dim myHeader As HighslideHeaderControl = HighslideHeaderControl.addToPage(Page)
            myHeader.isInnerFrame = Boolean.Parse(optionHash("isInnerFrame"))
            myMediaSearcher.UseSortButtons = True
            MyBase.getPage()
        End Sub

        Public Sub addThumbNail(ByRef thumbControl As Highslider, Optional ByRef secondaryControl As Control = Nothing)
            thumbControl.align = "center"
            thumbControl.slideshowGroup = optionHash("galleryId")

            Dim ctrlTable As New Table
            Dim thumbRow As New TableRow
            Dim thumbCell As New TableCell
            Dim infoRow As New TableRow
            Dim infoCell As New TableCell

            ctrlTable.CellPadding = 0
            ctrlTable.CellSpacing = 0
            ctrlTable.Attributes("border") = "0"
            ctrlTable.Width = New Unit(100, UnitType.Percentage)
            ctrlTable.Height = New Unit(100, UnitType.Percentage)

            thumbCell.CssClass = "thumbImageCell"
            thumbCell.Height = MaximumThumbnailHeight + 10
            thumbRow.Cells.Add(thumbCell)

            infoCell.CssClass = "secondaryControlsCell"
            infoRow.Cells.Add(infoCell)

            ctrlTable.Rows.Add(thumbRow)
            ctrlTable.Rows.Add(infoRow)

            thumbCell.Controls.Add(thumbControl)
            If Not secondaryControl Is Nothing Then
                secondaryControl.EnableViewState = False
                infoCell.Controls.Add(secondaryControl)
            End If

            Dim myThumb As ThumbControl = DirectCast(Page.LoadControl("~/res/DTIGallery/ThumbControl.ascx"), ThumbControl)
            myThumb.ThumbControls.Add(ctrlTable)
            myThumb.ThumbId = "lblThumb_" & slideCount
            myThumb.ThumbSpanHeight = ThumbSpanHeight
            myThumb.ThumbSpanWidth = ThumbSpanWidth

            Page.ResultPanel.Controls.Add(myThumb)
        End Sub

        Protected Function getThumbNailURL(ByVal id As Integer, ByVal type As String)
            For Each gallType As DTIMediaTypesRow In myDTIMediaTypes
                If gallType.Content_Name = type Then
                    Return gallType.ThumbURL & id & "&maxWidth=" & MaximumThumbnailWidth & "&maxHeight=" & MaximumThumbnailHeight
                    Exit Function
                End If
            Next
            Return ""
        End Function
    End Class
    End Class