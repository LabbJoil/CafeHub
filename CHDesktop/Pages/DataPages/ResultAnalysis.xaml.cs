
using CHDesktop.Models.Enums;
using CHDesktop.Models.ReportProcessing;
using CHDesktop.Services.DocumentGenerators;
using CHDesktop.Services.Helpers;
using CHDesktop.Services.Server;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using Microsoft.Win32;
using NADesktop.Services.DocumentGenerators;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static iText.Svg.SvgConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CHDesktop.Pages.Data;

public partial class ResultAnalysis : Page
{
    private readonly int ReportId;
    private ReportSummary? Report;

    public ResultAnalysis(int reportId)
    {
        ReportId = reportId;
        InitializeComponent();
    }

    private async void ResultAnalysis_Loaded(object sender, RoutedEventArgs e)
    {
        WindowHelper.StartLoadingGIF(true);
        var analyticReport = await MainHttp.ReportRequest.GetReportById(ReportId);
        WindowHelper.StartLoadingGIF(false);
        if (!WindowHelper.CheckStatusCode(analyticReport.Item1) || analyticReport.Item2 == null)
            return;
        Report = analyticReport.Item2;
        AddParams();
    }

    public void AddParams()
    {
        QuantityTB.Text = Report!.MainInfo.CountMessages.ToString();
        DateTB.Text = Report!.MainInfo.CreateDate.ToString();

        TonalityChart.Series =
        [
            new PieSeries
            {
                Title = "Positive",
                Values = new ChartValues<float> { Report!.AverageTonality?.Positive ?? 0f },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Negative",
                Values = new ChartValues<float> { Report!.AverageTonality?.Negative ?? 0f },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Median",
                Values = new ChartValues<float> { Report!.AverageTonality?.Median ?? 0f },
                DataLabels = true
            }
        ];

        Report!.AggregateСategoryComplaint.TryGetValue(СategoryComplaint.Food, out int foodValue);
        Report!.AggregateСategoryComplaint.TryGetValue(СategoryComplaint.Food, out int SituationValue);
        Report!.AggregateСategoryComplaint.TryGetValue(СategoryComplaint.Food, out int StaffValue);
        CategoryChart.Series =
        [
            new PieSeries
            {
                Title = "Еда",
                Values = new ChartValues<int> { foodValue },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Обстановка",
                Values = new ChartValues<int> { SituationValue },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Персонал",
                Values = new ChartValues<int> { StaffValue },
                DataLabels = true
            }
        ];

        Report!.AggregateLocationCafe.TryGetValue(LocationCafe.KuznechnyLane3, out int KuznechnyLane3Value);
        Report!.AggregateLocationCafe.TryGetValue(LocationCafe.NovoroshchinskayaStreet4, out int NovoroshchinskayaStreet4Value);
        Report!.AggregateLocationCafe.TryGetValue(LocationCafe.FedorAbramovStreet16k1, out int FedorAbramovStreet16k1Value);
        Report!.AggregateLocationCafe.TryGetValue(LocationCafe.KonstantinovskyProspekt23, out int KonstantinovskyProspekt23Value);
        Report!.AggregateLocationCafe.TryGetValue(LocationCafe.KirpichnyLane8, out int KirpichnyLane8Value);
        LocationChart.Series =
        [
            new PieSeries
            {
                Title = "Кузнечный переулок, 3",
                Values = new ChartValues<int> { KuznechnyLane3Value },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Новорощинская улица, 4",
                Values = new ChartValues<int> { NovoroshchinskayaStreet4Value },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Улица Федора Абрамова, 16к1",
                Values = new ChartValues<int> { FedorAbramovStreet16k1Value },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Константиновский проспект, 23",
                Values = new ChartValues<int> { KonstantinovskyProspekt23Value },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Кирпичный переулок, 8",
                Values = new ChartValues<int> { KirpichnyLane8Value },
                DataLabels = true
            }
        ];

        PartsSpeechChart.Series =
        [
            new ColumnSeries
            {
                Values = new ChartValues<int>
                {
                    Report!.AggregatePartsSpeech?.NOUN ?? 0,
                    Report!.AggregatePartsSpeech?.DET ?? 0,
                    Report!.AggregatePartsSpeech?.ADJ ?? 0,
                    Report!.AggregatePartsSpeech?.PART ?? 0,
                    Report!.AggregatePartsSpeech?.PRON ?? 0,
                    Report!.AggregatePartsSpeech?.ADP ?? 0,
                    Report!.AggregatePartsSpeech?.VERB ?? 0,
                    Report!.AggregatePartsSpeech?.NUM ?? 0,
                    Report!.AggregatePartsSpeech?.ADV ?? 0,
                    Report!.AggregatePartsSpeech?.INTJ ?? 0,
                    Report!.AggregatePartsSpeech?.SYM ?? 0,
                    Report!.AggregatePartsSpeech?.X ?? 0,
                }
            }
        ];

        List<(TextBlock, TextBlock)> tableWord = [
            (FirstWordTB, FirstValueTB),
            (SecondWordTB, SecondValueTB),
            (ThirdWordTB, ThirdValueTB),
            (FourthWordTB, FourthValueTB),
            (FifthWordTB, FifthValueTB),
        ];
        for (int indexWord = 0; indexWord < Report?.AggregateCommonWords.Count; indexWord++)
        {
            tableWord[indexWord].Item1.Text = Report?.AggregateCommonWords[indexWord].Word;
            tableWord[indexWord].Item2.Text = Report?.AggregateCommonWords[indexWord].NumberRepetitions.ToString();
        }
    }

    private void CreateFile_Click(object sender, RoutedEventArgs e)
    {
        string directory = DirectoryBTC.BannerTextBox.Text;
        string fileName = FileNameBTC.BannerTextBox.Text;
        string filePath = $"{directory}\\{fileName}";
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
        {
            MessageBox.Show("Такой директории не существует");
            return;
        }
        if (string.IsNullOrEmpty(fileName))
        {
            MessageBox.Show("Введите название файла");
            return;
        }
        var chartsPNGPaths = new string[]
            {
                CreatePNGChart(TonalityChart, 120, 215, 10, 10),
                CreatePNGChart(LocationChart, 120, 590, 10, 0),
                CreatePNGChart(CategoryChart, 120, 980, 10, 0),
                CreatePNGChart(PartsSpeechChart, 60, 1350, 30, 10)
            };
        
        if ((bool)CheckTB.IsChecked!)
        {
            DocxGenerator docs = new(filePath);
            docs.CreateWordDocument(chartsPNGPaths, Report!.MainInfo, Report!.AggregateCommonWords);
        }
        else
        {
            PdfGenerator pdf = new(filePath);
            pdf.CreatePDF(chartsPNGPaths, Report!.MainInfo, Report!.AggregateCommonWords);
        }
        DeletePNGCharts(chartsPNGPaths);
    }

    private static string CreatePNGChart(Chart chart, int x, int y, int addWidth, int addHeight)
    {
        string fileName = $"{chart.Name}.png";
        RenderTargetBitmap renderTarget = new(
          (int)chart.ActualWidth + addWidth + x,
          (int)chart.ActualHeight + addHeight + y,
          96, 96, PixelFormats.Pbgra32);

        renderTarget.Render(chart);

        int width = (int)chart.RenderSize.Width + addWidth;
        int height = (int)chart.RenderSize.Height + addHeight;

        CroppedBitmap croppedBitmap = new(
            renderTarget,
            new Int32Rect(x, y, width, height)
        );

        PngBitmapEncoder pngEncoder = new();
        pngEncoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

        using Stream stream = File.Create(fileName);
        pngEncoder.Save(stream);
        stream.Close();
        return $"{Directory.GetCurrentDirectory()}\\{fileName}";
    }

    private static void DeletePNGCharts(string[] chartsPNGPaths)
    {
        foreach (var chartPath in chartsPNGPaths)
            File.Delete(chartPath);
    }

    private void OpenDirectory_Click(object sender, RoutedEventArgs e)
    {
        OpenFolderDialog saveFileDialog = new();
        if (saveFileDialog.ShowDialog() == true)
            DirectoryBTC.BannerTextBox.Text = saveFileDialog.FolderName;
    }
    private void BackPage_Click(object sender, RoutedEventArgs e)
    {
        WindowHelper.RunToBackPage();
    }
}
