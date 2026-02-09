namespace CliScape.Core.Npcs;

public record NpcImmunities(
    bool Poison = false,
    bool Venom = false,
    bool Cannon = false,
    bool Thralls = false,
    bool Burn = false
);