Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.Web.Caching
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Module Extensions



#Region "Page Level Object Cache"

    <Extension()> _
    Public Function getControlCache(ByRef c As Control, ByVal key As Object) As Object
        Return controlHT(c)(key)
    End Function
    <Extension()> _
    Public Sub setControlCache(ByRef c As Control, ByVal key As Object, ByVal value As Object)
        controlHT(c)(key) = value
    End Sub

    <Extension()> _
    Private Function controlHT(ByRef c As Control) As Hashtable
        If c.Page.Cache(controlKey(c)) Is Nothing Then
            c.Page.Cache.Insert(controlKey(c), New Hashtable, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10))
        End If
        Return c.Page.Cache(controlKey(c))
    End Function

    <Extension()> _
    Private Function pageHT(ByRef c As Control) As Hashtable
        If Web.HttpContext.Current.Items("requestCache") Is Nothing Then
            Web.HttpContext.Current.Items("requestCache") = New Hashtable
        End If
        Return Web.HttpContext.Current.Items("requestCache")
    End Function

    <Extension()> _
    Private Function inCache(ByRef c As Control) As Boolean
        If refViewstate(c)("controlKey") Is Nothing Then Return False
        Return True
    End Function


    <Extension()> _
    Public Function refViewstate(ByRef c As Control) As StateBag
        Return runM(c, "ViewState")
    End Function

    Public Function runM(ByVal o As Object, ByVal methodname As String, ByVal ParamArray args() As Object) As Object
        Dim method As System.Reflection.MethodInfo = o.GetType().GetMethod(methodname, BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.IgnoreCase)
        If Not method Is Nothing Then
            Return method.Invoke(o, args)
        Else
            Dim prop As System.Reflection.PropertyInfo = o.GetType().GetProperty(methodname, BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.IgnoreCase)
            If Not prop Is Nothing Then
                If args.Length = prop.GetIndexParameters().Length Then
                    Return prop.GetValue(o, args)
                Else
                    Dim args2() As Object = New Object() {}
                    If args.Length > 1 Then
                        args.CopyTo(args2, 1)
                    Else
                        args2 = Nothing
                    End If
                    prop.SetValue(o, args(0), args2)
                    Return Nothing
                End If
            End If
        End If
        Return Nothing
    End Function

    <Extension()> _
    Public Function controlKey(ByRef c As Control) As String
        If refViewstate(c)("controlKey") Is Nothing Then
            refViewstate(c)("controlKey") = Guid.NewGuid.ToString
        End If
        Return refViewstate(c)("controlKey")
    End Function

#End Region

#Region "Row setters"

    <Extension()> _
    Public Function getPageTables(ByVal c As Control) As List(Of DataTable)
        Dim dtList As New List(Of DataTable)
        For Each r As DataRow In getPageRows(c)
            If Not dtList.Contains(r.Table) Then dtList.Add(r.Table)
        Next
        Return dtList
    End Function

    <Extension()> _
    Public Function getPageRows(ByVal c As Control) As List(Of DataRow)
        If Not pageHT(c).Contains("Rows") Then
            pageHT(c)("Rows") = New List(Of DataRow)
        End If
        Return pageHT(c)("Rows")
    End Function

    <Extension()> _
    Public Function getPageRowHash(ByVal c As Control) As Dictionary(Of DataRow, List(Of Control))
        If Not pageHT(c).Contains("RowHash") Then
            pageHT(c)("RowHash") = New Dictionary(Of DataRow, List(Of Control))
        End If
        Return pageHT(c)("RowHash")
    End Function

    <Extension()> _
    Public Function getRow(ByVal c As Control) As DataRow
        Return controlHT(c)("row")
    End Function

    <Extension()> _
    Public Function setRow(ByVal c As Control, r As DataRow) As DataRow
        controlHT(c)("row") = r
        If Not getPageRowHash(c).ContainsKey(r) Then getPageRowHash(c)(r) = New List(Of Control)
        If Not getPageRowHash(c)(r).Contains(c) Then getPageRowHash(c)(r).Add(c)
        If Not getPageRows(c).Contains(r) Then getPageRows(c).Add(r)
        Return r
    End Function

    <Extension()> _
    Public Function setAllRowValues(ByVal c As Control, Optional ByVal showerrors As Boolean = False) As List(Of ErrorSet)
        Dim errorList As New List(Of ErrorSet)
        For Each r As DataRow In getPageRowHash(c).Keys
            For Each ctrl As Control In getPageRowHash(c)(r)
                If (Not isLabelized(c)) Then
                    If ctrl.Controls.Count > 0 Then
						errorList.AddRange(setRowValues(ctrl, r, showerrors))
					Else
						errorList.AddRange(setRowValue(ctrl.Parent, ctrl, r))
					End If
                End If
            Next
        Next
        If showerrors Then showErrorList(c, errorList, True)
        Return errorList
    End Function

    <Extension()> _
    Public Function saveAllRows(ByVal c As Control, Optional ByVal showerrors As Boolean = False, Optional connection As IDbConnection = Nothing) As List(Of ErrorSet)
        Dim errorList As List(Of ErrorSet) = setAllRowValues(c, False)

        Dim h As BaseClasses.BaseHelper = Nothing
        If connection IsNot Nothing Then h = BaseClasses.DataBase.createHelper(connection)
        If h Is Nothing Then h = BaseClasses.DataBase.getHelper()

        For Each t As DataTable In getPageTables(c)
            h.Update(t)
            'Try
            '    h.Update(t)
            'Catch ex As Exception
            '    l.Add(New ErrorSet(ex, c, "Table Save"))
            'End Try
        Next
        If showerrors Then showErrorList(c, errorList, True)
        Return errorList
    End Function

#End Region

#Region "Generic Data setters"

#Region "Value Format helpers"

	Public Function getRowValue(row As DataRow, colName As String) As Object
		If row(colName) Is DBNull.Value Then Return Nothing
		Return row(colName)
	End Function

	Public Function getValueString(ByVal row As DataRow, ByVal ParamArray columns As String()) As String
        Return getValueStringDelimited(row, " ", columns)
    End Function

    Public Function getValueStringDelimited(ByVal row As DataRow, ByVal dilimiter As String, ByVal ParamArray columns As String()) As String
        Dim ret As String = ""
        If row Is Nothing OrElse columns Is Nothing Then Return ""
        For Each col As String In columns
            If row(col) IsNot DBNull.Value Then
                ret &= row(col) & dilimiter
            End If
        Next
        ret = ret.Trim
        Return ret
    End Function


    Public Function getFormattedString(ByVal row As DataRow, ByVal format As String, ByVal ParamArray columns As String()) As String
        Dim fmtlist As New ArrayList
        For Each col As String In columns
            fmtlist.Add(getValueString(row, col))
        Next
        Return String.Format(format, fmtlist.ToArray)
    End Function

    Public Function FindControlRecursive(ByVal ctrl As Control, ByVal id As String) As Control
        If ctrl.ID = id Then Return ctrl
        For Each subctrl As Control In ctrl.Controls
            Dim found As Control = FindControlRecursive(subctrl, id)
            If found IsNot Nothing Then Return found
        Next
        Return Nothing
    End Function


#End Region

    Public Class ErrorSet
        Public e As Exception
        Public c As Control
        Public columnName As String

        Public Sub New(ByVal e As Exception, ByVal c As Control, ByVal columnName As String)
            Me.e = e
            Me.c = c
            Me.columnName = columnName
        End Sub
    End Class

    Private Function setRowValueHelper(r As DataRow, colname As String, value As Object) As Boolean
        Try
        If (r(colname) Is DBNull.Value And value IsNot DBNull.Value) OrElse _
            (r(colname) IsNot DBNull.Value And value Is DBNull.Value) OrElse
            (Not r(colname) = value) Then
            r(colname) = value
            Return True
        End If
        Catch ex As Exception
            Try
                r(colname) = value
                'errors.Add(New ErrorSet(ex, ctrl, setControls(c)(ctrlid)))
                Return True
            Catch ex1 As Exception

            End Try
        End Try
        Return False
    End Function

    Private Function setRowValue(c As Control, ctrl As Control, r As DataRow) As List(Of ErrorSet)
        Dim errors As New List(Of ErrorSet)
        If ctrl Is Nothing Then Return errors
		Dim ctrlid As String = ctrl.ID
		Dim colname As String = setControls(c)(ctrlid)
		If Not ctrl Is Nothing AndAlso Not String.IsNullOrEmpty(colname) Then
			Try
				If r.Table.Columns.Contains(colname) Then
					If GetType(JqueryUIControls.maskedTextbox).IsAssignableFrom(ctrl.GetType()) Then
						If r(colname) Is DBNull.Value AndAlso CType(ctrl, TextBox).Text = "" Then
						Else
							setRowValueHelper(r, (colname), CType(ctrl, JqueryUIControls.maskedTextbox).unMaskedText)
						End If
					ElseIf GetType(JqueryUIControls.Autocomplete).IsAssignableFrom(ctrl.GetType()) Then
						Dim val As String = CType(ctrl, JqueryUIControls.Autocomplete).Value
						If val = "" Then
							val = CType(ctrl, TextBox).Text
						End If
						If r(colname) Is DBNull.Value AndAlso val = "" Then
						Else
							setRowValueHelper(r, (colname), val)
						End If
					ElseIf GetType(TextBox).IsAssignableFrom(ctrl.GetType()) Then
						If r(colname) Is DBNull.Value AndAlso CType(ctrl, TextBox).Text = "" Then
						Else
							setRowValueHelper(r, (colname), CType(ctrl, TextBox).Text)
						End If
					ElseIf GetType(CheckBox).IsAssignableFrom(ctrl.GetType()) Then
						If r(colname) Is DBNull.Value AndAlso CType(ctrl, CheckBox).Checked = False Then
							setRowValueHelper(r, (colname), 0)
						Else
							If CType(ctrl, CheckBox).Checked Then
								setRowValueHelper(r, (colname), 1)
							Else
								setRowValueHelper(r, (colname), 0)
							End If
						End If
					ElseIf GetType(RadioButton).IsAssignableFrom(ctrl.GetType()) Then
						'Search for all radioButtons with the same group name
						Dim rbIn As RadioButton = ctrl
						Dim rbList = getMatchingRadioButtons(c, rbIn)
						Dim found As Boolean = False
						'If the columname starts after the 3rd character use the 3rd character to indicate T/F (ex rbFisAdmin would set the isadmin col to false if checked)
						Dim setBooleanVal As Boolean = ctrlid.IndexOf(colname) > 2
						For Each rb In rbList
							If rb.Checked Then
								If setBooleanVal Then
									Dim thirdChar As Char = Char.ToUpper(rb.ID.Chars(2))
									'of the found radio buttons, look for a T/F or Y/N in the second column
									If thirdChar = "N" Or thirdChar = "F" Then
										setRowValueHelper(r, (colname), 0)
									End If
									If thirdChar = "Y" Or thirdChar = "T" Then
										setRowValueHelper(r, (colname), 1)
									End If
									found = True
								End If
							Else
								setRowValueHelper(r, (colname), rb.Text)
							End If
						Next
						If Not found Then
							setRowValueHelper(r, (colname), DBNull.Value)
						End If

					ElseIf GetType(DropDownList).IsAssignableFrom(ctrl.GetType()) Then
						Dim dd As DropDownList = CType(ctrl, DropDownList)
						If dd.SelectedValue = "NULL" Then
							setRowValueHelper(r, (colname), DBNull.Value)
						Else
							setRowValueHelper(r, (colname), dd.SelectedValue)
						End If
					End If
				End If
			Catch ex As Exception
				Try
					errors.Add(New ErrorSet(ex, ctrl, colname))
				Catch ex1 As Exception

				End Try
			End Try
		End If
		Return errors
    End Function

    <Extension()> _
    Public Function setRowValues(ByVal c As Control, Optional ByVal r As DataRow = Nothing, Optional ByVal showerrors As Boolean = False) As List(Of ErrorSet)
        If r Is Nothing Then r = getRow(c) Else setRow(c, r)
        Dim errors As New List(Of ErrorSet)
        If (setControls(c).Count = 0) Then
            autoBind(c, r, False)
        End If
        'It's labelized so don't set any values from the row.
        If isLabelized(c) Then Return errors
        For Each ctrlid As String In setControls(c).Keys
            Dim ctrl As Control = FindControlRecursive(c, ctrlid)
            If (ctrl Is Nothing) Then
                errors.Add(New ErrorSet(New Exception("Control with id:" & ctrlid & " was not found. Make sure controls retain their ID from postback in any dynamic code."), c, "NA"))
            End If
            errors.AddRange(setRowValue(c, ctrl, r))
            'If Not ctrl Is Nothing Then
            '    Try
            '        If r.Table.Columns.Contains(setControls(c)(ctrlid)) Then
            '            If GetType(JqueryUIControls.maskedTextbox).IsAssignableFrom(ctrl.GetType()) Then
            '                If r(setControls(c)(ctrlid)) Is DBNull.Value AndAlso CType(ctrl, TextBox).Text = "" Then
            '                Else
            '                    r(setControls(c)(ctrlid)) = CType(ctrl, JqueryUIControls.maskedTextbox).unMaskedText
            '                End If
            '            ElseIf GetType(TextBox).IsAssignableFrom(ctrl.GetType()) Then
            '                If r(setControls(c)(ctrlid)) Is DBNull.Value AndAlso CType(ctrl, TextBox).Text = "" Then
            '                Else
            '                    r(setControls(c)(ctrlid)) = CType(ctrl, TextBox).Text
            '                End If
            '            ElseIf GetType(CheckBox).IsAssignableFrom(ctrl.GetType()) Then
            '                If r(setControls(c)(ctrlid)) Is DBNull.Value AndAlso CType(ctrl, CheckBox).Checked = False Then
            '                    r(setControls(c)(ctrlid)) = 0
            '                Else
            '                    If CType(ctrl, CheckBox).Checked Then
            '                        r(setControls(c)(ctrlid)) = 1
            '                    Else
            '                        r(setControls(c)(ctrlid)) = 0
            '                    End If
            '                End If
            '            ElseIf GetType(DropDownList).IsAssignableFrom(ctrl.GetType()) Then
            '                Dim dd As DropDownList = CType(ctrl, DropDownList)
            '                If dd.SelectedValue = "NULL" Then
            '                    r(setControls(c)(ctrlid)) = DBNull.Value
            '                Else
            '                    r(setControls(c)(ctrlid)) = dd.SelectedValue
            '                End If
            '            End If
            '        End If
            '    Catch ex As Exception
            '        Try
            '            errors.Add(New ErrorSet(ex, ctrl, setControls(c)(ctrlid)))
            '        Catch ex1 As Exception

            '        End Try
            '    End Try
            'End If
        Next
        If showerrors Then showErrorList(c, errors, True)
        Return errors
    End Function

    <Extension()> _
    Public Function showErrorList(ByVal c As Control, errors As List(Of ErrorSet), Optional addToControl As Boolean = False, Optional appendFormat As String = "<b>Control:</b> {0}  <b>Col:</b> {1} <br/><b>Error: </b> {2}<br/>") As JqueryUIControls.InfoDiv
        Dim foundErrors As Boolean = False
        Dim errordiv As New JqueryUIControls.InfoDiv
        errordiv.isError = True
        For Each err As ErrorSet In errors
            foundErrors = True
            errordiv.Controls.Add(New LiteralControl(String.Format(appendFormat, err.c.ID, err.columnName, err.e.Message)))
        Next
        If foundErrors AndAlso addToControl Then
            c.Controls.Add(errordiv)
        End If
        Return errordiv
    End Function



    <Extension()> _
    Public Function autoBind(ByVal c As Control, Optional row As DataRow = Nothing, Optional setValues As Boolean = True, Optional doNothingIfPostback As Boolean = True) As List(Of ErrorSet)
        If row Is Nothing Then row = getRow(c) Else setRow(c, row)
        If doNothingIfPostback AndAlso c.Page.IsPostBack Then
            setValues = False
        End If
        Dim errors As New List(Of ErrorSet)
        Dim controls As New List(Of Control)
        Dim cols As DataColumnCollection = row.Table.Columns


        For Each tb As JqueryUIControls.maskedTextbox In GetControlList(Of JqueryUIControls.maskedTextbox)(c.Controls)
            Dim colname As String = canbindCtrl(c, tb, row, errors)
            If (Not colname Is Nothing) Then
                If setValues Then
                    setText(c, tb, row, colname)
                Else
                    setControls(c)(tb.ID) = colname
                End If
            End If
        Next
        For Each ac As JqueryUIControls.Autocomplete In GetControlList(Of JqueryUIControls.Autocomplete)(c.Controls)
            Dim colname As String = canbindCtrl(c, ac, row, errors)
            If (Not colname Is Nothing) Then
                If setValues Then
                    setText(c, ac, row, colname)
                Else
                    setControls(c)(ac.ID) = colname
                End If
            End If
        Next
        For Each tb As TextBox In GetControlList(Of TextBox)(c.Controls)
            Dim colname As String = canbindCtrl(c, tb, row, errors)
            If (Not colname Is Nothing) Then
                If setValues Then
                    setText(c, tb, row, colname)
                Else
                    setControls(c)(tb.ID) = colname
                End If
            End If
        Next
		For Each cb As CheckBox In GetControlList(Of CheckBox)(c.Controls)
			Dim colname As String = canbindCtrl(c, cb, row, errors)
			If (Not colname Is Nothing) Then
				If setValues Then
					setText(c, cb, row, colname)
				Else
					setControls(c)(cb.ID) = colname
				End If
			End If
		Next
		For Each rb As RadioButton In GetControlList(Of RadioButton)(c.Controls)
			Dim colname As String = canbindCtrl(c, rb, row, errors)
			If (Not colname Is Nothing) Then
				If setValues Then
					setText(c, rb, row, colname)
				Else
					setControls(c)(rb.ID) = colname
				End If
			End If
		Next
		For Each ddl As DropDownList In GetControlList(Of DropDownList)(c.Controls)
				Dim colname As String = canbindCtrl(c, ddl, row, errors)
				If (Not colname Is Nothing) Then
					If setValues Then
						setText(c, ddl, row, colname)
					Else
						setControls(c)(ddl.ID) = colname
					End If
				End If
			Next
			For Each lbl As Label In GetControlList(Of Label)(c.Controls)
            Dim colname As String = canbindCtrl(c, lbl, row, errors)
            If (Not colname Is Nothing) Then
                If setValues Then
                    setText(c, lbl, row, colname)
                Else
                    setControls(c)(lbl.ID) = colname
                End If
            End If
        Next

        Return errors
    End Function

    Private Function canbindCtrl(mainctrl As Control, ctrl As Control, row As DataRow, errors As List(Of ErrorSet)) As String
        If Not setControls(mainctrl).ContainsKey(ctrl.ID) Then
            Dim colname As String = ctrl.ID
            If (Not row.Table.Columns.Contains(colname)) Then
                If colname.Length > 2 Then
                    colname = colname.Substring(2)
                End If
            End If
            If (Not row.Table.Columns.Contains(colname)) Then
                If colname.Length > 1 Then
                    colname = colname.Substring(1)
                End If
            End If
            If (Not row.Table.Columns.Contains(colname)) Then
                errors.Add(New ErrorSet(New Exception("Could not find col match for:" & ctrl.ID), ctrl, ""))
                Return Nothing
            Else
                Return colname
            End If
        End If
        Return Nothing
    End Function

    Private Function GetControlList(Of T As Control)(controlCollection As ControlCollection, Optional resultCollection As List(Of T) = Nothing)
        If resultCollection Is Nothing Then resultCollection = New List(Of T)
        For Each control As Control In controlCollection
            If GetType(T).IsAssignableFrom(control.GetType()) Then
                resultCollection.Add(DirectCast(control, T))
            End If
            If control.HasControls() Then
                GetControlList(control.Controls, resultCollection)
            End If
        Next
        Return resultCollection
    End Function


    <Extension()> _
    Private Function setControls(ByVal c As Control) As Hashtable
        If getControlCache(c, "setControls") Is Nothing Then
            setControlCache(c, "setControls", New Hashtable)
        End If
        Return getControlCache(c, "setControls")
    End Function

    <Extension()> _
    Public Sub setText(ByVal c As Control, row As DataRow, ByVal ParamArray columns As String())
        setRow(c, row)
        If Not c.Page.IsPostBack Then
            setText(c.Parent, c, row, columns)
        Else
            If columns.Count = 1 Then
                If c.ID Is Nothing Then c.ID = c.ClientID
                setControls(c.Parent)(c.ID) = columns(0)
            End If
        End If
    End Sub

    <Extension()> _
    Public Sub setText(ByVal c As Control, item As Control, row As DataRow, ByVal ParamArray columns As String())
		If GetType(JqueryUIControls.Autocomplete).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, JqueryUIControls.Autocomplete), row, columns)
		ElseIf GetType(TextBox).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, TextBox), row, columns)
		ElseIf GetType(Label).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, Label), row, columns)
		ElseIf GetType(DropDownList).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, DropDownList), row, columns)
		ElseIf GetType(CheckBox).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, CheckBox), row, columns)
		ElseIf GetType(RadioButton).IsAssignableFrom(c.GetType()) Then
			setText(c, CType(item, RadioButton), row, columns)
		Else
            Try
                runM(item, "Text", New Object() {getValueString(row, columns)})
            Catch ex As Exception

            End Try
            Try
                runM(item, "Value", New Object() {getValueString(row, columns)})
            Catch ex As Exception

            End Try
        End If

    End Sub

    <Extension()> _
    Public Sub setText(ByVal c As Control, ByVal ac As JqueryUIControls.Autocomplete, ByVal row As DataRow, ByVal ParamArray columns As String())
        If columns.Length = 0 Then
            'columns = New String() {ac.ID}
            columns = New String() {canbindCtrl(c, ac, row, New List(Of ErrorSet))}
        End If
        ac.Text = getValueString(row, columns)
        If columns.Length = 1 Then
            If row.Table.Columns(columns(0)).MaxLength > 0 Then
                ac.MaxLength = row.Table.Columns(columns(0)).MaxLength
            End If
            setControls(c)(ac.ID) = columns(0)
            setAutocomplete(c, ac, row, columns(0))
        End If
    End Sub

    <Extension()> _
    Public Sub setText(ByVal c As Control, ByVal lbl As Label, ByVal row As DataRow, ByVal ParamArray columns As String())
        If columns.Length = 0 Then
            'columns = New String() {lbl.ID}
            columns = New String() {canbindCtrl(c, lbl, row, New List(Of ErrorSet))}
        End If
        lbl.Text = getValueString(row, columns)
    End Sub

    <Extension()> _
    Public Sub setText(ByVal c As Control, ByVal tb As TextBox, ByVal row As DataRow, ByVal ParamArray columns As String())
        If columns.Length = 0 Then
            'columns = New String() {tb.ID}
            columns = New String() {canbindCtrl(c, tb, row, New List(Of ErrorSet))}
        End If
        tb.Text = getValueString(row, columns)
        If columns.Length = 1 Then
            If row.Table.Columns(columns(0)).MaxLength > 0 Then
                tb.MaxLength = row.Table.Columns(columns(0)).MaxLength
            End If
            setControls(c)(tb.ID) = columns(0)
        End If
    End Sub

    <Extension()> _
    Public Sub setText(ByVal c As Control, ByVal cb As CheckBox, ByVal row As DataRow, ByVal ParamArray columns As String())
        If columns.Length = 0 Then
            'columns = New String() {cb.ID}
            columns = New String() {canbindCtrl(c, cb, row, New List(Of ErrorSet))}
        End If
        Dim val As String = getValueString(row, columns)
        Dim b As Boolean = False
        If Not Boolean.TryParse(val, b) Then
            If val.Length > 0 Then
                b = True
                If val(0).ToString.ToLower = "n" OrElse val(0).ToString.ToLower = "f" OrElse val(0).ToString.ToLower = "0" Then
                    b = False
                End If
            End If
        End If

        cb.Checked = b
        If columns.Length = 1 Then
            setControls(c)(cb.ID) = columns(0)
        End If
    End Sub

	<Extension()>
	Public Sub setText(ByVal c As Control, ByVal rb As RadioButton, ByVal row As DataRow, ByVal ParamArray columns As String())
		If columns.Length = 0 Then
			'columns = New String() {cb.ID}
			columns = New String() {canbindCtrl(c, rb, row, New List(Of ErrorSet))}
		End If
		Dim val As String = getValueString(row, columns)
		If columns.Length = 1 Then
			setControls(c)(rb.ID) = columns(0)
			Dim colname = columns(0)
			Dim setBooleanVal As Boolean = rb.ID.IndexOf(colname) > 2
			Dim b As Boolean = False
			'Parse Boolean Val from the database
			If Not Boolean.TryParse(val, b) Then
				If val.Length > 0 Then
					b = True
					If val(0).ToString.ToLower = "n" OrElse val(0).ToString.ToLower = "f" OrElse val(0).ToString.ToLower = "0" Then
						b = False
					End If
				End If
			End If

			Dim rbList = getMatchingRadioButtons(c, rb)

			For Each rb1 As RadioButton In GetControlList(Of RadioButton)(c.Controls)
				rb1.Checked = False
				If setBooleanVal Then
					Dim thirdChar As Char = Char.ToUpper(rb1.ID.Chars(2))
					'of the found radio buttons, look for a T/F or Y/N in the second column
					If Not b AndAlso (thirdChar = "N" Or thirdChar = "F") Then
						rb1.Checked = True
					End If
					If b AndAlso (thirdChar = "Y" Or thirdChar = "T") Then
						rb1.Checked = True
					End If
				Else
					If rb1.Text = val Then rb1.Checked = True
				End If
			Next




		End If

	End Sub

	Private Function getMatchingRadioButtons(ByVal c As Control, rb1 As RadioButton) As List(Of RadioButton)
		Dim rbList As New List(Of RadioButton)
		For Each rb As RadioButton In GetControlList(Of RadioButton)(c.Controls)
			If rb.GroupName = rb1.GroupName Then rbList.Add(rb)
		Next
		Return rbList
	End Function

	''' <summary>
	''' If the value is not in the item list it is added automatically to the top of the list. The string "NULL" will set the db value to null.
	''' </summary>
	''' <param name="c"></param>
	''' <param name="dd"></param>
	''' <param name="row"></param>
	''' <param name="columns"></param>
	''' <remarks></remarks>
    <System.ComponentModel.Description("If the value is not in the item list it is added automatically to the top of the list. The string ""NULL"" will set the db value to null."), Extension()> _
    Public Sub setText(ByVal c As Control, ByVal dd As DropDownList, ByVal row As DataRow, ByVal ParamArray columns As String())
        Dim col1 As String = Nothing
        If columns.Length > 0 Then col1 = columns(0)
        If col1 Is Nothing Then col1 = canbindCtrl(c, dd, row, New List(Of ErrorSet))
        setDDRow(c, dd, Nothing, Nothing, Nothing, row, col1, "NULL", True)
    End Sub

    <Extension()> _
    Public Sub setDDRow(ByVal c As Control, ByVal dd As DropDownList, ByVal dt As DataTable, ByVal displaycolumn As String, Optional ByVal valueColumn As String = Nothing, Optional ByVal SourceRow As DataRow = Nothing, Optional ByVal sourceColumn As String = Nothing, Optional ByVal NullValue As String = "NULL", Optional addIfMissing As Boolean = False)
        If valueColumn Is Nothing Then valueColumn = displaycolumn
        If dt IsNot Nothing Then
            For Each row As DataRow In dt.Rows
                Dim display As String = ""
                Dim value As String = NullValue
                If Not row(displaycolumn) Is DBNull.Value Then display = row(displaycolumn)
                If Not row(valueColumn) Is DBNull.Value Then value = row(valueColumn)
                dd.Items.Add(New ListItem(display, value))
            Next
        End If
        If SourceRow IsNot Nothing AndAlso Not sourceColumn Is Nothing Then

            Dim found As Boolean = False
            Dim value As String

            If SourceRow(sourceColumn) Is DBNull.Value Then
                value = NullValue
            Else
                value = SourceRow(sourceColumn)
            End If

            If addIfMissing Then
                For Each li As ListItem In dd.Items
                    li.Selected = False
                Next
                For Each li As ListItem In dd.Items
                    If li.Value = value Then
                        li.Selected = True
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    dd.Items.Insert(0, New ListItem(getValueString(SourceRow, sourceColumn), value))
                    dd.Items(0).Selected = True
                End If
            Else
                dd.SelectedValue = value
            End If

            setControls(c)(dd.ID) = sourceColumn
            'setControls(c).Add(dd.ID, sourceColumn)
        End If
    End Sub

    <Extension()> _
    Public Sub setDDRow(ByVal c As Control, ByVal dd As Label, ByVal dt As DataTable, ByVal displaycolumn As String, Optional ByVal valueColumn As String = Nothing, Optional ByVal SourceRow As DataRow = Nothing, Optional ByVal sourceColumn As String = Nothing, Optional ByVal NullValue As String = "NULL")
        setDDRow(c, dd.Text, dt, displaycolumn, valueColumn, SourceRow, sourceColumn, NullValue)
    End Sub

    <Extension()> _
    Public Sub setDDRow(ByVal c As Control, ByRef text As String, ByVal dt As DataTable, ByVal displaycolumn As String, Optional ByVal valueColumn As String = Nothing, Optional ByVal SourceRow As DataRow = Nothing, Optional ByVal sourceColumn As String = Nothing, Optional ByVal NullValue As String = "NULL")
        If valueColumn Is Nothing Then valueColumn = displaycolumn
        If dt IsNot Nothing Then
            For Each row As DataRow In dt.Rows
                Dim display As String = ""
                Dim value As String = NullValue
                If Not row(displaycolumn) Is DBNull.Value Then display = row(displaycolumn)
                If Not row(valueColumn) Is DBNull.Value Then value = row(valueColumn)
                If (SourceRow(sourceColumn) Is DBNull.Value AndAlso row(valueColumn) Is DBNull.Value) OrElse _
                  SourceRow(sourceColumn).ToString = row(valueColumn).ToString Then
                    text = display
                    Exit For
                End If
            Next
        Else
            text = getValueString(SourceRow, sourceColumn)
        End If
    End Sub

    <Extension()> _
    Public Sub setDD(ByVal c As Control, ByVal dd As DropDownList, ByVal dt As DataTable, ByVal displaycolumn As String, Optional ByVal valueColumn As String = Nothing, Optional ByVal selectedVal As Object = Nothing)
		If valueColumn Is Nothing Then valueColumn = displaycolumn
		Dim val = ""
		Dim display = ""
		For Each row As DataRow In dt.Rows
			dd.Items.Add(New ListItem(getRowValue(row, displaycolumn), getRowValue(row, valueColumn)))
		Next
		If selectedVal IsNot Nothing Then
            Try
                dd.SelectedValue = selectedVal
            Catch ex As Exception

            End Try
        End If
    End Sub

#Region "Setup Autocomplete on column"

    <Extension()> _
    Public Sub setAutocomplete(ByVal c As Control, ByVal ac As JqueryUIControls.Autocomplete, ByVal row As DataRow, ByVal col As String, Optional ByVal numberReturned As Integer = 20, Optional ByVal searchParmFormat As String = "{0}%")
        If ac.tableName is nothing orelse ac.tableName = "" Then
            ac.setDistinctAutocomplete(row.Table.TableName, col)
        End If
    End Sub

#End Region

#End Region

#Region "Application Specific Parsers"

    'Public Function generateTable(ByVal ParamArray columns As String()) As DataTable

    'End Function

    'Public Function generateTable(colNames As String(), ByVal ParamArray values As String()) As DataTable
    '    Dim dt As New DataTable
    '    For Each col As String In colNames
    '        dt.Columns.Add(col)
    '    Next
    '    For i As Integer = 0 To values.Length - 1

    '    Next

    'End Function

    Public Function getDateString(ByVal aDate As Date, Optional ByVal format As String = "MM/dd/yyyy hh:MM tt", Optional ByVal minvalueReturnString As String = "") As String
        If aDate = Nothing Then aDate = Date.MinValue
        If aDate = Date.MinValue Then
            Return minvalueReturnString
        End If
        Return aDate.ToString(format)
    End Function

    Public Sub setMultiColumnText(ByVal value As String, ByVal r As DataRow, ByVal ParamArray cols() As String)
        If cols.Length = 0 Then Return
        Dim maxLength As Integer = r.Table.Columns(cols(0)).MaxLength

        For Each col As String In cols
            If value.Length > maxLength Then
                r(col) = value.Substring(0, maxLength)
                value = value.Substring(maxLength)
            Else
                r(col) = value
                value = ""
            End If
        Next
    End Sub

    Public Sub setNumber(ByVal value As String, ByVal r As DataRow, ByVal col As String)
        Dim v As Integer
        If Integer.TryParse(value, v) Then
            r(col) = v
        Else
            r(col) = DBNull.Value
        End If
    End Sub

    Public Function zeroPad(ByVal value As Integer, ByVal length As Integer) As String
        Dim retstr As String = value
        While retstr.Length < length
            retstr = "0" & retstr
        End While
        Return retstr
    End Function

#End Region

#Region "Labelize"

    Private Sub replaceControl(ByVal ctrl As Control, ByVal reptext As String)
        Dim i As Integer = ctrl.Parent.Controls.IndexOf(ctrl)
        Dim l As New LiteralControl
        l.Text = reptext
        ctrl.Visible = False
		Try
			ctrl.Parent.Controls.AddAt(i, l)
		Catch ex As Exception
			Throw ex
		End Try
    End Sub

    <Extension()> _
    Public Sub labelize(ByVal ctrl As Control, Optional ByVal defaultFormat As String = "<b>{0}</b>")
        If ctrl.Visible Then
            setControlCache(ctrl, "labelized", True)
			If GetType(TextBox).IsAssignableFrom(ctrl.GetType()) Then
				Dim t As TextBox = ctrl
				replaceControl(t, String.Format(defaultFormat, t.Text))
			ElseIf GetType(DropDownList).IsAssignableFrom(ctrl.GetType()) Then
				Dim dd As DropDownList = CType(ctrl, DropDownList)
				If dd.SelectedValue = "NULL" OrElse dd.SelectedValue Is Nothing Then
					replaceControl(dd, String.Format(defaultFormat, ""))
				Else
					If dd.SelectedItem IsNot Nothing Then
						replaceControl(dd, String.Format(defaultFormat, dd.SelectedItem.Text))
					Else
						replaceControl(dd, String.Format(defaultFormat, ""))
					End If
				End If
			ElseIf GetType(Button).IsAssignableFrom(ctrl.GetType()) Then
				replaceControl(ctrl, "")
			ElseIf GetType(RadioButton).IsAssignableFrom(ctrl.GetType()) Then
				Dim rbIn As RadioButton = ctrl
				If rbIn.Checked Then
					If String.IsNullOrEmpty(rbIn.Text) Then
						replaceControl(ctrl, "X")
					Else
						replaceControl(ctrl, rbIn.Text)
					End If
				Else
					replaceControl(ctrl, "")
				End If
			ElseIf GetType(CheckBox).IsAssignableFrom(ctrl.GetType()) Then
				Dim cb As CheckBox = ctrl
                If cb.Checked Then
                    replaceControl(ctrl, "True")
                Else
                    replaceControl(ctrl, "False")
                End If
                replaceControl(ctrl, "")
            Else
                Dim i As Integer = 0
                While i < ctrl.Controls.Count
                    labelize(ctrl.Controls(i))
                    i += 1
                End While
                'For Each subctrl As Control In ctrl.Controls
                '    labelize(subctrl)
                'Next
            End If
        End If
    End Sub

    <Extension()> _
    Public Function isLabelized(ByVal ctrl As Control) As Boolean
        Return getControlCache(ctrl, "labelized")
    End Function


#End Region

End Module