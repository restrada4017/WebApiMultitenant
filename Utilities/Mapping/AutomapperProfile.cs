using AutoMapper;

namespace Utilities
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ProfileUser, ProfileUserDto>().ReverseMap();
        }
    }
}