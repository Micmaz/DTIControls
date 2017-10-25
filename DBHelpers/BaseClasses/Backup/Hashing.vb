Imports System.Security.Cryptography
Imports System.Text

Public Class Hashing

    ''' <summary>
    ''' Hashing algorithms supported by .Net
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Hashing algorithms supported by .Net")> _
    Public Enum HashTypes As Integer
        MD5
        SHA1
        SHA256
        SHA512
    End Enum

    ''' <summary>
    ''' Create hash based on the supplied hashing algorithm
    ''' </summary>
    ''' <param name="input">String to hash</param>
    ''' <param name="hashType">Hashing algorithim to hash input</param>
    ''' <returns>Hashed string based on supplied hashing algorithm</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Create hash based on the supplied hashing algorithm")> _
    Public Shared Function GetHash(input As String, hashType As HashTypes) As String
        Dim hasher As HashAlgorithm = Nothing

        Select Case hashType
            Case HashTypes.MD5
                hasher = MD5.Create
            Case HashTypes.SHA1
                hasher = SHA1.Create
            Case HashTypes.SHA256
                hasher = SHA256.Create
            Case HashTypes.SHA512
                hasher = SHA512.Create
        End Select

        Try
            Dim bytes() As Byte = Encoding.UTF8.GetBytes(input)
            Return Convert.ToBase64String(hasher.ComputeHash(bytes))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Checks if string equals hash based on the supplied hashing algorithm
    ''' using the SlowEquals function.
    ''' </summary>
    ''' <param name="original"></param>
    ''' <param name="hashString"></param>
    ''' <param name="hashType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Checks if string equals hash based on the supplied hashing algorithm   using the SlowEquals function.")> _
    Public Shared Function CheckHash(original As String, hashString As String, hashType As HashTypes) As Boolean
        Dim originalHash As String = GetHash(original, hashType)
        Return SlowEquals(originalHash, hashString)
    End Function

    ''' <summary>
    ''' Creates random string for salting passwords.
    ''' </summary>
    ''' <param name="Size">Size of string in bytes</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Creates random string for salting passwords.")> _
    Public Shared Function GenerateSalt(Optional ByVal Size As Integer = 24) As String
        Dim csprng As New RNGCryptoServiceProvider()
        Dim salt As Byte() = New Byte(Size - 1) {}
        csprng.GetBytes(salt)
        Return Convert.ToBase64String(salt)
    End Function

#Region "PBKDF2 Hashing"

    ''' <summary>
    ''' Hashes string using Password Based Key Derivation Function 2 (PBKDF2), generating a new salt.
    ''' Use CheckPBKDF2Hash to validate.
    ''' </summary>
    ''' <param name="value">String to hash</param>
    ''' <param name="salt">String to contain the unique salt to regenerate hash</param>
    ''' <param name="Iterations">Number of Iterations to produce derived key</param>
    ''' <param name="SaltSize">Number of bytes in Salt</param>
    ''' <returns>PBKDF2-SHA1 hash of a value and a unique salt</returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Hashes string using Password Based Key Derivation Function 2 (PBKDF2), generating a new salt.   Use CheckPBKDF2Hash to validate.")> _
    Public Shared Function SecureHash(ByVal value As String, ByRef salt As String, Optional ByVal Iterations As Integer = 1000, Optional ByVal SaltSize As Integer = 24) As String
        salt = GeneratePBKDF2Salt(SaltSize, Iterations)
        Return Hash(value, salt)
    End Function

    ''' <summary>
    ''' Computes the PBKDF2-SHA1 hash of a password.
    ''' </summary>
    ''' <param name="password">The password to hash.</param>
    ''' <param name="salt">The salt.</param>
    ''' <param name="iterations">The PBKDF2 iteration count.</param>
    ''' <param name="outputBytes">The length of the hash to generate, in bytes.</param>
    ''' <returns>A hash of the password.</returns>
    <System.ComponentModel.Description("Computes the PBKDF2-SHA1 hash of a password.")> _
    Public Shared Function PBKDF2(password As String, salt As String, iterations As Integer, outputBytes As Integer) As Byte()
        Dim saltb As Byte() = Encoding.UTF8.GetBytes(salt)
        Dim pbkdf As New Rfc2898DeriveBytes(password, saltb)
        pbkdf.IterationCount = iterations
        Return pbkdf.GetBytes(outputBytes)
    End Function

    ''' <summary>
    ''' Validates a password given a hash of the correct one.
    ''' </summary>
    ''' <param name="password">The hash to check.</param>
    ''' <param name="salt">The salt to use.</param>
    ''' <param name="hash">A hash of the correct password.</param>
    ''' <returns>True if the password is correct. False otherwise.</returns>
    <System.ComponentModel.Description("Validates a password given a hash of the correct one.")> _
    Public Shared Function CheckPBKDF2Hash(ByVal password As String, ByVal salt As String, hash As String) As Boolean
        Dim iterations As Integer = 0
        salt = splitSalt(salt, iterations)
        Dim hashb As Byte() = Convert.FromBase64String(hash)
        Dim testHash As Byte() = PBKDF2(password, salt, iterations, hashb.Length)
        Return SlowEquals(hashb, testHash)
    End Function

    Private Shared Function GeneratePBKDF2Salt(Optional ByVal Size As Integer = 24, Optional ByVal itterations As Integer = 1000) As String
        Dim it As String = itterations.ToString("X")
        Return it & "." & GenerateSalt(Size)
    End Function

    Private Shared Function Hash(ByVal value As String, ByVal salt As String) As String
        Dim iters As Integer = 0
        salt = splitSalt(salt, iters)

        Dim key As Byte() = PBKDF2(value, salt, iters, 24)
        Return Convert.ToBase64String(key)
    End Function

    Private Shared Function splitSalt(ByVal salt As String, ByRef itterations As Integer) As String
        Try
            Dim i As Integer = salt.IndexOf(".")
            itterations = Integer.Parse(salt.Substring(0, i), Globalization.NumberStyles.HexNumber)
            Return salt.Substring(i + 1)
        Catch ex As Exception
            itterations = 1000
            Return salt
        End Try
    End Function
#End Region

    ''' <summary>
    ''' Compares two byte arrays in length-constant time. This comparison
    ''' method is used so that password hashes cannot be extracted from 
    ''' on-line systems using a timing attack and then attacked off-line.
    ''' </summary>
    ''' <param name="a">The first byte array.</param>
    ''' <param name="b">The second byte array.</param>
    ''' <returns>True if both byte arrays are equal. False otherwise.</returns>
    <System.ComponentModel.Description("Compares two byte arrays in length-constant time. This comparison   method is used so that password hashes cannot be extracted from    on-line systems using a timing attack and then attacked off-line.")> _
    Public Shared Function SlowEquals(a As Byte(), b As Byte()) As Boolean
        Dim diff As UInteger = CUInt(a.Length) Xor CUInt(b.Length)
        Dim i As Integer = 0
        While i < a.Length AndAlso i < b.Length
            diff = diff Or CUInt(a(i) Xor b(i))
            i += 1
        End While
        Return diff = 0
    End Function

    ''' <summary>
    ''' Compares two strings in length-constant time. This comparison
    ''' method is used so that password hashes cannot be extracted from 
    ''' on-line systems using a timing attack and then attacked off-line.
    ''' </summary>
    ''' <param name="a">The first string.</param>
    ''' <param name="b">The second string.</param>
    ''' <returns>True if both strings are equal. False otherwise.</returns>
    <System.ComponentModel.Description("Compares two strings in length-constant time. This comparison   method is used so that password hashes cannot be extracted from    on-line systems using a timing attack and then attacked off-line.")> _
    Public Shared Function SlowEquals(a As String, b As String) As Boolean
        Dim ab As Byte() = Encoding.UTF8.GetBytes(a)
        Dim bb As Byte() = Encoding.UTF8.GetBytes(b)

        Return SlowEquals(ab, bb)
    End Function
End Class
