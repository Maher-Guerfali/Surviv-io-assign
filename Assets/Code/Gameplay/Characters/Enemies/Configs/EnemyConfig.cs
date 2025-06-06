using UnityEngine;

namespace Code.Gameplay.Characters.Enemies.Configs
{
	[CreateAssetMenu(fileName = "EnemyConfig", menuName = Constants.GameName + "/Configs/Enemy")]
	public class EnemyConfig : ScriptableObject
	{
		public EnemyId Id;
		
		public Behaviours.Enemy Prefab;
		
		public float Health = 100f;
		public float MovementSpeed = 5f;
		public float Damage = 10f;
	}
}