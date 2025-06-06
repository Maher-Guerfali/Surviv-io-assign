// 6/7/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.UIManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    public class HudWindow : WindowBase
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Text _killedEnemiesText;
        [SerializeField] private Slider _xpBar;
        [SerializeField] private Text _levelText;

        private IHeroProvider _heroProvider;
        private IEnemyDeathTracker _enemyDeathTracker;

        public override bool IsUserCanClose => false;

        [Inject]
        private void Construct(IHeroProvider heroProvider, IEnemyDeathTracker enemyDeathTracker)
        {
            _heroProvider = heroProvider;
            _enemyDeathTracker = enemyDeathTracker;
        }

        protected override void OnUpdate()
        {
            UpdateHealthBar();
            UpdateKilledEnemiesText();
            UpdateXPBarAndLevel(); // Update XP and level from Stats
        }

        private void UpdateKilledEnemiesText()
        {
            _killedEnemiesText.text = _enemyDeathTracker.TotalKilledEnemies.ToString();
        }

        private void UpdateHealthBar()
        {
            if (_heroProvider?.Hero != null)
            {
                var health = _heroProvider.Health;
                _healthBar.value = health.CurrentHealth / health.MaxHealth;
            }
            else
            {
                _healthBar.value = 0;
            }
        }

        private void UpdateXPBarAndLevel()
        {
            if (_heroProvider?.Stats != null)
            {
                Stats stats = _heroProvider.Stats;

                // Fetch XP and level values from Stats
                float currentXP = stats.GetStat(StatType.CurrentXP);
                float requiredXP = stats.GetStat(StatType.RequiredXP);
                int currentLevel = (int)stats.GetStat(StatType.Level);

                // Update XP bar
                _xpBar.value = currentXP / requiredXP;

                // Change bar color based on XP ratio
                var fill = _xpBar.fillRect?.GetComponent<Image>();
                if (fill != null)
                {
                    float xpRatio = currentXP / requiredXP;
                    fill.color = Color.Lerp(Color.red, Color.green, xpRatio);
                }

                // Update level text
                _levelText.text = currentLevel.ToString();
            }
        }
    }
}