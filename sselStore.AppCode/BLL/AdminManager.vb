Imports LNF.Repository

Namespace BLL
    Public Class AdminManager
        Public Shared Function GetSideMenuList(ByVal tabid As String) As DataTable
            Dim table As New DataTable
            Dim columns As DataColumnCollection = table.Columns

            columns.Add("MenuName", GetType(String))
            columns.Add("TabID", GetType(String))
            columns.Add("MenuID", GetType(String))
            columns.Add("MenuURL", GetType(String))

            Dim menu As New Dictionary(Of String, String)

            Dim dataRow As DataRow
            Select Case tabid
                'Data value format ("tab id", "menu id", "URL")
                Case "1"
                    menu.Add("Item Manager", "1,0,ItemManager.aspx")
                    'menu.Add("Kit Manager", "1,1,KitManager.aspx")
                    menu.Add("Category Manager", "1,1,CategoryManager.aspx")
                    menu.Add("Security Manager", "1,2,SecurityManager.aspx")
                Case Else
                    menu.Add("Order Manager", "0,0,OrderManager.aspx")
                    menu.Add("Inventory Manager", "0,1,InventoryManager.aspx")
                    menu.Add("Search Order", "0,2,SearchOrder.aspx")
                    menu.Add("Name Tags", "0,3,UserLabel.aspx")
            End Select

            For Each item As KeyValuePair(Of String, String) In menu
                dataRow = table.NewRow()
                dataRow("MenuName") = item.Key
                Dim temp() As String = Split(item.Value, ",")
                dataRow("TabID") = temp(0)
                dataRow("MenuID") = temp(1)
                dataRow("MenuURL") = temp(2)
                table.Rows.Add(dataRow)
            Next

            Return table
        End Function

        Public Shared Function GetAllClients() As DataTable
            Return DA.Command().Param("Action", "ByAll").FillDataTable("dbo.Client_Select")
        End Function

        Public Shared Function GetExternalClients() As DataTable
            Return DA.Command().Param("Action", "ByExternal").FillDataTable("dbo.Client_Select")
        End Function

        Public Shared Function GetInternalClients() As DataTable
            Return DA.Command().Param("Action", "ByInternal").FillDataTable("dbo.Client_Select")
        End Function

        Public Shared Function GetStaff() As DataTable
            Return DA.Command().Param("Action", "ByStaff").FillDataTable("dbo.Client_Select")
        End Function
    End Class
End Namespace