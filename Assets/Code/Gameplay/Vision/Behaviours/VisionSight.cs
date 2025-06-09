using System.Collections.Generic;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Vision.Behaviours
{
	[RequireComponent(typeof(Team))]
	[RequireComponent(typeof(Stats))]
	public class VisionSight : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;
		
		private Team _team;
		private Stats _stats;
		private Collider2D[] _hits;

		private List<GameObject> Enemies { get; } = new();

		private void Awake()
		{
			_team = GetComponent<Team>();
			_stats = GetComponent<Stats>();
			_hits = new Collider2D[50];
		}

		private void FixedUpdate()
		{
			GatherEnemiesInSight();
		}
    
		public GameObject GetClosestEnemy()
		{
			GameObject closest = null;
			float minDistance = float.MaxValue;
      
			foreach (GameObject enemy in Enemies)
			{
				if (enemy == null)
					continue;

				float distance = Vector3.Distance(transform.position, enemy.transform.position);

				if (distance < minDistance)
				{
					minDistance = distance;
					closest = enemy;
				}
			}

			return closest;
		}

		private void GatherEnemiesInSight()
		{
			float visionRange = _stats.GetStat(StatType.VisionRange);
      
			Enemies.Clear();
			int size = Physics2D.OverlapCircleNonAlloc(transform.position, visionRange, _hits, _layerMask);

			for (var i = 0; i < size; i++)
			{
				Collider2D hit = _hits[i];
        
				if (hit.TryGetComponent(out Team team) && team != _team)
				{
					Enemies.Add(hit.gameObject);
				}
			}
		}
	}
}