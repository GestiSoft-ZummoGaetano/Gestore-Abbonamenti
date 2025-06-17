using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View.ShowDialog;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;


namespace Gestore_Abbonamenti.ViewModel;
public class RiepilogoViewModel : BaseObservableObject
{
    bool semestre;
    #region Property

    ObservableCollection<Genitori>? _listGenitori;
    public ObservableCollection<Genitori>? ListGenitori
    {
        get => _listGenitori;
        set
        {
            _listGenitori = value;
            OnPropertyChanged(nameof(ListGenitori));
        }
    }
    ObservableCollection<ViewRiepilogo> _allGenitori = new();
    public ObservableCollection<ViewRiepilogo> AllGenitori
    {
        get => _allGenitori;
        set
        {
            _allGenitori = value;
            OnPropertyChanged(nameof(AllGenitori));
        }
    }
    ObservableCollection<int>? _years;
    public ObservableCollection<int>? Years
    {
        get => _years;
        set
        {
            _years = value;
            OnPropertyChanged(nameof(Years));
        }
    }
    string? _title;
    public string? Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    int? _filterForYears;
    public int? FilterForYears
    {
        get => _filterForYears;
        set
        {
            _filterForYears = value;
            OnPropertyChanged(nameof(FilterForYears));
        }
    }
    int? _selectedIndexYear;
    public int? SelectedIndexYear
    {
        get => _selectedIndexYear;
        set
        {
            _selectedIndexYear = value;
            OnPropertyChanged(nameof(SelectedIndexYear));
        }
    }

    bool _boolCedoleZero = false;
    public bool BoolCedoleZero
    {
        get => _boolCedoleZero;
        set
        {
            _boolCedoleZero = value;
            OnPropertyChanged(nameof(BoolCedoleZero));
        }
    }

    #endregion
    public RiepilogoViewModel(bool semestreArrived, string tagValue)
    {
        semestre = semestreArrived;
        SetContext(tagValue);
    }
    public void SetContext(string tagValue)
    {
        Title = tagValue;
        Years = new ObservableCollection<int>(LogicFactory.Instance.Riepilogo.GetAllYears() ?? new());
        if (Years.Count == 0)
            return;

        SelectedIndexYear = Years[Years.Count - 1];
        FilterForYears = Years[Years.Count - 1];

        Selectionchanged();
    }

    void Selectionchanged()
    {
        if (BoolCedoleZero)
            ListGenitori = new ObservableCollection<Genitori>(LogicFactory.Instance.Riepilogo.GetSemestreZero(semestre, FilterForYears));
        else
            ListGenitori = new ObservableCollection<Genitori>(LogicFactory.Instance.Riepilogo.GetSemestre(semestre, FilterForYears));

        AllGenitori = new();

        if (ListGenitori == null || ListGenitori.Count == 0)
            return;

        foreach (var genitore in ListGenitori)
        {
            foreach (var figlio in genitore.Figli)
            {
                if (figlio.CedoleMensili.Count == 0)
                    continue;

                var importo = figlio.CedoleMensili.Sum(s => s.Importo);
                var importoRidotto = figlio.CedoleMensili.Sum(s => s.ImportoRidotto);

                string tipoPagamento;

                if (importo == importoRidotto)
                    tipoPagamento = "NESSUNA RIDUZIONE APPLICATA";

                else
                {
                    var res = (1 - (importoRidotto / importo)) * 100;
                    tipoPagamento = $"{res:N2}%";
                }

                Brush coloreAlternato = (AllGenitori.Count % 2 == 0) ? Brushes.LightGray : Brushes.White;
                ViewRiepilogo showRow = new ViewRiepilogo
                {
                    NomeGenitore = genitore.Nome,
                    CognomeGenitore = genitore.Cognome,
                    NascitaGenitore = genitore.LuogoNascita,
                    DataNascitaGenitore = genitore.DataNascita,
                    CodFiscaleGenitore = genitore.CodiceFiscale,
                    PaeseGenitore = genitore.Localita,
                    IndirizzoGenitore = genitore.Indirizzo,
                    NCivicoGenitore = genitore.NCivico,
                    IbanGenitore = genitore.Iban,
                    NomeFiglio = figlio.Nome,
                    CognomeFiglio = figlio.Cognome,
                    NascitaFiglio = figlio.LuogoNascita,
                    Istituto = figlio.Scuola.DisplayMember,
                    ImportoDaLiquidare = figlio.CedoleMensili.Sum(i => i.ImportoRidotto),
                    TipologiaPagamento = tipoPagamento,
                    Color = coloreAlternato
                };
                AllGenitori.Add(showRow);
            }
        }
    }
    void GenerateFile()
    {
        string baseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Gestione Abbonamenti");
        string yearFolderPath = Path.Combine(baseFolderPath, DateTime.Now.Year.ToString());
        string filePath = Path.Combine(yearFolderPath, $"{Title}_ANNO_{FilterForYears}.pdf");

        if (!Directory.Exists(yearFolderPath))
        {
            Directory.CreateDirectory(yearFolderPath);
        }
        using (PdfDocument document = new PdfDocument())
        {
            try
            {
                document.Info.Title = "Riepilogo Abbonamenti";

                int pageWidth = 842;
                int pageHeight = 595;
                int rectWidth = 700;
                int rectHeight = 160;
                int padding = 10;
                int startX = (pageWidth - rectWidth) / 2;
                int startY = 30;
                int maxPerPage = 3;
                int count = 0;
                bool isWhite = true;

                PdfPage page = document.AddPage();
                page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                XFont fontRegular = new XFont("Arial", 10);
                XFont fontBold = new XFont("Arial", 10, XFontStyle.Bold);

                double titleWidth = gfx.MeasureString($"{Title} ANNO {FilterForYears}", titleFont).Width;
                gfx.DrawString($"{Title} ANNO {FilterForYears}", titleFont, XBrushes.Black, new XPoint((pageWidth - titleWidth) / 2, startY));

                startY += 20;

                foreach (var item in AllGenitori)
                {
                    if (count == maxPerPage)
                    {
                        page = document.AddPage();
                        page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                        gfx = XGraphics.FromPdfPage(page);
                        startY = 50;
                        count = 0;
                    }

                    XBrush bgColor = isWhite ? XBrushes.White : XBrushes.WhiteSmoke;
                    isWhite = !isWhite;

                    XPen pen = new XPen(XColors.Black, 1);
                    gfx.DrawRoundedRectangle(pen, bgColor, startX, startY, rectWidth, rectHeight, 20, 20);

                    int col1X = startX + padding;
                    int col2X = startX + (rectWidth / 2) + padding;

                    gfx.DrawString("NOME:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 20));
                    gfx.DrawString(item.NomeGenitore, fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 20));

                    gfx.DrawString("COGNOME:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 40));
                    gfx.DrawString(item.CognomeGenitore, fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 40));

                    gfx.DrawString("NATO A:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 60));
                    gfx.DrawString(item.NascitaGenitore, fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 60));

                    gfx.DrawString("DATA NASCITA:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 80));
                    gfx.DrawString(item.DataNascitaGenitore?.ToString("dd/MM/yyyy") ?? "N/D", fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 80));

                    gfx.DrawString("C.FISCALE:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 100));
                    gfx.DrawString(item.CodFiscaleGenitore, fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 100));

                    gfx.DrawString("IBAN:", fontBold, XBrushes.Black, new XPoint(col1X, startY + 120));
                    gfx.DrawString(item.IbanGenitore, fontRegular, XBrushes.Black, new XPoint(col1X + 100, startY + 120));

                    gfx.DrawString("NOME:", fontBold, XBrushes.Black, new XPoint(col2X, startY + 20));
                    gfx.DrawString(item.NomeFiglio, fontRegular, XBrushes.Black, new XPoint(col2X + 100, startY + 20));

                    gfx.DrawString("COGNOME:", fontBold, XBrushes.Black, new XPoint(col2X, startY + 40));
                    gfx.DrawString(item.CognomeFiglio, fontRegular, XBrushes.Black, new XPoint(col2X + 100, startY + 40));

                    gfx.DrawString("NATO A:", fontBold, XBrushes.Black, new XPoint(col2X, startY + 60));
                    gfx.DrawString(item.NascitaFiglio, fontRegular, XBrushes.Black, new XPoint(col2X + 100, startY + 60));

                    gfx.DrawString("ISTITUTO:", fontBold, XBrushes.Black, new XPoint(col2X, startY + 80));

                    int istitutoX = col2X + 100;
                    int istitutoY = startY + 80;
                    int maxWidth = rectWidth / 2 - 120;

                    string istituto = item.Istituto;
                    List<string> lines = WrapText(istituto, fontRegular, gfx, maxWidth);
                    foreach (string line in lines)
                    {
                        gfx.DrawString(line, fontRegular, XBrushes.Black, new XPoint(istitutoX, istitutoY));
                        istitutoY += 15;
                    }

                    gfx.DrawString("IMPORTO LIQUIDARE:", fontBold, XBrushes.Black, new XPoint(col2X, istitutoY + 10));
                    gfx.DrawString(item.ImportoDaLiquidare?.ToString("C") ?? "0", fontRegular, XBrushes.Black, new XPoint(col2X + 140, istitutoY + 10));

                    gfx.DrawString("RIDUZIONE APPLICATA:", fontBold, XBrushes.Black, new XPoint(col2X, istitutoY + 30));
                    gfx.DrawString(item.TipologiaPagamento, fontRegular, XBrushes.Black, new XPoint(col2X + 140, istitutoY + 30));

                    startY += rectHeight + 10;
                    count++;
                }

                document.Save(filePath);
                ShowDialogView.ShowDialogPage(ShowDialogResult.INFO, $"PDF salvato con successo in: {filePath}", ShowDialogImage.INFO);
            }
            catch (IOException)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, $"IMPOSSIBILE SALVARE.\nIL FILE RISULTA APERTO", ShowDialogImage.ERROR);
            }
        }
    }
    List<string> WrapText(string text, XFont font, XGraphics gfx, int maxWidth)
    {
        List<string> lines = new List<string>();
        string[] words = text.Split(' ');
        string currentLine = "";

        foreach (string word in words)
        {
            string testLine = (currentLine.Length > 0) ? currentLine + " " + word : word;
            double width = gfx.MeasureString(testLine, font).Width;

            if (width < maxWidth)
            {
                currentLine = testLine;
            }
            else
            {
                lines.Add(currentLine);
                currentLine = word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return lines;
    }


    #region ICommand
    public ICommand SelectionChangedCommand => new DelegateCommand(Selectionchanged);
    public ICommand PrintCommand => new DelegateCommand(GenerateFile);
    public ICommand ShowCedoleZero => new DelegateCommand(Selectionchanged);
    #endregion
}
