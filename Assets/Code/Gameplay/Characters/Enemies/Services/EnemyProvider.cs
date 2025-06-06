using System;
using System.Collections.Generic;
using Code.Gameplay.Characters.Enemies.Behaviours;

namespace Code.Gameplay.Characters.Enemies.Services
{
	public class EnemyProvider : IEnemyProvider
	{
		private readonly List<Enemy> _enemies = new();
		
		public event Action<Enemy> OnEnemyRegistered;
		public event Action<Enemy> OnEnemyUnregistered;
		
		public void RegisterEnemy(Enemy enemy)
		{
			if (enemy == null)
				throw new ArgumentNullException(nameof(enemy));
			
			if (_enemies.Contains(enemy))
				throw new ArgumentException($"Enemy {enemy} is already registered.");
			
			_enemies.Add(enemy);
			
			OnEnemyRegistered?.Invoke(enemy);
		}
		
		public void UnregisterEnemy(Enemy enemy)
		{
			if (enemy == null)
				throw new ArgumentNullException(nameof(enemy));
			
			if (_enemies.Contains(enemy) == false)
				throw new ArgumentException($"Enemy {enemy} is not registered.");
			
			_enemies.Remove(enemy);
			
			OnEnemyUnregistered?.Invoke(enemy);
		}
	}
}