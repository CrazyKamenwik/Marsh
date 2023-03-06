using Microsoft.OpenApi.Models;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using Swashbuckle.AspNetCore.SwaggerGen;
using NJsonSchema;
using NSwag;
using OpenApiParameter = NSwag.OpenApiParameter;

namespace TS.MailService.Application.SwaggerOperationProcessors;

public class AddRequiredHeaderParameter : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        context.OperationDescription.Operation.Parameters.Add(
        new OpenApiParameter()
        {
            Name = "x-api-key",
            Kind = OpenApiParameterKind.Header,
            Schema = new JsonSchema { Type = JsonObjectType.String },
            IsRequired = true,
            Description = "Api key for access to endpoint"
        });

        return true;
    }
}