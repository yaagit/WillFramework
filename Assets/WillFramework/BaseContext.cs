using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WillFramework.Attributes;
using WillFramework.Attributes.Injection;
using WillFramework.Attributes.Types;
using WillFramework.CommandManager;
using WillFramework.Containers;
using WillFramework.Rules;
using WillFramework.Tiers;
using WillFramework.Initialize;

namespace WillFramework
{
    /// <summary>
    /// </summary>
    public class BaseContext<T> : IContext where T : BaseContext<T>
    {
        private bool _hasStarted = false; //防止重复启动
        //IOC 容器
        private IocContainer _iocContainer = new IocContainer();
        public IocContainer IocContainer { get => _iocContainer;}
        //Command 容器
        private CommandContainer _commandContainer = new CommandContainer();
        public CommandContainer CommandContainer { get => _commandContainer; }

        #region 获取的实例在任何情况下都是单例的
        private static readonly Lazy<T> _lazyCreateInstance = new(() =>
        {
            Type type = typeof(T);
            var constructorInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            var constructorInfo = Array.Find(constructorInfos, c => c.GetParameters().Length == 0);
            if (constructorInfo == null)
            {
                throw new Exception("这个类没有找到无参且私有的构造器: " + typeof(T).FullName);
            }
            return constructorInfo.Invoke(null) as T;
        });

        public static T Instance
        {
            get => _lazyCreateInstance.Value;
        }
        #endregion
        
        #region 扫描类上面的注解, 添加进相应的 container
        private void ScanIdentities()
        {
            Assembly assembly = typeof(T).Assembly;
            Type[] types = assembly.GetTypes();
            foreach (var t in types)
            {
                IdentityAttribute identityAttribute = t.GetCustomAttribute(typeof(IdentityAttribute)) as IdentityAttribute;
                if (identityAttribute == null)
                {
                    continue;
                }
                object instance = CreateInstance(t);
                HandleCanSetContext(instance);
                _iocContainer.Add(identityAttribute.IdentityType, instance);
            }
        }

        private void HandleCanSetContext(object instance)
        {
            ICanSetContext canSetContext = instance as ICanSetContext;
            if (canSetContext != null)
            {
                canSetContext.SetContext(this); 
            }
        }
        private object CreateInstance(Type type)
        {
            object instance;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (MissingMethodException e)
            {
                try
                {
                    instance = Activator.CreateInstance(type, true);
                }
                catch (MissingMethodException e2)
                {
                    Debug.LogError($"{type.FullName} 找不到无参构造函数");
                    throw e2;
                }
            }
            return instance;
        }
        #endregion

        private void HandleIdentities()
        {
            Dictionary<IdentityType, Dictionary<Type, object>> identityIoc  = IocContainer.IdentityIoc;
            foreach (KeyValuePair<IdentityType, Dictionary<Type, object>> outerKv in identityIoc)
            {
                IdentityType identityType = outerKv.Key;

                foreach (KeyValuePair<Type,  object> innerKv in outerKv.Value)
                {
                    object instance = innerKv.Value;
                    PermissionFlags permissions = PermissionForIdentities.GetPermissionsByIdentityType(identityType);
                    //----- 依据权限注入
                    InjectByPermission(instance, permissions);
                    //----- 初始化
                    IAutoInitialize init = instance as IAutoInitialize;
                    if (init != null)
                    {
                        init.AutoInitialize();
                    }
                }
                
            }
            
        }
        private IdentityType GetIdentityTypeByType(Type type)
        {
            foreach(KeyValuePair<IdentityType, Dictionary<Type, object>> kv in IocContainer.IdentityIoc)
            {
                if (kv.Value.TryGetValue(type, out object instance))
                {
                    return kv.Key;
                }
            }
            return IdentityType._None;
        }

        private void SetInstanceField(IdentityType fieldIdentityType, Type fieldType, object instance, FieldInfo f)
        {
            if (IocContainer.IdentityIoc.TryGetValue(fieldIdentityType, out Dictionary<Type, object> dic))
            {
                if (dic.TryGetValue(fieldType, out object fieldInstance))
                {
                    f.SetValue(instance, fieldInstance);
                }
            }
        }

        private void ValidatePermissions(PermissionFlags permissions, PermissionFlags canInject, Type instanceType, IdentityType injectIdentityType)
        {
            if (!permissions.HasFlag(canInject))
            {
                throw new Exception($"{instanceType.FullName} 不允许注入 {nameof(injectIdentityType)} 类型字段");
            }
        }
        
        public void InjectByPermission(object instance, PermissionFlags permissions)
        {
            Type type = instance.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.NonPublic |
                                                    BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo f in fieldInfos)
            {
                if (f.GetCustomAttribute(typeof(InjectAttribute)) is InjectAttribute injectAttr)
                {
                    Type fieldType = f.FieldType;
                    IdentityType fieldIdentityType = GetIdentityTypeByType(fieldType);
                    if (fieldIdentityType == IdentityType._None)
                    {
                        throw new Exception($"{type.FullName} 无法注入非托管类型: {fieldType.FullName}");
                    }
                    if (fieldIdentityType == IdentityType.Model)
                    {
                        ValidatePermissions(permissions, PermissionFlags.InjectModel, type, fieldIdentityType);
                        SetInstanceField(fieldIdentityType, fieldType, instance, f);
                        continue;
                    }
                    if (fieldIdentityType == IdentityType.Service)
                    {
                        ValidatePermissions(permissions, PermissionFlags.InjectService, type, fieldIdentityType);
                        SetInstanceField(fieldIdentityType, fieldType, instance, f);
                        continue;
                    }
                    if (fieldIdentityType == IdentityType.View)
                    {
                        ValidatePermissions(permissions, PermissionFlags.InjectView, type, fieldIdentityType);
                        SetInstanceField(fieldIdentityType, fieldType, instance, f);
                        continue;
                    }
                    if (fieldIdentityType == IdentityType.Controller)
                    {
                        ValidatePermissions(permissions, PermissionFlags.InjectController, type, fieldIdentityType);
                        SetInstanceField(fieldIdentityType, fieldType, instance, f);
                        continue;
                    }

                    if (fieldIdentityType == IdentityType.Identity)
                    {
                        if (fieldType == typeof(LowLevelCommandManager) && !permissions.HasFlag(PermissionFlags.InjectLowLevelCommandManager))
                        {
                            throw new Exception($"{type.FullName} 不允许注入 {nameof(LowLevelCommandManager)} 类型字段");
                        }
                        if (fieldType == typeof(HighLevelCommandManager) && !permissions.HasFlag(PermissionFlags.InjectHighLevelCommandManager))
                        {
                            throw new Exception($"{type.FullName} 不允许注入 {nameof(HighLevelCommandManager)} 类型字段");
                        }
                        SetInstanceField(fieldIdentityType, fieldType, instance, f);
                        continue;
                    }
                }
            }
        }

        //View 类通常需要继承 MonoBehaviour, 对象创建不受框架控制, 因此要从 Unity 获取作为启动参数传入
        public void StartWithViews(params IView[] views)
        {
            if (!_hasStarted)
            {
                Debug.Log("-------------- Context 开始执行 --------------");
                DateTime startTime = DateTime.Now;
                if (views.Length != 0)
                {
                    foreach (IView v in views)
                    {
                        v.SetContext(this);;
                        IocContainer.Add(IdentityType.View, v);
                    }
                }
                //扫描并添加进 IOC 容器
                ScanIdentities();
                //注入依赖 + 调用初始化
                HandleIdentities();
                Debug.Log($"-------------- Context 执行完毕, 用时: {(DateTime.Now - startTime).Milliseconds} ms --------------");
                Debug.Log(_iocContainer);
                _hasStarted = true;
            }
            
        }
    }
    [Flags]
    public enum PermissionFlags : uint
    {
        _None = 0,
        InjectView = 0b00000000000000000000000000000001,
        InjectService = InjectView << 1,
        InjectModel = InjectView << 2, 
        InjectController = InjectView << 3,
        #region CommandManager 细化
        InjectHighLevelCommandManager = InjectView << 4,
        InjectLowLevelCommandManager = InjectView << 5,
        #endregion
    }
    static class PermissionForIdentities
    {
        public static PermissionFlags Controller = PermissionFlags.InjectView | PermissionFlags.InjectService | PermissionFlags.InjectModel | PermissionFlags.InjectHighLevelCommandManager;
        public static PermissionFlags View = PermissionFlags._None | PermissionFlags.InjectLowLevelCommandManager;
        public static PermissionFlags Service = PermissionFlags._None | PermissionFlags.InjectLowLevelCommandManager;
        public static PermissionFlags Model = PermissionFlags._None;
        public static PermissionFlags Identity = PermissionFlags._None;

        public static PermissionFlags GetPermissionsByIdentityType(IdentityType identityType)
        {
            switch (identityType)
            {
                case IdentityType.Controller:
                    return Controller;
                case IdentityType.View:
                    return View;
                case IdentityType.Service:
                    return Service;
                case IdentityType.Model:
                    return Model;
            }
            return Identity;
        }
    }
}
