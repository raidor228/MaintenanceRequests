using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MaintenanceRequests.Infrastructure.Identity;

public class IdentityUserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await GetByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        return await _userManager.IsInRoleAsync(user, role);
    }
}