using Code.Gameplay.Combat.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Guns.Behaviours
{
	public class GunAnimator : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _gunSpriteRenderer;
		[SerializeField] private RotateToCloseEnemy _rotateToCloseEnemy;

		private void Update()
		{
			_gunSpriteRenderer.flipY = Mathf.Approximately(FaceDirection(_rotateToCloseEnemy.Direction), 1) == false;
		}

		private float FaceDirection(Vector2 direction) =>
			direction.x <= 0
				? -1
				: 1;
	}
}