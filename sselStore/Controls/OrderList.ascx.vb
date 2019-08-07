Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Controls
    Public Class OrderList
        Inherits System.Web.UI.UserControl

        Public Property AdminMode As Boolean

        Dim detailPageURL As String = "~/OrderDetail.aspx?tab=2&menu=0&so={0}&status={1}"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If AdminMode Then
                spnPrint.Visible = True
                gvStoreOrder.DataSourceID = "OdsOrdersAdmin"
                detailPageURL = "~/admin/OrderManagerDetail.aspx?tab=0&menu=0&so={0}&status={1}"
                gvStoreOrder.Columns(2).Visible = True
            End If

            If Not IsPostBack Then
                'Used to load appropriate order type when user clicks back button on OrderManDetail.aspx page
                Dim orderType As String = String.Empty
                If DataSanitizer.Sanitize(Request.QueryString("status"), RegexConstants.REGEX_ORDERTYPE, orderType) Then
                    ddlStatus.SelectedValue = orderType
                Else
                    'Error happen, it's not big deal, because it's used only for display purpose
                    'So we load default order type value
                    ddlStatus.SelectedValue = AppConstants.STORE_ORDER_STATUS_OPEN
                End If
            End If
        End Sub

        Protected Sub gvStoreOrder_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStoreOrder.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim soid As HyperLink = CType(e.Row.Cells(1).Controls(0), HyperLink)
                soid.NavigateUrl = getOrderDetailURL(Convert.ToInt32(soid.Text))
                Dim currentStatus As String = e.Row.Cells(4).Text
                Dim imgDelete As Image = CType(e.Row.Cells(0).Controls(1), Image)
                Dim imgEdit As ImageButton = CType(e.Row.Cells(6).Controls(1), ImageButton)
                If currentStatus <> AppConstants.STORE_ORDER_STATUS_OPEN AndAlso Not AdminMode Then
                    imgDelete.Visible = False 'delete button
                    imgEdit.Visible = False 'edit button
                End If

                If AdminMode Then
                    imgEdit.Visible = False 'admin didn't have the option to put order back to cart
                    If currentStatus <> AppConstants.STORE_ORDER_STATUS_OPEN Then
                        imgDelete.Visible = False 'delete button
                    End If
                End If

                'Check if any order is over 14 days old
                If ddlStatus.SelectedValue = "Fulfilled" Then
                    Dim mydate As DateTime = DateTime.Parse(e.Row.Cells(3).Text)
                    Dim orderExpireCutoff As DateTime = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings("OrderExpireDays")))
                    If mydate < orderExpireCutoff Then
                        e.Row.BackColor = System.Drawing.Color.Pink
                    End If
                End If
            End If
        End Sub

        Protected Sub gvStoreOrder_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStoreOrder.RowCommand
            If e.CommandName = "Detail" Then
                Dim soid As String = gvStoreOrder.DataKeys(Convert.ToInt32(e.CommandArgument)).Value.ToString()
                Response.Redirect(String.Format(detailPageURL, soid, ddlStatus.SelectedValue))
            ElseIf e.CommandName = "MoveToCart" Then
                'First copy all the items into Session
                Dim soid As Integer = CType(e.CommandArgument, Integer)
                Dim order As StoreOrderItems = StoreOrderManager.GetStoreOrderDetail(soid)
                Session("Cart") = order

                'second, delete the order
                StoreOrderManager.DeleteOrder(soid)

                Response.Redirect("~/Cart.aspx?tab=1&edit=1")
            End If
        End Sub

        Private Function getOrderDetailURL(ByVal soid As Integer) As String
            Return VirtualPathUtility.ToAbsolute(String.Format(detailPageURL, soid, ddlStatus.SelectedValue))
        End Function

    End Class
End Namespace