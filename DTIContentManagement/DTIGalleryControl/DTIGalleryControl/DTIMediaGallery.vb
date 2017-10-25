Imports DTIGallery
Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports DTIImageManager.dsImageManager
Imports HighslideControls.SharedHighslideVariables
Imports DTIMediaManager
Imports DTIMediaManager.dsMedia

''' <summary>
''' base gallery control for browsing and displaying DTI-managed media objects
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public MustInherit Class DTIMediaGallery
    Inherits DTIGalleryControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public MustInherit Class DTIMediaGallery
        Inherits DTIGalleryControl
#End If
        Public WithEvents mediaSearcher As New DTIMediaManager.MediaSearcher
        Public callbacks As New List(Of String)
        Public options As New Hashtable

#Region "Properties"

    Private _retResOnEmptySearch As Boolean = True

    ''' <summary>
    ''' Return results if 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Return results if")> _
        Public Property ReturnResultsOnEmptySearch() As Boolean
            Get
                Return _retResOnEmptySearch
            End Get
            Set(ByVal value As Boolean)
                _retResOnEmptySearch = value
            End Set
        End Property

#End Region

#Region "Event handlers"

    Private Sub DTIMediaGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        callbacks.Insert(0, "UnfreezeIt" & MyGallery.ThumbFreezer.ClientID)
        Dim callbackString As String = ""
        For Each callString As String In callbacks
            callbackString &= callString & ", "
        Next
        callbackString = callbackString.Trim().Trim(",")

        options.Add("ajaxControlFunction", New GalleryOptionObject(GalleryOptionObject.OptionTypes.Function_option, _
            "ajax" & jsonControl.ClientID))
        options.Add("component_type", New GalleryOptionObject(GalleryOptionObject.OptionTypes.String_option, _
            Component_Type))
        options.Add("content_type", New GalleryOptionObject(GalleryOptionObject.OptionTypes.String_option, contentType))
        options.Add("freezeFunc", New GalleryOptionObject(GalleryOptionObject.OptionTypes.Function_option, _
            "FreezeIt" & MyGallery.ThumbFreezer.ClientID))
        options.Add("unfreezeFunc", New GalleryOptionObject(GalleryOptionObject.OptionTypes.Function_option, _
            "UnfreezeIt" & MyGallery.ThumbFreezer.ClientID))
        options.Add("returnResultsOnEmptySearch", New GalleryOptionObject(GalleryOptionObject.OptionTypes.boolean_option, _
            ReturnResultsOnEmptySearch))
        Dim optionsString As String = ""
        For Each optionKey As String In options.Keys
            Dim gallOpt As GalleryOptionObject = options(optionKey)
            If gallOpt.type = GalleryOptionObject.OptionTypes.String_option Then
                optionsString &= optionKey & ": '" & gallOpt.value & "', "
            ElseIf gallOpt.type = GalleryOptionObject.OptionTypes.boolean_option Then
                optionsString &= optionKey & ": " & gallOpt.value.ToLower & ", "
            Else
                optionsString &= optionKey & ": " & gallOpt.value & ", "
            End If
        Next

        Dim script As String = "$(function() { $('#" & Me.ClientID & "').dtiGallery({" & optionsString & _
            "additionalCallbacks: new Array(" & callbackString & ")});});"
        registerClientScriptBlock("gallInit" & Me.ClientID, script, True)
    End Sub

    Private Sub DTIMediaGallery_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
        searchPanel.Controls.Add(mediaSearcher)
    End Sub

#End Region

    ''' <summary>
    ''' Gallery Options 
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Gallery Options"),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Protected Class GalleryOptionObject
        Public Enum OptionTypes
            Function_option
            String_option
            numerical_option
            boolean_option
        End Enum

        Public type As OptionTypes = OptionTypes.String_option
        Public value As String = ""

        Public Sub New(ByVal _type As OptionTypes, ByVal _val As String)
            type = _type
            value = _val
        End Sub
    End Class

    ''' <summary>
    ''' Ajax worker class
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Ajax worker class"),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public MustInherit Class MediaGalleryAjaxWorker
        Inherits GalleryWorkerBase

        Public Event AddContentFillTables()
        Public myMediaSearcher As New MediaSearcher
        Protected mydsMedia As New dsMedia
        Public slideCount As Integer = 0

        Public Property Component_Type() As String
            Get
                Return myMediaSearcher.Component_Type
            End Get
            Set(ByVal value As String)
                myMediaSearcher.Component_Type = value
            End Set
        End Property

        Public ReadOnly Property Content_Types() As List(Of String)
            Get
                Return myMediaSearcher.Content_Types
            End Get
        End Property

        Protected ReadOnly Property myGallObjects() As DTIMediaManagerDataTable
            Get
                Return mydsMedia.DTIMediaManager
            End Get
        End Property

        Private ReadOnly Property myDTIMediaTypes() As DTIMediaTypesDataTable
            Get
                Return SharedMediaVariables.myDTIMediaTypes
            End Get
        End Property

        Public Overrides Sub getPage()
            If optionHash("returnResultsOnEmptySearch") OrElse optionHash("searchQuery") <> "" Then
                If optionHash("componentType") IsNot Nothing AndAlso optionHash("componentType") <> "" Then
                    myMediaSearcher.Component_Type = optionHash("componentType")
                End If
                myMediaSearcher.SearchText = optionHash("searchQuery")
                If optionHash("searchSort") <> "" Then
                    myMediaSearcher.Sort = optionHash("searchSort")
                End If

                RaiseEvent AddContentFillTables()
                Try
                    doExec(mydsMedia, myGallObjects.TableName, , items_per_page, optionHash("pageNumber"), _
                        myMediaSearcher.Sort, myMediaSearcher.QueryFilter, myMediaSearcher.SearchColumns)
                Catch ex As Exception
                    myMediaSearcher.createSqlFunctions()
                    doExec(mydsMedia, myGallObjects.TableName, , items_per_page, optionHash("pageNumber"), _
                    myMediaSearcher.Sort, myMediaSearcher.QueryFilter, myMediaSearcher.SearchColumns)
                End Try
            End If
            preparePage()
        End Sub

        Private Sub preparePage()
            If myGallObjects.Count = 0 Then
                Dim nothingPanel As New Panel
                nothingPanel.Style("color") = "red"
                nothingPanel.Controls.Add(New LiteralControl("No results found."))
                Page.ResultPanel.Controls.Add(nothingPanel)
            Else
                For Each gallObj As DTIMediaManagerRow In myGallObjects
                    For Each gallType As DTIMediaTypesRow In myDTIMediaTypes
                        If gallObj.Content_Type = gallType.Content_Name Then
                            getThumbnail(gallType, gallObj)
                            slideCount += 1
                            Exit For
                        End If
                    Next
                Next
            End If
            Dim pagesPanel As New Panel
            pagesPanel.Style("display") = "none"
            pagesPanel.CssClass = "totalPagesDiv"
            pagesPanel.Controls.Add(New LiteralControl(total_pages))
            Page.ResultPanel.Controls.Add(pagesPanel)
        End Sub

        Protected MustOverride Sub getThumbnail(ByRef content_type_row As DTIMediaTypesRow, ByRef media_row As DTIMediaManagerRow)

    End Class


    End Class
