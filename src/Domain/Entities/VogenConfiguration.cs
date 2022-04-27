using Vogen;
using WeShare.Domain.Entities;

[assembly: VogenDefaults(
    conversions: Conversions.EfCoreValueConverter | Conversions.SystemTextJson | Conversions.TypeConverter,
    throws: typeof(DomainValidationException)
)]

