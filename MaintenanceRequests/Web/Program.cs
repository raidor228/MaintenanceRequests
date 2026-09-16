using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Application.Services;
using MaintenanceRequests.Infrastructure.Data;
using MaintenanceRequests.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();

var app = builder.Build();

app.Run();