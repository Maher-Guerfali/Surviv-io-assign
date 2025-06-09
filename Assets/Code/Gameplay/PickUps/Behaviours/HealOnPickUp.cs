 using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;

namespace Code.Gameplay.PickUps.Behaviours
{
	[RequireComponent(typeof(PickUp))]
	public class HealOnPickUp : MonoBehaviour
	{
		[SerializeField] private float _healAmount;
		
		private PickUp _pickUp;

		private void Awake()
		{
			_pickUp = GetComponent<PickUp>();
		}

		private void OnEnable()
		{
			_pickUp.OnPickUp += HandlePickup;
		}

		private void OnDisable()
		{
			_pickUp.OnPickUp -= HandlePickup;
		}

		private void HandlePickup(GameObject pickUpper)
		{
			if (pickUpper.TryGetComponent(out Health health))
			{
				health.Heal(_healAmount);
			}
		}
	}
}