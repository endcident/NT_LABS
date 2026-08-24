namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Core round controller for the NT LABS game mode.
/// Difficulty is selected at round start. A stability period is followed by the main shift timer.
/// </summary>
[RegisterComponent]
public sealed partial class NtLabsRuleComponent : Component
{
    [DataField]
    public int Difficulty = 1;

    [DataField]
    public TimeSpan StabilityPeriod = TimeSpan.FromMinutes(10);

    [DataField]
    public TimeSpan BaseShiftTime = TimeSpan.FromHours(1);

    [DataField]
    public TimeSpan DifficultyStep = TimeSpan.FromMinutes(15);

    public TimeSpan StabilityRemaining;
    public TimeSpan ShiftRemaining;
    public bool StabilityActive;
    public bool ShiftActive;
}
