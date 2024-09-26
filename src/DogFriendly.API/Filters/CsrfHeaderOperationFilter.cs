using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DogFriendly.API.Filters
{
    /// <summary>
    /// CSRF Header Operation Filter.
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    public class CsrfHeaderOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-CSRF-TOKEN",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "CSRF token"
            });
        }
    }
}
