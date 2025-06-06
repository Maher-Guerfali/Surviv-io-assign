using System;
using Code.Gameplay.Characters.Enemies.Behaviours;

namespace Code.Gameplay.Characters.Enemies.Services
{
	public interface IEnemyProvider
	{
		void RegisterEnemy(Enemy enemy);
		void UnregisterEnemy(Enemy enemy);
		event Action<Enemy> OnEnemyRegistered;
		event Action<Enemy> OnEnemyUnregistered;
	}
}