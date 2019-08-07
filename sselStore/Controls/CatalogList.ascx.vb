Imports sselStore.AppCode
Imports sselStore.AppCode.BLL

Namespace Controls
    Public Class CatalogList
        Inherits System.Web.UI.UserControl

        Public Property Search As Boolean

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Search Then
                dlItem.DataSourceID = "odsSearch"
            End If
        End Sub

        Protected Function GetItemPrice(ByVal itemId As Integer) As String
            Return String.Format("{0:C}", -PriceManager.GetCurrentItemPriceByItemID(itemId)).Replace("(", "").Replace(")", "")
        End Function

        Protected Function GetImageStyle(ByVal itemId As Integer) As String
            If itemId = 233 Or itemId = 235 Then
                Return "thumbimagetweezer"
            ElseIf itemId > 156 AndAlso itemId < 163 Then
                Return "thumbimagepen"
            Else
                Return "thumbimage"
            End If
        End Function

        Protected Function GetWarningMessage(ByVal stockQ As Integer) As String
            If stockQ < 1 Then
                Dim stockDate As DateTime = Now
                Dim da As System.DayOfWeek = Now.DayOfWeek
                If da = DayOfWeek.Monday Then
                    stockDate = stockDate.AddDays(2)
                ElseIf da = DayOfWeek.Tuesday Then
                    stockDate = stockDate.AddDays(1)
                ElseIf da = DayOfWeek.Wednesday Then
                    stockDate = stockDate.AddDays(7)
                ElseIf da = DayOfWeek.Thursday Then
                    stockDate = stockDate.AddDays(6)
                ElseIf da = DayOfWeek.Friday Then
                    stockDate = stockDate.AddDays(5)
                ElseIf da = DayOfWeek.Saturday Then
                    stockDate = stockDate.AddDays(4)
                ElseIf da = DayOfWeek.Sunday Then
                    stockDate = stockDate.AddDays(3)
                End If
                Return MessageUtility.GetStockZeroMessage(stockDate)
            End If
            Return String.Empty
        End Function

        Protected Function GetVisible(ByVal stockQ As Integer) As Boolean
            If stockQ < 1 Then
                Return False
            Else
                Return True
            End If
        End Function

        Protected Function GetItemImageName(ByVal itemID As Integer) As String
            Return Item.GetItemImageName(itemID)
        End Function

        Protected Sub dlItem_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlItem.ItemCommand
            Dim quantity As Integer = 1

            Try
                quantity = Integer.Parse(CType(e.Item.FindControl("txtBuyQ"), TextBox).Text)
            Catch ex As Exception
                quantity = 1 'if error happens, we just treat as 1
            End Try

            If e.CommandName = "AddToCart" Then
                Dim str() As String = e.CommandArgument.ToString().Split(Char.Parse("!"))
                AddItemToCart(Integer.Parse(str(0)), Double.Parse(str(1)), str(2), str(3), quantity)
            End If
        End Sub

        Protected Function CombineString(ByVal itemId As Integer, ByVal itemPrice As Double, ByVal desc As String, ByVal mpn As String) As String
            Return String.Join("!", itemId, itemPrice, desc, mpn)
        End Function

        Private Sub AddItemToCart(ByVal itemId As Integer, ByVal price As Double, ByVal desc As String, ByVal mpn As String, ByVal quantity As Integer)
            Dim msg As String = String.Empty
            If CartManager.AddItemToCart("Cart", -1, itemId, price, desc, mpn, quantity, msg) Then
                litSecItemMessage.Text = String.Empty
                'We need to append the return url, so user can always come back the the same page after adding an item to cart
                Dim strArray As String() = Request.RawUrl.Split(Char.Parse("/"))
                Dim returnURL As String = strArray(strArray.Length - 1)
                returnURL = returnURL.Replace("&", "%26")

                Response.Redirect(String.Format("~/Cart.aspx?tab=1&returnurl={0}", returnURL))
            Else
                litSecItemMessage.Text = msg
            End If
        End Sub

        Protected Function GetNotes(ByVal obj As Object) As String
            Try
                Dim html As String = String.Empty

                If obj.Equals(DBNull.Value) Then
                    html = String.Empty
                ElseIf obj.Equals(String.Empty) Then
                    html = String.Empty
                Else
                    html = String.Format("<div class=""item-notes"">{0}</div>", obj)
                End If

                Return html
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

    End Class
End Namespace