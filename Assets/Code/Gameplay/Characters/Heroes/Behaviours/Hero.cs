

using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using UnityEngine;

namespace Code.Gameplay.Characters.Heroes.Behaviours
{
    public class Hero : MonoBehaviour
    {
        // List of abilities the hero currently has
        private List<AbilityConfig> _abilities = new List<AbilityConfig>();

        // Add a new ability to the hero
        public void AddAbility(AbilityConfig ability)
        {
            if (!_abilities.Contains(ability))
            {
                _abilities.Add(ability);
                Debug.Log($"Ability {ability.DisplayName} added to the hero!");
            }
            else
            {
                Debug.LogWarning($"Hero already has the ability {ability.DisplayName}!");
            }
        }

        // Example method to use an ability
        public void UseAbility(AbilityConfig ability)
        {
            if (_abilities.Contains(ability))
            {
                Debug.Log($"Using ability: {ability.DisplayName}");
                // Implement ability usage logic here
            }
            else
            {
                Debug.LogWarning($"Hero does not have the ability {ability.DisplayName}!");
            }
        }
    }
}