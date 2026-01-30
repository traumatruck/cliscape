using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Static registry of all item IDs in the game.
///     IDs are organized by category to avoid conflicts.
/// </summary>
public static class ItemIds
{
    // Currency: 1-99
    public static readonly ItemId Coins = new(1);

    // Bronze Equipment: 100-199
    public static readonly ItemId BronzeDagger = new(100);
    public static readonly ItemId BronzeSword = new(101);
    public static readonly ItemId BronzeScimitar = new(102);
    public static readonly ItemId BronzeAxe = new(103);
    public static readonly ItemId BronzeMace = new(104);
    public static readonly ItemId BronzeFullHelm = new(110);
    public static readonly ItemId BronzeMedHelm = new(111);
    public static readonly ItemId BronzePlatebody = new(120);
    public static readonly ItemId BronzeChainbody = new(121);
    public static readonly ItemId BronzePlatelegs = new(130);
    public static readonly ItemId BronzePlateskirt = new(131);
    public static readonly ItemId BronzeKiteshield = new(140);
    public static readonly ItemId BronzeSqShield = new(141);
    public static readonly ItemId BronzeBoots = new(150);
    public static readonly ItemId BronzeGloves = new(151);

    // Iron Equipment: 200-299
    public static readonly ItemId IronDagger = new(200);
    public static readonly ItemId IronSword = new(201);
    public static readonly ItemId IronScimitar = new(202);
    public static readonly ItemId IronAxe = new(203);
    public static readonly ItemId IronMace = new(204);
    public static readonly ItemId IronFullHelm = new(210);
    public static readonly ItemId IronMedHelm = new(211);
    public static readonly ItemId IronPlatebody = new(220);
    public static readonly ItemId IronChainbody = new(221);
    public static readonly ItemId IronPlatelegs = new(230);
    public static readonly ItemId IronPlateskirt = new(231);
    public static readonly ItemId IronKiteshield = new(240);
    public static readonly ItemId IronSqShield = new(241);

    // Steel Equipment: 300-399
    public static readonly ItemId SteelDagger = new(300);
    public static readonly ItemId SteelSword = new(301);
    public static readonly ItemId SteelScimitar = new(302);
    public static readonly ItemId SteelAxe = new(303);
    public static readonly ItemId SteelMace = new(304);
    public static readonly ItemId SteelFullHelm = new(310);
    public static readonly ItemId SteelMedHelm = new(311);
    public static readonly ItemId SteelPlatebody = new(320);
    public static readonly ItemId SteelChainbody = new(321);
    public static readonly ItemId SteelPlatelegs = new(330);
    public static readonly ItemId SteelKiteshield = new(340);

    // Leather Armor: 500-599
    public static readonly ItemId LeatherBody = new(500);
    public static readonly ItemId LeatherChaps = new(501);
    public static readonly ItemId LeatherGloves = new(502);
    public static readonly ItemId LeatherBoots = new(503);
    public static readonly ItemId LeatherCowl = new(504);
    public static readonly ItemId LeatherVambraces = new(505);
    public static readonly ItemId HardleatherBody = new(510);

    // Wooden Equipment: 600-699
    public static readonly ItemId WoodenShield = new(600);

    // Food: 1000-1099
    public static readonly ItemId Bread = new(1000);
    public static readonly ItemId Cake = new(1001);
    public static readonly ItemId Shrimp = new(1002);
    public static readonly ItemId Trout = new(1003);
    public static readonly ItemId Salmon = new(1004);
    public static readonly ItemId Lobster = new(1005);

    // Raw Materials: 1100-1199
    public static readonly ItemId Bones = new(1100);
    public static readonly ItemId RawBeef = new(1101);
    public static readonly ItemId Cowhide = new(1102);
    public static readonly ItemId Feather = new(1103);
    public static readonly ItemId RawChicken = new(1104);
}