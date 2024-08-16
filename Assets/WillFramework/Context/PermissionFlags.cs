using System;

namespace WillFramework.Context
{
    [Flags]
    internal enum PermissionFlags : uint
    {
        _None = 0,
        InjectView = 0b00000000000000000000000000000001,
        InjectService = InjectView << 1,
        InjectModel = InjectView << 2, 
        InjectController = InjectView << 3,
        #region CommandManager 细化
        InjectHighLevelCommandManager = InjectView << 4,
        InjectLowLevelCommandManager = InjectView << 5,
        InjectCommandManager = InjectView << 6,
        #endregion
    }
}