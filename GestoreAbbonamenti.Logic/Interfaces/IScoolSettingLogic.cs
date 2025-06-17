using GestoreAbbonamenti.Model;

namespace GestoreAbbonamenti.Logic.Interfaces;

public interface IScoolSettingLogic
{
    MessageResult? AddOrUpdateScool(Scuole scoolSetting, bool changeAll);
    List<Scuole>? GetScool();
    List<Istituti>? GetIstituti();
    MessageResult DeleteRow(Scuole? row);
}
