using Code.Common.Extensions;
using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Behaviours;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Infrastructure.ConfigsManagement;
using Code.Gameplay.Characters.Enemies.Configs;

using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Characters.Enemies.Behaviours
{
	public class EnemySpawner : MonoBehaviour
	{
		private ICameraProvider _cameraProvider;
		private IHeroProvider _heroProvider;
		private IEnemyFactory _enemyFactory;
		private DifficultyConfig _difficultyConfig;


		private float _timer;

		private const float SpawnInterval = 3f;
		private const float SpawnDistanceGap = 0.5f;

		//new
		private IConfigsService _configsService;
		private float _elapsedTime;
		private bool _bossSpawnedRecently;

		[Inject]
		
		private void Construct(
	ICameraProvider cameraProvider,
	IHeroProvider heroProvider,
	IEnemyFactory enemyFactory,
	IConfigsService configsService)
		{
			_enemyFactory = enemyFactory;
			_heroProvider = heroProvider;
			_cameraProvider = cameraProvider;
			_configsService = configsService;

			_difficultyConfig = _configsService.DifficultyConfig;

			_timer = SpawnInterval * 0.9f;
		}



		private void Update()
		{
			_elapsedTime += Time.deltaTime; // new
			Spawning();
		}

		private void Spawning()
		{
			Hero hero = _heroProvider.Hero;
			if (hero == null)
				return;

			_timer += Time.deltaTime;

			if (_timer >= SpawnInterval)
			{
				float walkerChance = _difficultyConfig.EvaluateWalkerSpawnChance(_elapsedTime);
				float bossChance = _difficultyConfig.EvaluateBossSpawnChance(_elapsedTime);

				Vector2 spawnPosition = RandomSpawnPosition(hero.transform.position);

				// Boss spawn (only one at a time or based on cooldown)
				if (!_bossSpawnedRecently && Random.value <= bossChance)
				{
					_enemyFactory.CreateEnemy(EnemyId.Boss, at: spawnPosition, Quaternion.identity);
					_bossSpawnedRecently = true;
					Invoke(nameof(ResetBossSpawn), 30f); // Prevent re-spawning boss too quickly
				}
				// Otherwise try spawning Skeleton Walker
				else if (Random.value <= walkerChance)
				{
					_enemyFactory.CreateEnemy(EnemyId.Skeleton, at: spawnPosition, Quaternion.identity);
				}
				// Fallback to default Walker
				else
				{
					_enemyFactory.CreateEnemy(EnemyId.Walker, at: spawnPosition, Quaternion.identity);
				}

				_timer = 0;
			}
		}

		private void ResetBossSpawn() => _bossSpawnedRecently = false;


		private Vector2 RandomSpawnPosition(Vector2 heroWorldPosition)
		{
			bool startWithHorizontal = Random.Range(0, 2) == 0;

			return startWithHorizontal 
				? HorizontalSpawnPosition(heroWorldPosition) 
				: VerticalSpawnPosition(heroWorldPosition);
		}

		private Vector2 HorizontalSpawnPosition(Vector2 heroWorldPosition)
		{
			Vector2[] horizontalDirections = { Vector2.left, Vector2.right };
			Vector2 primaryDirection = horizontalDirections.PickRandom();
      
			float horizontalOffsetDistance = _cameraProvider.WorldScreenWidth / 2 + SpawnDistanceGap;
			float verticalRandomOffset = Random.Range(-_cameraProvider.WorldScreenHeight / 2, _cameraProvider.WorldScreenHeight / 2);

			return heroWorldPosition + primaryDirection * horizontalOffsetDistance + Vector2.up * verticalRandomOffset;
		}

		private Vector2 VerticalSpawnPosition(Vector2 heroWorldPosition)
		{
			Vector2[] verticalDirections = { Vector2.up, Vector2.down };
			Vector2 primaryDirection = verticalDirections.PickRandom();
      
			float verticalOffsetDistance = _cameraProvider.WorldScreenHeight / 2 + SpawnDistanceGap;
			float horizontalRandomOffset = Random.Range(-_cameraProvider.WorldScreenWidth / 2, _cameraProvider.WorldScreenWidth / 2);

			return heroWorldPosition + primaryDirection * verticalOffsetDistance + Vector2.right * horizontalRandomOffset;
		}
	}
}