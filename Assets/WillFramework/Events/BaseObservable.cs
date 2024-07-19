namespace Framework.Events
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
        
        
        public void AddListener(RmcEventHandler<T, T> call, T oldValue = default(T), T newValue = default(T))
        {
            OnValueChanged.AddListener(call, false, oldValue, newValue);
        }

        public void RemoveListener(RmcEventHandler<T, T> call)
        {
            OnValueChanged.RemoveListener(call);
        }
    }
}