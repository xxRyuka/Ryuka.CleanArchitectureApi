using Ryuka.NlayerApi.Application.Dto.VehicleDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface IVehicleService
{
    Task<VehicleDto> GetByIdAsync(int id);
    Task<IEnumerable<VehicleDto>> GetAllAsync();
    Task<VehicleDto> CreateAsync(CreateVehicleDto dto);
    Task<bool> UpdateAsync(int id, UpdateVehicleDto dto);
    Task<bool> DeleteAsync(int id);
}
