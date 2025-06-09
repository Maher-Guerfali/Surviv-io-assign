using UnityEngine;
using System.Collections.Generic;

namespace Code.Gameplay.Abilities.Configs
{
    [CreateAssetMenu(fileName = "AbilityDatabase", menuName = "Configs/Ability Database")]
    public class AbilityDatabase : ScriptableObject
    {
        public List<AbilityConfig> Abilities;

        public AbilityConfig GetRandomAbility(List<AbilityType> excludeTypes)
        {
            var filtered = Abilities.FindAll(a => !excludeTypes.Contains(a.Type));
            return filtered.Count > 0 ? filtered[Random.Range(0, filtered.Count)] : null;
        }
    }
}