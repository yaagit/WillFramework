using System;

namespace WillFramework.Reporter
{
    /// <summary>
    /// </summary>
    public class ReportAction<T>
    {
        private Action<T> _action;

        public void AddListener(Action<T> action)
        {
            _action += action;
        }

        public void RemoveListener(Action<T> action)
        {
            _action -= action;
        }

        public void Trigger(T p)
        {
            _action?.Invoke(p);
        }
    }
    
    public class ReportAction
    {
        private Action _action;

        public void AddListener(Action action)
        {
            _action += action;
        }

        public void RemoveListener(Action action)
        {
            _action -= action;
        }

        public void Trigger()
        {
            _action?.Invoke();
        }
    }
}