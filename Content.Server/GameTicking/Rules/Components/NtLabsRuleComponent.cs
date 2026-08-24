namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Core round controller for the NT LABS game mode.
/// Difficulty is selected at round start. A stability period is followed by the main shift timer.
/// </summary>
[RegisterComponent]
public sealed partial class NtLabsRuleComponent : Component
{
    [DataField("difficulty")]
    public int Difficulty = 1;

    [DataField("stabilityPeriod")]
    public TimeSpan StabilityPeriod = TimeSpan.FromMinutes(10);

    [DataField("baseShiftTime")]
    public TimeSpan BaseShiftTime = TimeSpan.FromHours(1);

    [DataField("difficultyStep")]
    public TimeSpan DifficultyStep = TimeSpan.FromMinutes(15);

    public TimeSpan StabilityRemaining;
    public TimeSpan ShiftRemaining;
    public bool StabilityActive;
    public bool ShiftActive;
}
