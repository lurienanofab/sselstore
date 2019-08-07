Imports sselStore.Controls
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class Cart
    Inherits StorePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Check if we are proccessing kit shopping or regular shopping
        If Request.QueryString("kit") Is Nothing Then
            CartView1.IsKit = False
        Else
            CartView1.IsKit = True
        End If

        If Not Page.IsPostBack Then
            CartView1.LoadCart()
        End If
    End Sub

    Protected Sub CartView1_ItemDetailClick(sender As Object, e As CartItemEventArgs)
        Response.Redirect(String.Format("~/ItemDetail.aspx?item={0}", e.ItemID))
    End Sub

    Protected Sub CartView1_CheckoutClick(sender As Object, e As CartEventArgs)
        If e.IsKit Then
            Response.Redirect("~/Checkout.aspx?tab=1&kit=1")
        Else
            Response.Redirect("~/Checkout.aspx?tab=1")
        End If
    End Sub

    Protected Sub CartView1_ContinueShoppingClick(sender As Object, e As CartEventArgs)
        If Request.QueryString("ReturnUrl") IsNot Nothing Then
            Response.Redirect(Request.QueryString("ReturnUrl"))
        Else
            Response.Redirect("~/Catalog.aspx?tabid=0&h1=1&cid=1")
        End If
    End Sub
End Class