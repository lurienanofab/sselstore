Imports System.Configuration
Imports LNF.CommonTools
Imports LNF.Models.Data
Imports LNF.Repository

Namespace BLL
    Public Class StoreOrderManager
        Public Shared Function GetStoreOrderList(ByVal clientId As Integer, ByVal orderStatus As String) As DataView
            Return DA.Command() _
                .Param("ClientID", clientId) _
                .Param("Status", orderStatus) _
                .FillDataTable("sselMAS.dbo.StoreOrder_SelectAllByClientIDAndStatus").DefaultView
        End Function

        Public Shared Function GetClosedOrderList(ByVal clientID As Integer, ByVal sDate As Date, ByVal eDate As Date) As DataView
            Return DA.Command() _
                .Param("Action", "GetClosedOrder") _
                .Param("ClientID", clientID) _
                .Param("sDate", sDate) _
                .Param("eDate", eDate) _
                .FillDataTable("sselMAS.dbo.StoreOrder_Select").DefaultView
        End Function

        Public Shared Function GetStoreOrderList(ByVal orderStatus As String) As DataView
            Return DA.Command().Param("Status", orderStatus).FillDataTable("sselMAS.dbo.StoreOrder_SelectAllByStatus").DefaultView
        End Function

        Public Shared Function GetOpenOrderList() As DataView
            Return DA.Command().Param("Status", "KitOrder").FillDataTable("sselMAS.dbo.StoreOrder_SelectAllByStatus").DefaultView
        End Function


        Public Shared Function GetStoreOrderDetail(ByVal soid As Integer) As StoreOrderItems
            Using reader As ExecuteReaderResult = DA.Command().Param("SOID", soid).ExecuteReader("sselMAS.dbo.StoreOrderDetail_SelectBySOID")

                Dim soDetail As New StoreOrderItems()

                Dim dataRow As DataRow
                While reader.Read()
                    Dim priceId As Integer = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_PRICEID))
                    Dim quantity As Integer = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_QUANTITY))
                    dataRow = soDetail.NewRow()
                    dataRow(DBConstants.STORE_ORDER_DETAIL_SODID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_SODID))
                    dataRow(DBConstants.STORE_ORDER_DETAIL_SOID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_SOID))
                    dataRow(DBConstants.STORE_ORDER_DETAIL_ITEMID) = Convert.ToInt32(reader(DBConstants.STORE_ORDER_DETAIL_ITEMID))
                    dataRow(DBConstants.ITEM_DESCRIPTION) = reader(DBConstants.ITEM_DESCRIPTION).ToString()
                    dataRow(DBConstants.STORE_ORDER_DETAIL_PRICEID) = priceId
                    Dim unitPrice As Double = PriceManager.GetCurrentItemPriceByPriceIDAndStoreOrder(priceId, soid)
                    dataRow(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = unitPrice
                    dataRow(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = quantity
                    Dim lineTotal As Double = unitPrice * quantity
                    dataRow(DBConstants.STORE_ORDER_DETAIL_LINETOTAL) = lineTotal
                    dataRow(DBConstants.ITEM_MANUFACTURERPN) = reader(DBConstants.ITEM_MANUFACTURERPN)
                    soDetail.Rows.Add(dataRow)
                    soDetail.TotalPrice += lineTotal
                    soDetail.ItemCount += 1
                End While

                reader.Close()
                soDetail.AcceptChanges() 'sets all DataRowStates to Unchanged so when a quantity is updated it will be Modified not Added
                Return soDetail
            End Using
        End Function

        Public Shared Function GetStoreOrderDetailBySOID(ByVal soid As Integer) As DataSet
            Dim result As New DataSet()
            result.Tables.Add(GetStoreOrderDetail(soid))
            Return result

            'Using dba As New SQLDBAccess("cnSselMAS")
            '    dba.AddParameter("@SOID", soid)
            '    Dim ds As DataSet = dba.FillDataSet("StoreOrderDetail_SelectBySOID")
            '    For Each row As DataRow In ds.Tables(0).Rows
            '        Dim qty As Integer = Convert.ToInt32(row(DBConstants.STORE_ORDER_DETAIL_QUANTITY))
            '        Dim price As Double = PriceManager.GetCurrentItemPriceByPriceIDAndStoreOrder(row.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_PRICEID), soid)
            '        row.SetField(DBConstants.STORE_ORDER_DETAIL_UNITPRICE, price)
            '        row.SetField(DBConstants.STORE_ORDER_DETAIL_LINETOTAL, qty * price)
            '    Next
            '    Return ds
            'End Using
        End Function

        Public Shared Function GetStoreOrderStatus(ByVal showAll As Boolean, ByVal showCart As Boolean, ByVal currentStatus As String) As DataTable
            Dim dt As New DataTable()
            Dim columns As DataColumnCollection = dt.Columns
            columns.Add("Status", GetType(String))

            Dim rows As DataRowCollection = dt.Rows

            'In Store's MyOrder, we don't need to show the InCart Status
            If showCart Then
                rows.Add(AppConstants.STORE_ORDER_STATUS_INCART)
            End If

            'No need to show all in order detail page.  If we have to show all, then every status will be needed for sure
            If showAll Then
                rows.Add(AppConstants.STORE_ORDER_STATUS_ALL)
                rows.Add(AppConstants.STORE_ORDER_STATUS_OPEN)
                rows.Add(AppConstants.STORE_ORDER_STATUS_FULFILLED)
                rows.Add(AppConstants.STORE_ORDER_STATUS_CLOSED)
                rows.Add(AppConstants.STORE_ORDER_STATUS_CANCELLED)
            Else
                'Because there are limits in transition between state, we have to show only logical status accroding to current state
                If currentStatus = AppConstants.STORE_ORDER_STATUS_CLOSED Then
                    rows.Add(AppConstants.STORE_ORDER_STATUS_CLOSED)
                ElseIf currentStatus = AppConstants.STORE_ORDER_STATUS_CANCELLED Then
                    rows.Add(AppConstants.STORE_ORDER_STATUS_CANCELLED)
                ElseIf currentStatus = AppConstants.STORE_ORDER_STATUS_FULFILLED Then
                    rows.Add(AppConstants.STORE_ORDER_STATUS_FULFILLED)
                    rows.Add(AppConstants.STORE_ORDER_STATUS_CLOSED)
                    rows.Add(AppConstants.STORE_ORDER_STATUS_CANCELLED)
                Else 'Open state
                    rows.Add(AppConstants.STORE_ORDER_STATUS_OPEN)
                    rows.Add(AppConstants.STORE_ORDER_STATUS_FULFILLED)
                    rows.Add(AppConstants.STORE_ORDER_STATUS_CANCELLED)
                End If
            End If

            Return dt
        End Function

        Public Shared Function InsertNewOrder(ByVal clientId As Integer, ByVal accountId As Integer, ByRef soid As Integer, ByRef locationId As Integer) As Boolean
            Dim cmd As IDataCommand = DA.Command()

            With cmd
                .Param("@ClientID", clientId)
                .Param("@AccountID", accountId)
                .Param("@SOID", soid, ParameterDirection.Output)
                .Param("@InventoryLocationID", locationId)
            End With

            Dim result As ExecuteNonQueryResult = cmd.ExecuteNonQuery("sselMAS.dbo.StoreOrder_Insert")

            If result.Value = GeneralConstants.RET_ADO_SUCCESS Then
                soid = result.GetParamValue(Of Integer)("SOID")
                If soid = 0 Then
                    'a temporary solution, got to figure out how to read output variable from storeprocedure
                    'Dim tempdb As New SQLDBAccess("cnSselMAS")
                    'tempdb.AddParameter("@ClientID", clientID)
                    'soid = CType(tempdb.ExecuteScalar("StoreOrder_SelectInCartSOID"), Integer)
                    Return False
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GetStoreOrderBySOID(ByVal soid As Integer) As ExecuteReaderResult
            Return DA.Command().Param("SOID", soid).ExecuteReader("sselMAS.dbo.StoreOrder_SelectBySOID")
        End Function

        Public Shared Function UpdateStoreOrder(ByVal so As StoreOrder) As Boolean
            Return DA.Command() _
                .Param("@Action", "Default") _
                .Param("@SOID", so.SOID) _
                .Param("@CreationDate", so.CreationDate) _
                .Param("@Status", so.Status) _
                .Param("@StatusChangeDate", so.StatusChangeDate) _
                .ExecuteNonQuery("sselMAS.dbo.StoreOrder_Update").Value = GeneralConstants.RET_ADO_SUCCESS
        End Function

        'Public Shared Function UpdateStoreOrderDetail(ByVal ds As DataSet) As Boolean
        Public Shared Function UpdateStoreOrderDetail(ds As DataSet) As Boolean
            If ds IsNot Nothing Then

                Dim result As Integer = DA.Command().Update(ds, Sub(cfg)
                                                                    'Update
                                                                    With cfg.Update
                                                                        .SetCommandText("sselMAS.dbo.StoreOrderDetail_UpdateItemQuantity")
                                                                        .AddParameter("SODID", SqlDbType.Int)
                                                                        .AddParameter("Quantity", SqlDbType.Int)
                                                                    End With

                                                                    'Insert
                                                                    With cfg.Insert
                                                                        .SetCommandText("sselMAS.dbo.StoreOrderDetail_Insert")
                                                                        .AddParameter("SOID", SqlDbType.Int)
                                                                        .AddParameter("ItemID", SqlDbType.Int)
                                                                        .AddParameter("Quantity", SqlDbType.Int)
                                                                        .AddParameter("PriceID", SqlDbType.Int)
                                                                    End With

                                                                    'Delete
                                                                    With cfg.Delete
                                                                        .SetCommandText("sselMAS.dbo.StoreOrderDetail_Delete")
                                                                        .AddParameter("SODID", SqlDbType.Int)
                                                                    End With
                                                                End Sub)

                Return result >= GeneralConstants.RET_ADO_SUCCESS
            Else
                Return False
            End If
        End Function

        Public Shared Function ChangeOrderStatus(ByVal soid As Integer, ByVal status As String, Optional ByVal accountId As Integer = -1) As Boolean
            If DA.Command().Param("SOID", soid).Param("Status", status).Param("AccountID", accountId).ExecuteNonQuery("sselMAS.dbo.StoreOrder_ChangeStatus").Value = GeneralConstants.RET_ADO_SUCCESS Then
                If Not accountId = -1 Then
                    Dim emaillist As String = DA.Command().Param("Action", "EmailsByPriv").Param("Privs", Convert.ToInt32(ClientPrivilege.StoreManager)).ExecuteScalar(Of String)("dbo.Client_Select").Value
                    Dim dt As DataTable = InventoryManager.GetItemsStockInfo(soid, True)
                    For Each row As DataRow In dt.Rows
                        If row.Field(Of Integer)("AvailableStock") < row.Field(Of Integer)("MinStockQuantity") Then
                            Try
                                SendEmail.Email(ConfigurationManager.AppSettings("StoreAdminEmail"), emaillist, False, "Item out of stock - " + row("Description").ToString(), "PartNO : " & row("ManufacturerPN").ToString())
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function DeleteOrder(ByVal soid As Integer) As Boolean
            Return DA.Command().Param("Action", "BySOID").Param("", soid).ExecuteNonQuery("sselMAS.dbo.StoreOrder_Delete").Value >= GeneralConstants.RET_ADO_DELETE_SUCCESS
        End Function

        Public Shared Function SecurityItems(ByVal soid As Integer) As DataTable
            Return DA.Command(CommandType.Text).Param("SOID", soid).FillDataTable("SELECT sio.SecurityItemOrderID, sio.ItemID, i.[Description] AS 'ItemDescription', sio.SOID, sio.CardNumber AS 'ItemCardNumber', sio.EmailSentDate FROM sselMAS.dbo.SecurityItemOrder sio INNER JOIN sselMAS.dbo.Item i ON i.ItemID = sio.ItemID WHERE sio.SOID = @SOID")
        End Function

        Public Shared Function SaveSecurityItem(ByVal itemId As Integer, ByVal soid As Integer, ByVal cardno As String) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("ItemID", itemId) _
                .Param("SOID", soid) _
                .Param("CardNumber", cardno) _
                .ExecuteNonQuery("INSERT SecurityItemOrder (ItemID, SOID, CardNumber, EmailSentDate) VALUES (@ItemID, @SOID, @CardNumber, GETDATE())").Value
        End Function
    End Class
End Namespace