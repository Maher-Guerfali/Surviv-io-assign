using Code.Gameplay.Abilities.Configs;

namespace Code.Gameplay.Abilities.Data
{
    public class AbilityData
    {
        public AbilityConfig Config { get; private set; }
        public int Stacks { get; set; }

        public AbilityData(AbilityConfig config)
        {
            Config = config;
            Stacks = 0;
        }
    }
}
