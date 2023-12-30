using CHDesktop.Models.Domain;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System.IO;
using System.Windows;

namespace CHDesktop.Services.DocumentGenerators;

internal class DocxGenerator(string pathDocument)
{
    private readonly XWPFDocument Document = new();
    private readonly string PathDocument = pathDocument + ".docx";

    public void CreateWordDocument(string[] chartsName, Report report, List<CommonWord>? commonWords)
    {
        FileStream? sw = null;
        try
        {
            AddTextBlock("Аналитика с помощью сервиса", 16, 2700, 1000, true);
            AddTextBlock("Cafe Hub", 16, 3600, 0, true);
            AddTextBlock($"Кол-во выбранных сообщений: {report?.CountMessages} \t\t\t\t\t Дата: {report?.CreateDate}", 12, 20, 0);

            AddTextBlock("График тональной вероятности", 14, 1500, 2000, true);
            AddChartPNG(chartsName[0], 1400, 700);

            AddTextBlock("Диаграмма местоположений", 14, 3300, 2300, true);
            AddChartPNG(chartsName[1], 1400, 700);
            AddTextBlock("Диаграмма категорий", 14, 3000, 2000, true);
            AddChartPNG(chartsName[2], 1400, 700);
            AddTextBlock("Диаграмма частей речи по количеству", 14, 3000, 2000, true);
            AddChartPNG(chartsName[3], 1400, 700);

            AddTextBlock("Топ-5 используемых слов", 16, 3600, 0, true);
            int numRows = 5;
            int numCols = 2;

            XWPFTable table = Document.CreateTable(numRows, numCols);
            table.Width = 3000;
            //table.SetCellMargins(0, 0, 1000, 0);

            for (int row = 0; row < commonWords?.Count; row++)
            {
                XWPFTableCell cell = table.GetRow(row).GetCell(0);
                XWPFParagraph para = cell.AddParagraph();
                XWPFRun run = para.CreateRun();
                run.FontFamily = "Tahoma";
                run.SetText(commonWords?[row].Word);
                table.GetRow(row).Height = (50);
                para.Alignment = ParagraphAlignment.CENTER;
                cell.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.TOP);

                cell = table.GetRow(row).GetCell(1);
                para = cell.AddParagraph();
                run = para.CreateRun();
                run.FontFamily = "Tahoma";
                run.SetText(commonWords?[row].NumberRepetitions.ToString());
                table.GetRow(row).Height = (50);
                para.Alignment = ParagraphAlignment.CENTER;
                cell.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.TOP);
            }
            sw = File.Create(PathDocument);
            Document.Write(sw);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        sw?.Close();
    }

    private void AddChartPNG(string nameChart, int xPosition, int yPosition)
    {
        XWPFParagraph paragraph = Document.CreateParagraph();
        paragraph.IndentationLeft = xPosition;
        paragraph.SpacingBefore = yPosition;

        XWPFRun runPNG = paragraph.CreateRun();
        using FileStream img = new($"{nameChart}", FileMode.Open, FileAccess.Read);
        runPNG.AddPicture(img, (int)PictureType.PNG, nameChart, 450 * 10857, 252 * 12857);
    }

    private void AddTextBlock(string text, float fontSize, int xPosition, int yPosition, bool isBold = false)
    {
        XWPFParagraph paragraph = Document.CreateParagraph();
        paragraph.IndentationLeft = xPosition;
        paragraph.SpacingBefore = yPosition;
        paragraph.SpacingAfter = 0;

        XWPFRun run = paragraph.CreateRun();
        run.SetText(text);
        run.FontSize = fontSize;
        run.FontFamily = "Tahoma";
        run.IsBold = isBold;
    }

    private void DefaultParams()
    {
        CT_SectPr sectPr = Document.Document.body.sectPr ?? Document.Document.body.AddNewSectPr();
        CT_PageMar pageMar = sectPr.pgMar ?? sectPr.AddPageMar();
        pageMar.left = 500;
        pageMar.top = 500;
        pageMar.right = 500;
        pageMar.bottom = 500;
    }
}
