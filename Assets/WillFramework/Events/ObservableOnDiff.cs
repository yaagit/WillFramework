using System;

namespace Framework.Events
{
    public class ObservableOnDiff<T> : BaseObservable<T> where T : IEquatable<T>
    {
        public override T Value
        {
            set
            {
                _previousValue = _currentValue;
                _currentValue = value;
                if (!_currentValue.Equals(_previousValue))
                {
                    OnValueChanged.Invoke(_previousValue, _currentValue);
                }
            }
            get
            {
                return _currentValue;
            }
        }
    }
}