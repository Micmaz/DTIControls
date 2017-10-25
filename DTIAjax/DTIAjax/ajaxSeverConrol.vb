Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports BaseClasses

#If DEBUG Then
Public Class ajaxSeverConrol
    Inherits System.Web.UI.Control
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class ajaxSeverConrol
        Inherits System.Web.UI.Control
#End If

        Private Sub jsonSeverConrol_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptBlock(Page, _
                "function " & jsCallFunc & "(target,action1,datainput,successFunction){" & vbCrLf & _
                "if(!successFunction){successFunction=" & jsCompleteFunc & ";} " & vbCrLf & _
                "target.load('" & postURL & "', {action: action1,id:'" & ajaxId & "',input:datainput}, successFunction);}" & vbCrLf, False)
            Page.Session(ajaxId) = workerclass
        End Sub

        Private _workerclass As String = "DTIAjax.jsonWorker"
        Public Property workerclass() As String
            Get
                Return _workerclass
            End Get
            Set(ByVal value As String)
                _workerclass = value
            End Set
        End Property

        Private _ajaxId As String = Nothing
        Public Property ajaxId() As String
            Get
                If _ajaxId Is Nothing Then _ajaxId = Guid.NewGuid.ToString
                Return _ajaxId
            End Get
            Set(ByVal value As String)
                _ajaxId = value
            End Set
        End Property

        Private _ActionCompleteFunction As String = Nothing
        Public Property jsCompleteFunc() As String
            Get
                If _ActionCompleteFunction Is Nothing Then
                    Return "function(data){;}"
                End If
                Return _ActionCompleteFunction
            End Get
            Set(ByVal value As String)
                _ActionCompleteFunction = value
            End Set
        End Property

        Private _returnType As ajaxReturnType = ajaxReturnType.json
        Public Property ajaxReturn() As ajaxReturnType
            Get
                Return _returnType
            End Get
            Set(ByVal value As ajaxReturnType)
                _returnType = value
            End Set
        End Property

        Private _callingfunt As String = Nothing
        Public Property jsCallFunc() As String
            Get
                If _callingfunt Is Nothing Then
                    _callingfunt = "ajax" & Me.ClientID
                End If
                Return _callingfunt
            End Get
            Set(ByVal value As String)
                _callingfunt = value
            End Set
        End Property

        Private _postURL As String = "~/res/DTIAjax/ajaxpg.aspx #result > *"
        Public Property postURL() As String
            Get
                Return _postURL
            End Get
            Set(ByVal value As String)
                _postURL = value
            End Set
        End Property

        Public Enum ajaxReturnType
            xml
            html
            script
            json
            jsonp
            text
        End Enum

    End Class
