using Ryuka.NlayerApi.Application.Common.Concrete;
using Ryuka.NlayerApi.Application.Dto.SlotDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface ISlotService
{
    
    Task<Result<List<SlotDto>>> GetFreeSlots();
    Task<Result<SlotDto>> GetByIdAsync(int slotId);
    // Task<SlotDto> GetByIdAsync(int id);
    Task<Result<IEnumerable<SlotDto>>> GetAllAsync();
    Task<Result<SlotDto>> CreateAsync(CreateSlotDto dto);
    Task<Result> UpdateAsync(int id, UpdateSlotDto dto);
    Task<Result> DeleteAsync(int id);
}