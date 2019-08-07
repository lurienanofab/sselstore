Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Admin
    Public Class SearchOrder
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadClients()
                txtStartDate.Text = MonthStartDate(DateTime.Now).ToString("M/d/yyyy")
                txtEndDate.Text = DateTime.Now.Date.ToString("M/d/yyyy")
            End If
        End Sub

        Private Function MonthStartDate(d As DateTime) As DateTime
            Return New Date(d.Year, d.Month, 1)
        End Function

        Private Sub LoadClients()
            Dim dt As DataTable

            Select Case rblUserType.SelectedValue
                Case "external"
                    dt = AdminManager.GetExternalClients()
                Case "internal"
                    dt = AdminManager.GetInternalClients()
                Case "staff"
                    dt = AdminManager.GetStaff()
                Case Else
                    dt = AdminManager.GetAllClients()
            End Select

            ddlUser.DataSource = dt
            ddlUser.DataBind()
        End Sub

        Private Sub LoadOrders()
            rptOrders.DataSource = StoreOrderManager.GetClosedOrderList(Integer.Parse(ddlUser.SelectedValue), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text))
            rptOrders.DataBind()
        End Sub

        Protected Sub rblUserType_SelectedIndexChanged(sender As Object, e As EventArgs)
            LoadClients()
        End Sub

        Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
            LoadOrders()
        End Sub
    End Class
End Namespace