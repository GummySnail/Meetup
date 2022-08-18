using AutoMapper;
using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;

namespace Meetup.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ApplicationUserRefreshTokens, RefreshToken>()
            .ForMember(x => x.RefreshTokenValue, opt => opt.MapFrom(x => x.RefreshToken));
    }
}