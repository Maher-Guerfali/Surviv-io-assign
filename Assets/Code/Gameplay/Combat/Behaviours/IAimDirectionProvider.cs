using UnityEngine;

namespace Code.Gameplay.Combat.Behaviours
{
	public interface IAimDirectionProvider
	{
		public Vector2 GetAimDirection();
	}
}