using AutoMapper;
using PlatformService.Models;

public class PlatformProfile : Profile {
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>()
            .ForMember(dest => dest.Event, opt => opt.MapFrom(src => "DefaultEvent"));
    }
}