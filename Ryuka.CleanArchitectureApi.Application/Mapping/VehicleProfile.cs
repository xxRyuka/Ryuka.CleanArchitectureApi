using AutoMapper;
using Ryuka.NlayerApi.Application.Dto.VehicleDto;
using Ryuka.NlayerApi.Core.Entities;

namespace Ryuka.NlayerApi.Application.Mapping;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        // CreateMap<Source,Destination>
        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<VehicleDto, Vehicle>().ReverseMap();
    }
}