using MaintenanceRequests.Application.Interfaces;
using MaintenanceRequests.Application.Services;
using MaintenanceRequests.Domain.Entities;
using MaintenanceRequests.Infrastructure.Data;
using MaintenanceRequests.Infrastructure.Identity;
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

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();
builder.Services.AddScoped<IRepairCategoryRepository, RepairCategoryRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();