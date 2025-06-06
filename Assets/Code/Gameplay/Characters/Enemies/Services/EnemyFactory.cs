using Code.Gameplay.Characters.Enemies.Behaviours;
using Code.Gameplay.Characters.Enemies.Configs;
using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Characters.Enemies.Services
{
	public class EnemyFactory : IEnemyFactory
	{
		private readonly IConfigsService _configsService;
		private readonly IInstantiateService _instantiateService;
		private readonly IIdentifierService _identifiers;

		public EnemyFactory(
			IConfigsService configsService, 
			IInstantiateService instantiateService,
			IIdentifierService identifiers)
		{
			_configsService = configsService;
			_instantiateService = instantiateService;
			_identifiers = identifiers;
		}

        public Enemy CreateEnemy(EnemyId id, Vector3 at, Quaternion rotation)
        {
            // 1) Fetch the base EnemyConfig (prefab + base HP, base Damage, etc.)
            EnemyConfig enemyConfig = _configsService.GetEnemyConfig(id);

            // 2) Get the progressive‐difficulty config and compute multipliers
            DifficultyConfig prog = _configsService.DifficultyConfig;
            float elapsed = Time.timeSinceLevelLoad;

            // Cumulative multipliers based on elapsed time through all stages
            float hpMultiplier = prog.EvaluateHpMultiplier(elapsed);
            float dmgMultiplier = prog.EvaluateDamageMultiplier(elapsed);

            // 3) Compute final stats
            float scaledHealth = enemyConfig.Health * hpMultiplier;
            float scaledDamage = enemyConfig.Damage * dmgMultiplier;

            // 4) Instantiate the enemy prefab
            Enemy enemy = _instantiateService.InstantiatePrefabForComponent(
                enemyConfig.Prefab, at, rotation);

            // 5) Assign unique ID
            enemy.GetComponent<Id>()
                .Setup(_identifiers.Next());

            // 6) Set stats in the Stats component
            enemy.GetComponent<Stats>()
                .SetBaseStat(StatType.MaxHealth, scaledHealth)
                .SetBaseStat(StatType.MovementSpeed, enemyConfig.MovementSpeed)
                .SetBaseStat(StatType.Damage, scaledDamage);

            // 7) Initialize health with the newly scaled HP
            enemy.GetComponent<Health>()
                .Setup(scaledHealth, scaledHealth);

            return enemy;
        }


    }
}