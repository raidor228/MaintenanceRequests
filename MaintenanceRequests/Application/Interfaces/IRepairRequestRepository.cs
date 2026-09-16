using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Interfaces;

public interface IRepairRequestRepository
{
    Task<RepairRequest?> GetByIdAsync(int id);
    Task<List<RepairRequest>> GetByClientIdAsync(string clientId);
    Task<List<RepairRequest>> GetByWorkerIdAsync(string workerId);
    Task<List<RepairRequest>> GetAllAsync();
    Task AddAsync(RepairRequest request);
    Task SaveChangesAsync();
}