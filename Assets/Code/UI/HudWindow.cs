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
    /// <summary>
    /// Main Heads-Up Display (HUD) window shown during gameplay.
    /// Displays player health, XP, level, and killed enemies count.
    /// Also exposes parent transform for Level Up cards.
    /// </summary>
    public class HudWindow : WindowBase
    {
        [Header("Health & Stats")]
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Text _killedEnemiesText;
        [SerializeField] private Slider _xpBar;
        [SerializeField] private Text _levelText;

        [Header("Level-Up System")]
        [SerializeField] private Transform _levelUpCardsParent;

        private IHeroProvider _heroProvider;
        private IEnemyDeathTracker _enemyDeathTracker;

        /// <summary>
        /// Prevents the player from closing the HUD window manually.
        /// </summary>
        public override bool IsUserCanClose => false;

        /// <summary>
        /// Exposes the parent transform where Level Up ability cards will be spawned.
        /// Designers can reference this in UI setup.
        /// </summary>
        public Transform LevelUpCardsParent => _levelUpCardsParent;

        /// <summary>
        /// Inject required services.
        /// </summary>
        /// <param name="heroProvider">Provides access to the current player Hero and Stats.</param>
        /// <param name="enemyDeathTracker">Tracks total number of killed enemies.</param>
        [Inject]
        private void Construct(IHeroProvider heroProvider, IEnemyDeathTracker enemyDeathTracker)
        {
            _heroProvider = heroProvider;
            _enemyDeathTracker = enemyDeathTracker;
        }

        /// <summary>
        /// Called once per frame by the Window system.
        /// Updates the displayed stats and XP visuals.
        /// </summary>
        protected override void OnUpdate()
        {
            UpdateHealthBar();
            UpdateKilledEnemiesText();
            UpdateXPBarAndLevel();
        }

        /// <summary>
        /// Updates the displayed count of killed enemies.
        /// </summary>
        private void UpdateKilledEnemiesText()
        {
            _killedEnemiesText.text = _enemyDeathTracker.TotalKilledEnemies.ToString();
        }

        /// <summary>
        /// Updates the player's health bar value based on current health.
        /// </summary>
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

        /// <summary>
        /// Updates the XP progress bar and the player level text.
        /// Changes the XP bar color based on XP ratio.
        /// </summary>
        private void UpdateXPBarAndLevel()
        {
            if (_heroProvider?.Stats != null)
            {
                Stats stats = _heroProvider.Stats;

                float currentXP = stats.GetStat(StatType.CurrentXP);
                float requiredXP = stats.GetStat(StatType.RequiredXP);
                int currentLevel = (int)stats.GetStat(StatType.Level);

                // Update XP progress bar
                _xpBar.value = currentXP / requiredXP;

                // Optionally change XP bar color to give visual feedback
                var fill = _xpBar.fillRect?.GetComponent<Image>();
                if (fill != null)
                {
                    float xpRatio = currentXP / requiredXP;
                    fill.color = Color.Lerp(Color.green, Color.yellow, xpRatio);
                }

                // Update player level text
                _levelText.text = "Lv." + currentLevel.ToString();
            }
        }
    }
}
