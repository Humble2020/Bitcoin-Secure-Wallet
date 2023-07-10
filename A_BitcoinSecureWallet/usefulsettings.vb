Imports System.Runtime.InteropServices

Public Class usefulsettings

    Public Shared Function CheckInternetConnectivity() As Boolean
        Try
            Dim description As Integer = Nothing
            Return InternetGetConnectedState(description, 0)
        Catch ex As Exception
            Dim [error] As String = ex.Message
            Return False
        End Try
    End Function
    <DllImport("wininet.dll")>
    Private Shared Function InternetGetConnectedState(ByRef description As Integer, ByVal reservedValue As Integer) As Boolean
    End Function
End Class
