namespace PeakLims.Configurations;

public class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public const string PeakLimsKey = nameof(PeakLims);

    public string PeakLims { get; set; } = String.Empty;
}

public static class ConnectionStringOptionsExtensions
{
    public static ConnectionStringOptions GetConnectionStringOptions(this IConfiguration configuration)
        => configuration.GetSection(ConnectionStringOptions.SectionName).Get<ConnectionStringOptions>();
}