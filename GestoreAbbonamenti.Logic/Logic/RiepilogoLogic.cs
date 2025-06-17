using GestoreAbbonamenti.Data.Database;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Model;
using Microsoft.EntityFrameworkCore;

namespace GestoreAbbonamenti.Logic.Logic;
public class RiepilogoLogic : IRiepilogoLogic
{
    public List<int>? GetAllYears()
    {
        using var context = new DbEntities();

        return context.CedoleMensili
            .AsNoTracking()
            .Select(x => x.Anno)
            .Distinct()
            .ToList();
    }

    public List<Genitori> GetSemestre(bool semestre, int? year)
    {
        using var context = new DbEntities();

        List<string> Semestre = semestre
             ? new List<string> { "Gennaio", "FEBBRAIO", "MARZO", "APRILE", "MAGGIO", "GIUGNO" }
             : new List<string> { "SETTEMBRE", "OTTOBRE", "NOVEMBRE", "DICEMBRE" };

        return context.Genitori
            .AsNoTracking()
            .Include(g => g.Figli)
                .ThenInclude(f => f.CedoleMensili.Where(c => Semestre.Contains(c.Mese) && c.Anno == year))
            .Include(g => g.Figli)
                .ThenInclude(f => f.Scuola)
            .AsSplitQuery()
            .ToList();
    }

    public List<Genitori> GetSemestreZero(bool semestre, int? year)
    {
        using var context = new DbEntities();

        List<string> mesiSemestre = semestre
            ? new List<string> { "Gennaio", "FEBBRAIO", "MARZO", "APRILE", "MAGGIO", "GIUGNO" }
            : new List<string> { "SETTEMBRE", "OTTOBRE", "NOVEMBRE", "DICEMBRE" };

        var genitori = context.Genitori
            .AsNoTracking()
            .Include(g => g.Figli.Where(f => f.Frequenza == true))
                .ThenInclude(f => f.CedoleMensili.Where(a => a.Anno == year))
            .Include(g => g.Figli)
                .ThenInclude(f => f.Scuola)
            .AsSplitQuery()
            .ToList();

        // Filtra solo le cedole del semestre specificato
        foreach (var genitore in genitori)
        {
            foreach (var figlio in genitore.Figli)
            {
                List<CedoleMensili> cedole = new();
                var count = 0;
                foreach (var mese in mesiSemestre)
                {
                    
                    bool cedolaEsiste = figlio.CedoleMensili
                        .Any(c => string.Equals(c.Mese, mese, StringComparison.OrdinalIgnoreCase) && c.Anno == year);

                    if (!cedolaEsiste)
                        count += 1; 
                }

                if (count == mesiSemestre.Count)
                {
                    var cedola = new CedoleMensili //deve aggiungere alla nuova lista
                    {
                        NCedola = "N/A",
                        DataCedola = null,
                        Importo = 0,
                        ImportoRidotto = 0,
                        PercentualeRiduzione = 0,
                        FiglioId = figlio.Id,
                        Mese = "",
                        Anno = year ?? DateTime.Now.Year,
                    };
                    figlio.CedoleMensili = [cedola];
                }
                else figlio.CedoleMensili = [];
            }
        }
        return genitori;
    }
}



