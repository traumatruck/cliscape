using System.Text;

namespace CliScape.Infrastructure.Persistence;

/// <summary>
///     Manages a binary save file with a custom format consisting of a header and multiple sections.
///     The file format uses a 4KB header containing metadata and a section index, followed by section data.
/// </summary>
/// <param name="path">The file system path to the save file.</param>
internal sealed class BinarySaveFile(string path)
{
    /// <summary>
    ///     Magic number ("CLSC") used to identify valid save files.
    /// </summary>
    private const uint Magic = 0x43534C43;

    /// <summary>
    ///     The current version of the save file format.
    /// </summary>
    private const ushort Version = 1;

    /// <summary>
    ///     Total size of the file header in bytes (4KB).
    ///     The header contains the magic number, version, section count, and section index.
    /// </summary>
    private const int HeaderSize = 4096;

    /// <summary>
    ///     Size of the fixed portion of the header (magic + version + count).
    /// </summary>
    private const int HeaderFixedSize = sizeof(uint) + sizeof(ushort) + sizeof(ushort);

    /// <summary>
    ///     Size of each section entry in the index (id + offset + length).
    /// </summary>
    private const int SectionEntrySize = sizeof(int) + sizeof(long) + sizeof(int);

    /// <summary>
    ///     Gets the maximum number of sections that can fit in the header.
    /// </summary>
    private static int MaxSections => (HeaderSize - HeaderFixedSize) / SectionEntrySize;

    /// <summary>
    ///     Reads the data from a specific section in the save file.
    /// </summary>
    /// <param name="id">The unique identifier of the section to read.</param>
    /// <returns>
    ///     The section data as a byte array, or <c>null</c> if the section doesn't exist.
    /// </returns>
    /// <exception cref="InvalidDataException">Thrown when the section offset or length is invalid.</exception>
    /// <exception cref="EndOfStreamException">Thrown when the file ends before reading the complete section.</exception>
    public byte[]? ReadSection(int id)
    {
        using var stream = OpenFile();
        var index = ReadIndex(stream);

        // Check if the requested section exists
        if (!index.TryGetValue(id, out var entry))
        {
            return null;
        }

        // Validate that the section is within valid bounds
        if (entry.Offset < HeaderSize || entry.Offset + entry.Length > stream.Length)
        {
            throw new InvalidDataException("Save file section points outside the file.");
        }

        // Read the section data from the file
        var payload = new byte[entry.Length];
        stream.Position = entry.Offset;
        var bytesRead = stream.Read(payload, 0, payload.Length);
        if (bytesRead != payload.Length)
        {
            throw new EndOfStreamException("Unexpected end of data while reading section.");
        }

        return payload;
    }

    /// <summary>
    ///     Writes data to a specific section in the save file.
    ///     If the section already exists and the new data fits, it overwrites the existing data.
    ///     Otherwise, the data is appended to the end of the file.
    /// </summary>
    /// <param name="id">The unique identifier of the section to write.</param>
    /// <param name="payload">The data to write to the section.</param>
    /// <exception cref="InvalidDataException">Thrown when the maximum section count is reached.</exception>
    public void WriteSection(int id, ReadOnlySpan<byte> payload)
    {
        using var stream = OpenFile();
        var index = ReadIndex(stream);

        if (index.TryGetValue(id, out var entry))
        {
            // Section exists - check if new data fits in existing space
            if (payload.Length <= entry.Length)
            {
                // Overwrite existing section in-place
                stream.Position = entry.Offset;
                stream.Write(payload);
                entry = entry with { Length = payload.Length };
            }
            else
            {
                // New data doesn't fit - append to end of file
                entry = new SectionEntry(id, stream.Length, payload.Length);
                stream.Position = entry.Offset;
                stream.Write(payload);
            }

            index[id] = entry;
        }
        else
        {
            // New section - check if we have space in the header
            if (index.Count >= MaxSections)
            {
                throw new InvalidDataException("Save file has reached the maximum section count.");
            }

            // Append new section to end of file
            entry = new SectionEntry(id, stream.Length, payload.Length);
            stream.Position = entry.Offset;
            stream.Write(payload);
            index.Add(id, entry);
        }

        // Update the header with the new section index
        WriteIndex(stream, index);
    }

    /// <summary>
    ///     Opens the save file for reading and writing.
    ///     If the file is new, initializes it with an empty header.
    /// </summary>
    /// <returns>An open file stream positioned at the beginning.</returns>
    /// <exception cref="InvalidDataException">Thrown when the file exists but has an incomplete header.</exception>
    private FileStream OpenFile()
    {
        var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        if (stream.Length == 0)
        {
            // New file - initialize with empty header
            InitializeHeader(stream);
        }
        else if (stream.Length < HeaderSize)
        {
            // File exists but header is corrupted
            stream.Dispose();
            throw new InvalidDataException("Save file header is incomplete.");
        }

        return stream;
    }

    /// <summary>
    ///     Initializes a new save file with an empty header.
    ///     The header contains the magic number, version, and zero sections.
    /// </summary>
    /// <param name="stream">The file stream to initialize.</param>
    private static void InitializeHeader(FileStream stream)
    {
        stream.SetLength(HeaderSize);
        stream.Position = 0;
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(Magic); // Write magic number
        writer.Write(Version); // Write format version
        writer.Write((ushort)0); // Write section count (initially 0)
    }

    /// <summary>
    ///     Reads the section index from the file header.
    ///     The index maps section IDs to their locations and sizes within the file.
    /// </summary>
    /// <param name="stream">The file stream to read from.</param>
    /// <returns>A dictionary mapping section IDs to their entries.</returns>
    /// <exception cref="InvalidDataException">Thrown when the header is invalid or corrupted.</exception>
    private static Dictionary<int, SectionEntry> ReadIndex(FileStream stream)
    {
        stream.Position = 0;
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);

        // Validate magic number
        var magic = reader.ReadUInt32();
        if (magic != Magic)
        {
            throw new InvalidDataException("Save file has an invalid header.");
        }

        // Validate version
        var version = reader.ReadUInt16();
        if (version != Version)
        {
            throw new InvalidDataException($"Save file version {version} is not supported.");
        }

        // Read section count
        var count = reader.ReadUInt16();

        if (count > MaxSections)
        {
            throw new InvalidDataException("Save file has too many sections.");
        }

        var entries = new Dictionary<int, SectionEntry>(count);

        // Read each section entry from the index
        for (var i = 0; i < count; i++)
        {
            var id = reader.ReadInt32();
            var offset = reader.ReadInt64();
            var length = reader.ReadInt32();

            // Validate that section doesn't overlap with header
            if (offset < HeaderSize)
            {
                throw new InvalidDataException("Save file section points into the header.");
            }

            entries[id] = new SectionEntry(id, offset, length);
        }

        return entries;
    }

    /// <summary>
    ///     Writes the section index to the file header.
    ///     The index is written at the beginning of the file after the magic number and version.
    /// </summary>
    /// <param name="stream">The file stream to write to.</param>
    /// <param name="entries">The section entries to write to the index.</param>
    /// <exception cref="InvalidDataException">Thrown when there are too many sections to fit in the header.</exception>
    private static void WriteIndex(FileStream stream, Dictionary<int, SectionEntry> entries)
    {
        if (entries.Count > MaxSections)
        {
            throw new InvalidDataException("Save file index is too large for the header.");
        }

        stream.Position = 0;
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write((ushort)entries.Count);

        // Write each section entry in sorted order by ID
        foreach (var entry in entries.Values.OrderBy(entry => entry.Id))
        {
            writer.Write(entry.Id);
            writer.Write(entry.Offset);
            writer.Write(entry.Length);
        }
    }

    /// <summary>
    ///     Represents an entry in the section index.
    /// </summary>
    /// <param name="Id">The unique identifier of the section.</param>
    /// <param name="Offset">The byte offset from the start of the file where the section data begins.</param>
    /// <param name="Length">The size of the section data in bytes.</param>
    private readonly record struct SectionEntry(int Id, long Offset, int Length);
}
