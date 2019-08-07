Imports LNF.Repository
Imports LNF.Repository.Inventory
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Controls
    Public Class OrderDetail
        Inherits UserControl

        Dim ds As DataSet
        Dim dt As DataTable
        Dim dtSecItems As DataTable
        Dim currentStatus As String

        Public Property AdminMode As Boolean

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            If AdminMode Then
                spnStatusDDL.Visible = True
                CartView1.Locked = False
                CartView1.AdminMode = True
                btnCancel.Text = "Cancel"
            Else
                spnStatusDDL.Visible = False
                divAddNewItem.Visible = False
                CartView1.AdminMode = False
                btnCancel.Text = "Back to Order List"
            End If

            If Not IsPostBack Then

                LoadData()

                'lnkPrinter.NavigateUrl = "javascript:windowP('" + VirtualPathUtility.ToAbsolute(String.Format("~/admin/PrintOneOrder.aspx?so={0}", hidSOID.Value)) + "')"
                lnkPrinter.NavigateUrl = String.Format("~/admin/PrintOrder.aspx?soid={0}", hidSOID.Value)
                lnkPrinter.Target = "_blank"

                txtCreationDate.Enabled = False
                txtClient.Enabled = False
                txtAccount.Enabled = False
                txtContact.Enabled = False
                txtStatusChangeDate.Enabled = False

                If Request.QueryString("status") = "Search" Then
                    btnCancel.Visible = False
                End If
            End If
        End Sub

        Private Sub LoadData()
            'Get the store order id and then load this particular order
            Dim soid As Integer = 0
            If DataSanitizer.Sanitize(Request.QueryString("so"), RegexConstants.REGEX_SOID, soid) Then
                'Load the order's header data
                If LoadStoreOrder(soid) Then
                    'Load the order's details into DataSet
                    ds = StoreOrderManager.GetStoreOrderDetailBySOID(soid) 'dataset is already modified afer this call, keep this in mind

                    'I have to set the schema to this dataset, because SODID is a primary key with identity property set
                    ds.Tables(0).PrimaryKey = New DataColumn() {ds.Tables(0).Columns(DBConstants.STORE_ORDER_DETAIL_SODID)}

                    'Since SODID is the primary key in this dataset, i need to make it autoincrement because it's a identiy field in real database
                    Dim seed As Integer
                    Try
                        seed = Convert.ToInt32(ds.Tables(0).Compute("MAX(SODID)", String.Empty))
                    Catch ex As Exception
                        seed = 0
                    End Try

                    ds.Tables(0).Columns(DBConstants.STORE_ORDER_DETAIL_SODID).AutoIncrement = True
                    ds.Tables(0).Columns(DBConstants.STORE_ORDER_DETAIL_SODID).AutoIncrementSeed = seed + 1
                    ds.Tables(0).Columns(DBConstants.STORE_ORDER_DETAIL_SODID).AutoIncrementStep = 1

                    SetSessionData(ds)

                    'Misc operations
                    If Not AdminMode Then
                        lbtnBack.PostBackUrl = String.Format("~/Orders.aspx?tab=2&menu=0&status={0}", Request.QueryString("status"))
                    Else
                        lbtnBack.PostBackUrl = String.Format("~/admin/OrderManager.aspx?tab=0&menu=0&status={0}", Request.QueryString("status"))
                    End If

                    BindControls()
                Else
                    lblSOID.Text = "Invalid StoreOrderID"
                    lblSaveMessage.Text = "Error Invalid StoreOrderID"
                    divOrderHeader.Visible = False
                    'divTotalPrice.Visible = False
                    'divAddNewItem.Visible = False
                End If
            Else
                litError.Text = "<div class=""error"">Invalid StoreOrderID</div>"
                divMain.Visible = False
            End If
        End Sub

        Private Sub BindControls()
            BindTotalPrice()
            CartView1.LoadCart()

            'gvOrderDetail.DataSource = ds
            DataBind()
        End Sub

        Private Sub BindTotalPrice()
            Dim totalPrice As Double = 0
            For Each row As DataRow In CartView1.GetItems().Rows
                If Not row.RowState = DataRowState.Deleted Then
                    'Dim qty As Integer = CType(row(""), Integer)
                    Dim lineTotal As Double = row.Field(Of Double)(DBConstants.STORE_ORDER_DETAIL_LINETOTAL)
                    'total_price += unitPrice * qty
                    totalPrice += lineTotal
                End If
            Next
            'txtTotalPrice.Text = Format(total_price, "$,#,##0.00")
        End Sub

        Private Function LoadStoreOrder(ByVal soid As Integer) As Boolean
            Dim so As StoreOrder = StoreOrder.Create(soid)

            If so.Loaded Then
                If Not so.ClientID = CurrentUser.GetCurrentClientID() And Not AdminMode Then
                    Return False
                End If

                dtSecItems = StoreOrderManager.SecurityItems(so.SOID)
                hidSOID.Value = so.SOID.ToString()
                lblSOID.Text = hidSOID.Value
                txtCreationDate.Text = so.CreationDate.ToString("yyyy-MM-dd")
                txtClient.Text = SselData.GetClientNameByClientID(so.ClientID)
                txtContact.Text = SselData.GetClientContactByClientID(so.ClientID, so.AccountID)
                txtAccount.Text = so.AccountName

                'client ID is needed when user saves data 
                hidClientID.Value = so.ClientID.ToString()

                txtStatusChangeDate.Text = so.StatusChangeDate.ToString("yyyy-MM-dd")
                currentStatus = so.Status
                lblLastStatus.Text = so.Status 'I need this to check the transition between order status states.

                'Order Status related code
                If AdminMode Then
                    ddlStatus.SelectedValue = currentStatus
                    ddlStatus.DataSource = StoreOrderManager.GetStoreOrderStatus(False, False, currentStatus)
                Else
                    lblStatus.Text = currentStatus
                End If

                Dim iLoc As InventoryLocation = DA.Current.Single(Of InventoryLocation)(so.InventoryLocationID)
                txtLocation.Text = iLoc.LocationName()

                If currentStatus = AppConstants.STORE_ORDER_STATUS_CLOSED OrElse currentStatus = AppConstants.STORE_ORDER_STATUS_CANCELLED Then
                    SetControlsEnabledSetting()
                End If

                Return True
            Else
                Return False
            End If
        End Function

        Private Sub SetControlsEnabledSetting()
            'btnCreateNewOrderDetail.Enabled = False
            'btnSave.Visible = False
            'btnAdjustQ.Visible = False
            CartView1.Locked = True
            divAddNewItem.Visible = False

            txtCreationDate.Enabled = False
            txtClient.Enabled = False
            txtAccount.Enabled = False
            txtContact.Enabled = False
            txtStatusChangeDate.Enabled = False
        End Sub

        Private Function CheckGridViewItemQuantities() As Boolean
            'Dim newQuantity As Integer
            Dim bad_items As New StockQtyViolation() 'store items that exceeds current stock #

            'update the quantity
            'For Each grow As GridViewRow In gvOrderDetail.Rows
            '    'I need this sodid to consider current order's number, else user won't see the correct # of available stock
            '    Dim sodid As Integer = Convert.ToInt32(gvOrderDetail.DataKeys(grow.DataItemIndex).Value)
            '    Dim itemID As Integer = CType(grow.Cells(1).Text, Integer)
            '    newQuantity = CType(CType(grow.Cells(5).FindControl("txtQuantity"), TextBox).Text, Integer)

            '    Dim stockQty As Integer
            '    If Not ItemManager.CheckStockQtyErrorByItemID(itemID, newQuantity, stockQty, sodid) Then
            '        bad_items.Add(grow.Cells(3).Text, stockQty, newQuantity)
            '    End If
            'Next

            'If there are errors abount exceeding stock, we need to let user know
            If bad_items.Rows.Count > 0 Then
                'Some items' quantity exceeds the current stock
                Dim strTemp As String = "The qty ({0}) of Item '{1}' you are buying exceeds our current stock of {2}.  Nothing has been saved.<br />"
                Dim strOutput As String = ""
                For Each drow As DataRow In bad_items.Rows
                    strOutput += String.Format(strTemp, drow(DBConstants.STORE_ORDER_DETAIL_QUANTITY), drow(DBConstants.ITEM_DESCRIPTION), drow(DBConstants.ITEM_STOCKQUANTITY))
                Next
                lblSaveMessage.Text = strOutput
                Return False
            Else
                Return True
            End If
        End Function

        Private Function GetSessionData() As DataSet
            If Session("DS_OrderDetail") Is Nothing Then
                LoadData() 'SetSessionData is called inside
            End If
            Return CType(Session("DS_OrderDetail"), DataSet)
        End Function

        Private Sub SetSessionData(ByVal ds As DataSet)
            Session("DS_OrderDetail") = ds
            Dim name As String = CartView1.GetSessionName()
            Dim key As String = CartManager.GetSessionKey(name)
            Session(key) = CType(ds.Tables(0), StoreOrderItems)
        End Sub

        Private Sub SaveOrder()
            lblSaveMessage.Text = String.Empty

            Dim creationDate As Date
            Dim statusDate As Date

            If Not Date.TryParse(txtCreationDate.Text, creationDate) OrElse Not Date.TryParse(txtStatusChangeDate.Text, statusDate) Then
                lblSaveMessage.Text = "Date is incorrect - Please make sure it's mm/dd/yyyy"
                Return
            End If

            'if the order is being closed make sure all security item card numbers have been filled in
            If ddlStatus.SelectedValue = "Closed" Then
                Dim ok As Boolean = True
                For Each ritem As RepeaterItem In rptSecItems.Items
                    Dim hid As HiddenField = CType(ritem.FindControl("hidSecurityItemOrderID"), HiddenField)
                    If hid IsNot Nothing Then
                        If hid.Value = String.Empty Then
                            ok = False
                            Exit For
                        End If
                    End If
                Next
                If Not ok Then
                    lblSaveMessage.Text = "The order cannot be closed until all security item emails have been sent."
                    Return
                End If
            End If

            'Check if any of new quantity needed exceeds our stock quantity
            'If Not CheckGridViewItemQuantities() Then
            '    Return
            'End If

            'First, handle the order header, then we handle the order details 
            Dim order As New StoreOrder

            order.SOID = Convert.ToInt32(hidSOID.Value)
            order.CreationDate = creationDate
            order.ClientID = Convert.ToInt32(hidClientID.Value)
            order.StatusChangeDate = statusDate

            If AdminMode Then
                order.Status = ddlStatus.SelectedValue
            Else
                order.Status = lblStatus.Text
            End If

            'Second, handle the order details
            ds = GetSessionData()
            Dim items As StoreOrderItems = CartView1.GetItems()
            Dim tempHash As New Dictionary(Of Integer, Integer) 'used to update current stock quantity at later part of the code, because when admin change order status, we have to update stock Qty
            Dim offset As Integer = 0 ' this is used to keep aligned for dataset and grid - since dataset might contain deleted rows which grid doesn't,
            '                           we have to see if any of record's quantity has been changed, if so, we have to update the new qty back to dataset

            For Each row As GridViewRow In CartView1.GetRows()
                Dim quantityColumnIndex As Integer = 4 'was 5
                Dim textboxControlIndex As Integer = 1
                Dim itemIdColumnIndex As Integer = 0 'was 1
                Dim newQty As Integer = Convert.ToInt32(CType(row.Cells(quantityColumnIndex).Controls(textboxControlIndex), TextBox).Text)

                dt = ds.Tables(0)

                'Only Modified or Added rows are needed with updated qty values. So we increment offset to avoid updating data to unimportant rows
                While dt.Rows(row.RowIndex + offset).RowState <> DataRowState.Modified AndAlso dt.Rows(row.RowIndex + offset).RowState <> DataRowState.Added
                    offset += 1
                End While

                'Modified and added row needs to check if the quantity has been changed.  Dataset is always modifed due to UnitPrice and LineTotal column
                'If dt.Rows(row.RowIndex + offset).RowState = DataRowState.Modified Then
                Dim origQty As Integer = dt.Rows(row.RowIndex + offset).Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_QUANTITY, DataRowVersion.Current)

                If newQty <> origQty Then
                    Dim tempRow As DataRow = dt.Rows(row.RowIndex + offset)
                    Dim doUpdateQty As Boolean = True
                    If ItemManager.IsSecurityItem(Convert.ToInt32(tempRow(DBConstants.STORE_ORDER_DETAIL_ITEMID))) Then
                        If newQty > 1 Then doUpdateQty = False
                    End If

                    If doUpdateQty Then
                        tempRow(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = newQty
                    Else
                        lblSaveMessage.Text = "Security item quantity cannot exceed one item limit."
                        LoadData()
                        Return
                    End If
                End If
                tempHash.Add(Integer.Parse(row.Cells(itemIdColumnIndex).Text), newQty)
            Next

            ' Start updating the database
            ' First, update the order header
            StoreOrderManager.UpdateStoreOrder(order)

            'Second, update the order detail 
            If Not StoreOrderManager.UpdateStoreOrderDetail(ds.GetChanges()) Then
                'Either nothing changed in grid, or there is an error
            End If

            'Update the StockQuantity in Item table if the order is changed to Closed
            If AdminMode Then
                currentStatus = ddlStatus.SelectedValue
            Else
                currentStatus = lblStatus.Text
            End If

            If lblLastStatus.Text = AppConstants.STORE_ORDER_STATUS_FULFILLED And currentStatus = AppConstants.STORE_ORDER_STATUS_CLOSED Then
                For Each kvp As KeyValuePair(Of Integer, Integer) In tempHash
                    If Not InventoryManager.UpdateItemStockQuantity(kvp.Key, kvp.Value) Then
                        'something went wrong
                    End If
                Next
            End If

            lblSaveMessage.Text = "Changes saved successfully"
            LoadData()

        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Response.Redirect(lbtnBack.PostBackUrl)
        End Sub

        Protected Sub btnCreateNewOrderDetail_Click(sender As Object, e As EventArgs) 'Handles btnCreateNewOrderDetail.Click
            lblSaveMessage.Text = String.Empty

            Dim itemId As Integer = Integer.Parse(txtNewItemID.Text)
            Dim qty As Integer = Integer.Parse(txtNewQty.Text)

            'Before we save anything, we have to make sure this item didn't exist on current dataset yet
            Dim items As StoreOrderItems = CartView1.GetItems()
            Dim dr As DataRow = items.AsEnumerable().Where(Function(x) x.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId).FirstOrDefault()
            Dim existFlag As Boolean = dr IsNot Nothing

            If Not existFlag Then

                'Load Item table's information first, since this dataset is made up by joining two tables
                Dim item As AppCode.Item = AppCode.Item.Create(itemId)

                If item.Loaded Then
                    'we know the validated item didn't exist on dataset yet, so now we have to make sure the quantity specified 
                    'doesn't exceed the current stock value
                    Dim stockQty As Integer
                    If Not ItemManager.CheckStockQtyErrorByItemID(itemId, qty, stockQty) Then
                        lblSaveMessage.Text = String.Format("<div class=""stock-error"">The quantity {0} of '{1}' you are buying exceeds our current stock of {2}</div>", qty, item.Description, stockQty)
                        Return
                    End If

                    ds = GetSessionData()

                    If ds Is Nothing Then
                        Response.Redirect("~")
                        Return
                    End If

                    Dim soid As Integer = Integer.Parse(hidSOID.Value)
                    Dim priceId As Integer = PriceManager.GetPriceIDByItemID(itemId)
                    Dim unitPrice As Double = PriceManager.GetCurrentItemPriceByPriceIDAndStoreOrder(priceId, soid)
                    Dim msg As String = String.Empty

                    If CartView1.AddItem(soid, itemId, unitPrice, item.Description, item.ManufacturerPN, qty, msg) Then
                        dtSecItems = StoreOrderManager.SecurityItems(Convert.ToInt32(hidSOID.Value))
                        SetSessionData(ds)
                        BindControls()
                    Else
                        lblSaveMessage.Text = msg
                    End If

                    'Dim ndr As DataRow = items.NewRow()
                    'ndr(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = qty
                    'ndr(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId
                    'ndr(DBConstants.STORE_ORDER_DETAIL_SOID) = Integer.Parse(hidSOID.Value)
                    'ndr(DBConstants.STORE_ORDER_DETAIL_PRICEID) = PriceManager.GetPriceIDByItemID(itemId)
                    'ndr(DBConstants.ITEM_DESCRIPTION) = item.Description
                    'ndr(DBConstants.ITEM_MANUFACTURERPN) = item.ManufacturerPN
                    'ndr(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = price
                    'items.Rows.Add(row)

                    'Dim newrow As DataRow = dt.NewRow()
                    'newrow(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemID
                    'newrow(DBConstants.STORE_ORDER_DETAIL_SOID) = CType(hidSOID.Value, Integer)
                    ''Dim qty As Integer = newQty
                    'newrow(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = qty

                    ''Use the ItemID to find the newest PriceID
                    'Dim priceID As Integer = PriceManager.GetPriceIDByItemID(itemID)
                    'newrow(DBConstants.STORE_ORDER_DETAIL_PRICEID) = priceID
                    'Dim unitPrice As Double = PriceManager.GetCurrentItemPriceByPriceID(priceID)
                    'newrow(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = unitPrice
                    'newrow(DBConstants.STORE_ORDER_DETAIL_LINETOTAL) = unitPrice * qty
                    'newrow(DBConstants.ITEM_DESCRIPTION) = item.Description
                    'newrow(DBConstants.ITEM_MANUFACTURERPN) = item.ManufacturerPN
                    'dt.Rows.Add(newrow)

                    'dtSecItems = StoreOrderManager.SecurityItems(Convert.ToInt32(hidSOID.Value))
                    'SetSessionData(ds)
                    'BindControls()
                Else
                    lblSaveMessage.Text = "Error! The item # " + itemId.ToString() + " could not be found in database"
                End If
            Else
                lblSaveMessage.Text = "Error! The item # " + itemId.ToString() + " is already added to the above grid. You can only change the quantity field of that item"
            End If
        End Sub

        Protected Sub btnSendEmail_Click(sender As Object, e As EventArgs)
            Dim footer As UI.Control = rptSecItems.Controls(rptSecItems.Controls.Count - 1)
            Dim lblSendEmailError As Label = CType(footer.FindControl("lblSendEmailError"), Label)
            Dim lblItemID As Label
            Dim lblItemDescription As Label
            Dim txtCardNumber As TextBox
            Dim hidSecurityItemOrderID As HiddenField

            lblSendEmailError.Text = String.Empty

            Dim soid As Integer = Convert.ToInt32(hidSOID.Value)
            Dim itemID As Integer
            Dim desc As String
            Dim cardno As String
            Dim sioID As Object

            Dim dt As New DataTable()
            dt.Columns.Add("SecurityItemOrderID", GetType(Integer))
            dt.Columns.Add("ItemID", GetType(Integer))
            dt.Columns.Add("ItemDescription", GetType(String))
            dt.Columns.Add("SOID", GetType(Integer))
            dt.Columns.Add("CardNumber", GetType(String))

            For Each ritem As RepeaterItem In rptSecItems.Items
                lblItemID = CType(ritem.FindControl("lblItemID"), Label)
                lblItemDescription = CType(ritem.FindControl("lblItemDescription"), Label)
                txtCardNumber = CType(ritem.FindControl("txtCardNumber"), TextBox)
                hidSecurityItemOrderID = CType(ritem.FindControl("hidSecurityItemOrderID"), HiddenField)

                itemID = Convert.ToInt32(lblItemID.Text)
                desc = lblItemDescription.Text
                cardno = txtCardNumber.Text
                If hidSecurityItemOrderID.Value = String.Empty Then
                    sioID = DBNull.Value
                Else
                    sioID = Convert.ToInt32(hidSecurityItemOrderID.Value)
                End If

                If cardno = String.Empty OrElse cardno = "Scan here" Then
                    lblSendEmailError.Text = "All card numbers must be entered."
                    Return
                End If
                dt.Rows.Add(sioID, itemID, desc, soid, cardno)
            Next

            For Each dr As DataRow In dt.Rows
                If dr("SecurityItemOrderID") Is DBNull.Value Then
                    Dim nl As String = "<br />"
                    Dim body As String = "LNF Access Card Request" + nl + "----------------------------------------------"
                    body += nl + "Creation Date: " + txtCreationDate.Text
                    body += nl + "Order #: " + hidSOID.Value
                    body += nl + "Client: " + txtClient.Text
                    body += nl + "Item Sold: " + dr("ItemDescription").ToString() + " [Item #: " + dr("ItemID").ToString() + "]"
                    body += nl + "Card #: " + dr("CardNumber").ToString()

                    LNF.CommonTools.SendEmail.Email(ConfigurationManager.AppSettings("StoreAdminEmail"), ConfigurationManager.AppSettings("SecurityAdminEmail"), False, "LNF Access Card Request", body)
                    StoreOrderManager.SaveSecurityItem(Convert.ToInt32(dr("ItemID")), Convert.ToInt32(dr("SOID")), dr("CardNumber").ToString())
                End If
            Next

            LoadData()
        End Sub

        Protected Function DbDate(obj As Object) As String
            Try
                Return Convert.ToDateTime(obj).ToString("MM/dd/yyyy HH:mm:ss")
            Catch
                Return String.Empty
            End Try
        End Function

        Protected Sub CartView1_CheckoutClick(sender As Object, e As CartEventArgs)
            'this can only be called when viewing order detail as an admin because that is the only time the button is visible
            SaveOrder()
        End Sub

        Protected Sub CartView1_CartLoading(sender As Object, e As CartLoadingEventArgs)
            If AdminMode Then
                For Each dr As DataRow In e.Items.Rows
                    Dim itemId As Integer = dr.Field(Of Integer)("ItemID")
                    Dim isSecItem As Boolean = ItemManager.IsSecurityItem(itemId)
                    If isSecItem Then
                        panSecurityItems.Visible = True
                        Dim rows As DataRow() = dtSecItems.Select(String.Format("ItemID = {0}", itemId))
                        If rows.Length = 0 Then
                            dtSecItems.Rows.Add(DBNull.Value, dr("ItemID"), dr("Description"), hidSOID.Value, String.Empty, DBNull.Value)
                        End If
                    End If
                Next

                If panSecurityItems.Visible Then
                    rptSecItems.DataSource = dtSecItems
                    rptSecItems.DataBind()
                End If
            End If
        End Sub
    End Class
End Namespace