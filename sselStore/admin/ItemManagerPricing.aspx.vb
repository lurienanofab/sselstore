Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Admin
    Public Class ItemManagerPricing
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            litItemID.Text = Request.QueryString("item")

            If Not Page.IsPostBack Then
                If Not String.IsNullOrEmpty(Request.QueryString("item")) Then
                    rptPackage.DataSource = PriceManager.Packages(Integer.Parse(Request.QueryString("item")))
                    rptPackage.DataBind()
                    SetCurrentPrice()
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("pkgid")) Then
                    litPackageID.Text = Request.QueryString("pkgid")
                    hidCurrentPackageID.Value = Request.QueryString("pkgid")
                    SelectPackage()
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("vpid")) Then
                    hidCurrentVendorPackageID.Value = Request.QueryString("vpid")
                    rptVendorPackage.DataSource = PriceManager.SelectVendorPackagesByPackageID(Convert.ToInt32(hidCurrentPackageID.Value))
                    rptVendorPackage.DataBind()
                    divVendorPackage.Visible = True
                    SelectVendorPackage()

                    litPriceVendorPackageID.Text = Request.QueryString("vpid")
                    rptPrice.DataSource = PriceManager.Prices(Convert.ToInt32(hidCurrentVendorPackageID.Value))
                    rptPrice.DataBind()
                    divPrice.Visible = True
                End If
            End If
        End Sub

        Private Sub SetCurrentPrice()
            Dim dr As DataRow = PriceManager.GetItemHistoricalInfo(Convert.ToInt32(Request.QueryString("item")), DateTime.Now)
            Dim url As String = Request.Url.GetLeftPart(UriPartial.Path) + "?"
            url += "tab=" + Request.QueryString("tab")
            url += "&menu=" + Request.QueryString("menu")
            url += "&item=" + Request.QueryString("item")
            url += "&query=" + Request.QueryString("query")
            url += "&pkgid=" + dr("PackageID").ToString()
            url += "&vpid=" + dr("VendorPackageID").ToString()
            litCurrentPrice.Text = String.Format("<a href=""{0}"">{1}</a> [{2}/unit] <span style=""font-weight: normal; font-style: italic; color: #696969;"">(Date Active: {3})</span>", url, Convert.ToDouble(dr("PackagePrice")).ToString("C"), Convert.ToDouble(dr("UnitPrice")).ToString("C"), dr("MaxDateActive"))
        End Sub

        Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(String.Format("~/admin/ItemManagerDetail.aspx?tab={0}&menu={1}&item={2}&query={3}", Request.QueryString("tab"), Request.QueryString("menu"), Request.QueryString("item"), Request.QueryString("query")))
        End Sub

        Protected Sub btnPackageAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim footer As Control = rptPackage.Controls(rptPackage.Controls.Count - 1)
            Dim add_desc As String = CType(footer.FindControl("txtAddPackageDescription"), TextBox).Text
            Dim add_qty As String = CType(footer.FindControl("txtAddPackageBaseQty"), TextBox).Text
            Dim lit As Literal = CType(footer.FindControl("litPackageAddError"), Literal)

            If add_desc = String.Empty Then
                lit.Text = "Please enter a description."
                Return
            End If

            If add_qty = String.Empty Then
                lit.Text = "Please enter a base quantity."
                Return
            End If

            Dim baseq As Integer
            If Not Integer.TryParse(add_qty, baseq) Then
                lit.Text = "Base quantity must be an integer."
                Return
            End If

            PriceManager.AddPackage(Convert.ToInt32(Request.QueryString("item")), add_desc, baseq)

            rptPackage.DataSource = PriceManager.Packages(Convert.ToInt32(Request.QueryString("item")))
            rptPackage.DataBind()
        End Sub

        Protected Sub btnPackageEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim edit_item As RepeaterItem = Nothing

            For Each i As RepeaterItem In rptPackage.Items
                If CType(i.FindControl("lblPackageID"), Label).Text = e.CommandArgument.ToString() Then
                    edit_item = i
                End If
            Next

            If edit_item IsNot Nothing Then
                Select Case e.CommandName
                    Case "edit"
                        edit_item.FindControl("lblPackageDescription").Visible = False
                        edit_item.FindControl("txtPackageDescriptionEdit").Visible = True

                        edit_item.FindControl("lblPackageBaseQty").Visible = False
                        edit_item.FindControl("txtPackageBaseQtyEdit").Visible = True

                        CType(edit_item.FindControl("chkPackageActive"), CheckBox).Enabled = True

                        edit_item.FindControl("divPackageEditButton").Visible = False
                        edit_item.FindControl("divPackageSaveCancelButton").Visible = True
                    Case Else
                        edit_item.FindControl("lblPackageDescription").Visible = True
                        edit_item.FindControl("txtPackageDescriptionEdit").Visible = False

                        edit_item.FindControl("lblPackageBaseQty").Visible = True
                        edit_item.FindControl("txtPackageBaseQtyEdit").Visible = False

                        CType(edit_item.FindControl("chkPackageActive"), CheckBox).Enabled = False

                        edit_item.FindControl("divPackageEditButton").Visible = True
                        edit_item.FindControl("divPackageSaveCancelButton").Visible = False

                        If e.CommandName = "save" Then
                            Dim desc As String = CType(edit_item.FindControl("txtPackageDescriptionEdit"), TextBox).Text
                            Dim qty As Integer = Convert.ToInt32(CType(edit_item.FindControl("txtPackageBaseQtyEdit"), TextBox).Text)
                            Dim active As Boolean = CType(edit_item.FindControl("chkPackageActive"), CheckBox).Checked
                            PriceManager.UpdatePackage(Convert.ToInt32(e.CommandArgument), desc, qty, active)

                            rptPackage.DataSource = PriceManager.Packages(Convert.ToInt32(Request.QueryString("item")))
                            rptPackage.DataBind()
                        End If
                End Select
            End If
        End Sub

        Protected Sub lnkVendorPackage_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            litPackageID.Text = e.CommandArgument.ToString()
            hidCurrentPackageID.Value = e.CommandArgument.ToString()
            rptVendorPackage.DataSource = PriceManager.SelectVendorPackagesByPackageID(Convert.ToInt32(e.CommandArgument))
            rptVendorPackage.DataBind()
            divVendorPackage.Visible = True
            divPrice.Visible = False
            SelectPackage()
        End Sub

        Private Sub SelectPackage()
            Dim view_item As RepeaterItem = Nothing

            For Each i As RepeaterItem In rptPackage.Items
                CType(i.FindControl("trPackageItem"), HtmlTableRow).Style("background-color") = "#FFFFFF"
                If CType(i.FindControl("lblPackageID"), Label).Text = hidCurrentPackageID.Value Then
                    view_item = i
                End If
            Next

            If view_item IsNot Nothing Then
                CType(view_item.FindControl("trPackageItem"), HtmlTableRow).Style("background-color") = "#FFFFCC"
            End If
        End Sub

        Protected Sub lnkCost_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            litPriceVendorPackageID.Text = e.CommandArgument.ToString()
            hidCurrentVendorPackageID.Value = e.CommandArgument.ToString()
            rptPrice.DataSource = PriceManager.Prices(Convert.ToInt32(e.CommandArgument))
            rptPrice.DataBind()
            divPrice.Visible = True
            SelectVendorPackage()
        End Sub

        Private Sub SelectVendorPackage()
            Dim view_item As RepeaterItem = Nothing

            For Each i As RepeaterItem In rptVendorPackage.Items
                CType(i.FindControl("trVendorPackageItem"), HtmlTableRow).Style("background-color") = "#FFFFFF"
                If CType(i.FindControl("lblVendorPackageID"), Label).Text = hidCurrentVendorPackageID.Value Then
                    view_item = i
                End If
            Next

            If view_item IsNot Nothing Then
                CType(view_item.FindControl("trVendorPackageItem"), HtmlTableRow).Style("background-color") = "#FFFFCC"
            End If
        End Sub

        Protected Sub btnVendorPackageEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim edit_item As RepeaterItem = Nothing

            For Each i As RepeaterItem In rptVendorPackage.Items
                If CType(i.FindControl("lblVendorPackageID"), Label).Text = e.CommandArgument.ToString() Then
                    edit_item = i
                End If
            Next

            If edit_item IsNot Nothing Then
                Select Case e.CommandName
                    Case "edit"
                        edit_item.FindControl("lblVendorPackageVendor").Visible = False
                        edit_item.FindControl("txtVendorPackageVendorEdit").Visible = True

                        edit_item.FindControl("lblVendorPackageNumber").Visible = False
                        edit_item.FindControl("txtVendorPackageNumberEdit").Visible = True

                        CType(edit_item.FindControl("chkVendorPackageActive"), CheckBox).Enabled = True

                        edit_item.FindControl("divVendorPackageEditButton").Visible = False
                        edit_item.FindControl("divVendorPackageSaveCancelButton").Visible = True
                    Case Else
                        edit_item.FindControl("lblVendorPackageVendor").Visible = True
                        edit_item.FindControl("txtVendorPackageVendorEdit").Visible = False

                        edit_item.FindControl("lblVendorPackageNumber").Visible = True
                        edit_item.FindControl("txtVendorPackageNumberEdit").Visible = False

                        CType(edit_item.FindControl("chkVendorPackageActive"), CheckBox).Enabled = False

                        edit_item.FindControl("divVendorPackageEditButton").Visible = True
                        edit_item.FindControl("divVendorPackageSaveCancelButton").Visible = False

                        If e.CommandName = "save" Then
                            litVendorPackageUpdateError.Text = String.Empty

                            Dim vendorID As Integer
                            If Not Integer.TryParse(CType(edit_item.FindControl("txtVendorPackageVendorEdit"), TextBox).Text, vendorID) Then
                                litVendorPackageUpdateError.Text = "<div style=""color: #FF0000; margin-bottom: 10px;"">Vendor must be an integer.</div>"
                                Return
                            End If

                            Dim vendorPN As String = CType(edit_item.FindControl("txtVendorPackageNumberEdit"), TextBox).Text
                            Dim active As Boolean = CType(edit_item.FindControl("chkVendorPackageActive"), CheckBox).Checked
                            PriceManager.UpdateVendorPackage(Convert.ToInt32(e.CommandArgument), vendorID, vendorPN, active)

                            rptVendorPackage.DataSource = PriceManager.SelectVendorPackagesByPackageID(Convert.ToInt32(hidCurrentPackageID.Value))
                            rptVendorPackage.DataBind()

                            SetCurrentPrice()
                        End If
                End Select
            End If
        End Sub

        Protected Sub btnVendorPackageAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim footer As Control = rptVendorPackage.Controls(rptVendorPackage.Controls.Count - 1)
            Dim add_vendor As String = CType(footer.FindControl("txtAddVendorPackageVendor"), TextBox).Text
            Dim add_vpn As String = CType(footer.FindControl("txtAddVendorPackageNumber"), TextBox).Text
            Dim lit As Literal = CType(footer.FindControl("litVendorPackageAddError"), Literal)

            If add_vendor = String.Empty Then
                lit.Text = "Please enter a vendor."
                Return
            End If

            Dim vendorID As Integer
            If Not Integer.TryParse(add_vendor, vendorID) Then
                lit.Text = "Vendor must be an integer."
                Return
            End If

            If add_vpn = String.Empty Then
                lit.Text = "Please enter a vendor package number."
                Return
            End If

            PriceManager.AddVendorPackage(Convert.ToInt32(hidCurrentPackageID.Value), vendorID, add_vpn)

            rptVendorPackage.DataSource = PriceManager.SelectVendorPackagesByPackageID(Convert.ToInt32(hidCurrentPackageID.Value))
            rptVendorPackage.DataBind()

            SetCurrentPrice()
        End Sub

        Protected Sub btnPriceAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim footer As Control = rptPrice.Controls(rptPrice.Controls.Count - 1)
            Dim add_qty As String = CType(footer.FindControl("txtAddPriceBreakQty"), TextBox).Text
            Dim add_cost As String = CType(footer.FindControl("txtAddPricePackageCost"), TextBox).Text
            Dim add_markup As String = CType(footer.FindControl("txtAddPricePackageMarkup"), TextBox).Text
            Dim lit As Literal = CType(footer.FindControl("litPriceAddError"), Literal)

            If add_qty = String.Empty Then
                lit.Text = "Please enter a break quantity."
                Return
            End If

            Dim breakq As Integer
            If Not Integer.TryParse(add_qty, breakq) Then
                lit.Text = "Break quantity must be an integer."
                Return
            End If

            If add_cost = String.Empty Then
                lit.Text = "Please enter a package cost."
                Return
            End If

            Dim cost As Double
            If Not Double.TryParse(add_cost, cost) Then
                lit.Text = "Package cost must be numeric."
                Return
            End If

            Dim markup As Double
            If Not Double.TryParse(add_markup, markup) Then
                markup = 0
            End If

            PriceManager.AddPrice(Convert.ToInt32(hidCurrentVendorPackageID.Value), cost, markup, breakq)

            rptPrice.DataSource = PriceManager.Prices(Convert.ToInt32(hidCurrentVendorPackageID.Value))
            rptPrice.DataBind()

            SetCurrentPrice()
        End Sub

        Protected Sub rptPrice_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            Dim last_item As RepeaterItem = Nothing

            For Each i As RepeaterItem In rptPrice.Items
                CType(i.FindControl("trPriceItem"), HtmlTableRow).Style("color") = "#808080"
                CType(i.FindControl("trPriceItem"), HtmlTableRow).Style("font-weight") = "normal"
                last_item = i
            Next

            If last_item IsNot Nothing Then
                CType(last_item.FindControl("trPriceItem"), HtmlTableRow).Style("color") = "#000000"
                CType(last_item.FindControl("trPriceItem"), HtmlTableRow).Style("font-weight") = "bold"
            End If
        End Sub
    End Class
End Namespace