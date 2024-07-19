using Framework.CommandManager;
using Framework.Rules;

namespace Framework.CommandManager.Extensions
{
    /// <summary>
    /// </summary>
    public static class CanInvokeCommandExtension
    {
        public static void InvokeCommand(this ICanInvokeCommand self, ICommand command)
        {
            self.GetContext().CommandContainer.InvokeCommand(command);
        }
    }
}