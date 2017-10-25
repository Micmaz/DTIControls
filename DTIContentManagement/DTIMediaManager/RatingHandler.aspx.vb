Imports System.Web.Services
Imports DTIServerControls

#If DEBUG Then
Partial Public Class RatingHandler
    Inherits SettingsEditorPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class RatingHandler
        Inherits SettingsEditorPage
#End If
        Public Shared RaterHash As New ArrayList


        '***fixme database specific
        <WebMethod()> _
        Public Shared Function saveRating(ByVal ComponentType As String, ByVal ContentType As String, ByVal ContentId As String, ByVal Rating As String)
            If Not RaterHash.Contains(SharedSession.SessionID & ComponentType & ContentType & ContentId) Then
                RaterHash.Add(SharedSession.SessionID & ComponentType & ContentType & ContentId)

                Try
                    sharedSqlHelper.SafeExecuteNonQuery("exec UpdateMediaRating @compType, @contType, @contId, @rating", _
                        New Object() {ComponentType, ContentType, ContentId, Rating})
                Catch ex As Exception
                    Dim script As String = "CREATE PROCEDURE UpdateMediaRating( " & vbCrLf & _
                        "  @compType VARCHAR(100), " & vbCrLf & _
                        "  @contType VARCHAR(100), " & vbCrLf & _
                        "  @contId INT, " & vbCrLf & _
                        "  @rating FLOAT   " & vbCrLf & _
                        ") AS   " & vbCrLf & _
                        " " & vbCrLf & _
                        "DECLARE @RateCount BIGINT  " & vbCrLf & _
                        "DECLARE @TotalRating DECIMAL " & vbCrLf & _
                        "DECLARE @AvgNumOfRatingsForAll BIGINT " & vbCrLf & _
                        "DECLARE @AvgRatingForAll DECIMAL " & vbCrLf & _
                        " " & vbCrLf & _
                        "UPDATE DTIMediaManager set  " & vbCrLf & _
                        "Rating_Count = isnull(Rating_Count, 0) + 1,  " & vbCrLf & _
                        "Rating_Sum = isnull(Rating_Sum, 0) + @rating where " & vbCrLf & _
                        "Component_Type = @compType and Content_Type = @contType and Content_Id = @contId " & vbCrLf & _
                        " " & vbCrLf & _
                        "select @RateCount = Rating_Count, @TotalRating = Rating_Sum  from DTIMediaManager where  " & vbCrLf & _
                        "Component_Type = @compType and Content_Type = @contType and Content_Id = @contId " & vbCrLf & _
                        " " & vbCrLf & _
                        "select @AvgNumOfRatingsForAll = AVG(Rating_Count), @AvgRatingForAll = AVG(isnull(Rating, 0)) " & vbCrLf & _
                        "from DTIMediaManager where  " & vbCrLf & _
                        "Component_Type = @compType and Content_Type = @contType " & vbCrLf & _
                        " " & vbCrLf & _
                        "UPDATE DTIMediaManager SET Rating =  " & vbCrLf & _
                        "((@AvgNumOfRatingsForAll * @AvgRatingForAll) + @TotalRating) /  " & vbCrLf & _
                        "(@RateCount + @AvgNumOfRatingsForAll) where  " & vbCrLf & _
                        "Component_Type = @compType and Content_Type = @contType and Content_Id = @contId " & vbCrLf & _
                        " " & vbCrLf & _
                        "RETURN 0"
                    sharedSqlHelper.ExecuteNonQuery(script)
                    sharedSqlHelper.SafeExecuteNonQuery("exec UpdateMediaRating @compType, @contType, @contId, @rating", _
                        New Object() {ComponentType, ContentType, ContentId, Rating})
                End Try
            End If
            Return 1
        End Function

    End Class