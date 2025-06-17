using System.ComponentModel.DataAnnotations;
using System.Windows.Media;


namespace GestoreAbbonamenti.Model;
public class Figlii
{
    [Key]
    public long Id { get; set; }

    // Foreign Key
    public long GenitoreId { get; set; }
    public long IdScuola { get; set; }
    public string? Nome { get; set; }
    public string? Cognome { get; set; }
    public DateTime? DataNascita { get; set; }
    public string? Sesso { get; set; }
    public string? LuogoNascita { get; set; }
    public string? Indirizzo { get; set; }
    public bool? Frequenza { get; set; } = true;
    public ICollection<CedoleMensili> CedoleMensili { get; set; } = new List<CedoleMensili>();

    // Navigational Property
    public Scuole Scuola { get; set; } = null!;
    public Genitori? Genitore { get; set; }

    public Figlii() { }
    public Figlii(Figlii? figlo)
    {
        Id = figlo?.Id ?? 0;
        GenitoreId = figlo?.GenitoreId ?? 0;
        Cognome = figlo?.Cognome;
        Nome = figlo?.Nome;
        DataNascita = figlo?.DataNascita;
        Sesso = figlo?.Sesso;
        LuogoNascita = figlo?.LuogoNascita;
        Indirizzo = figlo?.Indirizzo;
        Frequenza = figlo?.Frequenza;
        CedoleMensili = figlo?.CedoleMensili ?? new List<CedoleMensili>();
        Scuola = figlo?.Scuola ?? new();
    }

    public override string ToString()
    {
        return Nome;
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Nome) &&
               !string.IsNullOrWhiteSpace(Cognome) &&
               DataNascita.HasValue &&
               !string.IsNullOrWhiteSpace(Sesso) &&
               !string.IsNullOrWhiteSpace(LuogoNascita) &&
               !string.IsNullOrWhiteSpace(Indirizzo);
    }

    public Brush FillPrenotationState
    {
        get
        {
            return Frequenza switch
            {
                true => Brushes.Green,
                false => Brushes.Red,
                null => Brushes.Transparent
            };
        }
    }
}
