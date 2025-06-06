using Code.Gameplay.Abilities.Configs;

public interface IAbilityService
{
    void ApplyAbility(AbilityConfig config);
    bool HasAbility(AbilityType type);
    int GetStack(AbilityType type);
}