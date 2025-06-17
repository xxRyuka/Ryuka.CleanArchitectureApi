using AutoMapper;
using Ryuka.NlayerApi.Application.Dto;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Mapping;

public class ParkingRecordProfile : Profile
{
    public ParkingRecordProfile()
    {
        CreateMap<ParkingRecord, ParkingRecordDto>()
            .ForMember(pr => pr.VehicleId, opt => opt.MapFrom(src => src.VehicleId))
            .ForMember(pr => pr.SlotId, opt => opt.MapFrom(src => src.SlotId))
            .ForMember(pr => pr.Vehicle, opt => opt.MapFrom(src => src.Vehicle))
            .ForMember(pr => pr.Slot, opt => opt.MapFrom(src => src.Slot))
            .ReverseMap();

    }
}