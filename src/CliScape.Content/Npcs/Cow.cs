using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Cow NPC found throughout Gielinor
/// </summary>
public class Cow : CombatableNpc
{
    public static readonly Cow Instance = new()
    {
        Name = new NpcName("Cow"),
        CombatLevel = 2,
        Hitpoints = 8,
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
        StabAttackBonus = -15,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -21,
        SlashDefenceBonus = -21,
        CrushDefenceBonus = -21,
        RangedDefenceBonus = -21,
        MagicDefenceBonus = -21,

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
        SlayerCategory = "Cows",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.RawBeef, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Cowhide, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.ClueScrollEasy, Rarity = DropRarity.Rare }
        )
    };

    private Cow()
    {
    }
}