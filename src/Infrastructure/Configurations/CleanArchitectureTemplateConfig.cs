namespace CleanArchitectureTemplate.Infrastructure.Configurations;

public class CleanArchitectureTemplateConfig
{
    public static string ConfigName => "CleanArchitectureTemplate";

    public bool UseInMemoryDatabase { get; set; }

    public ConnectionStringsConfig ConnectionStrings { get; set; } = null!;

    public JwtConfig Jwt { get; set; } = null!;

    public LoggingConfig Logging { get; set; } = null!;
}
