namespace WillFramework.Events
{
    public delegate void EventHandler<T, U>(T oldValue, U newValue);
    
    public interface IEvent<T, U>
    {
        void AddListener(EventHandler<T, U> call, bool willInvokeImmediately = false, T oldValue = default(T), U newValue = default(U));
        void RemoveListener(EventHandler<T, U> call);
        void Invoke(T oldValue, U newValue);
    }
}