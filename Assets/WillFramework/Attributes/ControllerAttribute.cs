using System;
using WillFramework.Attributes.Types;

namespace WillFramework.Attributes
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