using Application.Projects.DTOs;
using Domain.Model;
using AutoMapper;

namespace Application.Projects.Profiles;

public class ProjectProfiles : Profile
{
    public ProjectProfiles()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new User { Id = src.CreatedBy }));
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
    }
}
