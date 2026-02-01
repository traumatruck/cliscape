using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Anvil definitions for smithing bars into equipment.
/// </summary>
public static class Anvils
{
    /// <summary>
    ///     A standard anvil found near smithing areas.
    /// </summary>
    public static readonly Anvil StandardAnvil = new()
    {
        Name = "Anvil"
    };

    /// <summary>
    ///     The Varrock anvil near the west bank.
    /// </summary>
    public static readonly Anvil VarrockAnvil = new()
    {
        Name = "Varrock anvil"
    };

    /// <summary>
    ///     The Lumbridge anvil in the basement.
    /// </summary>
    public static readonly Anvil LumbridgeAnvil = new()
    {
        Name = "Lumbridge anvil"
    };

    /// <summary>
    ///     The Falador anvil near the furnace.
    /// </summary>
    public static readonly Anvil FaladorAnvil = new()
    {
        Name = "Falador anvil"
    };
}