using System.Threading.Tasks;
using WillFramework.Containers;
using WillFramework.Tiers;

namespace WillFramework
{
    /// <summary>
    /// </summary>
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        Task InitializeViewAsync(IView view);

        void StartWithViews(params IView[] views);
    }
}