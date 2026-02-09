using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Settings for initiating combat with an NPC.
/// </summary>
public class CombatAttackCommandSettings : CommandSettings
{
    [CommandArgument(0, "<NpcName>")]
    [Description("The name of the NPC to attack")]
    public required string NpcName { get; init; }
}