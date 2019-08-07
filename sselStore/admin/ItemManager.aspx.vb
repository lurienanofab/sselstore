Imports sselStore.AppCode

Namespace Admin
    Public Class ItemManager
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'handling the event if user comes back from the detail page
            litSearchMessage.Text = String.Empty
            If Not String.IsNullOrEmpty(Request.QueryString("query")) Then
                txtSearch.Text = Request.QueryString("query")
                gvItem.DataSourceID = "odsItemSearch"
                litSearchMessage.Text = String.Format("<div style=""margin-bottom: 10px;""><a href=""{0}?tab=1&menu=0"">Clear Search</a></div>", VirtualPathUtility.ToAbsolute("~/admin/ItemManager.aspx"))
            End If
        End Sub

        Protected Sub gvItem_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvItem.RowCommand
            If e.CommandName = "detail" Then
                Dim itemID As String = gvItem.DataKeys(CInt(e.CommandArgument)).Value.ToString()
                Response.Redirect("~/admin/ItemManagerDetail.aspx?tab=1&menu=0&item=" & itemID & "&query=" & txtSearch.Text)
            End If
        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            gvItem.DataSourceID = "odsItemSearch"
            litSearchMessage.Text = String.Format("<div style=""margin-bottom: 10px;""><a href=""{0}?tab=1&menu=0"">Clear Search</a></div>", VirtualPathUtility.ToAbsolute("~/admin/ItemManager.aspx"))
        End Sub

        Protected Sub btnCreateNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateNew.Click
            Response.Redirect("~/admin/ItemManagerCreateStep1.aspx?tab=1&menu=0")
        End Sub
    End Class
End Namespace