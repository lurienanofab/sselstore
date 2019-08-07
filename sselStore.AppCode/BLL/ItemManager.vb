Imports LNF.Repository

Namespace BLL
    Public Class ItemManager
        Public Shared Function GetAllItems(ByVal webitem As Boolean, ByVal lookup As Boolean) As DataView
            Dim sp As String

            If lookup Then
                sp = "sselMAS.dbo.Item_SelectAllForItemIDLookup"
            Else
                sp = "sselMAS.dbo.Item_SelectAll"
            End If

            Return DA.Command().Param("WebStoreOnly", webitem).FillDataTable(sp).DefaultView
        End Function

        Public Shared Function GetParentCatID(ByVal catID As Integer) As Integer
            Return DA.Command() _
                .Param("Action", "ParentCatID") _
                .Param("CatID", catID) _
                .ExecuteScalar(Of Integer)("sselMAS.dbo.Category_Select").Value
        End Function

        Public Shared Function UpdateItem(ByVal item As Item) As Boolean
            Return DA.Command() _
                .Param("ItemID", item.ItemID) _
                .Param("Description", item.Description) _
                .Param("Notes", item.Notes) _
                .Param("CatID", item.CategoryID) _
                .Param("ManufacturerPN", item.ManufacturerPN) _
                .Param("StoreDisplay", item.StoreDisplay) _
                .Param("CrossCharge", item.CrossCharge) _
                .Param("SearchKeyWords", item.SearchKeyWords) _
                .Param("StockQuantity", item.StockQuantity) _
                .Param("MinStockQuantity", item.MinStockQuantity) _
                .Param("Active", item.Active) _
                .ExecuteNonQuery("sselMAS.dbo.Item_Update").Value >= GeneralConstants.RET_ADO_SUCCESS
        End Function

        Public Shared Function InsertItem(ByVal item As Item, ByRef itemId As Integer) As Boolean
            Dim cmd As IDataCommand = DA.Command()

            With cmd
                .Param("ItemID", itemId, ParameterDirection.Output)
                .Param("Description", item.Description)
                .Param("Notes", item.Notes)
                .Param("CatID", item.CategoryID)
                .Param("ManufacturerPN", item.ManufacturerPN)
                .Param("Active", item.Active)
                .Param("StoreDisplay", item.StoreDisplay)
                .Param("CrossCharge", item.CrossCharge)
                .Param("StockQuantity", item.StockQuantity)
                .Param("MinStockQuantity", item.MinStockQuantity)
                .Param("SearchKeyWords", item.SearchKeyWords)
            End With

            Dim result As ExecuteNonQueryResult = cmd.ExecuteNonQuery("sselMAS.dbo.Item_Insert")

            If result.Value = GeneralConstants.RET_ADO_SUCCESS Then
                itemId = result.GetParamValue(Of Integer)("ItemID")
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function Search(ByVal query As String, ByVal action As String) As DataView
            Dim cmd As IDataCommand = DA.Command().Param("Query", query).Param("Action", action)

            If query.Substring(query.Length - 1, 1) = "s" Then
                cmd.Param("QuerySingular", query.Substring(0, query.Length - 1))
            End If

            Return cmd.FillDataTable("sselMAS.dbo.Item_Search").DefaultView
        End Function

        Public Shared Function SearchForItemIDLookup(ByVal query As String, ByVal webitem As Boolean) As DataView
            Return DA.Command().Param("WebStoreOnly", webitem).Param("Query", query).FillDataTable("sselMAS.dbo.Item_SelectAllForItemIDLookup").DefaultView
        End Function

        Public Shared Function DeleteItem(ByVal itemId As Integer) As Boolean
            Return DA.Command().Param("ItemID", itemId).ExecuteNonQuery("sselMAS.dbo.Item_Delete").Value = GeneralConstants.RET_ADO_SUCCESS
        End Function

        Public Shared Function GetItemByItemID(ByVal itemId As Integer) As ExecuteReaderResult
            Return DA.Command().Param("ItemID", itemId).ExecuteReader("sselMAS.dbo.Item_SelectItemByItemID")
        End Function

        Public Shared Function CheckStockQtyErrorByItemID(ByVal itemID As Integer, ByVal newQuantity As Integer, ByRef stockQty As Integer, Optional ByVal sodid As Integer = 0) As Boolean
            Dim cmd As IDataCommand = DA.Command()

            With cmd
                .Param("ItemID", itemID)
                .Param("SODID", sodid)
                .Param("Quantity", newQuantity)
                .Param("StockAvailable", stockQty, ParameterDirection.Output)
                .Param("Return", -1, ParameterDirection.Output)
            End With

            Dim result As ExecuteNonQueryResult = cmd.ExecuteNonQuery("sselMAS.dbo.Item_QuantityChecking")

            If Not result.Value = GeneralConstants.RET_ADO_SUCCESS Then
                'Error handling
            End If

            If result.GetParamValue(Of Integer)("Return") > 0 Then
                Return True
            Else
                stockQty = result.GetParamValue(Of Integer)("StockAvailable")
                Return False
            End If
        End Function

        Public Shared Function SecurityItems() As DataTable
            Return DA.Command(CommandType.Text).FillDataTable("SELECT si.SecurityItemID, si.ItemID, i.[Description], si.Active FROM sselMAS.dbo.SecurityItem si INNER JOIN Item i ON si.ItemID = i.ItemID")
        End Function

        Public Shared Function AddSecurityItem(ByVal itemId As Integer) As Boolean
            Dim doAdd As Boolean = False

            Dim count As Integer = DA.Command(CommandType.Text).Param("ItemID", itemId).ExecuteScalar(Of Integer)("SELECT COUNT(*) AS 'COUNT' FROM sselMAS.dbo.SecurityItem WHERE ItemID = @ItemID").Value

            If count = 0 Then
                doAdd = True
            End If

            If doAdd Then
                DA.Command(CommandType.Text).Param("ItemID", itemId).ExecuteNonQuery("INSERT sselMAS.dbo.SecurityItem (ItemID, Active) VALUES (@ItemID, 1)")
            End If

            Return doAdd
        End Function

        Public Shared Function UpdateSecurityItem(ByVal itemID As Integer, ByVal active As Boolean) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("ItemID", itemID) _
                .Param("Active", active) _
                .ExecuteNonQuery("UPDATE sselMAS.dbo.SecurityItem SET Active = @Active WHERE ItemID = @ItemID").Value
        End Function

        Public Shared Function IsSecurityItem(ByVal itemId As Integer) As Boolean
            Return DA.Command(CommandType.Text) _
                .Param("ItemID", itemId) _
                .ExecuteScalar(Of Integer)("SELECT COUNT(*) AS 'COUNT' FROM sselMAS.dbo.SecurityItem WHERE ItemID = @ItemID").Value > 0
        End Function
    End Class
End Namespace