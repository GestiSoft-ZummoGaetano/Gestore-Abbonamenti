using System.ComponentModel.DataAnnotations;
namespace GestoreAbbonamenti.Model
{
    public class Genitori
    {
        [Key]
        public long Id { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public DateTime? DataNascita { get; set;}
        public string? Sesso { get; set; }
        public string? LuogoNascita { get; set; }
        public string? CodiceFiscale { get; set; }
        public string? Indirizzo { get; set; }
        public string? NCivico { get; set; }
        public string? Cap { get; set; }
        public string? Localita { get; set; }
        public string? Iban { get; set; }

        public ICollection<Figlii> Figli { get; set; } = new List<Figlii>();

        public Genitori() { }

        public Genitori(Genitori? genitori)
        {
            Id = genitori?.Id ?? 0;
            Cognome = genitori?.Cognome;
            Nome = genitori?.Nome;
            DataNascita = genitori?.DataNascita;
            Sesso = genitori?.Sesso;
            LuogoNascita = genitori?.LuogoNascita;
            CodiceFiscale = genitori?.CodiceFiscale;
            Indirizzo = genitori?.Indirizzo;
            NCivico = genitori?.NCivico;
            Cap = genitori?.Cap;
            Localita =  genitori?.Localita;
            Iban =  genitori?.Iban;
            Figli = genitori?.Figli ?? new List<Figlii>(); ;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                   !string.IsNullOrWhiteSpace(Cognome) &&
                   DataNascita.HasValue &&
                   !string.IsNullOrWhiteSpace(Sesso) &&
                   !string.IsNullOrWhiteSpace(LuogoNascita) &&
                   !string.IsNullOrWhiteSpace(CodiceFiscale) &&
                   !string.IsNullOrWhiteSpace(Indirizzo) &&
                   !string.IsNullOrWhiteSpace(NCivico) &&
                   !string.IsNullOrWhiteSpace(Cap) &&
                   !string.IsNullOrWhiteSpace(Localita) &&
                   !string.IsNullOrWhiteSpace(Iban);
        }
    }
}
