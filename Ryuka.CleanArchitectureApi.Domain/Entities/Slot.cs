using System.Security.AccessControl;
using Ryuka.NlayerApi.Core.Enums;

namespace Ryuka.NlayerApi.Core.Entities;

public class Slot
{
    public int Id { get; set; }
    
    public bool isOccupied { get; set; } // Dolu mu değil mi anlamak için 
    // bir enum ile slot tipi ekleyebiliriz sot tipine gorede fiyat cekilir 
    
    
    public ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
}