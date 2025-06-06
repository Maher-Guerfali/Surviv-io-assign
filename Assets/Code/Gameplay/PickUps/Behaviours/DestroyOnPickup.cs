using UnityEngine;

namespace Code.Gameplay.PickUps.Behaviours
{
	[RequireComponent(typeof(PickUp))]
	public class DestroyOnPickup : MonoBehaviour
	{
		[SerializeField] private float _delay;
		
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

		private void HandlePickup(GameObject _)
		{
			Destroy(gameObject, _delay);
		}
	}
}