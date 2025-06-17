namespace GestoreAbbonamenti.Logic.Interfaces
{
    public interface ILogicFactory
    {
        #region Properties

        bool IsInitialized { get; }

        #endregion

        #region Public methods

        void Initialize();

        #endregion
    }
}
