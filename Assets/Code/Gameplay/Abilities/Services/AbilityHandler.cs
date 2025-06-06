using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Data;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Abilities.Services
{
    /// <summary>
    /// Handles abilities applied to the unit, delegating logic to the AbilityService.
    /// Designed for easy use by designers and for extension.
    /// </summary>
    [DisallowMultipleComponent]
    public class AbilityHandler : MonoBehaviour
    {
        private Stats _stats;
        private AbilityService _abilityService;

        // Local tracking dictionary for debugging (can be removed if handled fully inside AbilityService)
        private Dictionary<AbilityType, AbilityData> _abilities = new();

        /// <summary>
        /// Initializes the handler with the unit's Stats.
        /// </summary>
        public void Init(Stats stats)
        {
            _stats = stats;
            _abilityService = new AbilityService(_stats);
            Debug.Log("[AbilityHandler] Initialized with Stats.");
        }

        /// <summary>
        /// Applies an ability to the unit.
        /// </summary>
        public void ApplyAbility(AbilityConfig config)
        {
            if (_abilityService == null)
            {
                Debug.LogError("[AbilityHandler] AbilityService not initialized! Call Init(stats) first.");
                return;
            }

            Debug.Log($"[AbilityHandler] Applying ability: {config.Type}");
            _abilityService.ApplyAbility(config);

            // Optional: track locally for debug or UI purposes
            if (!_abilities.TryGetValue(config.Type, out var data))
            {
                data = new AbilityData(config);
                _abilities[config.Type] = data;
            }

            data.Stacks = _abilityService.GetStack(config.Type);
        }

        /// <summary>
        /// Check if the unit currently has a specific ability.
        /// </summary>
        public bool HasAbility(AbilityType type)
        {
            return _abilityService?.HasAbility(type) ?? false;
        }

        /// <summary>
        /// Returns the stack count of a specific ability.
        /// </summary>
        public int GetStack(AbilityType type)
        {
            return _abilityService?.GetStack(type) ?? 0;
        }
    }
}
