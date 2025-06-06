using System;
using Code.Common.Extensions;
using UnityEngine;

namespace Code.Gameplay.PickUps.Behaviours
{
	public class PickUp : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;
		
		public event Action<GameObject> OnPickUp;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.Matches(_layerMask))
			{
				OnPickUp?.Invoke(other.gameObject);
			}
		}
	}
}