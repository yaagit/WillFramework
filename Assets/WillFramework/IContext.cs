using Framework.CommandManager;
using Framework.Rules;

namespace Framework
{
    /// <summary>
    /// </summary>
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
    }
}