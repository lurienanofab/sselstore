Imports LNF
Imports sselStore.AppCode.BLL

Namespace Ajax
    Public Class Items
        Implements IHttpHandler, IReadOnlySessionState

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim itemOption As String = If(String.IsNullOrEmpty(context.Request("item-option")), "web-store-items", context.Request("item-option"))

            context.Response.ContentType = "application/json"
            context.Response.Write(ServiceProvider.Current.Serialization.Json.SerializeObject(ItemLookup(itemOption)))

        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Function ItemLookup(itemOption As String) As Object
            Dim dv As DataView = ItemManager.GetAllItems(itemOption = "web-store-items", True)
            Dim list As List(Of Object) = New List(Of Object)()

            For Each drv As DataRowView In dv
                list.Add(CreateDataTableItem(drv))
            Next

            Return New With {.data = list.ToArray()}
        End Function


        Function CreateDataTableItem(drv As DataRowView) As Object
            Return New With { _
                .ItemID = drv("ItemID"), _
                .ManufacturerPN = drv("ManufacturerPN"), _
                .Description = drv("Description"), _
                .Notes = drv("Notes"), _
                .StockQuantity = drv("StockQuantity"), _
                .StockReserve = drv("StockReserve"), _
                .StockAvailable = drv("StockAvailable")
            }
        End Function
    End Class
End Namespace