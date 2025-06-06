using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Movement.Behaviours;
using Code.Gameplay.Projectiles.Behaviours;
using Code.Gameplay.Teams;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Services
{
	public class ProjectileFactory : IProjectileFactory
	{	
		private readonly IInstantiateService _instantiateService;
		private readonly IIdentifierService _identifiers;
		private readonly IAssetsService _assetsService;

		public ProjectileFactory(
			IInstantiateService instantiateService,
			IIdentifierService identifiers,
			IAssetsService assetsService)
		{
			_instantiateService = instantiateService;
			_identifiers = identifiers;
			_assetsService = assetsService;
		}
		
		public Projectile CreateProjectile(Vector3 at, Vector2 direction, TeamType teamType, float damage, float movementSpeed)
		{
			var prefab = _assetsService.LoadAssetFromResources<Projectile>("Projectiles/Projectile");
			Projectile projectile = _instantiateService.InstantiatePrefabForComponent(prefab, at, Quaternion.FromToRotation(Vector3.up, direction));
			
			projectile.GetComponent<Id>()
				.Setup(_identifiers.Next());
			
			projectile.GetComponent<Stats>()
				.SetBaseStat(StatType.MovementSpeed, movementSpeed)
				.SetBaseStat(StatType.Damage, damage);

			projectile.GetComponent<Team>()
				.Type = teamType;
			
			projectile.GetComponent<IMovementDirectionProvider>()
				.SetDirection(direction);
			
			return projectile;
		}
	}
}