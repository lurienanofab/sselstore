Imports LNF.Repository

Namespace BLL
    Public Class KitManager
        Public Shared Function GetAllKits() As DataView
            Return DA.Command().FillDataTable("sselMAS.dbo.Kit_Select").DefaultView
        End Function

        Public Shared Function GetAllItemsByKitID(ByVal kitId As Integer) As StoreOrderItems
            Dim items As New StoreOrderItems
            Dim row As DataRow
            Dim itemId, priceId As Integer
            Dim dt As DataTable = DA.Command().Param("KitID", kitId).FillDataTable("sselMAS.dbo.KitItem_Select")
            For Each in_row As DataRow In dt.Rows
                row = items.NewRow()
                itemID = CType(in_row("ItemID"), Integer)
                priceID = PriceManager.GetPriceIDByItemID(itemID)
                row(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = in_row("Quantity")
                row(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemID
                row(DBConstants.STORE_ORDER_DETAIL_SOID) = -1 'tmp soid
                row(DBConstants.STORE_ORDER_DETAIL_PRICEID) = priceID
                row(DBConstants.ITEM_DESCRIPTION) = in_row(DBConstants.ITEM_DESCRIPTION).ToString()
                row(DBConstants.ITEM_MANUFACTURERPN) = in_row(DBConstants.ITEM_MANUFACTURERPN).ToString()
                row(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = PriceManager.GetCurrentItemPriceByPriceID(priceID)
                items.Rows.Add(row)
            Next
            items.RefreshPrice()
            Return items
        End Function

        Public Shared Function InsertKitOrder(ByVal soid As Integer, ByVal clientId As Integer) As Boolean
            Dim result = DA.Command().Param("SOID", soid).Param("ClientID", clientId).ExecuteNonQuery("sselMAS.dbo.KitOrder_Insert")

            If result.Value = GeneralConstants.RET_ADO_SUCCESS Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetAllItemsByKitIDSimple(ByVal kitId As Integer) As DataTable
            Return DA.Command().Param("KitID", kitId).FillDataTable("sselMAS.dbo.KitItem_Select")
        End Function

        Public Shared Function GetRecipientClientIDBySOID(ByVal soid As Integer) As Integer
            Using reader As ExecuteReaderResult = DA.Command().Param("SOID", soid).ExecuteReader("sselMAS.dbo.KitOrder_Select")
                If reader.Read() Then
                    Return Convert.ToInt32(reader("ClientID"))
                Else
                    Return -1
                End If
            End Using
        End Function
    End Class
End Namespace