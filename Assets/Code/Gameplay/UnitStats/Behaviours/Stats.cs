using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.UnitStats.Behaviours
{
	public class Stats : MonoBehaviour
	{
		private readonly Dictionary<StatType, float> _baseStats = new();
		private readonly List<StatModifier> _statsModifiers = new();

		public event Action<StatType, float> OnStatChanged;

		private void Awake()
		{
			ResetBaseStats();
		}

		public Stats SetBaseStat(StatType statType, float value)
		{
			if (statType == StatType.Unknown)
				throw new ArgumentException($"{nameof(StatType)} cannot be Unknown", nameof(statType));

			if (_baseStats.ContainsKey(statType))
			{
				_baseStats[statType] = value;
				OnStatChanged?.Invoke(statType, value);
			}
			else
			{
				throw new ArgumentException($"{nameof(StatType)} {statType} is not valid", nameof(statType));
			}

			return this;
		}


		public virtual float GetStat(StatType statType)
		{
			if (statType == StatType.Unknown)
				throw new ArgumentException($"{nameof(StatType)} cannot be Unknown", nameof(statType));

			if (_baseStats.TryGetValue(statType, out float baseStatValue))
			{
				return baseStatValue + GetStatModifiersValue(statType);
			}

			return 0;
		}

		public float GetStatModifiersValue(StatType statType)
		{
			if (statType == StatType.Unknown)
				throw new ArgumentException($"{nameof(StatType)} cannot be Unknown", nameof(statType));

			float modifiedStat = 0f;

			foreach (var statModifier in _statsModifiers)
			{
				if (statModifier.LinkedStatType == statType)
				{
					modifiedStat += statModifier.Value;
				}
			}

			return modifiedStat;
		}

		public void AddStatModifier(StatModifier statModifier)
		{
			_statsModifiers.Add(statModifier);

			float newStatValue = GetStat(statModifier.LinkedStatType);
			OnStatChanged?.Invoke(statModifier.LinkedStatType, newStatValue);
		}

		public void RemoveStatModifier(StatModifier statModifier)
		{
			_statsModifiers.Remove(statModifier);

			float newStatValue = GetStat(statModifier.LinkedStatType);
			OnStatChanged?.Invoke(statModifier.LinkedStatType, newStatValue);
		}

		private void ResetBaseStats()
		{
			foreach (StatType statType in Enum.GetValues(typeof(StatType)))
			{
				if (statType == StatType.Unknown)
					continue;

				_baseStats[statType] = 0f;
			}
		}
	}
}