Imports System.Web
Imports System.Drawing
Imports System.IO

Public Class GlobalUtility
    Public Shared Function GetSelectedTabColor() As Color
        Return Color.Beige
    End Function

    Public Shared Function GetItemImagePath(Optional ByVal isPhysical As Boolean = False) As String
        If isPhysical Then
            Return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "images\items\")
        Else
            Return HttpContext.Current.Request.ApplicationPath + "/images/items/"
        End If
    End Function

    Public Shared Function GetIconImagePath() As String
        Return HttpContext.Current.Request.ApplicationPath + "/images/icons/"
    End Function

    Public Shared Function GetTextBoxMaxLength(ByVal columnName As String) As Integer
        Select Case columnName
            'string date related
            Case DBConstants.ITEM_MANUFACTURERPN
                Return 50
            Case DBConstants.ITEM_DESCRIPTION
                Return 100
                'Date realted
            Case DBConstants.ITEM_ORDERDATE, DBConstants.ITEM_ESTIMATEDARRIVALDATE
                Return 10
                'Stock number related
            Case DBConstants.ITEM_STOCKQUANTITY, DBConstants.ITEM_MINSTOCKQUANTITY, DBConstants.ITEM_STOCKONORDER
                Return 4
            Case DBConstants.ITEM_ITEMID
                Return 5 '99999 as max item id counts
            Case Else
                Return 10
        End Select
    End Function

    Public Shared Function GetTextBoxWidth(ByVal columnName As String) As Integer
        Select Case columnName
            Case DBConstants.ITEM_MANUFACTURERPN
                Return 323
            Case DBConstants.ITEM_DESCRIPTION
                Return 350
                'Date realted
            Case DBConstants.ITEM_ORDERDATE, DBConstants.ITEM_ESTIMATEDARRIVALDATE
                Return 80
                'Stock number related
            Case DBConstants.ITEM_STOCKQUANTITY, DBConstants.ITEM_MINSTOCKQUANTITY, DBConstants.ITEM_STOCKONORDER
                Return 32
            Case DBConstants.ITEM_ITEMID
                Return 40
            Case Else
                Return 80
        End Select
    End Function
End Class