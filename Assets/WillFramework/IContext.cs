using WillFramework.Containers;

namespace WillFramework
{
    /// <summary>
    /// </summary>
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
    }
}