Imports System
Imports System.Collections
Imports System.Text

''' <summary>
''' A seperate toolar class.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class Toolbar
    Inherits CollectionBase

#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Toolbar
        Inherits CollectionBase
#End If

        Default Public Property Item(ByVal index As Integer) As ToolbarGroup
            Get
                Return CType(List(index), ToolbarGroup)
            End Get
            Set(ByVal value As ToolbarGroup)
                List(index) = value
            End Set
        End Property

        Public Function Add(ByVal value As String) As Integer
            Dim grp As New ToolbarGroup(New String() {value})
            Return List.Add(grp)
        End Function 'Add

        Public Function Add(ByVal value As ToolbarGroup) As Integer
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As ToolbarGroup) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf

        Public Sub Insert(ByVal index As Integer, ByVal value As ToolbarGroup)
            List.Insert(index, value)
        End Sub 'Insert

        Public Sub Remove(ByVal value As ToolbarGroup)
            List.Remove(value)
        End Sub 'Remove

        Public Function Contains(ByVal value As ToolbarGroup) As Boolean
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
            If Not GetType(ToolbarGroup).IsAssignableFrom(value.GetType()) Then
                Throw New ArgumentException("value must be of type Toolbar.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function ToString() As String
            Dim strbldr As String = ""
            strbldr &= "["
            For Each grp As ToolbarGroup In List
                strbldr &= grp.ToString & ","
            Next
            strbldr = strbldr.Trim(",")
            strbldr &= "]"
            Return strbldr.ToString
        End Function

#Region "toolbarPresets"

        Public Enum ToolbarLayout
            FullToolbar
            ModerateToolbar
            MinimalToolbar

        End Enum

        Public Shared Function ToolbarFromLayout(ByVal tl As ToolbarLayout) As Toolbar
            If tl = ToolbarLayout.FullToolbar Then
                Return fullToolbar()
            ElseIf tl = ToolbarLayout.ModerateToolbar Then
                Return moderateToolbar()
            ElseIf tl = ToolbarLayout.MinimalToolbar Then
                Return minimalToolbar()
        End If
        Return moderateToolbar()
        End Function

        Public Shared Function fullToolbar() As Toolbar
            Dim Toolbar As New Toolbar
        Toolbar.Add(New ToolbarGroup(New String() {"Sourcedialog"}))
            Toolbar.Add(New ToolbarGroup(New String() {"ShowBlocks"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Cut", "Copy", "Paste", "PasteText", "PasteFromWord", "RemoveFormat"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Undo", "Redo"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Find"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Styles", "Format"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Font", "FontSize"}))
            Toolbar.Add(New ToolbarGroup(New String() {"TextColor", "BGColor"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Bold", "Italic", "Underline", "Strike"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Subscript", "Superscript"}))
            Toolbar.Add(New ToolbarGroup(New String() {"NumberedList", "BulletedList"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Outdent", "Indent"}))
            Toolbar.Add(New ToolbarGroup(New String() {"JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Link", "Unlink", "Anchor"}))
        Toolbar.Add(New ToolbarGroup(New String() {"Image", "ImageMap", "Table", "HorizontalRule", "SpecialChar", "Flash"}))
            Return Toolbar
        End Function

        Public Shared Function minimalToolbar() As Toolbar
            Dim Toolbar As New Toolbar
        Toolbar.Add(New ToolbarGroup(New String() {"Sourcedialog"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Bold", "Italic", "Underline"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Link", "Unlink"}))
            Return Toolbar
        End Function

        Public Shared Function moderateToolbar() As Toolbar
            Dim Toolbar As New Toolbar
        Toolbar.Add(New ToolbarGroup(New String() {"Sourcedialog"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Format"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Font", "FontSize"}))
            Toolbar.Add(New ToolbarGroup(New String() {"TextColor", "BGColor"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Bold", "Italic", "Underline", "Strike"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Outdent", "Indent"}))
            Toolbar.Add(New ToolbarGroup(New String() {"JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock"}))
            Toolbar.Add(New ToolbarGroup(New String() {"Link", "Unlink", "Anchor"}))
            Return Toolbar
        End Function

    Public Shared Function ParseToolbar(s As String, Optional ByRef t As Toolbar = Nothing) As Toolbar
        If t Is Nothing Then t = New Toolbar
        t.Clear()
        s = s.Replace(" ", "").Replace("""", "")
        'Dim g As new ckEditor.ToolbarGroup()
        For Each grp As String In s.Split(New String() {"],["}, System.StringSplitOptions.RemoveEmptyEntries)
            t.Add(ParseToolbarGroup(grp))
        Next
        Return t
    End Function

    Public Shared Function ParseToolbarGroup(s As String) As ToolbarGroup
        s = s.Replace("]", "").Replace("[", "").Trim(",".Chars(0))
        Return New ToolbarGroup(s.Split(New String() {","}, System.StringSplitOptions.RemoveEmptyEntries))
    End Function

		

#End Region




    End Class