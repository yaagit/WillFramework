namespace Framework.Events
{
    public class EventImpl<T, U> : IEvent<T, U>
    {
        private event RmcEventHandler<T, U> _typedEventHandler;

        public void AddListener(RmcEventHandler<T, U> call, bool willInvokeImmediately = false, T oldValue = default(T), U newValue = default(U))
        {
            _typedEventHandler += call;
            if (willInvokeImmediately)
            {
                Invoke(oldValue, newValue);
            }
        }

        public void RemoveListener(RmcEventHandler<T, U> call)
        {
            _typedEventHandler -= call;
        }

        public void Invoke(T oldValue, U newValue)
        {
            _typedEventHandler?.Invoke(oldValue, newValue);
        }
        
    }
}