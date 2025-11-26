using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace FoodBev.API.Configurations
{
    /// <summary>
    /// Parameter filter to exclude IFormFile parameters from Swagger parameter generation.
    /// These will be handled by the FileUploadOperationFilter instead.
    /// </summary>
    public class FileUploadParameterFilter : IParameterFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiParameter parameter, ParameterFilterContext context)
        {
            // If this is an IFormFile parameter, we'll handle it in the operation filter
            // This filter prevents Swagger from trying to generate a schema for it
            if (context.ParameterInfo?.ParameterType == typeof(IFormFile) ||
                context.ParameterInfo?.ParameterType == typeof(IFormFileCollection))
            {
                // Don't generate a parameter schema - it will be in RequestBody instead
                // This prevents the SwaggerGeneratorException
            }
        }
    }
}

