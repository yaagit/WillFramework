using Framework.Rules;
using UnityEngine;

namespace Framework.CommandManager.Extensions
{
    /// <summary>
    /// </summary>
    public static class CanListenCommandExtension
    {
        public static void AddCommandListener<T>(this ICanListenCommand self, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            self.GetContext().CommandContainer.AddCommandListener(del);
        }
    }
}