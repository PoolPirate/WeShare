using System.ComponentModel.DataAnnotations;

namespace WeShare.Application.Common.Exceptions;
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationResult> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.MemberNames.SingleOrDefault() ?? String.Empty, e => e.ErrorMessage ?? String.Empty)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
