Imports System
Imports System.Collections
Imports System.Text

''' <summary>
''' Handles tool groups within a toolbar.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class ToolbarGroup
    Inherits CollectionBase
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ToolbarGroup
        Inherits CollectionBase
#End If

        Default Public Property Item(ByVal index As Integer) As String
            Get
                Return CType(List(index), String)
            End Get
            Set(ByVal value As String)
                List(index) = value
            End Set
        End Property

        Public Function Add(ByVal value As String) As String
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As String) As String
            Return List.IndexOf(value)
        End Function 'IndexOf

        Public Sub Insert(ByVal index As Integer, ByVal value As String)
            List.Insert(index, value)
        End Sub 'Insert

        Public Sub Remove(ByVal value As String)
            List.Remove(value)
        End Sub 'Remove

        Public Function Contains(ByVal value As String) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
            ' Insert additional code to be run only when inserting values.
        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As Object)
            ' Insert additional code to be run only when removing values.
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            ' Insert additional code to be run only when setting values.
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not GetType(String).IsAssignableFrom(value.GetType()) Then
                Throw New ArgumentException("value must be of type String.", "value")
            End If
        End Sub 'OnValidate 

        Public Sub New(ByRef arr As String())
            For Each str As String In arr
                Me.Add(str)
            Next
        End Sub

        Public Sub New()

        End Sub

        Public Overrides Function ToString() As String
            Dim strbldr As String = ""
            strbldr &= "["
            For Each tool As String In List
                strbldr &= """" & tool & ""","
            Next
            strbldr = strbldr.Trim(",")
            strbldr &= "]"
            Return strbldr
        End Function



    End Class
