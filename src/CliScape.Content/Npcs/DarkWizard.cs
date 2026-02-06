using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Dark Wizard NPC found south of Varrock
///     and around Draynor Village. Aggressive magic attacker.
/// </summary>
public class DarkWizard : CombatableNpc
{
    public static readonly DarkWizard Instance = new()
    {
        Name = new NpcName("Dark wizard"),
        CombatLevel = 7,
        Hitpoints = 12,
        AttackSpeed = 5,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 1,
        StrengthLevel = 1,
        DefenceLevel = 3,
        RangedLevel = 1,
        MagicLevel = 11,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 8,

        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = 0,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 8,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 3,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Magic, 3)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 0,
        SlayerCategory = null,

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 5, MaxQuantity = 20, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.BronzeDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollEasy, Rarity = DropRarity.Rare }
        )
    };

    private DarkWizard()
    {
    }
}
