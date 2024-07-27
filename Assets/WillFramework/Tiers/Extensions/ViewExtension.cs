using UnityEngine;
using WillFramework.Command;
using WillFramework.Containers;

namespace WillFramework.Tiers.Extensions
{
    public static class ViewExtension
    {
        public static void InvokeCommand(this IView self, ICommand command)
        {
            if (self == null || self.GetContext() == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.InvokeCommand(command);
        }
        
        public static void AddCommandListener<T>(this IView self, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (self == null || self.GetContext() == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.AddCommandListener(self, del);
        }
        
        public static void ErrorWarning()
        {
            Debug.LogError("检测到 Context 的引用为空,可能是你在 Monobehavior 的 Start / Awake 方法内调用了 CommandContainer 的方法.");
        }
    }
}