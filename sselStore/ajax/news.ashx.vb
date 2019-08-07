Imports System.Web
Imports System.Web.Services
Imports LNF
Imports LNF.Data
Imports LNF.Repository.Data
Imports sselStore.AppCode.BLL

Namespace Ajax
    Public Class News
        Implements System.Web.IHttpHandler, IReadOnlySessionState

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            If context.Request.HttpMethod = "POST" Then
                If StoreSettings.CanEditStoreNews() Then
                    Dim value As String = context.Request.Form("news")
                    Dim decoded As String = context.Server.HtmlDecode(value)
                    StoreSettings.StoreNews = decoded
                Else
                    Throw New Exception("You do not have permission to edit the store news.")
                End If
            End If
            context.Response.ContentType = "text/html"
            context.Response.Write(StoreSettings.StoreNews)
        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace