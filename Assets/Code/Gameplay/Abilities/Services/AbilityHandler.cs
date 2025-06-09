using UnityEngine;
using Code.Gameplay.UnitStats;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.UnitStats.Behaviours;
using System.Collections.Generic;

namespace Code.Gameplay.Abilities.Behaviours
{
    public class AbilityHandler : MonoBehaviour
    {
        [SerializeField] private Stats stats;

        private IAbilityService _abilityService;

        private void Awake()
        {
            // Make sure stats is assigned
            if (stats == null)
                stats = GetComponent<Stats>();

            _abilityService = new AbilityService();
        }

        public bool TryApplyAbility(AbilityConfig config)
        {
            Debug.Log($"[AbilityHandler] Trying to apply ability: {config.Type}");
             
            if (!_abilityService.CanApply(config))
            {
                Debug.Log($"[AbilityHandler] Cannot apply ability: {config.Type}");
                return false;
            }

            _abilityService.ApplyAbility(config, stats);
            Debug.Log($"[AbilityHandler] Successfully applied ability: {config.Type}");
            return true;
        }

        public bool HasAbility(AbilityType type)
        {
            return _abilityService.HasAbility(type);
        }

        public bool CanApply(AbilityConfig ability)
        {
            int current = GetStackCount(ability.Type);
            return current < ability.MaxStacks;
        }


        public int GetStackCount(AbilityType type)
        {
            return _abilityService.GetAbilityStackCount(type);
        }
    }
}