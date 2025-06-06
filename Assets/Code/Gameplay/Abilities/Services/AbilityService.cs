using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Data;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Abilities.Services
{
    public class AbilityService : IAbilityService
    {
        private readonly Stats _stats;
        private readonly Dictionary<AbilityType, AbilityData> _abilities = new();

        public AbilityService(Stats stats)
        {
            _stats = stats;
        }


public void ApplyAbility(AbilityConfig config)
    {
        if (!_abilities.TryGetValue(config.Type, out var abilityData))
        {
            abilityData = new AbilityData(config);
            _abilities[config.Type] = abilityData;
        }

        if (!config.CanStack && abilityData.Stacks > 0)
            return;

        if (abilityData.Stacks >= config.MaxStacks)
            return;

        abilityData.Stacks++;

        Debug.Log($"[AbilityService] Ability applied: {config.Type}, new stacks: {abilityData.Stacks}");

        var modifier = CreateStatModifier(config.Type);
        if (modifier != null)
        {
            _stats.AddStatModifier(modifier.Value);
        }
    }


    public bool HasAbility(AbilityType type)
        {
            return _abilities.TryGetValue(type, out var abilityData) && abilityData.Stacks > 0;
        }

        public int GetStack(AbilityType type)
        {
            return _abilities.TryGetValue(type, out var abilityData) ? abilityData.Stacks : 0;
        }

        private StatModifier? CreateStatModifier(AbilityType type)
        {
            return type switch
            {
                AbilityType.HealthUp => new StatModifier(StatType.MaxHealth, 25f, ModifierMode.Add),
                AbilityType.DamageUp => new StatModifier(StatType.Damage, 10f, ModifierMode.Add),
                AbilityType.AgilityUp => new StatModifier(StatType.RotationSpeed, 30f, ModifierMode.Add),
                AbilityType.PiercingProjectiles => new StatModifier(StatType.Piercing, 1f, ModifierMode.Add),

                // Abilities handled via other behaviours or systems
                AbilityType.HealthPotionsBoost => null,
                AbilityType.BouncingProjectiles => null,
                AbilityType.OrbitingProjectiles => null,
                _ => null,
            };
        }
    }
}
