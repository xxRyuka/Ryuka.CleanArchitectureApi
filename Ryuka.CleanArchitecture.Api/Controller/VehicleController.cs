using Microsoft.AspNetCore.Mvc;
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

    // GET
    [HttpGet("ping")]
    public IActionResult Index()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleDto dto)
    {
        await _vehicleService.CreateAsync(dto);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
     var vehicles = await  _vehicleService.GetAllAsync();
        return Ok(vehicles);
    }
}