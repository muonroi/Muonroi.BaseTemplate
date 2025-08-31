



namespace Muonroi.BaseTemplate.API.Seed;

public static class AdminRoleSeeder
{
    public static async Task SeedAsync<TContext>(IServiceProvider services)
        where TContext : MDbContext
    {
        using IServiceScope scope = services.CreateScope();
        TContext db = scope.ServiceProvider.GetRequiredService<TContext>();

        // Ensure Admin role exists (created by HostRoleAndUserCreator)
        MRole? admin = await db.Set<MRole>().AsNoTracking().FirstOrDefaultAsync(r => r.Name == "Admin");
        if (admin == null)
        {
            return;
        }

        // Get all permission ids
        List<Guid> allPermIds = await db.Set<MPermission>().AsNoTracking().Where(p => !p.IsDeleted).Select(p => p.EntityId).ToListAsync();
        if (allPermIds.Count == 0)
        {
            return;
        }

        // Existing mappings
        HashSet<Guid> existing = (await db.Set<MRolePermission>().AsNoTracking()
            .Where(rp => rp.RoleId == admin.EntityId && !rp.IsDeleted)
            .Select(rp => rp.PermissionId)
            .ToListAsync()).ToHashSet();

        List<MRolePermission> toAdd = [];
        foreach (Guid pid in allPermIds)
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

