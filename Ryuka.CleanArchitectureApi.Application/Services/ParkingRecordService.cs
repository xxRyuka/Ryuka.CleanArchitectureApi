using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ryuka.NlayerApi.Application.Common.Concrete;
using Ryuka.NlayerApi.Application.Dto;
using Ryuka.NlayerApi.Application.Dto.SlotDto;
using Ryuka.NlayerApi.Application.Dto.VehicleDto;
using Ryuka.NlayerApi.Application.Interfaces;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;


namespace Ryuka.NlayerApi.Application.Services;

public class ParkingRecordService : IParkingRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ParkingRecordService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private async Task<Slot> returnFreeSlot()
    {
        return await _unitOfWork.Slots.Where(s => s.isOccupied == false).FirstOrDefaultAsync();
    }

    public async Task<Result<ParkingRecordDto>> CreateAsync(CreateParkingRecordDto? dto)
    {
        var errList = new List<string>();
        if (dto == null)
        {
            errList.Add("dto is null");
            return Result<ParkingRecordDto>.Failure(errList);
        }

        try
        {
            await _unitOfWork.BeginTransaction(); // transactionu baslatiyoruz 


                var vehicle = await _unitOfWork.Vehicles.Where(w => w.PlateNumber == dto.VehiclePlate)
                    .FirstOrDefaultAsync();

            bool aracEkli = await _unitOfWork.ParkingRecords
                .Table
                .Include(c => c.Vehicle)
                .AnyAsync(cs => cs.Vehicle.PlateNumber == dto.VehiclePlate && cs.ExitTime==DateTime.MinValue);
            if (aracEkli)
            {
                var slotidd = await _unitOfWork.ParkingRecords
                    .FirstOrDefaultAsync(w => w.VehicleId == vehicle.Id && w.ExitTime==DateTime.MinValue);
                errList.Add("Vehicle already exists");
                return Result<ParkingRecordDto>.Failure(errList);
            }

            if (vehicle == null)
            {
                vehicle = new Vehicle()
                {
                    PlateNumber = dto.VehiclePlate
                };
                await _unitOfWork.Vehicles.CreateAsync(vehicle);
                await _unitOfWork
                    .SaveChangesAsync(); // transaction kullanmadan önce asagida br hata olustugunda bos yere buraya araba ekliyordu
                // sorunu araba eklemeyi en sona alarak çözebilirdik fakat unit of work pattern kullanmamımız en büyük sebeplerinden birisi
                // Transactiondur ve bunu iyi ogrenmek gerektiği için sorunu boyle çözmeyi tercih ettim
            }


            var slot = await _unitOfWork.Slots.Where(s => s.Id == dto.SlotDtoId).FirstOrDefaultAsync();

            if (slot == null || slot?.isOccupied == true) 
            {
                Console.WriteLine("slot is null");
                // Slot Boş ise boşta olan herhangi bir slota alabiliriz arabayi
                slot = await _unitOfWork.Slots.Where(s => s.isOccupied == false).FirstOrDefaultAsync();
                if (slot == null)
                {
                    errList.Add("all slots are full");
                    return Result<ParkingRecordDto>.Failure(errList);
                }
            }


            ParkingRecord entity = new ParkingRecord()
            {
                EntryTime = DateTime.UtcNow,
                VehicleId = vehicle.Id,
                SlotId = slot.Id,
            };

            await _unitOfWork.ParkingRecords.CreateAsync(entity);
            slot.isOccupied = true;

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork
                .CommitTransaction(); // Buraya kadar bir sorun olusmadiysa artik veritabanina gonderebiliriz değişiklierimizi
            //
            // ParkingRecordDto prDTO = new ParkingRecordDto()
            // {
            //     EntryTime = entity.EntryTime,
            //     VehicleId = entity.VehicleId,
            //     SlotId = slot.Id,
            // };
            
            var mappedDto = _mapper.Map<ParkingRecord,ParkingRecordDto>(entity);
            return Result<ParkingRecordDto>.Success(mappedDto,
                message: $"{mappedDto.Vehicle.PlateNumber} plakali aracin islem basarili  slot id {mappedDto.SlotId}");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction(); // Eger bir sorun olustuysa bu sekilde geri aliyoruz değişiklikleri
            return Result<ParkingRecordDto>.Failure(errList, e.Message);
        }
    } // Refactored 

  public async Task<Result> ExitAsync(string plate) // mapped
{
    var errList = new List<string>();
    try
    {
        var record = await _unitOfWork.ParkingRecords
            .Table
            .Include(v => v.Vehicle)
            .Include(p => p.Slot)
            .FirstOrDefaultAsync(v =>
                v.Vehicle.PlateNumber == plate);

        if (record == null)
        {
            errList.Add("record is null");
            return Result.Fail(errList);
        }

        await _unitOfWork.BeginTransaction();

        // Slot boşaltma
        record.Slot.isOccupied = false;

        // Çıkış zamanı nullable DateTime? olduğundan burası sorun çıkarmaz
        record.ExitTime = DateTime.UtcNow;

        // Duration hesaplaması için null kontrolü yapılmalı
        if (record.ExitTime == null)
        {
            errList.Add("ExitTime is null, cannot calculate duration");
            await _unitOfWork.RollbackTransaction();
            return Result.Fail(errList);
        }

        var duration = (record.ExitTime.Value - record.EntryTime).TotalSeconds;
        record.Price = (int)(duration);

        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransaction();

        var mappedDto =  _mapper.Map<ParkingRecord, ParkingRecordDto>(record);
        
        // ParkingRecordDto dto = new ParkingRecordDto()
        // {
        //     Slot = new SlotDto()
        //     {
        //         id = record.Slot.Id,
        //         isOccupied = record.Slot.isOccupied,
        //     },
        //     EntryTime = record.EntryTime,
        //     ExitTime = record.ExitTime.Value, // Burada DTO da nullable DateTime? olmalı
        //     VehicleId = record.Vehicle.Id,
        //     Vehicle = new VehicleDto()
        //     {
        //         PlateNumber = record.Vehicle.PlateNumber,
        //         id = record.Vehicle.Id,
        //     },
        //     SlotId = record.Slot.Id,
        //     id = record.Id,
        //     Price = record.Price,
        // };

        return Result.Succses(
            message: $"{mappedDto.Vehicle.PlateNumber} aracin cikis islemi basarili {mappedDto.Price} odeyeceksiniz");
    }
    catch (Exception e)
    {
        await _unitOfWork.RollbackTransaction();
        return Result.Fail(errList, e.Message);
    }
}

    public async Task<Result<IEnumerable<ParkingRecordDto>>> GetAllAsync() // mapped
    {
        // var list = new List<ParkingRecordDto>();
        var parkingRecords = await _unitOfWork.ParkingRecords
            .Where(pr => pr.EntryTime != null)
            .Include(pr => pr.Vehicle)
            .Include(pr => pr.Slot).ToListAsync();


        var MappedList = _mapper.Map<IEnumerable<ParkingRecord>, IEnumerable<ParkingRecordDto>>(parkingRecords);
        
        // foreach (var item in parkingRecords)
        // {
        //     ParkingRecordDto dto = new ParkingRecordDto()
        //     {
        //         id = item.Id,
        //         Slot = new SlotDto()
        //         {
        //             isOccupied = item.Slot.isOccupied,
        //             id = item.Slot.Id
        //         },
        //         Vehicle = new VehicleDto()
        //         {
        //             id = item.Vehicle.Id,
        //             PlateNumber = item.Vehicle.PlateNumber,
        //         },
        //         EntryTime = item.EntryTime,
        //         VehicleId = item.Vehicle.Id,
        //         SlotId = item.Slot.Id,
        //         ExitTime = item.ExitTime.Value,
        //         Price = item.Price,
        //     };
        //     list.Add(dto);
        // }

        return Result<IEnumerable<ParkingRecordDto>>.Success(MappedList);
    } //Refactored 

    public async Task<Result<ParkingRecordDto>> GetByIdAsync(int id) // mapped
    {
        var errList = new List<string>();

        var record = await _unitOfWork.ParkingRecords
            .Table
            .Include(pr => pr.Vehicle)
            .Include(pr => pr.Slot)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if (record == null)
        {
            errList.Add($"id : {id} record is null");
            return Result<ParkingRecordDto>.Failure(errList);
        }

        var mappedDto =  _mapper.Map<ParkingRecord, ParkingRecordDto>(record);
        // var dto = new ParkingRecordDto()
        // {
        //     EntryTime = record.EntryTime,
        //     ExitTime = record.ExitTime.Value,
        //     VehicleId = record.Vehicle.Id,
        //     Vehicle = new VehicleDto()
        //     {
        //         id = record.Vehicle.Id,
        //         PlateNumber = record.Vehicle.PlateNumber,
        //     },
        //     SlotId = record.Slot.Id,
        //     Slot = new SlotDto()
        //     {
        //         id = record.Slot.Id,
        //         isOccupied = record.Slot.isOccupied,
        //     },
        //     id = record.Id,
        //     Price = record.Price,
        // };
        return Result<ParkingRecordDto>.Success(mappedDto);
    } // İmplemented and Refactored 

    public async Task<Result<ParkingRecordDto>> GetActiveByVehiclePlateAsync(string plate) // mapped
    {
        var errList = new List<string>();
        var record = await _unitOfWork
            .ParkingRecords
            .Table
            .Include(pr => pr.Vehicle)
            .Include(pr => pr.Slot)
            .FirstOrDefaultAsync(pr =>
                pr.Vehicle.PlateNumber == plate && pr.ExitTime == null); // Cıkıs yapmamıs ve plakaya gore query

        if (record == null)
        {
            errList.Add($"plate :  {plate} active record is null");
            return Result<ParkingRecordDto>.Failure(errList);
        }
        
        var mappedDto = _mapper.Map<ParkingRecord, ParkingRecordDto>(record);
        // var dto = new ParkingRecordDto()
        // {
        //     EntryTime = record.EntryTime,
        //     ExitTime = record.ExitTime ?? DateTime.MinValue, // null ise hatayla karsılasiyoruz
        //     VehicleId = record.Vehicle.Id,
        //     Vehicle = new VehicleDto()
        //     {
        //         id = record.Vehicle.Id,
        //         PlateNumber = record.Vehicle.PlateNumber,
        //     },
        //     SlotId = record.Slot.Id,
        //     Slot = new SlotDto()
        //     {
        //         id = record.Slot.Id,
        //         isOccupied = record.Slot.isOccupied,
        //     },
        //     id = record.Id,
        //     Price = record.Price,
        // };
        return Result<ParkingRecordDto>.Success(mappedDto);
    } // İmplemented and Refactored


    public async Task<Result<IEnumerable<ParkingRecordDto>>> GetHistoryByPlateAsync(string plate) // mapped
    {
        var errList = new List<string>();

        var records = await _unitOfWork
            .ParkingRecords
            .Table
            .Include(pr => pr.Vehicle)
            .Include(pr => pr.Slot)
            .Where(pr => pr.Vehicle.PlateNumber == plate)
            .ToListAsync();
        // Secilen plakaya gore kayitlari getir 

        
        var mappedList = _mapper.Map<IEnumerable<ParkingRecord>, IEnumerable<ParkingRecordDto>>(records);
        
        // var dtoList = records.Select(record => new ParkingRecordDto()
        // {
        //     Price = record.Price,
        //     id = record.Id,
        //     EntryTime = record.EntryTime,
        //     ExitTime = record.ExitTime ?? DateTime.MinValue,
        //     VehicleId = record.Vehicle.Id,
        //     Vehicle = new VehicleDto()
        //     {
        //         id = record.Vehicle.Id,
        //         PlateNumber = record.Vehicle.PlateNumber,
        //     },
        //     SlotId = record.Slot.Id,
        //     Slot = new SlotDto()
        //     {
        //         id = record.Slot.Id,
        //         isOccupied = record.Slot.isOccupied,
        //     }
        // }).ToList();
        return Result<IEnumerable<ParkingRecordDto>>.Success(mappedList);  
    }

    public async Task<Result> GetOccupiedSlotCountAsync()
    {
        int count = await _unitOfWork.Slots.CountAsync(s => s.isOccupied);

        return Result.Succses(message: $" {count} adet dolu slot var  ");
    }
}