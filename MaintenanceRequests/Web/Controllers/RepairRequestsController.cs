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
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }
        
        return Ok(request);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateRepairRequestDto dto)
    {
        const string clientId = "test-client";
        var request = await _service.CreateAsync(dto, clientId);
        return Ok(request);
    }
    
    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> Submit(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-client";
        await _service.SubmitForApprovalAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-admin";
        await _service.ApproveAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-admin";
        await _service.RejectAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> Assign(int id, AssignRepairRequestDto dto)
    {
        const string userId = "test-admin";
        await _service.AssignAsync(id, dto.WorkerId, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/start")]
    public async Task<IActionResult> Start(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-worker";
        await _service.StartAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/waiting")]
    public async Task<IActionResult> Waiting(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-worker";
        await _service.SetWaitingAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-worker";
        await _service.CompleteAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-admin";
        await _service.CloseAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, ChangeRequestStatusDto dto)
    {
        const string userId = "test-client";
        await _service.CancelAsync(id, userId, dto.Comment);
        return Ok();
    }
}