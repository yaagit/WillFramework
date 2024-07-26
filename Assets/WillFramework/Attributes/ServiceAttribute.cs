using System;
using WillFramework.Attributes.Types;
namespace WillFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : IdentityAttribute
    {
        public ServiceAttribute() : base(IdentityType.Service)
        {
        }
    }
}