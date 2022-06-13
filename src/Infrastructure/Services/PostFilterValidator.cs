using Common.Services;
using NJsonSchema;
using System.Text;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class PostFilterValidator : Singleton, IPostFilterValidator
{
    public async Task<bool> ValidateFilterAsync(PostFilter filter, PostContent content) 
        => filter switch
    {
        JsonSchemaPostFilter f1 => await ValidateJsonSchemaFilter(f1, content),
        _ => throw new InvalidOperationException($"Unknown PostFilter Type: {filter.Type}")
    };

    private static async Task<bool> ValidateJsonSchemaFilter(JsonSchemaPostFilter filter, PostContent content)
    {
        var schema = await JsonSchema.FromJsonAsync(filter.Schema);
        
        if (!content.Headers.TryGetValue("Content-Type", out string[]? contentType) || !contentType.Contains("application/json"))
        {
            return false;
        }

        string json = Encoding.UTF8.GetString(content.Payload);
        return !schema.Validate(json).Any();
    }
}
