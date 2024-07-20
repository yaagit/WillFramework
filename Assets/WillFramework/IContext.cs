using WillFramework.CommandManager;

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