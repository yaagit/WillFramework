namespace Framework.Events
{
    public delegate void RmcEventHandler<T, U>(T oldValue, U newValue);
    
    public interface IEvent<T, U>
    {
        void AddListener(RmcEventHandler<T, U> call, bool willInvokeImmediately = false, T oldValue = default(T), U newValue = default(U));
        void RemoveListener(RmcEventHandler<T, U> call);
        void Invoke(T oldValue, U newValue);
    }
}