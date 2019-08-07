Imports LNF.Cache
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Controls
    Public Class CartView
        Inherits System.Web.UI.UserControl

        Private loadFlag As Boolean

        Public Property IsKit As Boolean
        Public Property Locked As Boolean
        Public Property AdminMode As Boolean
        Public Property SessionKey As String

        Public ReadOnly Property Message As Label
            Get
                Return lblTopMsg
            End Get
        End Property

        Public ReadOnly Property StockError As Label
            Get
                Return lblStockError
            End Get
        End Property

        Public Event ItemDetailClick As EventHandler(Of CartItemEventArgs)
        Public Event CheckoutClick As EventHandler(Of CartEventArgs)
        Public Event ContinueShoppingClick As EventHandler(Of CartEventArgs)
        Public Event CartLoading As EventHandler(Of CartLoadingEventArgs)

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            lblStockError.Text = String.Empty

            If Request.QueryString("edit") = "1" Then
                lblTopMsg.Visible = True
                lblTopMsg.Text = "The order you want to edit has been moved to the shopping cart. Please remember to check out again or else you will lose your order"
            ElseIf IsKit Then
                lblTopMsg.Visible = True
                lblTopMsg.Text = "There is no more shopping allowed because you are ordering a kit. Your cart will be reset if you don't click the Checkout button as your next step"
            Else
                lblTopMsg.Visible = False
            End If

            gvCart.Columns(0).Visible = AdminMode
        End Sub

        Public Function GetSessionName() As String
            Return String.Format("{0}Cart{1}", If(IsKit, "Kit", String.Empty), SessionKey)
        End Function

        Protected Function GetItemURL(ByVal soid As Integer) As String
            Return VirtualPathUtility.ToAbsolute(String.Format("~/ItemDetail.aspx?item={0}", soid))
        End Function

        Protected Function IsEnabled() As Boolean
            Return Not IsKit AndAlso Not Locked
        End Function

        Protected Function IsRegularShopping() As Boolean
            Return Not IsKit AndAlso Not Locked
        End Function

        Private Sub SetItems(items As StoreOrderItems)
            Dim name As String = GetSessionName()
            Dim key As String = CartManager.GetSessionKey(name)
            Session(key) = items
        End Sub

        Public Function GetItems() As StoreOrderItems
            Return CartManager.GetCart(GetSessionName())
        End Function

        Public Function GetRows() As GridViewRowCollection
            Return gvCart.Rows
        End Function

        Public ReadOnly Property ItemCount As Integer
            Get
                Return GetItems().Rows.Count
            End Get
        End Property

        Public Function AddItem(soid As Integer, itemId As Integer, price As Double, desc As String, mpn As String, quantity As Integer, ByRef message As String) As Boolean
            Return CartManager.AddItemToCart(GetSessionName(), soid, itemId, price, desc, mpn, quantity, message)
        End Function

        Public Sub LoadCart()
            'use a copy so that data can be modified by CartLoading but the underlying data will not change
            Dim items As StoreOrderItems = GetItems().Copy()

            RaiseEvent CartLoading(Me, New CartLoadingEventArgs(items, IsKit))

            gvCart.DataSource = items
            gvCart.DataBind()

            If items IsNot Nothing Then
                lblTotal.Text = items.TotalPrice.ToString("$,#,##0.00")
            End If

            If AdminMode Then
                btnCheckout.Text = "Save"
            End If

            If Locked Then
                divCartOperation.Visible = True
                btnAdjustQ.Visible = False
                btnContinueShopping.Visible = False
                btnCheckout.Visible = False
            Else
                If items Is Nothing OrElse items.Rows.Count = 0 Then
                    divCartOperation.Visible = AdminMode
                    btnAdjustQ.Visible = False
                    btnContinueShopping.Visible = False
                    btnCheckout.Visible = AdminMode
                Else
                    If AdminMode Then
                        btnContinueShopping.Visible = False
                        btnCheckout.Visible = True
                        btnAdjustQ.Visible = False
                    Else
                        btnContinueShopping.Enabled = Not IsKit
                        btnAdjustQ.Enabled = Not IsKit
                    End If
                End If
            End If
        End Sub

        Public Function UpdateQuantities() As Boolean
            Dim txtBox As TextBox
            Dim newQuantity As Integer
            Dim badItems As New StockQtyViolation() 'store items that exceeds current stock #
            Dim secItems As New StockQtyViolation() 'security items that exceed the 1 item limit

            lblStockError.Text = String.Empty

            'check the quantity
            Dim itemID As Integer
            For Each grow As GridViewRow In gvCart.Rows
                itemID = Convert.ToInt32(gvCart.DataKeys(grow.DataItemIndex).Value)
                txtBox = CType(grow.Cells(4).FindControl("txtQuantity"), TextBox)
                Try
                    newQuantity = CType(txtBox.Text, Integer)
                Catch ex As Exception
                    lblStockError.Text += "The quantity number is incorrect"
                    Return False
                End Try

                If newQuantity <> 0 Then
                    Dim stockQty As Integer
                    If Not CartManager.CheckQuantity(itemID, newQuantity, stockQty) Then
                        badItems.Add(CType(grow.Cells(1).FindControl("lbtnItemDetail"), LinkButton).Text, stockQty, newQuantity)
                    End If
                    If ItemManager.IsSecurityItem(itemID) Then
                        If newQuantity > 1 Then
                            secItems.Add(CType(grow.Cells(1).FindControl("lbtnItemDetail"), LinkButton).Text, 1, newQuantity)
                        End If
                    End If
                End If
            Next

            'If there are errors abount exceeding stock, we need to let user know
            If badItems.Rows.Count > 0 OrElse secItems.Rows.Count > 0 Then
                'Some items' quantity exceeds the current stock
                Dim temp As String = "<div class=""stock-error"">The quantity {0} of '{1}' exceeds our current stock of {2}</div>"
                Dim strOutput As String = String.Empty
                For Each drow As DataRow In badItems.Rows
                    strOutput += String.Format(temp, drow(DBConstants.STORE_ORDER_DETAIL_QUANTITY), drow(DBConstants.ITEM_DESCRIPTION), drow(DBConstants.ITEM_STOCKQUANTITY))
                Next
                lblStockError.Text += strOutput

                temp = "<div class=""stock-error"">The quantity {0} of '{1}' exceeds the limit of {2} for security items</div>"
                strOutput = String.Empty
                For Each drow As DataRow In secItems.Rows
                    strOutput += String.Format(temp, drow(DBConstants.STORE_ORDER_DETAIL_QUANTITY), drow(DBConstants.ITEM_DESCRIPTION), drow(DBConstants.ITEM_STOCKQUANTITY))
                Next
                lblStockError.Text += strOutput

                Return False
            Else
                Dim items As StoreOrderItems = GetItems()
                For Each grow As GridViewRow In gvCart.Rows
                    itemID = Convert.ToInt32(gvCart.DataKeys(grow.DataItemIndex).Value)
                    txtBox = CType(grow.Cells(4).FindControl("txtQuantity"), TextBox)
                    newQuantity = CType(txtBox.Text, Integer)

                    If newQuantity = 0 Then
                        Dim index As Integer
                        Dim total As Integer = items.Rows.Count - 1
                        For i As Integer = 0 To total
                            If items.Rows(i).RowState <> DataRowState.Deleted Then
                                If items.Rows(i).Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemID Then
                                    index = i
                                    Exit For
                                End If
                            End If
                        Next i

                        If AdminMode Then
                            items.Rows(index).Delete()
                        Else
                            items.Rows.RemoveAt(index)
                        End If
                    Else
                        For Each row As DataRow In items.Rows
                            If row.RowState <> DataRowState.Deleted Then
                                If row.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemID Then
                                    row(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = newQuantity
                                End If
                            End If
                        Next
                    End If
                Next
                SetItems(items)
            End If

            'Make sure if all items have 0 order quantity
            Dim count As Integer = 0
            For Each grow As GridViewRow In gvCart.Rows
                txtBox = CType(grow.Cells(4).FindControl("txtQuantity"), TextBox)
                Try
                    newQuantity = CType(txtBox.Text, Integer)
                Catch ex As Exception
                    lblStockError.Text += "The quantity number is incorrect"
                    Return False
                End Try

                If newQuantity = 0 Then
                    count += 1
                End If
            Next

            If gvCart.Rows.Count = count Then
                loadFlag = True
                Return False
            End If

            Return True
        End Function

        Protected Sub btnCheckout_Click(sender As Object, e As EventArgs)
            Dim args As New CartEventArgs(IsKit)

            If IsKit Then
                RaiseEvent CheckoutClick(Me, args)
            Else
                'check if the specified quantity exceeds the stock number
                If UpdateQuantities() Then
                    RaiseEvent CheckoutClick(Me, args)
                Else
                    LoadCart()
                    If AdminMode Then
                        RaiseEvent CheckoutClick(Me, args)
                    End If
                End If
            End If
        End Sub

        Protected Sub btnAdjustQ_Click(ByVal sender As Object, ByVal e As EventArgs)
            If UpdateQuantities() Then
                LoadCart()
            Else
                If loadFlag Then
                    LoadCart()
                End If
            End If
        End Sub

        Protected Sub btnContinueShopping_Click(sender As Object, e As EventArgs)
            RaiseEvent ContinueShoppingClick(Me, New CartEventArgs(IsKit))
        End Sub

        Protected Sub gvCart_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvCart.RowDeleting
            LoadCart()
        End Sub

        Protected Sub lbtnItemDetail_Command(sender As Object, e As CommandEventArgs)
            RaiseEvent ItemDetailClick(Me, New CartItemEventArgs(Convert.ToInt32(e.CommandArgument), IsKit))
        End Sub

        Protected Sub ibtnItemDelete_Command(sender As Object, e As CommandEventArgs)
            'Delete one single item from this order
            If e.CommandName = "Delete" Then
                Dim itemId As Integer = Convert.ToInt32(e.CommandArgument)
                Dim items As StoreOrderItems = GetItems()
                Dim index As Integer
                Dim total As Integer = items.Rows.Count - 1
                For i As Integer = 0 To total
                    If items.Rows(i).RowState <> DataRowState.Deleted Then
                        If items.Rows(i).Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId Then
                            index = i
                            Exit For
                        End If
                    End If
                Next i
                If items.Rows.Count > index Then
                    If AdminMode Then
                        items.Rows(index).Delete()
                    Else
                        items.Rows.RemoveAt(index)
                    End If
                    SetItems(items)
                End If
                LoadCart()
            End If
        End Sub
    End Class

    Public Class CartItemEventArgs
        Inherits CartEventArgs

        Private _ItemID As Integer

        Friend Sub New(itemId As Integer, isKit As Boolean)
            MyBase.New(isKit)
            _ItemID = itemId
        End Sub

        Public ReadOnly Property ItemID As Integer
            Get
                Return _ItemID
            End Get
        End Property
    End Class

    Public Class CartLoadingEventArgs
        Inherits CartEventArgs

        Private _Items As StoreOrderItems

        Friend Sub New(items As StoreOrderItems, isKit As Boolean)
            MyBase.New(isKit)
            _Items = items
        End Sub

        Public ReadOnly Property Items As StoreOrderItems
            Get
                Return _Items
            End Get
        End Property
    End Class

    Public Class CartEventArgs
        Inherits EventArgs

        Private _IsKit As Boolean

        Friend Sub New(isKit As Boolean)
            _IsKit = isKit
        End Sub

        Public ReadOnly Property IsKit As Boolean
            Get
                Return _IsKit
            End Get
        End Property
    End Class
End Namespace