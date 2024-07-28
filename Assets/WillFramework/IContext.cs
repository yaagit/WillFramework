using System.Collections;
using WillFramework.Containers;
using WillFramework.Tiers;

namespace WillFramework
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(params IView[] views);
    }
}