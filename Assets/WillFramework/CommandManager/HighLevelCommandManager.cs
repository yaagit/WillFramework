using WillFramework.Attributes;
using WillFramework.Rules;

namespace WillFramework.CommandManager
{
    [Identity]
    public class HighLevelCommandManager : BaseCommandManager, ICanInvokeCommand
    {
        private HighLevelCommandManager() {}
    }
}