using UnityEngine;

namespace Code.Gameplay.Guns.Behaviours
{
	public class GunOwner : MonoBehaviour
	{
		[SerializeField] private Gun _ownedGun;

		public Gun OwnedGun => _ownedGun;
	}
}