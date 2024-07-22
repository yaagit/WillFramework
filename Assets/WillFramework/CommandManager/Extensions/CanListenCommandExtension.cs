using UnityEngine;
using WillFramework.Command;
using WillFramework.Containers;
using WillFramework.Rules;

namespace WillFramework.CommandManager.Extensions
{
    /// <summary>
    /// </summary>
    public static class CanListenCommandExtension
    {
        //具有自动事件注销器功能
        public static void AddCommandListener<T>(this ICanListenCommand self, object user, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (self == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.AddCommandListener(user.GetType(), del);
        }
        
        
        public static void AddCommandListener<T>(this ICanListenCommand self, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (self == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.AddCommandListener(del);
            
        }
        
        public static void RemoveCommandListener<T>(this ICanListenCommand self, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (self == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.RemoveCommandListener(del);
        }

        public static void ErrorWarning()
        {
            Debug.LogError("检测到 CommandManager 的引用为空,可能是您在 Monobehavior 的 Start 方法内引用了 CommandManager 对象, 解决方式: 请在 IAutoInitialize 接口的 AutoInitialize 方法内引用此对象.");
        }
    }
}