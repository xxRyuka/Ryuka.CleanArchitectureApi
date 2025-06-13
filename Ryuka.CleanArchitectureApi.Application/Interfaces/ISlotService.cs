using Ryuka.NlayerApi.Application.Dto.SlotDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface ISlotService
{
    Task<SlotDto> GetByIdAsync(int id);
    Task<IEnumerable<SlotDto>> GetAllAsync();
    Task<SlotDto> CreateAsync(CreateSlotDto dto);
    Task<bool> UpdateAsync(int id, UpdateSlotDto dto);
    Task<bool> DeleteAsync(int id);
}