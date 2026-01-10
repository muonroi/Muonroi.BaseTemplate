namespace Muonroi.BaseTemplate.Data.Config
{
    /// <summary>
    /// Permission enum for the application.
    /// IMPORTANT: Permission names must match database mpermissions.Name exactly.
    /// Use underscores (_) instead of dots (.) in permission names.
    /// Example: Use "Admin_Auth_Dashboard" not "admin.auth.dashboard"
    /// </summary>
    [Flags]
    public enum Permission : long
    {
        None = 0,
        Auth_CreateRole = 1L << 0,
        Auth_UpdateRole = 1L << 1,
        Auth_DeleteRole = 1L << 2,
        Auth_GetRoles = 1L << 3,
        Auth_GetRoleById = 1L << 4,
        Auth_AssignPermission = 1L << 5,
        Auth_GetPermissions = 1L << 6,
        Auth_GetRolePermissions = 1L << 7,
        Auth_GetRoleUsers = 1L << 8,
        Auth_All = 1L << 9,
        Auth_AssignRoleUser = 1L << 11,
        Auth_RemovePermissionFromRole = 1L << 12,
        Sample_Create = 1L << 13,
        Sample_GetAll = 1L << 14,
        Notification_Create = 1L << 15,
        Notification_GetAll = 1L << 16,
        Container_Create = 1L << 17,
        Admin_Auth_Dashboard = 1L << 18,
        Admin_Roles_View = 1L << 19,
        Admin_Roles_Create = 1L << 20,
        Admin_Roles_Edit = 1L << 21,
        Admin_Roles_Users = 1L << 22,
        Users_Manage = 1L << 23,
        Rules_Configure = 1L << 24,
    }

}
