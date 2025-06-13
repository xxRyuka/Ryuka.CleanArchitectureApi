using Ryuka.NlayerApi.Application.Dto;
using static Ryuka.NlayerApi.Application.Dto.ParkingRecordDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface IParkingRecordService
{
    Task<string> CreateAsync(CreateParkingRecordDto dto);
    Task<List<ParkingRecordDto>> GetAllAsync();
    Task<ParkingRecordDto> GetByIdAsync(int id);
    Task<string> ExitAsync(string plate);




    //---------
    // Task<ParkingRecordDto> GetByIdAsync(int id);
    // Task<IEnumerable<ParkingRecordDto>> GetAllAsync();
    // // Task<bool> UpdateAsync(int id, UpdateParkingRecordDto dto);
    // Task<bool> DeleteAsync(int id);
}