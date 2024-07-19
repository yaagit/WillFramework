using System;

namespace Framework.Attributes.Injection
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : IdentityAttribute
    {
        public ServiceAttribute() : base(IdentityType.Service)
        {
        }
    }
}