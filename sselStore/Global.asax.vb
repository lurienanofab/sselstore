Imports LNF.Repository
Imports LNF.Web

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        If Environment.GetEnvironmentVariable("vs80comntools") IsNot Nothing Then
            Application("AppServer") = "http://localhost/"
        Else
            Application("AppServer") = "http://" + Environment.MachineName + ".eecs.umich.edu/"
        End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        If Request.IsAuthenticated Then
            Dim myIdent As FormsIdentity = CType(User.Identity, FormsIdentity)
            Dim aryRoles() As String = myIdent.Ticket.UserData.Split("|"c)
            Context.User = New Security.Principal.GenericPrincipal(myIdent, aryRoles)
        End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim enabled As Boolean = False

        If enabled Then
            Dim sb As New StringBuilder()
            Dim ex As Exception = Server.GetLastError()
            If ex IsNot Nothing Then
                sb.AppendLine("Error message: " + ex.Message + "<br /><br />")
                If ex.InnerException IsNot Nothing Then
                    sb.AppendLine("Inner exception: " + ex.InnerException.Message + "<br /><br />")
                End If
                sb.AppendLine("Stack trace: " + ex.StackTrace + "<br /><br />")
            Else
                sb.AppendLine("Server.GetLastError() Is Nothing")
            End If

            Dim errorMsg As String = sb.ToString()
            Dim errorGuid As Guid
            Try
                Dim cmd As IDataCommand = DA.Command()
                With cmd 'cmd As New SqlCommand("PassError_Select", conn)
                    .Param("Action", "Set")
                    .Param("ErrorMsg", errorMsg)
                    .Param("ClientID", Session("ClientID"))
                    .Param("ClientName", Session("DisplayName"))
                    .Param("FilePath", Context.Request.FilePath)
                End With
                errorGuid = New Guid(cmd.ExecuteScalar(Of String)("dbo.PassError_Select").Value)
            Catch
                errorGuid = Guid.Empty
            End Try

            Context.ClearError()
            Response.Redirect("/sselOnLine/ErrorPage.aspx?ErrorID=" + errorGuid.ToString())
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Session("Logout") = Application("appServer").ToString() + "sselonline/Login.aspx"

        ' remember - to get here, the user is already authenticated
        Dim context As HttpContextBase = New HttpContextWrapper(HttpContext.Current)
        context.CheckSession()
    End Sub
End Class