namespace WebApplication.Options;

public sealed record CorsOptions
{
    public const string SectionName = "Cors";

    public IReadOnlyList<string> AllowedOrigins { get; init; } = [];
}
