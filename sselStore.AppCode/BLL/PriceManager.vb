Imports LNF.Repository
Imports LNF.Store

Namespace BLL
    Public Class PriceManager
        Public Shared Function GetPriceIDByItemID(ByVal itemId As Integer) As Integer
            Using reader As ExecuteReaderResult = DA.Command().Param("ItemID", itemId).ExecuteReader("sselMAS.dbo.Price_GetPriceIDByItemID")
                If reader.Read() Then
                    Return reader.Value("PriceID", 0)
                Else
                    Return 0
                End If
            End Using
        End Function

        Public Shared Function GetCurrentItemPriceByItemID(ByVal itemId As Integer) As Double
            Return GetPrice(itemId, "Price_GetPackagePriceByItemID", "ItemID")
        End Function

        Public Shared Function GetCurrentItemPriceByPriceID(ByVal priceId As Integer) As Decimal
            Return GetPrice(priceId, "Price_GetPackagePriceByPriceID", "PriceID")
        End Function

        Public Shared Function GetCurrentItemPriceByPriceIDAndStoreOrder(ByVal priceId As Integer, ByVal soid As Integer) As Double
            Dim so As Store.StoreOrder = DA.Current.Single(Of Store.StoreOrder)(soid)
            Return PriceUtility.ApplyStoreMultiplier(GetCurrentItemPriceByPriceID(priceId), so.Account.AccountID)
        End Function

        Private Shared Function GetPrice(ByVal objectId As Integer, ByVal procedureName As String, ByVal paramName As String) As Decimal
            Dim dt As DataTable = DA.Command().Param(paramName, objectId).FillDataTable($"sselMAS.dbo.{procedureName}")
            If dt.Rows.Count > 0 Then
                Return Convert.ToDecimal(dt.Rows(0)("UnitPrice"))
            Else
                Return 0D
            End If
        End Function

        Public Shared Function GetItemHistoricalInfo(ByVal itemId As Integer, ByVal period As DateTime) As DataRow
            Dim dt As DataTable = DA.Command(CommandType.Text) _
                .Param("Period", period) _
                .Param("ItemID", itemId) _
                .FillDataTable("SELECT * FROM sselMAS.dbo.udf_ItemHistoricalInfo(@Period, @ItemID)")

            Return dt.AsEnumerable().FirstOrDefault()
        End Function

        Public Shared Function Packages(ByVal itemId As Integer) As DataTable
            Return DA.Command(CommandType.Text).Param("ItemID", itemId).FillDataTable("SELECT * FROM sselMAS.dbo.Package WHERE ItemID = @ItemID")
        End Function

        Public Shared Function UpdatePackage(ByVal packageId As Integer, ByVal description As String, ByVal baseQty As Integer, ByVal active As Boolean) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("PackageID", packageId) _
                .Param("BaseQMultiplier", baseQty) _
                .Param("Descriptor", description) _
                .Param("Active", active) _
                .ExecuteNonQuery("UPDATE sselMAS.dbo.Package SET BaseQMultiplier = @BaseQMultiplier, Descriptor = @Descriptor, Active = @Active WHERE PackageID = @PackageID").Value
        End Function

        Public Shared Function AddPackage(ByVal itemId As Integer, ByVal description As String, ByVal baseQty As Integer) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("ItemID", itemId) _
                .Param("BaseQMultiplier", baseQty) _
                .Param("Descriptor", description) _
                .ExecuteNonQuery("INSERT sselMAS.dbo.Package (ItemID, BaseQMultiplier, Descriptor, Active) VALUES (@ItemID, @BaseQMultiplier, @Descriptor, 1)").Value
        End Function

        Public Shared Function SelectVendorPackagesByPackageID(ByVal packageId As Integer) As DataTable
            Return DA.Command() _
                .Param("Action", "SelectVendorPackagesByPackageID") _
                .Param("PackageID", packageId) _
                .FillDataTable("sselMAS.dbo.VendorPackage_Select")
        End Function

        Public Shared Function UpdateVendorPackage(ByVal vendorPackageId As Integer, ByVal vendorId As Integer, ByVal vendorPN As String, ByVal active As Boolean) As Integer
            Return DA.Command() _
                .Param("Action", "UpdateVendorPackage") _
                .Param("VendorPackageID", vendorPackageId) _
                .Param("VendorID", vendorId) _
                .Param("VendorPN", vendorPN) _
                .Param("Active", active) _
                .ExecuteNonQuery("sselMAS.dbo.VendorPackage_Update").Value
        End Function

        Public Shared Function AddVendorPackage(ByVal packageId As Integer, ByVal vendorId As Integer, ByVal vendorPN As String) As Integer
            Return DA.Command() _
                .Param("Action", "AddVendorPackage") _
                .Param("VendorID", vendorId) _
                .Param("PackageID", packageId) _
                .Param("VendorPN", vendorPN) _
                .ExecuteNonQuery("sselMAS.dbo.VendorPackage_Insert").Value
        End Function

        Public Shared Function Prices(ByVal vendorPackageId As Integer) As DataTable
            Return DA.Command(CommandType.Text) _
                .Param("VendorPackageID", vendorPackageId) _
                .FillDataTable("SELECT * FROM sselMAS.dbo.Price WHERE VendorPackageID = @VendorPackageID ORDER BY DateActive")
        End Function

        Public Shared Function AddPrice(ByVal vendorPackageId As Integer, ByVal packageCost As Double, ByVal packageMarkup As Double, ByVal priceBreakQty As Integer) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("VendorPackageID", vendorPackageId) _
                .Param("PriceBreakQuantity", priceBreakQty) _
                .Param("PackageCost", packageCost) _
                .Param("PackageMarkup", packageMarkup) _
                .ExecuteNonQuery("INSERT sselMAS.dbo.Price (VendorPackageID, PriceBreakQuantity, PackageCost, PackageMarkup, DateActive) VALUES (@VendorPackageID, @PriceBreakQuantity, @PackageCost, @PackageMarkup, GETDATE())").Value
        End Function

        Public Shared Sub ApplyPriceMultiplier(items As StoreOrderItems, accountId As Integer)
            For Each dr As DataRow In items.Rows
                Dim unitPrice As Decimal = Convert.ToDecimal(dr(DBConstants.STORE_ORDER_DETAIL_UNITPRICE))
                Dim multiplierUnitPrice As Double = PriceUtility.ApplyStoreMultiplier(unitPrice, accountId)
                dr.SetField(DBConstants.STORE_ORDER_DETAIL_UNITPRICE, multiplierUnitPrice)
                items.RefreshPrice()
            Next
        End Sub
    End Class
End Namespace