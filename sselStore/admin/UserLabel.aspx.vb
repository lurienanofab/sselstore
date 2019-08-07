Imports LNF
Imports LNF.Models.Data
Imports LNF.Repository
Imports LNF.Repository.Data
Imports sselStore.AppCode

Namespace Admin
    Public Class UserLabel
        Inherits StorePage

        Private ReadOnly STAFF As String = "LNF Staff"  ' this text is used in UI(Javascript) too.

        Dim arrayAllUser As ArrayList = Nothing

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim allClients As IList(Of ListItem) = DA.Current.Query(Of ClientInfo)().Where(Function(x) x.ClientActive).OrderBy(Function(x) x.DisplayName).Select(Function(x) New ListItem(x.DisplayName, x.ClientID.ToString())).ToList()
                allClients.Insert(0, New ListItem("-- Select --", "0"))
                ddlUsers.DataSource = allClients
                ddlUsers.DataBind()
            End If

            'Dim script As String = "var someobj = {param1:'123', param2:6};"
            'Page.ClientScript.RegisterStartupScript(Page.GetType(), "test", script, True)
        End Sub

        Private Function IsValidClient(ByVal x As Client) As Boolean
            Dim dt As Date? = ServiceProvider.Current.Data.Client.LastRoomEntry(x.ClientID)
            Dim b As Boolean = x.Active AndAlso dt IsNot Nothing AndAlso dt.Value > Date.Now.AddDays(-180)
            Return b
        End Function

        Protected Sub BtnTest2_Click(ByVal sender As Object, ByVal e As EventArgs)
            arrayAllUser = New ArrayList()

            Dim p As ClientPrivilege = ClientPrivilege.LabUser Or ClientPrivilege.Staff
            Dim allClients As IEnumerable(Of Client) = DA.Current.Query(Of Client)().Where(Function(x) x.Active AndAlso (x.Privs And p) > 0).ToList().Where(Function(x) IsValidClient(x))

            For Each client As Client In allClients
                CreateUserLabelDataForClientOrg(client)

                'Dim uld As UserLabelData = New UserLabelData()
                'uld.fName = client.FName
                'uld.lName = client.LName
                'arrayAllUser.Add(uld)
            Next

            LabelPrinterUtility.CreatePDF(arrayAllUser)
        End Sub

        Private Sub CreateUserLabelDataForClientOrg(ByVal user As Client)
            Dim allUserAccounts As New List(Of Account)()
            Dim allUserOrgs As New List(Of Org)()
            'CurrentUser.GetUserAccountsOrgs(user, allUserAccounts, allUserOrgs)

            Dim isUserStaff As Boolean = IsStaff(user)

            'arrayAllUser.Add(createUldForEachClientOrg4UserAndAddedtoList(client, False))

            Dim allcorgs As IEnumerable(Of ClientOrg) = DA.Current.Query(Of ClientOrg)().Where(Function(x) x.Client Is user).ToList()

            If isUserStaff Then
                Dim uld As UserLabelData = New UserLabelData With {
                    .fName = user.FName,
                    .lName = user.LName,
                    .profOrgName = "LNF Staff"
                }

                arrayAllUser.Add(uld)
            Else
                For Each oneCOrg In allcorgs
                    Dim uld As UserLabelData = New UserLabelData With {
                        .fName = user.FName,
                        .lName = user.LName,
                        .startDate = String.Format("{0: MMM yyyy}", StartDate(user))
                    }

                    'Dim usertypeExIn = UserType(user)
                    'uld._externalInternal = usertypeExIn

                    ' get all managers of this client  through this perticular organization. select the first manager.    ???????????????????????????????
                    If oneCOrg.Org.OrgID = 17 Then   'get the technical manager name
                        Dim managers As List(Of ClientManager) = DA.Current.Query(Of ClientManager)().Where(Function(x) x.ClientOrg Is oneCOrg).ToList()
                        Dim allCMs As IEnumerable(Of ClientManager) = managers.Where(Function(x) x.Active)

                        Dim prof As String = "Prof. "

                        For Each cm As ClientManager In allCMs
                            If cm.ManagerOrg.Active Then
                                uld.profOrgName = prof + cm.ManagerOrg.Client.LName
                                Exit For
                            End If
                        Next

                        'Dim allMans As List(Of Client) = New List(Of Client)
                        'CurrentUser.GetUserAccountsOrgs(user, allUserAccounts, allUserOrgs)
                        'For Each acct In allUserAccounts
                        '    Dim query As IEnumerable(Of ClientAccount) = acct.ClientAccounts.Where(Function(x) x.Active AndAlso x.Manager)
                        '    'allMans.AddRange(query.Select(Function(x) x.ClientOrg.Client))
                        '    For Each ca As ClientAccount In query
                        '        If True = ca.ClientOrg.IsManager Then
                        '            allMans.Add(ca.ClientOrg.Client)
                        '        End If
                        '    Next
                        'Next

                        ''If usertypeExIn = LabelPrinterUtil.USER_INTERNAL Then
                        'If allMans.Count > 0 Then
                        '    uld.profOrgName = prof + allMans(0).LName
                        'End If
                        ''End If

                    Else
                        uld.profOrgName = oneCOrg.Org.OrgName 'allUserOrgs(0).OrgName
                    End If
                    If Not uld.profOrgName = "" Then
                        'Dim d As String = "testing"
                        arrayAllUser.Add(uld)  ' Add uld to the list  <--------------
                    End If

                Next

            End If

        End Sub

        Private Sub GetUserAccountsAndOrgs(client As Client, accts As List(Of Account), orgs As List(Of Org))
            Dim clientOrgs As IQueryable(Of ClientOrg) = DA.Current.Query(Of ClientOrg)().Where(Function(x) x.Client Is client)
            Dim clientAccounts As IQueryable(Of ClientAccount) = clientOrgs.Join(DA.Current.Query(Of ClientAccount), Function(o) o.ClientOrgID, Function(i) i.ClientOrg.ClientOrgID, Function(o, i) i)
            accts.AddRange(clientAccounts.Where(Function(x) x.Active AndAlso x.ClientOrg.Active).Select(Function(x) x.Account))
            orgs.AddRange(clientOrgs.Where(Function(x) x.Active).Select(Function(x) x.Org))
        End Sub

        Private Sub CreateUserLabelDataForUI(ByVal user As Client)
            Dim allUserAccounts As New List(Of Account)()
            Dim allUserOrgs As New List(Of Org)()

            Dim usertypeExIn = UserType(user)

            Dim isUserStaff As Boolean = IsStaff(user)

            Dim sDate As String = String.Format("{0: MMM yyyy}", StartDate(user))

            Dim allMans As List(Of Client) = New List(Of Client)
            GetUserAccountsAndOrgs(user, allUserAccounts, allUserOrgs)
            For Each acct In allUserAccounts
                Dim clientAccounts As IList(Of ClientAccount) = DA.Current.Query(Of ClientAccount)().Where(Function(x) x.Account Is acct).ToList()
                Dim query As IEnumerable(Of ClientAccount) = clientAccounts.Where(Function(x) x.Active AndAlso x.Manager)
                For Each ca As ClientAccount In query
                    If True = ca.ClientOrg.IsManager Then
                        allMans.Add(ca.ClientOrg.Client)
                    End If
                Next
            Next


            If isUserStaff Then lblStaff.Text = STAFF Else lblStaff.Text = "No"
            lblUserFirstName.Text = user.FName
            lblUserLastName.Text = user.LName
            ddlAccounts.Items.Clear()
            ddlOrgs.Items.Clear()
            ddlAccounts.DataSource = allUserAccounts
            ddlAccounts.DataBind()
            ddlOrgs.DataSource = allUserOrgs
            ddlOrgs.DataBind()
            lblExternalInternal.Text = usertypeExIn
            lblStartDate.Text = sDate
            'Return uld
            'For Each co As ClientOrg In user.ClientOrgs.Where(Function(x) x.Active)
            Dim clientOrgs As IList(Of ClientOrg) = DA.Current.Query(Of ClientOrg)().Where(Function(x) x.Client Is user).ToList()
            For Each co As ClientOrg In clientOrgs
                Dim clientManagers As IList(Of ClientManager) = DA.Current.Query(Of ClientManager)().Where(Function(x) x.ClientOrg Is co).ToList()
                For Each cmo In clientManagers
                    allMans.Add(cmo.ManagerOrg.Client)
                Next
            Next
            ddlAllManagers.DataSource = allMans.Distinct()
            ddlAllManagers.DataBind()

        End Sub

        Public Sub Print(ByVal data As Object)
            Debug.Print(data.ToString())
        End Sub

        Protected Sub DdlUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlUsers.SelectedIndexChanged
            If ddlUsers.SelectedIndex > 0 Then
                Dim cid As Integer = Convert.ToInt32(ddlUsers.SelectedValue)
                Dim user As Client = DA.Current.Single(Of Client)(cid)
                CreateUserLabelDataForUI(user)
            End If

            'ddlAllManagers.Items.Clear()

            'If Not ddlUsers.SelectedValue.StartsWith(TEXT_SELECT) Then
            '    Dim user As Client = Client.DataAccess.Search(Function(x) x.ClientID = ddlUsers.SelectedValue).FirstOrDefault()
            '    updateUser(user)
            '    For Each account In ddlAccounts.Items
            '        If account.ToString().StartsWith(TEXT_SELECT) Then
            '            Continue For
            '        End If
            '        allMans.AddRange(getManagers(Integer.Parse(account.ToString()), user))
            '    Next

            '    allMans.ForEach(Sub(xman) ddlAllManagers.Items.Add(xman.UserName))
            'End If
        End Sub

        Private Function UserType(ByVal user As Client) As String
            'Dim all_corgs As Integer = user.ClientOrgs.Count
            Dim clientOrgs As IList(Of ClientOrg) = DA.Current.Query(Of ClientOrg)().Where(Function(x) x.Client Is user).ToList()
            Dim corgs_with_UM As IList(Of ClientOrg) = clientOrgs.Where(Function(x) x.Org.OrgID = 17).ToList()  '17 is University of Michigan
            If 0 = corgs_with_UM.Count Then
                Return LabelPrinterUtility.USER_EXTERNAL   'only external orgs, no internal so Organization (ex: Neuro Nexus)
            ElseIf (corgs_with_UM.Count = clientOrgs.Count) Then
                Return LabelPrinterUtility.USER_INTERNAL 'Then all client org are Internal - (ex:- Prof)
            ElseIf (corgs_with_UM.Count < clientOrgs.Count) Then
                'Return LabelPrinterUtil.USER_MIXED 'Mixed, client has both internal aswell as External
                Return LabelPrinterUtility.USER_MIXED
            End If
            Return Nothing ' Null code. This should never be reached.
        End Function

        Private Function IsStaff(ByVal user As Client) As Boolean
            If ((user.Privs And ClientPrivilege.Staff) > 0 AndAlso user.Active) Then
                Return True
            End If
            Return False
            'Dim staff As IEnumerable(Of Client) = Client.DataAccess.Search(Function(x) (x.Privs And ClientPrivilege.Staff) > 0 AndAlso x.Active)
            'If staff.Count > 0 Then            Return True   End If
            Return False
        End Function

        Protected Sub DdlAccounts_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAccounts.SelectedIndexChanged
            'If ddlAccounts.SelectedIndex > 0 Then
            'Dim user As Client = Client.DataAccess.Search(Function(x) x.ClientID = ddlUsers.SelectedValue).FirstOrDefault()
            'Dim techMgrs As IEnumerable(Of Client) = CurrentUser.GetManagers(ddlAccounts.SelectedValue, user)
            'ddlManagers.DataSource = techMgrs
            'ddlManagers.DataBind()
            'End If
        End Sub

        Private Function StartDate(ByVal user As Client) As Date
            Return DA.Current.Query(Of ActiveLog)().Where(Function(x) x.TableName = "Client" AndAlso x.Record = user.ClientID).Min(Function(x) x.EnableDate)
        End Function
    End Class
End Namespace