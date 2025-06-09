using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Projectiles.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Abilities.Services
{
    public class AbilityService : IAbilityService
    {
        private readonly Dictionary<AbilityType, int> _appliedAbilities = new();


        public bool CanApply(AbilityConfig config)
        {
            if (!_appliedAbilities.TryGetValue(config.Type, out int currentCount))
                return true;

            return config.CanStack ? currentCount < config.MaxStacks : currentCount == 0;
        }
        public int GetStackCount(AbilityType type)
        {
            return _appliedAbilities.TryGetValue(type, out var count) ? count : 0;
        }


        public void ApplyAbility(AbilityConfig config, Stats stats)
        {
            if (!CanApply(config)) return;

            switch (config.Type)
            {
                case AbilityType.HealthPotionsBoost:
                    stats.AddStatModifier(new StatModifier(StatType.HealthPotionEffectiveness, 1f));
                    break;

                case AbilityType.PiercingProjectiles:
                    stats.AddStatModifier(new StatModifier(StatType.Piercing, 1f));
                    break;

                case AbilityType.BouncingProjectiles:
                    stats.AddStatModifier(new StatModifier(StatType.Bounces, 1f));
                    break;

                case AbilityType.OrbitingProjectiles:
                    stats.AddStatModifier(new StatModifier(StatType.OrbitingProjectiles, 1f));
                    CreateOrbitingProjectileSystem(stats.gameObject); // Add this line
                    break;

                case AbilityType.AgilityUp:
                    stats.AddStatModifier(new StatModifier(StatType.RotationSpeed, 1f));
                    break;

                case AbilityType.HealthUp:
                    stats.AddStatModifier(new StatModifier(StatType.MaxHealth, 10f));
                    break;

                case AbilityType.DamageUp:
                    stats.AddStatModifier(new StatModifier(StatType.Damage, 5f));
                    break;
            }

            if (_appliedAbilities.ContainsKey(config.Type))
                _appliedAbilities[config.Type]++;
            else
                _appliedAbilities[config.Type] = 1;
        }

        public bool HasAbility(AbilityType type)
        {
            return _appliedAbilities.ContainsKey(type);
        }


        public int GetAbilityStackCount(AbilityType type)
        {
            return _appliedAbilities.TryGetValue(type, out int count) ? count : 0;
        }

        public IReadOnlyDictionary<AbilityType, int> GetAllAppliedAbilities() => _appliedAbilities;

        private void CreateOrbitingProjectileSystem(GameObject heroObject)
        {
            // Check if already has orbiting system
            if (heroObject.GetComponentInChildren<OrbitingProjectileSystem>() != null)
                return;

            // Load the orbiting projectile prefab
            var orbitingPrefab = Resources.Load<GameObject>("Projectiles/orbitprojectal");
            if (orbitingPrefab != null)
            {
                // Instantiate as child of hero
                GameObject orbitingSystem = Object.Instantiate(orbitingPrefab, heroObject.transform);
                orbitingSystem.name = "OrbitingProjectileSystem";

                // The prefab should already have OrbitingProjectileSystem component
                var orbitSystem = orbitingSystem.GetComponent<OrbitingProjectileSystem>();
                if (orbitSystem != null)
                {
                    // System will auto-initialize and start orbiting
                    Debug.Log("Orbiting Projectiles activated!");
                }
            }
            else
            {
                Debug.LogError("Could not find orbitprojectal prefab in Resources/Projectiles/");
            }
        }

    }
}
