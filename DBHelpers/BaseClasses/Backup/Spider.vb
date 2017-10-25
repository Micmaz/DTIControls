Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

#If DEBUG Then
Public Class Spider
#Else

''' <summary>
''' Used to search a page or cotrol for contents. Spiders all sub controls in most cases.
''' </summary>
''' <remarks></remarks>
    <System.ComponentModel.Description("Used to search a page or cotrol for contents. Spiders all sub controls in most cases."),ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class Spider
#End If

#Region "Spidering Functions"

    ''' <summary>
    ''' Searches a Page and removes all literal controls it finds.
    ''' </summary>
    ''' <param name="pg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Searches a Page and removes all literal controls it finds.")> _
    Public Shared Function spiderRemoveLiterals(ByVal pg As Page) As Integer
        Dim cnt As Integer = 0
        Dim arrayToDelete As New ArrayList
        For Each ctrl As Control In pg.Controls
            Dim ret As Control = spidercontrolforType(ctrl, GetType(LiteralControl))
            If Not ret Is Nothing Then
                arrayToDelete.Add(ret)
                cnt += 1
            End If
        Next
        For Each ctrl As Control In arrayToDelete
            pg.Controls.Remove(ctrl)
        Next
        Return cnt
    End Function

    ''' <summary>
    ''' Searches a Control and removes all literal controls it finds.
    ''' </summary>
    ''' <param name="_ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Searches a Control and removes all literal controls it finds.")> _
        Public Shared Function spiderRemoveLiterals(ByVal _ctrl As Control) As Integer
            Dim cnt As Integer = 0
            Dim arrayToDelete As New ArrayList
            For Each ctrl As Control In _ctrl.Controls
                Dim ret As Control = spidercontrolforType(ctrl, GetType(LiteralControl))
                If Not ret Is Nothing Then
                    arrayToDelete.Add(ret)
                    cnt += 1
                End If
            Next
            For Each ctrl As Control In arrayToDelete
                _ctrl.Controls.Remove(ctrl)
            Next
            Return cnt
        End Function

    ''' <summary>
    ''' Determines weather a control containes a type in it's control list. Does not return if the contained type is a subclass of the type argument
    ''' </summary>
    ''' <param name="_ctrl"></param>
    ''' <param name="_types"></param>
    ''' <param name="ignoreLiterals"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Determines weather a control containes a type in it's control list. Does not return if the contained type is a subclass of the type argument")> _
        Public Shared Function spiderStrictControlSetPresence(ByVal _ctrl As Control, ByVal _types As Type(), Optional ByVal ignoreLiterals As Boolean = True) As Boolean
            Dim typs As New Hashtable
            For Each typ As Type In _types
                typs.Add(typ, typ)
            Next
            Dim found As Boolean = False
            For Each ctrl As Control In _ctrl.Controls
                If Not typs.ContainsKey(ctrl.GetType) Then
                    If Not ignoreLiterals OrElse ctrl.GetType IsNot GetType(LiteralControl) Then
                        found = True
                    End If
                End If
            Next
            Return found
        End Function

    ''' <summary>
    ''' Spiders a page for the first instance of a type and returns the first instance if found. Otherwise it returns nothing.
    ''' </summary>
    ''' <param name="pg">The page to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <returns>a control of type tp</returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Spiders a page for the first instance of a type and returns the first instance if found. Otherwise it returns nothing.")> _
        Public Shared Function spiderPageforType(ByVal pg As Page, ByVal tp As Type) As Control
            For Each ctrl As Control In pg.Controls
                Dim ret As Control = spidercontrolforType(ctrl, tp)
                If Not ret Is Nothing Then
                    Return ret
                End If
            Next
            Return Nothing
        End Function

    ''' <summary>
    '''  Spiders a page for the first instance of a type and returns all instances if found. Otherwise it returns an empty collection.
    ''' </summary>
    ''' <param name="pg">The page to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <param name="includeSubclasses">Determines weather to add subclasses of a the type to the return list.</param>
    ''' <returns>A Collection of objects of type tp</returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Spiders a page for the first instance of a type and returns all instances if found. Otherwise it returns an empty collection.")> _
        Public Shared Function spiderPageforTypeArray(ByVal pg As Page, ByVal tp As Type, Optional ByVal includeSubclasses As Boolean = True) As Collection
            Dim c As New Collection
            For Each ctrl As Control In pg.Controls
                spidercontrolforTypeArray(ctrl, tp, c, includeSubclasses)
            Next
            Return c
        End Function

    ''' <summary>
    '''  Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty collection.
    ''' </summary>
    ''' <param name="topctrl">The Control to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <param name="includeSubclasses">Determines weather to add subclasses of a the type to the return list.</param>
    ''' <returns>A Collection of objects of type tp</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty collection.")> _
    Public Shared Function spidercontrolforTypeArray(ByVal topctrl As Control, ByVal tp As Type, ByRef c As Collection, Optional ByVal includeSubclasses As Boolean = True, Optional ByVal recurse As Boolean = True) As Collection
        If c Is Nothing Then c = New Collection
        For Each subctrl As Control In topctrl.Controls
            If recurse Then spidercontrolforTypeArray(subctrl, tp, c, includeSubclasses)
            If includeSubclasses Then
                If tp.IsAssignableFrom(subctrl.GetType()) Then
                    c.Add(subctrl)
                End If
            Else
                If subctrl.GetType() Is tp Then c.Add(subctrl)
            End If
        Next
        Return c
    End Function

    ''' <summary>
    '''  Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty collection.
    ''' </summary>
    ''' <param name="topctrl">The Control to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <param name="includeSubclasses">Determines weather to add subclasses of a the type to the return list.</param>
    ''' <returns>A Collection of objects of type tp</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty collection.")> _
    Public Shared Function spidercontrolforTypeArray(ByVal topctrl As Control, ByVal tp As Type, Optional ByVal includeSubclasses As Boolean = True, Optional ByVal recurse As Boolean = True) As Collection
        Dim c As New Collection
        For Each subctrl As Control In topctrl.Controls
            If recurse Then spidercontrolforTypeArray(subctrl, tp, c, includeSubclasses)
            If includeSubclasses Then
                If tp.IsAssignableFrom(subctrl.GetType()) Then
                    c.Add(subctrl)
                End If
            Else
                If subctrl.GetType() Is tp Then c.Add(subctrl)
            End If
        Next
        Return c
    End Function

    ''' <summary>
    ''' Spiders a Control for the first instance of a type and returns the first instance if found. Otherwise it returns nothing.
    ''' </summary>
    ''' <param name="ctrl">The Control to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <returns>a control of type tp</returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Spiders a Control for the first instance of a type and returns the first instance if found. Otherwise it returns nothing.")> _
        Public Shared Function spidercontrolforType(ByVal ctrl As Control, ByVal tp As Type) As Control
            If ctrl.GetType() Is tp Then
                Return ctrl
            End If
            For Each subctrl As Control In ctrl.Controls
                Dim ret As Control = spidercontrolforType(subctrl, tp)
                If Not ret Is Nothing Then
                    Return ret
                End If
            Next
            Return Nothing
        End Function

    ''' <summary>
    ''' Searches from the control up until it reaches the page for a specific control. Returns the first istance of tp found going up or nothing if unfound.
    ''' </summary>
    ''' <param name="ctrl">The Control to search.</param>
    ''' <param name="tp">The type that is being searched for.</param>
    ''' <returns>a control of type tp</returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Searches from the control up until it reaches the page for a specific control. Returns the first istance of tp found going up or nothing if unfound.")> _
        Public Shared Function spiderUpforType(ByVal ctrl As Control, ByVal tp As Type) As Control
            If ctrl.GetType() Is tp Then
                Return ctrl
            End If
            If ctrl.Parent Is Nothing Then Return Nothing
            Return spiderUpforType(ctrl.Parent, tp)
            Return Nothing
        End Function

    ''' <summary>
    ''' Spiders a Page for the all instances of a type and returns all instances if found. Otherwise it returns an empty control list.
    ''' </summary>
    ''' <param name="pg"></param>
    ''' <param name="tp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Spiders a Page for the all instances of a type and returns all instances if found. Otherwise it returns an empty control list.")> _
    Public Shared Function spiderPageforAllOfType(ByVal pg As Page, ByVal tp As Type) As Generic.List(Of Control)
        Dim x As New Generic.List(Of Control)
        For Each ctrl As Control In pg.Controls
            x.AddRange(spidercontrolforAllOfType(ctrl, tp))
        Next
        Return x
    End Function

    ''' <summary>
    ''' Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty control list.
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <param name="tp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Spiders a Control for the all instances of a type and returns all instances if found. Otherwise it returns an empty control list.")> _
    Public Shared Function spidercontrolforAllOfType(ByVal ctrl As Control, ByVal tp As Type) As Generic.List(Of Control)
        Dim x As New Generic.List(Of Control)
        If tp.IsAssignableFrom(ctrl.GetType()) Then
            x.Add(ctrl)
        End If
        For Each subctrl As Control In ctrl.Controls
            x.AddRange(spidercontrolforAllOfType(subctrl, tp))
        Next
        Return x
    End Function



#End Region


    End Class
