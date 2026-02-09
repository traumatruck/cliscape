using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Rat NPC found in towns and dungeons.
/// </summary>
public class Rat : CombatableNpc
{
    public static readonly Rat Instance = new()
    {
        Name = new NpcName("Rat"),
        CombatLevel = 1,
        Hitpoints = 2,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 1,
        StrengthLevel = 1,
        DefenceLevel = 1,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = -47,
        SlashAttackBonus = -42,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -42,
        SlashDefenceBonus = -42,
        CrushDefenceBonus = -42,
        RangedDefenceBonus = -42,
        MagicDefenceBonus = -42,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 1)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = "Rats",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 1, MaxQuantity = 3, Rarity = DropRarity.Common }
        )
    };

    private Rat()
    {
    }
}