using System;
using WillFramework.Attributes.Types;

namespace WillFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : IdentityAttribute
    {
        public ControllerAttribute() : base(IdentityType.Controller)
        {
            
        }
    }
}