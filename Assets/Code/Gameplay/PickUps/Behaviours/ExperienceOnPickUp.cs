
using Code.Gameplay.Characters.Heroes.Services;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.PickUps.Behaviours
{
	[RequireComponent(typeof(PickUp))]
	public class ExperienceOnPickUp : MonoBehaviour
	{
		[SerializeField] private int _xpAmountt = 1;

		private PickUp _pickUp;
		private IExperienceService _xpService;

		[Inject]
		private void Construct(IExperienceService xpService)
		{
			_xpService = xpService;
		}

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
			Debug.Log("XP Gained: " + _xpAmountt);
			_xpService.AddExperience(_xpAmountt);
		}
	}
}
