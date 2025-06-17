using System.Windows.Media;

namespace GestoreAbbonamenti.Model;
public class ViewRiepilogo
{
    //ANAGRAFICA GENITORE
    public string? NomeGenitore { get; set; }
    public string? CognomeGenitore { get; set; }
    public string? NascitaGenitore { get; set; }
    public DateTime? DataNascitaGenitore { get; set; }
    public string? CodFiscaleGenitore { get; set; }
    public string? PaeseGenitore { get; set; }
    public string? IndirizzoGenitore { get; set; }
    public string? NCivicoGenitore { get; set; }
    public string? IbanGenitore { get; set; }

    //DATI FIGLIO + CEDOLE
    public string? NomeFiglio { get; set; }
    public string? CognomeFiglio { get; set; }
    public string? NascitaFiglio { get; set; }
    public decimal? ImportoDaLiquidare { get; set; } = 0;
    public string? Istituto { get; set; }
    public string? TipologiaPagamento { get; set; }

    //COLORE SFONDO
    public Brush? Color { get; set; }
}
