Imports System.Web
Imports LNF.Data
Imports LNF.Models.Data
Imports LNF.Repository
Imports LNF.Repository.Data
Imports LNF.Web

Namespace BLL
    Public Class CurrentUser
        Public Shared Function GetCurrentUser() As IClient
            Dim context As HttpContextBase = New HttpContextWrapper(HttpContext.Current)
            Return context.CurrentUser()
        End Function

        Public Shared Function GetCurrentUserName() As String
            Return GetCurrentUser().UserName
        End Function

        Public Shared Function GetCurrentClientName() As String
            Return HttpContext.Current.Session("DisplayName").ToString()
        End Function

        Public Shared Function GetCurrentClientID() As Integer
            Return Convert.ToInt32(HttpContext.Current.Session("ClientID"))
        End Function

        Public Shared Function GetCurrentClientContact() As String
            Return HttpContext.Current.Session("DisplayName").ToString()
        End Function

        Public Shared Function IsAdmin() As Boolean
            Dim authTypes As ClientPrivilege = ClientPrivilege.StoreManager Or ClientPrivilege.Administrator Or ClientPrivilege.Developer
            Return GetCurrentUser().HasPriv(authTypes)
        End Function

        Public Shared Sub RedirectMeToLogin(ByVal strAction As String)
            HttpContext.Current.Session.Abandon()
            HttpContext.Current.Response.Redirect(HttpContext.Current.Session("Logout").ToString() + "?Action=" + strAction)
        End Sub

        Public Shared Sub CheckSession()
            If HttpContext.Current.Session("UserName") IsNot Nothing AndAlso HttpContext.Current.Session("UserName").ToString() = HttpContext.Current.User.Identity.Name Then
                Return
            End If

            'check if record exists in DB
            Using reader As ExecuteReaderResult = DA.Command().Param("Action", "GetSessionInfo").Param("UserName", HttpContext.Current.User.Identity.Name).ExecuteReader("dbo.Client_CheckAuth")
                ' if this doesn't return true... 
                If reader.Read() Then
                    HttpContext.Current.Session("ClientID") = Convert.ToInt32(reader("ClientID"))
                    HttpContext.Current.Session("UserName") = Convert.ToString(reader("UserName"))
                    HttpContext.Current.Session("DisplayName") = Convert.ToString(reader("DisplayName"))
                    HttpContext.Current.Session("Privs") = Convert.ToInt32(reader("Privs"))
                    HttpContext.Current.Session("OrgID") = Convert.ToInt32(reader("OrgID"))
                    HttpContext.Current.Session("Cache") = Guid.NewGuid.ToString()
                Else
                    HttpContext.Current.Server.Transfer(HttpContext.Current.Session("Logout").ToString())
                End If
                reader.Close()
            End Using
        End Sub

        Public Shared Sub GetUserAccountsOrgs(ByVal user As Client, ByVal alluserAccounts As List(Of Account), ByVal allUserOrgs As List(Of LNF.Repository.Data.Org))
            Dim clientOrgs As IList(Of ClientOrg) = DA.Current.Query(Of ClientOrg)().Where(Function(x) x.Client Is user AndAlso x.Active).ToList()
            For Each co As ClientOrg In clientOrgs
                For Each ca As ClientAccount In co.ClientAccounts.Where(Function(x) x.Active)
                    alluserAccounts.Add(ca.Account)
                    Dim temp As ClientAccount = ca
                    Dim previousOrg = allUserOrgs.Where(Function(x) x.OrgID = temp.Account.Org.OrgID).FirstOrDefault()
                    If previousOrg Is Nothing Then
                        allUserOrgs.Add(ca.Account.Org)
                    End If
                Next
            Next
        End Sub

        Public Shared Function GetManagers(ByVal accountId As Integer, ByVal user As Client) As IEnumerable(Of Client)
            Dim ca As ClientAccount = DA.Current.Query(Of ClientAccount)().First(Function(x) x.Account.AccountID = accountId AndAlso x.ClientOrg.Client.ClientID = user.ClientID)
            Dim clientAccounts As IList(Of ClientAccount) = DA.Current.Query(Of ClientAccount).Where(Function(x) x.Account Is ca.Account).ToList()
            Dim techMgrs As IEnumerable(Of ClientAccount) = clientAccounts.Where(Function(x) x.ClientAccountID <> ca.ClientAccountID AndAlso x.Manager AndAlso x.Active)
            Return techMgrs.Select(Function(x) x.ClientOrg.Client)
        End Function
    End Class
End Namespace
