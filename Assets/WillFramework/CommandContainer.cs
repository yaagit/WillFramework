using System;
using System.Collections.Generic;

namespace Framework.CommandManager
{
    /// <summary>
    /// </summary>
    public class CommandContainer : IDisposable
    {
        //两个委托一个带泛型(公开),一个不带泛型(私用), 这是因为 Dictionary<TKey, TValue> 只能确定两个泛型, 无法确定第三个泛型
        //公开的泛型在 AddCommandListener 那里需要包装一下, 用私用的泛型包着公开的, 大肠包小肠 
        public delegate void InvokeCommandDelegate<T>(T t) where T : ICommand;
        //相当于一个方法的声明,无实用意义
        private delegate void InvokeCommandDelegate(ICommand e);
        private Dictionary<Type, InvokeCommandDelegate> _commandDelegates = new();
        //以方法为维度查重,防止事件被重复调用
        private Dictionary<Delegate, InvokeCommandDelegate> _commandDelegatesLookup = new();
        
        
        /// <summary>
        /// 一个 Command 类型对应一个委托对象, Command 只是个包含参数且继承了 ICommand 的普通实体类
        /// </summary>
        public void InvokeCommand(ICommand command)
        {
            if (_commandDelegates.TryGetValue(command.GetType(), out InvokeCommandDelegate del))
            {
                del.Invoke(command);
            }
        }

        public int GetListenerCount()
        {
            return _commandDelegates.Count;
        }
        
        
        public void AddCommandListener<T>(InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (_commandDelegatesLookup.ContainsKey(del))
            {
                return;
            }
            //相当于方法维度的包装,只是为了处理 Dictionary 的泛型问题
            InvokeCommandDelegate internalDelegate = (e) => del((T)e);
            _commandDelegatesLookup[del] = internalDelegate;
        
            if (_commandDelegates.TryGetValue(typeof(T), out InvokeCommandDelegate tempDel))
            {
                _commandDelegates[typeof(T)] = tempDel + internalDelegate;
            }
            else
            {
                _commandDelegates[typeof(T)] = internalDelegate;
            }
        }
        
        public void RemoveCommandListener<T>(InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (_commandDelegatesLookup.TryGetValue(del, out InvokeCommandDelegate internalDelegate))
            {
                if (_commandDelegates.TryGetValue(typeof(T), out InvokeCommandDelegate tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        _commandDelegates.Remove(typeof(T));
                    }
                    else
                    {
                        _commandDelegates[typeof(T)] = tempDel;
                    }
                }
        
                _commandDelegatesLookup.Remove(del);
            }
        }
        
        public void Dispose()
        {
            _commandDelegates.Clear();
            _commandDelegatesLookup.Clear();
        }
    }
}