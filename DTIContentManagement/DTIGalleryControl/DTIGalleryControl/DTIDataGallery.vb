Imports DTIServerControls.DTISharedVariables

''' <summary>
''' Data-centric gallery base class
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public MustInherit Class DTIDataGallery
    Inherits DTIGalleryControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public MustInherit Class DTIDataGallery
        Inherits DTIGalleryControl
#End If
        Public DoPreloading As Boolean = True

        Protected Event HandlePreloads(ByRef preloadsTabls As DataTable)

        Private Sub DTIDataGallery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not mypage.IsPostBack Then
                CurrentPage = 1
            End If
        End Sub

        Public Sub doExec(ByRef ds As DataSet, ByVal tableName As String, Optional ByVal primary_Key As String = "Id", Optional ByVal pageSize As Integer = 10, Optional ByRef pageNum As Integer = 1, Optional ByVal sort As String = "", Optional ByVal queryFilter As String = "", Optional ByVal cols As String = "")
            If DoPreloading Then
                If Not ds.Tables.Contains("PreloadTable") Then
                    ds.Tables.Add("PreloadTable")
                Else
                    ds.Tables("PreloadTable").Clear()
                End If
            End If

            parallelhelper.addGetSortedPage(ds, tableName, tableName, primary_Key, pageSize, pageNum, _
                sort, queryFilter, cols)
            If DoPreloading Then
                If pageNum > 1 Then
                    parallelhelper.addGetSortedPage(ds, tableName, "PreloadTable", primary_Key, pageSize, pageNum - 1, _
                        sort, queryFilter, cols)
                End If
                parallelhelper.addGetSortedPage(ds, tableName, "PreloadTable", primary_Key, pageSize, pageNum + 1, _
                    sort, queryFilter, cols)
            End If

            parallelhelper.executeParallelDBCall()

            TotalPages = ds.Tables("PageCount").Rows(0)(0)

            If pageNum > TotalPages Then
                pageNum = TotalPages

                ds.Clear()

                parallelhelper.addGetSortedPage(ds, tableName, tableName, primary_Key, pageSize, pageNum, _
                sort, queryFilter, cols)
                If DoPreloading Then
                    If pageNum > 1 Then
                        parallelhelper.addGetSortedPage(ds, tableName, "PreloadTable", primary_Key, pageSize, pageNum - 1, _
                            sort, queryFilter, cols)
                    End If
                    parallelhelper.addGetSortedPage(ds, tableName, "PreloadTable", primary_Key, pageSize, pageNum + 1, _
                        sort, queryFilter, cols)
                End If

                parallelhelper.executeParallelDBCall()

                TotalPages = ds.Tables("PageCount").Rows(0)(0)
            End If
            If DoPreloading Then RaiseEvent HandlePreloads(ds.Tables("PreloadTable"))

        End Sub

        Protected MustOverride Sub PreloadReady(ByRef preloadsTable As DataTable) Handles Me.HandlePreloads
    End Class
