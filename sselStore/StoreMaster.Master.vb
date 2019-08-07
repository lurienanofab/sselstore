Imports LNF.Models.Data
Imports LNF.Repository
Imports LNF.Web
Imports LNF.Web.Content
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class StoreMaster
    Inherits LNFMasterPage

    Dim tabid As String 'it has to be class scope, more than one function uses it

    Public Overrides ReadOnly Property ShowMenu As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property AddScripts As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property AddStyles As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If ConfigurationManager.AppSettings("StoreMaintenance") IsNot Nothing AndAlso ConfigurationManager.AppSettings("StoreMaintenance").Equals("Yes") Then
            Response.Redirect("~/Maintenance.aspx")
        End If
        ContextBase.CheckSession()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not CurrentUser.HasPriv(ClientPrivilege.StoreUser) Then
            Response.Redirect("~/NoAccess.aspx")
        End If

        If CurrentUser.HasPriv(ClientPrivilege.Administrator Or ClientPrivilege.StoreManager Or ClientPrivilege.Developer) Then
            liAdmin.Visible = True
        End If

        SetDisplaySettings()
    End Sub

    Private Sub SetDisplaySettings()
        'Set Tab Menu color
        'No need to sanitize tab data, since this is used only for aesthetic purpose
        tabid = Request.QueryString("tab")

        If tabid = "1" Then
            hylMyCart.Font.Bold = True
            hylMyCart.Font.Underline = True
        ElseIf tabid = "2" Then
            hylMyOrder.Font.Bold = True
            hylMyOrder.Font.Underline = True
        ElseIf tabid = "3" Then
            hylContact.Font.Bold = True
            hylContact.Font.Underline = True
        ElseIf tabid = "4" Then
            hylHelp.Font.Bold = True
            hylHelp.Font.Underline = True
        ElseIf tabid = "9" Then
            RedirectToLogin("Blank")
        Else
            tabid = "0"
            hylMain.Font.Bold = True
            hylMain.Font.Underline = True
        End If
    End Sub

    Private Sub RedirectToLogin(page As String)
        Select Case page
            Case "Blank"
                Response.Redirect("/sselonline/Blank.aspx")
            Case Else
                Throw New NotSupportedException("Unknown page: " + page)
        End Select
    End Sub

    Protected Sub Tree_Load(sender As Object, e As EventArgs) Handles tree.Load
        'special case for user click on the sub categories in the catalog page instead of the tree control
        'I still couldn't figure out how to control the tree object from the child page.
        'This is the temp solution

        Dim flag As Boolean = False
        If Not Request.QueryString("self") = Nothing Then
            If Request.QueryString("self").ToString() = "true" Then
                flag = True
            End If
        End If

        If Not Page.IsPostBack OrElse flag Then
            If flag Then
                'Two scenarios will come here
                '1. User click on the right panel link 
                '2. User click on treeview link right after click on right panel link
                tree.Nodes.Clear()

                'Save the tree setting again, for user click on right panel's link instead of treeview link
                If Session("NodeToExpand") IsNot Nothing Then
                    Dim lastNode As String = Session("NodeToExpand").ToString()
                    Dim path As String() = lastNode.Split(Char.Parse("/"))
                    Dim lastPathValue As String = path(path.Length - 1)

                    If Not Request.QueryString("cid") = Nothing Then
                        If lastPathValue <> Request.QueryString("cid").ToString() Then
                            lastNode = lastNode + "/" + Request.QueryString("cid")
                            Session.Add("NodeToExpand", lastNode)
                        End If
                    End If
                End If
            End If
            PopulateTreeRoot()
            SetTreeState()
        End If
    End Sub

    Protected Sub Tree_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tree.SelectedNodeChanged
        SaveTreeState()
    End Sub

    Protected Sub Tree_TreeNodePopulate(sender As Object, e As TreeNodeEventArgs) Handles tree.TreeNodePopulate
        'This function will only be called if node is expandable
        Select Case e.Node.Depth
            Case 0
                Dim cid As Integer = CType(e.Node.Value, Integer)
                Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(cid, 1, True)
                    'Add any child nodes if it has any
                    While reader.Read()
                        Dim newNode As TreeNode = New TreeNode(reader.Value(DBConstants.CATEGORY_CATNAME, "[unknown]"), reader.Value(DBConstants.CATEGORY_CATID, "-1")) With {
                            .SelectAction = TreeNodeSelectAction.SelectExpand,
                            .PopulateOnDemand = True,
                            .Expanded = True
                        }
                        e.Node.ChildNodes.Add(newNode)
                    End While
                    reader.Close()
                End Using
        End Select
    End Sub

    Public Sub SaveTreeState()
        'Whenever user selects a node, we need to find out if this is a leaf node
        Dim cid As Integer

        If Integer.TryParse(tree.SelectedValue, cid) Then
            'It's leaf node, so we have to display the catalog
            Session.Add("NodeToExpand", tree.SelectedNode.ValuePath)
        End If

        If cid = GeneralConstants.KIT_ID Then
            Response.Redirect("CatalogKit.aspx?tabid=0&h1=1")
        Else
            Response.Redirect(String.Format("Catalog.aspx?tabid=0&h1=1&cid={0}", cid))
        End If
    End Sub

    Public Sub SetTreeState()
        'We need to find out the old treeview setting - which node is selected?
        If tabid = "0" Then 'to make sure if user clicks on any tab other than main tab, we don't bother to remember the tree setting
            If Session("NodeToExpand") IsNot Nothing Then
                Dim temp As String = Session("NodeToExpand").ToString()
                Dim path() As String = temp.Split(Char.Parse("/"))
                Try
                    tree.FindNode(path(0)).Expand()
                    tree.FindNode(temp).Selected = True
                    tree.FindNode(temp).Expanded = True
                Catch ex As Exception
                    'some weird bugs appear here occasionaly, need to pay attention
                End Try
            End If
        End If
    End Sub

    Private Sub PopulateTreeRoot()
        'Load All of the root nodes available
        Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(0, 0, True)
            While reader.Read()
                Dim text As String = reader.Value(DBConstants.CATEGORY_CATNAME, "[unknown]")
                Dim value As String = reader.Value(DBConstants.CATEGORY_CATID, "-1")
                Dim newNode As New TreeNode(text, value) With {
                    .SelectAction = TreeNodeSelectAction.SelectExpand,
                    .PopulateOnDemand = True
                }
                tree.Nodes.Add(newNode)
            End While
            reader.Close()
        End Using

        'Load Kit
        ' when implemented, separate from categories with a large gap or line
        If liAdmin.Visible Then
            Dim kitNode As New TreeNode("Kits", GeneralConstants.KIT_ID.ToString())
            tree.Nodes.Add(kitNode)
        End If

        'Check if the root nodes have child, if they don't, expand that leaf root node
        For Each node As TreeNode In tree.Nodes
            If Not CatalogManager.HasChildCategories(CType(node.Value, Integer)) Then
                node.Expanded = True
            End If
        Next
    End Sub

    Protected Sub IbtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click
        If Not String.IsNullOrEmpty(txtSearch.Text) Then
            Dim url As String = "Search.aspx?q=" + Server.UrlEncode(Server.HtmlEncode(txtSearch.Text))
            Response.Redirect(url)
        End If
    End Sub

    Protected Sub LbtnExitApp_Click(sender As Object, e As EventArgs)
        Dim exitAppUrl As String = ConfigurationManager.AppSettings("ExitAppUrl")
        Response.Redirect(exitAppUrl, True)
    End Sub
End Class