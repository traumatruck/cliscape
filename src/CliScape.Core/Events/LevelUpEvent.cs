using CliScape.Core.Players.Skills;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when a player levels up in a skill.
/// </summary>
public sealed record LevelUpEvent(SkillName SkillName, int NewLevel) : DomainEventBase;