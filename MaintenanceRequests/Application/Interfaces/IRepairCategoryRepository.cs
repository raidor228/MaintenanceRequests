using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Interfaces;

public interface IRepairCategoryRepository
{
    Task<List<RepairCategory>> GetAllAsync();
}