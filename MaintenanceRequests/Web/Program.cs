using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Application.Services;
using MaintenanceRequests.Domain.Entities;
using MaintenanceRequests.Infrastructure.Data;
using MaintenanceRequests.Infrastructure.Repositories;
using MaintenanceRequests.Web.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "Web/wwwroot"
});

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager();

builder.Services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();
builder.Services.AddScoped<IRepairCategoryRepository, RepairCategoryRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();