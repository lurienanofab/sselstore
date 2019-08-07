Imports LNF.Repository
Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Admin
    Public Class ItemManagerCreateStep1
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Page.DataBind()
            End If
        End Sub

        Private Sub InitializeControls()
            'Second level Category dropdownlist
            Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(1, 1, False)
                ddlCat1.DataSource = reader
                ddlCat1.DataBind()
                reader.Close()
            End Using
        End Sub

        Protected Sub DdlCat0_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCat0.SelectedIndexChanged
            Using reader As ExecuteReaderResult = CatalogManager.GetCategoryListByHierarchy(Integer.Parse(ddlCat0.SelectedValue), 1, False)
                ddlCat1.DataSource = reader
                ddlCat1.DataBind()
                reader.Close()
            End Using
        End Sub

        Protected Sub DdlCat1_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCat1.DataBound
            If ddlCat1.Items.Count > 0 Then
                ddlCat1.Items.Insert(0, New ListItem("[None]", "-1"))
            End If
        End Sub

        Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect("~/admin/ItemManager.aspx?tab=1&menu=0")
        End Sub

        Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs)
            litSaveError.Text = String.Empty

            Dim err As Integer = 0

            If String.IsNullOrEmpty(txtDescription.Text) Then
                litSaveError.Text += "<div class=""stock-error"">Description is required</div>"
                err += 1
            End If

            If String.IsNullOrEmpty(txtManufacturerPN.Text) Then
                litSaveError.Text += "<div class=""stock-error"">Part # is required</div>"
                err += 1
            End If

            Dim stockQty As Integer
            Dim minQty As Integer

            If Not Integer.TryParse(txtStockQty.Text, stockQty) Then
                litSaveError.Text += "<div class=""stock-error"">Invalid Initial Stock Quantity</div>"
                err += 1
            End If

            If Not Integer.TryParse(txtMin.Text, minQty) Then
                litSaveError.Text += "<div class=""stock-error"">Invalid Minimum Quantity</div>"
                err += 1
            End If

            If err > 0 Then
                Return
            End If

            'Save the new item
            Dim newItem As New Item With {
                .Description = txtDescription.Text,
                .ManufacturerPN = txtManufacturerPN.Text,
                .StoreDisplay = chkStoreDisplay.Checked,
                .CrossCharge = chkCrossCharge.Checked,
                .SearchKeyWords = txtKeyWords.Text
            }

            If ddlCat1.SelectedValue = Nothing OrElse ddlCat1.SelectedValue = "-1" Then
                newItem.CategoryID = Integer.Parse(ddlCat0.SelectedValue)
            Else
                newItem.CategoryID = Integer.Parse(ddlCat1.SelectedValue)
            End If

            newItem.Active = chkActive.Checked
            newItem.StockQuantity = stockQty
            newItem.MinStockQuantity = minQty

            Dim itemId As Integer = 0

            If BLL.ItemManager.InsertItem(newItem, itemId) Then
                Response.Redirect(String.Format("~/admin/ItemManagerCreateStep2.aspx?tab=1&menu=0&item={0}", itemId))
            Else
                litSaveError.Text += "<div class=""stock-error"">Unable to save new item. A database problem occurred.</div>"
            End If
        End Sub
    End Class
End Namespace