using Microsoft.AspNetCore.Mvc;
using Ryuka.NlayerApi.Application.Dto.SlotDto;
using Ryuka.NlayerApi.Application.Interfaces;

namespace Ryuka.NlayerApi.Presentation.Controller;

[ApiController]
[Route("api/[controller]")]
public class SlotController : ControllerBase
{
    private readonly ISlotService _SlotService;

    public SlotController(ISlotService SlotService)
    {
        _SlotService = SlotService;
    }

    // GET
    [HttpGet("ping")]
    public IActionResult Index()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSlotDto dto)
    {
        await _SlotService.CreateAsync(dto);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Slots = await  _SlotService.GetAllAsync();
        return Ok(Slots);
    }
}