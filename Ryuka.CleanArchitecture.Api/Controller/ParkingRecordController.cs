using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var result = await _parkingRecordService.GetAllAsync();
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateParkingRecordDto? dto) // Dto geliyosa zten bodyden 
    {
        var result = await _parkingRecordService.CreateAsync(dto);
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("Remove")]
    public async Task<IActionResult> Remove( string plateNumber)
    {
        var result = await _parkingRecordService.ExitAsync(plateNumber);
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _parkingRecordService.GetByIdAsync(id);
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetActiveByPlate")]
    public async Task<IActionResult> GetActiveByPlate(string plate)
    {
        var result = await _parkingRecordService.GetActiveByVehiclePlateAsync(plate);
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetHistoryByPlate")]
    public async Task<IActionResult> GetHistoryByPlate(string plate)
    {
        var result = await _parkingRecordService.GetHistoryByPlateAsync(plate);
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetOccupiedSlotCount")]
    public async Task<IActionResult> GetOccupiedSlotCount()
    {
        var result = await _parkingRecordService.GetOccupiedSlotCountAsync();
        return result.isSuccess ? Ok(result) : BadRequest(result);
    }


}
