using UnityEngine;

namespace Code.Gameplay.Abilities.Configs
{
    public enum AbilityType
    {
        HealthPotionsBoost,
        PiercingProjectiles,
        BouncingProjectiles,
        OrbitingProjectiles,
        AgilityUp,
        HealthUp,
        DamageUp
    }

    [CreateAssetMenu(fileName = "AbilityConfig", menuName = "Configs/Ability Config")]
    public class AbilityConfig : ScriptableObject
    {
        public AbilityType Type;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;
        public bool CanStack;
        public int MaxStacks = 1;
    }
}