Public Class StockQtyViolation
    Inherits DataTable

    Public Sub New()
        Dim columns As DataColumnCollection = Me.Columns
        'Item table related
        columns.Add(DBConstants.ITEM_DESCRIPTION, Type.GetType("System.String"))
        columns.Add(DBConstants.ITEM_STOCKQUANTITY, Type.GetType("System.Int32"))

        'StoreOrderDetail table related
        'columns.Add(STORE_ORDER_DETAIL_SODID, Type.GetType("System.Int32"))
        'columns.Add(STORE_ORDER_DETAIL_SOID, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_QUANTITY, Type.GetType("System.Int32"))
    End Sub

    Public Sub Add(ByVal desc As String, ByVal stkQty As Integer, ByVal currentQty As Integer)
        Dim drow As DataRow = MyBase.NewRow()
        drow(DBConstants.ITEM_DESCRIPTION) = desc
        drow(DBConstants.ITEM_STOCKQUANTITY) = stkQty
        drow(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = currentQty
        MyBase.Rows.Add(drow)
    End Sub
End Class
