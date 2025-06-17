using AutoMapper;
using Ryuka.NlayerApi.Application.Common.Concrete;
using Ryuka.NlayerApi.Application.Dto.VehicleDto;
using Ryuka.NlayerApi.Application.Interfaces;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehicleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<VehicleDto>> GetByIdAsync(int id)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);
        if (entity == null)
        {
            return Result<VehicleDto>.Failure(new List<string> { "Vehicle not found" });
        }

        var mappedDto = _mapper.Map<Vehicle,VehicleDto>(entity);
        
        // var dto = new VehicleDto
        // {
        //     id = entity.Id,
        //     PlateNumber = entity.PlateNumber,
        // };

        return Result<VehicleDto>.Success(mappedDto, "Vehicle found");
    }

    public async Task<Result<IEnumerable<VehicleDto>>> GetAllAsync()
    {
        var entities = await _unitOfWork.Vehicles.GetAllAsync();
        if (entities.Count == 0)
        {
            return Result<IEnumerable<VehicleDto>>.Failure(new List<string> { "No vehicles found" });
        }

        
        var mappedEntities = _mapper.Map<IEnumerable<Vehicle>,IEnumerable<VehicleDto>>(entities);
        
        var list = entities.Select(entity => new VehicleDto
        {
            id = entity.Id,
            PlateNumber = entity.PlateNumber
        });

        return Result<IEnumerable<VehicleDto>>.Success(mappedEntities, "Vehicles listed");
    }

    public async Task<Result<VehicleDto>> CreateAsync(CreateVehicleDto dto)
    {
        var entity = new Vehicle
        {
            PlateNumber = dto.PlateNumber
        };

        await _unitOfWork.Vehicles.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    
        
        var mappedDto = _mapper.Map<Vehicle, VehicleDto>(entity);
        var newDto = new VehicleDto
        {
            id = entity.Id,
            PlateNumber = entity.PlateNumber
        };

        return Result<VehicleDto>.Success(mappedDto, "Vehicle created successfully");
    }

    public async Task<Result> UpdateAsync(int id, UpdateVehicleDto dto)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);
        if (entity == null)
        {
            return Result.Fail(new List<string> { "Vehicle not found" });
        }

        entity.PlateNumber = dto.PlateNumber;
        _unitOfWork.Vehicles.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var mappedDto = _mapper.Map<Vehicle, VehicleDto>(entity);
        return Result.Succses($"id : {mappedDto.id} Vehicle updated successfully");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Vehicles.FindAsync(id);
        if (entity == null)
        {
            return Result.Fail(new List<string> { "Vehicle not found" });
        }

        _unitOfWork.Vehicles.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        var mappedDto = _mapper.Map<Vehicle, VehicleDto>(entity);
        return Result.Succses($"plate : {mappedDto.PlateNumber}  Vehicle deleted successfully");
    }
}
