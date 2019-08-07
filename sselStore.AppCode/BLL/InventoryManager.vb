Imports LNF.Repository

Namespace BLL
    Public Class InventoryManager
        Public Shared Function GetAllInventory(ByVal webOnly As Boolean) As DataView
            Return DA.Command().Param("WebStoreOnly", webOnly).FillDataTable("sselMAS.dbo.Item_SelectAllInventoryData").DefaultView
        End Function

        Public Shared Function GetItemsStockInfo(ByVal soid As Integer, ByVal webOnly As Boolean) As DataTable
            Return DA.Command() _
                .Param("WebStoreOnly", webOnly) _
                .Param("SOID", soid) _
                .FillDataTable("sselMAS.dbo.Item_CheckStockStatus")
        End Function

        Public Shared Function Search(ByVal query As String) As DataView
            Return ItemManager.Search(query, "Inventory")
        End Function

        ' Used by ObjectDataSource server tags. Do not change parameter names.
        Public Shared Function UpdateInventoryItem(ByVal ItemID As Integer, ByVal StockQuantity As Integer, ByVal StockOnOrder As Integer, ByVal OrderDate As Date, ByVal EstimatedArrivalDate As Date) As Boolean
            Return DA.Command() _
                .Param("ItemID", ItemID) _
                .Param("StockQuantity", StockQuantity) _
                .Param("StockOnOrder", StockOnOrder) _
                .Param("OrderDate", OrderDate <> Nothing, OrderDate) _
                .Param("EstimatedArrivalDate", EstimatedArrivalDate <> Nothing, EstimatedArrivalDate) _
                .ExecuteNonQuery("sselMAS.dbo.Item_InventoryUpdate").Value = GeneralConstants.RET_ADO_SUCCESS
        End Function

        Public Shared Function GetCertainItems(ByVal soid As Integer) As DataSet
            Return DA.Command().Param("SOID", soid).FillDataSet("sselMAS.dbo.Item_SelectCertainInventoryData")
        End Function

        Public Shared Function UpdateItemStockQuantity(ByVal itemId As Integer, ByVal deductedQty As Integer) As Boolean
            Return DA.Command() _
                .Param("ItemID", itemId) _
                .Param("DeductedQuantity", deductedQty) _
                .ExecuteNonQuery("sselMAS.dbo.Item_InventoryUpdateStockQuantity").Value = GeneralConstants.RET_ADO_SUCCESS
        End Function
    End Class
End Namespace