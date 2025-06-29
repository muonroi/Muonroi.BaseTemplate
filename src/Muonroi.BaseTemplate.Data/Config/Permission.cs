namespace Muonroi.BaseTemplate.Data.Config
{
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
        Auth_AssignRoleUser = 1L << 11,
        Auth_RemovePermissionFromRole = 1L << 12,
    }

}
