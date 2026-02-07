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
    public static readonly ItemId BronzeHatchet = new(105);
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
    public static readonly ItemId IronBoots = new(250);
    public static readonly ItemId IronGloves = new(251);
    public static readonly ItemId IronHatchet = new(205);

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
    public static readonly ItemId SteelHatchet = new(305);
    public static readonly ItemId SteelPlatelegs = new(330);
    public static readonly ItemId SteelPlateskirt = new(331);
    public static readonly ItemId SteelKiteshield = new(340);
    public static readonly ItemId SteelSqShield = new(341);
    public static readonly ItemId SteelBoots = new(350);
    public static readonly ItemId SteelGloves = new(351);

    // Mithril Equipment: 400-499
    public static readonly ItemId MithrilDagger = new(400);
    public static readonly ItemId MithrilSword = new(401);
    public static readonly ItemId MithrilScimitar = new(402);
    public static readonly ItemId MithrilAxe = new(403);
    public static readonly ItemId MithrilMace = new(404);
    public static readonly ItemId MithrilHatchet = new(405);
    public static readonly ItemId MithrilFullHelm = new(410);
    public static readonly ItemId MithrilMedHelm = new(411);
    public static readonly ItemId MithrilPlatebody = new(420);
    public static readonly ItemId MithrilChainbody = new(421);
    public static readonly ItemId MithrilPlatelegs = new(430);
    public static readonly ItemId MithrilPlateskirt = new(431);
    public static readonly ItemId MithrilKiteshield = new(440);
    public static readonly ItemId MithrilSqShield = new(441);
    public static readonly ItemId MithrilBoots = new(450);
    public static readonly ItemId MithrilGloves = new(451);

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
    public static readonly ItemId Shortbow = new(601);

    // Tools: 700-799
    public static readonly ItemId Tinderbox = new(700);
    public static readonly ItemId BronzePickaxe = new(701);
    public static readonly ItemId SmallFishingNet = new(702);
    public static readonly ItemId Hammer = new(703);

    // Ammunition: 800-899
    public static readonly ItemId BronzeArrow = new(800);

    // Clue Scrolls: 900-913
    public static readonly ItemId ClueScrollEasy = new(900);
    public static readonly ItemId ClueScrollMedium = new(901);
    public static readonly ItemId ClueScrollHard = new(902);
    public static readonly ItemId ClueScrollElite = new(903);
    public static readonly ItemId RewardCasketEasy = new(910);
    public static readonly ItemId RewardCasketMedium = new(911);
    public static readonly ItemId RewardCasketHard = new(912);
    public static readonly ItemId RewardCasketElite = new(913);

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

    // Logs: 1200-1299
    public static readonly ItemId Logs = new(1200);
    public static readonly ItemId OakLogs = new(1201);
    public static readonly ItemId WillowLogs = new(1202);
    public static readonly ItemId MapleLogs = new(1203);
    public static readonly ItemId YewLogs = new(1204);
    public static readonly ItemId MagicLogs = new(1205);

    // Raw Fish: 1300-1399
    public static readonly ItemId RawShrimps = new(1300);
    public static readonly ItemId RawAnchovies = new(1301);
    public static readonly ItemId RawSardine = new(1302);
    public static readonly ItemId RawHerring = new(1303);
    public static readonly ItemId RawTrout = new(1304);
    public static readonly ItemId RawSalmon = new(1305);
    public static readonly ItemId RawLobster = new(1306);
    public static readonly ItemId RawTuna = new(1307);
    public static readonly ItemId RawSwordfish = new(1308);

    // Ores: 1400-1499
    public static readonly ItemId CopperOre = new(1400);
    public static readonly ItemId TinOre = new(1401);
    public static readonly ItemId IronOre = new(1402);
    public static readonly ItemId Coal = new(1403);
    public static readonly ItemId MithrilOre = new(1404);
    public static readonly ItemId AdamantiteOre = new(1405);
    public static readonly ItemId RuniteOre = new(1406);
    public static readonly ItemId SilverOre = new(1407);
    public static readonly ItemId GoldOre = new(1408);

    // Bars: 1500-1599
    public static readonly ItemId BronzeBar = new(1500);
    public static readonly ItemId IronBar = new(1501);
    public static readonly ItemId SteelBar = new(1502);
    public static readonly ItemId MithrilBar = new(1503);
    public static readonly ItemId AdamantiteBar = new(1504);
    public static readonly ItemId RuniteBar = new(1505);
    public static readonly ItemId SilverBar = new(1506);
    public static readonly ItemId GoldBar = new(1507);

    // Burnt Food: 1800-1899
    public static readonly ItemId BurntShrimps = new(1800);
    public static readonly ItemId BurntAnchovies = new(1801);
    public static readonly ItemId BurntSardine = new(1802);
    public static readonly ItemId BurntHerring = new(1803);
    public static readonly ItemId BurntTrout = new(1804);
    public static readonly ItemId BurntSalmon = new(1805);
    public static readonly ItemId BurntLobster = new(1806);
    public static readonly ItemId BurntTuna = new(1807);
    public static readonly ItemId BurntSwordfish = new(1808);
    public static readonly ItemId BurntChicken = new(1809);
    public static readonly ItemId BurntMeat = new(1810);

    // Cooked Meat: 1850-1899
    public static readonly ItemId CookedChicken = new(1850);
    public static readonly ItemId CookedMeat = new(1851);
    public static readonly ItemId Anchovies = new(1852);
    public static readonly ItemId Sardine = new(1853);
    public static readonly ItemId Herring = new(1854);
    public static readonly ItemId Tuna = new(1855);
    public static readonly ItemId Swordfish = new(1856);

    // Pickaxes: 710-719
    public static readonly ItemId IronPickaxe = new(710);
    public static readonly ItemId SteelPickaxe = new(711);
    public static readonly ItemId MithrilPickaxe = new(712);
    public static readonly ItemId AdamantPickaxe = new(713);
    public static readonly ItemId RunePickaxe = new(714);
}