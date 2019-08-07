Namespace Controls
    Public Class AdminSideMenu
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Private Sub ApplyDisplayStyle(link As HyperLink, drv As DataRowView)

            Dim inTabId As String = drv("TabID").ToString()
            Dim inMenuId As String = drv("MenuID").ToString()

            'We don't bother to check the validity of the querystring data, since it's only used for artistic display purpose
            Dim tabId As String = Request.QueryString("tab")
            Dim menuId As String = Request.QueryString("menu")

            If inTabId = tabId Then
                If inMenuId = menuId Then
                    link.Font.Bold = True
                    link.Font.Underline = True
                    'Return "font-weight:bold;text-decoration: underline;"
                End If
            End If

            'Check if first time coming to this site
            If String.IsNullOrEmpty(tabId) AndAlso String.IsNullOrEmpty(menuId) AndAlso inTabId = "0" AndAlso inMenuId = "0" Then
                link.Font.Bold = True
                link.Font.Underline = True
                'Return "font-weight:bold;text-decoration: underline;"
            End If

            'For everything else, return empty style
            'Return String.Empty
        End Sub

        Protected Sub rptMenu_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim link As HyperLink = CType(e.Item.FindControl("hypMenuItem"), HyperLink)
                Dim drv As DataRowView = CType(e.Item.DataItem, DataRowView)
                ApplyDisplayStyle(link, drv)
            End If
        End Sub
    End Class
End Namespace