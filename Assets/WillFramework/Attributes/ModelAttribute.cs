using System;

namespace Framework.Attributes.Injection
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : IdentityAttribute
    {
        public ModelAttribute() : base(IdentityType.Model)
        {
            
        }
    }
}