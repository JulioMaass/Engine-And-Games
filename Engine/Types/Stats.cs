using System;

namespace Engine.Types;

public class Stats
{
    // Stats
    public float DefensePercentage { get; set; }
    public int BurningRateMultiplier { get; set; }
    public int HealOnKillMultiplier { get; set; }
    public float ExtraMoveSpeed { get; set; }
    public float ExtraDashSpeed { get; set; }
    public int ExtraItemAttractionRadius { get; set; }

    // Abilities
    public bool DoubleJump { get; set; }
    public bool Dash { get; set; }

    // Shot Modifiers
    public Type Shooter { get; set; } // TODO: Enforce that this is a Shooter
    public Type SecondaryShooter { get; set; }
    public float ExtraAttackSpeed { get; set; }
    public float ExtraSpeed { get; set; }
    public int ExtraShots { get; set; }
    public int ExtraSplitLevel { get; set; }
    public float ExtraDamagePercentage { get; set; }
    public int ExtraSize { get; set; }
    public int AddedBlastLevel { get; set; }
}
