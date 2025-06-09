using Code.Gameplay.PickUps.Behaviours;
using UnityEngine;

namespace Code.Gameplay.PickUps.Services
{
	public interface IPickUpFactory
	{
		PickUp Create(PickUpId id, Vector3 at);
	}
}