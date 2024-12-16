using Application.Projects.Profiles;
using Application.Services;
using Application.Services.AppServices;
using Application.Settings;
using Application.Users.Profiles;
using AutoMapper;
using AutoMapper.Internal;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServicesCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        System.Reflection.Assembly applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.AddTransient<IEmailSender<User>, EmailSender>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();

        services.AddAutoMapper(typeof(UserProfiles), typeof(ProjectProfiles));


        // Diagnostic pour vérifier les profils AutoMapper
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(applicationAssembly);
        });

        var mapper = mapperConfiguration.CreateMapper();
        //var profiles = mappe
        
       // Console.WriteLine($"Nombre de profils chargés : {profiles.Length}");

    }
}
