namespace CliScape.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for application persistence.
/// </summary>
public sealed class PersistenceSettings
{
    /// <summary>
    /// The directory where save files are stored.
    /// </summary>
    public required string SaveDirectory { get; init; }

    /// <summary>
    /// The filename for the save file.
    /// </summary>
    public string SaveFileName { get; init; } = "save.bin";

    /// <summary>
    /// Gets the full path to the save file.
    /// </summary>
    public string SaveFilePath => Path.Combine(SaveDirectory, SaveFileName);

    /// <summary>
    /// Creates the default persistence settings using the user's home directory.
    /// This avoids issues with containerized environments (e.g., snap) that override XDG paths.
    /// </summary>
    public static PersistenceSettings CreateDefault()
    {
        // Use HOME directly to avoid snap/container path redirection
        var home = Environment.GetEnvironmentVariable("HOME") 
                   ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        var saveDirectory = Path.Combine(home, ".local", "share", "CliScape");
        
        return new PersistenceSettings
        {
            SaveDirectory = saveDirectory
        };
    }
}
