using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
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
		[SerializeField] private Text _levelText; // Add this for level display
		private IHeroProvider _heroProvider;
		private IEnemyDeathTracker _enemyDeathTracker;
		private IExperienceService _experienceService;

		public override bool IsUserCanClose => false;

		[Inject]
		private void Construct(IHeroProvider heroProvider, IEnemyDeathTracker enemyDeathTracker, IExperienceService experienceService)
		{
			_enemyDeathTracker = enemyDeathTracker;
			_heroProvider = heroProvider;
			_experienceService = experienceService;
		}

		protected override void OnUpdate()
		{
			UpdateHealthBar();
			UpdateKilledEnemiesText();
			UpdateXPBar();
			UpdateLevelText(); // Update level display
			Debug.Log(_experienceService.CurrentLevel);
		}

		private void UpdateKilledEnemiesText()
		{
			_killedEnemiesText.text = _enemyDeathTracker.TotalKilledEnemies.ToString();
		}

		private void UpdateHealthBar()
		{
			if (_heroProvider.Hero != null)
				_healthBar.value = _heroProvider.Health.CurrentHealth / _heroProvider.Health.MaxHealth;
			else
				_healthBar.value = 0;
		}
		private void UpdateLevelText()
		{
			if (_experienceService != null)
			{
				_levelText.text = $"{_experienceService.CurrentLevel}";
			}
			else
			{
				_levelText.text = " 0";
			}
		}
		private void UpdateXPBar()
		{
			if (_experienceService != null)
			{
				float xpRatio = _experienceService.CurrentXP / _experienceService.RequiredXP;
				_xpBar.value = xpRatio;

				var fill = _xpBar.fillRect.GetComponent<Image>();
				if (fill != null)
				{
					fill.color = Color.Lerp(Color.cadetBlue, Color.darkSeaGreen, xpRatio);
				}
			}
			else
			{
				_xpBar.value = 0;
			}
		}
	}
}
