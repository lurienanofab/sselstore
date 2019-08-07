Imports LNF.Printing
Imports LNF.Repository
Imports sselStore.AppCode

Namespace Admin
    Public Class PrintOrder
        Inherits Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim soid As Integer
            Dim status As String
            Dim test As Boolean

            If Not String.IsNullOrEmpty(Request.QueryString("soid")) Then
                If Integer.TryParse(Request.QueryString("soid"), soid) Then
                    PrintOneOrder(soid)
                End If
            ElseIf Not String.IsNullOrEmpty(Request.QueryString("status")) Then
                status = Request.QueryString("status")
                test = If(String.IsNullOrEmpty(Request.QueryString("test")), False, Boolean.Parse(Request.QueryString("test")))
                PrintOrdersByStatus(status, test)
            End If

        End Sub

        Private Sub PrintOneOrder(soid As Integer)
            Dim order As Store.StoreOrder = DA.Current.Single(Of Store.StoreOrder)(soid)

            If order Is Nothing Then
                litMessage.Text = String.Format("<div class=""stock-error"">Unable to find Order #{0}</div>", soid)
                Return
            End If

            Dim buffer As Byte() = StorePrintManager.GetStoreOrderPdf(order)
            WritePdf(buffer)
        End Sub

        Private Sub PrintOrdersByStatus(status As String, test As Boolean)
            Dim query As Store.StoreOrder() = DA.Current.Query(Of Store.StoreOrder)().Where(Function(x) x.Status.ToLower() = status).ToArray()

            If query.Length = 0 Then
                litMessage.Text = String.Format("<div class=""stock-error"">No {0} orders found</div>", status)
                Return
            End If

            Dim list As List(Of Byte()) = New List(Of Byte())()

            For Each item As Store.StoreOrder In query
                list.Add(StorePrintManager.GetStoreOrderPdf(item))
                If Not test AndAlso status = "open" Then
                    item.Status = AppConstants.STORE_ORDER_STATUS_FULFILLED
                    item.StatusChangeDate = DateTime.Now
                End If
            Next

            Dim combined As Byte() = PdfUtility.Combine(list)

            WritePdf(combined)
        End Sub

        Private Sub WritePdf(buffer As Byte())
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.BinaryWrite(buffer)
            Response.End()
        End Sub
    End Class
End Namespace