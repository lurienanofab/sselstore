Imports LNF.Data
Imports LNF.Models.Data
Imports LNF.Repository
Imports LNF.Web

Namespace BLL
    Public Class StoreSettings

        Private Shared data As DataTable

        Public Shared Property StoreNews() As String
            Get
                Return GetSingleSettingValue("STORE_NEWS")
            End Get
            Set(ByVal value As String)
                Dim current As String = StoreNews
                If Not value.Equals(current) Then
                    SaveSingleSetting("STORE_NEWS", value)
                End If
            End Set
        End Property

        Public Shared Function CanEditStoreNews() As Boolean
            Return CurrentUser.GetCurrentUser().HasPriv(ClientPrivilege.Administrator Or ClientPrivilege.Developer Or ClientPrivilege.StoreManager)
        End Function

        Private Shared Function GetSingleSettingValue(ByVal name As String) As String
            If data Is Nothing Then InitData()
            Dim rows As DataRow() = data.Select("SettingName = '" + name + "'")
            If rows.Length > 0 Then
                Return rows(0)("SettingValue").ToString()
            Else
                Return String.Empty
            End If
        End Function

        Private Shared Function GetSettingValues(ByVal name As String) As List(Of String)
            If data Is Nothing Then InitData()
            Dim rows As DataRow() = data.Select("SettingName = '" + name + "'")
            Dim result As New List(Of String)
            For Each dr As DataRow In rows
                result.Add(dr("SettingValue").ToString())
            Next
            Return result
        End Function

        Private Shared Sub SaveSingleSetting(ByVal name As String, ByVal value As String)
            If data Is Nothing Then InitData()
            Dim rows As DataRow() = data.Select("SettingName = '" + name + "'")
            If rows.Length = 0 Then
                AddNewSetting(name, value)
            Else
                UpdateValueByID(Convert.ToInt32(rows(0)("SettingID")), value)
            End If
            InitData()
        End Sub

        Private Shared Sub InitData()
            data = SelectAll()
        End Sub

        Private Shared Function SelectAll() As DataTable
            Return DA.Command().Param("Action", "SelectAll").FillDataTable("sselMAS.dbo.StoreSettings_Select")
        End Function

        Private Shared Function AddNewSetting(ByVal name As String, ByVal value As String) As Integer
            Return DA.Command() _
                .Param("Action", "AddNewSetting") _
                .Param("SettingName", name) _
                .Param("SettingValue", value) _
                .ExecuteNonQuery("sselMAS.dbo.StoreSettings_Insert").Value
        End Function

        Private Shared Function UpdateValueByID(ByVal id As Integer, ByVal value As String) As Integer
            Return DA.Command() _
                .Param("Action", "UpdateValueByID") _
                .Param("SettingID", id) _
                .Param("SettingValue", value) _
                .ExecuteNonQuery("sselMAS.dbo.StoreSettings_Update").Value
        End Function
    End Class
End Namespace