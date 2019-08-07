Imports System.Web
Imports LNF.Repository
Imports LNF.Web

Namespace BLL
    Public Class CartManager
        ''' <summary>
        ''' Get the cart object for current user  
        ''' </summary>
        ''' <param name="ClientID"></param>
        ''' <returns>A Cart and its associated items</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCart(ByVal clientId As Integer) As StoreOrderItems
            Using reader As ExecuteReaderResult = DA.Command().Param("ClientID", clientId).Param("Status", AppConstants.STORE_ORDER_STATUS_INCART).ExecuteReader("sselMAS.dbo.StoreOrderDetail_SelectInCartOrderByClientID")

                'We load all the items associated with this particular InCart StoreOrder of this particular user
                Dim cart As New StoreOrderItems
                Dim priceID, quantity As Integer
                Dim linetotal, unitprice As Double
                Dim dataRow As DataRow

                While reader.Read()
                    priceID = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_PRICEID))
                    unitprice = PriceManager.GetCurrentItemPriceByPriceID(priceID)
                    quantity = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_QUANTITY))
                    dataRow = cart.NewRow()
                    dataRow(DBConstants.STORE_ORDER_DETAIL_SODID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_SODID))
                    dataRow(DBConstants.STORE_ORDER_DETAIL_SOID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_SOID))
                    dataRow(DBConstants.STORE_ORDER_DETAIL_ITEMID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_ITEMID))
                    dataRow(DBConstants.ITEM_DESCRIPTION) = reader(DBConstants.ITEM_DESCRIPTION).ToString()
                    dataRow(DBConstants.STORE_ORDER_DETAIL_PRICEID) = priceID
                    dataRow(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = unitprice
                    dataRow(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = quantity
                    linetotal = unitprice * quantity
                    dataRow(DBConstants.STORE_ORDER_DETAIL_LINETOTAL) = linetotal
                    dataRow(DBConstants.ITEM_MANUFACTURERPN) = reader(DBConstants.ITEM_MANUFACTURERPN).ToString()
                    cart.Rows.Add(dataRow)
                    cart.ItemCount += 1
                    cart.TotalPrice += linetotal
                End While

                reader.Close()

                Return cart
            End Using
        End Function

        Public Shared Function GetCart(name As String) As StoreOrderItems
            Dim key As String = GetSessionKey(name)
            Dim items As StoreOrderItems = CType(HttpContext.Current.Session(key), StoreOrderItems)

            If items Is Nothing Then
                Return New StoreOrderItems()
            End If

            items.RefreshPrice()

            Return items
        End Function

        Public Shared Sub RemoveCart(name As String)
            Dim key As String = GetSessionKey(name)
            HttpContext.Current.Session.Remove(key)
        End Sub

        Public Shared Function RemoveItem(ByVal sodid As Integer) As Boolean
            Return DA.Command() _
                .Param("Action", "BySODID") _
                .Param("SODID", sodid) _
                .ExecuteNonQuery("sselMAS.dbo.StoreOrderDetail_Delete").Value = GeneralConstants.RET_ADO_SUCCESS
        End Function

        Public Shared Function CheckQuantity(ByVal itemId As Integer, ByVal newQuantity As Integer, ByRef stockQty As Integer) As Boolean
            stockQty = DA.Command() _
                .Param("Action", "QuantityChecking") _
                .Param("ItemID", itemId) _
                .Param("Quantity", newQuantity) _
                .Param("StockAvailable", stockQty, ParameterDirection.Output) _
                .ExecuteNonQuery("sselMAS.dbo.StoreOrderDetail_UpdateItemQuantity") _
                .GetParamValue(Of Integer)("StockAvailable")

            Return stockQty = -1
        End Function

        Public Shared Function AddItems(ByVal item As StoreOrderItems, ByVal soid As Integer) As Boolean

            Dim flag As Boolean = False

            For Each row As DataRow In item.Rows
                Dim parameters As New Dictionary(Of String, Object) From {
                    {"SOID", soid},
                    {"ItemID", row(DBConstants.STORE_ORDER_DETAIL_ITEMID)},
                    {"Quantity", row(DBConstants.STORE_ORDER_DETAIL_QUANTITY)},
                    {"PriceID", row(DBConstants.STORE_ORDER_DETAIL_PRICEID)}
                }

                If DA.Command().Param(parameters).ExecuteNonQuery("sselMAS.dbo.StoreOrderDetail_Insert").Value <> GeneralConstants.RET_ADO_SUCCESS Then
                    flag = True
                    Exit For
                End If
            Next

            If flag Then
                Return False
            Else
                Return True
            End If

        End Function

        Public Shared Function DeleteCurrentCart(ByVal clientId As Integer) As Boolean
            Return DA.Command() _
                .Param("Action", "CartByClientID") _
                .Param("ClientID", clientId) _
                .ExecuteNonQuery("sselMAS.dbo.StoreOrder_Delete").Value >= GeneralConstants.RET_ADO_DELETE_SUCCESS
        End Function

        Public Shared Function GetSessionKey(name As String) As String
            Return String.Format("{0}#{1}", name, CurrentUser.GetCurrentUserName())
        End Function

        Public Shared Function AddItemToCart(name As String, soid As Integer, itemId As Integer, price As Double, desc As String, mpn As String, quantity As Integer, ByRef message As String) As Boolean
            message = String.Empty

            Dim items As StoreOrderItems = GetCart(name)

            Dim isSecItem As Boolean = ItemManager.IsSecurityItem(itemId)

            If isSecItem AndAlso quantity > 1 Then
                message = "There is a one quantity limit on security items."
                Return False
            End If

            Dim rows As DataRow() = items.Select(String.Format("ItemID = {0}", itemId))
            Dim row As DataRow = Nothing
            If rows.Length > 0 Then
                For Each dr As DataRow In items.Rows
                    If dr.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId Then
                        row = dr
                    End If
                Next

                If Not isSecItem Then
                    row.SetField(DBConstants.STORE_ORDER_DETAIL_QUANTITY, row.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_QUANTITY) + quantity)  'Initial Quantity is always 1
                Else
                    message = "This item has already been added to your cart. There is a one quantity limit on security items."
                    Return False
                End If
            Else
                row = items.NewRow()
                row(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = quantity
                row(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId
                row(DBConstants.STORE_ORDER_DETAIL_SOID) = soid
                row(DBConstants.STORE_ORDER_DETAIL_PRICEID) = PriceManager.GetPriceIDByItemID(itemId)
                row(DBConstants.ITEM_DESCRIPTION) = desc
                row(DBConstants.ITEM_MANUFACTURERPN) = mpn
                row(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = price
                items.Rows.Add(row)
            End If

            row(DBConstants.STORE_ORDER_DETAIL_LINETOTAL) = price * quantity

            Dim key As String = GetSessionKey(name)
            HttpContext.Current.Session(key) = items

            Return True
        End Function
    End Class
End Namespace