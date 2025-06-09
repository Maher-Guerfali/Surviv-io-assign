using Code.Gameplay.Projectiles.Services;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Gameplay.Vision.Behaviours;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Combat.Behaviours
{
	public class ShootProjectiles : MonoBehaviour
	{
		[SerializeField] private Transform _shootPoint;
		[SerializeField] private Stats _ownerStats;
		[SerializeField] private Team _ownerTeam;
		[SerializeField] private VisionSight _visionSight;
		
		private IProjectileFactory _projectileFactory;
		private IAimDirectionProvider _aimDirectionProvider;

		private float _shootCooldownTimeLeft;

		[Inject]
		private void Construct(IProjectileFactory projectileFactory)
		{
			_projectileFactory = projectileFactory;
		}

		private void Awake()
		{
			_aimDirectionProvider = GetComponent<IAimDirectionProvider>();
		}

		private void Update()
		{
			if (_shootCooldownTimeLeft > 0)
			{
				_shootCooldownTimeLeft -= Time.deltaTime;
				return;
			}

			GameObject closestEnemy = _visionSight.GetClosestEnemy();

			if (closestEnemy != null)
			{
				Shoot();
				_shootCooldownTimeLeft = _ownerStats.GetStat(StatType.ShootCooldown);
			}
		}

		private void Shoot()
		{
			_projectileFactory.CreateProjectile(
				_shootPoint.position, 
				_aimDirectionProvider.GetAimDirection(), 
				_ownerTeam.Type, 
				_ownerStats.GetStat(StatType.Damage), 
				_ownerStats.GetStat(StatType.ProjectileSpeed));
		}
	}
}