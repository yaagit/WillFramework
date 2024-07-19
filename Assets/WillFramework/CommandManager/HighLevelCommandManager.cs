using Framework.Attributes.Injection;
using Framework.CommandManager;
using Framework.Rules;
using Framework.Tiers;

namespace Framework.CommandManager
{
    [Identity]
    public class HighLevelCommandManager : BaseCommandManager, ICanInvokeCommand
    {
        private HighLevelCommandManager() {}
    }
}