namespace Ryuka.NlayerApi.Application.Dto.VehicleDto;

public class VehicleDetailDto
{
    public int id { get; set; }
    public string PlateNumber { get; set; }

    public IReadOnlyCollection<ParkingRecordDto> ParkingRecords { get; set; }
}