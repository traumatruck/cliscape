using CliScape.Core.ClueScrolls;
using CliScape.Core.World;

namespace CliScape.Content.ClueScrolls;

/// <summary>
///     Provides the pool of clue steps for each tier, referencing existing game locations.
/// </summary>
public sealed class ClueStepPool : IClueStepPool
{
    public static readonly ClueStepPool Instance = new();

    private static readonly IReadOnlyList<ClueStep> EasySteps =
    [
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig near the cow field south of Lumbridge.",
            RequiredLocation = new LocationName("Lumbridge"),
            CompletionText = "You dig in the soft earth and find a scroll!"
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the crates in Draynor Village.",
            RequiredLocation = new LocationName("Draynor Village"),
            CompletionText = "You rummage through the crates and find a clue!"
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig behind the general store in Varrock.",
            RequiredLocation = new LocationName("Varrock"),
            CompletionText = "You unearth a small casket buried beneath the cobblestones."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the fisherman at Port Sarim.",
            RequiredLocation = new LocationName("Port Sarim"),
            CompletionText = "The fisherman nods and hands you a tattered note."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the bushes near the Al Kharid gate.",
            RequiredLocation = new LocationName("Al Kharid"),
            CompletionText = "Hidden among the thorns, you find the next clue."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig by the chicken coop south of Falador.",
            RequiredLocation = new LocationName("Falador"),
            CompletionText = "Feathers scatter as you dig up a buried parcel."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the barrels on Rimmington's dock.",
            RequiredLocation = new LocationName("Rimmington"),
            CompletionText = "Inside a salt-crusted barrel you find the next step."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig beside the river in Barbarian Village.",
            RequiredLocation = new LocationName("Barbarian Village"),
            CompletionText = "The riverside mud yields a wrapped parchment."
        }
    ];

    private static readonly IReadOnlyList<ClueStep> MediumSteps =
    [
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig at the crossroads east of Draynor Manor.",
            RequiredLocation = new LocationName("Draynor Manor"),
            CompletionText = "Beneath the dusty crossroads you find a locked box."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the weapon rack in the Edgeville general store.",
            RequiredLocation = new LocationName("Edgeville"),
            CompletionText = "Behind a rusty shield you discover a hidden note."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the guard at Falador's south gate.",
            RequiredLocation = new LocationName("Falador"),
            CompletionText = "The guard looks around nervously and slips you a scroll."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig near the furnace in Al Kharid.",
            RequiredLocation = new LocationName("Al Kharid"),
            CompletionText = "The sand gives way to reveal a heat-resistant container."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the fishing shop in Catherby.",
            RequiredLocation = new LocationName("Catherby"),
            CompletionText = "Tucked behind the nets, you find a waterproof pouch."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig by the well in Taverly.",
            RequiredLocation = new LocationName("Taverly"),
            CompletionText = "The earth around the well hides a small trinket box."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the warrior in Burthorpe.",
            RequiredLocation = new LocationName("Burthorpe"),
            CompletionText = "The warrior respects your dedication and reveals the next step."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the crates at Lumbridge castle.",
            RequiredLocation = new LocationName("Lumbridge"),
            CompletionText = "In the castle's dusty storage, you uncover a sealed envelope."
        }
    ];

    private static readonly IReadOnlyList<ClueStep> HardSteps =
    [
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig beneath the gallows at Draynor Manor.",
            RequiredLocation = new LocationName("Draynor Manor"),
            CompletionText = "You unearth something that was meant to stay buried."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the anvil room in Varrock.",
            RequiredLocation = new LocationName("Varrock"),
            CompletionText = "Wedged between the anvils you find a scorched scroll."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the monk in Edgeville monastery.",
            RequiredLocation = new LocationName("Edgeville"),
            CompletionText = "The monk whispers ancient words and reveals a hidden passage."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig in the mining site south of Falador.",
            RequiredLocation = new LocationName("Falador"),
            CompletionText = "Among the ore deposits, you find a reinforced strongbox."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the palace treasury in Al Kharid.",
            RequiredLocation = new LocationName("Al Kharid"),
            CompletionText = "Behind a false wall panel, you discover the next clue."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig at the barbarian outpost's training ground.",
            RequiredLocation = new LocationName("Barbarian Village"),
            CompletionText = "Buried under the training dummies lies a warrior's cache."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the witch in Port Sarim.",
            RequiredLocation = new LocationName("Port Sarim"),
            CompletionText = "The witch cackles and conjures the next clue from thin air."
        }
    ];

    private static readonly IReadOnlyList<ClueStep> EliteSteps =
    [
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig at the ruins north of Edgeville.",
            RequiredLocation = new LocationName("Edgeville"),
            CompletionText = "Ancient magic pulses through the earth as you uncover a relic."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the throne room in Varrock Palace.",
            RequiredLocation = new LocationName("Varrock"),
            CompletionText = "Behind the throne, a secret compartment reveals itself."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig in the haunted woods of Draynor Manor.",
            RequiredLocation = new LocationName("Draynor Manor"),
            CompletionText = "The spectral fog clears as you unearth a glowing artifact."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Seek the hermit who lives near Catherby shore.",
            RequiredLocation = new LocationName("Catherby"),
            CompletionText = "The hermit speaks in riddles, but the meaning becomes clear."
        },
        new()
        {
            StepType = ClueStepType.Search,
            HintText = "Search the White Knights' armoury in Falador.",
            RequiredLocation = new LocationName("Falador"),
            CompletionText = "A legendary knight's personal effects hold the final secret."
        },
        new()
        {
            StepType = ClueStepType.Dig,
            HintText = "Dig at the heroes' monument in Burthorpe.",
            RequiredLocation = new LocationName("Burthorpe"),
            CompletionText = "Beneath the monument, a chest sealed by ancient wards awaits."
        },
        new()
        {
            StepType = ClueStepType.Talk,
            HintText = "Speak to the master smith in Al Kharid.",
            RequiredLocation = new LocationName("Al Kharid"),
            CompletionText = "The master smith reveals a map etched onto the back of an ingot."
        }
    ];

    /// <inheritdoc />
    public IReadOnlyList<ClueStep> GetSteps(ClueScrollTier tier)
    {
        return tier switch
        {
            ClueScrollTier.Easy => EasySteps,
            ClueScrollTier.Medium => MediumSteps,
            ClueScrollTier.Hard => HardSteps,
            ClueScrollTier.Elite => EliteSteps,
            _ => EasySteps
        };
    }
}