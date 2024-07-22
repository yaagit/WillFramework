using WillFramework.Events;

namespace WillFramework.Observable
{
    public abstract class BaseObservable<T>
    {
        protected readonly IEvent<T, T> OnValueChanged;
        
        protected T _currentValue;
        protected T _previousValue;
        
        public BaseObservable()
        {
            OnValueChanged = new EventImpl<T, T>();
        }
        
        public abstract T Value { get; set; }
        
        
        public void AddListener(EventHandler<T, T> call, T oldValue = default(T), T newValue = default(T))
        {
            OnValueChanged.AddListener(call, false, oldValue, newValue);
        }

        public void RemoveListener(EventHandler<T, T> call)
        {
            OnValueChanged.RemoveListener(call);
        }
    }
}