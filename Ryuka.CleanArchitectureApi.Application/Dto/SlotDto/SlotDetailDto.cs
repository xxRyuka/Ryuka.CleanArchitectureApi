namespace Ryuka.NlayerApi.Application.Dto.SlotDto;

public class SlotDetailDto
{
    public int id { get; set; }
    public bool isOccupied { get; set; }
    public IReadOnlyCollection<ParkingRecordDto> ParkingRecords { get; set; } = new List<ParkingRecordDto>();
}