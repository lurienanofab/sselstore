Imports System.IO
Imports sselStore.AppCode

Namespace Admin
    Public Class ItemManagerCreateStep2
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            If Not String.IsNullOrEmpty(Request.QueryString("item")) Then
                lblItemID.Text = Request.QueryString("item")
            End If

            If Not Page.IsPostBack Then
                LoadImage()
            End If
        End Sub

        Private Sub LoadImage()
            Dim itemId As Integer
            If Integer.TryParse(Request.QueryString("item"), itemId) Then
                Dim imageName As String = Item.GetItemImageName(itemId, False)
                If File.Exists(GlobalUtility.GetItemImagePath(True) + imageName) Then
                    imgItem.ImageUrl = GlobalUtility.GetItemImagePath(False) + imageName + "?ts=" + DateTime.Now.Ticks.ToString()
                    imgItem.Visible = True
                End If
            End If
        End Sub

        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
            litSaveError.Text = String.Empty

            Dim itemId As Integer
            If Not Integer.TryParse(Request.QueryString("item"), itemId) Then
                'if there is no new item id in session, we simply redirect user back to item list
                Response.Redirect("~/admin/ItemManager.aspx?tab=1&menu=0")
            Else
                Dim msg As String = String.Empty
                If ImageUtility.UploadFile(itemId, FileUpload1, msg) Then
                    LoadImage()
                End If
                litSaveError.Text = msg
            End If
        End Sub

        Protected Sub btnDone_Click(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect("~/admin/ItemManager.aspx?tab=1&menu=0")
        End Sub
    End Class
End Namespace