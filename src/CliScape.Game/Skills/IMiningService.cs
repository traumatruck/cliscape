using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles mining logic.
/// </summary>
public interface IMiningService
{
    /// <summary>
    ///     Validates if the player can mine the specified rock.
    /// </summary>
    (bool CanMine, string? ErrorMessage, string? PickaxeName) CanMine(Player player, IMiningRock rock);

    /// <summary>
    ///     Performs mining at the specified rock.
    /// </summary>
    MiningResult Mine(Player player, IMiningRock rock, int count, Func<ItemId, IItem?> itemResolver);
}