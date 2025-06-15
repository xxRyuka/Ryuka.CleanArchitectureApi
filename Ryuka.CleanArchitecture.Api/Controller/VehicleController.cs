using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ryuka.NlayerApi.Application.Dto.VehicleDto;
using Ryuka.NlayerApi.Application.Interfaces;

namespace Ryuka.NlayerApi.Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet("ping")]
    public IActionResult Index()
    {
        return Ok("woırked");
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _vehicleService.GetAllAsync();
        return result.isFailure ? NotFound(result.Errors) : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _vehicleService.GetByIdAsync(id);
        return result.isFailure ? NotFound(result.Errors) : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleDto dto)
    {
        var result = await _vehicleService.CreateAsync(dto);
        return result.isFailure ? BadRequest(result.Errors) : Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateVehicleDto dto)
    {
        var result = await _vehicleService.UpdateAsync(id, dto);
        return result.isFailure ? BadRequest(result.Errors) : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _vehicleService.DeleteAsync(id);
        return result.isFailure ? NotFound(result.Errors) : Ok(result);
    }
}