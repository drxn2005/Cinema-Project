using Microsoft.AspNetCore.Identity;

public static class IdentitySeed
{
    public static async Task EnsureSeededAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = ["Admin", "Manager"];
        foreach (var r in roles)
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));

        var adminEmail = "admin@cinema.local";
        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userMgr.CreateAsync(admin, "Admin#123"); 
            await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
