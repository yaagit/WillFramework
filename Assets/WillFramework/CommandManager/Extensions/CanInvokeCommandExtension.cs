using Framework.CommandManager;
using Framework.Rules;
using UnityEngine;

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