namespace GestoreAbbonamenti.Logic.Interfaces
{
    public interface IStartUpLogic
    {
        void Initialize();
        void OnStarting();
        string? GetUser();
    }
}
