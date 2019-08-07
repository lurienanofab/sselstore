Imports sselStore.AppCode

Namespace Admin
    Public Class InventoryManager
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            gvInv.DataSourceID = Nothing
            gvInv.DataSourceID = "odsInvSearch"
        End Sub

        Protected Sub gvInv_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvInv.RowCancelingEdit
            lblErrorDateFormat.Text = ""
        End Sub

        Protected Sub gvInv_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInv.RowCreated
            If e.Row.RowType = DataControlRowType.Header Then
                'Generate an extra header in the grid view
                Dim extraHeader As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)

                Dim genericTD As New TableCell
                Dim stockTD As New TableCell
                Dim neworderTD As New TableCell
                Dim storeTD As New TableCell
                Dim emptyTD As New TableCell

                genericTD.ColumnSpan = 3
                genericTD.Text = "Generic"
                genericTD.HorizontalAlign = HorizontalAlign.Center
                extraHeader.Cells.Add(genericTD)

                stockTD.ColumnSpan = 2
                stockTD.Text = "Stock"
                stockTD.HorizontalAlign = HorizontalAlign.Center
                extraHeader.Cells.Add(stockTD)

                neworderTD.ColumnSpan = 3
                neworderTD.Text = "New IOF Order"
                neworderTD.HorizontalAlign = HorizontalAlign.Center
                extraHeader.Cells.Add(neworderTD)

                storeTD.ColumnSpan = 2
                storeTD.Text = "Store"
                storeTD.HorizontalAlign = HorizontalAlign.Center
                extraHeader.Cells.Add(storeTD)

                emptyTD.ColumnSpan = 1
                extraHeader.Cells.Add(emptyTD)

                gvInv.Controls(0).Controls.AddAt(0, extraHeader)
            End If
        End Sub

        Protected Sub gvInv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInv.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cell_index_image As Integer = 1
                Dim availQ As Integer
                Dim minQ As Integer

                'Column Arrangement: 0.MPN, 1.Status Icon, 2.Description, 3.MinStock, 4.StockQuantity, 5.OrderQuantity, 6.OrderDate, 7.Est Date, 8.StockAvailable, 9.StockOnReserve, 
                'When gridview is loading data normally
                If e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate Then
                    'availQ = CType(CType(e.Row.Cells(cell_index_image + 7).Controls(0), Label).Text, Integer)
                    Try
                        availQ = CType(e.Row.Cells(cell_index_image + 7).Text, Integer)
                    Catch ex As Exception
                        availQ = 0
                    End Try

                    'minQ = CType(CType(e.Row.Cells(cell_index_image + 2).Controls(1), Label).Text, Integer)
                    minQ = CType(e.Row.Cells(cell_index_image + 2).Text, Integer)

                    'When user clicks Edit button
                ElseIf (e.Row.RowState And DataControlRowState.Edit) <> 0 Then
                    'Since StockAvailable is readonly, so we cannot include here as one column in edit mode
                    Try
                        availQ = CType(e.Row.Cells(cell_index_image + 7).Text, Integer)
                    Catch ex As Exception
                        availQ = 0
                    End Try

                    'Dim txtMinQ As TextBox = e.Row.Cells(cell_index_image + 2).Controls(1)
                    'minQ = CType(txtMinQ.Text, Integer)
                    minQ = CType(e.Row.Cells(cell_index_image + 2).Text, Integer)

                    'Change the Date column display with date only (excluding time component)
                    Dim txtOrderDate As TextBox = CType(e.Row.Cells(cell_index_image + 5).Controls(0), TextBox)
                    txtOrderDate.Width = 80
                    If txtOrderDate.Text <> Nothing Then
                        Dim temp As DateTime = CType(txtOrderDate.Text, DateTime)
                        txtOrderDate.Text = temp.Month.ToString() & "/" & temp.Day.ToString() & "/" & temp.Year.ToString()
                    End If

                    Dim txtEst As TextBox = CType(e.Row.Cells(cell_index_image + 6).Controls(0), TextBox)
                    txtEst.Width = 80
                    If txtEst.Text <> Nothing Then
                        Dim temp As DateTime = CType(txtEst.Text, DateTime)
                        txtEst.Text = temp.Month.ToString() & "/" & temp.Day.ToString() & "/" & temp.Year.ToString()
                    End If
                End If

                Dim img As System.Web.UI.WebControls.Image = CType(e.Row.Cells(cell_index_image).Controls(1), Image)
                If availQ > minQ Then
                    img.ImageUrl = GlobalUtility.GetIconImagePath + "stock_ok.gif"
                ElseIf availQ <= 0 Then
                    img.ImageUrl = GlobalUtility.GetIconImagePath + "stock_zero.gif"
                Else
                    img.ImageUrl = GlobalUtility.GetIconImagePath + "stock_warning.gif"
                End If
            End If
        End Sub

        Protected Sub gvInv_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvInv.RowUpdated
            lblErrorDateFormat.Text = ""
        End Sub

        Protected Sub gvInv_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvInv.RowUpdating
            Try
                'DateTime datatype format checking, I don't do it in regular expression validator becuase the regex is too difficult to create, 
                'so I rely on the .NET DateTime type to check for me
                Dim new_OrderDate As DateTime = CType(e.NewValues("OrderDate"), DateTime)
                Dim new_ArrivalDate As DateTime = CType(e.NewValues("EstimatedArrivalDate"), DateTime)
            Catch ex As Exception
                'either one of the date format is bad, so we cancel the operation
                lblErrorDateFormat.Text = "* Date is incorrect - Please make sure it's mm/dd/yyyy"

                e.Cancel = True
            End Try
        End Sub

        Protected Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
            gvInv.DataSourceID = Nothing
            gvInv.DataSourceID = "odsInvSearch"
        End Sub
    End Class
End Namespace