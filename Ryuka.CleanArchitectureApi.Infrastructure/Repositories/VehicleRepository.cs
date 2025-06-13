using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;
using Ryuka.NlayerApi.Infrastructure.Data;

namespace Ryuka.NlayerApi.Infrastructure.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    // miras almak ve inheritance farklı seylerdir tamam mi repository ozelliklerini tasıması için Repository<T> sinifini miras aliyor
    // fakat IVehicleRepository interfacesi ile interfaceyi implement ediyoruz
    
    public VehicleRepository(ApiDbContext context) : base(context)
    {
    }
}