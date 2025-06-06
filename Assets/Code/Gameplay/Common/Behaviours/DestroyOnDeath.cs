using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Common.Behaviours
{
	[RequireComponent(typeof(Health))]
	public class DestroyOnDeath : MonoBehaviour
	{
		[SerializeField] private float _delay;
		
		private Health _health;

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
			Destroy(gameObject, _delay);
		}
	}
}