using System;
using System.Runtime.Serialization;

namespace Common.Attributes
{
    public class DescribePermissionAttribute : Attribute
    {
        public SystemModule Module { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }

        public DescribePermissionAttribute(SystemModule module, string key, string title, string group)
        {
            Title = title;
            Group = group;
            Key = key;
            Module = module;
        }
    }

    [Flags]
    public enum SystemModule
    {
        [EnumMember(Value = "user_management_module")]
        UserManagementModule = 1 << 1,
        [EnumMember(Value = "tenant_module")] Batch = 1 << 2,
        [EnumMember(Value = "setting_module")] Setting = 1 << 3,
        [EnumMember(Value = "setting_module")] Charge = 1 << 4,
    }
}