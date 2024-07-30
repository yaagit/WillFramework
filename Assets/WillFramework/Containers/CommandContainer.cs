using System;
using System.Collections.Generic;
using System.Text;
using WillFramework.Command;
using WillFramework.Initialize;

namespace WillFramework.Containers
{
    public class CommandContainer : IDisposable
    {
        public delegate void InvokeCommandDelegate<T>(T t) where T : ICommand;
        //相当于一个方法的声明,无实用意义
        private delegate void InvokeCommandDelegate(ICommand e);
        private readonly Dictionary<Type, InvokeCommandDelegate> _commandDelegates = new();
        
        private readonly Dictionary<object, Dictionary<Delegate, InvokeCommandDelegate>> _userCommandDelegatesLookup = new();
        //自动注销器
        private readonly Dictionary<object, Dictionary<Type, List<Delegate>>> _autoCheckoutListenerContainer = new();
        
        public readonly Action<object> OnAutoCheckoutListenerAction;

        public CommandContainer()
        {
            OnAutoCheckoutListenerAction += OnAutoCheckout;
        }
        
        private void OnAutoCheckout(object user)
        {
            if (_autoCheckoutListenerContainer.TryGetValue(user, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
            {
                foreach (KeyValuePair<Type, List<Delegate>> kv in commandTypeWithDelegates)
                {
                    Type commandType = kv.Key;
                    List<Delegate> userDelegates = kv.Value;
                    foreach (var del in userDelegates)
                    {
                        RemoveCommandListenerImpl(user, commandType, del);
                    }
                }
            }
            _autoCheckoutListenerContainer.Remove(user);
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("-------------------------- Command Container --------------------------\n");
            result.Append("-------------").Append("Command Delegates:").Append("\n");
            foreach (KeyValuePair<Type, InvokeCommandDelegate> outerKv in _commandDelegates)
            {
                result.Append($"{outerKv.Key.Name}:").Append("    ").Append(outerKv.Value.Method.Name).Append($"({outerKv.Value.GetInvocationList().Length})");
                result.Append("\n");
            }
            result.Append("-------------").Append("Command Delegates Lookup:").Append("\n");
            foreach (KeyValuePair<object, Dictionary<Delegate, InvokeCommandDelegate>> outerKv in _userCommandDelegatesLookup)
            {
                result.Append($"One of {outerKv.Key.GetType().Name}'s Instances:").Append("\n");
                foreach (KeyValuePair<Delegate, InvokeCommandDelegate> innerKv in outerKv.Value)
                {
                    result.Append("\t").Append($"{innerKv.Key.Method.Name}").Append(":  ").Append($"{innerKv.Value.Method.Name}").Append("\n");
                }
                result.Append("\n");
            }
            result.Append("-------------").Append("AutoCheckout Listener Container:").Append("\n");
            foreach (KeyValuePair<object, Dictionary<Type, List<Delegate>>> outerKv in _autoCheckoutListenerContainer)
            {
                result.Append($"One of {outerKv.Key.GetType().Name}'s Instances:").Append("\n\t");
                foreach (KeyValuePair<Type, List<Delegate>> innerKv  in outerKv.Value)
                {
                    foreach (var del in innerKv.Value)
                    {
                        result.Append($"{innerKv.Key.Name}").Append(":").Append("  ").Append($"{del.Method.Name}")
                            .Append($"({del.GetInvocationList().Length})").Append("\n\t");
                    }
                }
                result.Append("\n");
            }
            return result.ToString();
        }

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
        
        private bool AddCommandListenerImpl<T>(object user, InvokeCommandDelegate<T> del) where T : ICommand
        {
            InvokeCommandDelegate internalDelegate;
            if (_userCommandDelegatesLookup.TryGetValue(user, out Dictionary<Delegate, InvokeCommandDelegate> delegatesLookup))
            {
                if (delegatesLookup.ContainsKey(del))
                {
                    return false;
                }
                internalDelegate = (e) => del((T)e);
                delegatesLookup[del] = internalDelegate;
            }
            else
            {
                internalDelegate = (e) => del((T)e);
                Dictionary<Delegate, InvokeCommandDelegate> newDelegatesLookup = new();
                newDelegatesLookup[del] = internalDelegate;
                _userCommandDelegatesLookup.Add(user, newDelegatesLookup);
            }
            if (_commandDelegates.TryGetValue(typeof(T), out InvokeCommandDelegate tempDel))
            {
                _commandDelegates[typeof(T)] = tempDel + internalDelegate;
            }
            else
            {
                _commandDelegates[typeof(T)] = internalDelegate;
            }
            return true;
        }
        /// <summary>
        /// 添加 Listener + 添加进可以监听事件来注销的注销器
        /// </summary>
        public void AddCommandListener<T>(object user, InvokeCommandDelegate<T> del) where T : ICommand
        {
            bool added = AddCommandListenerImpl(user, del);
            
            if (added)
            {
                AddToAutoCheckoutContainerImpl(user, typeof(T), del);
            }
        }
        
        //添加进注销器
        private void AddToAutoCheckoutContainerImpl<T>(object user, Type commandType, InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (_autoCheckoutListenerContainer.TryGetValue(user, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
            {
                if (commandTypeWithDelegates.TryGetValue(commandType, out List<Delegate> userDelegates))
                {
                    userDelegates.Add(del);
                }
                else
                {
                    List<Delegate> newUserDelegates = new List<Delegate>() {del};
                    commandTypeWithDelegates.Add(commandType, newUserDelegates);
                }
            }
            else
            {
                List<Delegate> newUserDelegates = new List<Delegate>() {del};
                Dictionary<Type, List<Delegate>> newCommandTypeWithDelegates = new() { { commandType, newUserDelegates } };
                _autoCheckoutListenerContainer.Add(user, newCommandTypeWithDelegates);
            }
        }

        private void RemoveCommandListenerImpl(object user, Type commandType, Delegate del)
        {
            if (_userCommandDelegatesLookup.TryGetValue(user, out Dictionary<Delegate, InvokeCommandDelegate> delegatesLookup))
            {
                if (delegatesLookup.TryGetValue(del, out InvokeCommandDelegate internalDelegate))
                {
                    if (_commandDelegates.TryGetValue(commandType, out InvokeCommandDelegate tempDel))
                    {
                        tempDel -= internalDelegate;
                        if (tempDel == null)
                        {
                            _commandDelegates.Remove(commandType);
                        }
                        else
                        {
                            _commandDelegates[commandType] = tempDel;
                        }
                    }
                    delegatesLookup.Remove(del);
                    if (delegatesLookup.Count == 0)
                    {
                        _userCommandDelegatesLookup.Remove(user);
                    }
                }
            }
        }
        
        public void Dispose()
        {
            _commandDelegates.Clear();
            _userCommandDelegatesLookup.Clear();
            _autoCheckoutListenerContainer.Clear();
        }
    }
}