namespace WebApplication.Options;

public sealed record PersonStoreOptions
{
    public const string SectionName = "PersonStore";

    public string FilePath { get; init; } = "people.json";
}
