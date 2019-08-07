Imports sselStore.AppCode

Namespace Admin
    Public Class OrderManager
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub ddlFobClient_DataBound(sender As Object, e As EventArgs)
            ddlFobClient.Items.Insert(0, New ListItem("-- Select a Client --", "-1"))
        End Sub
    End Class
End Namespace