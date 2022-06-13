using Common.Services;
using NJsonSchema;
using WeShare.Application.Services;

namespace WeShare.Infrastructure.Services;
public class JsonSchemaValidator : Singleton, IJsonSchemaValidator
{
    public async Task<bool> IsValidJsonSchema(string schema, CancellationToken cancellationToken)
    {
        try
        {
            await JsonSchema.FromJsonAsync(schema, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}