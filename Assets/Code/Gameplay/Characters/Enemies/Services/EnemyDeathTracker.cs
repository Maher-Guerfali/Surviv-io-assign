using System;
using Code.Gameplay.Characters.Enemies.Behaviours;
using Code.Gameplay.Lifetime.Behaviours;

namespace Code.Gameplay.Characters.Enemies.Services
{
	public class EnemyDeathTracker : IEnemyDeathTracker, IDisposable
	{
		private readonly IEnemyProvider _enemyProvider;
		
		public int TotalKilledEnemies { get; private set; }

		public EnemyDeathTracker(IEnemyProvider enemyProvider)
		{
			_enemyProvider = enemyProvider;
			
			_enemyProvider.OnEnemyRegistered += HandleEnemyRegistered;
			_enemyProvider.OnEnemyUnregistered += HandleEnemyUnregistered;
		}

		public void Dispose()
		{
			_enemyProvider.OnEnemyRegistered -= HandleEnemyRegistered;
			_enemyProvider.OnEnemyUnregistered -= HandleEnemyUnregistered;
		}

		private void HandleEnemyRegistered(Enemy enemy)
		{
			if (enemy.TryGetComponent(out Health health))
			{
				health.OnDeath += HandleEnemyDeath;
			}
		}

		private void HandleEnemyUnregistered(Enemy enemy)
		{
			if (enemy.TryGetComponent(out Health health))
			{
				health.OnDeath -= HandleEnemyDeath;
			}
		}

		private void HandleEnemyDeath()
		{
			TotalKilledEnemies++;
		}
	}
}