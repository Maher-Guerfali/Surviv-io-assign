using Code.Gameplay.Characters.Heroes.Behaviours;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.Movement.Behaviours;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Input.Behaviours
{
	public class EnemyBrainInput : MonoBehaviour, IInput, IMovementDirectionProvider
	{
		private IHeroProvider _heroProvider;

		[Inject]
		private void Construct(IHeroProvider heroProvider)
		{
			_heroProvider = heroProvider;
		}
		
		public Vector2 GetDirection()
		{
			Hero hero = _heroProvider.Hero;
			
			if (hero == null)
				return Vector2.zero;

			Vector3 direction = (hero.transform.position - transform.position).normalized;
			
			return direction;
		}

		public void SetDirection(Vector2 direction)
		{
			throw new System.NotImplementedException($"{nameof(EnemyBrainInput)} is not designed to set direction.");
		}
	}
}