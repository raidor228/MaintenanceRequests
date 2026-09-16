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

    public async Task<List<RepairRequest>> GetByWorkerIdAsync(string workerId)
    {
        return await _repository.GetByWorkerIdAsync(workerId);
    }
    
    public async Task<List<RepairRequest>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    
    public async Task SubmitForApprovalAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.SubmitForApproval(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task ApproveAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Approve(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task RejectAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Reject(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task AssignAsync(int id, string workerId, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Assign(workerId, userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task StartAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Start(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task SetWaitingAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.SetWaiting(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task CompleteAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Complete(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task CloseAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Close(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    public async Task CancelAsync(int id, string userId, string? comment)
    {
        var request = await GetRequiredRequestAsync(id);
        request.Cancel(userId, comment);
        await _repository.SaveChangesAsync();
    }
    
    private async Task<RepairRequest> GetRequiredRequestAsync(int id)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request is null)
        {
            throw new KeyNotFoundException($"Заявка с идентификатором {id} не найдена.");
        }

        return request;
    }
}