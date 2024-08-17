using System.Reflection;
using WillFramework.Containers;
using WillFramework.Tiers;

namespace WillFramework.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(Assembly localAssembly, params IView[] views);
    }
}