using UnityEngine;
using WillFramework.Attributes;
using WillFramework.Attributes.Types;
using WillFramework.Rules;

namespace WillFramework.Tiers
{
    public class BaseView<T> : MonoBehaviour, IView where T : BaseView<T>
    {
        private IContext _context;
        
        void OnDestroy()
        {
            _context.IocContainer.Remove(IdentityType.View, this);
            _context.CommandContainer.OnAutoCheckoutListenerAction.Invoke(typeof(T));
            _OnDestroy();
        }
        //留给子类去实现
        protected virtual void _OnDestroy()
        {
            
        }

        // IContext IView.Context { get; set; }
        IContext ICanGetContext.GetContext()
        {
            return _context;
        }

        void ICanSetContext.SetContext(IContext context)
        {
            _context = context;
        }
    }
}