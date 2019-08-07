Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class ItemDetail
    Inherits StorePage

    Dim itemId As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Get the item id and then load it
            If DataSanitizer.Sanitize(Request.QueryString("item"), RegexConstants.REGEX_ITEMID, itemId) Then
                If itemId = 233 Or itemId = 235 Then
                    imgItem.CssClass = "thumbimagetweezer"
                ElseIf (itemId > 157 And itemId < 163) Then
                    imgItem.CssClass = "thumbimagepen"
                Else
                    imgItem.CssClass = "thumbimage"
                End If
                LoadItem(itemId)

                'Sometimes we need to show only item information, not allow user to add item to cart.  e.g. view kit item detail
                If Request.QueryString("readonly") = "1" Then
                    pnlAdd.Visible = False
                End If
            Else
                litError.Text = "<div class=""error"">Invalid ItemID.</div>"
                divMain.Visible = False
            End If
        End If
    End Sub

    Protected Sub lnkAddToCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddToCart.Click
        Dim itemId As Integer = Convert.ToInt32(lblItemID.Text)

        Dim quantity As Integer = 0

        If Not Integer.TryParse(txtBuyQ.Text, quantity) Then
            quantity = 1
        End If

        Dim price As Double = 0
        Double.TryParse(lblUnitCost.Text.Substring(1), price)

        Dim desc As String = lblDescription.Text

        Dim mpn As String = lblManufacturerPN.Text

        AddItemToCart(itemId, price, desc, mpn, quantity)
    End Sub

    Private Sub AddItemToCart(ByVal itemId As Integer, ByVal price As Double, ByVal desc As String, ByVal mpn As String, ByVal quantity As Integer)
        Dim msg As String = String.Empty
        If CartManager.AddItemToCart("Cart", -1, itemId, price, desc, mpn, quantity, msg) Then
            litSecItemMessage.Text = String.Empty
            'We need to append the return url, so user can always come back the the same page after adding an item to cart
            Dim strArray As String() = Request.RawUrl.Split(Char.Parse("/"))
            Dim returnURL As String = strArray(strArray.Length - 1)
            returnURL = returnURL.Replace("&", "%26")

            Response.Redirect(String.Format("~/Cart.aspx?tab=1&returnurl={0}", returnURL))
        Else
            litSecItemMessage.Text = msg
        End If
    End Sub

    'Private Sub AddItemToCart()
    '    Dim item As StoreOrderItems
    '    If Session("Cart") Is Nothing Then
    '        item = New StoreOrderItems
    '    Else
    '        item = CType(Session("Cart"), StoreOrderItems)
    '    End If

    '    itemId = Convert.ToInt32(lblItemID.Text)
    '    Dim test_row() As DataRow = item.Select("ItemID = " & itemId.ToString())
    '    Dim row As DataRow = Nothing

    '    Dim quantity As Integer
    '    Try
    '        quantity = Convert.ToInt32(txtBuyQ.Text)
    '    Catch ex As Exception
    '        quantity = 1
    '    End Try

    '    If test_row.Length > 0 Then
    '        For Each r As DataRow In item.Rows
    '            If r.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId Then
    '                row = r
    '            End If
    '        Next
    '        row(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = row.Field(Of Integer)(DBConstants.STORE_ORDER_DETAIL_QUANTITY) + quantity  'Initial Quantity is always 1
    '    Else
    '        row = item.NewRow()

    '        row(DBConstants.STORE_ORDER_DETAIL_QUANTITY) = quantity
    '        row(DBConstants.STORE_ORDER_DETAIL_ITEMID) = itemId
    '        row(DBConstants.STORE_ORDER_DETAIL_SOID) = -1 'tmp soid
    '        row(DBConstants.STORE_ORDER_DETAIL_PRICEID) = PriceManager.GetPriceIDByItemID(itemId)
    '        row(DBConstants.ITEM_DESCRIPTION) = lblDescription.Text
    '        row(DBConstants.ITEM_MANUFACTURERPN) = lblManufacturerPN.Text
    '        row(DBConstants.STORE_ORDER_DETAIL_UNITPRICE) = lblUnitCost.Text.Substring(1)
    '        item.Rows.Add(row)
    '    End If
    '    Session("Cart") = item

    '    Response.Redirect("~/Cart.aspx?tab=1")
    'End Sub

    Sub LoadItem(ByVal itemId As Integer)
        Dim item As Item = Item.Create(itemId)

        If item.Loaded Then
            lblDescription.Text = item.Description
            lblNotes.Text = item.Notes
            lblManufacturerPN.Text = item.ManufacturerPN
            imgItem.ImageUrl = GlobalUtility.GetItemImagePath() & item.ItemImageName
            imgItem.AlternateText = "product - " & item.Description

            'get the available stock quantity
            Dim temp As Integer
            If DataSanitizer.Sanitize(Request.QueryString("avail"), RegexConstants.REGEX_ITEMID, temp) Then
                item.StockQuantity = temp
            End If

            lblStockQ.Text = item.StockQuantity.ToString()
            If item.StockQuantity < 1 Then
                Dim stockDate As DateTime = Now
                Dim da As System.DayOfWeek = Now.DayOfWeek
                If da = DayOfWeek.Monday Then
                    stockDate = stockDate.AddDays(2)
                ElseIf da = DayOfWeek.Tuesday Then
                    stockDate = stockDate.AddDays(1)
                ElseIf da = DayOfWeek.Wednesday Then
                    stockDate = stockDate.AddDays(7)
                ElseIf da = DayOfWeek.Thursday Then
                    stockDate = stockDate.AddDays(6)
                ElseIf da = DayOfWeek.Friday Then
                    stockDate = stockDate.AddDays(5)
                ElseIf da = DayOfWeek.Saturday Then
                    stockDate = stockDate.AddDays(4)
                ElseIf da = DayOfWeek.Sunday Then
                    stockDate = stockDate.AddDays(3)
                End If
                lblStockWarning.Text = MessageUtility.GetStockZeroMessage(stockDate)
                lnkAddToCart.Visible = False
            Else
                lblStockWarning.Text = ""
                lnkAddToCart.Visible = True
            End If

            pnlAdd.Visible = True
            lblItemID.Text = itemId.ToString()
            lblUnitCost.Text = String.Format("{0:C}", -PriceManager.GetCurrentItemPriceByItemID(item.ItemID)).Replace("(", "").Replace(")", "")
            Page.Title = "Product Detail: " + item.ManufacturerPN
            If item.CrossCharge Then
                lblCrossCharge.Text = "Yes"
            Else
                lblCrossCharge.Text = "No"
            End If


            hypReturn.NavigateUrl = String.Format("~/Catalog.aspx?tabid=0&h1=1&cid={0}", item.CategoryID)
            hypReturn.Text = String.Format("&larr; Return to {0}", item.CategoryName)
        Else
            lblDescription.Text = "Invalid Product"
            'lblManufacturerPN.Text = "Invalid Product"
            imgItem.Visible = False
            lnkAddToCart.Visible = False
        End If
    End Sub

End Class