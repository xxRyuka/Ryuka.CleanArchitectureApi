using System.Security.AccessControl;

namespace Ryuka.NlayerApi.Core.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public string PlateNumber { get; set; }
    
    public ICollection<ParkingRecord> ParkingRecords { get; set; }
}