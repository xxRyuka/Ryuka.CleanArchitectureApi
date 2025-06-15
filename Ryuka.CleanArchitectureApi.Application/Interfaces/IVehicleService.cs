using Ryuka.NlayerApi.Application.Common.Concrete;
using Ryuka.NlayerApi.Application.Dto.VehicleDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface IVehicleService
{
    Task<Result<VehicleDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<VehicleDto>>> GetAllAsync();
    Task<Result<VehicleDto>> CreateAsync(CreateVehicleDto dto);
    Task<Result> UpdateAsync(int id, UpdateVehicleDto dto);
    Task<Result> DeleteAsync(int id);
}