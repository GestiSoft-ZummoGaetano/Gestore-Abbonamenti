using GestoreAbbonamenti.Model;

namespace GestoreAbbonamenti.Logic.Interfaces;
public interface IGenitoriLogic
{
    List<Genitori>? GetGenitori();
    Genitori? GetGenitoreById(long id, int anno);
    MessageResult AddOrUpdate(Genitori? genitore);
    decimal GetPercentageCalculation(Scuole scuola);
    string CalcolaCodiceFiscale(string cognome, string nome, DateTime dataNascita, string sesso, string comune);
}
