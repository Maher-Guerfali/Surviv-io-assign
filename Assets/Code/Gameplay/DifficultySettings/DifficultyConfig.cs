using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DifficultyStage
{
    [Tooltip("How many seconds this stage lasts. If ≤ 0, this stage is infinite (continues forever).")]
    public float Duration;

    [Tooltip("HP growth rate per second (e.g. 0.02 means +2% of base HP each second).")]
    public float HpGrowthRate;

    [Tooltip("Damage growth rate per second (e.g. 0.015 means +1.5% of base Damage each second).")]
    public float DamageGrowthRate;

    [Header("Spawn Chance for This Stage (0–1 range)")]
    [Tooltip("Relative chance (0–1) that a Boss spawns when rolling. Higher stages typically have higher values.")]
    public float BossSpawnChance;
    [Tooltip("Relative chance (0–1) that a Walker spawns when rolling.")]
    public float WalkerSpawnChance;
    [Tooltip("Relative chance (0–1) that a Skeleton spawns when rolling.")]
    public float SkeletonSpawnChance;
    [Tooltip("Chance of Skeleton Walker spawning in this stage (0 to 1)")]
[Range(0, 1)] public float SkeletonWalkerSpawnChance;



}

[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Configs/DifficultyConfig")]
public class DifficultyConfig : ScriptableObject
{
    [Tooltip("List of stages in chronological order. Each 'Duration' is how many seconds that stage lasts; if Duration ≤ 0, it's infinite.")]
    public List<DifficultyStage> Stages = new List<DifficultyStage>()
    {
        // Example defaults (edit these in the Inspector as desired):
        new DifficultyStage
        {
            Duration = 60f,      // Stage 1 lasts 0–60s
            HpGrowthRate = 0.01f,
            DamageGrowthRate = 0.008f,
            BossSpawnChance = 0.05f,
            WalkerSpawnChance = 0.8f,
            SkeletonSpawnChance = 0.15f
        },
        new DifficultyStage
        {
            Duration = 120f,     // Stage 2 lasts 60–180s
            HpGrowthRate = 0.02f,
            DamageGrowthRate = 0.015f,
            BossSpawnChance = 0.10f,
            WalkerSpawnChance = 0.6f,
            SkeletonSpawnChance = 0.30f
        },
        new DifficultyStage
        {
            Duration = 0f,       // Stage 3 begins at 180s and never ends
            HpGrowthRate = 0.05f,
            DamageGrowthRate = 0.03f,
            BossSpawnChance = 0.15f,
            WalkerSpawnChance = 0.4f,
            SkeletonSpawnChance = 0.45f
        }
    };

    [Header("Boss Spawn Logic")]
    [Tooltip("How many seconds must pass after a boss spawns before another boss can appear.")]
    public float BossRespawnCooldown = 30f;

    [Header("Optional Spawn Interval Tuning")]
    [Tooltip("If > 0, you can use this to multiply (reduce) the base spawn interval each time you respawn enemies.")]
    public float SpawnIntervalReductionRate = 0.95f;
    public float EvaluateWalkerSpawnChance(float elapsedTime)
    {
        float chance = 0f;
        float remaining = elapsedTime;

        foreach (var stage in Stages)
        {
            if (stage.Duration <= 0)
            {
                chance += stage.SkeletonWalkerSpawnChance;
                break;
            }

            if (remaining > stage.Duration)
            {
                remaining -= stage.Duration;
            }
            else
            {
                chance += stage.SkeletonWalkerSpawnChance;
                break;
            }
        }

        return Mathf.Clamp01(chance);
    }

    public float EvaluateBossSpawnChance(float elapsedTime)
    {
        float chance = 0f;
        float remaining = elapsedTime;

        foreach (var stage in Stages)
        {
            if (stage.Duration <= 0)
            {
                chance += stage.BossSpawnChance;
                break;
            }

            if (remaining > stage.Duration)
            {
                remaining -= stage.Duration;
            }
            else
            {
                chance += stage.BossSpawnChance;
                break;
            }
        }

        return Mathf.Clamp01(chance);
    }


    /// <summary>
    /// Given elapsedTime (seconds), returns the cumulative HP multiplier.
    /// At t = 0, returns 1.0. 
    /// If the final stage has Duration ≤ 0, that stage’s rate is applied forever.
    /// </summary>
    public float EvaluateHpMultiplier(float elapsedTime)
    {
        float multiplier = 1f;
        float remainingTime = elapsedTime;

        for (int i = 0; i < Stages.Count; i++)
        {
            DifficultyStage stage = Stages[i];

            // If Duration ≤ 0, treat as infinite:
            if (stage.Duration <= 0f)
            {
                multiplier += stage.HpGrowthRate * remainingTime;
                return multiplier;
            }

            if (remainingTime > stage.Duration)
            {
                // Fully use this stage’s duration
                multiplier += stage.HpGrowthRate * stage.Duration;
                remainingTime -= stage.Duration;
            }
            else
            {
                // Partially through this stage
                multiplier += stage.HpGrowthRate * remainingTime;
                return multiplier;
            }
        }

        // If we exit the loop (elapsedTime > sum of all positive durations, no infinite stage):
        // Use the last stage’s growth rate to continue indefinitely.
        if (Stages.Count > 0)
        {
            DifficultyStage last = Stages[Stages.Count - 1];
            multiplier += last.HpGrowthRate * remainingTime;
        }

        return multiplier;
    }


    /// <summary>
    /// Given elapsedTime (seconds), returns the cumulative Damage multiplier.
    /// Works just like EvaluateHpMultiplier but with damage growth rates.
    /// </summary>
    public float EvaluateDamageMultiplier(float elapsedTime)
    {
        float multiplier = 1f;
        float remainingTime = elapsedTime;

        for (int i = 0; i < Stages.Count; i++)
        {
            DifficultyStage stage = Stages[i];

            if (stage.Duration <= 0f)
            {
                multiplier += stage.DamageGrowthRate * remainingTime;
                return multiplier;
            }

            if (remainingTime > stage.Duration)
            {
                multiplier += stage.DamageGrowthRate * stage.Duration;
                remainingTime -= stage.Duration;
            }
            else
            {
                multiplier += stage.DamageGrowthRate * remainingTime;
                return multiplier;
            }
        }

        if (Stages.Count > 0)
        {
            DifficultyStage last = Stages[Stages.Count - 1];
            multiplier += last.DamageGrowthRate * remainingTime;
        }

        return multiplier;
    }


    /// <summary>
    /// Returns the spawn‐chance trio (Boss, Walker, Skeleton) for the stage that contains elapsedTime.
    /// Each value is in [0,1] and should be used as “relative weights” when rolling.
    /// </summary>
    public (float boss, float walker, float skeleton) EvaluateSpawnChances(float elapsedTime)
    {
        float remainingTime = elapsedTime;

        for (int i = 0; i < Stages.Count; i++)
        {
            DifficultyStage stage = Stages[i];

            // If Duration ≤ 0, or if we're within this stage's time window, return its spawn chances:
            if (stage.Duration <= 0f || remainingTime <= stage.Duration)
            {
                return (stage.BossSpawnChance, stage.WalkerSpawnChance, stage.SkeletonSpawnChance);
            }

            remainingTime -= stage.Duration;
        }

        // If elapsedTime exceeds all positive durations (and no infinite stage was found),
        // use the spawn chances of the last stage:
        if (Stages.Count > 0)
        {
            var last = Stages[Stages.Count - 1];
            return (last.BossSpawnChance, last.WalkerSpawnChance, last.SkeletonSpawnChance);
        }

        // Fallback: no stages defined
        return (0f, 0f, 0f);
    }
}
