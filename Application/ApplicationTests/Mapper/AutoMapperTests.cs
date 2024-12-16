using AutoMapper;
using Application.Users.DTOs;
using Domain.Model;
using Xunit;
using Application.Users.Profiles;
using Assert = Xunit.Assert;

namespace AutoMapperTests;

public class AutoMapperTests
{
    private readonly IMapper _mapper;

    public AutoMapperTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserProfiles>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void UserToUserDtoMapping_IsValid()
    {
        var user = new User
        {
            UserName = "testuser",
            Email = "test@example.com",
            Roles = new List<Role> { new Role { Name = "Admin" } }
        };

        var userDto = _mapper.Map<UserDto>(user);

        Assert.Equal(user.UserName, userDto.UserName);
        Assert.Equal(user.Email, userDto.Email);
        Assert.Equal(user.Roles.Select(r => r.Name), userDto.Roles);
    }
}
