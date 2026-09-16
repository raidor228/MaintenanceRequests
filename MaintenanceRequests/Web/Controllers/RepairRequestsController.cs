using MaintenanceRequests.Application.DTOs.RepairRequest;
using MaintenanceRequests.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceRequests.Web.Controllers;

[ApiController]
[Route("api/repair-requests")]
public class RepairRequestsController : ControllerBase
{
    private readonly IRepairRequestService _service;
    
    public RepairRequestsController(IRepairRequestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _service.GetAllAsync();
        return Ok(requests);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateRepairRequestDto dto)
    {
        const string clientId = "test-client";
        var request = await _service.CreateAsync(dto, clientId);
        return Ok(request);
    }
}