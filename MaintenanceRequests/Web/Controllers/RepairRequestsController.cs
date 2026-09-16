using System.Security.Claims;
using MaintenanceRequests.Application.DTOs.RepairRequest;
using MaintenanceRequests.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Client")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var requests = await _service.GetByClientIdAsync(userId);
        return Ok(requests);
    }
    
    [Authorize(Roles = "Worker")]
    [HttpGet("assigned")]
    public async Task<IActionResult> GetAssignedRequests()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var requests = await _service.GetByWorkerIdAsync(userId);
        return Ok(requests);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _service.GetAllAsync();
        return Ok(requests);
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (User.IsInRole("Admin"))
        {
            return Ok(request);
        }

        if (User.IsInRole("Client") && request.ClientId != userId)
        {
            return Forbid();
        }

        if (User.IsInRole("Worker") && request.WorkerId != userId)
        {
            return Forbid();
        }

        return Ok(request);
    }
    
    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRepairRequestDto dto)
    {
        var clientId = GetUserId();
        if (clientId is null)
        {
            return Unauthorized();
        }

        var request = await _service.CreateAsync(dto, clientId);
        return Ok(request);
    }
    
    [Authorize(Roles = "Client")]
    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> Submit(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (request.ClientId != userId)
        {
            return Forbid();
        }

        await _service.SubmitForApprovalAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        await _service.ApproveAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        await _service.RejectAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> Assign(int id, AssignRepairRequestDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        await _service.AssignAsync(id, dto.WorkerId, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Worker")]
    [HttpPost("{id:int}/start")]
    public async Task<IActionResult> Start(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (request.WorkerId != userId)
        {
            return Forbid();
        }

        await _service.StartAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Worker")]
    [HttpPost("{id:int}/waiting")]
    public async Task<IActionResult> Waiting(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (request.WorkerId != userId)
        {
            return Forbid();
        }
        
        await _service.SetWaitingAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Worker")]
    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (request.WorkerId != userId)
        {
            return Forbid();
        }
        
        await _service.CompleteAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        
        await _service.CloseAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    [Authorize(Roles = "Client")]
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, ChangeRequestStatusDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var request = await _service.GetByIdAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        if (request.ClientId != userId)
        {
            return Forbid();
        }

        await _service.CancelAsync(id, userId, dto.Comment);
        return Ok();
    }
    
    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}