using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Data.Database;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Model;
using Microsoft.EntityFrameworkCore;

namespace GestoreAbbonamenti.Logic.Logic;
public class ScoolSettingLogic : IScoolSettingLogic
{
    public List<Scuole>? GetScool()
    {
        using var context = new DbEntities();

        return context.Scuole
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(c => c.Citta)
            .ToList();
    }
    public MessageResult? AddOrUpdateScool(Scuole scoolSetting, bool changeAll)
    {
        try
        {
            using var context = new DbEntities();

            if (scoolSetting.Id == 0)
            {
                // Nuovo inserimento
                context.Scuole.Add(scoolSetting);
            }
            else
            {
                // Modifica
                var existingScool = context.Scuole
                    .FirstOrDefault(g => g.Id == scoolSetting.Id);

                if (existingScool == null)
                {
                    context.Scuole.Add(scoolSetting);
                }
                else
                {
                    // Aggiorna la scuola corrente
                    context.Entry(existingScool).CurrentValues.SetValues(scoolSetting);

                    if (changeAll)
                    {
                        var sameCitySchools = context.Scuole
                            .Where(s => s.Citta == scoolSetting.Citta && s.Id != scoolSetting.Id)
                            .ToList();

                        foreach (var school in sameCitySchools)
                        {
                            school.PrezzoCedolaSettimanale = scoolSetting.PrezzoCedolaSettimanale;
                            school.DistanzaDalComune = scoolSetting.DistanzaDalComune;
                        }
                    }
                }
            }

            context.SaveChanges();
            return new MessageResult { Result = ResultType.Success };
        }
        catch (Exception ex)
        {
            return new MessageResult { Result = ResultType.Ko, Message = ex.Message };
        }
    }


    public MessageResult DeleteRow(Scuole? row)
    {
        try
        {
            if (row == null || row?.Id == 0)
                return new();

            using var context = new DbEntities();

            context.Scuole.Attach(row);
            context.Scuole.Remove(row);

            context.SaveChanges();
            return new MessageResult { Result = ResultType.Success, Message = "CARICAMENTO AVVENUTO CON SUCCESSO" };
        }
        catch (Exception ex)
        {
            return new MessageResult { Result = ResultType.Ko, Message = ex.Message };
        }
    }

    public List<Istituti>? GetIstituti()
    {
        using var context = new DbEntities();

        return context.Istituti
            .AsNoTracking()
            .AsSplitQuery()
            .ToList();
    }
}
