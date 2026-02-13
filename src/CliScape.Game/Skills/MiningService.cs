using CliScape.Core;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Default implementation of <see cref="IMiningService" />.
/// </summary>
public sealed class MiningService : IMiningService
{
    /// <summary>
    ///     Pickaxe item IDs in order of tier.
    /// </summary>
    private static readonly (ItemId Id, PickaxeTier Tier, int RequiredLevel)[] Pickaxes =
    [
        (new ItemId(701), PickaxeTier.Bronze, 1),
        (new ItemId(710), PickaxeTier.Iron, 1),
        (new ItemId(711), PickaxeTier.Steel, 6),
        (new ItemId(712), PickaxeTier.Mithril, 21),
        (new ItemId(713), PickaxeTier.Adamant, 31),
        (new ItemId(714), PickaxeTier.Rune, 41)
    ];

    public static readonly MiningService Instance = new(
        ToolChecker.Instance,
        DomainEventDispatcher.Instance);

    private readonly IDomainEventDispatcher _eventDispatcher;

    private readonly IToolChecker _toolChecker;

    public MiningService(
        IToolChecker toolChecker,
        IDomainEventDispatcher eventDispatcher)
    {
        _toolChecker = toolChecker;
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public ServiceResult<string> CanMine(Player player, IMiningRock rock)
    {
        var miningLevel = player.GetSkillLevel(SkillConstants.MiningSkillName).Value;

        if (miningLevel < rock.RequiredLevel)
        {
            return ServiceResult<string>.Fail($"You need level {rock.RequiredLevel} Mining to mine this rock.");
        }

        if (!HasAppropriatePickaxe(player, rock.RequiredPickaxe, miningLevel, out var pickaxeName))
        {
            return ServiceResult<string>.Fail($"You need at least a {rock.RequiredPickaxe} pickaxe to mine this rock.");
        }

        return ServiceResult<string>.Ok(pickaxeName!);
    }

    /// <inheritdoc />
    public MiningResult Mine(Player player, IMiningRock rock, int count, Func<ItemId, IItem?> itemResolver)
    {
        var miningSkill = player.GetSkill(SkillConstants.MiningSkillName);

        var oreItem = itemResolver(rock.OreItemId);
        if (oreItem is null)
        {
            return new MiningResult(false, "Something went wrong while mining.", 0, null, 0);
        }

        var oresObtained = 0;
        var totalXp = 0;
        var levelBefore = miningSkill.Level.Value;

        for (var i = 0; i < count; i++)
        {
            if (player.Inventory.IsFull)
            {
                if (oresObtained == 0)
                {
                    return new MiningResult(false, "Your inventory is full.", 0, null, 0);
                }

                break;
            }

            if (!player.Inventory.TryAdd(oreItem))
            {
                break;
            }

            Player.AddExperience(miningSkill, rock.Experience);
            oresObtained++;
            totalXp += rock.Experience;
        }

        if (oresObtained == 0)
        {
            return new MiningResult(false, "You didn't get any ore.", 0, null, 0);
        }

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = miningSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.MiningSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.MiningSkillName, levelAfter));
        }

        _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.MiningSkillName, totalXp,
            miningSkill.Level.Experience));

        return new MiningResult(true, $"You get some {oreItem.Name}.", oresObtained, oreItem.Name.Value, totalXp,
            levelUp);
    }

    private bool HasAppropriatePickaxe(Player player, PickaxeTier requiredTier, int miningLevel, out string pickaxeName)
    {
        pickaxeName = string.Empty;

        // Check from best to worst pickaxe
        foreach (var (id, tier, requiredLevel) in Pickaxes.Reverse())
        {
            // Skip if player doesn't have the level for this pickaxe
            if (miningLevel < requiredLevel)
            {
                continue;
            }

            // Skip if this pickaxe isn't good enough
            if (tier < requiredTier)
            {
                continue;
            }

            if (_toolChecker.HasTool(player, id))
            {
                pickaxeName = $"{tier} pickaxe";
                return true;
            }
        }

        return false;
    }
}