Imports System.IO
Imports Microsoft.VisualBasic
Imports System.text

''' <summary>
''' Compresses javascript files for faster client download.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class JsMinimizer
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class JsMinimizer
#End If
        Const EOF As Integer = -1
        Dim theA As Integer
        Dim theB As Integer
        Dim theLookahead As Integer = EOF
        Dim strbldr As New StringBuilder

        Public Function Minify(ByRef src As System.IO.Stream) As String
            Using sr As New StreamReader(src)
                jsMin(sr)
            End Using
            Return strbldr.ToString
        End Function

        Public Shared Function SMinify(ByRef src As System.IO.Stream) As String
            Dim jsm As New JsMinimizer
            Return jsm.Minify(src)
        End Function

        Private Sub jsMin(ByRef sr As StreamReader)
            theA = AscW(ControlChars.Lf)
            action(3, sr)
            Do While theA <> EOF
                Select Case theA
                    Case AscW(" "c)
                        If isAlphanum(theB) Then
                            action(1, sr)
                        Else
                            action(2, sr)
                        End If
                        Exit Select
                    Case AscW(ControlChars.Lf)
                        Select Case theB
                            Case AscW("{"c), AscW("["c), AscW("("c), AscW("+"c), AscW("-"c)
                                action(1, sr)
                                Exit Select
                            Case AscW(" "c)
                                action(3, sr)
                                Exit Select
                            Case Else
                                If isAlphanum(theB) Then
                                    action(1, sr)
                                Else
                                    action(2, sr)
                                End If
                                Exit Select
                        End Select
                        Exit Select
                    Case Else
                        Select Case theB
                            Case AscW(" "c)
                                If isAlphanum(theA) Then
                                    action(1, sr)
                                    Exit Select
                                End If
                                action(3, sr)
                                Exit Select
                            Case AscW(ControlChars.Lf)
                                Select Case theA
                                    Case AscW("}"c), AscW("]"c), AscW(")"c), AscW("+"c), AscW("-"c), AscW(""""c), AscW("'"c)
                                        action(1, sr)
                                        Exit Select
                                    Case Else
                                        If isAlphanum(theA) Then
                                            action(1, sr)
                                        Else
                                            action(3, sr)
                                        End If
                                        Exit Select
                                End Select
                                Exit Select
                            Case Else
                                action(1, sr)
                                Exit Select
                        End Select
                        Exit Select
                End Select
            Loop
        End Sub

        'action -- do something! What you do is determined by the argument:
        '   1   Output A. Copy B to A. Get the next B.
        '   2   Copy B to A. Get the next B. (Delete A).
        '   3   Get the next B. (Delete B).
        'action treats a string as a single character. Wow!
        'action recognizes a regular expression if it is preceded by ( or , or =.
        Private Sub action(ByVal d As Integer, ByRef sr As StreamReader)
            If d <= 1 Then
                put(theA)
            End If
            If d <= 2 Then
                theA = theB
                If theA = AscW("'"c) OrElse theA = AscW(""""c) Then
                    Do
                        put(theA)
                        theA = [get](sr)
                        If theA = theB Then
                            Exit Do
                        End If
                        If theA <= AscW(ControlChars.Lf) Then
                            Throw New Exception(String.Format("Error: JSMIN unterminated string literal: {0}" & vbLf, theA))
                        End If
                        If theA = AscW("\"c) Then
                            put(theA)
                            theA = [get](sr)
                        End If
                    Loop
                End If
            End If
            If d <= 3 Then
                theB = [next](sr)
                If theB = AscW("/"c) AndAlso (theA = AscW("("c) OrElse _
                 theA = AscW(","c) OrElse theA = AscW("="c) OrElse theA = AscW("["c) _
                 OrElse theA = AscW("!"c) OrElse AscW(theA = AscW(":"c)) OrElse theA = AscW("&"c) _
                 OrElse theA = AscW("|"c) OrElse theA = AscW("?"c) OrElse theA = AscW("{"c) _
                 OrElse theA = AscW("}"c) OrElse theA = AscW(";"c) OrElse _
                 theA = AscW(ControlChars.Lf)) Then
                    put(theA)
                    put(theB)
                    Do
                        theA = [get](sr)
                        If theA = AscW("/"c) Then
                            Exit Do
                        ElseIf theA = AscW("\"c) Then
                            put(theA)
                            theA = [get](sr)
                        ElseIf theA <= AscW(ControlChars.Lf) Then
                            Throw New Exception(String.Format("Error: JSMIN unterminated Regular Expression literal : {0}." & vbLf, theA))
                        End If
                        put(theA)
                    Loop
                    theB = [next](sr)
                End If
            End If
        End Sub

        'next -- get the next character, excluding comments. peek() is used to see
        '   if a '/' is followed by a '/' or '*'.
        Private Function [next](ByRef sr As StreamReader) As Integer
            Dim c As Integer = [get](sr)
            If c = AscW("/"c) Then
                Select Case peek(sr)
                    Case AscW("/"c)
                        Do
                            c = [get](sr)
                            If c <= AscW(ControlChars.Lf) Then
                                Return c
                            End If
                        Loop
                    Case AscW("*"c)
                        [get](sr)
                        Do
                            Select Case [get](sr)
                                Case AscW("*"c)
                                    If peek(sr) = AscW("/"c) Then
                                        [get](sr)
                                        Return AscW(" "c)
                                    End If
                                    Exit Select
                                Case EOF
                                    Throw New Exception("Error: JSMIN Unterminated comment." & vbLf)
                            End Select
                        Loop
                    Case Else
                        Return c
                End Select
            End If
            Return c
        End Function

        'peek -- get the next character without getting it.
        Private Function peek(ByRef sr As StreamReader) As Integer
            theLookahead = [get](sr)
            Return theLookahead
        End Function

        '   get -- return the next character from stdin. Watch out for lookahead. If
        '   the character is a control character, translate it to a space or
        '   linefeed.
        Private Function [get](ByRef sr As StreamReader) As Integer
            Dim c As Integer = theLookahead
            theLookahead = EOF
            If c = EOF Then
                c = sr.Read()
            End If
            If c >= AscW(" "c) OrElse c = AscW(ControlChars.Lf) OrElse c = EOF Then
                Return c
            End If
            If c = AscW(ControlChars.Cr) Then
                Return AscW(ControlChars.Lf)
            End If
            Return AscW(" "c)
        End Function

        Private Sub put(ByVal c As Integer)
            strbldr.Append(ChrW(c))
        End Sub

        Private Function isAlphanum(ByVal c As Integer) As Boolean
            Return ((c >= AscW("a"c) AndAlso c <= AscW("z"c)) OrElse _
                (c >= AscW("0"c) AndAlso c <= AscW("9"c)) OrElse _
                (c >= AscW("A"c) AndAlso c <= AscW("Z"c)) OrElse _
                c = AscW("_"c) OrElse c = AscW("$"c) OrElse c = AscW("\"c) _
                OrElse c > 126)
        End Function
    End Class