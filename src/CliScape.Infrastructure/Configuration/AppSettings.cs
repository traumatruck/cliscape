namespace CliScape.Infrastructure.Configuration;

/// <summary>
///     Root configuration object for the application.
///     Aggregates all configuration sections.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    ///     Settings for game state persistence.
    /// </summary>
    public required PersistenceSettings Persistence { get; init; }

    /// <summary>
    ///     Creates the default application settings.
    /// </summary>
    public static AppSettings CreateDefault()
    {
        return new AppSettings
        {
            Persistence = PersistenceSettings.CreateDefault()
        };
    }
}