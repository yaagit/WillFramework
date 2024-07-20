using UnityEngine;
using WillFramework.Attributes;

namespace WillFramework.Tiers
{
    public class BaseView<T> : MonoBehaviour, IView where T : BaseView<T>
    {
        void OnDestroy()
        {
            IContext context = (this as IView).Context;
            context.IocContainer.Remove(IdentityType.View, this);
            context.CommandContainer.OnAutoCheckoutListenerAction.Invoke(typeof(T));
            _OnDestroy();
        }
        //留给子类去实现
        protected virtual void _OnDestroy()
        {
            
        }

        IContext IView.Context { get; set; }
    }
}