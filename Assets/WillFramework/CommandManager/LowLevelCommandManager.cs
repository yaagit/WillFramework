using WillFramework.Attributes;
using WillFramework.Rules;

namespace WillFramework.CommandManager
{
    [Identity]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}