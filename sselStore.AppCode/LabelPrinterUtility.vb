Imports System.IO
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml

Public Class UserLabelData
    Public fName As String = "" '"Krishnaaaaa"
    Public lName As String = "" '"Paga"
    Public profOrgName As String = "" '"Prof.Reddy"
    Public startDate As String = "" '"Sep 2010"
    Public _externalInternal As String = LabelPrinterUtility.USER_INTERNAL  'temp variable
    Public isStaff As Boolean = False
End Class

Public Class LabelPrinterUtility
    Public Shared ReadOnly USER_EXTERNAL As String = "External"
    Public Shared ReadOnly USER_INTERNAL As String = "Internal"
    Public Shared ReadOnly USER_MIXED As String = "Mixed"

    Shared fontfirstname As Font = New Font(Font.FontFamily.HELVETICA, 24.0F, Font.BOLD)
    Shared fontlastname As Font = New Font(Font.FontFamily.HELVETICA, 22.0F)

    Shared fontmanager_normal As Font = New Font(Font.FontFamily.HELVETICA, 20.0F)
    Shared fontmanager_small As Font = New Font(Font.FontFamily.HELVETICA, 14.0F)

    Shared fontstartdate As Font = New Font(Font.FontFamily.HELVETICA, 22.0F, Font.BOLD)
    Shared cellsize As Integer = 34 '34
    Shared emptycellsize As Integer = 28

    Private Shared _currentUserIndex As Integer = -1
    Private Shared _totalUsers As Integer = -1
    Private Shared _arrayAllUser As ArrayList = Nothing

    Private Shared Function GetNextUserData() As UserLabelData
        _currentUserIndex = _currentUserIndex + 1
        If _currentUserIndex >= _totalUsers Then
            Return New UserLabelData()
        End If
        Dim uld As UserLabelData = CType(_arrayAllUser(_currentUserIndex), UserLabelData)
        Return uld
    End Function

    Public Shared Sub CreatePDF(ByVal arrayAllUser As ArrayList)

        _arrayAllUser = arrayAllUser
        _totalUsers = arrayAllUser.Count

        Dim pdfDocument As Document = New Document(PageSize.A4, 0, 0, 0, 0)
        'iTextSharp.text.
        Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDocument, New FileStream("C:\name-tag-last-6months-small-16.PDF", FileMode.Create))
        pdfDocument.Open()
        Dim cb As PdfContentByte = writer.DirectContent

        'Dim pl As PdfLine = Nothing
        'pdfDocument.Add(pl)

        'addPreciseText(cb)
        'addGrid(cb)
        AddPdfTable(pdfDocument, arrayAllUser)

        'addBasicTest(pdfDocument)
        'AddHTML(writer, pdfDocument)
        pdfDocument.Close()

    End Sub

    Public Shared Sub AddPdfTable(ByVal pdfDocument As Document, ByVal arrayAllUser As ArrayList)
        Dim table As New PdfPTable(3)
        'pdfDocument.SetMargins(0, 0, 0, 0)


        'table.DefaultCell.Border = PdfPCell.NO_BORDER
        'table.DefaultCell.BackgroundColor = BaseColor.GREEN
        'table.DefaultCell.Padding = 10

        'Dim cell As New PdfPCell(New Phrase(""))
        'cell.Colspan = 3
        'cell.HorizontalAlignment = 1
        'cell.FixedHeight = cellsize
        ''0=Left, 1=Centre, 2=Right
        'table.AddCell(cell)



        'Dim uld As UserLabelData = New UserLabelData()
        'For Each eachUser In arrayAllUser
        While True
            If _currentUserIndex >= _totalUsers Then
                Exit While
            End If
            AddSinglePageTable(pdfDocument, table)
        End While

        'addSinglePageTable(pdfDocument, table, uld)
        'pdfDocument.Add(table)
    End Sub

    Public Shared Sub AddSinglePageTable(ByVal pdfDocument As Document, ByVal dummyRef As PdfPTable)

        pdfDocument.NewPage()

        Dim table As New PdfPTable(4)

        AddSingleRowOfUsers(table)
        AddSingleRowOfUsers(table)
        AddSingleRowOfUsers(table)

        table.WidthPercentage = 100
        table.HorizontalAlignment = Element.ALIGN_LEFT

        pdfDocument.Add(table)
    End Sub

    Public Shared Sub AddSingleRowOfUsers(ByVal table As PdfPTable)
        AddSpaceCells(table, 8, PdfPCell.RIGHT_BORDER)

        Dim uld1 As UserLabelData = getNextUserData()
        Dim uld2 As UserLabelData = getNextUserData()
        Dim uld3 As UserLabelData = getNextUserData()
        Dim uld4 As UserLabelData = getNextUserData()

        table.AddCell(CreateFirstName(uld1.fName))
        table.AddCell(CreateFirstName(uld2.fName))
        table.AddCell(CreateFirstName(uld3.fName))
        table.AddCell(CreateFirstName(uld4.fName))

        table.AddCell(CreateFirstName(uld1.lName))
        table.AddCell(CreateFirstName(uld2.lName))
        table.AddCell(CreateFirstName(uld3.lName))
        table.AddCell(CreateFirstName(uld4.lName))

        AddSpaceCells(table, 8, PdfPCell.RIGHT_BORDER)

        table.AddCell(CreateManagerName(uld1.profOrgName))
        table.AddCell(CreateManagerName(uld2.profOrgName))
        table.AddCell(CreateManagerName(uld3.profOrgName))
        table.AddCell(CreateManagerName(uld4.profOrgName))

        table.AddCell(CreateManagerName(uld1.startDate))
        table.AddCell(CreateManagerName(uld2.startDate))
        table.AddCell(CreateManagerName(uld3.startDate))
        table.AddCell(CreateManagerName(uld4.startDate))

        AddSpaceCells(table, 4, PdfPCell.BOTTOM_BORDER)
    End Sub

    Public Shared Sub AddSpaceCells(ByVal table As PdfPTable, ByVal numOfCells As Integer, ByVal border As Integer)
        For a As Integer = 1 To numOfCells
            Dim cellspace As New PdfPCell(New Phrase(""))

            cellspace.Border = border 'PdfPCell.RIGHT_BORDER

            cellspace.FixedHeight = emptycellsize
            table.AddCell(cellspace)
        Next
    End Sub

    Public Shared Function CreateCell(ByVal text As String, ByVal fontobj As Font) As PdfPCell
        Dim celltext As New PdfPCell(New Phrase(text, fontobj))
        celltext.Border = PdfPCell.RIGHT_BORDER
        celltext.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        celltext.FixedHeight = cellsize
        Return celltext
    End Function

    Public Shared Function CreateFirstName(ByVal text As String) As PdfPCell
        Return createCell(text, fontfirstname)
    End Function

    Public Shared Function CreateLastName(ByVal text As String) As PdfPCell
        Return createCell(text, fontlastname)
    End Function

    Public Shared Function CreateManagerName(ByVal text As String) As PdfPCell
        Dim len As Integer = text.Length
        If len > 14 Then
            Return createCell(text, fontmanager_small)
        End If
        Return createCell(text, fontmanager_normal)

    End Function

    Public Shared Function CreateStartDate(ByVal text As String) As PdfPCell
        Return createCell(text, fontstartdate)
    End Function

    Public Shared Function CreateTextCell(ByVal text As String) As PdfPCell
        Dim tfont As Font = New Font(Font.FontFamily.TIMES_ROMAN, 16.0F, Font.BOLD)
        Dim celltext As New PdfPCell(New Phrase(text, tfont))
        celltext.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        'Dim celltext As New PdfPCell(New Phrase(text))
        celltext.FixedHeight = cellsize

        Return celltext
    End Function

    Public Shared Sub AddGrid(ByVal cb As PdfContentByte)
        cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, False), 10)
        cb.BeginText()
        Dim str As String = ""
        Dim inddot As String = "|"
        For index As Integer = 1 To 45
            str = str + index.ToString() + ","
        Next

        Dim y As Integer = 0
        For indy As Integer = 1 To 100
            'Dim s2 As String = str + "[" + index.ToString() + "]"
            Dim xx As Integer = 0
            For indx As Integer = 1 To 4
                cb.ShowTextAligned(Element.ALIGN_LEFT, inddot, xx, y, 0.0F)
                xx = xx + 190
            Next
            y = indy * 10
        Next

        Dim lineHorizontal As String = "_______________________________________________________________________________________________________________"
        Dim yy As Integer = 0
        For iy As Integer = 1 To 5
            cb.ShowTextAligned(Element.ALIGN_LEFT, lineHorizontal, 0, yy, 0.0F)
            yy = yy + 285
        Next


        cb.EndText()
        cb.Stroke()
    End Sub

    Public Shared Sub AddPreciseText(ByVal cb As PdfContentByte)
        cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, False), 24)
        cb.BeginText()

        cb.ShowTextAligned(Element.ALIGN_CENTER, "Show text using PdfContentByte", 100.0F, 150.0F, 0.0F)

        cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, False), 12)

        For index As Integer = 1 To 10
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Show text using PdfContentByte", 400.0F, 200 + (index * 20), 0.0F)
        Next

        cb.ShowTextAligned(Element.ALIGN_CENTER, "0123456789abcdefghijklmnopqrstuvwxyz", 0, 0, 0.0F)

        cb.ShowTextAligned(Element.ALIGN_CENTER, "This is 0,0 Text CENTER", 400, 0, 0.0F)
        cb.ShowTextAligned(Element.ALIGN_LEFT, "This is 0,0 Text   LEFT", 400, 30, 0.0F)
        cb.ShowTextAligned(Element.ALIGN_RIGHT, "This is 0,0 Text RIGHT", 400, 60, 0.0F)

        cb.ShowTextAligned(Element.ALIGN_CENTER, "This is 0,0 Text RIGHT", 400, 100, 0.0F)
        cb.ShowTextAligned(Element.ALIGN_CENTER, "BIG TEXT TO This is 0,0 Text RIGHT", 400, 120, 0.0F)
        cb.ShowTextAligned(Element.ALIGN_CENTER, "-+-", 400, 140, 0.0F)

        'For index2 As Integer = 1 To 10
        '   cb.ShowTextAligned(Element.ALIGN_CENTER, "This is 0,0 Text", 400, index2, 0.0F)
        'Next
        cb.EndText()
        cb.Stroke()
    End Sub

    Public Shared Sub AddBasicTest(ByVal pdfDocument As Document)
        pdfDocument.Add(New Paragraph("Here is a test of creating a PDF"))
        Dim p As Paragraph = New Paragraph()
        Dim c As Chunk = New Chunk("Some text in Verdana n", FontFactory.GetFont("Verdana", 12))
        p.Add(c)
    End Sub

    Public Shared Sub AddHTML(writer As PdfWriter, doc As Document)

        Dim contents As String = File.ReadAllText("C:\test.html")

        ' Replace the placeholders with the user-specified text
        contents = contents.Replace("[ORDERID]", "AAAAAAAAbcorderid")
        contents = contents.Replace("[TOTALPRICE]", "totttttttttttttal")

        'HTMLWorker is depricated, I think this is the correct way to do it now
        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, New StringReader(contents))

        '' Step 4: Parse the HTML string into a collection of elements...
        'Dim parsedHtmlElements = HTMLWorker.ParseToList(New StringReader(contents), Nothing)

        '' Enumerate the elements, adding each one to the Document...
        'For Each htmlElement In parsedHtmlElements
        '    doc.Add(htmlElement)
        'Next
    End Sub
End Class