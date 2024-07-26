using System;
using WillFramework.Attributes.Types;

namespace WillFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : IdentityAttribute
    {
        public ModelAttribute() : base(IdentityType.Model)
        {
            
        }
    }
}