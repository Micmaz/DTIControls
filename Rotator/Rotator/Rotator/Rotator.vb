Public Class Rotator
    Inherits DTIServerControls.DTIServerControl

    Dim rotator As New DTIMiniControls.DivRotator
    Dim ep As New DTIContentManagement.EditPanel

    Public Sub New()
        Me.settingsPageUrl = "settingsForm.aspx"
        Me.useGenericDTIControlsProperties = True
        Me.ShowBorder = False
    End Sub

    Public Overrides ReadOnly Property Menu_Icon_Url() As String
        Get
            Return BaseClasses.Scripts.ScriptsURL & "Rotator/distibuteverticalcenter.png"
        End Get
    End Property

    Public Property TransitionSpeed() As Integer
        Get
            Return rotator.speed
        End Get
        Set(ByVal value As Integer)
            rotator.speed = value
        End Set
    End Property

    Public Property waitTime() As Integer
        Get
            Return rotator.timeout
        End Get
        Set(ByVal value As Integer)
            rotator.timeout = value
        End Set
    End Property

    Public Property Easing() As String
        Get
            Return rotator.easing
            'For Each effectName As String In [Enum].GetNames(GetType(effect))
            '    If rotator.fx.ToLower = effectName Then Return [Enum].Parse(GetType(effect), effectName)
            'Next
        End Get
        Set(ByVal value As String)
            If value = "none" Then value = "null"
            rotator.easing = value
            'rotator.fx = [Enum].GetName(GetType(effect), value)
        End Set
    End Property
    Public Enum easingEnum
        none
        easeIn
        easeOut
        easeInOut
        expoin
        expoout
        expoinout
        bouncein
        bounceout
        bounceinout
        elasin
        elasout
        elasinout
        backin
        backout
        backinout
        swing
    End Enum

    Public Property Transition() As String
        Get
            Return rotator.fx
            'For Each effectName As String In [Enum].GetNames(GetType(effect))
            '    If rotator.fx.ToLower = effectName Then Return [Enum].Parse(GetType(effect), effectName)
            'Next
        End Get
        Set(ByVal value As String)
            rotator.fx = value
            'rotator.fx = [Enum].GetName(GetType(effect), value)
        End Set
    End Property
    Public Enum effect
        blindX
        blindY
        blindZ
        cover
        curtainX
        curtainY
        fade
        fadeZoom
        growX
        growY
        scrollUp
        scrollDown
        scrollLeft
        scrollRight
        scrollHorz
        scrollVert
        shuffle
        slideX
        slideY
        toss
        turnUp
        turnDown
        turnLeft
        turnRight
        uncover
        wipe
        zoom
        all
    End Enum

    Public Property Pause() As Boolean
        Get
            Return rotator.pause
        End Get
        Set(ByVal value As Boolean)
            rotator.pause = value
        End Set
    End Property

    Public Property Randomize() As Boolean
        Get
            Return rotator.random
        End Get
        Set(ByVal value As Boolean)
            rotator.random = value
        End Set
    End Property

    Dim scriptLit As New LiteralControl
    Private Sub Rotator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '("#Rotator1 >*:last-child img").css("visibility","hidden")

        Me.Controls.Add(scriptLit)
        ep.MainID = Me.MainID
        ep.contentType = Me.contentType & "_rotator"
        If Me.Height.IsEmpty Then Me.Height = 200
        If Me.Width.IsEmpty Then Me.Width = 200
        ep.Width = Me.Width
        ep.Height = Me.Height
        Me.Controls.Add(ep)
        If Me.Mode = modes.Read Then
            rotator.Container = ep
            Me.Controls.Add(rotator)
        Else
            ep.htmlEdit.Style("overflow") = "hidden"
            ep.htmlEdit.Width = Me.Width.Value - 20
            ep.htmlEdit.Height = Me.Height.Value - 27
			'ep.htmlEdit.Entermode = 1
			ep.htmlEdit.BeforeReady = "editorOn(divid);"
            ep.htmlEdit.BeforeClientDestroyed = "editorOff(divid);"
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/Rotator/editorFunctions.js")
            'ep.htmlEdit.htmlDiv.Attributes("CKEDITOR.config.resize_maxHeight") = Me.Height.Value
        End If


    End Sub


    Private Sub Rotator_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ep.ShowBorder = False
        'If Me.Mode = modes.Write Then
        '    scriptLit.Text = "<script language='javascript'>$('#" & ep.htmlEdit.ClientID & "').bind('dtickCreated', function(e, divid, editor){editorOn(divid); });</script>"
        'End If
    End Sub
End Class
