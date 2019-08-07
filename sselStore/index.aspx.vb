Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class Index
    Inherits StorePage

    Protected Function CanEditStoreNews() As Boolean
        Return StoreSettings.CanEditStoreNews()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' check to see if session is valid
            If Request.QueryString.Count > 0 Then ' probably coming from sselOnLine
                If Not String.IsNullOrEmpty(Request.QueryString("ClientID")) Then
                    Dim clientId As Integer
                    If Integer.TryParse(Request.QueryString("ClientID"), clientId) AndAlso Session("ClientID") IsNot Nothing Then
                        If Convert.ToInt32(Session("ClientID")) <> clientId Then
                            Session.Abandon()
                            Response.Redirect("~")
                        End If
                    End If
                End If
            End If

            'If AllowStoreNewsEdit Then
            '    txtStoreNews.Text = StoreSettings.StoreNews
            'End If

            'litStoreNews.Text = StoreSettings.StoreNews
            'Else
            '    If hidDoStoreNewsEdit.Value.Equals("1") Then
            '        SaveStoreNews()
            '    End If
        End If

        lblUserName.Text = CurrentUser.DisplayName

        phAdminEdit.Visible = CanEditStoreNews()
    End Sub

    Private Sub SaveStoreNews()
        'Dim news As String = Server.HtmlDecode(txtStoreNews.Text)
        'StoreSettings.StoreNews = news
        'txtStoreNews.Text = StoreSettings.StoreNews
        'litStoreNews.Text = StoreSettings.StoreNews
        'hidDoStoreNewsEdit.Value = "0"
    End Sub
End Class