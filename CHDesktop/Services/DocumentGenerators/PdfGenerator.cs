
using CHDesktop.Models.Domain;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace NADesktop.Services.DocumentGenerators;

class PdfGenerator
{
    private readonly PdfDocument PDFMainPoint;
    private readonly Document RootDocument;
    PdfFont Font = PdfFontFactory.CreateFont("Tahoma.ttf", "CP1251");

    public PdfGenerator(string pathDocument)
    {
        PdfWriter writer = new(pathDocument + ".pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
        PDFMainPoint = new(writer);
        RootDocument = new Document(PDFMainPoint, PageSize.A4);
    }

    public void CreatePDF(string[] chartsName, Report mainInfo, List<CommonWord>? commonWords)
    {
        PDFMainPoint.AddNewPage(PageSize.A4);
        AddTextBlock("Аналитика с помощью сервиса\nNetwork Analysis", -200, 700, 16, true, TextAlignment.CENTER);
        AddTextBlock($"Кол-во выбранных сообщений: {mainInfo?.CountMessages}", 15, 600, 12, false, TextAlignment.LEFT);
        AddTextBlock($"Дата: {mainInfo?.CreateDate}", -480, 600, 12, false, TextAlignment.RIGHT);
        AddTextBlock("График тональной вероятности", -200, 450, 14, true, TextAlignment.CENTER);
        AddChartPNG(chartsName[0], 20, 100);

        RootDocument.Add(new AreaBreak());
        AddTextBlock("Диаграмма местоположений", -200, 800, 14, true, TextAlignment.CENTER);
        AddChartPNG(chartsName[1], 100, 500);
        AddTextBlock("Диаграмма категорий", -200, 450, 14, true, TextAlignment.CENTER);
        AddChartPNG(chartsName[2], 20, 100);

        RootDocument.Add(new AreaBreak());
        AddTextBlock("Диаграмма частей речи по количеству", -200, 800, 14, true, TextAlignment.CENTER);
        AddChartPNG(chartsName[3], 20, 450);
        AddTextBlock("Топ-5 используемых слов", -200, 400, 14, true, TextAlignment.CENTER);

        Table table = new(2, false);
        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        table.SetVerticalAlignment(VerticalAlignment.MIDDLE);
        table.SetFont(Font);

        for (int row = 0; row < commonWords?.Count; row++)
        {
            Cell cellLeft = new Cell().Add(new Paragraph(commonWords[row].Word ?? ""));
            table.AddCell(cellLeft);
            Cell cellRight = new Cell().Add(new Paragraph(commonWords[row].NumberRepetitions.ToString() ?? ""));
            table.AddCell(cellRight);
        }

        table.SetFixedPosition(200, 260, 200);
        RootDocument.Add(table);

        PDFMainPoint.Close();
    }

    private void CreateFirstPage(Report mainInfo, string themChart)
    {
        
    }

    private void CreateSecondPage(string tonalityChart, string PartsSpeech)
    {
       
    }

    private void CreateThirdPage(List<CommonWord>? commonWords)
    {
        
    }

    private void AddTextBlock(string text, float xPosition, float yPosition, float fontSize, bool isBold = false, TextAlignment textAlignment = TextAlignment.JUSTIFIED)
    {
        Paragraph paragraph = new Paragraph(text)
            .SetFont(Font)
            .SetFontSize(fontSize).SetKeepTogether(true);
        if (isBold)
            paragraph.SetBold();
        paragraph.SetFixedPosition(xPosition, yPosition, 1000);
        paragraph.SetTextAlignment(textAlignment);
        RootDocument.Add(paragraph);
    }

    private void AddChartPNG(string nameChart, int x, int y)
    {
        PdfCanvas canvasImage = new(PDFMainPoint.GetLastPage());
        string tempImagePath = $"{nameChart}";
        var image = ImageDataFactory.Create(tempImagePath);
        canvasImage.AddImageAt(image, x, y, false);
    }

}
