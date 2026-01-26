namespace CliScape.Core.Npcs;

public record ElementalWeakness(
    MagicElement Element,
    int PercentageBonus // e.g., 50 for 50% increased magic damage
);