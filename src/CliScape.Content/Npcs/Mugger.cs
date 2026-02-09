using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Mugger NPC found lurking near roads.
/// </summary>
public class Mugger : CombatableNpc
{
    public static readonly Mugger Instance = new()
    {
        Name = new NpcName("Mugger"),
        CombatLevel = 6,
        Hitpoints = 10,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 4,
        StrengthLevel = 4,
        DefenceLevel = 2,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 4,
        SlashAttackBonus = 2,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = 0,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 0,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Stab, 2)
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
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 3, MaxQuantity = 15, Rarity = DropRarity.Always }
        )
    };

    private Mugger()
    {
    }
}