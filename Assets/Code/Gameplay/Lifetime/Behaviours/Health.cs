using System;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Lifetime.Behaviours
{
	[RequireComponent(typeof(Stats))]
	public class Health : MonoBehaviour
	{
		[field: SerializeField] public float CurrentHealth { get; private set; }
		[field: SerializeField] public float MaxHealth { get; private set; }

		private Stats _stats;

		public bool IsDead => CurrentHealth <= 0;

		public event Action<float> OnHealthChanged;
		public event Action OnDeath;

		private void Awake()
		{
			_stats = GetComponent<Stats>();

			_stats.OnStatChanged += HandleStatChanged;
		}

		private void OnDestroy()
		{
			_stats.OnStatChanged -= HandleStatChanged;
		}

		public void Setup(float currentHealth, float maxHealth)
		{
			CurrentHealth = currentHealth;
			MaxHealth = maxHealth;
		}

		public void ApplyDamage(float damage)
		{
			float change = Mathf.Clamp(damage, 0, CurrentHealth);
			CurrentHealth -= change;

			OnHealthChanged?.Invoke(change);
			if (IsDead)
			{
				OnDeath?.Invoke();
			}
		}

		public void Heal(float healAmount)
		{
			// Apply health potion effectiveness modifier
			float effectiveHealAmount = ApplyHealingEffectiveness(healAmount);

			float change = Mathf.Clamp(effectiveHealAmount, 0, MaxHealth - CurrentHealth);
			CurrentHealth += change;

			OnHealthChanged?.Invoke(change);
		}

		private float ApplyHealingEffectiveness(float healAmount)
		{
			float effectiveness = _stats.GetStat(StatType.HealthPotionEffectiveness);
			if (effectiveness > 0)
			{
				// Effectiveness of 1 means double healing (1 + 1 = 2x multiplier)
				return healAmount * (1f + effectiveness);
			}
			return healAmount;
		}

		private void HandleStatChanged(StatType statType, float value)
		{
			if (statType == StatType.MaxHealth)
			{
				MaxHealth = value;
			}
		}
	}
}