﻿using System;
using System.Collections.Generic;
using System.Text;
using WillFramework.Command;
using WillFramework.Initialize;

namespace WillFramework.Containers
{
    /// <summary>
    /// </summary>
    public class CommandContainer : IDisposable, IInitialize
    {
        //两个委托一个带泛型(公开),一个不带泛型(私用), 这是因为 Dictionary<TKey, TValue> 只能确定两个泛型, 无法确定第三个泛型
        //公开的泛型在 AddCommandListener 那里需要包装一下, 用私用的泛型包着公开的, 大肠包小肠 
        public delegate void InvokeCommandDelegate<T>(T t) where T : ICommand;
        //相当于一个方法的声明,无实用意义
        private delegate void InvokeCommandDelegate(ICommand e);
        private Dictionary<Type, InvokeCommandDelegate> _commandDelegates = new();
        //以方法为维度查重,防止事件被重复调用
        private Dictionary<Delegate, InvokeCommandDelegate> _commandDelegatesLookup = new();
        //自动注销器
        // private Dictionary<Type, Dictionary<Type, List<Delegate>>> AutoCheckoutListenerContainer = new();
        private Dictionary<object, Dictionary<Type, List<Delegate>>> AutoCheckoutListenerContainer = new();
        
        public Action<object> OnAutoCheckoutListenerAction;

        public CommandContainer()
        {
            Initialize();
        }
        public void Initialize()
        {

            OnAutoCheckoutListenerAction += OnAutoCheckout;
        }
        
        private void OnAutoCheckout(object user)
        {
            if (AutoCheckoutListenerContainer.TryGetValue(user, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
            {
                foreach (KeyValuePair<Type, List<Delegate>> kv in commandTypeWithDelegates)
                {
                    Type commandType = kv.Key;
                    List<Delegate> userDelegates = kv.Value;
                    foreach (var del in userDelegates)
                    {
                        RemoveCommandListenerImpl(commandType, del);
                    }
                }
            }
        
            AutoCheckoutListenerContainer.Remove(user);
        }
        // private void OnAutoCheckout(Type userType)
        // {
        //     if (AutoCheckoutListenerContainer.TryGetValue(userType, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
        //     {
        //         foreach (KeyValuePair<Type, List<Delegate>> kv in commandTypeWithDelegates)
        //         {
        //             Type commandType = kv.Key;
        //             List<Delegate> userDelegates = kv.Value;
        //             foreach (var del in userDelegates)
        //             {
        //                 RemoveCommandListenerImpl(commandType, del);
        //             }
        //         }
        //     }
        //
        //     AutoCheckoutListenerContainer.Remove(userType);
        // }

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

        public bool AddCommandListenerImpl<T>(InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (_commandDelegatesLookup.ContainsKey(del))
            {
                return false;
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
            return true;
        }
        /// <summary>
        /// 添加 Listener
        /// </summary>
        public void AddCommandListener<T>(InvokeCommandDelegate<T> del) where T : ICommand
        {
            AddCommandListenerImpl(del);
        }

        /// <summary>
        /// 添加 Listener + 添加进可以监听事件来注销的注销器
        /// </summary>
        public void AddCommandListener<T>(object user, InvokeCommandDelegate<T> del) where T : ICommand
        {
            bool added = AddCommandListenerImpl(del);
            
            if (added)
            {
                AddToAutoCheckoutContainerImpl(user, typeof(T), del);
            }
        }
        
        //todo 保存进去的类型应该是 Dictionary<object, Dictionary<Type, List<Delegate>>>  ===>  object, CommandType, List<Delegate>
        //添加进注销器
        public void AddToAutoCheckoutContainerImpl<T>(object user, Type commandType, InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (AutoCheckoutListenerContainer.TryGetValue(user, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
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
                AutoCheckoutListenerContainer.Add(user, newCommandTypeWithDelegates);
            }
        }
        //添加进注销器
        // public void AddToAutoCheckoutContainerImpl<T>(Type userType, Type commandType, InvokeCommandDelegate<T> del) where T : ICommand
        // {
        //     if (AutoCheckoutListenerContainer.TryGetValue(userType, out Dictionary<Type, List<Delegate>> commandTypeWithDelegates))
        //     {
        //         if (commandTypeWithDelegates.TryGetValue(commandType, out List<Delegate> userDelegates))
        //         {
        //             userDelegates.Add(del);
        //         }
        //         else
        //         {
        //             List<Delegate> newUserDelegates = new List<Delegate>() {del};
        //             commandTypeWithDelegates.Add(commandType, newUserDelegates);
        //         }
        //     }
        //     else
        //     {
        //         List<Delegate> newUserDelegates = new List<Delegate>() {del};
        //         Dictionary<Type, List<Delegate>> newCommandTypeWithDelegates = new();
        //         newCommandTypeWithDelegates.Add(commandType, newUserDelegates);
        //         AutoCheckoutListenerContainer.Add(userType, newCommandTypeWithDelegates);
        //     }
        // }
        
        public void RemoveCommandListener<T>(InvokeCommandDelegate<T> del) where T : ICommand
        {
            RemoveCommandListenerImpl(typeof(T), del);
        }

        public void RemoveCommandListenerImpl(Type commandType, Delegate del)
        {
            if (_commandDelegatesLookup.TryGetValue(del, out InvokeCommandDelegate internalDelegate))
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