Imports sselStore.AppCode

Public Class OrderReceipt
    Inherits StorePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblSOID.Text = Request.QueryString("soid")
        Session.Remove("CheckoutSOID") 'For security reason
        litOrderReceiptMessage.Text = MessageUtility.GetOrderReceiptMessage()
    End Sub
End Class