using System;
using WillFramework.Attributes.Types;
namespace WillFramework.Attributes
{
    /// <summary>
    /// 通用类型的 Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IdentityAttribute : Attribute
    {
        private IdentityType _identityType;
        public IdentityType IdentityType { get => _identityType; }

        public IdentityAttribute()
        {
            _identityType = IdentityType.Identity;
        }
        public IdentityAttribute(IdentityType identityType)
        {
            _identityType = identityType;
        }
    }
}