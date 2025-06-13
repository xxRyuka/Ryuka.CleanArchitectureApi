using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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

    public ParkingRecordService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private async Task<Slot> returnFreeSlot()
    {
        return await _unitOfWork.Slots.Where(s => s.isOccupied == false).FirstOrDefaultAsync();
    }

    public async Task<string> CreateAsync(CreateParkingRecordDto? dto)
    {
        if (dto == null)
        {
            return "dto is null";
        }

        try
        {
            await _unitOfWork.BeginTransaction(); // transactionu baslatiyoruz 


            var vehicle = await _unitOfWork.Vehicles.Where(w => w.PlateNumber == dto.VehiclePlate)
                .FirstOrDefaultAsync();

            bool aracEkli = await _unitOfWork.ParkingRecords
                .Table
                .Include(c=>c.Vehicle)
                .AnyAsync(cs=>cs.Vehicle.PlateNumber == dto.VehiclePlate);
            if (aracEkli)
            {
                var slotidd = await _unitOfWork.ParkingRecords.FirstOrDefaultAsync(w => w.VehicleId == vehicle.Id);
                
                return $" Araba zaten ekli  mevcut slot : {slotidd.SlotId} ";
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

            if (slot == null || slot.isOccupied == true)
            {
                Console.WriteLine("slot is null");
                // Slot Boş ise boşta olan herhangi bir slota alabiliriz arabayi
                slot = await _unitOfWork.Slots.Where(s => s.isOccupied == false).FirstOrDefaultAsync();
                if (slot.isOccupied == true)
                {
                    return "all slots are full";
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

            return $"{dto.VehiclePlate} plakali aracin islem basarili  slot id {slot.Id}";
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction(); // Eger bir sorun olustuysa bu sekilde geri aliyoruz değişiklikleri
            return e.Message;
        }
    }

    public async Task<string> ExitAsync(string plate)
    {
        try
        {
            var record = await _unitOfWork.ParkingRecords
                .Table
                .Include(v => v.Vehicle)
                .Include(p => p.Slot)
                .FirstOrDefaultAsync(v => v.Vehicle.PlateNumber == plate ); // && v.Slot.isOccupied == true ekleyerek daha düzgün yazicaz sorguyu
                                                                                        // ama bi kontrol etmem lazim digerlerini

            if (record == null)
            {
                return "record is null";
            }

            await _unitOfWork.BeginTransaction();
            
            // zaten record ifadesinde dolu ise getir dediğim için slot zaten doludur tekrardan kontrolunu saglamaya gerek yok 
            record.Slot.isOccupied = false; // slotu boşalt 

            record.ExitTime = DateTime.UtcNow;

            var duration = (record.ExitTime - record.EntryTime).TotalSeconds;
            record.Price = (int)(duration);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            ParkingRecordDto dto = new ParkingRecordDto()
            {
                Slot = new SlotDto()
                {
                    id = record.Slot.Id,
                    isOccupied = record.Slot.isOccupied,
                },
                EntryTime = record.EntryTime,
                ExitTime = record.ExitTime,
                VehicleId = record.Vehicle.Id,
                Vehicle = new VehicleDto()
                {
                    PlateNumber = record.Vehicle.PlateNumber,
                    id = record.Vehicle.Id,
                },
                SlotId = record.Slot.Id,
                id = record.Id,
                Price = record.Price,
            };
            return $"{dto.Vehicle.PlateNumber} aracin cikis islemi basarili {dto.Price} odeyeceksiniz";
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransaction();
            return $"error: {e.Message}";
        }


        return "ok";
    }

    public async Task<List<ParkingRecordDto>> GetAllAsync()
    {
        var list = new List<ParkingRecordDto>();
        var parkingRecords = await _unitOfWork.ParkingRecords
            .Where(pr => pr.EntryTime != null)
            .Include(pr => pr.Vehicle)
            .Include(pr => pr.Slot).ToListAsync();


        foreach (var item in parkingRecords)
        {
            ParkingRecordDto dto = new ParkingRecordDto()
            {
                id = item.Id,
                Slot = new SlotDto()
                {
                    isOccupied = item.Slot.isOccupied,
                    id = item.Slot.Id
                },
                Vehicle = new VehicleDto()
                {
                    id = item.Vehicle.Id,
                    PlateNumber = item.Vehicle.PlateNumber,
                },
                EntryTime = item.EntryTime,
                VehicleId = item.Vehicle.Id,
                SlotId = item.Slot.Id,
                ExitTime = item.ExitTime,
                Price = item.Price,
            };
            list.Add(dto);
        }

        return list;
    }

    public Task<ParkingRecordDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}