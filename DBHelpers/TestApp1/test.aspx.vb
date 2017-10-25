Public Partial Class test
    Inherits BaseClasses.BaseSecurityPage

    Public ReadOnly Property demographicsConn() As System.Data.Common.DbConnection
        Get
            Return New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("Demographics").ConnectionString)
        End Get
    End Property

    Private dhelper As BaseClasses.BaseHelper
    Public ReadOnly Property demoGraphicsHelper() As BaseClasses.BaseHelper
        Get
            If dhelper Is Nothing Then
                dhelper = Data.createHelper(demographicsConn)
            End If
            Return dhelper
        End Get
    End Property


    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim helper As BaseClasses.BaseHelper = demoGraphicsHelper
        Dim ds As New dsTester
        helper.checkAndCreateAllTables(ds)
        Dim row As dsTester.Client_ImageRow = ds.Client_Image.NewRow
        row.contentType = ""
        row.Image = New Byte() {0}
        ds.Client_Image.Rows.Add(row)
        Dim da As Data.Common.DbDataAdapter = helper.Adaptor("case_info", , ds.Case_Info)
        helper.Update(ds.Client_Image)
    End Sub
End Class