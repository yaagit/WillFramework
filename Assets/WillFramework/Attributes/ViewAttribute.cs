using System;

namespace Framework.Attributes.Injection
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : IdentityAttribute
    {
        public ViewAttribute() : base(IdentityType.View)
        {
        }
    }
}