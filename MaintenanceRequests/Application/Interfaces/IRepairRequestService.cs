using MaintenanceRequests.Application.DTOs.RepairRequest;
using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Interfaces;

public interface IRepairRequestService
{
    Task<RepairRequest> CreateAsync(CreateRepairRequestDto dto, string clientId);
    Task<RepairRequest?> GetByIdAsync(int id);
    Task<List<RepairRequest>> GetByClientIdAsync(string clientId);
    Task<List<RepairRequest>> GetAllAsync();

    Task SubmitForApprovalAsync(int id, string userId, string? comment);
    Task ApproveAsync(int id, string userId, string? comment);
    Task RejectAsync(int id, string userId, string? comment);
    Task AssignAsync(int id, string workerId, string userId, string? comment);
    Task StartAsync(int id, string userId, string? comment);
    Task SetWaitingAsync(int id, string userId, string? comment);
    Task CompleteAsync(int id, string userId, string? comment);
    Task CloseAsync(int id, string userId, string? comment);
    Task CancelAsync(int id, string userId, string? comment);
}