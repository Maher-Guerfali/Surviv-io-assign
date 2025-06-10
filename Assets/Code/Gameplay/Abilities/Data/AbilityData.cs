using Code.Gameplay.Abilities.Configs;

namespace Code.Gameplay.Abilities.Data
{
    public class AbilityDatas
    {
        public AbilityConfig Config { get; private set; } // Extra Script not used Yet
        public int Stacks { get; set; }

        public AbilityDatas(AbilityConfig config)
        {
            Config = config;
            Stacks = 0;
        }
    }
}
