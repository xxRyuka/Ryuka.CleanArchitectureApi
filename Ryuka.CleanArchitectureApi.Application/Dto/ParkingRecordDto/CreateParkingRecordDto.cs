namespace Ryuka.NlayerApi.Application.Dto;

public class CreateParkingRecordDto
{
    public string VehiclePlate { get; set; }
    public int? SlotDtoId { get; set; } // Slot tercihi varsa slota yoksa boş slota aticaz 
}