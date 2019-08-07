Public Class StoreOrderItems
    Inherits DataTable

    Dim _ItemCount As Integer
    Dim _TotalPrice As Double

    Public Property ItemCount() As Integer
        Get
            ItemCount = _ItemCount
        End Get
        Set(ByVal value As Integer)
            _ItemCount = value
        End Set
    End Property

    Public Property TotalPrice() As Double
        Get
            TotalPrice = _TotalPrice
        End Get
        Set(ByVal value As Double)
            _TotalPrice = value
        End Set
    End Property

    Public Sub RefreshPrice()
        Dim quantity As Integer
        Dim linetotal, unitprice As Double

        _TotalPrice = 0
        For Each dataRow As DataRow In Me.AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted)
            unitprice = dataRow.Field(Of Double)(DBConstants.STORE_ORDER_DETAIL_UNITPRICE)
            quantity = dataRow.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_QUANTITY)
            linetotal = unitprice * quantity
            dataRow.SetField(DBConstants.STORE_ORDER_DETAIL_LINETOTAL, linetotal)
            _TotalPrice += linetotal
        Next
    End Sub

    Public Sub New()
        _ItemCount = 0
        Dim columns As DataColumnCollection = Me.Columns
        'StoreOrderDetail table related
        columns.Add(DBConstants.STORE_ORDER_DETAIL_SODID, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_SOID, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_ITEMID, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_PRICEID, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_QUANTITY, Type.GetType("System.Int32"))
        columns.Add(DBConstants.STORE_ORDER_DETAIL_LINETOTAL, Type.GetType("System.Double")) 'not included in table
        columns.Add(DBConstants.STORE_ORDER_DETAIL_UNITPRICE, Type.GetType("System.Double")) 'get this from PriceID

        'Item table related
        columns.Add(DBConstants.ITEM_MANUFACTURERPN, Type.GetType("System.String"))
        columns.Add(DBConstants.ITEM_DESCRIPTION, Type.GetType("System.String"))
    End Sub

    Public Shadows Function Copy() As StoreOrderItems
        Dim dt As DataTable = MyBase.Copy()
        Dim result As New StoreOrderItems()
        For Each dr As DataRow In dt.Rows
            If dr.RowState <> DataRowState.Deleted Then
                Dim ndr As DataRow = result.NewRow()
                For c As Integer = 0 To dt.Columns.Count - 1
                    ndr(c) = dr(c)
                Next
                result.Rows.Add(ndr)
            End If
        Next
        result.RefreshPrice()
        Return result
    End Function
End Class