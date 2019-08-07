Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class CatalogKit
    Inherits StorePage

    Protected Function GetKitImageName() As String
        Return "kit.jpg"
    End Function

    Protected Sub dlKit_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlKit.ItemCommand
        If e.CommandName = "AddToCart" Then
            AddItemToCart(Convert.ToInt32(e.CommandArgument))
        End If
    End Sub

    Private Sub AddItemToCart(ByVal kitID As Integer)
        Session.Remove("KitCart")
        Dim items As StoreOrderItems = KitManager.GetAllItemsByKitID(kitID)
        Session("KitCart") = items
        Response.Redirect("~/Cart.aspx?tab=1&kit=1")
    End Sub

    Protected Function GetItemList(ByVal kitID As Integer) As String
        Dim ret As String = String.Empty
        Dim dt As DataTable = KitManager.GetAllItemsByKitIDSimple(kitID)
        For Each row As DataRow In dt.Rows
            ret = ret + "<div>" + row.Field(Of String)("Description") + "</div>"
        Next
        Return ret
    End Function
End Class