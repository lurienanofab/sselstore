Public Class NoAccess
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim email As String = ConfigurationManager.AppSettings("StoreAdminEmail")
        If Not String.IsNullOrEmpty(email) Then
            litContact.Text = String.Format("If you have any questions, please send an email to <a href=""mailto:{0}"">{0}</a>.", email)
        Else
            litContact.Text = "If you have any questions, please contact the system administrator."
        End If
    End Sub

End Class