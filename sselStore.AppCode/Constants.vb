Public Class AppConstants
    'Store order status
    Public Const STORE_ORDER_STATUS_DEFAULT As String = "Open"
    Public Const STORE_ORDER_STATUS_ALL As String = "All"
    Public Const STORE_ORDER_STATUS_INCART As String = "InCart"
    Public Const STORE_ORDER_STATUS_OPEN As String = "Open"
    Public Const STORE_ORDER_STATUS_CANCELLED As String = "Cancelled"
    Public Const STORE_ORDER_STATUS_CLOSED As String = "Closed"
    Public Const STORE_ORDER_STATUS_FULFILLED As String = "Fulfilled"

    'Session variable names
    Public Const SESSION_CLIENT_ID As String = "ClientID"

    'Miscellaneous
    Public Const NUM_OF_DIGIT_ITEMNO As Integer = 6 'used for appending zeros on item image name
End Class

''' <summary>
''' Constants that map to the database table and column names
''' </summary>
''' <remarks></remarks>
Public Class DBConstants
    'Table: StoreOrder
    Public Const STORE_ORDER_SOID As String = "SOID"
    Public Const STORE_ORDER_CLIENTID As String = "ClientID"
    Public Const STORE_ORDER_ACCOUNTID As String = "AccountID"
    Public Const STORE_ORDER_ACCOUNTNAME As String = "Name"
    Public Const STORE_ORDER_CREATIONDATE As String = "CreationDate"
    Public Const STORE_ORDER_STATUS As String = "Status"
    Public Const STORE_ORDER_STATUSCHANGEDATE As String = "StatusChangeDate"
    Public Const STORE_ORDER_TOTALPRICE As String = "TotalPrice" 'not in table schema, derived from other columns
    Public Const STORE_ORDER_INVENTORY_LOCATIONID As String = "InventoryLocationID"

    'Table: StoreOrderDetail
    Public Const STORE_ORDER_DETAIL_SODID As String = "SODID"
    Public Const STORE_ORDER_DETAIL_PRICEID As String = "PriceID"
    Public Const STORE_ORDER_DETAIL_QUANTITY As String = "Quantity"
    Public Const STORE_ORDER_DETAIL_SOID As String = "SOID"
    Public Const STORE_ORDER_DETAIL_ITEMID As String = "ItemID"
    Public Const STORE_ORDER_DETAIL_LINETOTAL As String = "LineTotal" 'not in table schema, derived from other columns
    Public Const STORE_ORDER_DETAIL_UNITPRICE As String = "UnitPrice"

    'Table: Item
    Public Const ITEM_ITEMID As String = "ItemID"
    Public Const ITEM_DESCRIPTION As String = "Description"
    Public Const ITEM_CATID As String = "CatID"
    Public Const ITEM_MANUFACTURERPN As String = "ManufacturerPN"
    Public Const ITEM_STOCKQUANTITY As String = "StockQuantity"
    Public Const ITEM_MINSTOCKQUANTITY As String = "MinStockQuantity"
    Public Const ITEM_STOCKRESERVE As String = "StockReserve"
    Public Const ITEM_STOCKONORDER As String = "StockOnOrder"
    Public Const ITEM_ORDERDATE As String = "OrderDate"
    Public Const ITEM_ESTIMATEDARRIVALDATE As String = "EstimatedArrivalDate"

    'Table: Category
    Public Const CATEGORY_CATID As String = "CatID"
    Public Const CATEGORY_CATNAME As String = "CatName"
End Class

Public Class GeneralConstants
    'Return Values
    Public Const RET_ADO_SUCCESS As Integer = 1 'Check for Insert/Delete/Update operation
    Public Const RET_ADO_DELETE_SUCCESS As Integer = 0 'Sometimes it's possible for deletion operation to returns 0
    Public Const RET_ADO_FAILURE As Integer = -1

    'Kit Category ID
    Public Const KIT_ID As Integer = -99
End Class

Public Class RegexConstants
    Public Const REGEX_ITEMID As String = "\b[1-9][0-9]{0,3}\b" '1 to 9999 only
    Public Const REGEX_CATID As String = "\b[1-9][0-9]{0,2}\b" '1 to 999 only
    Public Const REGEX_SOID As String = "\b[1-9][0-9]{0,5}\b" '1 to 999999 only
    Public Const REGEX_STEPS As String = "\b[1-9]\b" '1 to 9 only
    Public Const REGEX_ORDERTYPE As String = "[" + AppConstants.STORE_ORDER_STATUS_ALL + "]|[" + AppConstants.STORE_ORDER_STATUS_INCART + "]|[" + AppConstants.STORE_ORDER_STATUS_OPEN + "]|[" + AppConstants.STORE_ORDER_STATUS_CANCELLED + "]|[" + AppConstants.STORE_ORDER_STATUS_CLOSED + "]|[" + AppConstants.STORE_ORDER_STATUS_FULFILLED + "]"
End Class