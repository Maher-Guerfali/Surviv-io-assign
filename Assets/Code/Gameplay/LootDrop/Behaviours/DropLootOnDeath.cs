using System;
using System.Collections.Generic;
using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.PickUps;
using Code.Gameplay.PickUps.Services;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.LootDrop.Behaviours
{
	[RequireComponent(typeof(Health))]
	public class DropLootOnDeath : MonoBehaviour
	{
		[SerializeField] private List<LootDropInfo> _possibleLootDrops = new();

		private IPickUpFactory _pickUpFactory;
		
		private Health _health;

		[Inject]
		private void Construct(IPickUpFactory pickUpFactory)
		{
			_pickUpFactory = pickUpFactory;
		}

		private void Awake()
		{
			_health = GetComponent<Health>();
		}

		private void OnEnable()
		{
			_health.OnDeath += HandleDeath;
		}

		private void OnDisable()
		{
			_health.OnDeath -= HandleDeath;
		}

		private void HandleDeath()
		{
			foreach (LootDropInfo possibleLootDrop in _possibleLootDrops)
			{
				if (UnityEngine.Random.Range(0f, 1f) <= possibleLootDrop.Chance)
				{
					_pickUpFactory.Create(possibleLootDrop.Id, transform.position);
				}
			}
		}

		[Serializable]
		private class LootDropInfo
		{
			public PickUpId Id;
			public int Amount;
			[Range(0f, 1f)] public float Chance;
		}
	}
}