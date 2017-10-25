Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

''' <summary>
''' A helper class to simplify string encryption calls
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class EncryptionHelper
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class EncryptionHelper
#End If

    ''' <summary>
    ''' The function used to encrypt the text from a string key.
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="strEncrKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The function used to encrypt the text from a string key.")> _
        Public Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
            If strText Is Nothing Then strText = ""
            While strEncrKey.Length < 8
                strEncrKey &= "0"
            End While
            Dim byKey() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

            'Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))

            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())

            'Catch ex As Exception
            '    Return ex.Message
            'End Try

        End Function

    ''' <summary>
    ''' The function used to decrypt the text from a string key.
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="sDecrKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("The function used to decrypt the text from a string key.")> _
        Public Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
            If strText Is Nothing Then strText = ""
            While sDecrKey.Length < 8
                sDecrKey &= "0"
            End While
            Dim byKey() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
            Dim inputByteArray(strText.Length) As Byte

            'Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

            'Catch ex As Exception
            '    Return ex.Message
            'End Try

        End Function
    End Class
