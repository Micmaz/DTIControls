Imports DTIGallery
Imports DTIMediaManager

''' <summary>
''' Gallery control dedicated to displaying search results
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class SearchResultGallery
    Inherits DTIMediaGallery
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SearchResultGallery
        Inherits DTIMediaGallery
#End If

        Private Sub SearchResultGallery_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            ReturnResultsOnEmptySearch = False
            ShowSearching = True
            GalleryWidth = 400
            jsonControl.workerclass = "DTIContentManagement.SearchResultGallery+SearchGalleryAjaxWorker"
            Component_Type = "ContentManagement"
            callbacks.Add("initStarRaters")
        End Sub

        Private Sub SearchResultGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim scriptLoader As New DTIMediaManager.MediaRater
            scriptLoader.ValuesPerStar = 1
            Controls.Add(scriptLoader)
            scriptLoader.Visible = False
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/jQueryLibrary/jquery.truncator.js", , True)
        End Sub

    ''' <summary>
    ''' The ajax worker class that dynamically loads the search results.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The ajax worker class that dynamically loads the search results.")> _
        Public Class SearchGalleryAjaxWorker
            Inherits DTIMediaGallery.MediaGalleryAjaxWorker

            Private searchResultHash As New Hashtable

            Private _myPages As dsMedia.DTIMediaManagerDataTable
            Protected ReadOnly Property MyPages() As dsMedia.DTIMediaManagerDataTable
                Get
                    If _myPages Is Nothing Then
                        _myPages = New dsMedia.DTIMediaManagerDataTable
                        _myPages.TableName = optionHash("galleryId")
                    End If
                    Return _myPages
                End Get
            End Property

            Private Sub SearchResultGallery_AddContentFillTables() Handles Me.AddContentFillTables
                parallelhelper.addFillDataTable("select distinct a.* from DTIMediaManager a, DTIMediaManager b " & _
                    "where a.Published <> 0 and a.Removed <> 1 and a.Component_Type = 'ContentManagement' " & _
                    "and a.Content_Type = 'page' and b.Component_Type = 'ContentManagement' and b.Content_Type = " & _
                    "'editPanel' and a.Permanent_URL like b.Permanent_URL + '%'", MyPages)
            End Sub

            Protected Overrides Sub getThumbnail(ByRef content_type_row As DTIMediaManager.dsMedia.DTIMediaTypesRow, ByRef media_row As DTIMediaManager.dsMedia.DTIMediaManagerRow)
                If content_type_row.Content_Name = "page" Then
                    addPageSummary(media_row)
                ElseIf content_type_row.Content_Name = "editPanel" AndAlso Not media_row.IsPermanent_URLNull Then
                    Dim found As Boolean = False
                    For Each row As dsMedia.DTIMediaManagerRow In searchResultHash.Keys
                        If media_row.Permanent_URL.Contains(row.Permanent_URL) Then
                            found = True
                            Exit For
                        End If
                    Next
                    If Not found Then
                        For Each row As dsMedia.DTIMediaManagerRow In MyPages
                            If media_row.Permanent_URL.Contains(row.Permanent_URL) AndAlso row.Content_Type = "page" Then
                                addPageSummary(row)
                                Exit For
                            End If
                        Next
                    End If
                End If
            End Sub

            Public Function addPageSummary(ByRef media_row As dsMedia.DTIMediaManagerRow) As Control
                Dim searchResult As New PageSummary
                Page.ResultPanel.Controls.Add(searchResult)
                searchResult.HighlightedWords = myMediaSearcher.SearchWords
                searchResult.MediaRow = media_row
                searchResultHash.Add(media_row, searchResult)
                Return searchResult
            End Function

            Public Sub New()
                myMediaSearcher.Content_Types.AddRange(New String() {"page", "editPanel"})
                myMediaSearcher.mediaSearchTableEntries.Add(New MediaSearcher.mediaSearchTable("editPanel", _
                "DTIContentManager", "content"))
            End Sub
        End Class


    End Class
