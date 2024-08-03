using System.Threading.Tasks;
using UnityEngine;
using WillFramework.Attributes;
using WillFramework.Attributes.Types;
using WillFramework.Initialize;
using WillFramework.Rules;

namespace WillFramework.Tiers
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        private IContext _context;
        
        void OnDestroy()
        {
            _context.IocContainer.Remove(IdentityType.View, this);
            _context.CommandContainer.OnAutoCheckoutListenerAction.Invoke(this);
            _OnDestroy();
        }
        //留给子类去实现
        protected virtual void _OnDestroy()
        {
            
        }

        IContext ICanGetContext.GetContext()
        {
            return _context;
        }

        void ICanSetContext.SetContext(IContext context)
        {
            _context = context;
        }

        protected T Instantiate<T>(T original, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent);
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            return instance;
        }
        protected T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent, instantiateInWorldSpace);
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            return instance;
        }
        protected T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation);
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            return instance;
        }
        protected T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation, parent);
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            return instance;
        }
    }
}