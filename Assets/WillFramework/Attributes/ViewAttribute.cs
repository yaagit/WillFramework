using System;
using WillFramework.Attributes.Types;
namespace WillFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : IdentityAttribute
    {
        public ViewAttribute() : base(IdentityType.View)
        {
        }
    }
}