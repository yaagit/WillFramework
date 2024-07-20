using System;
using System.Collections.Generic;
using System.Text;
using WillFramework.Attributes;

namespace WillFramework
{
    /// <summary>
    /// </summary>
    public class IocContainer
    {
        private Dictionary<IdentityType, Dictionary<Type, object>> _identityIoc;
        
        public Dictionary<IdentityType, Dictionary<Type, object>> IdentityIoc
        {
            get => _identityIoc;
        }
        
        public IocContainer()
        {
            _identityIoc = new();
        }

        public void Add(IdentityType identityType, object instance)
        {
            Type instanceType = instance.GetType();
            if (_identityIoc.TryGetValue(identityType, out Dictionary<Type, object> value))
            {
                value.Add(instanceType, instance);
            }
            else
            {
                value = new Dictionary<Type, object>();
                value.Add(instanceType, instance);
                _identityIoc.Add(identityType, value);
            }
        }

        public void Remove(IdentityType identityType, object instance)
        {
            if (_identityIoc.TryGetValue(identityType, out Dictionary<Type, object> value))
            {
                value.Remove(instance.GetType());
                if (value.Count == 0)
                {
                    _identityIoc.Remove(identityType);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("-------------------------- IOC Container --------------------------\n");
            foreach (KeyValuePair<IdentityType, Dictionary<Type, object>> outerKv in _identityIoc)
            {
                result.Append($"{outerKv.Key}:").Append("\n\t");
                foreach (KeyValuePair<Type, object> innerKv in outerKv.Value)
                {
                    result.Append($"{innerKv.Key.FullName}").Append(" : ").Append(nameof(Object)).Append("\n\t");
                }
                result.Append("\n");
            }
            result.Append("-------------------------------------------------------------------");
            return result.ToString();
        }
    }
}