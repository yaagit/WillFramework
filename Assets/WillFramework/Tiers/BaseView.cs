using Framework.Attributes.Injection;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.Tiers
{
    public class BaseView<T> : MonoBehaviour, IView where T : BaseView<T>
    {
        
        
        void OnDestroy()
        {
            Context.IocContainer.Remove(IdentityType.View, this);
            _OnDestroy();
        }
        //留给子类去实现
        protected virtual void _OnDestroy()
        {
            
        }

        public IContext Context { get; set; }
    }
}