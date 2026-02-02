using CliScape.Core.Players.Skills;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when experience is gained in a skill.
/// </summary>
public sealed record ExperienceGainedEvent(SkillName SkillName, int Amount, int TotalExperience) : DomainEventBase;