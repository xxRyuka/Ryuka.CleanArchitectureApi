    using Microsoft.AspNetCore.Mvc;
    using Ryuka.NlayerApi.Application.Dto.SlotDto;
    using Ryuka.NlayerApi.Application.Interfaces;

    namespace Ryuka.NlayerApi.Presentation.Controller;
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet("ping")]
        public IActionResult Index() => Ok();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _slotService.GetAllAsync();
            return result.isFailure ? BadRequest(result.Errors) : Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _slotService.GetByIdAsync(id);
            return result.isFailure ? NotFound(result.Errors) : Ok(result);
        }

        [HttpGet("free")]
        public async Task<IActionResult> GetFreeSlots()
        {
            var result = await _slotService.GetFreeSlots();
            return result.isFailure ? BadRequest(result.Errors) : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlotDto dto)
        {
            var result = await _slotService.CreateAsync(dto);
            return result.isFailure ? BadRequest(result.Errors) : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSlotDto dto)
        {
            var result = await _slotService.UpdateAsync(id, dto);
            return result.isFailure ? BadRequest(result.Errors) : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _slotService.DeleteAsync(id);
            return result.isFailure ? NotFound(result.Errors) : Ok(result);
        }
    }
