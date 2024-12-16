using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TT_API.Middlewares;
using TT_API.Middlewares.Swagger;

namespace TT_API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.Name = "YourAppCookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                            },
                            new string[] {}
                        }
                });



                // Chemin du fichier de commentaires XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // Ajouter le filtre pour les exemples de réponses
                c.OperationFilter<SwaggerResponseStatusCodeFilter>();

            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
