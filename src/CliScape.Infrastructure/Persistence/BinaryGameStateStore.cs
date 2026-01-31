using System.Text;
using CliScape.Game.Persistence;
using CliScape.Infrastructure.Configuration;

namespace CliScape.Infrastructure.Persistence;

/// <summary>
///     Implements game state persistence using a binary file format.
///     This store handles serialization and deserialization of game data.
/// </summary>
public sealed class BinaryGameStateStore : IGameStateStore
{
    /// <summary>
    ///     UTF-8 encoding without BOM that throws on invalid bytes.
    /// </summary>
    private static readonly Encoding TextEncoding =
        new UTF8Encoding(false, true);

    /// <summary>
    ///     The underlying binary file handler for reading and writing save data.
    /// </summary>
    private readonly BinarySaveFile _file;

    /// <summary>
    ///     Creates a new instance of the binary game state store.
    /// </summary>
    /// <param name="settings">The persistence settings containing the save file path.</param>
    public BinaryGameStateStore(PersistenceSettings settings)
    {
        Directory.CreateDirectory(settings.SaveDirectory);
        _file = new BinarySaveFile(settings.SaveFilePath);
    }

    /// <summary>
    ///     Loads the player's saved state from the binary save file.
    /// </summary>
    /// <returns>
    ///     A <see cref="PlayerSnapshot" /> if save data exists, otherwise <c>null</c>.
    /// </returns>
    public PlayerSnapshot? LoadPlayer()
    {
        // Read the player section from the save file
        var data = _file.ReadSection((int)SaveSection.Player);

        if (data == null)
        {
            return null;
        }

        using var stream = new MemoryStream(data, false);
        using var reader = new BinaryReader(stream, TextEncoding, false);

        // Deserialize player data in the order it was written
        var id = reader.ReadInt32();
        var name = ReadString(reader);
        var locationName = ReadString(reader);
        var currentHealth = reader.ReadInt32();
        var maxHealth = reader.ReadInt32();

        // Deserialize skills
        var skillCount = reader.ReadInt32();
        var skills = new SkillSnapshot[skillCount];

        for (var i = 0; i < skillCount; i++)
        {
            var skillName = ReadString(reader);
            var experience = reader.ReadInt32();
            skills[i] = new SkillSnapshot(skillName, experience);
        }

        // Deserialize inventory and equipment
        InventorySlotSnapshot[]? inventorySlots = null;
        EquippedItemSnapshot[]? equippedItems = null;
        SlayerTaskSnapshot? slayerTask = null;

        if (stream.Position < stream.Length)
        {
            // Deserialize inventory
            var inventoryCount = reader.ReadInt32();
            inventorySlots = new InventorySlotSnapshot[inventoryCount];
            
            for (var i = 0; i < inventoryCount; i++)
            {
                var slotIndex = reader.ReadInt32();
                var itemId = reader.ReadInt32();
                var quantity = reader.ReadInt32();
                inventorySlots[i] = new InventorySlotSnapshot(slotIndex, itemId, quantity);
            }

            // Deserialize equipment
            var equippedCount = reader.ReadInt32();
            equippedItems = new EquippedItemSnapshot[equippedCount];
            
            for (var i = 0; i < equippedCount; i++)
            {
                var slot = reader.ReadInt32();
                var itemId = reader.ReadInt32();
                equippedItems[i] = new EquippedItemSnapshot(slot, itemId);
            }

            // Deserialize slayer task (if present)
            if (stream.Position < stream.Length)
            {
                var hasSlayerTask = reader.ReadBoolean();
                if (hasSlayerTask)
                {
                    var category = ReadString(reader);
                    var remainingKills = reader.ReadInt32();
                    var totalKills = reader.ReadInt32();
                    var slayerMaster = ReadString(reader);
                    slayerTask = new SlayerTaskSnapshot(category, remainingKills, totalKills, slayerMaster);
                }
            }
        }

        return new PlayerSnapshot(id, name, locationName, currentHealth, maxHealth, skills, inventorySlots,
            equippedItems, slayerTask);
    }

    /// <summary>
    ///     Saves the player's current state to the binary save file.
    /// </summary>
    /// <param name="snapshot">The player state to persist.</param>
    public void SavePlayer(PlayerSnapshot snapshot)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, TextEncoding, true);

        // Serialize player data in a specific order
        writer.Write(snapshot.Id);
        WriteString(writer, snapshot.Name);
        WriteString(writer, snapshot.LocationName);
        writer.Write(snapshot.CurrentHealth);
        writer.Write(snapshot.MaximumHealth);

        // Serialize skills
        writer.Write(snapshot.Skills.Length);

        foreach (var skill in snapshot.Skills)
        {
            WriteString(writer, skill.Name);
            writer.Write(skill.Experience);
        }

        // Serialize inventory (gold is now stored as Coins item in inventory)
        var inventorySlots = snapshot.InventorySlots ?? [];
        writer.Write(inventorySlots.Length);
        
        foreach (var slot in inventorySlots)
        {
            writer.Write(slot.SlotIndex);
            writer.Write(slot.ItemId);
            writer.Write(slot.Quantity);
        }

        // Serialize equipment
        var equippedItems = snapshot.EquippedItems ?? [];
        writer.Write(equippedItems.Length);
        
        foreach (var equipped in equippedItems)
        {
            writer.Write(equipped.Slot);
            writer.Write(equipped.ItemId);
        }

        // Serialize slayer task
        var hasSlayerTask = snapshot.SlayerTask.HasValue;
        writer.Write(hasSlayerTask);
        if (hasSlayerTask)
        {
            var task = snapshot.SlayerTask!.Value;
            WriteString(writer, task.Category);
            writer.Write(task.RemainingKills);
            writer.Write(task.TotalKills);
            WriteString(writer, task.SlayerMaster);
        }

        writer.Flush();

        // Write the serialized data to the save file
        _file.WriteSection((int)SaveSection.Player, stream.ToArray());
    }

    /// <summary>
    ///     Reads a length-prefixed string from a binary reader.
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
    ///     Writes a string to a binary writer with a length prefix.
    ///     The format is: [4-byte length][UTF-8 bytes].
    /// </summary>
    /// <param name="writer">The binary writer to write to.</param>
    /// <param name="value">The string to serialize.</param>
    private static void WriteString(BinaryWriter writer, string value)
    {
        var bytes = TextEncoding.GetBytes(value);
        writer.Write(bytes.Length); // Write length prefix
        writer.Write(bytes); // Write UTF-8 encoded bytes
    }
}