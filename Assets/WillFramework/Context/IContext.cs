using WillFramework.Containers;
using WillFramework.Tiers;

namespace WillFramework.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(BaseApplication application, params IView[] views);
    }
}