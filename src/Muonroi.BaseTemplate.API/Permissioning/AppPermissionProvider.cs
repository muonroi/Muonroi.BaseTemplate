using Muonroi.BuildingBlock.External.Interfaces;
using Muonroi.BuildingBlock.External.Models;

namespace MUONROI.Permissioning;

public class AppPermissionProvider : IPermissionProvider
{
    public IEnumerable<PermissionDefinition> GetPermissions()
    {
        // Administration group: dashboard + roles management
        // IMPORTANT: Permission names must use underscores (_) not dots (.)
        // They must match the Permission enum exactly
        yield return new PermissionDefinition
        {
            GroupName = "Administration",
            GroupDisplayName = "Administration",
            Permissions =
            [
                "Admin_Auth_Dashboard",
                "Admin_Roles_View",
                "Admin_Roles_Create",
                "Admin_Roles_Edit",
                "Admin_Roles_Users"
            ]
        };

        // Users group (example)
        yield return new PermissionDefinition
        {
            GroupName = "Users",
            GroupDisplayName = "User Management",
            Permissions =
            [
                "Users_Manage"
            ]
        };

        // Rules group
        yield return new PermissionDefinition
        {
            GroupName = "Rules",
            GroupDisplayName = "Rules",
            Permissions =
            [
                "Rules_Configure"
            ]
        };
    }
}

