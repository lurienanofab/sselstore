Imports LNF.Models.Data
Imports LNF.Web.Content


Public MustInherit Class StorePage
    Inherits LNFPage

    Public Overrides ReadOnly Property AuthTypes As ClientPrivilege
        Get
            Return Nothing
        End Get
    End Property
End Class
