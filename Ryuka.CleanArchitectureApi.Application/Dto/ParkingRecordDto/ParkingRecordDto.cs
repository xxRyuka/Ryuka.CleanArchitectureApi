using Microsoft.EntityFrameworkCore.Metadata;

namespace Ryuka.NlayerApi.Application.Dto;

public class ParkingRecordDto
{
    public int id { get; set; }
    public int? VehicleId { get; set; }
    public int? SlotId { get; set; }

    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }

    public decimal Price { get; set; }
    
    // Navigation Props
    public VehicleDto.VehicleDto? Vehicle { get; set; }
    public SlotDto.SlotDto? Slot { get; set; }

}