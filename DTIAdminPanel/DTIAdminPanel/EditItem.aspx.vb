Imports DTIMediaManager

#If DEBUG Then
Partial Public Class EditItem
    Inherits DTIServerControls.DTIServerControlBasePage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class EditItem
        Inherits DTIServerControls.DTIServerControlBasePage
#End If

#Region "Properties"
        Private _myPageEditor As PageEditControl
        Private ReadOnly Property MyPageEditor() As PageEditControl
            Get
                If _myPageEditor Is Nothing Then
                    _myPageEditor = DirectCast(Page.LoadControl("~/res/DTIAdminPanel/PageEditControl.ascx"), PageEditControl)
                    _myPageEditor.dynpage = ds.DTIDynamicPage.FindByid(pid)
                End If
                Return _myPageEditor
            End Get
        End Property

        Protected ReadOnly Property ds() As dsDTIAdminPanel
            Get
                If Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") Is Nothing Then
                    Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") = New dsDTIAdminPanel
                End If
                Return Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl")
            End Get
        End Property

        Private _id As Integer = -1
        Private ReadOnly Property pid() As Integer
            Get
                If _id = -1 Then
                    Try
                        Return Integer.Parse(Request.QueryString("id"))
                    Catch
                        Return -1
                    End Try
                End If
            End Get
        End Property

        Private _medRow As DTIMediaManager.dsMedia.DTIMediaManagerRow
        Private ReadOnly Property mediaRow() As DTIMediaManager.dsMedia.DTIMediaManagerRow
            Get
                If _medRow Is Nothing AndAlso pid <> -1 Then
                    For Each row As DTIMediaManager.dsMedia.DTIMediaManagerRow In SharedMediaVariables.myMediaTable
                        If row.Content_Type = "page" AndAlso row.Content_Id = pid Then
                            _medRow = row
                            Exit For
                        End If
                    Next
                    If _medRow Is Nothing Then
                        sqlHelper.SafeFillTable("select * from DTIMediaManager where Content_Id = @pid and " & _
                            "Content_Type = @contType", SharedMediaVariables.myMediaTable, _
                            New Object() {pid, "page"})

                        For Each row As DTIMediaManager.dsMedia.DTIMediaManagerRow In SharedMediaVariables.myMediaTable
                            If row.Content_Type = "page" AndAlso row.Content_Id = pid Then
                                _medRow = row
                                Exit For
                            End If
                        Next

                        If _medRow Is Nothing Then
                            _medRow = SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow
                            With _medRow
                                .Component_Type = "contentManagement"
                                .Content_Type = "page"
                                .Content_Id = pid
                                .Date_Added = Now
                                .User_Id = -1 'currentUser.id
                                .Permanent_URL = Request.Url.AbsolutePath
                            End With
                            SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(_medRow)
                        End If
                    End If
                End If
                Return _medRow
            End Get
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            editMedia1.myControlHolder.Add(MyPageEditor)
            editMedia1.MyMediaRow = mediaRow
        End Sub

        Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            MyPageEditor.save()
            mediaRow.Description = editMedia1.Description
            mediaRow.Title = editMedia1.Title
            sqlHelper.Update(SharedMediaVariables.myMediaTable)
            editMedia1.saveTags(mediaRow.Id)

            Response.Redirect("~/res/DTIAdminPanel/PageList.aspx")
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("~/res/DTIAdminPanel/PageList.aspx")
        End Sub

        Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            btnSave.OnClientClick = "prepareCurrentTags();"
        End Sub
    End Class