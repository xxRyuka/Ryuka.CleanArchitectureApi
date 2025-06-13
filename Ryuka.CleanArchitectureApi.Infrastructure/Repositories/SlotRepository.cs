using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Core.Entities;
using Ryuka.NlayerApi.Infrastructure.Data;

namespace Ryuka.NlayerApi.Infrastructure.Repositories;

public class SlotRepository : Repository<Slot>, ISlotRepository
{
    public SlotRepository(ApiDbContext context) : base(context)
    {
    }
}