using GestoreAbbonamenti.Model;
using System.ComponentModel.DataAnnotations;

public class CedoleMensili
{
    [Key]
    public long Id { get; set; }

    // Foreign Key
    public long FiglioId { get; set; }

    public string? Mese { get; set; }
    public string? NCedola { get; set; }
    public DateTime? DataCedola { get; set; }
    public decimal? Importo { get; set; } = 0;
    public decimal? ImportoRidotto { get; set; } = 0;
    public decimal? PercentualeRiduzione { get; set; } = 0;
    public int Anno { get; set; } = DateTime.Now.Year;

    public Figlii? Figlio { get; set; } = null;

    public CedoleMensili() { }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Mese) &&
               !string.IsNullOrWhiteSpace(NCedola) &&
               DataCedola.HasValue;             
    }
}
