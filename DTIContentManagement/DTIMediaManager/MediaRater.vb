''' <summary>
''' star rating control to rate DTI-managed media objects
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class MediaRater
    Inherits DTIMiniControls.StarRater
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class MediaRater
        Inherits DTIMiniControls.StarRater
#End If
        Private _media_row As dsMedia.DTIMediaManagerRow
        Public Property MediaRow() As dsMedia.DTIMediaManagerRow
            Get
                Return _media_row
            End Get
            Set(ByVal value As dsMedia.DTIMediaManagerRow)
                _media_row = value
            End Set
        End Property

        Private _values_per_star As Integer = 4
        Public Property ValuesPerStar() As Integer
            Get
                Return _values_per_star
            End Get
            Set(ByVal value As Integer)
                _values_per_star = value
            End Set
        End Property

        Private Sub MediaRater_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim stepVal As Double = 1 / ValuesPerStar
            Dim i As Double = stepVal
            While i <= NumberOfStars
                Me.Items.Add(New ListItem(i))
                i += stepVal
            End While
            CallbackFunction = "function() {$.ajax({type:""POST"",url:""~/res/DTIMediaManager/RatingHandler.aspx/saveRating""," & _
                "data: ""{'ComponentType':'"" + $(this).closest('.DTIStarRater').attr('componenttype') + ""','ContentType':'" & _
                """ + $(this).closest('.DTIStarRater').attr('contenttype') + ""','ContentId':'"" + " & _
                "$(this).closest('.DTIStarRater').attr('contentid') + ""','Rating':'"" + $(this).val()" & _
                " + ""'}"",contentType: ""application/json; charset=utf-8"",dataType:""json""});}"
        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Not MediaRow Is Nothing Then
                If Not MediaRow.IsRatingNull Then
                    Me.SelectedValue = (((MediaRow.Rating * 100) + Math.Floor(((1 / ValuesPerStar) * 100)) / 2) \ ((1 / ValuesPerStar) * 100)) * ((1 / ValuesPerStar) * 100) / 100
                End If

                Attributes.Add("ComponentType", MediaRow.Component_Type)
                Attributes.Add("ContentType", MediaRow.Content_Type)
                Attributes.Add("ContentId", MediaRow.Content_Id)
            End If

            MyBase.Render(writer)
        End Sub


    End Class
