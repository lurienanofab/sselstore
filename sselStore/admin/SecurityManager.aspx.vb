Imports sselStore.AppCode

Namespace Admin
    Public Class SecurityManager
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                gvSecurityItems.DataSource = BLL.ItemManager.SecurityItems()
                gvSecurityItems.DataBind()
            End If
        End Sub

        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)

            litAddErrorMessage.Text = String.Empty

            Dim id As Integer

            If txtItemID.Text = String.Empty Then
                litAddErrorMessage.Text = "Please enter an Item ID."
                Return
            End If

            If Integer.TryParse(txtItemID.Text, id) Then
                Dim result As Boolean = BLL.ItemManager.AddSecurityItem(id)

                If result Then
                    gvSecurityItems.DataSource = BLL.ItemManager.SecurityItems()
                    gvSecurityItems.DataBind()
                Else
                    litAddErrorMessage.Text = "That item has already been added."
                End If
            Else
                litAddErrorMessage.Text = "Item ID must be an integer."
            End If
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs)
            For Each gvr As GridViewRow In gvSecurityItems.Rows
                Dim itemId As Integer = Integer.Parse(gvr.Cells(0).Text)
                Dim active As Boolean = CType(gvr.Cells(2).FindControl("chkActive"), CheckBox).Checked

                BLL.ItemManager.UpdateSecurityItem(itemID, active)

                gvSecurityItems.DataSource = BLL.ItemManager.SecurityItems()
                gvSecurityItems.DataBind()
            Next
        End Sub
    End Class
End Namespace