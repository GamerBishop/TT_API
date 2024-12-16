using Application.Projects.DTOs;
using Domain.Model;
using AutoMapper;

namespace Application.Projects.Profiles;

public class ProjectProfiles : Profile
{
    public ProjectProfiles()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
    }
}
