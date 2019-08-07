Imports sselStore.AppCode

Namespace Admin
    Public Class PrintInventory
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'defaults to true, pass false for both web and not-web
            Dim webOnly As Boolean = If(String.IsNullOrEmpty(Request.QueryString("web")), True, Boolean.Parse(Request.QueryString("web")))
            rptInventory.DataSource = BLL.InventoryManager.GetAllInventory(webOnly)
            rptInventory.DataBind()
        End Sub
    End Class
End Namespace