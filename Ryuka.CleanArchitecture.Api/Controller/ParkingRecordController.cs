using Microsoft.AspNetCore.Mvc;
using Ryuka.NlayerApi.Application.Dto;
using Ryuka.NlayerApi.Application.Interfaces;

namespace Ryuka.NlayerApi.Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class ParkingRecordController : ControllerBase
{
    private readonly IParkingRecordService _parkingRecordService;

    public ParkingRecordController(IParkingRecordService parkingRecordService)
    {
        _parkingRecordService = parkingRecordService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _parkingRecordService.GetAllAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateParkingRecordDto? createParkingRecordDto)
    {
        var result = await _parkingRecordService.CreateAsync(createParkingRecordDto);
        return Ok(result);
    }

    [HttpPost(template:"Remoove")]
    public async Task<IActionResult> Remove(string plateNumber)
    {
        var x = await  _parkingRecordService.ExitAsync(plateNumber);
        return Ok(x);
    }
}