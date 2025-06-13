using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;
using Ryuka.NlayerApi.Infrastructure.Data;

namespace Ryuka.NlayerApi.Infrastructure.Repositories;

public class ParkingRecordRepository :Repository<ParkingRecord>, IParkingRecordRepository
{
    
    public ParkingRecordRepository(ApiDbContext context) : base(context)
    {
    }


}