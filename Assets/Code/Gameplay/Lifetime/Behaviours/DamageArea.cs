using System;
using System.Collections.Generic;
using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Gameplay.Lifetime.Behaviours
{
	[RequireComponent(typeof(Stats))]
	public class DamageArea : MonoBehaviour, IDamageApplier
	{
		private Stats _stats;
		[CanBeNull] private Team _team;
		
		private readonly List<int> _damagedTargetIds = new();
		
		public event Action<Health> OnDamageApplied;

		private void Awake()
		{
			_stats = GetComponent<Stats>();
			_team = GetComponent<Team>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out Health health) == false)  
				return;
			
			if (CanDamage(health))
				Damage(health);
		}

		private bool CanDamage(Health health)
		{
			var result = true;
			
			if (health.TryGetComponent(out Id id))
			{
				if (_damagedTargetIds.Contains(id.Value))
					result = false;
			}
			
			if (health.TryGetComponent(out Team otherTeam) && _team != null)
			{
				if (otherTeam.Type == _team.Type)
					result = false;
			}

			return result;
		}
		
		private void Damage(Health health)
		{
			float damage = _stats.GetStat(StatType.Damage);
			health.ApplyDamage(damage);
			
			if (health.TryGetComponent(out Id id))
			{
				if (_damagedTargetIds.Contains(id.Value) == false)
				{
					_damagedTargetIds.Add(id.Value);
				}
			}
			
			OnDamageApplied?.Invoke(health);
		}
	}
}