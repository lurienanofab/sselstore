Namespace Admin
    Public Class StoreAdminMaster
        Inherits LNF.Web.Content.LNFMasterPage

        Public Overrides ReadOnly Property ShowMenu As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property AddScripts As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property AddStyles As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

    End Class
End Namespace