using UnityEngine;

namespace Code.Gameplay.Characters.Enemies.Services
{
	public interface IEnemyFactory
	{
		Behaviours.Enemy CreateEnemy(EnemyId id, Vector3 at, Quaternion rotation);
	}
}