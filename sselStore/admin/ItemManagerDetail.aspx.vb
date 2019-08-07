Imports LNF.Repository
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Admin
    Public Class ItemManagerDetail
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Get the item id and then load it
                Dim itemId As Integer
                If DataSanitizer.Sanitize(Request.QueryString("item"), RegexConstants.REGEX_ITEMID, itemId) Then
                    If Not String.IsNullOrEmpty(Request.QueryString("query")) Then
                        lbtnBack.PostBackUrl = "~/admin/ItemManager.aspx?tab=1&menu=0&query=" + Request.QueryString("query")
                    End If
                    DataBind()
                    LoadItem(itemId)
                Else
                    litError.Text = "<div class=""stock-error"">Invalid ItemID</div>"
                    divMain.Visible = False
                End If
            End If
        End Sub

        Sub LoadItem(ByVal itemId As Integer)
            litSaveError.Text = String.Empty

            Dim item As Item = Item.Create(itemId)

            If item.Loaded Then
                lblItemID.Text = itemId.ToString()
                txtDescription.Text = item.Description
                txtNotes.Text = item.Notes
                txtManufacturerPN.Text = item.ManufacturerPN
                chkStoreDisplay.Checked = item.StoreDisplay
                chkCrossCharge.Checked = item.CrossCharge
                txtPrice.Text = PriceManager.GetCurrentItemPriceByItemID(item.ItemID).ToString("#,##0.00")
                chkActive.Checked = item.Active
                txtStockQty.Text = item.StockQuantity.ToString()
                txtMin.Text = item.MinStockQuantity.ToString()
                txtKeyWords.Text = item.SearchKeyWords

                LoadImage()

                Dim parent_catid As Integer = BLL.ItemManager.GetParentCatID(item.CategoryID)
                If parent_catid > 0 Then
                    'This item has 2 level of hierarchy
                    ddlCat0.SelectedValue = parent_catid.ToString()
                    Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(parent_catid, 1, False)
                        ddlCat1.DataSource = reader
                        ddlCat1.SelectedValue = item.CategoryID.ToString()
                        ddlCat1.DataBind()
                        reader.Close()
                    End Using
                Else
                    'This item is directly under root category
                    ddlCat0.SelectedValue = item.CategoryID.ToString()
                End If
            Else
                lblItemID.Text = itemId.ToString()
                litSaveError.Text += "<div class=""stock-error"">Fail to load the appropriate item. Please contact the IT support personnel</div>"
            End If

            'To warn user about uploading and overwriting the old image file
            btnUpload.OnClientClick = "return confirm('The old image will be overwritten, are you sure you want to do that?');"

        End Sub

        Private Sub LoadImage()
            Dim itemId As Integer
            If Integer.TryParse(Request.QueryString("item"), itemId) Then
                Dim imageName As String = Item.GetItemImageName(itemId, False)
                imgItem.ImageUrl = GlobalUtility.GetItemImagePath(False) + imageName + "?ts=" + Date.Now.Ticks.ToString()
                imgItem.ToolTip = imageName
            End If
        End Sub

        Protected Sub BtnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
            litSaveError.Text = String.Empty

            Dim itemId As Integer
            If Not Integer.TryParse(Request.QueryString("item"), itemId) Then
                'if there is no new item id in session, we simply redirect user back to item list
                Response.Redirect("~/admin/ItemManager.aspx?tab=1&menu=0")
            Else
                Dim msg As String = String.Empty
                If ImageUtility.UploadFile(itemId, FileUpload1, msg) Then
                    LoadImage()
                End If
                litSaveError.Text = msg
            End If
        End Sub

        Protected Sub DdlCat0_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCat0.SelectedIndexChanged
            Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(CType(ddlCat0.SelectedValue, Integer), 1, False)
                ddlCat1.DataSource = reader
                ddlCat1.DataBind()
                reader.Close()
            End Using
        End Sub

        Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            litSaveError.Text = String.Empty

            Dim itemId As Integer
            If Integer.TryParse(Request.QueryString("item"), itemId) Then

                Dim err As Integer = 0

                If String.IsNullOrEmpty(txtDescription.Text) Then
                    litSaveError.Text += "<div class=""stock-error"">Description is required</div>"
                    err += 1
                End If

                Dim stockQty As Integer
                Dim minQty As Integer

                If Not Integer.TryParse(txtStockQty.Text, stockQty) Then
                    litSaveError.Text += "<div class=""stock-error"">Invalid Stock Quantity</div>"
                    err += 1
                End If

                If Not Integer.TryParse(txtMin.Text, minQty) Then
                    litSaveError.Text += "<div class=""stock-error"">Invalid Minimum Quantity</div>"
                    err += 1
                End If

                If err > 0 Then
                    Return
                End If

                Dim item As New Item With {
                    .ItemID = itemId,
                    .Description = DataSanitizer.SanitizeText(txtDescription.Text),
                    .Notes = DataSanitizer.SanitizeText(txtNotes.Text),
                    .ManufacturerPN = DataSanitizer.SanitizeText(txtManufacturerPN.Text),
                    .StoreDisplay = chkStoreDisplay.Checked,
                    .CrossCharge = chkCrossCharge.Checked,
                    .SearchKeyWords = DataSanitizer.SanitizeText(txtKeyWords.Text),
                    .MinStockQuantity = minQty,
                    .StockQuantity = stockQty,
                    .Active = chkActive.Checked
                }

                If ddlCat1.SelectedValue = Nothing Then
                    item.CategoryID = Integer.Parse(ddlCat0.SelectedValue)
                Else
                    item.CategoryID = Integer.Parse(ddlCat1.SelectedValue)
                End If

                If BLL.ItemManager.UpdateItem(item) Then
                    litSaveError.Text = "<div style=""margin-top: 20px;""><strong>Item data updated successfully.</strong></div>"
                Else
                    litSaveError.Text = "<div class=""stock-error"">Error: Updating item data failed.</div>"
                End If
            Else
                litError.Text = "<div class=""stock-error"">Invalid ItemID</div>"
            End If
        End Sub

        Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Response.Redirect(lbtnBack.PostBackUrl)
        End Sub

        Protected Sub BtnPricing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPricing.Click
            Response.Redirect("~/admin/ItemManagerPricing.aspx" + Request.Url.Query)
        End Sub
    End Class
End Namespace