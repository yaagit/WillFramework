namespace WillFramework.Events
{
    public class ObservableWhatever<T> : BaseObservable<T>
    {
        public override T Value
        {
            set
            {
                _previousValue = _currentValue;
                _currentValue = value;
                //赋值后立即调用
                OnValueChanged.Invoke(_previousValue, _currentValue);
            }
            get
            {
                return _currentValue;
            }
        }
        
    }
}
