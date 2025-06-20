using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Data.Database;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Http;

namespace GestoreAbbonamenti.Logic.Logic;
public class GenitoriLogic : IGenitoriLogic
{
    public List<Genitori>? GetGenitori()
    {
        using var context = new DbEntities();
        return context.Genitori
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(g => g.Cognome)
            .ToList();
    }
    public Genitori? GetGenitoreById(long id, int anno)
    {
        using var context = new DbEntities();
        return context.Genitori
            .AsNoTracking()
            .Include(f => f.Figli)
            .ThenInclude(s => s.Scuola)
            .Include(o => o.Figli)
            .ThenInclude(f => f.CedoleMensili.Where(z => z.Anno == anno))
            .AsSplitQuery()
            .FirstOrDefault(o => o.Id == id);
    }
    public MessageResult AddOrUpdate(Genitori? genitore)
    {
        if (genitore == null)
            return new();

        using var context = new DbEntities();

        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.Execute(() =>
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                if (genitore.Id == 0)
                {
                    context.Genitori.Add(genitore);
                }
                else
                {
                    var existingGenitore = context.Genitori
                        .Include(g => g.Figli)
                            .ThenInclude(f => f.CedoleMensili)
                        .FirstOrDefault(g => g.Id == genitore.Id);

                    if (existingGenitore != null)
                    {
                        context.Entry(existingGenitore).CurrentValues.SetValues(genitore);

                        foreach (var figlio in genitore.Figli ?? new List<Figlii>())
                        {
                            var existingFiglio = existingGenitore.Figli.FirstOrDefault(f => f.Id == figlio.Id);

                            if (existingFiglio == null)
                            {
                                existingGenitore.Figli.Add(figlio);
                            }
                            else
                            {
                                context.Entry(existingFiglio).CurrentValues.SetValues(figlio);
                            }

                            foreach (var cedola in figlio.CedoleMensili ?? new List<CedoleMensili>())
                            {
                                if (cedola.Id == 0)
                                {
                                    existingFiglio?.CedoleMensili.Add(new CedoleMensili
                                    {
                                        FiglioId = figlio.Id,
                                        Mese = cedola.Mese,
                                        NCedola = cedola.NCedola,
                                        DataCedola = cedola.DataCedola,
                                        Importo = cedola.Importo,
                                        ImportoRidotto = cedola.ImportoRidotto,
                                        PercentualeRiduzione = cedola.PercentualeRiduzione
                                    });
                                }
                                else
                                {
                                    var existingCedola = existingFiglio.CedoleMensili.FirstOrDefault(c => c.Id == cedola.Id);
                                    if (existingCedola == null)
                                    {
                                        existingFiglio.CedoleMensili.Add(cedola);
                                    }
                                    else
                                    {
                                        context.Entry(existingCedola).CurrentValues.SetValues(cedola);
                                    }
                                }
                            }
                        }

                        // Rimuovi i figli non più presenti
                        foreach (var existingFiglio in existingGenitore.Figli.ToList())
                        {
                            if (genitore.Figli.All(f => f.Id != existingFiglio.Id))
                            {
                                context.Figli.Remove(existingFiglio);
                            }
                        }
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return new MessageResult
                {
                    Result = ResultType.Success,
                    Message = "CARICAMENTO AVVENUTO CON SUCCESSO",
                    Data = genitore
                };
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
                return new MessageResult { Result = ResultType.Ko, Message = "QUESTO CODICE FISCALE E' GIA' PRESENTE." };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new MessageResult { Result = ResultType.Ko, Message = ex.Message };
            }
        });
    }
    public decimal GetPercentageCalculation(Scuole scuola)
    {
        using var context = new DbEntities();

        var istituti = context.Scuole
            .AsNoTracking()
            .Where(c => c.CodiceIstituto == scuola.CodiceIstituto)
            .AsSplitQuery()
            .ToList();

        if (istituti.Count <= 1)
            return 0;

        var scuolaMinimaDistanza = istituti
       .Where(c => c.DistanzaDalComune.HasValue)
       .OrderBy(c => c.DistanzaDalComune)
       .FirstOrDefault();

        decimal prezzoAttuale = scuola.PrezzoCedolaSettimanale ?? 0m;
        decimal prezzoMinimoNonNullable = scuolaMinimaDistanza?.PrezzoCedolaSettimanale ?? 1m;
        decimal percentualeDifferenza = (1 - (prezzoMinimoNonNullable / prezzoAttuale)) * 100;

        return Math.Round(percentualeDifferenza, 2);


    }

    public string CalcolaCodiceFiscale(string cognome, string nome, DateTime dataNascita, string sesso, string comune)
    {
        using var context = new DbEntities();

        var belfioreCode = context.ComuniItaliani
            .AsNoTracking()
            .FirstOrDefault(c => c.Comune == comune);

        if (belfioreCode?.CodiceBelfiore == null)
            return string.Empty;

        var codfisc = CodiceFiscaleGenerator.CalcolaCodiceFiscale(cognome, nome, dataNascita, sesso, belfioreCode.CodiceBelfiore);
        return codfisc;
    }
}
