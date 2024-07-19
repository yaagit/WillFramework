using Framework.Attributes.Injection;
using Framework.CommandManager;
using Framework.Rules;

namespace Framework.CommandManager
{
    [Identity]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}