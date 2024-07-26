using UnityEngine;
using WillFramework.Command;
using WillFramework.Rules;

namespace WillFramework.CommandManager.Extensions
{
    public static class CanInvokeCommandExtension
    {
        public static void InvokeCommand(this ICanInvokeCommand self, ICommand command)
        {
            if (self == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.InvokeCommand(command);
        }
        public static void ErrorWarning()
        {
            Debug.LogError("检测到 CommandManager 的引用为空,可能是你在 Monobehavior 的 Start 方法内引用了 CommandManager 对象, 解决方式: 请在 IAutoInitialize 接口的 AutoInitialize 方法内引用此对象.");
        }
    }
}