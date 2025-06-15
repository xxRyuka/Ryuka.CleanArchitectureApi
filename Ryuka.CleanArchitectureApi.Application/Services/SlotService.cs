using Microsoft.EntityFrameworkCore;
using Ryuka.NlayerApi.Application.Common.Concrete;
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


    public async Task<Result<List<SlotDto>>> GetFreeSlots()
    {
        List<string> err = new List<string>();
        var freeSlots = await _unitOfWork.Slots
            .Where(x => x.isOccupied == false)
            .ToListAsync();
        if (freeSlots.Count == 0)
        {
            err.Add("No Slots found");
            return Result<List<SlotDto>>.Failure(err);
        }

        List<SlotDto> freeSlotsDto = new List<SlotDto>();
        //manual mapping 
        foreach (var slot in freeSlots)
        {
            freeSlotsDto.Add(new SlotDto()
            {
                isOccupied = slot.isOccupied,
                id = slot.Id,
            });
        }

        var msg = $"{freeSlots.Count} Slots found";
        return Result<List<SlotDto>>.Success(freeSlotsDto,msg);
    }

    public async Task<Result<SlotDto>> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);
        var errList = new List<string>();
        if (entity == null)
        {
            errList.Add("slot is not found");
            return Result<SlotDto>.Failure(errList);
        }
        SlotDto dto = new SlotDto()
        {
            id = entity.Id,
            isOccupied = entity.isOccupied
        };

        return Result<SlotDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<SlotDto>>> GetAllAsync()
    {
        var entities = await _unitOfWork.Slots.GetAllAsync();
        var errList = new List<string>();
        if (entities.Count==0)
        {
            errList.Add("No Slots found");
            return Result<IEnumerable<SlotDto>>.Failure(errList);
        }
        var list = new List<SlotDto>();

        foreach (var entity in entities)
        {
            list.Add(new SlotDto()
            {
                id = entity.Id,
                isOccupied = entity.isOccupied,
            });
        }
        
        return Result<IEnumerable<SlotDto>>.Success(list,message:"islem Basarili");
        
    }

    public async Task<Result<SlotDto>> CreateAsync(CreateSlotDto dto)
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

        return Result<SlotDto>.Success(newDto, message:"slot olusturma islemi Basarili");
    }

    public async Task<Result> UpdateAsync(int id, UpdateSlotDto dto)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);
        var errList = new List<string>(); 
        if (entity == null)
        {
            errList.Add("slot No found");
            // Console.WriteLine("Slot not found");
            return Result.Fail(errList);
        }

        entity.isOccupied = dto.isOccupied;
        _unitOfWork.Slots.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Result.Succses($"{id}'li slot güncelleme islemi Basarili");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Slots.FindAsync(id);
        var errlist = new List<string>();
        if (entity == null)
        {
            errlist.Add("No Slots found");
            return Result.Fail(errlist);
        }

        _unitOfWork.Slots.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return Result.Succses($"{id}'li slot silme işlemi islemi Basarili");    }
}