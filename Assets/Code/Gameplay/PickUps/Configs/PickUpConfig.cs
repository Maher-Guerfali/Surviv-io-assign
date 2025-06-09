using Code.Gameplay.PickUps.Behaviours;
using UnityEngine;

namespace Code.Gameplay.PickUps.Configs
{
	[CreateAssetMenu(fileName = "PickUpConfig", menuName = Constants.GameName + "/Configs/PickUp")]
	public class PickUpConfig : ScriptableObject
	{
		public PickUpId Id;

		public PickUp Prefab;
	}
}