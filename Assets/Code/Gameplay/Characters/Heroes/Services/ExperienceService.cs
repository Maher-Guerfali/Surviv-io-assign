using Code.Gameplay.UnitStats;
using System;
using UnityEngine;
using Code.Gameplay.Abilities.Behaviours;
using Zenject;

namespace Code.Gameplay.Characters.Heroes.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IHeroProvider _heroProvider;
        private readonly LevelUpWindowBehaviour _levelUpWindow;

        private const float XP_PER_LEVEL = 10f;

        public float CurrentXP => _heroProvider.Stats.GetStat(StatType.CurrentXP);
        public float RequiredXP => XP_PER_LEVEL;
        public int CurrentLevel => (int)_heroProvider.Stats.GetStat(StatType.Level);

        public event Action<int> OnLevelUp;
        public event Action<float, float> OnXPChanged;

        public ExperienceService(IHeroProvider heroProvider, LevelUpWindowBehaviour levelUpWindow)
        {
            _heroProvider = heroProvider;
            _levelUpWindow = levelUpWindow;

            if (_heroProvider.Stats.GetStat(StatType.Level) <= 0)
                _heroProvider.Stats.SetBaseStat(StatType.Level, 1);

            _heroProvider.Stats.SetBaseStat(StatType.RequiredXP, XP_PER_LEVEL);
        }

        public void AddExperience(float amount)
        {
            if (_heroProvider.Stats == null) return;

            float newXP = CurrentXP + amount;

            while (newXP >= XP_PER_LEVEL)
            {
                newXP -= XP_PER_LEVEL;
                int nextLevel = CurrentLevel + 1;

                _heroProvider.Stats.SetBaseStat(StatType.Level, nextLevel);
                OnLevelUp?.Invoke(nextLevel);

                // Trigger level-up window
                _levelUpWindow.ShowLevelUpWindow();
            }

            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, newXP);
            OnXPChanged?.Invoke(newXP, XP_PER_LEVEL);
        }

        public void ResetExperience()
        {
            _heroProvider.Stats.SetBaseStat(StatType.Level, 1);
            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, 0f);
            _heroProvider.Stats.SetBaseStat(StatType.RequiredXP, XP_PER_LEVEL);
            OnXPChanged?.Invoke(0f, XP_PER_LEVEL);
            Time.timeScale = 1f;
        }

        public void ConfirmLevelUp()
        {
            throw new NotImplementedException();
        }
    }
}