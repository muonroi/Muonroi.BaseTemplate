using Muonroi.BuildingBlock.External.Interfaces;
using Muonroi.BuildingBlock.External.Models;

namespace MUONROI.Permissioning;

public class AppPermissionProvider : IPermissionProvider
{
    public IEnumerable<PermissionDefinition> GetPermissions()
    {
        // Administration group: dashboard + roles management
        yield return new PermissionDefinition
        {
            GroupName = "Administration",
            GroupDisplayName = "Administration",
            Permissions = new[]
            {
                "admin.auth.dashboard",
                "admin.roles.view",
                "admin.roles.create",
                "admin.roles.edit",
                "admin.roles.users"
            }
        };

        // Users group (example)
        yield return new PermissionDefinition
        {
            GroupName = "Users",
            GroupDisplayName = "User Management",
            Permissions = new[]
            {
                "users.manage"
            }
        };

        // Rules group
        yield return new PermissionDefinition
        {
            GroupName = "Rules",
            GroupDisplayName = "Rules",
            Permissions = new[]
            {
                "rules.configure"
            }
        };
    }
}

