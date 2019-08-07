Imports LNF.Repository
Imports sselStore.AppCode.BLL

Public Class KitOrder
    Public SOID As Integer
    Public ClientID As Integer
End Class

Public Class StoreOrder
    Private _Loaded As Boolean

    Public Property SOID As Integer
    Public Property ClientID As Integer
    Public Property CreationDate As DateTime
    Public Property Status As String
    Public Property StatusChangeDate As DateTime
    Public Property AccountID As Integer
    Public Property AccountName As String
    Public Property InventoryLocationID As Integer

    Public ReadOnly Property Loaded As Boolean
        Get
            Return _Loaded
        End Get
    End Property

    Public Sub Load(ByRef reader As ExecuteReaderResult)
        If reader.Read() Then
            SOID = Convert.ToInt32(reader(DBConstants.STORE_ORDER_SOID))
            ClientID = Convert.ToInt32(reader(DBConstants.STORE_ORDER_CLIENTID))
            CreationDate = Convert.ToDateTime(reader(DBConstants.STORE_ORDER_CREATIONDATE))
            Status = reader(DBConstants.STORE_ORDER_STATUS).ToString()
            StatusChangeDate = Convert.ToDateTime(reader(DBConstants.STORE_ORDER_STATUSCHANGEDATE))
            AccountID = Convert.ToInt32(reader(DBConstants.STORE_ORDER_ACCOUNTID))
            AccountName = reader(DBConstants.STORE_ORDER_ACCOUNTNAME).ToString()

            If reader(DBConstants.STORE_ORDER_INVENTORY_LOCATIONID) IsNot DBNull.Value Then
                _InventoryLocationID = Convert.ToInt32(reader(DBConstants.STORE_ORDER_INVENTORY_LOCATIONID))
            Else
                _InventoryLocationID = 1
            End If

            _Loaded = True
        End If

        reader.Close()
    End Sub

    Public Shared Function Create(soid As Integer) As StoreOrder
        Dim result As New StoreOrder()

        Using reader As ExecuteReaderResult = StoreOrderManager.GetStoreOrderBySOID(soid)
            result.Load(reader)
        End Using

        Return result
    End Function
End Class