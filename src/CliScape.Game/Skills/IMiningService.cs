using CliScape.Core;
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
    ///     On success, the result message contains the pickaxe name.
    /// </summary>
    ServiceResult<string> CanMine(Player player, IMiningRock rock);

    /// <summary>
    ///     Performs mining at the specified rock.
    /// </summary>
    MiningResult Mine(Player player, IMiningRock rock, int count, Func<ItemId, IItem?> itemResolver);
}