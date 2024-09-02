using UnityEngine;
using WillFramework.Attributes.Types;
using WillFramework.Context;
using WillFramework.Rules;
using Object = UnityEngine.Object;

namespace WillFramework.Tiers
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        private IContext _context;
        
        void OnDestroy()
        {
            _context.IocContainer.Remove(IdentityType.View, this);
            _context.CommandContainer.UnbindEvents(this);
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
        protected T Instantiate<T>(T original) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original);
            HandleInstantiated(instance);
            return instance;
        }
        protected T Instantiate<T>(T original, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent);
            HandleInstantiated(instance);
            return instance;
        }
        protected T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent, instantiateInWorldSpace);
            HandleInstantiated(instance);
            return instance;
        }
        protected T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation);
            HandleInstantiated(instance);
            return instance;
        }
        protected T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation, parent);
            HandleInstantiated(instance);
            return instance;
        }

        private void HandleInstantiated<T>(T instance) where T : Object
        {
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            else
            {
                GameObject go = instance as GameObject;
                view = go.GetComponent<IView>();
                if (view != null)
                {
                    _context.PresetGeneratedView(view);
                }
            }
        }
    }
}