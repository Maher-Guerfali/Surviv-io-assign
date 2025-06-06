using Code.Gameplay.UnitStats;
using System;
using UnityEngine;

namespace Code.Gameplay.Characters.Heroes.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IHeroProvider _heroProvider;
        private const float XP_PER_LEVEL = 10f;

        public float CurrentXP => _heroProvider.Stats.GetStat(StatType.CurrentXP);
        public float RequiredXP => XP_PER_LEVEL;
        public int CurrentLevel => (int)_heroProvider.Stats.GetStat(StatType.Level);

        public event Action<int> OnLevelUp;
        public event Action<float, float> OnXPChanged;

        private bool _levelUpPending = false;
        private int _nextLevel;

        public ExperienceService(IHeroProvider heroProvider)
        {
            _heroProvider = heroProvider;

            if (_heroProvider.Stats.GetStat(StatType.Level) <= 0)
                _heroProvider.Stats.SetBaseStat(StatType.Level, 1);

            _heroProvider.Stats.SetBaseStat(StatType.RequiredXP, XP_PER_LEVEL);
        }

        public void AddExperience(float amount)
        {
            if (_heroProvider.Stats == null || _levelUpPending) return;

            float newXP = CurrentXP + amount;
            newXP = Mathf.Clamp(newXP, 0f, XP_PER_LEVEL); // Ensures XP never goes above 10

            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, newXP);
            OnXPChanged?.Invoke(newXP, XP_PER_LEVEL);

            if (newXP >= XP_PER_LEVEL)
            {
                _levelUpPending = true;
                _nextLevel = CurrentLevel + 1;

                Time.timeScale = 0f;
                OnLevelUp?.Invoke(_nextLevel);
            }
        }

        public void ConfirmLevelUp()
        {
            if (!_levelUpPending) return;

            _levelUpPending = false;

            _heroProvider.Stats.SetBaseStat(StatType.Level, _nextLevel);
            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, 0f);

            OnXPChanged?.Invoke(0f, XP_PER_LEVEL);
            Time.timeScale = 1f;
        }

        public void ResetExperience()
        {
            _levelUpPending = false;
            _nextLevel = 1;

            _heroProvider.Stats.SetBaseStat(StatType.Level, 1);
            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, 0f);
            _heroProvider.Stats.SetBaseStat(StatType.RequiredXP, XP_PER_LEVEL);

            OnXPChanged?.Invoke(0f, XP_PER_LEVEL);
            OnLevelUp?.Invoke(1);

            Time.timeScale = 1f;
        }
    }
}
