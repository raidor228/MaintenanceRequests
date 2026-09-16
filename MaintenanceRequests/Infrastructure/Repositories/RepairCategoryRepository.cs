using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Domain.Entities;
using MaintenanceRequests.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceRequests.Infrastructure.Repositories;

public class RepairCategoryRepository : IRepairCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RepairCategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RepairCategory>> GetAllAsync()
    {
        return await _dbContext.RepairCategories
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}