using Microsoft.EntityFrameworkCore;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Infrastructure.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<ParkingRecord> Records { get; set; }

    public ApiDbContext(DbContextOptions opt) :  base(opt)
    {
        
    }
}   