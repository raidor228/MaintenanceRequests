using MaintenanceRequests.Application.DTOs.RepairRequest;
using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Interfaces;

public interface IRepairRequestService
{
    Task<RepairRequest> CreateAsync(CreateRepairRequestDto dto, string clientId);
    Task<RepairRequest?> GetByIdAsync(int id);
    Task<List<RepairRequest>> GetByClientIdAsync(string clientId);
    Task<List<RepairRequest>> GetAllAsync();
}