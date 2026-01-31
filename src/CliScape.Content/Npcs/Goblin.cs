using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
/// Represents the Goblin NPC found throughout Gielinor
/// </summary>
public class Goblin : CombatableNpc
{
    private Goblin() { }
    
    public static readonly Goblin Instance = new()
    {
        Name = new NpcName("Goblin"),
        CombatLevel = 2,
        Hitpoints = 5,
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
        StabAttackBonus = -21,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = -15,
        
        // Defensive Bonuses
        StabDefenceBonus = -15,
        SlashDefenceBonus = -15,
        CrushDefenceBonus = -15,
        RangedDefenceBonus = -15,
        MagicDefenceBonus = -15,
        
        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,
        
        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 5,
        SlayerCategory = "Goblins",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 1, MaxQuantity = 5, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.BronzeHatchet, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.BronzeDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.BronzeSqShield, Rarity = DropRarity.Uncommon }
        )
    };
}
