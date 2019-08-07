Imports LNF.Data
Imports LNF.Models.Data
Imports LNF.Repository
Imports LNF.Repository.Inventory
Imports LNF.Web
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL
Imports sselStore.Controls

Public Class Checkout
    Inherits StorePage

    'Dim isKit As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Check if we are proccessing kit shopping or regular shopping
        CartView1.IsKit = Request.QueryString("kit") IsNot Nothing

        If CartView1.ItemCount <= 0 Then
            Response.Redirect("~/Cart.aspx?tab=1")
        End If

        If Not IsPostBack Then
            If CartView1.IsKit Then
                divClient.Visible = True
                divReviewMsg.Visible = False
                ddlClient.DataSource = SselData.GetClients()
            End If

            Dim accts As IList(Of IClientAccount) = ContextBase.GetCurrentUserClientAccounts().ToList()

            Dim orderedAccts As IList(Of IClientAccount) = ClientPreferenceUtility.OrderListByUserPreference(CurrentUser, accts, Function(x) x.AccountID, Function(x) x.AccountName)

            Dim listItems As IEnumerable(Of ListItem) = orderedAccts.Select(Function(x) New ListItem(AccountItem.GetFullAccountName(x.ShortCode, x.AccountName, x.OrgName), x.AccountID.ToString()))

            ddlAccount.DataSource = listItems
            ddlAccount.DataBind()

            divLocation.Visible = False

            Dim locCats As IList(Of Category) = DA.Current.Query(Of Category)().Where(Function(x) x.RequireLocation).ToList()
            Dim items As StoreOrderItems = CartView1.GetItems()
            For Each row As DataRow In items.Rows
                Dim itemID As Integer = row.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID)
                Dim aItem As Inventory.Item = DA.Current.Single(Of Inventory.Item)(itemID)
                For Each cat In locCats
                    If cat.CatID = aItem.Category.CatID Then
                        divLocation.Visible = True
                        Exit For
                    End If
                Next
            Next

            Dim iloc As IList(Of InventoryLocation) = DA.Current.Query(Of InventoryLocation)().Where(Function(x) x.IsStoreLocation).ToList()
            ddlLocation.DataSource = iloc
            ddlLocation.DataBind()

            CartView1.LoadCart()
        End If
    End Sub

    Private Function GetLocationID() As Integer
        If divLocation.Visible Then
            Return Convert.ToInt32(ddlLocation.SelectedValue)
        Else
            Return Convert.ToInt32(ConfigurationManager.AppSettings("DefaultPickupLocationID"))
        End If
    End Function

    Protected Sub btnPlaceOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlaceOrder.Click
        'make sure the user has an account we can charge for
        Dim accountId As Integer
        Dim locationId As Integer = GetLocationID()

        If Not Integer.TryParse(ddlAccount.SelectedValue, accountId) Then
            lblMessage.Text = "There is not account specified for this purchase. Please contact LNF staff to find out why you don't have an account associated"
            Return
        End If

        'Save to db
        Dim newSOID As Integer
        Dim clientId As Integer = ContextBase.CurrentUser().ClientID
        If StoreOrderManager.InsertNewOrder(clientId, accountId, newSOID, locationId) Then
            'insert all items
            Dim items As StoreOrderItems = CartView1.GetItems()
            If CartManager.AddItems(items, newSOID) Then
                If Not CartView1.IsKit Then
                    CartManager.RemoveCart("Cart")
                Else
                    clientId = Convert.ToInt32(ddlClient.SelectedValue)
                    KitManager.InsertKitOrder(newSOID, clientId)
                    Session.Remove("KitCart")
                End If
                Response.Redirect(String.Format("~/OrderReceipt.aspx?tab=1&soid={0}", newSOID))
            Else
                lblMessage.Text = "Error in placing an order, please try again by going back to your shopping cart(1)"
                btnPlaceOrder.Enabled = False
            End If
        Else
            lblMessage.Text = "Error in placing an order, please try again by going back to your shopping cart(2)"
            btnPlaceOrder.Enabled = False
        End If

    End Sub

    Protected Sub CartView1_CartLoading(sender As Object, e As CartLoadingEventArgs)
        Dim accountId As Integer
        If Integer.TryParse(ddlAccount.SelectedValue, accountId) Then
            PriceManager.ApplyPriceMultiplier(e.Items, accountId)
        End If
    End Sub

    Protected Sub ddlAccount_SelectedIndexChanged(sender As Object, e As EventArgs)
        CartView1.LoadCart()
    End Sub
End Class