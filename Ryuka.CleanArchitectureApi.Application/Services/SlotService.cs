using AutoMapper;
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
    private readonly IMapper _mapper;

    public SlotService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        // List<SlotDto> freeSlotsDto = new List<SlotDto>();
        // //manual mapping 
        // foreach (var slot in freeSlots)
        // {
        //     freeSlotsDto.Add(new SlotDto()
        //     {
        //         isOccupied = slot.isOccupied,
        //         id = slot.Id,
        //     });
        // }
        var mappedFreeSlots = _mapper.Map<List<SlotDto>>(freeSlots);
        var msg = $"{mappedFreeSlots.Count} Slots found";
        return Result<List<SlotDto>>.Success(mappedFreeSlots,msg);
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
        
        SlotDto MappedDto = _mapper.Map<Slot,SlotDto>(entity);
        // SlotDto dto = new SlotDto()
        // {
        //     id = entity.Id,
        //     isOccupied = entity.isOccupied
        // };

        return Result<SlotDto>.Success(MappedDto);
    }

    public async Task<Result<IEnumerable<SlotDto>>> GetAllAsync()
    {
        var entities = await _unitOfWork.Slots.GetAllAsync();
        var errList = new List<string>();
        if (!entities.Any())
        {
            errList.Add("No Slots found");
            return Result<IEnumerable<SlotDto>>.Failure(errList);
        }
        // var list = new List<SlotDto>();

        // foreach (var entity in entities)
        // {
        //     list.Add(new SlotDto()
        //     {
        //         id = entity.Id,
        //         isOccupied = entity.isOccupied,
        //     });
        // }
        
        var MappedDtoList = _mapper.Map<List<Slot>, List<SlotDto>>(entities.ToList());
        
        return Result<IEnumerable<SlotDto>>.Success(MappedDtoList,message:"islem Basarili");
        
    }

    public async Task<Result<SlotDto>> CreateAsync(CreateSlotDto dto)
    {
        if (dto is null)
        {
            return Result<SlotDto>.Failure(new List<string>(){"slot dto is null"});
        }
        
        var mappedEntity = _mapper.Map<CreateSlotDto, Slot>(dto);
        // var entity = new Slot()
        // {
        //     isOccupied = dto.isOccupied,
        // };
        await _unitOfWork.Slots.CreateAsync(mappedEntity);
        await _unitOfWork.SaveChangesAsync();

        
        
        var mappedDto = _mapper.Map<Slot, SlotDto>(mappedEntity);
        // var newDto = new SlotDto()
        // {
        //     id = entity.Id,
        //     isOccupied = entity.isOccupied,
        // };

        return Result<SlotDto>.Success(mappedDto, message:"slot olusturma islemi Basarili");
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
        var mappedDto = _mapper.Map<Slot, SlotDto>(entity);
        return Result.Succses($"{mappedDto.id}'li slot güncelleme islemi Basarili");
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