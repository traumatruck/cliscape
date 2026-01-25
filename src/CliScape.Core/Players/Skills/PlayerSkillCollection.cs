namespace CliScape.Core.Players.Skills;

public class PlayerSkillCollection
{
    public PlayerSkillCollection()
    {
        All =
        [
            Attack,
            Defence,
            Strength,
            Hitpoints,
            Ranged,
            Prayer,
            Magic,
            Cooking,
            Woodcutting,
            Fletching,
            Fishing,
            Firemaking,
            Crafting,
            Smithing,
            Mining,
            Herblore,
            Agility,
            Thieving,
            Slayer,
            Farming,
            Runecraft,
            Hunter,
            Construction
        ];
    }

    public IPlayerSkill[] All { get; }

    // Combat Skills
    private AttackSkill Attack { get; } = new();
    private StrengthSkill Strength { get; } = new();
    private DefenceSkill Defence { get; } = new();
    private HitpointsSkill Hitpoints { get; } = new();
    private RangedSkill Ranged { get; } = new();
    private PrayerSkill Prayer { get; } = new();
    private MagicSkill Magic { get; } = new();

    // Gathering Skills
    private MiningSkill Mining { get; } = new();
    private FishingSkill Fishing { get; } = new();
    private WoodcuttingSkill Woodcutting { get; } = new();
    private FarmingSkill Farming { get; } = new();
    private HunterSkill Hunter { get; } = new();

    // Artisan Skills
    private SmithingSkill Smithing { get; } = new();
    private CraftingSkill Crafting { get; } = new();
    private FletchingSkill Fletching { get; } = new();
    private CookingSkill Cooking { get; } = new();
    private FiremakingSkill Firemaking { get; } = new();
    private HerbloreSkill Herblore { get; } = new();
    private ConstructionSkill Construction { get; } = new();

    // Support Skills
    private AgilitySkill Agility { get; } = new();
    private ThievingSkill Thieving { get; } = new();
    private SlayerSkill Slayer { get; } = new();
    private RunecraftSkill Runecraft { get; } = new();

    /// <summary>
    ///     Calculates the player's combat level using OSRS formula.
    /// </summary>
    public int CombatLevel
    {
        get
        {
            var baseLevel = 0.25 * (Defence.GetLevel() + Hitpoints.GetLevel() + Math.Floor(Prayer.GetLevel() / 2.0));
            var meleeLevel = 0.325 * (Attack.GetLevel() + Strength.GetLevel());
            var rangedLevel = 0.325 * (Math.Floor(Ranged.GetLevel() / 2.0) + Ranged.GetLevel());
            var magicLevel = 0.325 * (Math.Floor(Magic.GetLevel() / 2.0) + Magic.GetLevel());
            var combatLevel = baseLevel + Math.Max(meleeLevel, Math.Max(rangedLevel, magicLevel));

            return (int)Math.Floor(combatLevel);
        }
    }

    /// <summary>
    ///     Calculates the player's total level (sum of all skill levels).
    /// </summary>
    public int TotalLevel
    {
        get { return All.Sum(skill => skill.Level.Value); }
    }
}