using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.UnitStats.Behaviours;

namespace Code.Gameplay.Abilities.Services
{
    public interface IAbilityService
    {
        bool CanApply(AbilityConfig config);
        void ApplyAbility(AbilityConfig config, Stats stats);
        bool HasAbility(AbilityType type);
        int GetAbilityStackCount(AbilityType type);
        IReadOnlyDictionary<AbilityType, int> GetAllAppliedAbilities();
        int GetStackCount(AbilityType type); // Add this

    }
}
