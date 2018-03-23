Imports System.Web

Public Class DesignTimeSessionState

	Public Sub Remove(ByVal name As String)
		If isdesign Then
			designProperty.Remove(name)
		Else
			httpSession.Remove(name)
		End If
	End Sub

	Public ReadOnly Property SessionID() As String
		Get
			If isdesign Then
				Return ""
			Else
				Return httpSession.SessionID
			End If
		End Get
	End Property

	Public ReadOnly Property httpResponce() As System.Web.HttpResponse
		Get
			If isdesign Then Return Nothing
			Return HttpContext.Current.Response
		End Get
	End Property

	Public ReadOnly Property httpRequest() As System.Web.HttpRequest
		Get
			If isdesign Then Return Nothing
			Return HttpContext.Current.Request
		End Get
	End Property

	Public ReadOnly Property httpSession() As System.Web.SessionState.HttpSessionState
		Get
			Return HttpContext.Current.Session
		End Get
	End Property

	Private _isdesign As Boolean = False
	Private designProperty As Hashtable
	Public Property isdesign() As Boolean
		Get
			Return _isdesign
		End Get
		Set(ByVal value As Boolean)
			_isdesign = value
			If _isdesign Then designProperty = New Hashtable
		End Set
	End Property

	Default Public Property Item(ByVal name As String) As Object
		Get
			If isdesign Then
				Return designProperty(name)
			Else
				Return httpSession(name)
			End If
		End Get
		Set(ByVal value As Object)
			If isdesign Then
				designProperty(name) = value
			Else
				httpSession(name) = value
			End If
		End Set
	End Property

End Class