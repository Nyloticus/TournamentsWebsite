using Common.Attributes;

namespace Domain.Enums.Roles
{
    public enum PermissionKeys : short
    {

        //UserManagementModule Permissions
        [DescribePermission(SystemModule.UserManagementModule, "read_user", "read_user", "users")]
        ReadUser = 1,
        [DescribePermission(SystemModule.UserManagementModule, "add_user", "add_user", "users")]
        AddUser,
        [DescribePermission(SystemModule.UserManagementModule, "edit_user", "edit_user", "users")]
        EditUser,
        [DescribePermission(SystemModule.UserManagementModule, "remove_user", "remove_user", "users")]
        RemoveUser,


        //Role Module Permissions
        [DescribePermission(SystemModule.UserManagementModule, "read_role", "read_role", "roles")]
        ReadRole,
        [DescribePermission(SystemModule.UserManagementModule, "add_role", "add_role", "roles")]
        AddRole,
        [DescribePermission(SystemModule.UserManagementModule, "edit_role", "edit_role", "roles")]
        EditRole,
        [DescribePermission(SystemModule.UserManagementModule, "remove_role", "remove_role", "roles")]
        RemoveRole,


        //Charge Permissions
        [DescribePermission(SystemModule.Charge, "charge", "charge", "Charges")]
        ReadTenant,


        //Batch Permissions
        [DescribePermission(SystemModule.Batch, "restart-batch", "restart-batch", "Batches")]
        OpenBatch,


        //Setting Permissions
        [DescribePermission(SystemModule.Setting, "read_faq", "read_faq", "Settings")]
        ReadFAQ,
        [DescribePermission(SystemModule.Setting, "edit_faq", "edit_faq", "Settings")]
        EditFAQ,
        [DescribePermission(SystemModule.Setting, "add_faq", "add_faq", "Settings")]
        AddFAQ,
        [DescribePermission(SystemModule.Setting, "remove_faq", "remove_faq", "Settings")]
        RemoveFAQ

    }
}