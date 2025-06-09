using UnityEngine;

namespace Code.Gameplay.Movement.Behaviours
{
	public interface IMovement
	{
		Vector2 Velocity { get; }
	}
}