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

        // 6/6/2025 AI-Tag
        // This was created with the help of Assistant, a Unity Artificial Intelligence product.

        // 6/7/2025 AI-Tag
        // This was created with the help of Assistant, a Unity Artificial Intelligence product.

        public void AddExperience(float amount)
        {
            Debug.Log($"Before XP: {CurrentXP}, Adding: {amount}");

            if (_heroProvider.Stats == null) return;

            float newXP = CurrentXP + amount;

            while (newXP >= XP_PER_LEVEL)
            {
                newXP -= XP_PER_LEVEL; // Subtract the XP required for the current level
                _nextLevel = CurrentLevel + 1;

                // Trigger level-up event
                OnLevelUp?.Invoke(_nextLevel);

                // Update the level
                _heroProvider.Stats.SetBaseStat(StatType.Level, _nextLevel);
            }

            // Update XP after all level-ups are processed
            _heroProvider.Stats.SetBaseStat(StatType.CurrentXP, newXP);
            OnXPChanged?.Invoke(newXP, XP_PER_LEVEL);

            Debug.Log($"New XP: {newXP}");
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
