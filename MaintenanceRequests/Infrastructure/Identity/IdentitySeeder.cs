using MaintenanceRequests.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MaintenanceRequests.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles =
        {
            "Client",
            "Worker",
            "Admin"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminPassword = configuration["Seed:AdminPassword"];
        await CreateUserAsync(
            userManager,
            "admin",
            "admin@example.com",
            adminPassword,
            "Admin");

        var workerPassword = configuration["Seed:WorkerPassword"];
        await CreateUserAsync(
            userManager,
            "worker",
            "worker@example.com",
            workerPassword,
            "Worker");
    }

    private static async Task CreateUserAsync(UserManager<ApplicationUser> userManager,
        string userName, string email, string password, string role)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is not null)
        {
            return;
        }

        user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException($"Не удалось создать пользователя {userName}: {errors}");
        }

        await userManager.AddToRoleAsync(user, role);
    }
}