Imports sselStore.AppCode

Public Class Contact
    Inherits StorePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lnkStoreAdminEmail.Text = ConfigurationManager.AppSettings("StoreAdminEmail")
        lnkStoreAdminEmail.NavigateUrl = "mailto:" + ConfigurationManager.AppSettings("StoreAdminEmail")

        lnkWebAdminEmail.Text = ConfigurationManager.AppSettings("WebAdminEmail")
        lnkWebAdminEmail.NavigateUrl = "mailto:" + ConfigurationManager.AppSettings("WebAdminEmail")
    End Sub
End Class