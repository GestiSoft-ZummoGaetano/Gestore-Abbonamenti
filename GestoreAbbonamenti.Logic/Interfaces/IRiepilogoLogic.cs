using GestoreAbbonamenti.Model;

namespace GestoreAbbonamenti.Logic.Interfaces;
public  interface IRiepilogoLogic
{
    List<Genitori> GetSemestre(bool semestre, int? year);
    List<Genitori> GetSemestreZero(bool semestre, int? year);
    List<int>? GetAllYears();
}
