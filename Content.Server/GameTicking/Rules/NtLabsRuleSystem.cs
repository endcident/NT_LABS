using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Timing;

namespace Content.Server.GameTicking.Rules;

/// <summary>
/// Runs the core NT LABS shift clock.
/// Difficulty 1 starts at one hour and every higher difficulty adds fifteen minutes.
/// Difficulty is randomised from 1 to 5 and cannot repeat the previous round.
/// </summary>
public sealed class NtLabsRuleSystem : GameRuleSystem<NtLabsRuleComponent>
{
    private int? _previousDifficulty;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void Started(EntityUid uid, NtLabsRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        component.Difficulty = RollDifficulty();
        component.StabilityRemaining = component.StabilityPeriod;
        component.ShiftRemaining = component.BaseShiftTime + component.DifficultyStep * (component.Difficulty - 1);
        component.StabilityActive = true;
        component.ShiftActive = false;

        Log.Info($"NT LABS started. Difficulty: {component.Difficulty}. Stability: {component.StabilityRemaining}. Shift: {component.ShiftRemaining}.");
    }

    protected override void Ended(EntityUid uid, NtLabsRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        component.StabilityActive = false;
        component.ShiftActive = false;
    }

    protected override void ActiveTick(EntityUid uid, NtLabsRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        base.ActiveTick(uid, component, gameRule, frameTime);

        var delta = TimeSpan.FromSeconds(frameTime);

        if (component.StabilityActive)
        {
            component.StabilityRemaining -= delta;
            if (component.StabilityRemaining > TimeSpan.Zero)
                return;

            component.StabilityRemaining = TimeSpan.Zero;
            component.StabilityActive = false;
            component.ShiftActive = true;
            Log.Info("NT LABS stability period ended. Objects may now begin their containment behaviour.");
            return;
        }

        if (!component.ShiftActive)
            return;

        component.ShiftRemaining -= delta;
        if (component.ShiftRemaining > TimeSpan.Zero)
            return;

        component.ShiftRemaining = TimeSpan.Zero;
        component.ShiftActive = false;

        // Evacuation by train will replace the round restart in the evacuation phase.
        GameTicker.EndRound(Loc.GetString("nt-labs-shift-complete"));
    }

    private int RollDifficulty()
    {
        var difficulty = RobustRandom.Next(1, 6);
        if (_previousDifficulty is { } previous && difficulty == previous)
            difficulty = difficulty == 5 ? 1 : difficulty + 1;

        _previousDifficulty = difficulty;
        return difficulty;
    }
}
