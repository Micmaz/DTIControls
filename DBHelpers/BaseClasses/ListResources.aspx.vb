''' <summary>
''' Lists available embedded resources.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Partial Public Class ListResources
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ListResources
        Inherits BaseClasses.BaseSecurityPage
#End If

        Private Sub ListResources_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.SecurePage = False
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            addCell(Table1, "<b>searchPath: " & AppDomain.CurrentDomain.RelativeSearchPath & "</b>", 2, Drawing.Color.Gray)
            Me.Table1.Rows.Add(New Web.UI.WebControls.TableRow)

            If Not BaseClasses.BaseVirtualPathProvider.initialized Then BaseClasses.BaseVirtualPathProvider.buildLocalResources()
            Dim keys(BaseClasses.BaseVirtualPathProvider.resources.Keys.Count - 1) As String
            BaseClasses.BaseVirtualPathProvider.resources.Keys.CopyTo(keys, 0)
            Array.Sort(keys)
            Dim asm As System.Reflection.Assembly = Nothing
            Dim currcolor As System.Drawing.Color = Drawing.Color.White
            For Each key As String In keys
                Me.Table1.Rows.Add(New Web.UI.WebControls.TableRow)
                Dim obj As Object() = BaseClasses.BaseVirtualPathProvider.resources(key)
                If obj Is Nothing Then
                    addCell(Table1, "Both Nothing")
                Else
                    Try
                        If asm Is Nothing OrElse Not obj(0) Is asm Then
                            asm = obj(0)
                            addCell(Table1, asm.CodeBase & "<br>" & asm.FullName, 2, Drawing.Color.CornflowerBlue)
                            Me.Table1.Rows.Add(New Web.UI.WebControls.TableRow)
                            currcolor = Drawing.Color.Wheat
                        End If
                        Dim rname As String = obj(1)
                        addCell(Table1, "<a href=~/res/" & key & ">" & key & "</a>", 1, currcolor)
                        addCell(Table1, rname, 1, currcolor)
                        If currcolor = Drawing.Color.White Then currcolor = Drawing.Color.Wheat Else currcolor = Drawing.Color.White
                    Catch ex As Exception
                        addCell(Table1, key)
                        addCell(Table1, "ERROR: " & ex.Message)
                    End Try
                End If
            Next
        End Sub

        Public Shared Sub addCell(ByRef row As Web.UI.WebControls.TableRow, ByVal celltext As String, ByVal colspan As Integer, ByVal backcolor As System.Drawing.Color)
            Dim cell As New Web.UI.WebControls.TableCell
            If Not backcolor = Drawing.Color.White Then cell.BackColor = backcolor
            If colspan > 1 Then cell.ColumnSpan = colspan
            cell.Text = celltext
            row.Cells.Add(cell)
        End Sub


        Public Shared Sub addCell(ByRef row As Web.UI.WebControls.TableRow, ByVal celltext As String, Optional ByVal colspan As Integer = 1)
            addCell(row, celltext, colspan, Drawing.Color.White)
        End Sub

        Public Shared Sub addCell(ByRef table As Web.UI.WebControls.Table, ByVal celltext As String, ByVal colspan As Integer, ByVal backcolor As System.Drawing.Color)
            If table.Rows.Count = 0 Then
                table.Rows.Add(New Web.UI.WebControls.TableRow)
            End If
            addCell(table.Rows(table.Rows.Count - 1), celltext, colspan, backcolor)
        End Sub

        Public Shared Sub addCell(ByRef table As Web.UI.WebControls.Table, ByVal celltext As String, Optional ByVal colspan As Integer = 1)
            addCell(table, celltext, colspan, Drawing.Color.White)
        End Sub


    End Class