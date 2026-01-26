namespace CliScape.Core.Npcs;

/// <summary>
/// Represents a variant of an NPC (e.g., "Level 2", "Level 5 (Spear)", "Armed")
/// </summary>
public record NpcVariant(string Name, int CombatLevel);
