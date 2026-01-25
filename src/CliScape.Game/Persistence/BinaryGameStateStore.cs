using System.Text;

namespace CliScape.Game.Persistence;

/// <summary>
/// Implements game state persistence using a binary file format.
/// This store handles serialization and deserialization of game data.
/// </summary>
/// <param name="path">The file path where the save file will be stored.</param>
public sealed class BinaryGameStateStore(string path) : IGameStateStore
{
    /// <summary>
    /// UTF-8 encoding without BOM that throws on invalid bytes.
    /// </summary>
    private static readonly Encoding TextEncoding =
        new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>
    /// The underlying binary file handler for reading and writing save data.
    /// </summary>
    private readonly BinarySaveFile _file = new(path);

    /// <summary>
    /// Loads the player's saved state from the binary save file.
    /// </summary>
    /// <returns>
    /// A <see cref="PlayerSnapshot"/> if save data exists, otherwise <c>null</c>.
    /// </returns>
    public PlayerSnapshot? LoadPlayer()
    {
        // Read the player section from the save file
        var data = _file.ReadSection((int)SaveSection.Player);
        
        if (data == null)
        {
            return null;
        }

        using var stream = new MemoryStream(data, writable: false);
        using var reader = new BinaryReader(stream, TextEncoding, leaveOpen: false);
        
        // Deserialize player data in the order it was written
        var id = reader.ReadInt32();
        var name = ReadString(reader);
        var locationName = ReadString(reader);
        var currentHealth = reader.ReadInt32();
        var maxHealth = reader.ReadInt32();
        
        return new PlayerSnapshot(id, name, locationName, currentHealth, maxHealth);
    }

    /// <summary>
    /// Saves the player's current state to the binary save file.
    /// </summary>
    /// <param name="snapshot">The player state to persist.</param>
    public void SavePlayer(PlayerSnapshot snapshot)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, TextEncoding, leaveOpen: true);
        
        // Serialize player data in a specific order
        writer.Write(snapshot.Id);
        WriteString(writer, snapshot.Name);
        WriteString(writer, snapshot.LocationName);
        writer.Write(snapshot.CurrentHealth);
        writer.Write(snapshot.MaxHealth);
        writer.Flush();
        
        // Write the serialized data to the save file
        _file.WriteSection((int)SaveSection.Player, stream.ToArray());
    }

    /// <summary>
    /// Reads a length-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">The binary reader to read from.</param>
    /// <returns>The deserialized string.</returns>
    /// <exception cref="InvalidDataException">Thrown when the string length is negative.</exception>
    /// <exception cref="EndOfStreamException">Thrown when the stream ends before the expected string length.</exception>
    private static string ReadString(BinaryReader reader)
    {
        // Read the length prefix (4 bytes)
        var length = reader.ReadInt32();
        
        if (length < 0)
        {
            throw new InvalidDataException("String length is invalid.");
        }

        var bytes = reader.ReadBytes(length);

        // Verify we read the expected number of bytes
        return bytes.Length == length
            ? TextEncoding.GetString(bytes)
            : throw new EndOfStreamException("Unexpected end of data while reading a string.");
    }

    /// <summary>
    /// Writes a string to a binary writer with a length prefix.
    /// The format is: [4-byte length][UTF-8 bytes].
    /// </summary>
    /// <param name="writer">The binary writer to write to.</param>
    /// <param name="value">The string to serialize.</param>
    private static void WriteString(BinaryWriter writer, string value)
    {
        var bytes = TextEncoding.GetBytes(value);
        writer.Write(bytes.Length);  // Write length prefix
        writer.Write(bytes);          // Write UTF-8 encoded bytes
    }
}
