using GestoreAbbonamenti.Logic.Interfaces;


namespace GestoreAbbonamenti.Logic.LogicSetter
{
    public class LogicFactory : ILogicFactory
    {
        public static LogicFactory Instance { get; private set; }
        public IGenitoriLogic Genitori {  get; set; } 
        public bool IsInitialized { get; set; }
        public IStartUpLogic StartUp { get; set; }
        public IScoolSettingLogic Scool { get; set; }
        public IRiepilogoLogic Riepilogo { get; set; }


        #region Public methods

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException();

            Instance = this;
            IsInitialized = true;
        }

        #endregion

    }
}
