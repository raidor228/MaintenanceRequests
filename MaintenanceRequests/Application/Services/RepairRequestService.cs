using MaintenanceRequests.Application.DTOs.RepairRequest;
using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Domain.Entities;

namespace MaintenanceRequests.Application.Services;

public class RepairRequestService : IRepairRequestService
{
    private readonly IRepairRequestRepository _repository;

    public RepairRequestService(IRepairRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<RepairRequest> CreateAsync(CreateRepairRequestDto dto, string clientId)
    {
        var request = new RepairRequest(
            dto.Title,
            dto.Description,
            dto.Priority,
            clientId,
            dto.CategoryId);

        await _repository.AddAsync(request);
        await _repository.SaveChangesAsync();

        return request;
    }

    public async Task<RepairRequest?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<RepairRequest>> GetByClientIdAsync(string clientId)
    {
        return await _repository.GetByClientIdAsync(clientId);
    }

    public async Task<List<RepairRequest>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}