Imports sselStore.AppCode

Public Class Search
    Inherits StorePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("q") IsNot Nothing Then
            lblSubTitle.Text = String.Format("Search for '{0}'", Request.QueryString("q"))
        End If
    End Sub

End Class