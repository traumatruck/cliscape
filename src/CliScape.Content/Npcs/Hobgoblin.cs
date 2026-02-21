using CliScape.Content.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Slayer;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Hobgoblin NPC found in wilderness
///     and coastal areas. Stronger than goblins.
/// </summary>
public class Hobgoblin : CombatableNpc
{
    public static readonly Hobgoblin Instance = new()
    {
        Name = new NpcName("Hobgoblin"),
        CombatLevel = 28,
        Hitpoints = 29,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 22,
        StrengthLevel = 24,
        DefenceLevel = 20,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = 0,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = -10,

        // Other Combat Properties
        StrengthBonus = 5,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 5)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 29,
        SlayerCategory = new SlayerCategory("Hobgoblins"),

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 10, MaxQuantity = 40, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.IronDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.SteelSword, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.Bread, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollEasy, Rarity = DropRarity.Rare }
        )
    };

    private Hobgoblin()
    {
    }
}