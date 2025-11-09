using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class FlattenAllOfDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths.Values)
            foreach (var op in path.Operations.Values)
            {
                if (op.RequestBody == null)
                    continue;
                foreach (var media in op.RequestBody.Content.Values)
                {
                    var schema = media.Schema;
                    if (schema?.AllOf?.Count == 1 && schema.AllOf[0].Reference != null)
                    {
                        media.Schema = schema.AllOf[0];
                    }
                }
            }
    }
}
