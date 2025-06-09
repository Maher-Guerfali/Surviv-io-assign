using Code.Gameplay.Characters.Enemies.Services;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Characters.Enemies.Behaviours
{
	public class Enemy : MonoBehaviour
	{
		private IEnemyProvider _enemyProvider;

		[Inject]
		private void Construct(IEnemyProvider enemyProvider)
		{
			_enemyProvider = enemyProvider;
		}
		
		private void OnEnable()
		{
			_enemyProvider.RegisterEnemy(this);
		}
		
		private void OnDisable()
		{
			_enemyProvider.UnregisterEnemy(this);
		}
	}
}