namespace WillFramework.Events
{
    public class EventImpl<T, U> : IEvent<T, U>
    {
        private event EventHandler<T, U> _typedEventHandler;

        public void AddListener(EventHandler<T, U> call, bool willInvokeImmediately = false, T oldValue = default(T), U newValue = default(U))
        {
            _typedEventHandler += call;
            if (willInvokeImmediately)
            {
                Invoke(oldValue, newValue);
            }
        }

        public void RemoveListener(EventHandler<T, U> call)
        {
            _typedEventHandler -= call;
        }

        public void Invoke(T oldValue, U newValue)
        {
            _typedEventHandler?.Invoke(oldValue, newValue);
        }
        
    }
}