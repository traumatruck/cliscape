using CliScape.Core;
using CliScape.Core.Players;
using CliScape.Core.Slayer;

namespace CliScape.Game.Slayer;

/// <summary>
///     Manages slayer task assignment and completion.
/// </summary>
public interface ISlayerService
{
    /// <summary>
    ///     Attempts to assign a new slayer task from the specified master.
    /// </summary>
    SlayerTaskResult AssignTask(Player player, SlayerMaster master);

    /// <summary>
    ///     Cancels the player's current slayer task.
    /// </summary>
    bool CancelTask(Player player);
}

/// <summary>
///     Default implementation of <see cref="ISlayerService" />.
/// </summary>
public sealed class SlayerService : ISlayerService
{
    public static readonly SlayerService Instance = new(RandomProvider.Instance);
    private readonly IRandomProvider _random;

    public SlayerService(IRandomProvider random)
    {
        _random = random;
    }

    /// <inheritdoc />
    public SlayerTaskResult AssignTask(Player player, SlayerMaster master)
    {
        // Check if player already has a task
        if (player.SlayerTask != null && !player.SlayerTask.IsComplete)
        {
            return new SlayerTaskResult.AlreadyHaveTask(player.SlayerTask);
        }

        // Check combat level requirement
        if (player.CombatLevel < master.RequiredCombatLevel)
        {
            return new SlayerTaskResult.InsufficientCombatLevel(master.RequiredCombatLevel);
        }

        // Check slayer level requirement
        var slayerSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Slayer");
        var slayerLevel = slayerSkill?.Level.Value ?? 1;
        if (slayerLevel < master.RequiredSlayerLevel)
        {
            return new SlayerTaskResult.InsufficientSlayerLevel(master.RequiredSlayerLevel);
        }

        // Get valid assignments based on slayer level
        var validAssignments = master.Assignments
            .Where(a => slayerLevel >= a.RequiredSlayerLevel)
            .ToList();

        if (validAssignments.Count == 0)
        {
            return new SlayerTaskResult.NoValidAssignments();
        }

        // Weighted random selection
        var totalWeight = validAssignments.Sum(a => a.Weight);
        var roll = _random.Next(totalWeight);
        var cumulativeWeight = 0;
        SlayerAssignment? selectedAssignment = null;

        foreach (var assignment in validAssignments)
        {
            cumulativeWeight += assignment.Weight;
            if (roll < cumulativeWeight)
            {
                selectedAssignment = assignment;
                break;
            }
        }

        selectedAssignment ??= validAssignments[0];

        // Generate random kill count
        var killCount = _random.Next(selectedAssignment.MinKills, selectedAssignment.MaxKills + 1);

        // Create and assign the task
        var task = new SlayerTask
        {
            Category = selectedAssignment.Category,
            RemainingKills = killCount,
            TotalKills = killCount,
            SlayerMaster = master.Name
        };

        player.SlayerTask = task;

        return new SlayerTaskResult.TaskAssigned(task);
    }

    /// <inheritdoc />
    public bool CancelTask(Player player)
    {
        if (player.SlayerTask == null)
        {
            return false;
        }

        player.SlayerTask = null;
        return true;
    }
}