using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Interfaces;

public interface IUserService
{
    Task<ApplicationUser?> GetByIdAsync(string userId);
    Task<bool> IsInRoleAsync(string userId, string role);
}