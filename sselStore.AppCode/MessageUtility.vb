Imports System.Configuration

Public Class MessageUtility
    Public Shared Function GetStockZeroMessage(stockDate As DateTime) As String
        Return String.Format(ConfigurationManager.AppSettings("StockZeroMessage"), stockDate.ToShortDateString())
    End Function

    Public Shared Function GetOrderReceiptMessage() As String
        Return ConfigurationManager.AppSettings("OrderReceiptMessage")
    End Function
End Class
