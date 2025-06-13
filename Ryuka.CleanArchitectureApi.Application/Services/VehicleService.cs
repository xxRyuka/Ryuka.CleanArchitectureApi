using Ryuka.NlayerApi.Application.Dto.VehicleDto;
using Ryuka.NlayerApi.Application.Interfaces;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<VehicleDto> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);

        VehicleDto dto = new VehicleDto()
        {
            id = entity.Id,
            PlateNumber = entity.PlateNumber,
        };

        return dto;
    }

    public async Task<IEnumerable<VehicleDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.Vehicles.GetAllAsync();

        var list = new List<VehicleDto>();

        foreach (var entity in entities)
        {
            list.Add(new VehicleDto()
            {
                id = entity.Id,
                PlateNumber = entity.PlateNumber,
            });
        }
        
        // yazilimsal katliam 
        //   list.ForEach(entities => new VehicleDto()
        // {
        //     id = entities.id,
        //     PlateNumber = entities.PlateNumber,
        // });

        return list;
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto)
    {
        var entity = new Vehicle()
        {
            PlateNumber = dto.PlateNumber,
        };
        await _unitOfWork.Vehicles.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var newDto = new VehicleDto()
        {
            id = entity.Id,
            PlateNumber = entity.PlateNumber,
        };

        return newDto;
    }

    public async Task<bool> UpdateAsync(int id, UpdateVehicleDto dto)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);
        if (entity == null)
        {
            Console.WriteLine("Vehicle not found");
            return false;
        }
        
        entity.PlateNumber = dto.PlateNumber;
        _unitOfWork.Vehicles.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);
        if (entity == null)  
        {return false;}
        _unitOfWork.Vehicles.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}