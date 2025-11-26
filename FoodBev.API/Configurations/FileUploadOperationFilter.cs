using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace FoodBev.API.Configurations
{
    /// <summary>
    /// Swagger operation filter to properly handle file uploads in Swagger UI.
    /// This enables the file upload button for endpoints that accept IFormFile.
    /// </summary>
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if any parameters are IFormFile
            var fileParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Type == typeof(IFormFile) || 
                           p.Type == typeof(IFormFileCollection) ||
                           (p.Type != null && p.Type.IsGenericType && 
                            p.Type.GetGenericTypeDefinition() == typeof(List<>) &&
                            p.Type.GetGenericArguments()[0] == typeof(IFormFile)))
                .ToList();

            if (fileParameters.Any())
            {
                // Remove IFormFile parameters from the parameters list
                var parametersToRemove = fileParameters.Select(p => p.Name).ToHashSet();
                operation.Parameters = operation.Parameters?
                    .Where(p => !parametersToRemove.Contains(p.Name))
                    .ToList() ?? new List<OpenApiParameter>();

                // Create or update RequestBody for multipart/form-data
                if (operation.RequestBody == null)
                {
                    operation.RequestBody = new OpenApiRequestBody();
                }

                if (operation.RequestBody.Content == null)
                {
                    operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
                }

                // Add or update multipart/form-data content
                var formDataContent = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = fileParameters.ToDictionary(
                            p => p.Name ?? "file",
                            p => new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                                Description = "File to upload"
                            }
                        ),
                        Required = fileParameters
                            .Where(p => p.IsRequired)
                            .Select(p => p.Name ?? "file")
                            .ToHashSet()
                    }
                };

                operation.RequestBody.Content["multipart/form-data"] = formDataContent;
                operation.RequestBody.Required = fileParameters.Any(p => p.IsRequired);
            }
        }
    }
}

