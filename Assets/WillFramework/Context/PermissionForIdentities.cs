using WillFramework.Attributes.Types;

namespace WillFramework.Context
{
    internal static class PermissionForIdentities
    {
        public static PermissionFlags View = PermissionFlags._None | PermissionFlags.InjectModel | PermissionFlags.InjectService;
        public static PermissionFlags Service = PermissionFlags._None | PermissionFlags.InjectModel | PermissionFlags.InjectHighLevelCommandManager;
        public static PermissionFlags Model = PermissionFlags._None;
        public static PermissionFlags Identity = PermissionFlags._None;

        public static PermissionFlags GetPermissionsByIdentityType(IdentityType identityType)
        {
            switch (identityType)
            {
                case IdentityType.View:
                    return View;
                case IdentityType.Service:
                    return Service;
                case IdentityType.Model:
                    return Model;
            }
            return Identity;
        }
    }
}