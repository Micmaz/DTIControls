Public Class PollChart
    Inherits Panel

    Private _hasdatabound As Boolean = False

#Region "Properties"
    Private _datasource As Object

    ''' <summary>
    ''' Gets or sets the data source that the PollChart is displaying data for.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the data source that the PollChart is displaying data for.")> _
    Public Property DataSource() As Object
        Get
            Return _datasource
        End Get
        Set(ByVal value As Object)
            _datasource = value
        End Set
    End Property

    Private _dataMember As String = ""

    ''' <summary>
    ''' Gets or sets the name of the list or table in the data source for which the PollChart is displaying data.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the name of the list or table in the data source for which the PollChart is displaying data.")> _
    Public Property DataMember() As String
        Get
            Return _dataMember
        End Get
        Set(ByVal value As String)
            _dataMember = value
        End Set
    End Property

    Protected ReadOnly Property dt() As DataTable
        Get
            If _datasource Is Nothing OrElse Not TypeOf _datasource Is DataTable Then
                If TypeOf _datasource Is DataView Then
                    _datasource = CType(_datasource, DataView).ToTable
                ElseIf TypeOf _datasource Is DataSet AndAlso DataMember <> "" Then
                    Dim ds As DataSet = CType(_datasource, DataSet)
                    _datasource = ds.Tables(DataMember)
                Else
                    _datasource = Nothing
                End If
            End If
            Return _datasource
        End Get
    End Property

    Private _voteString As String = "votes"

    ''' <summary>
    ''' Display Text of type of data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Display Text of type of data")> _
    Public Property VoteString() As String
        Get
            If _voteString <> "" Then
                _voteString = " " & _voteString
            End If
            Return _voteString
        End Get
        Set(ByVal value As String)
            _voteString = value
        End Set
    End Property

    Private _dataTextField As String

    ''' <summary>
    ''' Gets or sets the field of the data source that provides the text of each Poll item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the field of the data source that provides the text of each Poll item.")> _
    Public Property DataTextField() As String
        Get
            Return _dataTextField
        End Get
        Set(ByVal value As String)
            _dataTextField = value
        End Set
    End Property

    Private _dataValueField As String

    ''' <summary>
    ''' Gets or sets the field of the data source that provides the value of each Poll item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Gets or sets the field of the data source that provides the value of each Poll item.")> _
    Public Property DataValueField() As String
        Get
            Return _dataValueField
        End Get
        Set(ByVal value As String)
            _dataValueField = value
        End Set
    End Property
#End Region

    Public Shared Function MixedCase(ByVal origText As String) As String
        Dim counter As Integer
        Dim textParts() As String = Split(origText, " ")

        For counter = 0 To textParts.Length - 1
            If (textParts(counter).Length > 0) Then
                textParts(counter) = UCase(Microsoft.VisualBasic.Left(textParts(counter), 1)) & LCase(Mid(textParts(counter), 2))
            End If
        Next counter

        Return Join(textParts, " ")
    End Function

    Public Overrides Sub DataBind()
        If dt IsNot Nothing Then
            Dim sum As Integer = CType(dt.Compute("sum(" & DataValueField & ")", ""), Integer)

            Me.Controls.Clear()
            For Each row As DataRow In dt.Rows
                Dim percentStr = FormatPercent(row(DataValueField) / sum, 1)
                Dim percent As Double = Double.Parse(percentStr.Replace("%", ""))
                Me.Controls.Add(New LiteralControl("<div>" & row(DataTextField) & " " & percent & "% (" & FormatNumber(row(DataValueField), 0, , , TriState.True) & VoteString & ")</div>" & vbCrLf))
                Me.Controls.Add(New ProgressBar(FormatNumber(percent, 0, TriState.True, TriState.False, TriState.False)))
            Next
            Me.Controls.Add(New LiteralControl("<div>Total" & MixedCase(VoteString) & ": " & FormatNumber(sum, 0, , , TriState.True) & "</div>"))
        End If
        _hasdatabound = True
    End Sub

    Private Sub BarChart_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not _hasdatabound Then DataBind()
    End Sub
End Class
