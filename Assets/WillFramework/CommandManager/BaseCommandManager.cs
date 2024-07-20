using WillFramework.Rules;

namespace WillFramework.CommandManager
{
    /// <summary>
    /// </summary>
    public class BaseCommandManager : ICommandManager, ICanSetContext, ICanGetContext
    {

        private IContext _context;
        
        void ICanSetContext.SetContext(IContext context)
        {
            _context = context;
        }

        IContext ICanGetContext.GetContext()
        {
            return _context;
        }
        
    }
}