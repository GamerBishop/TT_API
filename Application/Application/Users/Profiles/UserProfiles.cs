using Application.Users.DTOs;
using AutoMapper;
using Domain.Model;

namespace Application.Users.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    { 
        CreateMap<RegisterUserDto, User>();
        CreateMap<LoginUserDto, User>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
            .ReverseMap();
    }
}
