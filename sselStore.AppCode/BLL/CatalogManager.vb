Imports LNF.Repository

Namespace BLL
    Public Class CatalogManager
        Public Shared Function GetItemsByCategory(ByVal catId As Integer) As ExecuteReaderResult
            Return DA.Command().Param("CatID", catId).ExecuteReader("sselMAS.dbo.Item_GetListByCategory")
        End Function

        Public Shared Function GetCategoryListByHierarchy(ByVal catId As Integer, ByVal level As Integer, ByVal webOnly As Boolean) As ExecuteReaderResult
            Return DA.Command() _
                .Param("Action", "ByIDAndHierarchy") _
                .Param("CatID", catId) _
                .Param("HierarchyLevel", level) _
                .Param("WebStoreOnly", webOnly) _
                .ExecuteReader("sselMAS.dbo.Category_Select")
        End Function

        Public Shared Function GetAllSubCategory(ByVal catId As Integer) As ExecuteReaderResult
            Return DA.Command().Param("Action", "AllChildNodes").Param("CatID", catId).ExecuteReader("sselMAS.dbo.Category_Select")
        End Function

        Public Shared Function HasChildCategories(ByVal catId As Integer) As Boolean
            Return DA.Command() _
                .Param("Action", "OneChildNode") _
                .Param("CatID", catId) _
                .ExecuteReader("sselMAS.dbo.Category_Select") _
                .ReadAndClose()
        End Function

        Public Shared Function HasItems(ByVal catId As Integer) As Boolean
            Return DA.Command() _
                .Param("Action", "ByCatID") _
                .Param("CatID", catId) _
                .ExecuteReader("sselMAS.dbo.Item_Select") _
                .ReadAndClose()
        End Function
    End Class
End Namespace