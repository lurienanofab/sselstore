Imports System.IO
Imports LNF.Repository
Imports sselStore.AppCode.BLL

''' <summary>
''' Represent an item object.  Some additional properties are added for the convenience of client code such as ItemImageName
''' This class is not an exact representation of Item table's schema for each record
''' </summary>
''' <remarks></remarks>
Public Class Item
    Private _Loaded As Boolean = False

    Public Property ItemID As Integer
    Public Property Description As String
    Public Property Notes As String
    Public Property CategoryID As Integer
    Public Property CategoryName As String
    Public Property ManufacturerPN As String
    Public Property ItemImageName As String
    Public Property Active As Boolean
    Public Property StoreDisplay As Boolean
    Public Property CrossCharge As Boolean
    Public Property StockQuantity As Integer
    Public Property MinStockQuantity As Integer
    Public Property StockOnStoreOrder As Integer
    Public Property StockAvailable As Integer 'used only for display in itemdetail.aspx for cart shopping
    Public Property Price As Double 'temporary column
    Public Property SearchKeyWords As String

    Public ReadOnly Property Loaded As Boolean
        Get
            Return _Loaded
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal itemId As Integer)
        Me.ItemID = itemId
        ItemImageName = GetImageFileName()
    End Sub

    Public Sub Load(reader As ExecuteReaderResult)
        If reader.Read() Then
            ItemID = reader.Value("ItemID", 0)
            Description = reader.Value("Description", String.Empty)
            Notes = reader.Value("Notes", String.Empty)
            CategoryID = reader.Value("CatID", 0)
            CategoryName = reader.Value("CatName", String.Empty)
            ManufacturerPN = reader.Value("ManufacturerPN", String.Empty)
            ItemImageName = GetImageFileName()
            Active = reader.Value("Active", False)
            StoreDisplay = reader.Value("StoreDisplay", False)
            CrossCharge = reader.Value("CrossCharge", False)
            StockQuantity = reader.Value("StockQuantity", 0)
            MinStockQuantity = reader.Value("MinStockQuantity", 0)
            SearchKeyWords = reader.Value("SearchKeyWords", String.Empty)
            StockAvailable = reader.Value("StockAvailable", 0)
            _Loaded = True
        End If
    End Sub

    Private Function GetImageFileName() As String
        Return GenerateImageName(_ItemID)
    End Function

    Private Shared Function GenerateImageName(ByVal itemID As Integer, Optional ByVal isDisplay As Boolean = True) As String
        Dim input As String = itemID.ToString()
        Dim len As Integer = input.Length + 1
        Dim i As Integer

        For i = len To AppConstants.NUM_OF_DIGIT_ITEMNO Step +1
            input = "0" + input
        Next i

        input = "img" + input + ".jpg"

        'check if the image file exists - if not, we return the default image name
        'If this function is called for non-display purpose, then we know the client wants the newly generated
        'image name for storing image.  in this situation, we have to return the real image name no matter what
        If File.Exists(GlobalUtility.GetItemImagePath(True) & input) OrElse Not isDisplay Then
            Return input
        Else
            Return "default.jpg"
        End If
    End Function

    Public Shared Function GetItemImageName(ByVal itemID As Integer, Optional ByVal isDisplay As Boolean = True) As String
        Return GenerateImageName(itemID, isDisplay)
    End Function

    Public Shared Function Create(itemId As Integer) As Item
        Dim result As New Item()

        Using reader As ExecuteReaderResult = ItemManager.GetItemByItemID(itemId)
            result.Load(reader)
        End Using

        Return result
    End Function
End Class