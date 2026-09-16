using MaintenanceRequests.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceRequests.Web.Controllers;

[ApiController]
[Route("api/repair-categories")]
public class RepairCategoriesController : ControllerBase
{
    private readonly IRepairCategoryRepository _categoryRepository;

    public RepairCategoriesController(IRepairCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }
}