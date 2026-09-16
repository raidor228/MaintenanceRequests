using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Domain.Entities;
using MaintenanceRequests.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceRequests.Infrastructure.Repositories;

public class RepairRequestRepository : IRepairRequestRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RepairRequestRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RepairRequest?> GetByIdAsync(int id)
    {
        return await _dbContext.RepairRequests
            .Include(x => x.Category)
            .Include(x => x.Comments)
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<RepairRequest>> GetByClientIdAsync(string clientId)
    {
        return await _dbContext.RepairRequests
            .Include(x => x.Category)
            .Where(x => x.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<List<RepairRequest>> GetAllAsync()
    {
        return await _dbContext.RepairRequests
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task AddAsync(RepairRequest request)
    {
        await _dbContext.RepairRequests.AddAsync(request);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}