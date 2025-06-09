using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Gameplay.Vision.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Combat.Behaviours
{
	[RequireComponent(typeof(Stats))]
	public class RotateToCloseEnemy : MonoBehaviour, IAimDirectionProvider
	{
		[SerializeField] private VisionSight _visionSight;

		private Stats _stats;
		private Health _health;
		
		public Vector2 Direction { get; private set; }

		private void Awake()
		{
			_stats = GetComponent<Stats>();
			_health = GetComponent<Health>();
		}

		private void Update()
		{
			if (_health != null && _health.IsDead)
			{
				return;
			}

			Rotate();
		}

		public Vector2 GetAimDirection()
		{
			if (_health != null && _health.IsDead)
			{
				return Vector2.zero;
			}

			return Direction;
		}

		private void Rotate()
		{
			GameObject closestEnemy = _visionSight.GetClosestEnemy();

			if (closestEnemy != null)
			{
				float rotationSpeed = _stats.GetStat(StatType.RotationSpeed);
				Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
				Direction = Vector2.Lerp(transform.right, direction, rotationSpeed * Time.deltaTime);
				
				if (Direction.sqrMagnitude >= 0.01f)
				{
					float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.Euler(0, 0, angle);
				}
			}
		}
	}
}