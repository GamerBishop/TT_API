using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TT_API.Middlewares.Swagger;
public class SwaggerResponseStatusCodeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.TryGetValue("404", out OpenApiResponse? value))
        {
            value.Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Example = new OpenApiString("{ \"message\": \"?????????? with id XXxxxxxxxxxxxxxxXX doesn't exist.\" }")
                }
            };
        }
    }
}
