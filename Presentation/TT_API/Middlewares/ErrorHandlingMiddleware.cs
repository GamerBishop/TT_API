using Domain.Exceptions;
using System.Net;
using System.Security.Principal;

namespace TT_API.Middlewares;
public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(IdentityException identity)
        {
            logger.LogError(identity, "An unhandled exception has occured");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(identity.Message);

        }
        catch (NotFoundException notFound)
        {
            logger.LogError(notFound, "An unhandled exception has occurred");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (ForbidException forbid)
        {
            logger.LogError(forbid, "Access forbidden");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(forbid.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}