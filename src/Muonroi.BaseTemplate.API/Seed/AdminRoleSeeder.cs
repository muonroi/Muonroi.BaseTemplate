using Muonroi.BuildingBlock.External.Entity;
using Muonroi.BuildingBlock.External.Entity.Identity;

namespace Muonroi.BaseTemplate.API.Seed;

public static class AdminRoleSeeder
{
    public static async Task SeedAsync<TContext>(IServiceProvider services)
        where TContext : MDbContext
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();

        // Ensure Admin role exists (created by HostRoleAndUserCreator)
        var admin = await db.Set<MRole>().AsNoTracking().FirstOrDefaultAsync(r => r.Name == "Admin");
        if (admin == null)
        {
            return;
        }

        // Get all permission ids
        var allPermIds = await db.Set<MPermission>().AsNoTracking().Where(p => !p.IsDeleted).Select(p => p.EntityId).ToListAsync();
        if (allPermIds.Count == 0)
        {
            return;
        }

        // Existing mappings
        var existing = (await db.Set<MRolePermission>().AsNoTracking()
            .Where(rp => rp.RoleId == admin.EntityId && !rp.IsDeleted)
            .Select(rp => rp.PermissionId)
            .ToListAsync()).ToHashSet();

        List<MRolePermission> toAdd = [];
        foreach (var pid in allPermIds)
        {
            if (!existing.Contains(pid))
            {
                toAdd.Add(new MRolePermission { RoleId = admin.EntityId, PermissionId = pid });
            }
        }

        if (toAdd.Count > 0)
        {
            await db.Set<MRolePermission>().AddRangeAsync(toAdd);
            await db.SaveChangesAsync();
        }
    }
}

