using StronglyTypedIds;

[assembly: StronglyTypedIdDefaults(
    backingType: StronglyTypedIdBackingType.Long,
    converters: StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter
)] 