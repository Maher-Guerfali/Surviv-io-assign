using UnityEngine;

namespace Code.Gameplay.Movement.Behaviours
{
	public class SettableMovementDirection : MonoBehaviour, IMovementDirectionProvider
	{
		[SerializeField] private Vector2 _direction;
		
		public Vector2 GetDirection()
		{
			return _direction.normalized;
		}

		public void SetDirection(Vector2 direction)
		{
			_direction = direction.normalized;
		}
	}
}