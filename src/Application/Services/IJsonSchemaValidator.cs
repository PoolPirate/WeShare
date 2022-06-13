using Common.Services;

namespace WeShare.Application.Services;
public interface IJsonSchemaValidator : IService
{
    public Task<bool> IsValidJsonSchema(string schema, CancellationToken cancellationToken);
}
