using Ryuka.NlayerApi.Application.Common.Concrete;
using Ryuka.NlayerApi.Application.Dto;
using static Ryuka.NlayerApi.Application.Dto.ParkingRecordDto;

namespace Ryuka.NlayerApi.Application.Interfaces;

public interface IParkingRecordService
{
    Task<Result<ParkingRecordDto>> CreateAsync(CreateParkingRecordDto dto);
    Task<Result<IEnumerable<ParkingRecordDto>>> GetAllAsync();
    Task<Result> ExitAsync(string plate);
    Task<Result<ParkingRecordDto>> GetByIdAsync(int id);
    Task<Result<ParkingRecordDto>> GetActiveByVehiclePlateAsync(string plate);
    Task<Result<IEnumerable<ParkingRecordDto>>> GetHistoryByPlateAsync(string plate);
    Task<Result> GetOccupiedSlotCountAsync();





    //---------
    // Task<ParkingRecordDto> GetByIdAsync(int id);
    // Task<IEnumerable<ParkingRecordDto>> GetAllAsync();
    // // Task<bool> UpdateAsync(int id, UpdateParkingRecordDto dto);
    // Task<bool> DeleteAsync(int id);
}