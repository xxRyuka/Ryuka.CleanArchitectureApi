using Ryuka.NlayerApi.Application.Dto.SlotDto;
using Ryuka.NlayerApi.Application.Interfaces;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Services;

public class SlotService : ISlotService
{
    private readonly IUnitOfWork _unitOfWork;

    public SlotService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<SlotDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);

        SlotDto dto = new SlotDto()
        {
            id = entity.Id,
           isOccupied = entity.isOccupied
        };

        return dto;
    }

    public async Task<IEnumerable<SlotDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.Slots.GetAllAsync();

        var list = new List<SlotDto>();

        foreach (var entity in entities)
        {
            list.Add(new SlotDto()
            {
                id = entity.Id,
                isOccupied = entity.isOccupied,
            });
        }


        return list;
    }

    public async Task<SlotDto> CreateAsync(CreateSlotDto dto)
    {
        var entity = new Slot()
        {
            isOccupied = dto.isOccupied,
        };
        await _unitOfWork.Slots.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var newDto = new SlotDto()
        {
            id = entity.Id,
            isOccupied = entity.isOccupied,
        };

        return newDto;
    }

    public async Task<bool> UpdateAsync(int id, UpdateSlotDto dto)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);
        if (entity == null)
        {
            Console.WriteLine("Slot not found");
            return false;
        }
        
        entity.isOccupied = dto.isOccupied;
        _unitOfWork.Slots.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);
        if (entity == null)  
        {return false;}
        _unitOfWork.Slots.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}