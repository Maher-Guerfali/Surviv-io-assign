using UnityEngine;

namespace Code.Gameplay.Movement.Behaviours
{
	public interface IMovementDirectionProvider
	{
		public Vector2 GetDirection();
		public void SetDirection(Vector2 direction);
	}
}