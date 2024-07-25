using WillFramework.Attributes;
using WillFramework.Rules;

namespace WillFramework.CommandManager
{
    [Identity]
    public class CommandManager : BaseCommandManager, ICanInvokeCommand, ICanListenCommand
    {
        private CommandManager() {}
    }
}