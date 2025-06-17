using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoreAbbonamenti.Model;
public class Scuole
{
    [Key]
    public long Id { get; set; }
    public string? Citta { get; set; }
    public string? Istituto { get; set; }
    public string? Indirizzo { get; set; }
    public string? CodiceIstituto { get; set; }
    public decimal? DistanzaDalComune { get; set; }
    public decimal? PrezzoCedolaSettimanale { get; set; } = 0;

    [NotMapped]
    public string? DisplayMember => $"{Citta}, {Indirizzo}, {CodiceIstituto}";
    public virtual ICollection<Figlii>? Figli { get; set; } = null;

    public Scuole() { }

    public Scuole(Scuole scuola)
    {
        Id = scuola.Id;
        Citta = scuola.Citta;
        Istituto = scuola.Istituto;
        Indirizzo = scuola.Indirizzo;
        CodiceIstituto = scuola.CodiceIstituto;
        DistanzaDalComune = scuola.DistanzaDalComune;
        PrezzoCedolaSettimanale = scuola.PrezzoCedolaSettimanale;
    }
}
