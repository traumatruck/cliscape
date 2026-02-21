using CliScape.Content.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Slayer;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Moss Giant NPC found in wilderness areas
///     and caves. A strong melee opponent.
/// </summary>
public class MossGiant : CombatableNpc
{
    public static readonly MossGiant Instance = new()
    {
        Name = new NpcName("Moss giant"),
        CombatLevel = 42,
        Hitpoints = 60,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 30,
        StrengthLevel = 30,
        DefenceLevel = 30,
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
        StrengthBonus = 10,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 7)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 60,
        SlayerCategory = new SlayerCategory("Giants"),

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 20, MaxQuantity = 100, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.SteelSword, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.SteelFullHelm, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.MithrilOre, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.Lobster, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollMedium, Rarity = DropRarity.Rare }
        )
    };

    private MossGiant()
    {
    }
}