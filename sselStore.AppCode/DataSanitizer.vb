Imports System.Text.RegularExpressions
Imports System.Web

Public Class DataSanitizer
    Public Shared Function Sanitize(Of T)(ByVal data As String, ByVal regularExpression As String, ByRef retValue As T) As Boolean
        Try
            If Regex.IsMatch(data, regularExpression) Then
                retValue = DirectCast(Convert.ChangeType(data, GetType(T)), T)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function SanitizeText(ByVal text As String) As String
        Dim clean As String = HttpContext.Current.Server.HtmlEncode(text)
        Dim result As String = clean
        result = result.Replace("&amp;", "&")
        result = result.Replace("&quot;", """")
        result = result.Replace("&#39;", "'")
        Return result
    End Function
End Class
