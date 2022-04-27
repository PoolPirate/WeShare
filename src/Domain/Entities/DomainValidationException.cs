using System.ComponentModel.DataAnnotations;

namespace WeShare.Domain.Entities;
public class DomainValidationException : ValidationException
{
    public DomainValidationException(string? message) : base(message)
    {
    }
}

