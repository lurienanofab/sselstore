Imports System.IO
Imports System.Web.UI.WebControls

Public Class ImageUtility

    Public Shared Function UploadFile(itemId As Integer, fileUpload As FileUpload, ByRef message As String) As Boolean
        If fileUpload.HasFile Then
            Dim ext As String = Path.GetExtension(fileUpload.FileName).Substring(1).ToLower()
            Dim allowed As String() = {"jpg", "gif", "bmp", "ico", "png"}

            Dim err As Integer = 0

            'Dim itemId As Integer
            'If Not Integer.TryParse(Request.QueryString("item"), itemId) Then
            '    'if there is no new item id in session, we simply redirect user back to item list
            '    Response.Redirect("~/admin/ItemManager.aspx?tab=1&menu=0")
            'End If

            If Not allowed.Contains(ext) Then
                message += String.Format("<div class=""stock-error"">Please upload image files only ({0})</div>", String.Join("/", allowed))
                err += 1
            End If

            If fileUpload.PostedFile.ContentLength > 1024 * 1000 Then
                message += "<div class=""stock-error"">Please make sure your file size is less than 1MB</div>"
                err += 1
            End If

            If err > 0 Then
                Return False
            End If

            Dim imageName As String = Item.GetItemImageName(itemId, False)
            fileUpload.SaveAs(GlobalUtility.GetItemImagePath(True) + imageName)
            'LoadImage()

            'imgItem.ImageUrl = GlobalUtility.GetItemImagePath(False) + newImageName
            'imgItem.Visible = True

            message += "<div style=""margin-top: 20px;""><strong>New item image saved successfully</strong></div>"
            Return True
        Else
            message += "<div class=""stock-error"">An error occurred. Please make sure your file path is valid. You can add an image later if necessary.</div>"
            Return False
        End If
    End Function
End Class
