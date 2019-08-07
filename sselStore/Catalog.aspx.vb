Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Public Class Catalog
    Inherits StorePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Get the item id and then load it
            Dim cID As Integer = 0
            If DataSanitizer.Sanitize(Request.QueryString("cid"), RegexConstants.REGEX_CATID, cID) Then
                Dim hasChild As Boolean = CatalogManager.HasChildCategories(cID)
                Dim hasItems As Boolean = CatalogManager.HasItems(cID)

                divItem.Visible = hasItems
                divItem.Disabled = Not hasItems
                divCat.Visible = hasChild
                divCat.Disabled = Not hasChild
            Else
                litError.Text = "<div class=""error"">Invalid CatID.</div>"
                divItem.Visible = False
            End If
        End If
    End Sub

    Protected Function SubCatImageURL(ByVal catID As String) As String
        Return VirtualPathUtility.ToAbsolute("~/images/items/") + CategoryManager.CategoryImageFileName(Convert.ToInt32(catID))
    End Function

    Protected Function GetImageStyle(ByVal catID As String) As String
        If catID = "12" Then
            Return "thumbimagetweezer"
        Else
            Return "thumbimage"
        End If
    End Function

End Class