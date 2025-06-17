using AutoMapper;
using Ryuka.NlayerApi.Application.Dto.SlotDto;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Mapping;

public class SlotProfile : Profile
{
    public SlotProfile()
    {
        CreateMap<Slot, SlotDto>().ReverseMap();
        CreateMap<CreateSlotDto, Slot>();
        CreateMap<UpdateSlotDto, Slot>();
    }
}