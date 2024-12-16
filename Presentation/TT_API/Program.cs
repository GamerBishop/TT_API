using Application.Extensions;
using Domain.Model;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Serilog;
using TT_API.Extensions;
using TT_API.Middlewares;

namespace TT_API;

 public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddPresentation();

        // Add services to the container.
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication(builder.Configuration);

        builder.Services.AddControllers();

        // Register data protection services
        builder.Services.AddDataProtection();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton(TimeProvider.System);

        var app = builder.Build();

        // Seed the database
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IProjectSeeder>();
        seeder.Seed();

        // Configure the HTTP request pipeline.
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGroup("api/Identity")
            .WithTags("Identity")
            .MapIdentityApi<User>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
