using CliScape.Content.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Slayer;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Skeleton NPC found in dungeons and haunted locations.
/// </summary>
public class Skeleton : CombatableNpc
{
    public static readonly Skeleton Instance = new()
    {
        Name = new NpcName("Skeleton"),
        CombatLevel = 13,
        Hitpoints = 16,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 11,
        StrengthLevel = 8,
        DefenceLevel = 4,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -5,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = -5,
        RangedDefenceBonus = -5,
        MagicDefenceBonus = -5,

        // Other Combat Properties
        StrengthBonus = 2,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Slash, 2)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 16,
        SlayerCategory = new SlayerCategory("Skeletons"),

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 5, MaxQuantity = 25, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.IronDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.BronzeArrow, MinQuantity = 3, MaxQuantity = 12, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.ClueScrollEasy, Rarity = DropRarity.Rare }
        )
    };

    private Skeleton()
    {
    }
}