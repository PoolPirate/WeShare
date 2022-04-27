using MediatR;
using System.ComponentModel.DataAnnotations;
using ValidationException = WeShare.Application.Common.Exceptions.ValidationException;

namespace WeShare.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public ValidationBehaviour()
    {
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        return !Validator.TryValidateObject(request, context, results)
            ? throw new ValidationException(results)
            : await next();
    }
}
