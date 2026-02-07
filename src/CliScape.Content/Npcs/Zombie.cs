using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Zombie NPC found in crypts and graveyards.
/// </summary>
public class Zombie : CombatableNpc
{
    public static readonly Zombie Instance = new()
    {
        Name = new NpcName("Zombie"),
        CombatLevel = 13,
        Hitpoints = 19,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 8,
        StrengthLevel = 12,
        DefenceLevel = 5,
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
        SlashDefenceBonus = -5,
        CrushDefenceBonus = -5,
        RangedDefenceBonus = -5,
        MagicDefenceBonus = -10,

        // Other Combat Properties
        StrengthBonus = 3,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 3)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 19,
        SlayerCategory = "Zombies",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 3, MaxQuantity = 20, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.IronDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.BronzeMedHelm, Rarity = DropRarity.Uncommon }
        )
    };

    private Zombie()
    {
    }
}
