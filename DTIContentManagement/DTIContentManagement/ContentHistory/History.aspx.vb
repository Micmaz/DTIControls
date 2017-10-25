#If DEBUG Then
Partial Public Class History
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class History
        Inherits BaseClasses.BaseSecurityPage
#End If

#Region "Properties"
        Private ReadOnly Property MainId() As Long
            Get
                Return Request.QueryString("m")
            End Get
        End Property

        Private ReadOnly Property Content_Type() As String
            Get
                Return Request.QueryString("c")
            End Get
        End Property

#End Region

#Region "Data Bindings"


#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.SecurePage = True
        jQueryLibrary.ThemeAdder.AddThemeToIframe(Me)
            If Not Session(DTIServerControls.DTISharedVariables.siteEditOnDefaultKey) Is Nothing AndAlso Session(DTIServerControls.DTISharedVariables.siteEditOnDefaultKey) = True Then
                Me.SecurePage = False
            End If
        End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("c") Is Nothing Then
            pnlRedirectPg.Visible = True
        Else

            If Not IsPostBack Then
                DropDownList1.Items.Clear()
                Dim dt As New DataTable
                Try
                    sqlHelper.SafeFillTable("select id,DateModified from DTIContentManagerHistory where areaName = @areaname and MainID = @mainid ", dt, New Object() {Content_Type, MainId})
                Catch ex As Exception
                    If Not sqlHelper.checkDBObjectExists("DTIContentManagerHistory") Then
                        sqlHelper.createTable(dt)
                        sqlHelper.SafeFillTable("select id,DateModified from DTIContentManagerHistory where areaName = @areaname and MainID = @mainid ", dt, New Object() {Content_Type, MainId})
                    Else
                        Throw ex
                    End If
                End Try
                Dim dv As New DataView(dt, "", "DateModified desc", DataViewRowState.CurrentRows)
                For Each row As DataRowView In dv
                    DropDownList1.Items.Add(New Web.UI.WebControls.ListItem(row("DateModified"), row("id")))
                Next
                If dt.Rows.Count > 0 Then
                    setContent()
                Else
                    Literal1.Text = "No history found for this content area."
                End If

            End If
        End If

    End Sub

        Private Sub setContent()
            Dim dt As New DataTable
            sqlHelper.SafeFillTable("select content from DTIContentManagerHistory where id =@id ", dt, New Object() {DropDownList1.SelectedValue})
            If dt.Rows.Count > 0 AndAlso Not dt.Rows(0)("content") Is DBNull.Value Then
                Literal1.Text = dt.Rows(0)("content")
            End If
        End Sub

        Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
            setContent()
        End Sub
    End Class