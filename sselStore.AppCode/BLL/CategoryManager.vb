Imports System.IO
Imports System.Web
Imports LNF.Repository

Namespace BLL
    Public Class CategoryManager
        Public Shared Function SelectParents() As DataTable
            Return DA.Command(CommandType.Text).FillDataTable("SELECT * FROM sselMAS.dbo.Category WHERE ParentID = CatID ORDER BY ParentID, HierarchyLevel, CatName")
        End Function

        Public Shared Function SelectByParentID(ByVal parentId As Integer) As DataTable
            If parentId = -1 Then
                Return SelectParents()
            Else
                Return DA.Command(CommandType.Text) _
                    .Param("ParentID", parentId) _
                    .FillDataTable("SELECT * FROM sselMAS.dbo.Category WHERE ParentID = @ParentID ORDER BY HierarchyLevel, CatName")
            End If
        End Function

        Public Shared Function Update(ByVal catId As Integer, ByVal parentId As Integer, ByVal hierarchy As Integer, ByVal name As String, ByVal desc As String, ByVal active As Boolean, ByVal display As Boolean) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("CatID", catId) _
                .Param("ParentID", parentId) _
                .Param("HierarchyLevel", hierarchy) _
                .Param("CatName", name) _
                .Param("Description", String.IsNullOrEmpty(desc), desc, DBNull.Value) _
                .Param("Active", active) _
                .Param("StoreDisplay", display) _
                .ExecuteNonQuery("UPDATE sselMAS.dbo.Category SET ParentID = @ParentID, HierarchyLevel = @HierarchyLevel, CatName = @CatName, Description = @Description, Active = @Active, StoreDisplay = @StoreDisplay WHERE CatID = @CatID").Value
        End Function

        Public Shared Function AddParent(ByVal hierarchy As Integer, ByVal name As String, ByVal desc As String) As Integer
            Dim catId As Integer = AddChild(0, hierarchy, name, desc)
            Return DA.Command(CommandType.Text).Param("CatID", catId).ExecuteNonQuery("UPDATE sselMAS.dbo.Category SET ParentID = CatID WHERE CatID = @CatID").Value
        End Function

        Public Shared Function AddChild(ByVal parentId As Integer, ByVal hierarchy As Integer, ByVal name As String, ByVal desc As String) As Integer
            Return DA.Command(CommandType.Text) _
                .Param("ParentID", parentId) _
                .Param("HierarchyLevel", hierarchy) _
                .Param("CatName", name) _
                .Param("Description", String.IsNullOrEmpty(desc), DBNull.Value, desc) _
                .ExecuteScalar(Of Integer)("INSERT sselMAS.dbo.Category (ParentID, HierarchyLevel, CatName, Description, Active, StoreDisplay) VALUES (@ParentID, @HierarchyLevel, @CatName, @Description, 1, 1); SELECT SCOPE_IDENTITY() AS 'CatID'").Value
        End Function

        Public Shared Function CategoryImageFileName(ByVal catID As Integer) As String
            Dim fileName As String = String.Format("cat{0}.jpg", catID)
            If File.Exists(HttpContext.Current.Server.MapPath("~/images/items/") + fileName) Then
                Return fileName
            Else
                Return "default.jpg"
            End If
        End Function
    End Class
End Namespace