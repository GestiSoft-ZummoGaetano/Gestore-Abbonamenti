using GestoreAbbonamenti.Logic.Interfaces;

namespace GestoreAbbonamenti.Logic.LogicSetter
{
    public sealed class LogicCreator
    {
        public LogicCreator(IEnumerable<ILogicFactory> env)
        {
            if (env.Any((ILogicFactory x) => !x.IsInitialized))
            {
                throw new InvalidOperationException();
            }
        }
    }
}
