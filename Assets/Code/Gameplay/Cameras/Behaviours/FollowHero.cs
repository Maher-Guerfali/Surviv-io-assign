using Code.Gameplay.Characters.Heroes.Services;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Cameras.Behaviours
{
	public class FollowHero : MonoBehaviour
	{
		private IHeroProvider _heroProvider;

		[Inject]
		private void Construct(IHeroProvider heroProvider)
		{
			_heroProvider = heroProvider;
		}

		private void Update()
		{
			if (_heroProvider.Hero == null)
				return;

			Vector3 heroPosition = _heroProvider.Hero.transform.position;
			Vector3 cameraPosition = transform.position;
			
			cameraPosition.x = heroPosition.x;
			cameraPosition.y = heroPosition.y;
			
			transform.position = cameraPosition;
		}
	}
}