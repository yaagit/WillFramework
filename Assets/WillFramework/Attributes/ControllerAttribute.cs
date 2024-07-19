using System;

namespace Framework.Attributes.Injection
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : IdentityAttribute
    {
        public ControllerAttribute() : base(IdentityType.Controller)
        {
            
        }
    }
}