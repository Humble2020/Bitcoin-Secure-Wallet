Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class MyPrettyFile
    Public Function ReturnFilePath() As String
        Dim ofd As New OpenFileDialog()
        If ofd.ShowDialog() = DialogResult.OK Then
            Return ofd.FileName
        Else
            Return "9_walT.9J1Q"
        End If
    End Function

    Public Function CheckWalletName(ByVal name As String) As Boolean
        'Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        If File.Exists(name) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ReadFile(ByVal name As String) As String
        ''Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        Return File.ReadAllText(name)
    End Function

    Public Sub SaveToFile(ByVal content As String, ByVal path As String)
        'Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        Dim fs As New FileStream(path, FileMode.Create)
        Dim data() As Byte = (New UTF8Encoding(True)).GetBytes(content)
        fs.Write(data, 0, data.Length)
        fs.Close()
    End Sub
    Public Sub SaveUnecryptedFile(ByVal plain As String, ByVal path As String)
        SaveToFile(plain, path)
    End Sub
    Public Sub SaveEncryptedFile(ByVal cipher As String, ByVal password As String, ByVal path As String)
        Dim aes As New Aes()
        Dim encrypted As String = aes.Encrypt(cipher, password)
        SaveToFile(encrypted, path)
    End Sub
End Class
