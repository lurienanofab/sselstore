Imports LNF.Repository

Namespace BLL
    Public Class SselData
        Public Shared Function GetClientNameByClientID(ByVal clientId As Integer) As String
            Using reader As ExecuteReaderResult = DA.Command().Param("Action", "GetEmails").Param("ClientID", clientId).ExecuteReader("dbo.Client_Select")
                If reader.Read() Then
                    Return reader.Value("DisplayName", String.Empty)
                Else
                    Return "Error"
                End If
            End Using
        End Function

        Public Shared Function GetClientContactByClientID(ByVal clientId As Integer, ByVal accountId As Integer) As String
            Dim result As String = String.Empty
            Dim newline As String = String.Empty

            Dim dt As DataTable = DA.Command(CommandType.Text) _
                    .Param("ClientID", clientId) _
                    .Param("AccountID", accountId) _
                    .FillDataTable("SELECT TOP 1 co.Phone, co.Email FROM sselData.dbo.Client c INNER JOIN sselData.dbo.ClientOrg co ON co.ClientID = c.ClientID INNER JOIN sselData.dbo.ClientAccount ca ON ca.ClientOrgID = co.ClientOrgID WHERE c.ClientID = @ClientID AND ca.AccountID = @AccountID")

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Email") IsNot DBNull.Value Then
                    If Not String.IsNullOrEmpty(dt.Rows(0)("Email").ToString()) Then
                        result += newline + dt.Rows(0)("Email").ToString()
                        newline = Environment.NewLine
                    End If
                End If
                If dt.Rows(0)("Phone") IsNot DBNull.Value Then
                    If Not String.IsNullOrEmpty(dt.Rows(0)("Phone").ToString()) Then
                        result += newline + dt.Rows(0)("Phone").ToString()
                        newline = Environment.NewLine
                    End If
                End If
            End If

            Return result
        End Function

        Public Shared Function GetClients() As DataView
            Return DA.Command().Param("Action", "All").FillDataTable("dbo.Client_Select").DefaultView
        End Function
    End Class
End Namespace