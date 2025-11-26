using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace FoodBev.API.Configurations
{
    /// <summary>
    /// Schema filter to map IFormFile to binary format in Swagger.
    /// </summary>
    public class FileUploadSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(IFormFile) || context.Type == typeof(IFormFileCollection))
            {
                schema.Type = "string";
                schema.Format = "binary";
                schema.Description = "File to upload";
            }
        }
    }
}

