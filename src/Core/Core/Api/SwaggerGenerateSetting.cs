namespace Core.Api;

public record SwaggerGenerateSetting(
    int Version,
    string Title,
    string Description = ""
);
