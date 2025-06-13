namespace Ryuka.NlayerApi.Core.Entities;

public class ParkingRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int SlotId { get; set; }

    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }

    public decimal Price { get; set; }
    // giriş - çıkıs saatine gore fiyat cekeriz 
    // ek olarak slot type ekleyince fiyat parametremiz -> 
    // (çıkıs date - giriş date )*slotType  
    
    // Navigation Props
    public Vehicle Vehicle { get; set; }
    public Slot Slot { get; set; }

}
