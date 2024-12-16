using Domain.Model;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString;
        if(IsRunningInDocker())
           connectionString = configuration.GetConnectionString("DockerTest")!;
        else
            connectionString = configuration.GetConnectionString("TTWADB")!;
        services.AddDbContext<ProjectsDbContext>(options => options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()); 



        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectsRepository, ProjectRepository>();

        // Apply migrations at startup only if running in Docker
        if (IsRunningInDocker())
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var context = serviceProvider.GetRequiredService<ProjectsDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        services.AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ProjectsDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager()
            .AddUserManager<UserManager<User>>();
    }

    private static bool IsRunningInDocker()
    {
        return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    }
}
