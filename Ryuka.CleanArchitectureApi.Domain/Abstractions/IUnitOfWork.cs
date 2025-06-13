namespace Ryuka.NlayerApi.Core.Abstractions;

public interface IUnitOfWork : IDisposable
{
    
    // Transaction kuralım
    Task BeginTransaction(); // isimlerini async yap 
    Task CommitTransaction();
    Task RollbackTransaction();
    
    
    IVehicleRepository Vehicles { get; }
    ISlotRepository Slots { get; }
    IParkingRecordRepository ParkingRecords { get; }

    // 3 vehicle eklersen → return 3
    // 2 parking record güncellemesen → return 2  
    // Hiçbir değişiklik yoksa → return 0
    
    // savechanges return tipinin int olma sebebi 
    Task<int> SaveChangesAsync();
    int SaveChanges();
}