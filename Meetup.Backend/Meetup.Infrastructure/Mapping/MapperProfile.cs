using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;

namespace Meetup.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ApplicationUserRefreshTokens, RefreshToken>()
            .ForMember(x => x.RefreshTokenValue, opt => opt.MapFrom(x => x.RefreshToken));

        CreateMap<TagDto, Tag>()
            .ForMember(t => t.Id, opt => opt.Ignore());
        CreateMap<Tag, TagDto>();
    }
}